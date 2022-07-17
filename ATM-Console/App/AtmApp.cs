// See https://aka.ms/new-console-template for more information
using AtmConsole.TextUi;
using ConsoleTables;
using AtmConsole.Domain.UserAccounts;
using AtmConsole.Domain.InternalTransfers;
using AtmConsole.Domain.Transactions;
using AtmConsole.Domain.Authentication;

namespace AtmConsole.App
{
    public class AtmApp
    {
        private const decimal _minimumKeptAccount = 500;
        private readonly ICardAuthentication _cardAuth;
        private readonly Repositories.AtmConsoleContext _context;
        private bool _isRunning;
        private UserAccount? _selectedAccount;
        private readonly IUserAuthentication _userAuth;

        public AtmApp(Repositories.AtmConsoleContext context, IUserAuthentication userAuth, ICardAuthentication cardAuth)
        {
            _context = context;
            _userAuth = userAuth;
            _cardAuth = cardAuth;
        }

        public void Run()
        {
            ConsoleUi.Welcome();
            CheckUserNumberAndPassword();
            ConsoleUi.WelcomeCustomer(_selectedAccount.FullName);
            while (_isRunning)
            {
                ConsoleUi.DisplayAppMenu();
                ProcessMainOption();
            }
        }

        public void CheckUserNumberAndPassword()
        {
            _selectedAccount = null;
            while (_selectedAccount == null)
            {
                var cardNumber = ConsoleUi.AskCardNumber();

                ConsoleUi.LoginProgress();
                var inputAccount = _context.UserAccounts.Search(cardNumber);

                // TODO could lock terminal for 30 secs after n failed card numbers
                if (inputAccount == null)
                    continue;

                _selectedAccount = inputAccount;
            }

            bool cardVerified;
            do
            {
                if (_selectedAccount.IsLocked)
                {
                    ConsoleUi.PrintLockedScreen();
                    _isRunning = false;
                    break;
                }

                int pin = ConsoleUi.AskPinNumber();

                // TODO multiple logics depending on user roles and card types (btw separate user and card)
                // for example gold card could withdraw more money than then silver one
                cardVerified = _cardAuth.VerifyCard(_selectedAccount, pin);
                _userAuth.UpdateUserAuth(_selectedAccount, cardVerified);

                if (cardVerified == false)
                {
                    Utility.PrintMessage("\nInvalid Card Number or PIN .", false);
                    Console.WriteLine($"Pin check failed {_selectedAccount.AuthFailsCount} times");
                    break;
                }

                Console.Clear();
            } while (cardVerified == false);
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(_selectedAccount.AccountBalance)}");
            Utility.PressEnterToContinue();
            ProcessMainOption();
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\n Only multiples of 500 and 1000 naira allowed");
            var transaction_amt = Validator.Convert<int>($"amount {ConsoleUi.cur}");

            Console.WriteLine("\n Checking and Counting Bank Notes");
            Utility.PrintDotAnimation();
            Console.WriteLine("");
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than 0. Try Again.", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in mutiple of 500 or 1000.Try Again.", false);
                return;
            }
            if (PreviewBankNotes(transaction_amt) == false)
            {
                Utility.PrintMessage("You have cancelled your action.", false);
                return;
            }

            InsertTransaction(_selectedAccount.UserAccountId, TransactionType.Deposit, transaction_amt, "");
            _selectedAccount.AccountBalance += transaction_amt;
            Utility.PrintMessage($"Deposit of {transaction_amt} has been submitted successfully", true);
        }

        public void MakeWithdrawal()
        {
            int transaction_amt;
            int selectedAmount = ConsoleUi.SelectAmount();
            if (selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {ConsoleUi.cur}");
            }

            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try Again", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000", false);
                return;
            }

            if (transaction_amt > _selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account balance is too low to withdraw" +
                    $"{Utility.FormatAmount(transaction_amt)}");
                return;
            }

            if (_selectedAccount.AccountBalance - transaction_amt < _minimumKeptAccount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of{Utility.FormatAmount(_minimumKeptAccount)} ", false);
                return;
            }

            InsertTransaction(_selectedAccount.UserAccountId, TransactionType.Withdrawal, transaction_amt, " ");
            _selectedAccount.AccountBalance -= transaction_amt;
            Utility.PrintMessage($"You have successfully withdrawn {Utility.FormatAmount(transaction_amt)}", true);
        }

        public void InsertTransaction(long userBankAccountId, TransactionType transactionType, decimal transactionAmount, string description)
        {
            var transaction = new Transaction(default, description, transactionAmount, DateTime.Now, transactionType, userBankAccountId);
            _context.Transactions.Add(transaction);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _context.Transactions.Get().Where(t => t.UserBankAccountID == _selectedAccount.UserAccountId).ToList();

            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", false);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Description", "Amount" + ConsoleUi.cur);
                foreach (var transaction in filteredTransactionList)
                {
                    table.AddRow(transaction.TransactionId, transaction.TransactionDate, transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }
        }

        private void ProcessMainOption()
        {
            switch (Validator.Convert<int>("An Option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;

                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;

                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;

                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = ConsoleUi.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;

                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;

                case (int)AppMenu.Logout:
                    ConsoleUi.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM Card");
                    _isRunning = false;
                    break;

                default:
                    Utility.PrintMessage("Invalid Option", false);
                    break;
            }
        }

        private bool PreviewBankNotes(int amt)
        {
            int thousandNotes = amt / 1000;
            int fiveHundredNotes = (amt % 1000) / 500;
            Console.WriteLine("\n Summary");
            Console.WriteLine("------");
            Console.WriteLine($"{ConsoleUi.cur}1000 X {thousandNotes} = {1000 * thousandNotes}");
            Console.WriteLine($"{ConsoleUi.cur}500 X {fiveHundredNotes} = {500 * fiveHundredNotes}");
            Console.WriteLine($"Total Amount:{Utility.FormatAmount(amt)}\n\n");

            int opt = Validator.Convert<int>("1 to Confirm");
            return opt.Equals(1);
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try Again", false);
                return;
            }
            if (internalTransfer.TransferAmount > _selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance to transfer " +
                    $"a balance of {Utility.FormatAmount(internalTransfer.TransferAmount)} ", false);
                return;
            }

            if (_selectedAccount.AccountBalance - internalTransfer.TransferAmount < _minimumKeptAccount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of {Utility.FormatAmount(_minimumKeptAccount)} ", false);
                return;
            }

            //Checking Receiver's bank account is valid
            var selectedBankAccountReceiver = _context.UserAccounts.Get()
                .Where(ua => ua.AccountNumber == internalTransfer.RecipientBankAccountNumber)
                .FirstOrDefault();

            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer Failed. Receiver bank account is invalid.", false);
                return;
            }

            if (selectedBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recepient's bank account name is invalid.", false);
                return;
            }

            InsertTransaction(_selectedAccount.UserAccountId, TransactionType.Transfer, internalTransfer.TransferAmount, $"Transferred Amount to {selectedBankAccountReceiver.FullName} {selectedBankAccountReceiver.AccountNumber}");

            _selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            InsertTransaction(selectedBankAccountReceiver.UserAccountId, TransactionType.Transfer, internalTransfer.TransferAmount, $"Received Amount from {_selectedAccount.FullName} {_selectedAccount.AccountNumber}");

            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;

            Utility.PrintMessage($"You have successfuly transferred {Utility.FormatAmount(internalTransfer.TransferAmount)} to {internalTransfer.RecipientBankAccountName}", true);
        }
    }
}
