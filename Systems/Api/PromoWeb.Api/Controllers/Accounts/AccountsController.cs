using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Controllers.Accounts.Models;
using PromoWeb.Common.Exceptions;
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

		[HttpGet("")]
		[ProducesResponseType(typeof(IEnumerable<UserAccountResponse>), 200)]
		[Authorize(Policy = "admin")]
		public async Task<IEnumerable<UserAccountResponse>> GetAccounts()
		{
			var accounts = await userAccountService.GetAccounts();
			var response = mapper.Map<IEnumerable<UserAccountResponse>>(accounts);

			return response;
		}

		[HttpPost("")]
        [Authorize(Policy = "admin")]
		public async Task<UserAccountResponse> Register([FromQuery] RegisterUserAccountRequest request)
        {
            var user = await userAccountService.Create(mapper.Map<RegisterUserAccountModel>(request));

            var responce = mapper.Map<UserAccountResponse>(user);

            return responce;
        }

		[HttpPut("{userName}")]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> UpdateAccount([FromRoute] string userName, [FromBody] UpdateAccountRequest request)
		{
			var model = mapper.Map<UpdateAccountModel>(request);
			await userAccountService.UpdateAccount(userName, model);

			return Ok();
		}

		[HttpPost("password/change")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
		{
			var model = mapper.Map<ChangePasswordModel>(request);
			await userAccountService.ChangePassword(model);

			return Ok();
		}

		[HttpPost("password/forgot")]
		[AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
			
			var model = mapper.Map<ForgotPasswordModel>(request);
			await userAccountService.ForgotPassword(model);

			return Ok();
		}

		[HttpPost("password/reset")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var model = mapper.Map<ResetPasswordModel>(request);
			await userAccountService.ResetPassword(model);

			return Ok();
		}

		[HttpDelete("{userName}")]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> DeleteAccount([FromRoute] string userName)
		{
			await userAccountService.DeleteAccount(userName);

			return Ok();
		}
	}
}
