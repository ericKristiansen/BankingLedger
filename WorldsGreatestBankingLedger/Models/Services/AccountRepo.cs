
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Models.Data;

namespace Models.Services
{
    /// <summary>
    /// Handle how the account information is stored and retrieved from memory.
    /// </summary>
    class AccountRepo
    {
        private readonly string Accounts = "accounts";

        /// <summary>
        /// Return all accounts.
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAll()
        {
            return AccountsExist() ? MemoryCache.Default.Get(Accounts) as List<Account> : null;
        }

        /// <summary>
        /// Return true if any accounts exist.
        /// </summary>
        /// <returns></returns>
        public bool AccountsExist()
        {
            return MemoryCache.Default.Contains(Accounts);
        }

        /// <summary>
        /// Add an account to the collection of accounts.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool AddAccount(Account a)
        {
            List<Account> accounts = AccountsExist() ? MemoryCache.Default.Remove(Accounts) as List<Account> : new List<Account>();
            accounts.Add(a);
            MemoryCache.Default.Add(new CacheItem(Accounts, accounts), new CacheItemPolicy());
            return GetAccount(a.AccountNumber) != null;
        }

        /// <summary>
        /// Return the account specified by the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetAccount(ulong id)
        {
            return GetAll().Single(x => x.AccountNumber == id);
        }

        /// <summary>
        /// Put an account back into the collection after it has been modified.
        /// </summary>
        /// <param name="a"></param>
        internal void PutBack(Account a)
        {
            var accounts = MemoryCache.Default.Remove(Accounts) as List<Account>;

            accounts.RemoveAll(x => x.AccountNumber == a.AccountNumber);
            accounts.Add(a);

            MemoryCache.Default.Add(new CacheItem(Accounts, accounts), new CacheItemPolicy());
        }
    }
}
