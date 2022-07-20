namespace AtmConsole.Domain.BankAccounts
{
    public interface IBankAccountRepository
    {
        IEnumerable<BankAccount> Get();
        BankAccount? Find(long userId);
        BankAccount? Search(long cardNumber);
        void Update(BankAccount ua);
    }
}
