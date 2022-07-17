using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Domain.Authentication
{
    public interface IUserAuthentication
    {
        void UpdateUserAuth(UserAccount ua, bool succeeded);
    }
}
