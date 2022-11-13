using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Bank.Models;

namespace BankTest
{
    [TestClass]
    class PaymentTest
    {
        static DbContextOptionsBuilder<BankContext> optionsBuilder = new DbContextOptionsBuilder<BankContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankContext-afea5610-d4cd-47d8-8ae0-6e4372c48298;Trusted_Connection=True;MultipleActiveResultSets=true");
        BankContext _context = new BankContext(optionsBuilder.Options);

        static long number = 999999999;
        User user = new User() { Name = "Anicka", BirthNumber = 666, Email = "anicka@trautenberk.cz", Login = "ance", Pin = "anicka", Role = 0, AccountNumber = number, CardNumber = number };

        Payment pay = new Payment() { FromAccount = 123, DestAccount = number, DestBank = 0100, Amount = 1000 };

        [TestInitialize]
        public async Task TestInitAsync()
        {
            try
            {
                _context.Add(user);
                pay.UserId = user.Id;
                _context.Add(pay);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            try
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                _context.Remove(pay);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        [TestMethod]
        public async Task TestAddPayment()
        {
            Assert.IsTrue(await _context.Payment.AnyAsync(a => a.FromAccount == pay.FromAccount && a.DestAccount == pay.DestAccount));
        }

        [TestMethod]
        public async Task MakePaymentInBank()
        {
            long n = number++;
            User user2 = new User() { Name = "Pepa", BirthNumber = 666, Email = "pepa@trautenberk.cz", Login = "pepa", Pin = "pepa", Role = 0, AccountNumber = n, CardNumber = number };
            _context.Add(user2);
            await _context.SaveChangesAsync();

            double? m1 = user.Money;
            double? m2 = user2.Money;

            pay.DestAccount = n;
            pay.DestBank = 666;
            await _context.MakePayment(user, pay);

            Assert.IsTrue(user.Money == m1 - pay.Amount);
            Assert.IsTrue(user2.Money == m2 + pay.Amount);

            _context.Remove(user2);
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task MakePaymentOutBank()
        {
            double? m = user.Money;

            await _context.MakePayment(user, pay);

            Assert.IsTrue(user.Money == m - pay.Amount);
        }
    }
}
