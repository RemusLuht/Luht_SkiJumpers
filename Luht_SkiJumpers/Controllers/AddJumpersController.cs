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
            return View();
        }

        // POST: AddJumpers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJumper([Bind("Id,Name")] AddJumpers addJumpers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addJumpers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(JumpersList));
            }
            return View(addJumpers);
        }


        private async Task SaveToDatabase(List<AddJumpers> jumpers)
        {
            foreach (var jumper in jumpers)
            {
                _context.Update(jumper);
            }

            await _context.SaveChangesAsync();
        }


        public async Task<IActionResult> AddDistance(string id)
        {
            if (id == null || _context.AddJumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.AddJumpers.FindAsync(id);
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
        public async Task<IActionResult> AddDistance(string id, [Bind("Id,Name, Distance")] AddJumpers addJumpers)
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
                return RedirectToAction(nameof(Rankings));
            }
            return View(addJumpers);
        }

        public IActionResult Rankings()
        {
            var jumpers = _context.AddJumpers.OrderByDescending(j => j.Distance).ToList();

            for (int i = 0; i < jumpers.Count; i++)
            {
                jumpers[i].Standings = i + 1;
            }

            SaveToDatabase(jumpers);
            return View(jumpers);
        }





        // GET: AddJumpers
        public async Task<IActionResult> JumpersList()
        {
            var jumpers = await _context.AddJumpers.ToListAsync();
            SaveToDatabase(jumpers);
            return View(jumpers);
        }

        // GET: AddJumpers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AddJumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.AddJumpers
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
        public async Task<IActionResult> Create([Bind("Id,Name")] AddJumpers addJumpers)
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
            if (id == null || _context.AddJumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.AddJumpers.FindAsync(id);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] AddJumpers addJumpers)
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
            if (id == null || _context.AddJumpers == null)
            {
                return NotFound();
            }

            var addJumpers = await _context.AddJumpers
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
            if (_context.AddJumpers == null)
            {
                return Problem("Entity set 'Luht_SkiJumpersContext.AddJumpers'  is null.");
            }
            var addJumpers = await _context.AddJumpers.FindAsync(id);
            if (addJumpers != null)
            {
                _context.AddJumpers.Remove(addJumpers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddJumpersExists(string id)
        {
          return _context.AddJumpers.Any(e => e.Id == id);
        }
    }
}
