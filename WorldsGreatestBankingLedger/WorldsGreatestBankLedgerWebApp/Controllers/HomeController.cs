
using System;
using System.Web.Mvc;
using Models;
using Models.Services;
using WorldsGreatestBankLedgerWebApp.Data;
using Models.Utilities;
using WorldsGreatestBankLedgerWebApp.Utilities;

namespace WorldsGreatestBankLedgerWebApp.Controllers
{
    /// <summary>
    /// Handle the main functionality of the ledger.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly AccountModel _am = new AccountModel(new DataService());
        private readonly ApplicationDataModel _adm = new ApplicationDataModel(new DataService());

        /// <summary>
        /// Redirect to the login if the user is not logged in.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return _adm.GetCurrentUser() == null ? 
                RedirectToAction(LocationString.Index, LocationString.Login) : (ActionResult)View();
        }

        /// <summary>
        /// Remove the user as the current user, and redirect to the login.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            _adm.RemoveCurrentUser();
            return RedirectToAction(LocationString.Index,LocationString.Login);
        }

        /// <summary>
        /// Redirect if the client is not logged in, or get the client details,
        /// and return the withdrawal view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdrawal()
        {
            if (_adm.GetCurrentUser() == null)
            {
                return RedirectToAction(LocationString.Index, LocationString.Login);
            }
            GetClientDetails();
            return View();
        }

        /// <summary>
        /// Redirect to the login if the user is not logged in, or get the client's details
        /// and return the deposit view.
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        public ActionResult Deposit(string deposit)
        {
            if (_adm.GetCurrentUser() == null)
            {
                return RedirectToAction(LocationString.Index, LocationString.Login);
            }
            GetClientDetails();
            return View();
        }

        /// <summary>
        /// Check the user's entry for errors, and process the withdrawal request.
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessWithdrawal(AccountData ad)
        {
            GetClientDetails();
            decimal withdrawal = -1;
            var success = ValidateEntry(ad.Withdrawal, out withdrawal, Validation.WithdrawalMax, Validation.WithdrawalMin);

            ViewBag.Message = success ? string.Format(Validation.WithdrawalSuccess, withdrawal.ToString(FormatString.CurrencyFormat)) :
                string.Format(ErrorString.WithdrawalEntry, Validation.WithdrawalMax.ToString(FormatString.CurrencyFormat), 
                    Validation.WithdrawalMin.ToString(FormatString.CurrencyFormat));

            if (success)
            {
                _am.Withdrawal(withdrawal);
            }

            return View(LocationString.Withdrawal);
        }

        /// <summary>
        /// Check the user's entry for errors, and process the deposit.
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProcessDeposit(AccountData ad)
        {
            GetClientDetails();
            decimal deposit = -1; 
            var success = ValidateEntry(ad.Deposit, out deposit, Validation.DepositMax, Validation.DepositMin);

            ViewBag.Message = success ? string.Format(Validation.DepositSuccess, deposit.ToString(FormatString.CurrencyFormat)) :
                string.Format(ErrorString.DepositEntry, Validation.DepositMax.ToString(FormatString.CurrencyFormat), 
                    Validation.DepositMin.ToString(FormatString.CurrencyFormat));

            if (success)
            {
                _am.Deposit(deposit);
            }

            return View(LocationString.Deposit);
        }

        /// <summary>
        /// Returns true if the entry is a number and within range of the requirements.
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="deposit"></param>
        /// <returns></returns>
        private static bool ValidateEntry(string num, out decimal d, int max,  int min)
        {
            var success = decimal.TryParse(num, out d);

            if (success)
            {
                success = d > min && d < max;
            }
            return success;
        }

        /// <summary>
        /// Redirect to the login if the user is not logged in, or present the balance
        /// view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Balance()
        {
            if (_adm.GetCurrentUser() == null)
            {
                return RedirectToAction(LocationString.Index, LocationString.Login);
            }
            GetClientDetails();
            GetAccountDetails();

            return View();
        }

        /// <summary>
        /// Redirect to the login if the user is not logged in, or present the history view.
        /// </summary>
        /// <returns></returns>
        public ActionResult History()
        {
            if (_adm.GetCurrentUser() == null)
            {
                return RedirectToAction(LocationString.Index, LocationString.Login);
            }

            GetClientDetails();

            return View(_am.GetHistory());
        }

        /// <summary>
        /// Retrieve the user's information.
        /// </summary>
        private void GetClientDetails()
        {
            var u = _adm.GetCurrentUser();
            ViewBag.Client = u.UserName;
            ViewBag.AccountNumber = u.UserId.ToString(FormatString.AccountNumberFormat);
        }

        /// <summary>
        /// Retrieves the details for the balance view.
        /// </summary>
        private void GetAccountDetails()
        {
            var str = _am.Balance();
            var balanceIndex = str.IndexOf("Balance: ", StringComparison.Ordinal);
            ViewBag.AccountName = str.Substring(0, balanceIndex);
            ViewBag.AccountBalance = str.Substring(balanceIndex);
        }
    }
}
