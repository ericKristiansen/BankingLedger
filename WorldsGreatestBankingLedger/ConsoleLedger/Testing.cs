using ConsoleLedger.Controllers;
using NUnit.Framework;

namespace ConsoleLedger
{
    [TestFixture]
    class Testing
    {
        [TestCase]
        public void VerifyCredentials()
        {
            var lc = new LoginController();
            lc.SeedUser("test", "test");

            Assert.AreEqual(lc.VerifyCredentials("test", "test"), true);
            Assert.AreNotEqual(lc.VerifyCredentials("test", "Test"), true);
            Assert.AreNotEqual(lc.VerifyCredentials("Test", "test"), true);
        }


        //remove user

        //add account

        //remove account

    }
}
