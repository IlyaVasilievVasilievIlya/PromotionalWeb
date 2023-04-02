namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Links;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Links;
using Microsoft.AspNetCore.Authorization;
using PromoWeb.Common.Security;


/// <summary>
/// Links controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/links")]
[ApiController]
[ApiVersion("1.0")]
public class LinksController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<LinksController> logger;
    private readonly ILinkService linkService;

    public LinksController(IMapper mapper, ILogger<LinksController> logger, ILinkService linkService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.linkService = linkService;
    }

    /// <summary>
    /// Get links
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of LinkResponses</response>
    [ProducesResponseType(typeof(IEnumerable<LinkResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<LinkResponse>> GetLinks([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var links = await linkService.GetLinks(offset, limit);
        var response = mapper.Map<IEnumerable<LinkResponse>>(links);

        return response;
    }

	[ProducesResponseType(typeof(IEnumerable<LinkResponse>), 200)]
	[HttpGet("bySection/{sectionId}")]
	public async Task<IEnumerable<LinkResponse>> GetLinksBySectionId([FromRoute] int sectionId)
	{
		var links = await linkService.GetLinksBySectionId(sectionId);
		var response = mapper.Map<IEnumerable<LinkResponse>>(links);

		return response;
	}

	/// <summary>
	/// Get links by Id
	/// </summary>
	/// <response code="200">LinkResponse></response>
	[ProducesResponseType(typeof(LinkResponse), 200)]
    [HttpGet("{id}")]
    public async Task<LinkResponse> GetLinkById([FromRoute] int id)
    {
        var link = await linkService.GetLink(id);
        var response = mapper.Map<LinkResponse>(link);

        return response;
    }

    [HttpPost("")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<LinkResponse> AddLink([FromBody] AddLinkRequest request)
    {
        var model = mapper.Map<AddLinkModel>(request);
        var link = await linkService.AddLink(model);
        var response = mapper.Map<LinkResponse>(link);

        return response;
    }

    [HttpPut("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> UpdateLink([FromRoute] int id, [FromBody] UpdateLinkRequest request)
    {
        var model = mapper.Map<UpdateLinkModel>(request);
        await linkService.UpdateLink(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> DeleteLink([FromRoute] int id)
    {
        await linkService.DeleteLink(id);

        return Ok();
    }
}
