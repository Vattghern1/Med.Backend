using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for dictionary management
/// </summary>
[ApiController]
[Route("api/dictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService _dictionaryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dictionaryService"></param>
    public DictionaryController(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    /// <summary>
    /// Get speciality list
    /// </summary>
    [HttpGet]
    [Route("speciality")]
    public async Task<ActionResult<PagedList<SpecialityModel>>> GetSpecialityList([FromQuery] PaginationParamsDto pagination)
    {
        return Ok(await _dictionaryService.GetSpecialityList(pagination));
    }

    /// <summary>
    /// Search for diagnoses in ICD-10 dictionary
    /// </summary>
    [HttpGet]
    [Route("icd10")]
    public async Task<ActionResult<PagedList<Icd10RecordModel>>> SearchDiagnosesInDictionaty([FromQuery] string? request,
        [FromQuery] int page,
        [FromQuery] int size)
    {
        return Ok(await _dictionaryService.SearchDiagnosesInDictionaty(request, page, size));
    }

    /// <summary>
    /// Get root ICD-10 elements
    /// </summary>
    [HttpGet]
    [Route("icd/roots")]
    public async Task<ActionResult<List<Icd10RecordModel>>> GetRootICD10Elemements()
    {
       return Ok(await _dictionaryService.GetRootICD10Elemements());
    }
}