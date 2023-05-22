using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Luht_SkiJumpers.Data;
using Luht_SkiJumpers.Models;
using Luht_SkiJumpers.Migrations;

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
            return View(jumper);
        }


        // POST: AddJumpers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                ViewBag.Names = addJumpers;
                await _context.SaveChangesAsync();
                return RedirectToAction("JumpersList", "AddJumpers");
            }
            return View(addJumpers);
        }



        private async Task SaveToDatabase(List<Jumpers> jumpers)
        {
            foreach (var jumper in jumpers)
            {
                _context.Update(jumper);
            }

            await _context.SaveChangesAsync();
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

        // POST: AddJumpers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        public IActionResult Rankings()
        {
            var jumpers = _context.Jumpers.OrderByDescending(j => j.Distance).ToList();

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





        // GET: AddJumpers
        public async Task<IActionResult> JumpersList()
        {
            List<Jumpers> jumpers = _context.Jumpers.OrderByDescending(j => j.Distance).ToList();

            for (int i = 0; i < jumpers.Count; i++)
            {
                jumpers[i].Standings = i + 1;
            }
            SaveToDatabase(jumpers);
            return View(jumpers);
        }

        // GET: AddJumpers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Jumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.Jumpers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addJumpers == null)
            {
                return NotFound();
            }

            return View(addJumpers);
        }

        // GET: AddJumpers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddJumpers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Jumpers addJumpers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addJumpers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addJumpers);
        }

        // GET: AddJumpers/Edit/5
        public async Task<IActionResult> Edit(string id)
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

        // POST: AddJumpers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Jumpers addJumpers)
        {
            if (id != addJumpers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            return View(addJumpers);
        }

        // GET: AddJumpers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Jumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.Jumpers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addJumpers == null)
            {
                return NotFound();
            }

            return View(addJumpers);
        }

        // POST: AddJumpers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Jumpers == null)
            {
                return Problem("Entity set 'Luht_SkiJumpersContext.Jumpers'  is null.");
            }
            var addJumpers = await _context.Jumpers.FindAsync(id);
            if (addJumpers != null)
            {
                _context.Jumpers.Remove(addJumpers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddJumpersExists(string id)
        {
          return _context.Jumpers.Any(e => e.Id == id);
        }
    }
}
