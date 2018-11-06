
using Models.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using System.Text;
using Models.Interfaces;
using Models.Utilities;

namespace Models.Services
{
    public class DataService : IDataService
    {
        private readonly UserRepo _userRepo = new UserRepo();
        private readonly AccountRepo _accountRepo = new AccountRepo();
        private readonly ApplicationDataRepo _appDataRepo = new ApplicationDataRepo();
        private readonly HistoryRepo _historyRepo = new HistoryRepo();

        private const string Separator = " - ";
        private const string SingleSpace = " ";
        private const string Withdrawal = "Withdrawal";
        private const string Deposit = "Deposit";
        private const string Balance = "Balance Inquiry";
        private const string Creation = "Account Creation";

        /// <summary>
        /// Return true if the user is unique, and the credentials are valid.
        /// Also, this method stores the user into memory as the current user.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        bool IDataService.VerifyUserCredentials(string name, string passwordHash)
        {
            var success = false;

            if (_userRepo.UsersExist())
            {
                var users = _userRepo.GetAll();
                var user = users.SingleOrDefault(x => x.UserName.Equals(name) && x.IsMatchPassword(passwordHash));
                var userExists = user != null;

                if (userExists)
                {
                    AddCurrentUser(user);
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// Add an account for the current user.
        /// </summary>
        /// <returns></returns>
        bool IDataService.AddAccount()
        {
            var u = _appDataRepo.GetCurrentUser();
            return _accountRepo.AddAccount(new Account(u.UserId));
        }

        /// <summary>
        /// Add a user.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passHash"></param>
        /// <returns></returns>
        bool IDataService.AddUser(string name, string passHash)
        {
            var success = true;
            try
            {
                success = !_userRepo.UsersExist() || IsUserUnique(_userRepo.GetAll(), name);

                if (success)
                {
                    var userNumber = _appDataRepo.GetNextUserNumber();

                    _accountRepo.AddAccount(new Account(userNumber));
                    AddHistory(userNumber, Creation);
                    _userRepo.AddUser(new User(userNumber, name, passHash));
                }
            }
            catch { success = false; }
            return success;
        }

        /// <summary>
        /// Return the current user information.
        /// </summary>
        /// <returns></returns>
        User IDataService.GetCurrentUser()
        {
            return _appDataRepo.GetCurrentUser();
        }

        /// <summary>
        /// Add the given user as the current user.
        /// </summary>
        /// <param name="u"></param>
        private void AddCurrentUser(User u)
        {
            _appDataRepo.AddCurrentUser(u);
        }

        /// <summary>
        /// Return true if the user name is not in the collection of registered users.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static bool IsUserUnique(IEnumerable<User> users, string userName)
        {
            var user = users.SingleOrDefault(x => x.UserName.Equals(userName));
            return user == null;
        }

        /// <summary>
        /// Deposit the given amount into the current user's account.
        /// </summary>
        /// <param name="d"></param>
        void IDataService.Deposit(decimal d)
        {
            var a = _accountRepo.GetAccount(_appDataRepo.GetCurrentUser().UserId);
            a.Balance = a.Balance + d;
            _accountRepo.PutBack(a);
            AddHistory(a.AccountNumber, Deposit, d);
        }

        /// <summary>
        /// Return a string consisting of the current user's balance information.
        /// </summary>
        /// <returns></returns>
        string IDataService.GetBalance()
        {
            ulong accountNumber = _appDataRepo.GetCurrentUser().UserId;
            AddHistory(accountNumber, Balance);
            return _accountRepo.GetAccount(accountNumber).ToString();
        }

        /// <summary>
        /// Withdraw the given amount from the current user's account.
        /// </summary>
        /// <param name="d"></param>
        void IDataService.Withdrawal(decimal d)
        {
            var a = _accountRepo.GetAccount(_appDataRepo.GetCurrentUser().UserId);
            a.Balance = a.Balance - d;
            _accountRepo.PutBack(a);
            AddHistory(a.AccountNumber, Withdrawal, d);
        }

        /// <summary>
        /// Add a transaction to the history of the current user's account.
        /// </summary>
        /// <param name="accNumber"></param>
        /// <param name="action"></param>
        /// <param name="d"></param>
        private void AddHistory(ulong accNumber, string action, decimal d = -1)
        {
            var history = d > -1 ? ConcatenateStrings(GetHistoryString(action), SingleSpace, 
                    d.ToString(FormatString.CurrencyFormat)) :
                GetHistoryString(action);

            _historyRepo.AddHistory(accNumber, history);
        }

        /// <summary>
        /// Return the part of the history transaction string common to all actions.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static string GetHistoryString(string action)
        {
            return ConcatenateStrings(
                DateTime.Now.ToString(FormatString.DateTimeStringFormat,
                    CultureInfo.CreateSpecificCulture(FormatString.DateTimeCulture)) , Separator, action );
        }

        /// <summary>
        /// Return a constructed string from the array of string objects
        /// to eliminate some string concatenation.
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        private static string ConcatenateStrings(params string[] strArray)
        {
            var sb = new StringBuilder();
            Array.ForEach(strArray, s => sb.Append(s));
            return sb.ToString();
        }

        /// <summary>
        /// Return the collection of transactions for the current user.
        /// </summary>
        /// <returns></returns>
        List<string> IDataService.GetHistory()
        {
            return _historyRepo.GetHistory(_appDataRepo.GetCurrentUser().UserId);
        }

        /// <summary>
        /// Remove the current user from memory.
        /// </summary>
        void IDataService.RemoveCurrentUser()
        {
            _appDataRepo.RemoveCurrentUser();
        }
    }
}
