using Med.Backend.DAL.Migrations;
using Med.Common.DataTransferObjects;
using Med.Common.Exceptions;
using Med.Common.Interfaces;
using Med.Common.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Med.Backend.API.Controllers;

/// <summary>
/// Controller for consultation management
/// </summary>
[ApiController]
[Route("api/consultation")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ConsultationController : ControllerBase
{


    private readonly ILogger<ConsultationController> _logger;
    private readonly IConsultationService _consultationService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="consultationService"></param>
    public ConsultationController(ILogger<ConsultationController> logger, IConsultationService consultationService)
    {
        _logger = logger;
        _consultationService = consultationService;
    }

    /// <summary>
    /// Get a list of medical inspections for consultation
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedList<InspectionPreviewModel>>> GetConsultationList([FromQuery] bool grouped,
        [FromQuery] List<string> icdRoots,
        [FromQuery] int page,
        [FromQuery] int size)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
             throw new UnauthorizedException("User is not authorized");
        }
        return Ok(await _consultationService.GetConsultationList(grouped, icdRoots, page, size));
    }

    /// <summary>
    /// Get concrete consultation
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ConsultationModel>> GetConsultation(Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _consultationService.GetConsultation(id));
    }

    /// <summary>
    /// Add comment to concrete consultation
    /// </summary>
    [HttpPost]
    [Route("{id}/comment")]
    public async Task<ActionResult<Guid>> AddCommentToConsultation([FromBody] CommentCreateModel commentCreateModel,Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }

        return Ok(await _consultationService.AddCommentToConsultation(commentCreateModel, id, userId));
    }

    /// <summary>
    /// Edit comment
    /// </summary>
    [HttpPut]
    [Route("comment/{id}")]
    public async Task<ActionResult> EditConsultationComment([FromBody] InspectionCommentCreateModel inspectionCommentCreateModel, Guid id)
    {
        if (User.Identity == null || Guid.TryParse(User.Identity.Name, out Guid userId) == false)
        {
            throw new UnauthorizedException("User is not authorized");
        }
        await _consultationService.EditConsultationComment(inspectionCommentCreateModel, id, userId);
        return Ok();
    }
}