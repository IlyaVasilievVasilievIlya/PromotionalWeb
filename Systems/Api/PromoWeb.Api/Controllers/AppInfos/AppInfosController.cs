namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.AppInfos;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.AppInfos;
using Microsoft.AspNetCore.Authorization;
using PromoWeb.Common.Security;
using PromoWeb.Services.Images;


/// <summary>
/// AppInfos controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/appInfos")]
[ApiController]
[ApiVersion("1.0")]
public class AppInfosController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<AppInfosController> logger;
    private readonly IAppInfoService appInfoService;
    private readonly IImageService imageService;
	private readonly IWebHostEnvironment appEnvironment;

	public AppInfosController(IMapper mapper, ILogger<AppInfosController> logger, IAppInfoService appInfoService,
                                IImageService imageService, IWebHostEnvironment appEnvironment)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.appInfoService = appInfoService;
        this.imageService = imageService;
        this.appEnvironment = appEnvironment;
    }

    /// <summary>
    /// Get appInfos
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of AppInfoResponses</response>
    [ProducesResponseType(typeof(IEnumerable<AppInfoResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<AppInfoResponse>> GetAppInfos([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var appInfos = await appInfoService.GetAppInfos(offset, limit);
        var response = mapper.Map<IEnumerable<AppInfoResponse>>(appInfos);

        return response;
    }

	/// <summary>
	/// Get appInfos by section
	/// </summary>
	/// <param name="sectionId">Section id</param>
	/// <response code="200">List of AppInfoResponses</response>
	[ProducesResponseType(typeof(IEnumerable<AppInfoResponse>), 200)]
    [HttpGet("bySection/{sectionId}")]
    public async Task<IEnumerable<AppInfoResponse>> GetAppInfosBySectionId(int sectionId)
    {
        var appInfos = await appInfoService.GetAppInfosBySectionId(sectionId);
        var response = mapper.Map<IEnumerable<AppInfoResponse>>(appInfos);

        return response;
    }


    /// <summary>
    /// Get appInfo by Id
    /// </summary>
    /// <response code="200">AppInfoResponse</response>
    [ProducesResponseType(typeof(AppInfoResponse), 200)]
    [HttpGet("{id}")]
    public async Task<AppInfoResponse> GetAppInfoById([FromRoute] int id)
    {
        var appInfo = await appInfoService.GetAppInfo(id);
        var response = mapper.Map<AppInfoResponse>(appInfo);

        return response;
    }

	/// <summary>
	/// Add appInfo
	/// </summary>
	/// <response code="200">AppInfoResponse</response>
	[HttpPost("")]
    [Authorize(Policy = AppScopes.AppApi)]
    public async Task<AppInfoResponse> AddAppInfo([FromBody] AddAppInfoRequest request)
    {
        var model = mapper.Map<AddAppInfoModel>(request);
        var appInfo = await appInfoService.AddAppInfo(model);
        var response = mapper.Map<AppInfoResponse>(appInfo);

        return response;
    }

	/// <summary>
	/// Update appInfo
	/// </summary>
	/// <param name="id">AppInfo id to be updated</param>
	/// <response code="200"></response>
	[HttpPut("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> UpdateAppInfo([FromRoute] int id, [FromBody] UpdateAppInfoRequest request)
    {
        var model = mapper.Map<UpdateAppInfoModel>(request);
        await appInfoService.UpdateAppInfo(id, model);

        return Ok();
    }

	/// <summary>
	/// Delete appInfo
	/// </summary>
	/// <param name="id">AppInfo id to be deleted</param>
	/// <response code="200"></response>
	[HttpDelete("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> DeleteAppInfo([FromRoute] int id)
	{

		var imageFiles = (await imageService.GetImagesByAppInfoId(id)).Select(x => x.UniqueName);
		await appInfoService.DeleteAppInfo(id);
        string path;
        foreach (var filename in imageFiles)
        {
		    path = Path.Combine(appEnvironment.WebRootPath, "Images/", filename);
			System.IO.File.Delete(path);
        }
            
		return Ok();
    }
}
