using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

                mhasm.match_id_match = match.id_match;

                foreach (var data in mannschaft)
                {
                    mhasm.mannschaft_id_mannschaft = Convert.ToInt32(data);

                    _dbMatchplaner.Match_Has_Mannschaft.Add(mhasm);

                    if(ModelState.IsValid)
                    {
                        ViewBag.CreateMatchMessage = "Das Match wurde erfolgreich erstellt!";

                        _dbMatchplaner.SaveChanges();
                    }
                    else
                    {
                        return View(model);
                    }
                }

                Logger logger = new Logger();
                logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                logger.logging = "Match erstellt";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

                _dbMatchplaner.SaveChanges();

                return View(model);
            }
        }

        public IActionResult DetailsMatch(int? rowguid, matchHelper model)
        {
            if(rowguid == null)
            {
                return NotFound();
            }

            var mannschaft = _dbMatchplaner.Match_Has_Mannschaft.Include(x => x.Mannschaft).Where(m => m.match_id_match == rowguid).Select(m => m.Mannschaft).ToList();

            model.Mannschaften = mannschaft;

            var mhasb = _dbMatchplaner.Match_Has_Benutzer.FirstOrDefault(b => b.match_id_match == rowguid && b.benutzer_id_benutzer.ToString() == User.Identity.Name);

            model.MhasB = mhasb;

            var match = _dbMatchplaner.Match.Where(m => m.id_match == rowguid).FirstOrDefault();

            if (match == null)
            {
                return NotFound();
            }

            model.Match = match;

            ViewBag.countSpieler = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_spieler == 1);
            ViewBag.countSchiedsrichter = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_schiedsrichter == 1);
            ViewBag.countPunkteschreiber = _dbMatchplaner.Match_Has_Benutzer.Count(b => b.match_id_match == rowguid && b.benutzer_is_punkteschreiber == 1);

            model.Spieler = _dbMatchplaner.Match_Has_Benutzer.Include(b => b.Benutzer).Where(m => m.match_id_match == rowguid && m.benutzer_is_spieler == 1).Select(m => m.Benutzer).ToList();

            model.Punkteschreiber = _dbMatchplaner.Match_Has_Benutzer.Include(b => b.Benutzer).Where(m => m.match_id_match == rowguid && m.benutzer_is_punkteschreiber == 1).Select(m => m.Benutzer).ToList();

            model.Schiedsrichter = _dbMatchplaner.Match_Has_Benutzer.Include(b => b.Benutzer).Where(m => m.match_id_match == rowguid && m.benutzer_is_schiedsrichter == 1).Select(m => m.Benutzer).ToList();

            if (User.Identity.Name != null)
            {
                model.Benutzer = _dbMatchplaner.Benutzer.Where(b => b.id_benutzer.ToString() == User.Identity.Name).FirstOrDefault();
            }
            else
            {
                model.Benutzer = _dbMatchplaner.Benutzer.FirstOrDefault();
            }

            return View(model);
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

            Logger logger = new Logger();
            logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
            logger.logging = "Match gelöscht";
            logger.zeit = DateTime.Now;
            _dbMatchplaner.Logger.Add(logger);

            _dbMatchplaner.Match.Remove(match);
            _dbMatchplaner.SaveChanges();

            return RedirectToAction(nameof(ShowMatch));
        }

        [Authorize(Roles = "0")]
        public async Task<IActionResult> Schiedsrichter(int rowguid)
        {
            MhasB mhasb = new MhasB();

            var benutzer = await _dbMatchplaner.Benutzer.FirstOrDefaultAsync(b => b.id_benutzer.ToString() == User.Identity.Name);

            if (benutzer.is_schiedsrichter != 1)
            {
                TempData["MannschaftMessage"] = "Sie sind kein Schiedsrichter.";

                return RedirectToAction("DetailsMatch", new { rowguid });
            }

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_schiedsrichter = 1;

            try
            {
                _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                Logger logger = new Logger();
                logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                logger.logging = "Als Schiedsrichter angemeldet";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

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

            if (benutzer.is_punkteschreiber != 1)
            {
                TempData["MannschaftMessage"] = "Sie sind kein Punkteschreiber.";

                return RedirectToAction("DetailsMatch", new { rowguid });
            }

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_punkteschreiber = 1;

            try
            {
                _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                Logger logger = new Logger();
                logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                logger.logging = "Als Punkteschreiber angemeldet";
                logger.zeit = DateTime.Now;
                _dbMatchplaner.Logger.Add(logger);

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

            if (benutzer.is_spieler != 1)
            {
                TempData["MannschaftMessage"] = "Sie sind kein Spieler.";

                return RedirectToAction("DetailsMatch", new { rowguid });
            }

            var mannschaft = _dbMatchplaner.Match_Has_Mannschaft.Include(x => x.Mannschaft).Where(m => m.match_id_match == rowguid).Select(m => m.Mannschaft).ToList();
            

            mhasb.match_id_match = rowguid;

            mhasb.benutzer_id_benutzer = benutzer.id_benutzer;

            mhasb.benutzer_is_spieler = 1;

            try
            {
                int i = 0;

                foreach (var data in mannschaft)
                {
                    i++;

                    if (data.id_mannschaft == benutzer.fk_mannschaft_id)
                    {
                        _dbMatchplaner.Match_Has_Benutzer.Add(mhasb);

                        Logger logger = new Logger();
                        logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
                        logger.logging = "Als Spieler angemeldet";
                        logger.zeit = DateTime.Now;
                        _dbMatchplaner.Logger.Add(logger);

                        _dbMatchplaner.SaveChanges();

                        break;
                    }
                    else
                    {
                        if(i != 1)
                        {
                            TempData["MannschaftMessage"] = "Sie spielen in keiner der beiden Mannschaften.";
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return RedirectToAction("DetailsMatch", new { rowguid });
        }

        [Authorize(Roles = "0")]
        public IActionResult DeleteFromMatch(int? rowguid)
        {
            if(rowguid == null)
            {
                return NotFound();
            }

            var mhasb = _dbMatchplaner.Match_Has_Benutzer.FirstOrDefault(m => m.match_id_match == rowguid && m.benutzer_id_benutzer == Convert.ToInt32(User.Identity.Name));

            if(mhasb == null)
            {
                return NotFound();
            }

            _dbMatchplaner.Match_Has_Benutzer.Remove(mhasb);

            Logger logger = new Logger();
            logger.fk_benutzer_id = Convert.ToInt32(User.Identity.Name);
            logger.logging = "Vom Match abgemeldet";
            logger.zeit = DateTime.Now;
            _dbMatchplaner.Logger.Add(logger);

            _dbMatchplaner.SaveChanges();

            return RedirectToAction("DetailsMatch", new { rowguid });
        }
    }
}