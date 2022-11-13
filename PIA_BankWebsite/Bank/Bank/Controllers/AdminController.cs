using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Filters;
using Bank.Handlers;
using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Controllers
{
    /// <summary>
    /// Controller for admin pages
    /// </summary>
    [AdminFilter]
    public class AdminController : Controller
    {
        private readonly BankContext _context;

        public AdminController(BankContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// GET/Admin
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        /// <summary>
        /// GET/Admin/AddUser
        /// </summary>
        /// <returns></returns>
        public IActionResult AddUser()
        {
            return View();
        }

        /// <summary>
        /// POST/Admin/AddUser
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser([Bind("Id,Name,BirthNumber,Adress,Email,Phone,AccountNumber,CardNumber,Money,Login,Pin,Role")] User user, string Turing, [Bind("AccGen")] bool AccGen = false, [Bind("CardGen")] bool CardGen = false)
        {
            if (!Turing.ToLower().Equals("human"))
            {
                ViewBag.ErrTuring = "We don't talk to machines, call a human being.";
                return View(user);
            }

            if (user.Role == Role.User)
            {
                if (AccGen) user.AccountNumber = _context.GenerateAccountNumber();
                if (CardGen) user.CardNumber = _context.GenerateCardNumber();

                if (user.AccountNumber == null)
                {
                    ViewBag.ErrAcc = "Account number is required";
                    return View(user);
                }

                if (!_context.IsAccountUnique((long)user.AccountNumber))
                {
                    ViewBag.ErrAcc = "Account number is not unique.";
                    return View(user);
                }

                if (user.CardNumber == null)
                {
                    ViewBag.ErrAcc = "Card number is required";
                    return View(user);
                }

                if (!_context.IsCardUnique((long)user.CardNumber))
                {
                    ViewBag.ErrCard = "Card number is not unique.";
                    return View(user);
                }

                if (user.Money == null) user.Money = 0;
            }
            
            
            if (ModelState.IsValid)
            {
                int pin = _context.GenerateLogin(user);

                _context.Add(user);
                await _context.SaveChangesAsync();

                MailClient.Singleton.SendAddUser(user, pin);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrMsg = "Values are not valid.";
                return View(user);
            }
        }

        /// <summary>
        /// GET/Admin/EditUser/id
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var user = await _context.User.FindAsync(id);
            if (user == null) return RedirectToAction(nameof(Index));
            
            return View(user);
        }

        /// <summary>
        /// POST/Admin/EditUser/id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id,Name,BirthNumber,Adress,Email,Phone,AccountNumber,CardNumber,Money,Login,Pin,Role")] User user, [Bind("CardGen")] bool CardGen)
        {
            if (id != user.Id) return RedirectToAction(nameof(Index));

            User u = await _context.User.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            if (user.Role == Role.User)
            {
                if (CardGen) user.CardNumber = _context.GenerateCardNumber();

                if (user.CardNumber == null)
                {
                    ViewBag.ErrAcc = "Card number is required";
                    return View(user);
                }

                if (!_context.IsCardUnique((long)user.CardNumber) && user.CardNumber != u.CardNumber)
                {
                    ViewBag.ErrCard = "Card number is not unique.";
                    return View(user);
                }

                if (user.Money == null) user.Money = 0;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return Redirect("/User/UNotFound");
                    }
                    else
                    {
                        return Redirect("/Home/Error");
                    }
                }

                MailClient.Singleton.SendEditUser(user);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrMsg = "Values are not valid.";
                return View(user);
            }
        }

        /// <summary>
        /// GET/Admin/DeleteUser/id
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return RedirectToAction(nameof(Index));

            return View(user);
        }

        /// <summary>
        /// POST/Admin/DeleteUser/id
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            MailClient.Singleton.SendDeleteUser(user);

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}