using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Handlers
{
    /// <summary>
    /// Handle transaction verification
    /// </summary>
    public class TransactionHandler
    {
        private static Dictionary<long, string> transactions = new Dictionary<long, string>();

        private static int counter = 0;
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random r = new Random();

        /// <summary>
        /// Adds new authorization transaction
        /// </summary>
        /// <param name="user">User to login</param>
        /// <returns>Transaction id</returns>
        public static int NewAuth(User user)
        {
            counter++;

            string code = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[r.Next(s.Length)]).ToArray());

            transactions.Add(counter, code);

            MailClient.Singleton.SendAuth(user, code);

            return counter;
        }

        /// <summary>
        /// Adds new payment transaction
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="pay">Payment</param>
        /// <returns>Transaction id</returns>
        public static int NewPayment(User user, Payment pay)
        {
            counter++;

            string code = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[r.Next(s.Length)]).ToArray());

            transactions.Add(counter, code);

            MailClient.Singleton.SendPayment(user, pay, code);

            return counter;
        }

        /// <summary>
        /// Returns if transaction code is valid
        /// </summary>
        /// <param name="t">Transaction id</param>
        /// <param name="code">Transaction code</param>
        /// <returns>True if transaction code is valid, false if not</returns>
        public static bool IsValid(int t, string code)
        {
            if(transactions.ContainsKey(t) && transactions[t].Equals(code))
            {
                transactions.Remove(t);
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
