
using ConsoleStrReadWrite;

namespace ConsoleLedger.Menus
{
    /// <summary>
    /// Provide some base functionality for the two menus used in the
    /// console application.
    /// </summary>
    internal class Menu
    {
        private const string SelectionError = "Error: Please make a valid selection (Example: 2<ENTER>).";

        private readonly StrReadWrite _srw = new StrReadWrite();
        protected void Display(string str)
        {
            _srw.ClearConsole();
            _srw.WriteString(str);
        }

        protected void DisplaySelectionError()
        {
            _srw.DisplayMessageWithDelay(SelectionError);
        }

        protected string GetResponse()
        {
            return _srw.ReadString();
        }
    }
}