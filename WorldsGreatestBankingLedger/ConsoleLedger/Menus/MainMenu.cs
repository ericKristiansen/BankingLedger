
using Models.Data;
using System.Text;

namespace ConsoleLedger.Menus
{
    /// <summary>
    /// Display the main menu options, and get the user's selection.
    /// Validation is done to ensure that the user's selection is
    /// valid.
    /// </summary>
    class MainMenu : Menu
    {
        enum MenuChoice
        {
            WITHDRAWAL = 1,
            DEPOSIT,
            BALANCE,
            HISTORY,
            EXIT
        }

        private readonly User _currentUser;

        public MainMenu(User currentUser)
        {
            _currentUser = currentUser;
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            var sb = new StringBuilder();

            sb.AppendLine(_currentUser.ToString());
            sb.AppendLine("-------- Main Menu --------");
            sb.AppendLine("Please, enter a number from the menu below:");
            sb.AppendLine("1) Withdrawal");
            sb.AppendLine("2) Deposit");
            sb.AppendLine("3) Check balance");
            sb.AppendLine("4) See transaction history");
            sb.AppendLine("5) Log out");
            Display(sb.ToString());
        }

        public string GetMenuChoice()
        {
            var strResult = GetResponse();
            var intResult = -1;

            while (!MenuChoiceValidates(strResult, out intResult))
            {
                DisplaySelectionError();
                DisplayMenu();

                strResult = GetResponse();
            }

            return ((MenuChoice)intResult).ToString();
        }

        private static bool MenuChoiceValidates(string str, out int menuChoice)
        {
            return int.TryParse(str, out menuChoice) && MenuChoiceInRange(menuChoice);
        }

        private static bool MenuChoiceInRange(int choice)
        {
            return choice > 0 && choice <= (int)MenuChoice.EXIT;
        }

    }
}
