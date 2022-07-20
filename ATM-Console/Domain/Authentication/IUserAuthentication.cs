using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.Domain.Authentication
{
    public interface IUserAuthentication
    {
        void UpdateUserAuth(BankAccount ua, bool succeeded);
    }
}
