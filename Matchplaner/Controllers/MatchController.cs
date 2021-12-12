using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Matchplaner.Controllers
{
    public class MatchController : Controller
    {

        private DbMatchplaner _dbMatchplaner;

        public MatchController(DbMatchplaner dbMatchplaner)
        {
            _dbMatchplaner = dbMatchplaner;
        }

        [Authorize(Roles = "1")]
        public IActionResult CreateMatch()
        {
            matchHelper model = new matchHelper();

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            return View(model);
        }

        [Authorize(Roles = "0")]
        public IActionResult DetailsMatch(int? rowguid, matchHelper model)
        {
            /*var mannschaft = _dbMatchplaner.Match_Has_Mannschaft.Include(x => x.mannschaft_).Where(m => m.match_id_match == rowguid).Select(m => m.mannschaft_).ToList();

            model.Mannschaften = mannschaft;*/

            if(rowguid != null)
            {
                model.MhasB = _dbMatchplaner.Match_Has_Benutzer.FirstOrDefault(b => b.match_id_match == rowguid && b.benutzer_id_benutzer.ToString() == User.Identity.Name);
            }
            else
            {
                return NotFound();
            }

            model.Match = _dbMatchplaner.Match.Where(m => m.id_match == rowguid).FirstOrDefault();

            ViewBag.countSpieler = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_spieler == 1);

            ViewBag.countSchiedsrichter = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_schiedsrichter == 1);

            ViewBag.countPunkteschreiber = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_punkteschreiber == 1);

            model.Benutzer = _dbMatchplaner.Benutzer.Where(b => b.id_benutzer.ToString() == User.Identity.Name).FirstOrDefault();

            return View(model);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateMatch(MhasM mhasm, Match match, matchHelper model)
        {
            string[] mannschaft = Request.Form["mannschaft"];

            model.Mannschaften = _dbMatchplaner.Mannschaft.ToList();

            if(mannschaft.Length == 0 || mannschaft.Length == 1)
            {
                ViewBag.CreateMatchError = "Wählen Sie bitte zwei Mannschaften!";

                return View(model);
            }

            if (mannschaft[0] == mannschaft[1])
            {
                ViewBag.CreateMatchError = "Es können nicht dieselben Teams gegeneinander spielen.";

                return View(model);
            }
            else
            {
                _dbMatchplaner.Match.Add(match);

                _dbMatchplaner.SaveChanges();

                var id_match = _dbMatchplaner.Match.Where(m => m.id_match == match.id_match).FirstOrDefault();

                foreach (var data in mannschaft)
                {
                    mhasm.match_id_match = id_match.id_match;
                    mhasm.mannschaft_id_mannschaft = Convert.ToInt32(data);

                    _dbMatchplaner.Match_Has_Mannschaft.Add(mhasm);


                    _dbMatchplaner.SaveChanges();

                    ViewBag.CreateMatchMessage = "Das Match wurde erfolgreich erstellt!";

                }

                return View(model);
            }
        }

        [Authorize(Roles = "1")]
        public IActionResult ShowMatch()
        {
            var match = _dbMatchplaner.Match.ToList();

            return View(match);
        }

        public async Task<IActionResult> DeleteMatch(int? rowguid)
        {
            if (rowguid == null)
            {
                return NotFound();
            }

            var match = await _dbMatchplaner.Match.FirstOrDefaultAsync(b => b.id_match == rowguid);

            if (match == null)
            {
                return NotFound();
            }

            _dbMatchplaner.Match.Remove(match);
            await _dbMatchplaner.SaveChangesAsync();

            return RedirectToAction(nameof(ShowMatch));
        }

        [Authorize(Roles = "0")]
        public async Task<IActionResult> Schiedsrichter(int rowguid)
        {
            MhasB mhasb = new MhasB();

            var benutzer = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.id_benutzer.ToString() == User.Identity.Name);

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_schiedsrichter = 1;

            try
            {
                _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                _dbMatchplaner.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("DetailsMatch", new { rowguid });
        }

        [Authorize(Roles = "0")]
        public async Task<IActionResult> Punkteschreiber(int rowguid)
        {
            MhasB mhasb = new MhasB();

            var benutzer = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.id_benutzer.ToString() == User.Identity.Name);

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_punkteschreiber = 1;

            try
            {
                _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                _dbMatchplaner.SaveChanges();
            }
            catch(Exception)
            {

            }

            return RedirectToAction("DetailsMatch", new { rowguid });
        }

        [Authorize(Roles = "0")]
        public async Task<IActionResult> Spieler(int rowguid)
        {
            MhasB mhasb = new MhasB();

            var benutzer = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.id_benutzer.ToString() == User.Identity.Name);

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_spieler = 1;

            try
            {
                _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                _dbMatchplaner.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("DetailsMatch", new { rowguid });
        }

        [Authorize(Roles = "0")]
        public IActionResult DeleteFromMatch(int rowguid)
        {
            var mhasb = _dbMatchplaner.Match_Has_Benutzer.FirstOrDefault(m => m.match_id_match == rowguid && m.benutzer_id_benutzer == Convert.ToInt32(User.Identity.Name));

            _dbMatchplaner.Match_Has_Benutzer.Remove(mhasb);

            _dbMatchplaner.SaveChangesAsync();

            return RedirectToAction("DetailsMatch", new { rowguid });
        }
    }
}