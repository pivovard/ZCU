using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Handlers;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly BankContext _context;

        public AuthorizationController(BankContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Login")] string login, [Bind("Pin")] int pin)
        {
            try
            {
                User user = await _context.User.FirstOrDefaultAsync(u => u.Login == login);

                if (user == null || !user.HashPin(pin).Equals(user.Pin))
                {
                    ViewBag.Login = login;
                    ViewBag.ErrMsg = "Wrong login or pin!";
                    return View();
                }

                if(user.Role == Role.User)
                {
                    //HttpContext.Session.SetString("UserId", SessionHandler.NewSession(user));
                    //return Redirect("/User");
                    
                    int t = TransactionHandler.NewAuth(user);
                    HttpContext.Session.SetInt32("Auth", t);
                    return View("LoginConfirm", user);
                }
                else
                {
                    HttpContext.Session.SetString("UserId", SessionHandler.NewSession(user));
                    return Redirect("/Admin");
                }
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginConfirm(string login, string code)
        {
            var t = HttpContext.Session.GetInt32("Auth");

            if (t == null) RedirectToAction(nameof(Login));

            if (TransactionHandler.IsValid((int)t, code))
            {
                User user = await _context.User.FirstOrDefaultAsync(u => u.Login == login);

                HttpContext.Session.SetString("UserId", SessionHandler.NewSession(user));
                return Redirect("/User");
            }
            else
            {
                ViewBag.ErrMsg = "Wrong confirmation code!";
                return View(login);
            }
        }

        public IActionResult Logout()
        {
            SessionHandler.DestroySession(HttpContext.Session.GetString("UserId"));
            HttpContext.Session.Clear();
            ViewBag.Role = "None";

            return Redirect("/Home/Index");
        }

        public IActionResult Unauth()
        {
            return View();
        }

    }
}