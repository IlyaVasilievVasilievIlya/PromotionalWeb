namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Images;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Images;
using Microsoft.AspNetCore.Authorization;
using PromoWeb.Common.Security;
using PromoWeb.Api.AppInfos;
using PromoWeb.Api.Configuration;
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
        var path = Path.Combine(appEnvironment.WebRootPath, "Images/", request.ImageName + Path.GetExtension(request.Image.FileName));

        var model = mapper.Map<AddImageModel>(request);
        model.ImagePath = path;
        var image = await imageService.AddImage(model);

        await using (FileStream stream = new FileStream(path, FileMode.CreateNew))
        {
            await request.Image.CopyToAsync(stream);
        }

        var response = mapper.Map<ImageResponse>(image);

        return response;
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AppScopes.AppApi)]
    public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromForm] UpdateImageRequest request)
    {
        var path = Path.Combine(appEnvironment.WebRootPath, "Images/", request.ImageName + Path.GetExtension(request.Image.FileName));

        var model = mapper.Map<UpdateImageModel>(request); 
        model.ImagePath = path;
        model.ImageStream = request.Image.OpenReadStream();

        await imageService.UpdateImage(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
	[Authorize(Policy = AppScopes.AppApi)]
	public async Task<IActionResult> DeleteImage([FromRoute] int id)
    {
        await imageService.DeleteImage(id);

        return Ok();
    }
}
