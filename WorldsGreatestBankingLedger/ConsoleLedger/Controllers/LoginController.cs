
using ConsoleLedger.Menus;
using ConsoleStrReadWrite;
using Models;
using Models.Data;
using Models.Services;
using Models.Utilities;
using ConsoleLedger.Utilities;

namespace ConsoleLedger.Controllers
{
    /// <summary>
    /// Coordinate the data and logic of the login processes.
    /// </summary>
    class LoginController
    {
        private const string Exit = "EXIT";
        private const string Login = "LOGIN";
        private const string Register = "REGISTER";
        private const string RegisterMessage = "Registering User...";

        private string _name = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;

        private readonly LoginModel _lm = new LoginModel(new DataService());
        private readonly ApplicationDataModel _adm = new ApplicationDataModel(new DataService());
        private readonly StrReadWrite _srw = new StrReadWrite();

        /// <summary>
        /// This method is currently called from the program controller class, and returns true
        /// after the user successfully logs in.
        /// </summary>
        /// <returns></returns>
        internal bool UserIsLoggedIn()
        {
            var userLoggedIn = false;
            string loginAction;

            do
            {
                loginAction = GetLoginAction();

                switch (loginAction)
                {
                    case Login:
                        {
                            userLoggedIn = LoginUser();
                            break;
                        }
                    case Register:
                        {
                            RegisterUser();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

            } while (!loginAction.Equals(Exit) && !userLoggedIn);

            return userLoggedIn;
        }

        /// <summary>
        /// Return the current user.
        /// </summary>
        /// <returns></returns>
        internal User GetCurrentUser()
        {
            return _adm.GetCurrentUser();
        }

        /// <summary>
        /// Return true if user is able to login. Display an error otherwise.
        /// </summary>
        /// <returns></returns>
        private bool LoginUser()
        {
            GetLoginCredentials();
            var success = VerifyCredentials(_name, _password);

            if (!success)
            {
                _srw.DisplayMessageWithDelay(ErrorString.LoginAttempt);
            }

            return success;
        }

        /// <summary>
        /// Return true if the user is able to register. Display an error
        /// if the user's name is a duplicate. Display a success message
        /// if the registration validation checks out.
        /// </summary>
        /// <returns></returns>
        private bool RegisterUser()
        {
            GetRegistration();
            var success = VerifyFieldsNotNull() && 
                          VerifyRegistration(_password, _confirmPassword);

            if (!success)
            {
                _srw.DisplayMessageWithDelay(ErrorString.PasswordConfirmation);
            }
            else
            {
                _srw.DisplayMessageWithDelay(
                    SaveRegistration() ? RegisterMessage : 
                    ErrorString.RegistrationUserExists);
            }
            return success;
        }

        /// <summary>
        /// Handle empty fields.
        /// </summary>
        /// <returns></returns>
        private bool VerifyFieldsNotNull()
        {
            var success =  !string.IsNullOrEmpty(_name) &&
                           !string.IsNullOrEmpty(_password) &&
                           !string.IsNullOrEmpty(_confirmPassword);
            if (!success)
            {
                _srw.DisplayMessageWithDelay(ErrorString.EmptyFields);
            }

            return success;
        }

        /// <summary>
        /// Return true if user entered the same password twice to confirm.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        internal bool VerifyRegistration(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }

        /// <summary>
        /// Seed a user for test purposes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        internal void SeedUser(string name, string pass)
        {
            _lm.RegisterUser(name, pass);
        }

        /// <summary>
        /// Get the registration information from the user.
        /// </summary>
        private void GetRegistration()
        {
            _srw.ClearConsole();
            _srw.WriteString(Prompt.Register);

            _name = GetNameFromUser();
            _password = GetPasswordFromUser();
            _confirmPassword = GetPasswordConfirmationFromUser();
        }
        
        /// <summary>
        /// Save the registration information.
        /// </summary>
        /// <returns></returns>
        private bool SaveRegistration()
        {
            return _lm.RegisterUser(_name, _password);
        }

        /// <summary>
        /// Get the users login menu choice.
        /// </summary>
        /// <returns></returns>
        private static string GetLoginAction()
        {
            var loginMenu = new LoginMenu();
            return loginMenu.GetMenuChoice();
        }

        /// <summary>
        /// Get the login credentials from the user.
        /// </summary>
        private void GetLoginCredentials()
        {
            _srw.ClearConsole();
            _srw.WriteString(Prompt.Login);

            _name = GetNameFromUser();
            _password = GetPasswordFromUser();

        }

        /// <summary>
        /// Get the name from the user.
        /// </summary>
        /// <returns></returns>
        private string GetNameFromUser()
        {
            _srw.WriteString(Prompt.EnterUserName);
            return _srw.ReadString();
        }

        /// <summary>
        /// Get the password from the user.
        /// </summary>
        /// <returns></returns>
        private string GetPasswordFromUser()
        {
            _srw.WriteString(Prompt.EnterPassword);
            return _srw.ReadString();
        }

        /// <summary>
        /// Get the password confirmation from the user.
        /// </summary>
        /// <returns></returns>
        private string GetPasswordConfirmationFromUser()
        {
            _srw.WriteString(Prompt.ConfirmPassword);
            return _srw.ReadString();
        }

        /// <summary>
        /// Return true if the user's credentials match a set of credentials
        /// in the user collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool VerifyCredentials(string name, string password)
        {
            return _lm.VerifyCredentials(name, password);
        }

    }
}
