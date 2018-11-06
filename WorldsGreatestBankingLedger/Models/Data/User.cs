
using Models.Utilities;

namespace Models.Data
{
    public class User
    {
        private const string ClientPrompt = "Client: ";
        private const string AccountPrompt = "\nAccount: ";
        private readonly string _passwordHash;

        public ulong UserId { get; set; }

        public string UserName { get; }

        public User(ulong userId, string name, string passHash)
        {
            UserId = userId;
            UserName = name;
            _passwordHash = passHash;
        }

        public bool IsMatchPassword(string passHash)
        {
            return passHash.Equals(_passwordHash);
        }

        public override string ToString()
        {
            return ClientPrompt + UserName + AccountPrompt + UserId.ToString(FormatString.AccountNumberFormat);
        }

    }
}
