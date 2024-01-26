using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for report management
/// </summary>
[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="reportService"></param>
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Get a report on patients' visits based on ICD-10 roots for a specified time interval
    /// </summary>
    [HttpGet]
    [Route("icdrootsreport")]
    public async Task<ActionResult<IcdRootsReportModel>> GetIcdReport([FromQuery] DateTime start,
        [FromQuery] DateTime finish,
        [FromQuery] List<string>? icdRoots)
    {
        return Ok(await _reportService.GetIcdReport(start, finish, icdRoots));
    }

    /// <summary>
    /// Generate report in excel
    /// </summary>
    [HttpGet]
    [Route("icdrootsreportinexcel")]
    public async Task<ActionResult<List<FileKeyDto>>> GenerateIcdReportExcel([FromQuery] DateTime start,
    [FromQuery] DateTime finish,
    [FromQuery] List<string>? icdRoots)
    {
        
        return Ok(await _reportService.GenerateIcdReportExcel(start, finish, icdRoots));
    }
}