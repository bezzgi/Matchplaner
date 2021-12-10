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
    public class MatchController : Controller
    {

        private DbMatchplaner _dbMatchplaner;

        public MatchController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
        }

        public IActionResult CreateMatch()
        {
            matchHelper model = new matchHelper();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            return View(model);

            
        }

        [HttpPost]
        public IActionResult CreateMatch(MhasM mhasm, Match match)
        {
            string[] mannschaft = Request.Form["mannschaft"];

            _dbMatchplaner.Match.Add(match);

            _dbMatchplaner.SaveChanges();

            var id_ma = _dbMatchplaner.Match.Where(m => m.id_match == match.id_match).FirstOrDefault();

            foreach (var data in mannschaft)
            {
                mhasm.match_id_match = id_ma.id_match;
                mhasm.mannschaft_id_mannschaft = Convert.ToInt32(data);

                _dbMatchplaner.Match_Has_Mannschaft.Add(mhasm);

                try
                {
                    _dbMatchplaner.SaveChanges();
                }
                catch(Microsoft.EntityFrameworkCore.DbUpdateException e)
                {
                    ViewBag.CreateMatchError = "Es können nicht dieselben Teams gegeneinander spielen.";

                    return View();
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}