using Luht_SkiJumpers.Data;
using Microsoft.AspNetCore.Mvc;
using Luht_SkiJumpers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luht_SkiJumpers.Controllers
{
    public class DescendingController : Controller
    {
        //private static bool _isDescending = false;
        private readonly Luht_SkiJumpersContext _context;

        public DescendingController(Luht_SkiJumpersContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Descend(string Id, bool Started)
        {
            TempData["id"] = Id;

            if (Started)
            {
                ViewData["ErrorMessage"] = "Another jumper is currently descending. Please wait.";
                return View("Index");
            }

            var jumper = _context.AddJumpers.FirstOrDefault(j => j.Id == Id);

            if (jumper == null)
            {
                ViewData["ErrorMessage"] = "Jumper not found.";
                return View("Index");
            }

            Started = true;
            ViewData["JumperName"] = jumper.Name;
            return View();
        }
        [HttpPost]
        public IActionResult Fail([Bind("Started,Id")] Jumpers addJumpers)
        {
            addJumpers.Id = TempData["id"] as string;
            addJumpers.Started = true;
            _context.Update(addJumpers);
            return RedirectToAction("Rankings", "AddJumpers");
        }

        [HttpPost]
        public IActionResult Leave()
        {
            var id = TempData["id"] as string;
            return RedirectToAction("AddDistance", "AddJumpers", new { id = id });
        }
    }
}
