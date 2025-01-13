using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Med.Backend.BL.Services;

public class ConsultationService : IConsultationService
{
    private readonly ILogger<ConsultationService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public ConsultationService(ILogger<ConsultationService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }


    //Получене консультаций
    public async Task<PagedList<InspectionPreviewModel>> GetConsultationList(bool? grouped, List<string> icdRoots, int page, int size)
    {
        string icdRootsRegex = "";
        int counter = 0;
        foreach (var icdRoot in icdRoots!)
        {
            if (icdRoot == null)
            {
                throw new BadRequestException("Wrong filter data.");
            }
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

        var inspections = _backendDbContext.Inspections
            .Include(i => i.Diagnoses)
            .Where(i => i.Diagnoses.Where(d => Regex.IsMatch(d.IcdDiagnosisId, icdRootsRegex)).Count() > 0)
            .AsQueryable()
            .AsNoTracking();

        var inspectionsModels = inspections
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
                PatientId = inspection.DoctorId,
                Patient = inspection.PatientName,
                Diagnosis = inspection.Diagnoses
                .Select(diagnos => new DiagnosisModel
                {
                    Id = diagnos.Id,
                    CreateTime = diagnos.CreateTime,
                    Code = diagnos.IcdDiagnosisId,
                    Name = diagnos.DiagnosName,
                    Description = diagnos.Description,
                    Type = diagnos.DiagnosisType,
                }).First(),
                HasChain = inspections!.Any(i => i.PreviousInspectionId.HasValue),
                HasNested = inspections!.Any(i => i.PreviousInspectionId == inspection.Id),
            });

        var response = await PagedList<InspectionPreviewModel>.ToPagedList(inspectionsModels, page, size);
        return response;
    }

    //Получене консультаций
    public async Task<ConsultationModel> GetConsultation(Guid id)
    {
        var c = await _backendDbContext.Consultations
            .Include(c => c.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (c == null)
        {
            throw new NotFoundException("Consultation not found.");
        }

        var s = await _backendDbContext.Specialities
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == c.SpecialityId);

        if (s == null)
        {
            throw new ConflictException("Speciality not found.");
        }

        return new ConsultationModel
        {
            Id = c.Id,
            CreateTime = c.CreateTime,
            InspectionId = c.InspectionId,
            Speciality = new SpecialityModel
            {
                Id = s.Id,
                CreateTime = s.CreateTime,
                Name = s.Name,
            },
            Comments = c.Comments.Select(co => new CommentModel
            {
                Id = co.Id,
                CreateTime = co.CreateTime,
                ModifiedDate = co.ModifiedTime,
                Content = co.Content,
                AuthorId = co.AuthorId,
                AuthorName = co.AuthorName,
                ParentId = co.ParentCommentId
            }).ToList(),
        };
    }

    //Добавление комментария
    public async Task<Guid> AddCommentToConsultation(CommentCreateModel commentCreateModel, Guid consultationId, Guid userId)
    {

        //Поиск констультации
        var с = await _backendDbContext.Consultations
            .Include(c => c.Comments)
            .FirstOrDefaultAsync(c => c.Id == consultationId);

        if (с == null)
        {
            throw new NotFoundException("Consultation not found.");
        }

        //Поиск пользователя
        var user = await _backendDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ConflictException("User not found.");
        }


        //Поиск специальности
        var specialityOfConsoltation = await _backendDbContext.Specialities
            .FirstOrDefaultAsync(s => s.Id == с.SpecialityId);

        var i = await _backendDbContext.Inspections
            .Include(i => i.Consultations)
            .FirstOrDefaultAsync(i => i.Consultations.Any(c => c.Id == с.Id));


        //Учет прав
        if (i == null || i.DoctorId != userId || !user.UserSpecialities.Contains(specialityOfConsoltation!.Id))
        {
            throw new ForbiddenException("You dont have permissions to write comment.");
        }

        //Просмотр род ком
        if (commentCreateModel.ParentId != null)
        {
            var parentComment = await _backendDbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == commentCreateModel.ParentId);

            if (parentComment == null)
            {
                throw new NotFoundException("Parent comment not found.");
            }

            if (!с.Comments.Contains(parentComment))
            {
                throw new ConflictException("Parent comment don't relate to consultation.");
            }
        }


        //Создание нового
        var com = new Comment
        {
            Content = commentCreateModel.Content,
            AuthorId = userId,
            AuthorName = user.UserName,
            ParentCommentId = commentCreateModel.ParentId
        };


        //Добавленеи в бд
        с.Comments.Add(com);
        await _backendDbContext.AddAsync(com);
        await _backendDbContext.SaveChangesAsync();

        return com.Id;
    }

    //Редактирование комментария консльтации
    public async Task EditConsultationComment(InspectionCommentCreateModel inspectionCommentCreateModel, Guid commentId, Guid userId)
    {
        var com = await _backendDbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);

        //Проверки
        if (com == null)
        {
            throw new NotFoundException("Comment not found.");
        }

        if (inspectionCommentCreateModel.Content == null || inspectionCommentCreateModel.Content == "")
        {
            throw new BadRequestException("Comment content can't be empty.");
        }

        if (com.AuthorId != userId)
        {
            throw new ForbiddenException("You dont have permissions to edit that comment.");
        }

        com.Content = inspectionCommentCreateModel.Content;
        com.ModifiedTime = DateTime.UtcNow;

        _backendDbContext.Comments.Update(com);
        await _backendDbContext.SaveChangesAsync();
    }
}
