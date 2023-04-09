using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Actions;
using PromoWeb.Services.EmailSender;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace PromoWeb.Services.UserAccount
{
    public class UserAccountService : IUserAccountService //га каждый запрос в контроллере создаетсяс вновь
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IAction actions;
        private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;
        private readonly IModelValidator<ForgotPasswordModel> forgotPasswordModelValidator;
        private readonly IModelValidator<ResetPasswordModel> resetPasswordModelValidator;
        private readonly IModelValidator<ChangePasswordModel> changePasswordModelValidator;
        private readonly IModelValidator<UpdateAccountModel> updateAccountModelValidator;



		public UserAccountService(IMapper mapper, UserManager<User> userManager, 
            IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator, IAction actions, 
            IModelValidator<ForgotPasswordModel> forgotPasswordModelValidator, IModelValidator<ResetPasswordModel> resetPasswordModelValidator,
            IModelValidator<ChangePasswordModel> changePasswordModelValidator, IModelValidator<UpdateAccountModel> updateAccountModelValidator)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.registerUserAccountModelValidator = registerUserAccountModelValidator;
            this.forgotPasswordModelValidator = forgotPasswordModelValidator;
            this.resetPasswordModelValidator = resetPasswordModelValidator;
            this.changePasswordModelValidator = changePasswordModelValidator;
            this.updateAccountModelValidator = updateAccountModelValidator;
			this.actions = actions;
        }

		public async Task<IEnumerable<UserAccountModel>> GetAccounts()
        {
            var users =  userManager.Users.AsQueryable();

            var data = (await users.ToListAsync())
                .Select(user => new UserAccountModel
                {   
                    Role = userManager.IsInRoleAsync(user, "admin").Result ? "admin" : "moderator",
                    Email = user.Email,
                    Id = user.Id, 
                    Name = user.FullName 
                });

			return data;
		}

		public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
        {
            registerUserAccountModelValidator.Check(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
                throw new ProcessException($"User account with email {model.Email} already exists!");

            user = new User()
            {
                Status = UserStatus.Active,
                FullName = model.Name,
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,// Так как это учебный проект, то сразу считаем, что почта подтверждена. В реальном проекте, скорее всего, надо будет ее подтвердить через ссылку в письме
                PhoneNumber = null,
                PhoneNumberConfirmed = false
            };


            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new ProcessException($"Creating user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");

            await userManager.AddToRoleAsync(user, model.isAdmin ? "admin" : "moderator");

			await actions.SendEmail(new EmailModel
            {
                Email = model.Email,
                Subject = "PromoWeb notification",
                Message = "You are successfully registered"
            });

            return mapper.Map<UserAccountModel>(user);
        }

		public async Task UpdateAccount(string userName, UpdateAccountModel model)
        {
			updateAccountModelValidator.Check(model);

			var user = await userManager.FindByNameAsync(userName);
			if (user == null)
				throw new ProcessException($"User account with email {model.Email} does not exist!");

            user.UserName = model.Email;
            user.FullName = model.FullName;

            string newRole = model.isAdmin ? "admin" : "moderator";

			var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(newRole))
            {
				await userManager.RemoveFromRolesAsync(user, userRoles);
				await userManager.AddToRoleAsync(user, newRole);
			}

			await userManager.UpdateAsync(user);
		}

		public async Task ChangePassword(ChangePasswordModel model)
        {
			changePasswordModelValidator.Check(model);

            var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
				throw new ProcessException($"User account with email {model.Email} does not exist!");

			var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

			if (!result.Succeeded)
				throw new ProcessException($"Changing password error. {string.Join(", ", result.Errors.Select(s => s.Description))}");
		}

		public async Task ForgotPassword(ForgotPasswordModel model)
		{
			forgotPasswordModelValidator.Check(model);

			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
				throw new ProcessException($"User account with email {model.Email} does not exist!");

			var code = await userManager.GeneratePasswordResetTokenAsync(user);

			await actions.SendEmail(new EmailModel
			{
				Email = model.Email,
				Subject = "Reset Password",
				Message = $"Token to reset password: {code}"
			});
		}

        public async Task ResetPassword(ResetPasswordModel model)
        {
			resetPasswordModelValidator.Check(model);

			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
				throw new ProcessException($"User account with email {model.Email} does not exist!");

			var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

			if (!result.Succeeded)
				throw new ProcessException($"Resetting user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");
		}

		public async Task DeleteAccount(string userName)
		{
            var user = await userManager.FindByNameAsync(userName)
			    ?? throw new ProcessException($"The user (Login: {userName}) was not found");

			await userManager.DeleteAsync(user);
		}
	}
}
