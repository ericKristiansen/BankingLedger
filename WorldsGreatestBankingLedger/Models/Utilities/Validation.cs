

namespace Models.Utilities
{
    public class Validation
    {
        public static int DepositMax => 100000;
        public static int WithdrawalMax => 10000;
        public static int DepositMin => 0;
        public static int WithdrawalMin => 0;
        public const string DepositSuccess = "Thank you for your deposit of {0}.";
        public const string WithdrawalSuccess = "Thank you for your withdrawal of {0}.";
    }
}
