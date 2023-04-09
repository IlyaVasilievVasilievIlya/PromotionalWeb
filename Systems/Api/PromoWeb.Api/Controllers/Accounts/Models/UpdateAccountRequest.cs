using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
	public class UpdateAccountRequest
	{
		public string Email { get; set; }
		public string FullName { get; set; }
		public bool isAdmin { get; set; }
	}

	public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
	{
		public UpdateAccountRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");
		}
	}

	public class UpdateAccountRequestProfile : Profile
	{
		public UpdateAccountRequestProfile()
		{
			CreateMap<UpdateAccountRequest, UpdateAccountModel>();
		}
	}
}
