
using Models.Utilities;

namespace Models.Data
{
    public class Account
    {
        private const string Default = "Default";
        private const string AccountNamePrompt = "Account Name: ";
        private const string BalancePrompt = "\nBalance: ";

        public decimal Balance { get; set; }

        public string Name { get; set; }

        public ulong AccountNumber { get; }

        public Account(ulong accountNumber, string name = Default)
        {
            AccountNumber = accountNumber;
            Name = name;
            Balance = 0;
        }

        public override string ToString()
        {
            return AccountNamePrompt + Name + BalancePrompt + Balance.ToString(FormatString.CurrencyFormat);
        }
    }
}
