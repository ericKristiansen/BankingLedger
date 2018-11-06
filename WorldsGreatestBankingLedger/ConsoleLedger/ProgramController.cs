
using ConsoleStrReadWrite;
using ConsoleLedger.Controllers;

namespace ConsoleLedger
{
    /// <summary>
    /// Coordinate the program outside of the static main entry class.
    /// </summary>
    public class ProgramController
    {
        private const string WelcomeMessage = "--------- Welcome to ---------\n     The World's Greatest Bank Ledger Program!!!";
        private const string ExitMessage = "--------- Thank You ---------\n         Come Again";

        private readonly LoginController _lc = new LoginController();

        public ProgramController()
        {
            SeedTestData();
        }

        /// <summary>
        /// Display a welcome message, get the user's login data, and
        /// begin the main menu process.
        /// </summary>
        public void StartProgram()
        {
            var srw = new StrReadWrite();
            srw.WriteString(WelcomeMessage);

            if (_lc.UserIsLoggedIn())
            {
                var mmc = new MainMenuController(_lc.GetCurrentUser());
                mmc.StartMenu();
            }
            else
            {
                srw.WriteString(ExitMessage);
            }
        }

        #region Seed Test Data
        /// <summary>
        /// Seed a user for testing purposes.
        /// </summary>
        public void SeedTestData()
        {
            _lc.SeedUser("test", "test");
        }
        #endregion Seed Test Data
    }


}