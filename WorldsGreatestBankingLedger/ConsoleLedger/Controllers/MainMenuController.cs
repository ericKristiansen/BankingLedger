
using System.Collections.Generic;
using ConsoleLedger.Menus;
using ConsoleStrReadWrite;
using Models;
using Models.Data;
using Models.Services;
using ConsoleLedger.Utilities;
using Models.Utilities;

namespace ConsoleLedger.Controllers
{
    /// <summary>
    /// Coordinate the logic and data for the main menu processes.
    /// </summary>
    class MainMenuController
    {
        private const string Exit = "EXIT";
        private const string Withdrawal = "WITHDRAWAL";
        private const string Deposit = "DEPOSIT";
        private const string Balance = "BALANCE";
        private const string History = "HISTORY";
        private const int BalanceDelay = 4000;
        private const int EchoDelay = 500;

        private readonly StrReadWrite _srw = new StrReadWrite();
        private readonly AccountModel _am = new AccountModel(new DataService());
        private readonly User _currentUser;

        public MainMenuController(User currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// Loop the main menu for a valid selection, and execute the user's
        /// action choice.
        /// </summary>
        internal void StartMenu()
        {
            const string userSelection = "User Selection: ";
            string mainAction;
            do
            {
                mainAction = GetMainMenuAction();
                _srw.DisplayMessageWithDelay(userSelection + mainAction, EchoDelay);

                switch (mainAction)
                {
                    case Deposit:
                        {
                            decimal d = GetDeposit();
                            if (d > Validation.DepositMin && d < Validation.DepositMax)
                            {
                                _am.Deposit(d);
                            }
                            break;
                        }
                    case Withdrawal:
                        {
                            decimal d = GetWithdrawal();
                            if (d > Validation.WithdrawalMax && d < Validation.WithdrawalMax)
                            {
                                _am.Withdrawal(d);
                            }
                            break;
                        }
                    case Balance:
                        {
                            DisplayBalance();
                            break;
                        }
                    case History:
                        {
                            var history = GetHistory();
                            foreach (var s in history)
                            {
                                _srw.WriteString(s);
                            }

                            _srw.WriteString(Prompt.EnterKeyMainMenu);
                            _srw.PauseForKey();

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            } while (!mainAction.Equals(Exit));
        }

        /// <summary>
        /// Return the list of transaction history for the current user.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetHistory()
        {
            return _am.GetHistory();
        }

        /// <summary>
        /// Get the withdrawal information from the user.
        /// </summary>
        /// <returns></returns>
        private decimal GetWithdrawal()
        {
            _srw.WriteString(string.Format(Prompt.EnterWithdrawal, Validation.WithdrawalMax));
            var withdrawal = _srw.ReadString();

            var success = decimal.TryParse(withdrawal, out var d) && d <= Validation.WithdrawalMax && d > Validation.WithdrawalMin;

            var message = success ? string.Format(Validation.WithdrawalSuccess, d.ToString(FormatString.CurrencyFormat)) :
                string.Format(ErrorString.WithdrawalEntry, Validation.WithdrawalMax.ToString(FormatString.CurrencyFormat), 
                    Validation.WithdrawalMin.ToString(FormatString.CurrencyFormat));
            _srw.DisplayMessageWithDelay(message);

            return d;
        }

        /// <summary>
        /// Display the user's account balance.
        /// </summary>
        private void DisplayBalance()
        {
            _srw.DisplayMessageWithDelay(_am.Balance(), BalanceDelay);
        }

        /// <summary>
        /// Get the deposit information from the user.
        /// </summary>
        /// <returns></returns>
        private decimal GetDeposit()
        {
            _srw.WriteString(string.Format(Prompt.EnterDeposit, Validation.DepositMax));
            var deposit = _srw.ReadString();

            var success = decimal.TryParse(deposit, out var d) && d <= Validation.DepositMax && d > Validation.DepositMin;

            var message = success ? string.Format(Validation.DepositSuccess, d.ToString(FormatString.CurrencyFormat)) : 
                string.Format(ErrorString.DepositEntry, Validation.DepositMax.ToString(FormatString.CurrencyFormat), 
                    Validation.DepositMin.ToString(FormatString.CurrencyFormat));
            _srw.DisplayMessageWithDelay(message);

            return d;
        }

        /// <summary>
        /// Get the user's action choice.
        /// </summary>
        /// <returns></returns>
        private string GetMainMenuAction()
        {
            var mainMenu = new MainMenu(_currentUser);
            return mainMenu.GetMenuChoice();
        }
    }
}
