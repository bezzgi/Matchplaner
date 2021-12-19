using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Matchplaner.Controllers
{
    public class AccountController : Controller
    {

        private DbMatchplaner _dbMatchplaner;

        public AccountController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
        }

        public IActionResult Login()
        {
            return View();
        }


        //Login
        [HttpPost]
        public async Task<IActionResult> Login(Benutzer benutzer, string returnURL)
        {
            var user = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.benutzername == benutzer.benutzername);

            if (user != null)
            {
                bool validate = Crypto.VerifyHashedPassword(user.passwort, benutzer.passwort);

                if(validate == true)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.id_benutzer.ToString()),
                        new Claim(ClaimTypes.Role, user.admin.ToString())

                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    Logger logger = new Logger();
                    logger.fk_benutzer_id = user.id_benutzer;
                    logger.logging = "Angemeldet";
                    logger.zeit = DateTime.Now;
                    _dbMatchplaner.Logger.Add(logger);
                    _dbMatchplaner.SaveChanges();

                    return Redirect(returnURL == null ? "/Home" : returnURL);
                }
                else
                {
                    ViewBag.LoginError = "Falscher Benutzername oder Passwort";

                    return View();
                }
            }
            else
            {
                ViewBag.LoginError = "Falscher Benutzername oder Passwort";

                return View();
            }
        }





        //Register
        public IActionResult Register()
        {
            registerHelper model = new registerHelper();

            model.Qualifikationen = _dbMatchplaner.Qualifikation.ToList();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            return View(model);
        }

        
        //Register POST
        [HttpPost]
        public IActionResult Register(registerHelper model, Benutzer benutzer)
        {
            model.Qualifikationen = _dbMatchplaner.Qualifikation.ToList();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            string[] qualifikation = Request.Form["qualifikation"];

            if (qualifikation.Length == 0)
            {
                ViewBag.RegisterError = ("Wählen Sie eine Qualifikation!");

                return View(model);
            }

            int benutzerAlreadyTaken = _dbMatchplaner.Benutzer.Count(b => b.benutzername == benutzer.benutzername);

            if(benutzerAlreadyTaken >= 1)
            {
                ViewBag.RegisterError = ("Dieser Benutzername ist bereits vergeben!");

                return View(model);
            }

            for(int i = 0; i < qualifikation.Length; i++)
            {
                if (qualifikation[i].Equals("1"))
                {
                    benutzer.is_schiedsrichter = 1;
                }
                if (qualifikation[i].Equals("2"))
                {
                    benutzer.is_punkteschreiber = 1;
                }
                if (qualifikation[i].Equals("3"))
                {
                    benutzer.is_spieler = 1;
                }
            }

            benutzer.admin = 0;

            if(ModelState.IsValid)
            {
                benutzer.passwort = Crypto.HashPassword(benutzer.passwort);
            }
            else
            {
                return View(model);
            }
           

            string mannschaft = Request.Form["mannschaft"];

            if(benutzer.is_spieler == 1)
            {
                if(mannschaft != null)
                {
                    benutzer.fk_mannschaft_id = Convert.ToInt32(mannschaft);
                }
                else
                {
                    ViewBag.RegisterError = ("Wählen Sie eine Mannschaft!");

                    return View(model);
                }
            }
            else
            {
                //hier nehme ich einfach den Wert 1, da der Fremdschlüssel nicht null sein darf. Dieser Wert hat später keinen Einfluss auf das Programm
                benutzer.fk_mannschaft_id = 1;
            }

            _dbMatchplaner.Benutzer.Add(benutzer);

            if (ModelState.IsValid)
            {
                _dbMatchplaner.SaveChanges();

                var id_benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.benutzername == benutzer.benutzername);

                Logger logger = new Logger();
                logger.fk_benutzer_id = id_benutzer.id_benutzer;
                logger.logging = "Registriert";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

                _dbMatchplaner.SaveChanges();

                TempData["RegisterSuccess"] = "Ihr Benutzerkonto wurde erstellt.";

                return RedirectToAction(nameof(Login));
            }
            else
            {
                return View(model);
            }           
        }



        public IActionResult AccessDenied()
        {
            return View();
        }



        [Authorize(Roles = "0")]
        public IActionResult EditBenutzer()
        {
            var benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.id_benutzer == Convert.ToInt32(User.Identity.Name));

            return View(benutzer);
        }

        [Authorize(Roles = "0")]
        [HttpPost]
        public IActionResult EditBenutzer([Bind("nachname, vorname, benutzername, passwort")] Benutzer benutzerData)
        {
            var benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.id_benutzer == Convert.ToInt32(User.Identity.Name)); 

            benutzer.vorname = benutzerData.vorname;
            benutzer.nachname = benutzerData.nachname;
            benutzer.passwort = benutzerData.passwort;
            benutzer.benutzername = benutzerData.benutzername;

            if (ModelState.IsValid)
            {
                string hashedPassword = Crypto.HashPassword(benutzer.passwort);

                benutzer.passwort = hashedPassword;
            }
            else
            {
                return View();
            }

            _dbMatchplaner.Benutzer.Update(benutzer);

            try
            {
                Logger logger = new Logger();
                logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                logger.logging = "Konto bearbeitet";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

                _dbMatchplaner.SaveChanges();

                ViewBag.EditMessage = "Daten erfolgreich geändert.";
            }
            catch (Exception)
            {
                
            }

            return View();
        }




        public IActionResult DeleteBenutzer()
        {
            var benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.id_benutzer == Convert.ToInt32(User.Identity.Name));

            _dbMatchplaner.Remove(benutzer);

            _dbMatchplaner.SaveChanges();

            return RedirectToAction("Logout", "Home");
        }
    }
}