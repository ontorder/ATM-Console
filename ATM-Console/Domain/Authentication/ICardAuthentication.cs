using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Domain.Authentication
{
    public interface ICardAuthentication
    {
        bool VerifyCard(UserAccount user, int pin);
    }
}
