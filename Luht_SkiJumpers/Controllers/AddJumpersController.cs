using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Luht_SkiJumpers.Data;
using Luht_SkiJumpers.Models;

namespace Luht_SkiJumpers.Controllers
{
    public class AddJumpersController : Controller
    {
        private readonly Luht_SkiJumpersContext _context;

        public AddJumpersController(Luht_SkiJumpersContext context)
        {
            _context = context;
        }

        public IActionResult AddJumper()
        {
            var jumper = new Jumpers();
            jumper.Id = GenerateId();
            return View(jumper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJumper([Bind("Id,Name")] Jumpers addJumpers)
        {
            if (ModelState.IsValid)
            {
                if (!int.TryParse(addJumpers.Id, out _))
                {
                    ModelState.AddModelError("Id", "The Id field should contain only numbers.");
                    return View(addJumpers);
                }
                _context.Add(addJumpers);
                await _context.SaveChangesAsync();
                return RedirectToAction("JumpersList", "AddJumpers");
            }
            return View(addJumpers);
        }

        private string GenerateId()
        {
            var lastId = _context.Jumpers.OrderByDescending(j => j.Id).Select(j => j.Id).FirstOrDefault();
            if (lastId != null)
            {
                var newId = Convert.ToInt32(lastId) + 1;
                return newId.ToString();
            }
            else
            {
                return "1";
            }
        }

        public async Task<IActionResult> JumpersList()
        {
            List<Jumpers> jumpers = await _context.Jumpers.OrderByDescending(j => j.Distance).ToListAsync();

            for (int i = 0; i < jumpers.Count; i++)
            {
                jumpers[i].Standings = i + 1;
            }

            await SaveToDatabase(jumpers);
            return View(jumpers);
        }

        public IActionResult Rankings()
        {
            List<Jumpers> jumpers = _context.Jumpers.OrderByDescending(j => j.Distance).ToList();

            int lastPlace = 1;
            for (int i = 0; i < jumpers.Count; i++)
            {
                if (jumpers[i].Started == true)
                {
                    jumpers[i].Standings = jumpers.Count;
                    jumpers[i].Distance = 0;
                }
                else
                {
                    jumpers[i].Standings = lastPlace;
                    lastPlace++;
                }
            }

            SaveToDatabase(jumpers);
            return View(jumpers);
        }

        public async Task<IActionResult> AddDistance(string id)
        {
            if (id == null || _context.Jumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.Jumpers.FindAsync(id);
            if (addJumpers == null)
            {
                return NotFound();
            }
            return View(addJumpers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistance(string id, [Bind("Id,Name,Distance,Started,Finished")] Jumpers addJumpers)
        {
            if (id != addJumpers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    addJumpers.Finished = true;
                    _context.Update(addJumpers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddJumpersExists(addJumpers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (addJumpers.Started == true)
                {
                    addJumpers.Distance = 0;
                }
                return RedirectToAction(nameof(Rankings));
            }
            return View(addJumpers);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Jumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.Jumpers.FirstOrDefaultAsync(m => m.Id == id);
            if (addJumpers == null)
            {
                return NotFound();
            }

            return View(addJumpers);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Jumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.Jumpers.FirstOrDefaultAsync(m => m.Id == id);
            if (addJumpers == null)
            {
                return NotFound();
            }

            return View(addJumpers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Jumpers == null)
            {
                return Problem("Entity set 'Luht_SkiJumpersContext.Jumpers' is null.");
            }
            var addJumpers = await _context.Jumpers.FindAsync(id);
            if (addJumpers != null)
            {
                _context.Jumpers.Remove(addJumpers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(JumpersList));
        }

        private bool AddJumpersExists(string id)
        {
            return _context.Jumpers.Any(e => e.Id == id);
        }

        private async Task SaveToDatabase(List<Jumpers> jumpers)
        {
            foreach (var jumper in jumpers)
            {
                _context.Update(jumper);
            }

            await _context.SaveChangesAsync();
        }
    }
}
