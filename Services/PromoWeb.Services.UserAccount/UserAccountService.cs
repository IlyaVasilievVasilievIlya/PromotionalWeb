using AutoMapper;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context.Entities;
using PromoWeb.Services.Actions;
using PromoWeb.Services.EmailSender;
using Microsoft.AspNetCore.Identity;

namespace PromoWeb.Services.UserAccount
{
    public class UserAccountService : IUserAccountService //га каждый запрос в контроллере создаетсяс вновь
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IAction actions;
        private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;

        public UserAccountService(IMapper mapper, UserManager<User> userManager, IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator, IAction actions)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.registerUserAccountModelValidator = registerUserAccountModelValidator;
            this.actions = actions;
        }

        public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
        {
            registerUserAccountModelValidator.Check(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
                throw new ProcessException($"User account with email {model.Email} already exists!");

            user = new User()
            {
                Status = UserStatus.Active, //кастомные 2 поля
                FullName = model.Name,
                //из потомка (identityuser)
                UserName = model.Email, // Это логин. Мы будем его приравнивать к email, хотя это и не обязательно
                Email = model.Email,
                EmailConfirmed = true,// Так как это учебный проект, то сразу считаем, что почта подтверждена. В реальном проекте, скорее всего, надо будет ее подтвердить через ссылку в письме
                PhoneNumber = null,
                PhoneNumberConfirmed = false
            };


            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new ProcessException($"Creating user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");

            await actions.SendEmail(new EmailModel
            {
                Email = model.Email,
                Subject = "PromoWeb notification",
                Message = "You are successfully registered"

            });

            return mapper.Map<UserAccountModel>(user);
        }
    }
}
