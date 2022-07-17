// See https://aka.ms/new-console-template for more information
using AtmConsole.TextUi;
using ConsoleTables;
using AtmConsole.Domain.InternalTransfers;
using AtmConsole.Domain.Authentication;

namespace AtmConsole.App
{
    public class AtmApp
    {
        private const decimal _minimumKeptAccount = 500;
        private readonly ICardAuthentication _cardAuth;
        private readonly IAtmAppService _atm;
        private bool _isRunning;
        private long? _selectedAccountId;
        private readonly IUserAuthentication _userAuth;

        public AtmApp(IUserAuthentication userAuth, ICardAuthentication cardAuth, IAtmAppService atm)
        {
            _userAuth = userAuth;
            _cardAuth = cardAuth;
            _atm = atm;
        }

        public void Run()
        {
            ConsoleUi.Welcome();
            CheckUserNumberAndPassword();
            var account = _atm.GetUserAccount(_selectedAccountId.Value);
            ConsoleUi.WelcomeCustomer(account.FullName);
            while (_isRunning)
            {
                ConsoleUi.DisplayAppMenu();
                ProcessMainOption();
            }
        }

        public void CheckUserNumberAndPassword()
        {
            _selectedAccountId = null;
            Domain.UserAccounts.UserAccount? selected = null;

            do
            {
                var cardNumber = ConsoleUi.AskCardNumber();

                ConsoleUi.LoginProgress();
                var inputAccount = _atm.GetUserAccount(cardNumber);

                // TODO could lock terminal for 30 secs after n failed card numbers
                if (inputAccount == null)
                    continue;

                selected = inputAccount;
            } while (selected == null);

            bool cardVerified;
            do
            {
                if (selected.IsLocked)
                {
                    ConsoleUi.PrintLockedScreen();
                    _isRunning = false;
                    break;
                }

                int pin = ConsoleUi.AskPinNumber();

                // TODO multiple logics depending on user roles and card types (btw separate user and card)
                // for example gold card could withdraw more money than then silver one
                cardVerified = _cardAuth.VerifyCard(selected, pin);
                _userAuth.UpdateUserAuth(selected, cardVerified);

                if (cardVerified == false)
                {
                    Utility.PrintMessage("\nInvalid Card Number or PIN .", false);
                    Console.WriteLine($"Pin check failed {selected.AuthFailsCount} times");
                    break;
                }

                Console.Clear();
            } while (cardVerified == false);
            _selectedAccountId = selected.UserAccountId;
        }

        public void CheckBalance()
        {
            var account = _atm.GetUserAccount(_selectedAccountId.Value);
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(account.AccountBalance)}");
            Utility.PressEnterToContinue();
            ProcessMainOption();
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\n Only multiples of 500 and 1000 Naira allowed");
            var transaction_amt = Validator.Convert<int>($"amount {ConsoleUi.cur}");

            Console.WriteLine("\n Checking and Counting Bank Notes");
            Utility.PrintDotAnimation();
            Console.WriteLine();

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

            _atm.PlaceDeposit(_selectedAccountId.Value, transaction_amt, null);
            Utility.PrintMessage($"Deposit of {transaction_amt} has been submitted successfully", true);
        }

        public void MakeWithdrawal()
        {
            int transaction_amt;
            int? selectedAmount;

            do
            {
                selectedAmount = ConsoleUi.SelectAmount();
                if (selectedAmount == 0)
                {
                    selectedAmount = Validator.Convert<int>($"amount {ConsoleUi.cur}");

                    if (selectedAmount % 500 != 0)
                    {
                        Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000", false);
                    }
                }
            }
            while (selectedAmount == null || selectedAmount <= 0);

            transaction_amt = selectedAmount.Value;

            var account = _atm.GetUserAccount(_selectedAccountId.Value);
            if (account.AccountBalance - transaction_amt < _minimumKeptAccount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of {Utility.FormatAmount(_minimumKeptAccount)} ", false);
                return;
            }

            _atm.Withdraw(_selectedAccountId.Value, transaction_amt, null);
            Utility.PrintMessage($"You have successfully withdrawn {Utility.FormatAmount(transaction_amt)}", true);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _atm.GetTransactions(_selectedAccountId.Value);

            if (!filteredTransactionList.Any())
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
                Utility.PrintMessage($"You have {filteredTransactionList.Count()} transaction(s)", true);
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
            var recipient = _atm.GetUserAccount(internalTransfer.RecipientBankAccountNumber);
            var current = _atm.GetUserAccount(_selectedAccountId.Value);

            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try Again", false);
                return;
            }

            if (internalTransfer.TransferAmount > current.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance to transfer " +
                    $"a balance of {Utility.FormatAmount(internalTransfer.TransferAmount)} ", false);
                return;
            }

            if (current.AccountBalance - internalTransfer.TransferAmount < _minimumKeptAccount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of {Utility.FormatAmount(_minimumKeptAccount)} ", false);
                return;
            }

            // UNDONE
            //if (selectedBankAccountReceiver == null)
            //{
            //    Utility.PrintMessage("Transfer Failed. Receiver bank account is invalid.", false);
            //    return;
            //}

            //if (selectedBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            //{
            //    Utility.PrintMessage("Transfer Failed. Recepient's bank account name is invalid.", false);
            //    return;
            //}

            _atm.Transfer(_selectedAccountId.Value, internalTransfer.RecipientBankAccountNumber, internalTransfer.TransferAmount);

            Utility.PrintMessage($"You have successfuly transferred {Utility.FormatAmount(internalTransfer.TransferAmount)} to {internalTransfer.RecipientBankAccountName}", true);
        }
    }
}
