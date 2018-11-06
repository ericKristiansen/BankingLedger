using System.Collections.Generic;
using Models.Data;

namespace Models.Interfaces
{
    /// <summary>
    /// This is just a contract for the data service.
    /// This might be better factored out.
    /// </summary>
    public interface IDataService
    {
        bool VerifyUserCredentials(string name, string passwordHash);

        bool AddUser(string name, string passHash);

        bool AddAccount();

        User GetCurrentUser();

        void Deposit(decimal d);

        string GetBalance();

        void Withdrawal(decimal d);

        List<string> GetHistory();

        void RemoveCurrentUser();
    }
}
