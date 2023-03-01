﻿using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
    public class RegisterUserAccountRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserAccountRequestValidator : AbstractValidator<RegisterUserAccountRequest>
    {
        public RegisterUserAccountRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(50).WithMessage("Password is long."); //регулярку на символы-цифры
        }
    }

    public class RegisterUserAccountRequestProfile : Profile
    {
        public RegisterUserAccountRequestProfile()
        {
            CreateMap<RegisterUserAccountRequest, RegisterUserAccountModel>();
        }
    }
}
