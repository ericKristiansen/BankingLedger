
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Models.Data;

namespace Models.Services
{
    public class UserRepo
    {
        private readonly string Users = "users";

        /// <summary>
        /// Return all users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return MemoryCache.Default.Get(Users) as List<User>;
        }

        /// <summary>
        /// Return a user specified by the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User Get(ulong id)
        {
            return GetAll().Single(x => x.UserId == id);
        }

        /// <summary>
        /// Add a user to the collection of users.
        /// </summary>
        /// <param name="u"></param>
        public void AddUser(User u)
        {
            List<User> users = UsersExist() ? MemoryCache.Default.Remove(Users) as List<User> : new List<User>();
            users.Add(u);
            MemoryCache.Default.Add(new CacheItem(Users, users), new CacheItemPolicy());
        }

        /// <summary>
        /// Return true if any users exist.
        /// </summary>
        /// <returns></returns>
        public bool UsersExist()
        {
            return MemoryCache.Default.Contains(Users);
        }

        /// <summary>
        /// Remove a user from the collection of users.
        /// </summary>
        /// <param name="id"></param>
        public void Remove(ulong id)
        {
            var users = UsersExist() ? MemoryCache.Default.Remove(Users) as List<User> : null;
            users.RemoveAll(x => x.UserId == id);
            MemoryCache.Default.Set(new CacheItem(Users, users), new CacheItemPolicy());
        }

    }
}
