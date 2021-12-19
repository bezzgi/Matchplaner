using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

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

                benutzer.passwort = Crypto.HashPassword(benutzer.passwort);

                int benutzerAlreadyTaken = _dbMatchplaner.Benutzer.Count(b => b.benutzername == benutzer.benutzername);

                if (benutzerAlreadyTaken >= 1)
                {
                    ViewBag.CreateAdminError = ("Dieser Benutzername ist bereits vergeben!");

                    return View();
                }

                _dbMatchplaner.Benutzer.Add(benutzer);

                Logger logger = new Logger();
                logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                logger.logging = "Admin erstellt";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

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

            Logger logger = new Logger();
            logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
            logger.logging = "Admin gelöscht";
            logger.zeit = DateTime.Now;
            _dbMatchplaner.Logger.Add(logger);

            _dbMatchplaner.SaveChanges();

            return RedirectToAction(nameof(ShowAdmin));
        } 
    }
}