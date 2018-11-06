
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorldsGreatestBankLedgerWebApp.Data
{
    public class LoginData
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
