using Bank.Handlers;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Filters
{
    /// <summary>
    /// Filter if user is logged
    /// Sets navbar to user/admin
    /// </summary>
    public class AuthorizationFilter : IActionFilter
    {
        public int Order => int.MinValue;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Controller controller = context.Controller as Controller;
            User u = null;

            string userId = context.HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userId) && SessionHandler.GetUser(userId, out u))
            {
                controller.ViewBag.UserRole = u.Role;
            }
            else
            {
                controller.ViewBag.UserRole = null;
            }
        }
    }
}
