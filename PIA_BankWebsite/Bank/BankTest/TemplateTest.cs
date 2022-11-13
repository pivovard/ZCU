using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Bank.Models;

namespace BankTest
{
    [TestClass]
    class TemplateTest
    {
        static DbContextOptionsBuilder<BankContext> optionsBuilder = new DbContextOptionsBuilder<BankContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankContext-afea5610-d4cd-47d8-8ae0-6e4372c48298;Trusted_Connection=True;MultipleActiveResultSets=true");
        BankContext _context = new BankContext(optionsBuilder.Options);

        static long number = 999999999;
        User user = new User() { Name = "Anicka", BirthNumber = 666, Email = "anicka@trautenberk.cz", Login = "ance", Pin = "anicka", Role = 0, AccountNumber = number, CardNumber = number };

        Template temp = new Template() { Name = "Temp1", DestAccount = number, DestBank = 666, Amount = 1000 };

        [TestInitialize]
        public async Task TestInitAsync()
        {
            _context.Add(user);
            temp.UserId = user.Id;
            _context.Add(temp);
            await _context.SaveChangesAsync();
        }

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            try
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                _context.Remove(temp);
                await _context.SaveChangesAsync();
            }
            catch { }
        }

        [TestMethod]
        public async Task TestAddTemplate()
        {
            Assert.IsTrue(await _context.Template.AnyAsync(a => a.Name == "Temp1"));
        }

        [TestMethod]
        public async Task TestEditTemplate()
        {
            temp.Name = "Temp2";
            _context.Update(temp);
            await _context.SaveChangesAsync();

            Assert.IsTrue(await _context.Template.AnyAsync(a => a.Name == "Temp2"));
        }

        [TestMethod]
        public async Task TestDeleteTemplate()
        {
            _context.Remove(temp);
            await _context.SaveChangesAsync();

            Assert.IsFalse(await _context.Template.AnyAsync(a => a.Name == "Anicka"));
        }
    }
}
