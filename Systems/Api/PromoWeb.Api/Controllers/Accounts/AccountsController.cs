using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Api.Controllers.Accounts.Models;
using PromoWeb.Services.UserAccount;

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

        [HttpPost("")]
        [Authorize(Policy = "admin")]
		public async Task<UserAccountResponse> Register([FromQuery] RegisterUserAccountRequest request)
        {
            var user = await userAccountService.Create(mapper.Map<RegisterUserAccountModel>(request));

            var responce = mapper.Map<UserAccountResponse>(user);

            return responce;
        }
    }
}
