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
            var user = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.benutzername == benutzer.benutzername && b.passwort == benutzer.passwort);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.id_benutzer.ToString()),
                    new Claim(ClaimTypes.Role, user.admin.ToString())

                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(returnURL == null ? "/Home" : returnURL);
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

        
        //Regoster POST
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

            string mannschaft = Request.Form["mannschaft"];

            if(benutzer.is_spieler == 1)
            {
                benutzer.fk_mannschaft_id = Convert.ToInt32(mannschaft);
            }
            else
            {
                //hier nehme ich einfach den Wert 1, da der Fremdschlüssel nicht null sein darf. Dieser Wert hat später keinen Einfluss auf das Progrmam
                benutzer.fk_mannschaft_id = 1;
            }


            _dbMatchplaner.Benutzer.Add(benutzer);

            _dbMatchplaner.SaveChanges();

            TempData["RegisterSuccess"] = "Ihr Benutzerkonto wurde erstellt.";

            return RedirectToAction(nameof(Login));
        }







        [Authorize(Roles = "0")]
        public IActionResult EditBenutzer(string id_benutzer)
        {
            var benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.id_benutzer.ToString() == id_benutzer);

            return View(benutzer);
        }

        [Authorize(Roles = "0")]
        [HttpPost]
        public IActionResult EditBenutzer(string id_benutzer, [Bind("nachname, vorname, benutzername, passwort")] Benutzer benutzerData)
        {
            var benutzer = _dbMatchplaner.Benutzer.FirstOrDefault(b => b.id_benutzer.ToString() == id_benutzer); 

            benutzer.vorname = benutzerData.vorname;
            benutzer.nachname = benutzerData.nachname;
            benutzer.passwort = benutzerData.passwort;
            benutzer.benutzername = benutzerData.benutzername;

            _dbMatchplaner.Benutzer.Update(benutzer);

            try
            {
                _dbMatchplaner.SaveChanges();

                ViewBag.EditMessage = "Daten erfolgreich geändert.";
            }
            catch (Exception)
            {
                ViewBag.EditError = "Es ist ein Fehler aufgetreten.";
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