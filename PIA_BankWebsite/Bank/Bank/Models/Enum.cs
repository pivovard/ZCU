using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    /// <summary>
    /// List of frequencies for standing payments
    /// </summary>
    public enum Frequency
    {
        Day, Week, Month, Year
    }

    /// <summary>
    /// List of user roles
    /// </summary>
    public enum Role
    {
        User, Admin
    }
}
