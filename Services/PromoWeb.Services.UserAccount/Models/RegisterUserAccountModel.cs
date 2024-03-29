﻿using FluentValidation;

namespace PromoWeb.Services.UserAccount
{
    public class RegisterUserAccountModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
    }

    public class RegisterUserAccountModelValidator : AbstractValidator<RegisterUserAccountModel>
    {
        public RegisterUserAccountModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is invalid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(50).WithMessage("Password is long.");
        }
    }
}
