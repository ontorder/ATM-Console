using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.Domain.Authentication
{
    public interface ICardAuthentication
    {
        bool VerifyCard(BankAccount user, int pin);
    }
}
