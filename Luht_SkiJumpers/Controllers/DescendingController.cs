using Luht_SkiJumpers.Data;
using Microsoft.AspNetCore.Mvc;
using Luht_SkiJumpers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace Luht_SkiJumpers.Controllers
{
    public class DescendingController : Controller
    {
        private static bool _isDescending = false;
        private readonly Luht_SkiJumpersContext _context;

        public DescendingController(Luht_SkiJumpersContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jumpers = _context.Jumpers.ToList();
            return View(jumpers);
        }


        [HttpPost]
        public async Task<IActionResult> Descend(string Jumper, bool Finished)
        {
            if (_isDescending)
            {
                ViewData["ErrorMessage"] = "Another jumper is currently descending. Please wait.";
                return View("Index");
            }

            var selectedJumper = await _context.Jumpers.FirstOrDefaultAsync(j => j.Id == Jumper && j.Finished == Finished);
            



            if (selectedJumper == null)
            {
                ViewData["ErrorMessage"] = "Jumper not found.";
                return View("Index");
            }

            _isDescending = true;
            ViewData["JumperName"] = selectedJumper.Name;
            selectedJumper.Started = true;

            _context.Update(selectedJumper);
            await _context.SaveChangesAsync();

            TempData["id"] = selectedJumper.Id;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Fail()
        {
            _isDescending=false;
            // retrieve the Id from TempData
            string id = TempData["id"].ToString();

            // find the Jumper object with the matching Id
            var jumper = await _context.Jumpers.FindAsync(id);

            if (jumper == null)
            {
                ViewData["ErrorMessage"] = "Jumper not found.";
                return View("Index");
            }

            // update the Started property and save changes
            jumper.Started = true;
            jumper.Finished = true;
            _context.Update(jumper);
            await _context.SaveChangesAsync();

            return RedirectToAction("Rankings", "AddJumpers");
        }

        [HttpPost]
        public IActionResult Leave()
        {
            _isDescending = false;
            var id = TempData["id"] as string;
            return RedirectToAction("AddDistance", "AddJumpers", new { id = id });
        }
    }
}
