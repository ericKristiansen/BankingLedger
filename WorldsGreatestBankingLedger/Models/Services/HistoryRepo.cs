
using System.Collections.Generic;
using System.Runtime.Caching;


namespace Models.Services
{
    class HistoryRepo
    {
        private readonly string History = "history";

        /// <summary>
        /// Add a transaction to the account history record collection.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="transaction"></param>
        public void AddHistory(ulong accountNumber, string transaction)
        {
            string key = History + accountNumber;
            List<string> history = HistoryExists(key) ?
                MemoryCache.Default.Remove(key) as List<string> : new List<string>();
            history.Add(transaction);
            MemoryCache.Default.Add(new CacheItem(key, history), new CacheItemPolicy());
        }

        /// <summary>
        /// Return a copy of the transaction record collection for the given account.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public List<string> GetHistory(ulong accountNumber)
        {
            return MemoryCache.Default.Get(History + accountNumber) as List<string>;
        }

        /// <summary>
        /// Return true if a history exists for a given account.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool HistoryExists(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

    }
}
