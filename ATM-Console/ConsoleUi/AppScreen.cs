using AtmConsole.Domain.UserAccounts;
using AtmConsole.Domain.InternalTransfers;

namespace AtmConsole.ConsoleUi
{
    public class AppScreen
    {
        internal const string cur = "N";

        internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "ATM App";
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\n\n--------------Welcome to My ATM App--------------\n\n");
            Console.WriteLine("Please insert your ATM Card");
            Console.WriteLine("Note: Actual ATM Machines will accept and validate physical ATM Card, " +
                "read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        public static UserAccount UserLoginForm()
        {
            var CardNumber = Validator.Convert<long>("Your card number");
            var CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your Card Pin"));
            var tempUserAccount = new UserAccount(0, 0, 0, CardNumber, CardPin, null, false, 0);

            return tempUserAccount;
        }

        public static void LoginProgress()
        {
            Console.WriteLine("\nChecking Card number and PIN...");
            // I already sent a defualt timer of 10, check the Utility class out
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockedScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch to " +
                "unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullname)
        {
            Console.WriteLine($"Welcome back, {fullname}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("-------My ATM App Menu-------");
            Console.WriteLine(":                            :");
            Console.WriteLine("1. Account Balance           :");
            Console.WriteLine("2. Cash Deposit              :");
            Console.WriteLine("3. Withdrawal                :");
            Console.WriteLine("4. Transfer                  :");
            Console.WriteLine("5. Transactions              :");
            Console.WriteLine("6. Logout                    :");
            //Utility.PressEnterToContinue();
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank You for using my ATM App.");
            Utility.PrintDotAnimation();
            Console.Clear();

        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}500         5.{0}10,000", cur);
            Console.WriteLine(":2.{0}1000        6.{0}15,000", cur);
            Console.WriteLine(":3.{0}2000        7.{0}20,000", cur);
            Console.WriteLine(":4.{0}5000        8.{0}40,000", cur);
            Console.WriteLine(":0.Other");
            Console.WriteLine();

            int selectedAmount = Validator.Convert<int>("option: ");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                //break;
                case 2:
                    return 1000;
                //break;
                case 3:
                    return 2000;
                //break;
                case 4:
                    return 5000;
                case 5:
                    return 10000;
                case 6:
                    return 15000;
                case 7:
                    return 20000;
                case 8:
                    return 40000;
                case 0:
                    return 0;
                default:
                    Utility.PrintMessage("Invalid Input. Try Again.", false);
                    //SelectAmount();
                    return -1;
            }
        }

        internal static InternalTransfer InternalTransferForm()
        {
            long recipientBankAccountNumber = Validator.Convert<long>("Recipient's Account Number");
            decimal transferAmount = Validator.Convert<decimal>($"amount {cur}");
            string recipientBankAccountName = Utility.GetUserInput("Recipient's Account Name");

            return new InternalTransfer(recipientBankAccountName, recipientBankAccountNumber, transferAmount);
        }
    }
}
