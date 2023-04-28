using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Controllers.Accounts.Models;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Security;
using PromoWeb.Services.UserAccount;
using System.Security.Claims;

namespace PromoWeb.Api.Controllers.Accounts
{
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<AccountsController> logger;
        private readonly IUserAccountService userAccountService;

        public AccountsController(IMapper mapper, ILogger<AccountsController> logger, IUserAccountService userAccountService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.userAccountService = userAccountService;
        }

		/// <summary>
		/// Get user accounts
		/// </summary>
		/// <response code="200">List of UserAccountResponse</response>
		[HttpGet("")]
		[ProducesResponseType(typeof(IEnumerable<UserAccountResponse>), 200)]
		[Authorize(Policy = Roles.Admin)]
		public async Task<IEnumerable<UserAccountResponse>> GetAccounts()
		{
			var accounts = await userAccountService.GetAccounts();
			var response = mapper.Map<IEnumerable<UserAccountResponse>>(accounts);

			return response;
		}

		/// <summary>
		/// Register user account
		/// </summary>
		/// <response code="200">UserAccountResponse</response>
		[HttpPost("")]
        [Authorize(Policy = Roles.Admin)]
		public async Task<UserAccountResponse> Register([FromQuery] RegisterUserAccountRequest request)
        {
            var user = await userAccountService.Create(mapper.Map<RegisterUserAccountModel>(request));

            var responce = mapper.Map<UserAccountResponse>(user);

            return responce;
        }

		/// <summary>
		/// Update user account
		/// </summary>
		/// <param name="userName">Unique user name</param>
		/// <response code="200"></response>
		[HttpPut("{userName}")]
		[Authorize(Policy = Roles.Admin)]
		public async Task<IActionResult> UpdateAccount([FromRoute] string userName, [FromBody] UpdateAccountRequest request)
		{
			var model = mapper.Map<UpdateAccountModel>(request);
			await userAccountService.UpdateAccount(userName, model);

			return Ok();
		}

		/// <summary>
		/// Change password
		/// </summary>
		/// <response code="200"></response>
		[HttpPost("password/change")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
		{
			var model = mapper.Map<ChangePasswordModel>(request);
			await userAccountService.ChangePassword(model);

			return Ok();
		}

		/// <summary>
		/// Password recovery
		/// </summary>
		/// <response code="200"></response>
		[HttpPost("password/forgot")]
		[AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
			var model = mapper.Map<ForgotPasswordModel>(request);
			await userAccountService.ForgotPassword(model);

			return Ok();
		}

		/// <summary>
		/// Password recovery
		/// </summary>
		/// <response code="200"></response>
		[HttpPost("password/reset")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var model = mapper.Map<ResetPasswordModel>(request);
			await userAccountService.ResetPassword(model);

			return Ok();
		}

		/// <summary>
		/// Delete user account
		/// </summary>
		/// <param name="userName">Unique user name</param>
		/// <response code="200"></response>
		[HttpDelete("{userName}")]
		[Authorize(Policy = Roles.Admin)]
		public async Task<IActionResult> DeleteAccount([FromRoute] string userName)
		{
			await userAccountService.DeleteAccount(userName);

			return Ok();
		}
	}
}
