// See https://aka.ms/new-console-template for more information
using AtmConsole.ConsoleUi;
using ConsoleTables;
using AtmConsole.Domain.UserAccounts;
using AtmConsole.Domain.InternalTransfers;
using AtmConsole.Domain.Transactions;

namespace AtmConsole.App
{
    public class AtmApp
    {
        private const decimal _minimumKeptAccount = 500;
        private readonly AppScreen screen;
        private UserAccount? _selectedAccount;
        private ITransactionRepository _transactions;
        private IUserAccountsRepository _userAccounts;

        public AtmApp(IUserAccountsRepository userAccountList, ITransactionRepository transactions)
        {
            screen = new AppScreen();
            _userAccounts = userAccountList;
            _transactions = transactions;
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserNumberAndPassword();
            AppScreen.WelcomeCustomer(_selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMainOption();
            }
        }

        public void CheckUserNumberAndPassword()
        {
            bool isCorrectLogin = false;

            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();

                AppScreen.LoginProgress();
                foreach (UserAccount account in _userAccounts.Get())
                {
                    _selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(_selectedAccount.CardNumber))
                    {
                        _selectedAccount.TotalLogin++;
                        //Console.WriteLine(selectedAccount.TotalLogin);
                        if (inputAccount.CardPin.Equals(_selectedAccount.CardPin))
                        {
                            _selectedAccount = account;
                            if (_selectedAccount.IsLocked || _selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockedScreen();
                            }
                            else
                            {
                                _selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                        if (isCorrectLogin == false)
                        {
                            Utility.PrintMessage("\nInvalid Card Number or PIN .", false);
                            Console.WriteLine(_selectedAccount.TotalLogin);
                            _selectedAccount.IsLocked = _selectedAccount.TotalLogin == 3;
                            if (_selectedAccount.IsLocked)
                            {
                                AppScreen.PrintLockedScreen();
                            }
                        }
                        Console.Clear();
                    }
                }
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is:{Utility.FormatAmount(_selectedAccount.AccountBalance)}");
            Utility.PressEnterToContinue();
            ProcessMainOption();
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\n Only multiples of 500 and 1000 naira allowed");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            //Simulating Counting
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
            //Update Account balance
            _selectedAccount.AccountBalance += transaction_amt;
            //Print Message
            Utility.PrintMessage($"Deposit of {transaction_amt} has been submitted successfully", true);

            //ProcessMainOption();
        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
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
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            }
            //Input Validation
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

            //Business Logic Validation
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

            //Bind Withdrawal details to transaction object

            InsertTransaction(_selectedAccount.UserAccountId, TransactionType.Withdrawal, transaction_amt, " ");
            //Updating the account balance
            _selectedAccount.AccountBalance -= transaction_amt;
            //Printing Success Message
            Utility.PrintMessage($"You have successfully withdrawn {Utility.FormatAmount(transaction_amt)}", true);

            //ProcessMainOption();
        }

        public void InsertTransaction(long userBankAccountId, TransactionType transactionType, decimal transactionAmount, string description)
        {
            var transaction = new Transaction(default, description, transactionAmount, DateTime.Now, transactionType, userBankAccountId);
            _transactions.Add(transaction);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _transactions.Get().Where(t => t.UserBankAccountID == _selectedAccount.UserAccountId).ToList();
            //Check if there is a transaction
            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", false);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Description", "Amount" + AppScreen.cur);
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
                    var internalTransfer = AppScreen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;

                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;

                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM Card");
                    Run();
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
            Console.WriteLine($"{AppScreen.cur}1000 X {thousandNotes} = {1000 * thousandNotes}");
            Console.WriteLine($"{AppScreen.cur}500 X {fiveHundredNotes} = {500 * fiveHundredNotes}");
            Console.WriteLine($"Total Amount:{Utility.FormatAmount(amt)}\n\n");

            int opt = Validator.Convert<int>("1 to Confirm");
            return opt.Equals(1);
            //return true;
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
            //Check the minimum amount
            if (_selectedAccount.AccountBalance - internalTransfer.TransferAmount < _minimumKeptAccount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of {Utility.FormatAmount(_minimumKeptAccount)} ", false);
                return;
            }

            //Checking Receiver's bank account is valid
            var selectedBankAccountReceiver = _userAccounts.Get()
                .Where(ua => ua.AccountNumber == internalTransfer.RecipientBankAccountNumber)
                .FirstOrDefault();
            //InternalTransferForm();
            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer Failed. Receiver bank account is invalid.", false);
                return;
            }

            //Check Receiver Name
            if (selectedBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recepient's bank account name is invalid.", false);
                return;
            }

            //Add transaction to transactions record - sender
            InsertTransaction(_selectedAccount.UserAccountId, TransactionType.Transfer, internalTransfer.TransferAmount, $"Transferred Amount to {selectedBankAccountReceiver.FullName} {selectedBankAccountReceiver.AccountNumber}");

            //Update Sender's account balance
            _selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //Add Transaction to transactions record - receiver
            InsertTransaction(selectedBankAccountReceiver.UserAccountId, TransactionType.Transfer, internalTransfer.TransferAmount, $"Received Amount from {_selectedAccount.FullName} {_selectedAccount.AccountNumber}");

            //Update Receiver's account balance
            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;

            //Prints success message
            Utility.PrintMessage($"You have successfuly transferred {Utility.FormatAmount(internalTransfer.TransferAmount)} to {internalTransfer.RecipientBankAccountName}", true);
        }
    }
}
