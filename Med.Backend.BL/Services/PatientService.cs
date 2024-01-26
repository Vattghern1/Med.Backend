using Med.Backend.BL.Extensions;
using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.DataTransferObjects;
using Med.Common.Enums;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Med.Backend.BL.Services;

public class PatientService : IPatientService
{
    private readonly ILogger<PatientService> _logger;
    private readonly BackendDbContext _backendDbContext;

    public PatientService(ILogger<PatientService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

    public async Task<Guid> CreateNewPatient(PatientCreateModel patientCreateModel)
    {
        if (patientCreateModel.Birthday >= DateTime.UtcNow)
        {
            throw new BadRequestException("Birthday more then now.");
        }

        var newPatient = new Patient
        {
            Name = patientCreateModel.Name,
            BirthDate = patientCreateModel.Birthday,
            Gender = patientCreateModel.Gender,
        };
        await _backendDbContext.AddAsync(newPatient);
        await _backendDbContext.SaveChangesAsync();

        return newPatient.Id;
    }

    public async Task<PatientModel> GetPatientCard(Guid id)
    {
        var patient = await _backendDbContext.Patients
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            throw new NotFoundException("Patient with this id not found");
        }

        var response = new PatientModel
        {
            Id = id,
            CreateTime = patient.CreateDate,
            Name = patient.Name,
            Birthday = patient.BirthDate,
            Gender = patient.Gender
        };

        return response;
    }

    public async Task<PagedList<PatientModel>> GetPatientList(string? name, List<Conclusion>? conclusions,
        Sorting? sorting, bool? scheduledVisits, bool? onlyMine, int page, int size, Guid userId)
    {
        

        var patients = _backendDbContext.Patients
            .Include(p => p.Inspections)!
            .Where(u => name == null || u.Name.ToLower().Contains(name.ToLower()))
            .Where(p => conclusions!.Count == 0 ||
            (conclusions.Contains(Conclusion.Recovery) && p.Inspections!.Count > 0 && p.Inspections!.Any(i => i.Conclusion == Conclusion.Recovery)) ||
            (conclusions.Contains(Conclusion.Death) && p.Inspections!.Count > 0 && p.Inspections!.Any(i => i.Conclusion == Conclusion.Death)) ||
            (conclusions.Contains(Conclusion.Disease) && p.Inspections!.Count > 0 && p.Inspections!.Any(i => i.Conclusion == Conclusion.Disease)))
            .Where(p => onlyMine == null || 
            (onlyMine == true && p.Inspections!.Count > 0 && p.Inspections!.Any(i => i.DoctorId ==  userId)) ||
            (onlyMine == false && p.Inspections!.Any(i => i.DoctorId != userId)))
            .Where(p => scheduledVisits == null || 
            (scheduledVisits == true && p.Inspections!.Count > 0 && p.Inspections!.Any(i => i.NextVisitDate > DateTime.UtcNow)) ||
            (scheduledVisits == false && p.Inspections!.Any(i => i.NextVisitDate < DateTime.UtcNow)))
            .PatientOrderBy(sorting)
            .AsNoTracking();

        var patientsModels = patients.Select(patient => new PatientModel
        {
            Id = patient.Id,
            Name = patient.Name,
            CreateTime = patient.CreateDate,
            Birthday = patient.BirthDate,
            Gender = patient.Gender,
        });

        var response = await PagedList<PatientModel>.ToPagedList(patientsModels, page, size);
        return response;
    }

    public async Task<Guid> CreateInspectionForPatient(InspectionCreateModel inspectionCreateModel, Guid patientId, Guid doctorId)
    {
        var patient = await _backendDbContext.Patients
            .Include(p => p.Inspections)!
            .FirstOrDefaultAsync(p => p.Id == patientId); 

        if (patient == null)
        {
            throw new NotFoundException("Patient with this id not found");
        }

        var doctor = await _backendDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == doctorId);

        if (doctor == null)
        {
            throw new NotFoundException("Doctor with this id not found");
        }

        if (inspectionCreateModel.Date > DateTime.UtcNow)
        {
            throw new BadRequestException("Incorrect date.");
        }
        
        if (inspectionCreateModel.PreviousInspectionId != null)
        {
            var lastInspection = await _backendDbContext.Inspections
                .FirstOrDefaultAsync(i => i.Id == inspectionCreateModel.PreviousInspectionId);

            if (lastInspection == null)
            {
                throw new NotFoundException("Base inspection not found");
            }

            if (lastInspection!.InspectionDate > inspectionCreateModel.Date)
            {
                throw new BadRequestException("Inspection date cann't be more then date of previous inspection");
            }
        }

        if (inspectionCreateModel.DeathDate == null && inspectionCreateModel.Conclusion == Conclusion.Death)
        {
            throw new BadRequestException("If conslusion - death, must be set death time.");
        }

        if (inspectionCreateModel.NextVisitDate == null && inspectionCreateModel.Conclusion == Conclusion.Disease)
        {
            throw new BadRequestException("If conslusion - Disease, must be set next visit date.");
        }

        if (inspectionCreateModel.DeathDate != null && inspectionCreateModel.Conclusion == Conclusion.Recovery)
        {
            throw new BadRequestException("If conslusion - Recovery, death date must be null.");
        }

        if (inspectionCreateModel.DeathDate != null && inspectionCreateModel.DeathDate > DateTime.UtcNow)
        {
            throw new BadRequestException("Wrong date for deathtime.");
        }

        if (inspectionCreateModel.NextVisitDate != null && inspectionCreateModel.Conclusion == Conclusion.Recovery)
        {
            throw new BadRequestException("If conslusion - Recovery, next visit must be null.");
        }

        if (inspectionCreateModel.Conclusion == Conclusion.Death && patient.Inspections!.Any(i => i.Conclusion == Conclusion.Death) == true)
        {
            throw new BadRequestException("Patient already have inspection with conclusion - Death.");
        }

        if (patient.Inspections!.Any(i => i.Conclusion == Conclusion.Recovery && i.Id == inspectionCreateModel.PreviousInspectionId) == true)
        { 
            throw new BadRequestException("You cant create inspection with previous inspection conclusion - recovery.");
        }

        if (inspectionCreateModel.Diagnoses.Where(d => d.Type == DiagnosisType.Main).Count() != 1)
        {
            throw new BadRequestException("Wrong count of diagnosis with type 'Main'.");
        }

        foreach (var consultation in inspectionCreateModel.Consultations)
        {
            var speciality = await _backendDbContext.Specialities
                .FirstOrDefaultAsync(c => c.Id == consultation.SpecialityId);            
            
            if (speciality == null)
            {
                throw new NotFoundException($"Speciality {consultation.SpecialityId} not found.");
            }


            if (inspectionCreateModel.Consultations.Where(c => c.SpecialityId == consultation.SpecialityId).Count() > 1)
            {
                throw new BadRequestException("One inspection cant have more then one consulation with same speciality.");
            }
        }


        var newInspection = new Inspection
        {
            InspectionDate = inspectionCreateModel.Date,
            Anamnesis = inspectionCreateModel.Anamnesis,
            Complaints = inspectionCreateModel.Complaints,
            Treatment = inspectionCreateModel.Treatment,
            Conclusion = inspectionCreateModel.Conclusion,
            NextVisitDate = inspectionCreateModel.NextVisitDate,
            DeathDate = inspectionCreateModel.DeathDate,
            PreviousInspectionId = inspectionCreateModel.PreviousInspectionId,
            DoctorId = doctorId,
            DoctorName = doctor.FullName,
            PatientId = patientId,
            PatientName = patient.Name,
        };

        foreach (var diagnos in inspectionCreateModel.Diagnoses)
        {
            var diagnosIcd10 = await _backendDbContext.Icd10s
                .FirstOrDefaultAsync(d => d.Code == diagnos.IcdDiagnosisId);

            if (diagnosIcd10 == null)
            {
                throw new NotFoundException($"Diagnos {diagnos.IcdDiagnosisId} not found found.");
            }

            var newDiagnos = new Diagnos
            {
                IcdDiagnosisId = diagnos.IcdDiagnosisId,
                DiagnosName = diagnosIcd10.Name,
                Description = diagnos.Description,
                DiagnosisType = diagnos.Type,
                Inspection = newInspection
            };

            newInspection.Diagnoses.Add(newDiagnos);
        }

        foreach (var consultation in inspectionCreateModel.Consultations)
        {
            var newConsultation = new Consultation
            {
                SpecialityId = consultation.SpecialityId,
                InspectionId = newInspection.Id
            };

            var newComment = new Comment
            {
                Content = consultation.Comment.Content,
                AuthorId = doctorId,
                AuthorName = doctor.FullName,
            };

            newConsultation.Comments.Add(newComment);

            newInspection.Consultations.Add(newConsultation);
        }

        patient.Inspections.Add(newInspection);
        await _backendDbContext.AddAsync(newInspection);
        await _backendDbContext.SaveChangesAsync();

        return newInspection.Id;
    }

    public async Task<List<InspectionShortModel>> SearchForPatientInspections(string? request, Guid patientId)
    {
        var patient = await _backendDbContext.Patients
            .Include(p => p.Inspections)!
            .ThenInclude(i => i.Diagnoses)!
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
        {
            throw new NotFoundException("Patient with this id not found");
        }

        var inspectionsModels = patient.Inspections!
            .Where(i => request == null || i.Diagnoses.Any(d => 
            (d.DiagnosName.ToLower().Contains(request.ToLower())) || 
            (d.IcdDiagnosisId.ToLower().Contains(request.ToLower()))))
            .Where(i => !patient.Inspections.Any(s => s.PreviousInspectionId == i.Id))
            .Select(inspection => new InspectionShortModel
            {
                Id = inspection.Id,
                Date = inspection.InspectionDate,
                CreateTime = inspection.CreateDate,
                DiagnosisModel = inspection.Diagnoses.Select(diagnos => new DiagnosisModel
                {
                    Id = diagnos.Id,
                    CreateTime = diagnos.CreateTime,
                    Code = diagnos.IcdDiagnosisId,
                    Name = diagnos.DiagnosName,
                    Description = diagnos.Description,
                    Type = diagnos.DiagnosisType
                }).ToList(),
            }).ToList();

        if (inspectionsModels.Count == 0)
        {
            throw new NotFoundException("Requested patient's inspections not found.");
        }

        return inspectionsModels;
    }

    public async Task<PagedList<InspectionPreviewModel>> GetListOfPatientInspections(Guid patientId, bool? grouped, List<string>? icdRoots, int page, int size)
    {
        var patient = await _backendDbContext.Patients
            .Include(p => p.Inspections)!
            .FirstOrDefaultAsync(p => p.Id == patientId);

        string icdRootsRegex = "";
        int counter = 0;
        foreach (var icdRoot in icdRoots!)
        {
            icdRootsRegex += $"[{icdRoot[0]}-{icdRoot[4]}]";
            int start = Int32.Parse(Convert.ToString(icdRoot[1]) + Convert.ToString(icdRoot[2])); 
            int finish = Int32.Parse(Convert.ToString(icdRoot[5]) + Convert.ToString(icdRoot[6]));    
            if (start < 10)
            {
                icdRootsRegex += $"(0{start}";
            }
            else
            {
                icdRootsRegex += $"({start}";
            }
            while (start < finish)  
            {  
                if (start + 1 < 10)
                {
                    icdRootsRegex += $"|0{start + 1}";
                }
                else
                {
                    icdRootsRegex += $"|{start + 1}";
                }
                start++;
            }
            icdRootsRegex += $")";
            
            counter++;
            if (counter != icdRoots.Count())
            {
                icdRootsRegex += "|";
            }
        }

        if (patient == null)
        {
            throw new NotFoundException("Patient with this id not found");
        }

        var inspections = _backendDbContext.Inspections
            .Include(i => i.Diagnoses)
            .Where(i => i.PatientId == patientId)
            .AsQueryable()
            .AsNoTracking();

        var inspectionModel = inspections
            .Where(i => i.Diagnoses.Where(d => Regex.IsMatch(d.IcdDiagnosisId, icdRootsRegex)).Count() > 0)
            .Where(i => (grouped == null) ||
            (grouped == true && (i.PreviousInspectionId.HasValue || inspections!.Any(n => n.PreviousInspectionId == i.Id) == true)) ||
            (grouped == false && (i.PreviousInspectionId == null && inspections!.Any(n => n.PreviousInspectionId == i.Id) == false)))
            .Select(inspection => new InspectionPreviewModel
        {
            Id = inspection.Id,
            CreateTime = inspection.CreateDate,
            PreviousId = inspection.PreviousInspectionId,
            Conclusion = inspection.Conclusion,
            DoctorId = inspection.DoctorId,
            Doctor = inspection.DoctorName,
            PatientId = inspection.PatientId,
            Patient = inspection.PatientName,
            Diagnosis = inspection.Diagnoses.Select(diagnosis => new DiagnosisModel
            {
                Id = diagnosis.Id,
                CreateTime = diagnosis.CreateTime,
                Code = diagnosis.IcdDiagnosisId,
                Name = diagnosis.DiagnosName,
                Description = diagnosis.Description,
                Type = diagnosis.DiagnosisType
            }).First(),
            HasChain = inspections!.Any(i => i.PreviousInspectionId.HasValue),
            HasNested = inspections!.Any(i => i.PreviousInspectionId == inspection.Id)
        });

        if (inspectionModel.Count() == 0)
        {
            throw new BadRequestException("Current patient have zero inspections");
        }

        var response = await PagedList<InspectionPreviewModel>.ToPagedList(inspectionModel, page, size);
        return response;
    }
}