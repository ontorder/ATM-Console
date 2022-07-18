namespace AtmConsole.Domain.UserAccounts
{
    public interface IUserAccountRepository
    {
        IEnumerable<UserAccount> Get();
        UserAccount? Find(long userId);
        UserAccount? Search(long cardNumber);
        void Update(UserAccount ua);
    }
}
