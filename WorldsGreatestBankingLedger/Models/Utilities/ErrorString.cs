
namespace Models.Utilities
{
    public static class ErrorString
    {
        public static string LoginAttempt => "Error: Failed Login Attempt...";
        public static string PasswordConfirmation => "Error: Passwords do not match...";
        public static string RegistrationUserExists => "Error: Registration Failed - user exists";
        public static string DepositEntry => "Error: Your entry must be a number less than {0} and more than {1}.";
        public static string WithdrawalEntry => "Error: Your entry must be a number less than {0} and more than {1}.";
        public static string EmptyFields => "Error: One or more fields of information were left empty.";
    }
}
