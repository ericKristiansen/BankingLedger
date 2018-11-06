
using System.Runtime.Caching;
using Models.Data;

namespace Models.Services
{
    /// <summary>
    /// This class is used to maintain application wide data for quick reference.
    /// </summary>
    class ApplicationDataRepo
    {
        private readonly string CurrentUser = "current_user";
        private readonly string NextUserNumber = "next_user_number";

        /// <summary>
        /// Return the next number to associate with a new user.
        /// </summary>
        /// <returns></returns>
        public ulong GetNextUserNumber()
        {
            return GetNextNumber(NextUserNumber);
        }

        /// <summary>
        /// Return the current user information.
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            return MemoryCache.Default.Get(CurrentUser) as User;
        }

        /// <summary>
        /// Add a user as the current user.
        /// </summary>
        /// <param name="u"></param>
        public void AddCurrentUser(User u)
        {
            if (CurrentUserExists())
            {
                MemoryCache.Default.Remove(CurrentUser);
            }
            MemoryCache.Default.Add(new CacheItem(CurrentUser, u), new CacheItemPolicy());

        }

        /// <summary>
        /// Return true if a current user exists.
        /// </summary>
        /// <returns></returns>
        public bool CurrentUserExists()
        {
            return MemoryCache.Default.Contains(CurrentUser);
        }

        /// <summary>
        /// Get the next number, and increment for additional users.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private ulong GetNextNumber(string key)
        {
            ulong result = 1;
            var numberExists = MemoryCache.Default.Contains(key);
            if (numberExists)
            {
                result = (ulong)MemoryCache.Default.Get(key);
                MemoryCache.Default.Remove(key);
            }

            var next = result + 1;
            MemoryCache.Default.Set(new CacheItem(key, next), new CacheItemPolicy());

            return result;
        }

        /// <summary>
        /// Remove the current user.
        /// </summary>
        internal void RemoveCurrentUser()
        {
            if (MemoryCache.Default.Contains(CurrentUser))
            {
                MemoryCache.Default.Remove(CurrentUser);
            }
        }
    }
}
