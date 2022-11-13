using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Filters
{
    public class UserFilterAttribute : TypeFilterAttribute
    {
        public UserFilterAttribute() : base(typeof(UserFilter)) { }
    }
}
