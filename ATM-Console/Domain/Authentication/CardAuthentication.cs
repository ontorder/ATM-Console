using AtmConsole.Domain.UserAccounts;

namespace AtmConsole.Domain.Authentication
{
    public class CardAuthentication : ICardAuthentication
    {
        public bool VerifyCard(UserAccount user, int pin)
        {
            return user.CardPin == pin;
        }
    }
}
