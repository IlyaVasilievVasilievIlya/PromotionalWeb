namespace PromoWeb.Api.Controllers;

using AutoMapper;
using PromoWeb.Common.Responses;
using PromoWeb.Services.Contacts;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Contacts;

/// <summary>
/// Contacts controller
/// </summary>
/// <response code="400">Bad Request</response>
/// <response code="401">Unauthorized</response>
/// <response code="403">Forbidden</response>
/// <response code="404">Not Found</response>
[ProducesResponseType(typeof(ErrorResponse), 400)]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/contacts")]
[ApiController]
[ApiVersion("1.0")]
public class ContactsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<ContactsController> logger;
    private readonly IContactService contactService;

    public ContactsController(IMapper mapper, ILogger<ContactsController> logger, IContactService contactService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.contactService = contactService;
    }

    /// <summary>
    /// Get contacts
    /// </summary>
    /// <param name="offset">Offset to the first element</param>
    /// <param name="limit">Count elements on the page</param>
    /// <response code="200">List of ContactResponses</response>
    [ProducesResponseType(typeof(IEnumerable<ContactResponse>), 200)]
    [HttpGet("")]
    public async Task<IEnumerable<ContactResponse>> GetContacts([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var contacts = await contactService.GetContacts(offset, limit);
        var response = mapper.Map<IEnumerable<ContactResponse>>(contacts);

        return response;
    }

    /// <summary>
    /// Get contacts by Id
    /// </summary>
    /// <response code="200">ContactResponse></response>
    [ProducesResponseType(typeof(ContactResponse), 200)]
    [HttpGet("{id}")]
    public async Task<ContactResponse> GetContactById([FromRoute] int id)
    {
        var contact = await contactService.GetContact(id);
        var response = mapper.Map<ContactResponse>(contact);

        return response;
    }

    [HttpPost("")]
    public async Task<ContactResponse> AddContact([FromBody] AddContactRequest request)
    {
        var model = mapper.Map<AddContactModel>(request);
        var contact = await contactService.AddContact(model);
        var response = mapper.Map<ContactResponse>(contact);

        return response;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] UpdateContactRequest request)
    {
        var model = mapper.Map<UpdateContactModel>(request);
        await contactService.UpdateContact(id, model);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id)
    {
        await contactService.DeleteContact(id);

        return Ok();
    }
}
