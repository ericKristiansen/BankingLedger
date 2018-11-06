
using ConsoleLedger.Controllers;
using Models;
using NUnit.Framework;
using Models.Services;

namespace ConsoleLedger
{
    [TestFixture]
    class Testing
    {
        private const string StrTest = "test";

        [TestCase]
        public void VerifyCredentials()
        {
            var lc = new LoginController();
            lc.SeedUser(StrTest, StrTest);

            Assert.AreEqual(lc.VerifyCredentials(StrTest, StrTest), true);
            Assert.AreNotEqual(lc.VerifyCredentials(StrTest, "Test"), true);
            Assert.AreNotEqual(lc.VerifyCredentials("Test", StrTest), true);
            Assert.AreNotEqual(lc.VerifyCredentials(StrTest, string.Empty), true);
            Assert.AreNotEqual(lc.VerifyCredentials(string.Empty, StrTest), true);
        }

        [TestCase]
        public void CurrentUser()
        {
            var lc = new LoginController();
            lc.SeedUser(StrTest, StrTest);
            lc.VerifyCredentials(StrTest, StrTest);

            Assert.AreEqual(lc.GetCurrentUser().UserName.Equals(StrTest), true);

        }

        [TestCase]
        public void CreateHistory()
        {
            var lc = new LoginController();
            lc.SeedUser(StrTest, StrTest);
            lc.VerifyCredentials(StrTest, StrTest);

            var am = new AccountModel(new DataService());
            Assert.AreNotEqual(am.GetHistory(), null);
        }

        [TestCase]
        public void GetBalance()
        {
            var lc = new LoginController();
            lc.SeedUser(StrTest, StrTest);
            lc.VerifyCredentials(StrTest, StrTest);

            var am = new AccountModel(new DataService());
            Assert.AreNotEqual(am.Balance(), null);
        }

    }
}
