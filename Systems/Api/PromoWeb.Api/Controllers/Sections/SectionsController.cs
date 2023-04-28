﻿namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Sections;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Sections;
using Microsoft.AspNetCore.Authorization;
using PromoWeb.Common.Security;


/// <summary>
/// Sections controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/sections")]
[ApiController]
[ApiVersion("1.0")]
public class SectionsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<SectionsController> logger;
    private readonly ISectionService sectionService;

    public SectionsController(IMapper mapper, ILogger<SectionsController> logger, ISectionService sectionService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.sectionService = sectionService;
    }


    /// <summary>
    /// Get sections
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of SectionResponses</response>
    [ProducesResponseType(typeof(IEnumerable<SectionResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<SectionResponse>> GetSections([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var sections = await sectionService.GetSections(offset, limit);
        var response = mapper.Map<IEnumerable<SectionResponse>>(sections);

        return response;
    }

	/// <summary>
	/// Get section by Id
	/// </summary>
	/// <response code="200">SectionResponse</response>
	[ProducesResponseType(typeof(SectionResponse), 200)]
    [HttpGet("{id}")]
    public async Task<SectionResponse> GetSectionById([FromRoute] int id)
    {
        var section = await sectionService.GetSection(id);
        var response = mapper.Map<SectionResponse>(section);

        return response;
    }

	/// <summary>
	/// Add section
	/// </summary>
	/// <response code="200">SectionResponse</response>
	[HttpPost("")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<SectionResponse> AddSection([FromBody] AddSectionRequest request)
    {
        var model = mapper.Map<AddSectionModel>(request);
        var section = await sectionService.AddSection(model);
        var response = mapper.Map<SectionResponse>(section);

        return response;
    }

	/// <summary>
	/// Update section
	/// </summary>
	/// <param name="id">Id of section to be updated</param>
	/// <response code="200"></response>
	[HttpPut("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> UpdateSection([FromRoute] int id, [FromBody] UpdateSectionRequest request)
    {
        var model = mapper.Map<UpdateSectionModel>(request);
        await sectionService.UpdateSection(id, model);

        return Ok();
    }

	/// <summary>
	/// Delete section
	/// </summary>
	/// <param name="id">Id of section to be deleted</param>
	/// <response code="200"></response>
	[HttpDelete("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> DeleteSection([FromRoute] int id)
    {
        await sectionService.DeleteSection(id);

        return Ok();
    }
}
