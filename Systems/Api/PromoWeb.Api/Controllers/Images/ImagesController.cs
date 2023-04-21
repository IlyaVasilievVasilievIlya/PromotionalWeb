namespace PromoWeb.Api.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Images;
using PromoWeb.Common.Extensions;
using PromoWeb.Common.Responses;
using PromoWeb.Common.Security;
using PromoWeb.Services.Images;

//using Serilog;


/// <summary>
/// Images controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/images")]
[ApiController]
[ApiVersion("1.0")]
public class ImagesController : ControllerBase
{
	private readonly IMapper mapper;
	private readonly ILogger<ImagesController> logger;
	private readonly IImageService imageService;
	private readonly IWebHostEnvironment appEnvironment;

	public ImagesController(IMapper mapper, ILogger<ImagesController> logger, IImageService imageService, IWebHostEnvironment appEnvironment)
	{
		this.mapper = mapper;
		this.logger = logger;
		this.imageService = imageService;
		this.appEnvironment = appEnvironment;
	}

	/// <summary>
	/// Get images
	/// </summary>
	/// <param name="offset">Offset to the first element</param>
	/// <param name="limit">Count elements on the page</param>
	/// <response code="200">List of ImageResponses</response>
	[ProducesResponseType(typeof(IEnumerable<ImageResponse>), 200)]
	[HttpGet("")]
	public async Task<IEnumerable<ImageResponse>> GetImages([FromQuery] int offset = 0, [FromQuery] int limit = 10)
	{
		var images = await imageService.GetImages(offset, limit);
		var response = mapper.Map<IEnumerable<ImageResponse>>(images);

		return response;
	}

	/// <summary>
	/// Get images by Id
	/// </summary>
	/// <response code="200">ImageResponse></response>
	[ProducesResponseType(typeof(ImageResponse), 200)]
	[HttpGet("{id}")]
	public async Task<ImageResponse> GetImageById([FromRoute] int id)
	{
		var image = await imageService.GetImage(id);
		var response = mapper.Map<ImageResponse>(image);

		return response;
	}

	[ProducesResponseType(typeof(IEnumerable<ImageResponse>), 200)]
	[HttpGet("byAppInfo/{appInfoId}")]
	public async Task<IEnumerable<ImageResponse>> GetImagesByAppInfoId(int appInfoId)
	{
		var images = await imageService.GetImagesByAppInfoId(appInfoId);
		var response = mapper.Map<IEnumerable<ImageResponse>>(images);

		return response;
	}

	[HttpPost("")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<ImageResponse> AddImage([FromForm] AddImageRequest request)
	{
		string uniqueName = GuidExtension.GetUniqueFileName(request.Image.FileName, 5);
		var path = Path.Combine(appEnvironment.WebRootPath, "Images/", uniqueName);

		var model = mapper.Map<AddImageModel>(request);
		model.UniqueName = uniqueName;

		var image = await imageService.AddImage(model);

		await request.Image.CopyToAsync(new FileStream(path, FileMode.Create));

		var response = mapper.Map<ImageResponse>(image);

		return response;
	}

	[HttpPut("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromForm] UpdateImageRequest request)
	{
		string uniqueName = GuidExtension.GetUniqueFileName(request.Image.FileName, 5);

		var model = mapper.Map<UpdateImageModel>(request);
		model.UniqueName = uniqueName;

		var image = await imageService.GetImage(id);

		await imageService.UpdateImage(id, model);

		var path = Path.Combine(appEnvironment.WebRootPath, "Images/", uniqueName);
		var oldPath = Path.Combine(appEnvironment.WebRootPath, "Images/", image.UniqueName);

		System.IO.File.Delete(oldPath);
		await request.Image.CopyToAsync(new FileStream(path, FileMode.Create));

		return Ok();
	}

	[HttpDelete("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> DeleteImage([FromRoute] int id)
	{
		var image = await imageService.GetImage(id);
		await imageService.DeleteImage(id);
		
		var path = Path.Combine(appEnvironment.WebRootPath, "Images/", image.UniqueName);

		System.IO.File.Delete(path);

		return Ok();
	}
}
