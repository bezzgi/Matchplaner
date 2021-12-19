using Matchplaner.Helpers;
using Matchplaner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Matchplaner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DbMatchplaner _dbMatchplaner;

        public HomeController(ILogger<HomeController> logger, DbMatchplaner dbMatchplaner)
        {
            _logger = logger;
            _dbMatchplaner = dbMatchplaner;
        }

        public async Task<IActionResult> Index()
        {
            homeHelper model = new homeHelper();

            var match = await _dbMatchplaner.Match.OrderBy(m => m.id_match).ToListAsync();
            var mhash = await _dbMatchplaner.Match_Has_Mannschaft.ToListAsync();

            model.MhasM = mhash;
            model.Matches = match;

            var mannschaft = _dbMatchplaner.Match_Has_Mannschaft.OrderBy(o => o.match_id_match).Include(x => x.Mannschaft).Select(m => m.Mannschaft).ToList();

            model.Mannschaften = mannschaft;

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
