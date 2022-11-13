using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public partial class BankContext
    {
        /// <summary>
        /// Generate unique login and hash pin.
        /// Set it to the given user.
        /// Returns pin in no hash.
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Pin in no hash</returns>
        public int GenerateLogin(User user)
        {
            Random r = new Random();
            string login;

            do
            {
                login = r.Next(10000000, 100000000).ToString();
            } while (this.User.Any(a => a.Login == login));

            user.Login = login;

            int pin = r.Next(1000, 10000);
            user.Pin = user.HashPin(pin);

            return pin;
        }

        /// <summary>
        /// Generate unique account number
        /// </summary>
        /// <returns>account number</returns>
        public long GenerateAccountNumber()
        {
            Random r = new Random();
            long n;

            do
            {
                n = r.Next(10000000, 100000000) * 100000000L + r.Next(10000000, 100000000);
            } while (this.User.Any(a => a.AccountNumber == n));

            return n;
        }

        /// <summary>
        /// Generate unique card number
        /// </summary>
        /// <returns>card number</returns>
        public long GenerateCardNumber()
        {
            Random r = new Random();
            long n;

            do
            {
                n = r.Next(10000000, 100000000) * 100000000L + r.Next(10000000, 100000000);
            } while (this.User.Any(a => a.CardNumber == n));

            return n;
        }

        /// <summary>
        /// Returns if account number is unique
        /// </summary>
        /// <param name="acc">account number</param>
        /// <returns>True if unique, false if not</returns>
        public bool IsAccountUnique(long acc)
        {
            return !this.User.Any(a => a.AccountNumber == acc);
        }

        /// <summary>
        /// Returns if card number is unique
        /// </summary>
        /// <param name="acc">card number</param>
        /// <returns>True if unique, false if not</returns>
        public bool IsCardUnique(long card)
        {
            return !this.User.Any(a => a.CardNumber == card);
        }
    }
}
