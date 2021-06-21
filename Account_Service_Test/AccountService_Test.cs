using NUnit.Framework;
using Account_Service.Services;
using Account_Service.Entities;
using Account_Service.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Account_Service_Test
{
    public class AccountService_Test
    {
        private DbContextOptions<AccountDbContext> dbContextOptions;
        private AccountService accountservice;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<AccountDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
           
            SeedDb();

            accountservice = new AccountService(new AccountDbContext(dbContextOptions));
        }

        private void SeedDb()
        {
            using var context = new AccountDbContext(dbContextOptions);
            Account account = new Account
            {
                email = "testAccount1@test.nl",
                password = "TestPassword"
            };
            context.Account.Add(account);

            context.SaveChanges();
        }

        [Test]
        public void Create_Account()
        {
            Account account = new Account
            {
                email = "test@test.nl",
                password = "TestPassword"
            };

            int id = accountservice.create(account);

            using var context = new AccountDbContext(dbContextOptions);
            bool accountCreated = context.Account.Any(a => a.ID == id);

            Assert.IsTrue(accountCreated);
        }

        [Test]
        public void Get_By_ID()
        {
            string expectedEmail = "testAccount1@test.nl";
            int id = 1;

            Account account = accountservice.GetById(id);

            Assert.AreEqual(expectedEmail, account.email);
        }

        [Test]
        public void Get_By_ID_Fail()
        {
            int id = 10;

            Account account = accountservice.GetById(id);

            Assert.IsNull(account);
        }

        [Test]
        public void Check_Email_Success()
        {
            string email = "testAccount1@test.nl";

            using var context = new AccountDbContext(dbContextOptions);
            var allusers = context.Account.ToList();

            bool actualResult = accountservice.checkEmail(email);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void Check_Email_Fail()
        {
            string email = "NotExisting@test.nl";

            bool actualResult = accountservice.checkEmail(email);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Delete_Account_Success()
        {
            using var context = new AccountDbContext(dbContextOptions);
            var account = context.Account.FirstOrDefault(account => account.ID == 1);

            bool actualResult = accountservice.delete(account);

            bool deletedResult = context.Account.Any(a => a.ID == 1);

            Assert.IsTrue(actualResult);
            Assert.IsFalse(deletedResult);
        }

        [Test]
        public void Delete_Account_Fail()
        {
            using var context = new AccountDbContext(dbContextOptions);

            Account account = new Account
            {
                ID = 1,
                email = "updatedEmail@test.nl",
                password = "WrongPassword"
            };

            bool actualResult = accountservice.delete(account);
            bool deletedResult = context.Account.Any(a => a.ID == 1);

            Assert.IsFalse(actualResult);
            Assert.IsTrue(deletedResult);
        }

        [Test]
        public void Update_Account_Success()
        {
            Account account = new Account
            {
                ID = 1,
                email = "updatedEmail@test.nl",
                password = "TestPassword"
            };

            bool actualResult = accountservice.update(account);

            using var context = new AccountDbContext(dbContextOptions);
            var updatedAccount = context.Account.FirstOrDefault(account => account.ID == 1);

            Assert.IsTrue(actualResult);
            Assert.AreEqual(account.email, updatedAccount.email);
        }
    }
}
