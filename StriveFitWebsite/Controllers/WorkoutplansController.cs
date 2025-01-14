using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveFitWebsite.Models;

namespace StriveFitWebsite.Controllers
{
    public class WorkoutplansController : Controller
    {
        private readonly ModelContext _context;

        public WorkoutplansController(ModelContext context)
        {
            _context = context;
        }

        // GET: Workoutplans
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Workoutplans.Include(w => w.Member).Include(w => w.Schedule).Include(w => w.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Workoutplans/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Workoutplans == null)
            {
                return NotFound();
            }

            var workoutplan = await _context.Workoutplans
                .Include(w => w.Member)
                .Include(w => w.Schedule)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.Workoutid == id);
            if (workoutplan == null)
            {
                return NotFound();
            }

            return View(workoutplan);
        }

        // GET: Workoutplans/Create
        public IActionResult Create()
        {
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Scheduleid"] = new SelectList(_context.Schedules, "Scheduleid", "Scheduleid");
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Workoutplans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Workoutid,Trainerid,Memberid,Scheduleid")] Workoutplan workoutplan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutplan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Memberid);
            ViewData["Scheduleid"] = new SelectList(_context.Schedules, "Scheduleid", "Scheduleid", workoutplan.Scheduleid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Trainerid);
            return View(workoutplan);
        }

        // GET: Workoutplans/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Workoutplans == null)
            {
                return NotFound();
            }

            var workoutplan = await _context.Workoutplans.FindAsync(id);
            if (workoutplan == null)
            {
                return NotFound();
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Memberid);
            ViewData["Scheduleid"] = new SelectList(_context.Schedules, "Scheduleid", "Scheduleid", workoutplan.Scheduleid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Trainerid);
            return View(workoutplan);
        }

        // POST: Workoutplans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Workoutid,Trainerid,Memberid,Scheduleid")] Workoutplan workoutplan)
        {
            if (id != workoutplan.Workoutid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutplan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutplanExists(workoutplan.Workoutid))
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
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Memberid);
            ViewData["Scheduleid"] = new SelectList(_context.Schedules, "Scheduleid", "Scheduleid", workoutplan.Scheduleid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workoutplan.Trainerid);
            return View(workoutplan);
        }

        // GET: Workoutplans/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Workoutplans == null)
            {
                return NotFound();
            }

            var workoutplan = await _context.Workoutplans
                .Include(w => w.Member)
                .Include(w => w.Schedule)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.Workoutid == id);
            if (workoutplan == null)
            {
                return NotFound();
            }

            return View(workoutplan);
        }

        // POST: Workoutplans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Workoutplans == null)
            {
                return Problem("Entity set 'ModelContext.Workoutplans'  is null.");
            }
            var workoutplan = await _context.Workoutplans.FindAsync(id);
            if (workoutplan != null)
            {
                _context.Workoutplans.Remove(workoutplan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutplanExists(decimal id)
        {
          return (_context.Workoutplans?.Any(e => e.Workoutid == id)).GetValueOrDefault();
        }
    }
}
