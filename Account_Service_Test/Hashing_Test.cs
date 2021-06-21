using NUnit.Framework;
using Account_Service.Services;

namespace Account_Service_Test
{
    public class Hashing_Test
    {
        Hashing hashing;

        [SetUp]
        public void Setup()
        {
            hashing = new Hashing();
        }

        [Test]
        public void Hash_Password_Success()
        {
            string password = "TestPassword";
            string expectedHash = "7bcf9d89298f1bfae16fa02ed6b61908fd2fa8de45dd8e2153a3c47300765328";

            string actualHash = hashing.ComputeSha256Hash(password);


            Assert.AreEqual(expectedHash, actualHash);
        }

        [Test]
        public void Hash_Password_Fail()
        {
            string password = "TestPassword123";
            string expectedHash = "7bcf9d89298f1bfae16fa02ed6b61908fd2fa8de45dd8e2153a3c47300765328";

            string actualHash = hashing.ComputeSha256Hash(password);


            Assert.AreNotEqual(expectedHash, actualHash);
        }
    }
}