using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public partial class BankContext
    {
        /// <summary>
        /// Makes a payment.
        /// If destination account is in 666 bank transfers money to destination account
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="pay">Payment</param>
        /// <returns>True if successfull, false if not</returns>
        public async Task<bool> MakePayment(User user, Payment pay)
        {
            if (user.Money >= pay.Amount)
            {
                try
                {
                    pay.UserId = user.Id;
                    pay.FromAccount = (long)user.AccountNumber;
                    user.Money -= pay.Amount;

                    if (pay.DestBank == 666 && AccountExist(pay.DestAccount))
                    {
                        User u = this.User.FirstOrDefault(a => a.AccountNumber == pay.DestAccount);
                        u.Money += pay.Amount;

                        this.Update(u);
                        //await this.SaveChangesAsync();
                    }
                    
                    this.Add(pay);
                    this.Update(user);

                    await this.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns if account exist
        /// </summary>
        /// <param name="acc">Account number</param>
        /// <returns>True if exist, false if not</returns>
        public bool AccountExist(long acc)
        {
            return this.User.Any(a => a.AccountNumber == acc);
        }

        /// <summary>
        /// Returns if name of template is unique for this user
        /// </summary>
        /// <param name="name">Name of template</param>
        /// <param name="uid">User id</param>
        /// <returns>True if unique, false if not</returns>
        public bool IsTemplateNameUnique(string name, int uid)
        {
            return !this.Template.Any(a => a.Name == name && a.UserId == uid);
        }


    }
}
