using AtmConsole.Domain.Authentication;
using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.DomainServices.Authentication
{
    public class CardAuthentication : ICardAuthentication
    {
        public bool VerifyCard(UserAccount user, int pin)
        {
            return user.CardPin == pin;
        }
    }
}
