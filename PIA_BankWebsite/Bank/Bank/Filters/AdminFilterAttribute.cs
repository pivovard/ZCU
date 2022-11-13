using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Filters
{
    public class AdminFilterAttribute : TypeFilterAttribute
    {
        public AdminFilterAttribute() : base(typeof(AdminFilter)) { }
    }
}
