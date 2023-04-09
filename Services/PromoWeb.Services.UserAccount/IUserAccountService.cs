namespace PromoWeb.Services.UserAccount
{
    public interface IUserAccountService
    {
        Task<IEnumerable<UserAccountModel>> GetAccounts();

		/// <summary>
		/// Create user account
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		Task<UserAccountModel> Create(RegisterUserAccountModel model);
		Task UpdateAccount(string userName, UpdateAccountModel model); 

		Task ChangePassword(ChangePasswordModel model);
        Task ForgotPassword(ForgotPasswordModel model);
        Task ResetPassword(ResetPasswordModel model);

		Task DeleteAccount(string userName);
	}
}
