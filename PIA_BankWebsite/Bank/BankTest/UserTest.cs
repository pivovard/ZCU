using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Bank.Models;


namespace BankTest
{
    [TestClass]
    public class UserTest
    {
        static DbContextOptionsBuilder<BankContext> optionsBuilder = new DbContextOptionsBuilder<BankContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankContext-afea5610-d4cd-47d8-8ae0-6e4372c48298;Trusted_Connection=True;MultipleActiveResultSets=true");
        BankContext _context = new BankContext(optionsBuilder.Options);

        static long number = 999999999;
        User user = new User() { Name = "Anicka", BirthNumber = 666, Email = "anicka@trautenberk.cz", Login = "ance", Pin = "anicka", Role = 0, AccountNumber = number, CardNumber= number };

        

        [TestInitialize]
        public async Task TestInitAsync()
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            try
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        

        [TestMethod]
        public async Task TestAddUser()
        {
            Assert.IsTrue(await _context.User.AnyAsync(a => a.Name == "Anicka") );
        }

        [TestMethod]
        public async Task TestEditUser()
        {
            user.Name = "Venca";
            _context.Update(user);
            await _context.SaveChangesAsync();

            Assert.IsTrue(await _context.User.AnyAsync(a => a.Name == "Venca"));
        }

        [TestMethod]
        public async Task TestDeleteUser()
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();

            Assert.IsFalse(await _context.User.AnyAsync(a => a.Name == "Anicka"));
        }

        [TestMethod]
        public async Task TestAccountGen()
        {
            long acc = _context.GenerateAccountNumber();

            Assert.IsFalse(await _context.User.AnyAsync(a => a.AccountNumber == acc));
        }

        [TestMethod]
        public async Task TestCardGen()
        {
            long card = _context.GenerateCardNumber();

            Assert.IsFalse(await _context.User.AnyAsync(a => a.CardNumber == card));
        }

        [TestMethod]
        public async Task TestGenerateLogin()
        {
            User u = new User();
            int pin = _context.GenerateLogin(u);

            Assert.IsFalse(await _context.User.AnyAsync(a => a.Login == u.Login));
            Assert.AreEqual(u.HashPin(pin), u.Pin);
        }
    }


}