namespace AtmConsole.Domain.Cards
{
    public class DebitCard : Card
    {
        public DebitCard(long cardId, long cardNumber, int cardPin, bool isLocked, long bankAccountId, long userId)
            : base(cardId, cardNumber, cardPin, isLocked, bankAccountId, userId)
        {
        }
    }
}
