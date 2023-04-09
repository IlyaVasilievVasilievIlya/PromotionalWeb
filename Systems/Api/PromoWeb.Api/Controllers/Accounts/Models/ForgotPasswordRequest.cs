﻿using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
	public class ForgotPasswordRequest
	{
		public string Email { get; set; }
	}

	public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
	{
		public ForgotPasswordRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");
		}
	}

	public class ForgotPasswordRequestProfile : Profile
	{
		public ForgotPasswordRequestProfile()
		{
			CreateMap<ForgotPasswordRequest, ForgotPasswordModel>();
		}
	}
}
