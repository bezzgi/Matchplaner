using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Matchplaner.Controllers
{
    public class AdminController : Controller
    {

        private DbMatchplaner _dbMatchplaner;

        private registerHelper data = new registerHelper();

        public AdminController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
        }
        [Authorize(Roles = "1")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateAdmin(Benutzer benutzer)
        {
            try
            {
                benutzer.fk_mannschaft_id = 1;

                _dbMatchplaner.Benutzer.Add(benutzer);

                _dbMatchplaner.SaveChanges();

                ViewBag.CreateAdminMessage = "Der Admin wurde erfolgreich erstellt!";
            }
            catch (Exception)
            {
                
            }

            return View();
        }

        [Authorize(Roles = "1")]
        public IActionResult ShowAdmin()
        {
            var benutzer = _dbMatchplaner.Benutzer.Where(b => b.admin == 1).ToList();

            return View(benutzer);
        }

        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteAdmin(int? rowguid)
        {
            if (rowguid == null)
            {
                return NotFound();
            }

            var benutzer = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.id_benutzer == rowguid);

            if (benutzer == null)
            {
                return NotFound();
            }

            _dbMatchplaner.Benutzer.Remove(benutzer);
            await _dbMatchplaner.SaveChangesAsync();

            return RedirectToAction(nameof(ShowAdmin));
        }

        
    }
}