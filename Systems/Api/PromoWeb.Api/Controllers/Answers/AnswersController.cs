namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Answers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PromoWeb.Common.Security;


/// <summary>
/// Answers controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/answers")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class AnswersController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<AnswersController> logger;
    private readonly IAnswerService answerService;

    public AnswersController(IMapper mapper, ILogger<AnswersController> logger, IAnswerService answerService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.answerService = answerService;
    }

    /// <summary>
    /// Get answers
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of AnswerResponses</response>
    [ProducesResponseType(typeof(IEnumerable<AnswerResponse>), 200)]
    [HttpGet("")]
    [Authorize(Policy = AppScopes.AnswerRead)]
    public async Task<IEnumerable<AnswerResponse>> GetAnswers([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var answers = await answerService.GetAnswers(offset, limit);
        var response = mapper.Map<IEnumerable<AnswerResponse>>(answers);

        return response;
    }

    /// <summary>
    /// Get answers by Id
    /// </summary>
    /// <response code="200">AnswerResponse></response>
    [ProducesResponseType(typeof(AnswerResponse), 200)]
    [HttpGet("{id}")]
    [Authorize(Policy = AppScopes.AnswerRead)]
    public async Task<AnswerResponse> GetAnswerById([FromRoute] int id)
    {
        var answer = await answerService.GetAnswer(id);
        var response = mapper.Map<AnswerResponse>(answer);

        return response;
    }

    [HttpPost("")]
    [Authorize(Policy = AppScopes.AnswerWrite)]
    public async Task<AnswerResponse> AddAnswer([FromBody] AddAnswerRequest request)
    {
        var model = mapper.Map<AddAnswerModel>(request);
        var answer = await answerService.AddAnswer(model);
        var response = mapper.Map<AnswerResponse>(answer);

        return response;
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AppScopes.AnswerWrite)]
    public async Task<IActionResult> DeleteAnswer([FromRoute] int id)
    {
        await answerService.DeleteAnswer(id);

        return Ok();
    }
}
