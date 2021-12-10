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
            registerHelper model = new registerHelper();

            model.Qualifikationen = _dbMatchplaner.Qualifikation.ToList();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();



            return View(model);
        }

        //<>

        [HttpPost]
        public IActionResult Register(registerHelper model, Benutzer benutzer)
        {
            model.Qualifikationen = _dbMatchplaner.Qualifikation.ToList();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            benutzer.admin = 0;

            _dbMatchplaner.Benutzer.Add(benutzer);

            var chosenQualifikationen = model.Qualifikationen.Where(x => x.IsChecked == true).ToList();

            _dbMatchplaner.SaveChangesAsync();

            return View();
        }
    }
}

/*@for (int i = 0; i < Model.Qualifikation.Count; i++)
{
    @Html.CheckBoxFor(model => model.Qualifikation[i].IsChecked)
                    @Html.DisplayFor(model => model.Qualifikation[i].name)
                    @Html.HiddenFor(model => model.Qualifikation[i].id_qualifikation)
                }




@foreach(var registerModel in Model.Qualifikation)
                {
                    < div class= "form-check form-check-inline" >

                         @Html.CheckBoxFor(model => registerModel.IsChecked)
                        @Html.DisplayFor(model => registerModel.name)
                        @Html.HiddenFor(model => registerModel.id_qualifikation)*/

