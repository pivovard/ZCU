using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Bank.Handlers;
using Bank.Filters;

namespace Bank.Controllers
{
    /// <summary>
    /// Controller for payment operations pages
    /// </summary>
    [UserFilter]
    public class PaymentController : Controller
    {
        private readonly BankContext _context;

        public PaymentController(BankContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET/Payment
        /// 
        /// Redirect to payment
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Payment));
        }


        /// <summary>
        /// GET/Payment/Payment
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Payment()
        {
            ViewBag.Templates = await GetTemplates();
            ViewBag.BankCodes = DataHandler.GetBankCodes();

            return View();
        }

        /// <summary>
        /// POST/Payment/Payment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(Payment payment)
        {
            ViewBag.Templates = await GetTemplates();
            ViewBag.BankCodes = DataHandler.GetBankCodes();

            if (ModelState.IsValid)
            {
                User user = null;
                string userId = HttpContext.Session.GetString("UserId");
                SessionHandler.GetUser(userId, out user);

                if (user.Money < payment.Amount)
                {
                    ViewBag.Insufficient = "Insufficient money.";
                    return View(payment);
                }

                if (payment.DestBank == 666 && !_context.AccountExist(payment.DestAccount)) return View("NoAccount", payment);

                int t = TransactionHandler.NewPayment(user, payment);
                HttpContext.Session.SetInt32("Payment", t);
                return View("PaymentConfirm", payment);
            }

            ViewBag.ErrMsg = "Values are not valid.";
            return View(payment);
        }

        /// <summary>
        /// POST/Payment/PaymentConfirm
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentConfirm(Payment payment, string code)
        {
            var t = HttpContext.Session.GetInt32("Payment");

            if (t == null) RedirectToAction(nameof(Payment));

            if (TransactionHandler.IsValid((int)t, code))
            {
                User user = null;
                string userId = HttpContext.Session.GetString("UserId");
                SessionHandler.GetUser(userId, out user);

                try
                {
                    await _context.MakePayment(user, payment);

                }
                catch
                {
                    return Redirect("/Home/Error");
                }

                return RedirectToAction(nameof(PaymentList));
            }
            else
            {
                ViewBag.ErrMsg = "Wrong confirmation code!";
                return View(payment);
            }
        }

        /// <summary>
        /// POST/Payment/NoAccount
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NoAccount(Payment payment)
        {
            User user = null;
            string userId = HttpContext.Session.GetString("UserId");
            SessionHandler.GetUser(userId, out user);

            int t = TransactionHandler.NewPayment(user, payment);
            HttpContext.Session.SetInt32("Payment", t);
            return View("PaymentConfirm", payment);
        }

        /// <summary>
        /// GET/Payment/PaymentList
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PaymentList()
        {
            Models.Payment.page = 0;
            ViewBag.Page = Models.Payment.page;

            User user = null;
            string userId = HttpContext.Session.GetString("UserId");
            SessionHandler.GetUser(userId, out user);

            List<Payment> list = await _context.Payment.Where(e => e.UserId == user.Id).ToListAsync();

            int offset = Models.Payment.page * Models.Payment.paginator;

            if (list.Count >= offset + Models.Payment.paginator)
            {
                list = list.GetRange(offset, Models.Payment.paginator);
            }
            else
            {
                list = list.GetRange(offset, list.Count - offset);
            }

            return View(list);
        }

        /// <summary>
        /// GET/Payment/PaymentList/id
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PaymentListPage(int? id)
        {
            switch (id)
            {
                case 1: Models.Payment.page++; break;
                case 0: Models.Payment.page--; break;
                case 20:
                case 50:
                case 100:
                    Models.Payment.paginator = (int)id;
                    break;
                case null:
                default:
                    return RedirectToAction(nameof(PaymentList));
            }

            ViewBag.DisP = "";
            ViewBag.DisN = "";

            if (Models.Payment.page < 0) Models.Payment.page = 0;
            if (Models.Payment.page == 0) ViewBag.DisP = "disabled";
            ViewBag.Page = Models.Payment.page;

            User user = null;
            string userId = HttpContext.Session.GetString("UserId");
            SessionHandler.GetUser(userId, out user);

            List<Payment> list = await _context.Payment.Where(e => e.UserId == user.Id).ToListAsync();

            int offset = Models.Payment.page * Models.Payment.paginator;

            if (list.Count >= offset + Models.Payment.paginator)
            {
                list = list.GetRange(offset, Models.Payment.paginator);
            }
            else if (list.Count >= offset)
            {
                list = list.GetRange(offset, list.Count - offset);
                ViewBag.DisN = "disabled";
            }
            else
            {
                return RedirectToAction(nameof(PaymentList));
            }

            

            return View("PaymentList", list);
        }

        /// <summary>
        /// GET/Payment/PaymentTemplate/id
        /// 
        /// Redirect to Payment
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PaymentTemplate(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Payment));

            var template = await _context.Template.FindAsync(id);
            if (template == null) return RedirectToAction(nameof(Payment));

            ViewBag.Templates = await GetTemplates();
            ViewBag.BankCodes = DataHandler.GetBankCodes();

            return View("Payment", new Payment(template));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        /// <summary>
        /// Returns list of templates
        /// </summary>
        /// <returns></returns>
        private async Task<List<Template>> GetTemplates()
        {
            User user = null;
            string userId = HttpContext.Session.GetString("UserId");
            SessionHandler.GetUser(userId, out user);

            return await _context.Template.Where(e => e.UserId == user.Id).ToListAsync();
        }
    }
}
