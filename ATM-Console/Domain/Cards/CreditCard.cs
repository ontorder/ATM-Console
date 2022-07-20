namespace AtmConsole.Domain.Cards
{
    public class CreditCard : Card
    {
        public CreditCard(long cardId, long cardNumber, int cardPin, bool isLocked, long bankAccountId, long userId)
            : base(cardId, cardNumber, cardPin, isLocked, bankAccountId, userId)
        {
        }
    }
}
