namespace AtmConsole
{
    public class Atm
    {
        public int Amount = 10000;
        private List<Logs> _allLogs = new();

        public int MakeWithdrawal(int amount, string reason)
            => Amount - amount;

        public int MakeDeposit(int amount)
            => Amount + amount;
    }
}
