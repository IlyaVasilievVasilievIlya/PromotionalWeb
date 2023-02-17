namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Common.Security;
using PromoWeb.Services.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Questions controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/questions")]
[ApiController]
[ApiVersion("1.0")]

public class QuestionsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<QuestionsController> logger;
    private readonly IQuestionService questionService;

    public QuestionsController(IMapper mapper, ILogger<QuestionsController> logger, IQuestionService questionService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.questionService = questionService;
    }


    /// <summary>
    /// Get questions
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of QuestionResponses</response>
    [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<QuestionResponse>> GetQuestions([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var questions = await questionService.GetQuestions(offset, limit);
        var response = mapper.Map<IEnumerable<QuestionResponse>>(questions);

        return response;
    }

    /// <summary>
    /// Get questions by Id
    /// </summary>
    /// <response code="200">QuestionResponse></response>
    [ProducesResponseType(typeof(QuestionResponse), 200)]
    [HttpGet("{id}")]
    public async Task<QuestionResponse> GetQuestionById([FromRoute] int id)
    {
        var question = await questionService.GetQuestion(id);
        var response = mapper.Map<QuestionResponse>(question);

        return response;
    }

    [HttpPost("")]
    public async Task<QuestionResponse> AddQuestion([FromBody] AddQuestionRequest request)
    {
        var model = mapper.Map<AddQuestionModel>(request);
        var question = await questionService.AddQuestion(model);
        var response = mapper.Map<QuestionResponse>(question);

        return response;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
    {
        await questionService.DeleteQuestion(id);

        return Ok();
    }
}
