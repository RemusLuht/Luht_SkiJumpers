using Luht_SkiJumpers.Data;
using Microsoft.AspNetCore.Mvc;

namespace Luht_SkiJumpers.Controllers
{
    public class JumpController : Controller
    {
        private static bool _isDescending = false;
        private readonly Luht_SkiJumpersContext _context;

        public JumpController(Luht_SkiJumpersContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Descend(string jumperId)
        {
            if (_isDescending)
            {
                ViewData["ErrorMessage"] = "Another jumper is currently descending. Please wait.";
                return View("Index");
            }

            var jumper = _context.AddJumpers.FirstOrDefault(j => j.Id == jumperId);

            if (jumper == null)
            {
                ViewData["ErrorMessage"] = "Jumper not found.";
                return View("Index");
            }

            _isDescending = true;
            ViewData["JumperName"] = jumper.Name;
            return View();
        }

        [HttpPost]
        public IActionResult Leave()
        {
            _isDescending = false;
            return RedirectToAction("Index");
        }
    }
}
