using Med.Backend.DAL.Data;
using Med.Backend.DAL.Data.Entities;
using Med.Common.DataTransferObjects;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Med.Backend.BL.Services;

public class DictionaryService : IDictionaryService
{
    private readonly ILogger<DictionaryService> _logger;
    private readonly BackendDbContext _backendDbContext;


    public DictionaryService(ILogger<DictionaryService> logger, BackendDbContext backendDbContext)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
    }

    public async Task<PagedList<SpecialityModel>> GetSpecialityList(PaginationParamsDto pagination)
    {
        var specialities = _backendDbContext.Specialities
            .Where(u => pagination.Name == null || u.Name.ToLower().Contains(pagination.Name.ToLower()))
            .AsNoTracking();

        var shortSpecialities = specialities.Select(speciality => new SpecialityModel
        {
            Id = speciality.Id,
            Name = speciality.Name,
            CreateTime = speciality.CreateTime
        });

        var response = await PagedList<SpecialityModel>.ToPagedList(shortSpecialities, pagination.Page, pagination.Size);

        return response;
    }

    public async Task<PagedList<Icd10RecordModel>> SearchDiagnosesInDictionaty(string? request, int page, int size)
    {
        var diagnoses = _backendDbContext.Icd10s
            .Where(u => request == null || u.Name.ToLower().Contains(request.ToLower()) || u.Code.ToLower().Contains(request.ToLower()))
            .AsNoTracking();

        var shortDiagnoses = diagnoses.Select(diagnoses => new Icd10RecordModel
        {
            Id = diagnoses.Id,
            CreateTime = diagnoses.CreateTime,
            Code = diagnoses.Code,
            Name = diagnoses.Name,
        });

        var response = await PagedList<Icd10RecordModel>.ToPagedList(shortDiagnoses, page, size);

        return response;
    }

    public async Task<List<Icd10RecordModel>> GetRootICD10Elemements()
    {
        var diagnoses = _backendDbContext.Icd10s
            .Where(u => u.Code.Contains("-"))
            .AsNoTracking();

        List<Icd10RecordModel> shortDiagnoses = new List<Icd10RecordModel>(diagnoses.Select(diagnoses => new Icd10RecordModel
        {
            Id = diagnoses.Id,
            CreateTime = diagnoses.CreateTime,
            Code = diagnoses.Code,
            Name = diagnoses.Name,
        }));

        return shortDiagnoses;
    }
}