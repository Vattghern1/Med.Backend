using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.DataTransferObjects;
using Med.Common.Enums;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Med.Backend.BL.Services;

public class InspectionService : IInspectionService
{
    private readonly ILogger<InspectionService> _logger;
    private readonly BackendDbContext _backendDbContext;

    public InspectionService(ILogger<InspectionService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

    public async Task<InspectionModel> GetInfoAboutInspection(Guid id)
    {
        var inspection = await _backendDbContext.Inspections
            .Include(i => i.Diagnoses)
            .Include(i => i.Consultations)
            .ThenInclude(c => c.Comments)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inspection == null)
        {
            throw new NotFoundException("Inspection with this id not found");
        }

        var patient = await _backendDbContext.Patients
            .FirstOrDefaultAsync(p => p.Id == inspection.PatientId);

        if (patient == null)
        {
            throw new ConflictException("Patient not found.");
        }

        var doctor = await _backendDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == inspection.DoctorId);

        if (doctor == null)
        {
            throw new ConflictException("Doctor not found.");
        }

        var inspectionModel = new InspectionModel
        {
            InspectionId = inspection.Id,
            CreateTime = inspection.CreateDate,
            Date = inspection.InspectionDate,
            Anamnesis = inspection.Anamnesis,
            Complaints = inspection.Complaints,
            Treatment = inspection.Treatment,
            Conclusion = inspection.Conclusion,
            NextVisitDate = inspection.NextVisitDate,
            DeathDate = inspection.DeathDate,
            BaseInspectionId = null,
            PreviousInspectionId = inspection.PreviousInspectionId,
            Patient = new PatientModel
            {
                Id = patient.Id,
                CreateTime = patient.CreateDate,
                Name = patient.Name,
                Birthday = patient.BirthDate,
                Gender = patient.Gender,
            },
            Doctor = new DoctorModel
            {
                Id = doctor.Id,
                CreateTime = doctor.CreateTime,
                Name = doctor.FullName,
                Birthday = doctor.BirthDate,
                Gender = doctor.Gender,
                Email = doctor.Email,
                Phone = doctor.PhoneNumber
            },
            Diagnoses = inspection.Diagnoses.Select(diagnos => new DiagnosisModel
            {
                Id = diagnos.Id,
                CreateTime = diagnos.CreateTime,
                Code = diagnos.IcdDiagnosisId,
                Name = diagnos.DiagnosName,
                Description = diagnos.Description,
                Type = diagnos.DiagnosisType
            }).ToList(),
            Consultations = inspection.Consultations.Select(consultation => new InspectionConsultationModel
            {
                ConsultationId = consultation.Id,
                InspectionId = consultation.InspectionId,
                CreateTime = consultation.CreateTime,
                Speciality = _backendDbContext.Specialities.Where(s => s.Id == consultation.SpecialityId).Select(speciality => new SpecialityModel
                {
                    Id = speciality.Id,
                    CreateTime = speciality.CreateTime,
                    Name = speciality.Name,
                }).First(),
                RootComment = consultation.Comments.OrderBy(c => c.CreateTime).Select(comment => new InspectionCommentModel
                {
                    Id = comment.Id,
                    CreateTime = comment.CreateTime,
                    ParentId = comment.ParentCommentId,
                    Content = comment.Content,
                    ModifyTime = comment.ModifiedTime,
                    Author = _backendDbContext.Users.Where(u => u.Id == comment.AuthorId).Select(doctor => new DoctorModel
                    {
                        Id = doctor.Id,
                        CreateTime = doctor.CreateTime,
                        Name = doctor.FullName,
                        Birthday = doctor.BirthDate,
                        Gender = doctor.Gender,
                        Email = doctor.Email,
                        Phone = doctor.PhoneNumber
                    }).First()
                }).First(),
                CommentsNumber = consultation.Comments.Count,
            }).ToList(),
        };

        return inspectionModel;
    }

    public async Task EditConcreteInspection(InspectionEditModel inspectionEditModel, Guid id, Guid doctorId)
    {
        var inspection = await _backendDbContext.Inspections
            .Include(i => i.Diagnoses)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inspection == null)
        {
            throw new NotFoundException("Inspection with this id not found");
        }

        if (inspection.DoctorId != doctorId)
        {
            throw new ForbiddenException("You can't edit this inspection.");
        }

        if (inspectionEditModel.Diagnoses.Where(d => d.Type == DiagnosisType.Main).Count() != 1)
        {
            throw new BadRequestException("Wrong count of diagnosis with type 'Main'.");
        }

        if (inspectionEditModel.Conclusion == Conclusion.Recovery)
        {
            if (inspectionEditModel.NextVisitDate != null)
            {
                throw new BadRequestException("If conslusion - Recovery, next visit must be empty.");
            }
            if (inspectionEditModel.DeathDate != null)
            {
                throw new BadRequestException("If conslusion - Recovery, death date must be empty.");
            }
        }

        if (inspectionEditModel.Conclusion == Conclusion.Disease)
        {
            if (inspectionEditModel.NextVisitDate == null)
            {
                throw new BadRequestException("If conslusion - Disease, must be set next visit date.");
            }
            if (inspectionEditModel.NextVisitDate < inspection.CreateDate)
            {
                throw new BadRequestException("Inspection's date must be less then next visit date.");
            }
            if (inspectionEditModel.DeathDate != null)
            {
                throw new BadRequestException("If conslusion - Disease, death date must be empty.");
            }
        }

        if (inspectionEditModel.Conclusion == Conclusion.Death)
        {
            if (inspectionEditModel.DeathDate == null)
            {
                throw new BadRequestException("If conslusion - Death, must be set death date.");
            }
            if (inspectionEditModel.DeathDate < inspection.CreateDate)
            {
                throw new BadRequestException("Inspection's date must be less then death date.");
            }
            if (inspectionEditModel.NextVisitDate != null)
            {
                throw new BadRequestException("If conslusion - Death, next visit date must be empty.");
            }

            var allInspections = _backendDbContext.Inspections
                .Where(i => i.PatientId == id)
                .AsEnumerable();

            if (allInspections.Any(i => i.CreateDate > inspection.DeathDate))
            {
                throw new BadRequestException("Wrong death date.");
            }
        }

        foreach (var diagnos in inspectionEditModel.Diagnoses)
        {
            var diagnoses = _backendDbContext.Diagnos
                .Where(d => d.IcdDiagnosisId == diagnos.IcdDiagnosisId)
                .AsEnumerable();

            if (diagnoses == null)
            {
                throw new NotFoundException($"Diagnos with that {diagnos.IcdDiagnosisId} not found.");
            }
        }

        inspection.Anamnesis = inspectionEditModel.Anamnesis;
        inspection.Complaints = inspectionEditModel.Complaints;
        inspection.Treatment = inspectionEditModel.Treatment;
        inspection.Conclusion = inspectionEditModel.Conclusion;
        if (inspectionEditModel.NextVisitDate < DateTime.UtcNow)
        {
            throw new BadRequestException("Next visit date must be more then now.");
        }

        inspection.NextVisitDate = inspectionEditModel.NextVisitDate;
        inspection.DeathDate = inspectionEditModel.DeathDate;

        inspection.Diagnoses = new List<Diagnos>();
        foreach (var diagnos in inspectionEditModel.Diagnoses)
        {
            var newDiagnos = new Diagnos
            {
                IcdDiagnosisId = diagnos.IcdDiagnosisId,
                DiagnosName = _backendDbContext.Diagnos
                .FirstOrDefault(d => d.IcdDiagnosisId == diagnos.IcdDiagnosisId).DiagnosName 
                ?? throw new NotFoundException("Diagnoses not found."),
                Description = diagnos.Description,
                DiagnosisType = diagnos.Type,
                Inspection = inspection
            };
            inspection.Diagnoses.Add(newDiagnos);
        }

        _backendDbContext.Inspections.Update(inspection);
        _backendDbContext.Diagnos.AddRange(inspection.Diagnoses);
        await _backendDbContext.SaveChangesAsync();
    }

    public async Task<List<InspectionPreviewModel>> GetInspectionChain(Guid id)
    {

        throw new NotImplementedException();
/*        var inspection = await _backendDbContext.Inspections
            .Include(i => i.Diagnoses)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inspection == null)
        {
            throw new NotFoundException("Inspection not found.");
        }

        var inspections = _backendDbContext.Inspections
            .ToList();

        List<Inspection> inspectionChain = GetAllInspectionsRecursively(id, inspections);

        List<InspectionPreviewModel> inspectionChainModels = new List<InspectionPreviewModel>();
        foreach (var inspcetion in inspectionChain)
        {
            inspection.Select
        }*/
    }

    public List<Inspection> GetAllInspectionsRecursively(Guid? parentId, List<Inspection> nodeList)
    {
        var childInspections = nodeList.Where(n => n.PreviousInspectionId == parentId);

        List<Inspection> result = new List<Inspection>();
        result.AddRange(childInspections);

        foreach (var childNode in childInspections)
        {
            result.AddRange(GetAllInspectionsRecursively(childNode.Id, nodeList));
        }

        return result;
    }
}