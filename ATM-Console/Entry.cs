using AtmConsole.App;
using AtmConsole.Repositories.Transactions;
using AtmConsole.Repositories.UserAccounts;

namespace AtmConsole
{
    internal class Entry
    {
        static void Main()
        {
            var userAccounts = new UserAccountsStaticRepository();
            var transactions = new TransactionMemoryRepository();
            AtmApp atmApp = new(userAccounts, transactions);
            atmApp.Run();
            //Utility.PressEnterToContinue();
        }
    }
}
