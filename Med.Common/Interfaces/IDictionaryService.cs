using Med.Common.DataTransferObjects;
using Med.Common.Other;
using Microsoft.AspNetCore.Mvc;

namespace Med.Common.Interfaces;

public interface IDictionaryService
{
    public Task<PagedList<SpecialityModel>> GetSpecialityList(PaginationParamsDto pagination);
    public Task<PagedList<Icd10RecordModel>> SearchDiagnosesInDictionaty(string? request, int page, int size);
    public Task<List<Icd10RecordModel>> GetRootICD10Elemements();

}