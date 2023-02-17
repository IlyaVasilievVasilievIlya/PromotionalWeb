namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Images;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Images;


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

    public ImagesController(IMapper mapper, ILogger<ImagesController> logger, IImageService imageService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.imageService = imageService;
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

    [HttpPost("")]
    public async Task<ImageResponse> AddImage([FromBody] AddImageRequest request)
    {
        var model = mapper.Map<AddImageModel>(request);
        var image = await imageService.AddImage(model);
        var response = mapper.Map<ImageResponse>(image);

        return response;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromBody] UpdateImageRequest request)
    {
        var model = mapper.Map<UpdateImageModel>(request);
        await imageService.UpdateImage(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage([FromRoute] int id)
    {
        await imageService.DeleteImage(id);

        return Ok();
    }
}
