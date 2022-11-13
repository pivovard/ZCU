using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Bank.Models
{
    /// <summary>
    /// Store information about user/admin
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int BirthNumber { get; set; }
        public string Adress { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int? Phone { get; set; }
        
        public long? AccountNumber { get; set; }
        public long? CardNumber { get; set; }
        public double? Money { get; set; }

        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Pin { get; set; }
        [Required]
        public Role Role { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Template> Templates { get; set; }
        

        /// <summary>
        /// Returns hash of given pin
        /// </summary>
        /// <param name="pin">Pin to hash</param>
        /// <returns>Hash</returns>
        public string HashPin(int pin)
        {
            byte[] hash;
            using (var sha = new SHA256Managed())
            {
                hash = sha.ComputeHash(BitConverter.GetBytes(pin));
            }
            return BitConverter.ToString(hash);
        }

    }
}
