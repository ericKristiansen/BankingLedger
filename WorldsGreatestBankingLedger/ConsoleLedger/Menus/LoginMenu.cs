
using System.Text;

namespace ConsoleLedger.Menus
{
    /// <summary>
    /// Display a login menu, and return the user's selection.
    /// Validation is done to ensure that the user selected from
    /// the existing options.
    /// </summary>
    class LoginMenu : Menu
    {
        enum MenuChoice
        {
            LOGIN = 1,
            REGISTER,
            EXIT
        }

        public LoginMenu()
        {
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            var sb = new StringBuilder();
            sb.AppendLine("-------- Login or Register --------");
            sb.AppendLine("Please, enter a number from the menu below:");
            sb.AppendLine("1) Login");
            sb.AppendLine("2) Register");
            sb.AppendLine("3) Exit Program");
            Display(sb.ToString());
        }

        internal string GetMenuChoice()
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
