using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Handlers;
using Bank.Filters;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Controllers
{
    /// <summary>
    /// Controller for template operations pages
    /// </summary>
    [UserFilter]
    public class TemplateController : Controller
    {
        private readonly BankContext _context;

        public TemplateController(BankContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET/Template
        /// 
        /// Redirect to TemplateList
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction(nameof(TemplateList));
        }

        /// <summary>
        /// GET/Template/TemplateList
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TemplateList()
        {
            User user = null;
            string userId = HttpContext.Session.GetString("UserId");
            SessionHandler.GetUser(userId, out user);

            return View(await _context.Template.Where(t => t.UserId == user.Id).ToListAsync());
        }

        /// <summary>
        /// GET/Template/AddTemplate
        /// </summary>
        /// <returns></returns>
        public IActionResult AddTemplate()
        {
            return View();
        }

        /// <summary>
        /// POST/Template/AddTemplate
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTemplate(Template template)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                string userId = HttpContext.Session.GetString("UserId");
                SessionHandler.GetUser(userId, out user);

                if (!_context.IsTemplateNameUnique(template.Name, user.Id))
                {

                    ViewBag.ErrName = "Template name must be unique.";
                    return View(template);
                }

                template.UserId = user.Id;

                try
                {
                    _context.Add(template);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(TemplateList));
                }
                catch
                {
                    return Redirect("/Home/Error");
                }
            }

            return View(template);
        }

        /// <summary>
        /// GET/Template/EditTemplate/id
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditTemplate(int? id)
        {
            if (id == null) return View();

            var temp = await _context.Template.FindAsync(id);
            if (temp == null) return View();

            return View(temp);
        }

        /// <summary>
        /// POST/Template/EditTemplate/id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTemplate(int id, Template template)
        {
            if (ModelState.IsValid)
            {
                Template t = await _context.Template.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

                if (!_context.IsTemplateNameUnique(template.Name, t.Id) && template.Name != t.Name)
                {
                    ViewBag.ErrName = "Template name must be unique.";
                    return View(template);
                }

                template.UserId = t.UserId;

                try
                {
                    _context.Update(template);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(TemplateList));
                }
                catch
                {
                    return Redirect("/Home/Error");
                }
            }
            else
            {
                return View(template);
            }
        }

        /// <summary>
        /// GET/Template/DeleteTemplate/id
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DeleteTemplate(int? id)
        {
            if (id == null) return RedirectToAction(nameof(TemplateList));

            var temp = await _context.Template.FirstOrDefaultAsync(e => e.Id == id);
            if (temp == null) return RedirectToAction(nameof(Index));

            _context.Template.Remove(temp);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TemplateList));
        }
    }
}