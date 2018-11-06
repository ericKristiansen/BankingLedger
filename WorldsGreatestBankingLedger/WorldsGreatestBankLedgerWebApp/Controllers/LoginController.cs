
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WorldsGreatestBankLedgerWebApp.Data;
using Models;
using Models.Services;
using Models.Utilities;
using WorldsGreatestBankLedgerWebApp.Utilities;

namespace WorldsGreatestBankLedgerWebApp.Controllers
{
    /// <summary>
    /// Handle the functionality of the login and registration.
    /// </summary>
    public class LoginController : Controller
    {
        private readonly List<string> _errors = new List<string>();
        private readonly LoginModel _lm = new LoginModel(new DataService());

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Validate the user's entry is not null, and the password matches.
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authorize(LoginData ld)
        {
            var success = false;
            var view = View(LocationString.HomeFull);

            if (ValidateStrings(ld.UserName, ld.Password))
            {
                success = VerifyCredentials(ld);
            }

            if (!success)
            {
                ViewBag.ErrorMessages = _errors;
                view = View(LocationString.LoginError);
            }

            return view;
        }

        /// <summary>
        /// Validate that the entry strings are not null, and the
        /// password and confirmation password match. Then, register
        /// the user, or display an error message.
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(LoginData ld)
        {
            var view = View(LocationString.Index);
            ValidateRegistration(ld);
            if (_errors.Any())
            {
                ViewBag.ErrorMessages = _errors;
                view = View(LocationString.LoginError);
            }
            else
            {
                _lm.RegisterUser(ld.UserName, ld.Password);
            }
            return view;
        }

        /// <summary>
        /// Ensure that the fields are not empty.
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        private bool ValidateRegistration(LoginData ld)
        {
            var success = ValidateStrings(ld.UserName, ld.Password, ld.ConfirmPassword);
            if (!success)
            {
                _errors.Add(ErrorString.EmptyFields);
            }
            return success && PasswordConfirmed(ld);
        }

        /// <summary>
        /// Ensure that the credential set matches.
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        private bool VerifyCredentials(LoginData ld)
        {
            var success = _lm.VerifyCredentials(ld.UserName, ld.Password);
            if (!success)
            {
                _errors.Add(ErrorString.LoginAttempt);
            }
            return success;
        }

        /// <summary>
        /// Ensure that the password and confirmation match.
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        private bool PasswordConfirmed(LoginData ld)
        {
            var success = true;
            if(!ld.Password.Equals(ld.ConfirmPassword))
            {
                _errors.Add(ErrorString.PasswordConfirmation);
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Validate that all strings are not null or empty.
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        private bool ValidateStrings(params string [] strArray)
        {
            var result = true;
            if (strArray.Any(string.IsNullOrEmpty))
            {
                result = false;
                _errors.Add(ErrorString.EmptyFields);
            }
            return result;
        }
    }
}