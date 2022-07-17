namespace AtmConsole.Domain.UserAccounts
{
    public interface IUserAccountRepository
    {
        IEnumerable<UserAccount> Get();
        UserAccount? Search(long cardNumber);
        void Update(UserAccount ua);
    }
}
