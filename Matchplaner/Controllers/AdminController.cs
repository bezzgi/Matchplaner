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
    public class AdminController : Controller
    {

        private DbMatchplaner _dbMatchplaner;

        private registerHelper data = new registerHelper();

        public AdminController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
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
            catch (Exception e)
            {
                ViewBag.CreateAdminError = "Es ist ein Fehler aufgetreten: " + e.Message;
            }

            return View();
        }


        public IActionResult ShowAdmin()
        {
            var benutzer = _dbMatchplaner.Benutzer.ToList();

            return View(benutzer);
        }

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

