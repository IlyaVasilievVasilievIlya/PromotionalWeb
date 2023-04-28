using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
    public class RegisterUserAccountRequest
    {
        /// <summary>
        /// User full name
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// User email
		/// </summary>
		public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Has admin role
        /// </summary>
        public bool isAdmin { get; set; }
    }

    public class RegisterUserAccountRequestValidator : AbstractValidator<RegisterUserAccountRequest>
    {
        public RegisterUserAccountRequestValidator()
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

    public class RegisterUserAccountRequestProfile : Profile
    {
        public RegisterUserAccountRequestProfile()
        {
            CreateMap<RegisterUserAccountRequest, RegisterUserAccountModel>();
        }
    }
}
