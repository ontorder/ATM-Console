namespace AtmConsole.Domain.Cards
{
    public abstract class Card
    {
        public long BankAccountId;
        public long CardId;
        public long CardNumber;
        public int CardPin;
        public bool IsLocked;
        public long UserId;

        public Card(long cardId, long cardNumber, int cardPin, bool isLocked, long bankAccountId, long userId)
        {
            CardId = cardId;
            CardNumber = cardNumber;
            CardPin = cardPin;
            IsLocked = isLocked;
            BankAccountId = bankAccountId;
            UserId = userId;
        }
    }
}
