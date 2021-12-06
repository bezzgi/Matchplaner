using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        private registerHelper data = new registerHelper();

        public AccountController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Benutzer benutzer, string returnURL)
        {
            var user = _dbMatchplaner.Benutzer.Where(b => b.benutzername == benutzer.benutzername && b.passwort == benutzer.passwort).FirstOrDefault();

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.benutzername),
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

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAdmin(Benutzer benutzer)
        {
            try
            {
                _dbMatchplaner.Benutzer.Add(benutzer);

                _dbMatchplaner.SaveChangesAsync();

                ViewBag.CreateAdminMessage = "Der Admin wurde erfolgreich erstellt!";
            }
            catch(Exception e)
            {
                ViewBag.CreateAdminError = "Es ist ein Fehler aufgetreten: " + e.Message;
            }

            return View();
        }
    }
}
