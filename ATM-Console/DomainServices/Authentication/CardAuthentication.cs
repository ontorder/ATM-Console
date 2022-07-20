using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.BankAccounts;

namespace AtmConsole.DomainServices.Authentication
{
    public class CardAuthentication : ICardAuthentication
    {
        public bool VerifyCard(BankAccount user, int pin)
        {
            return user.CardPin == pin;
        }
    }
}
