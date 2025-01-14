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
    public class MembershipplansController : Controller
    {
        private readonly ModelContext _context;

        public MembershipplansController(ModelContext context)
        {
            _context = context;
        }

        // GET: Membershipplans
        public async Task<IActionResult> Index()
        {
              return _context.Membershipplans != null ? 
                          View(await _context.Membershipplans.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Membershipplans'  is null.");
        }

        // GET: Membershipplans/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Membershipplans == null)
            {
                return NotFound();
            }

            var membershipplan = await _context.Membershipplans
                .FirstOrDefaultAsync(m => m.Planid == id);
            if (membershipplan == null)
            {
                return NotFound();
            }

            return View(membershipplan);
        }

        // GET: Membershipplans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Membershipplans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Planid,Planname,Price,Durationmonths,Details")] Membershipplan membershipplan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipplan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipplan);
        }

        // GET: Membershipplans/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Membershipplans == null)
            {
                return NotFound();
            }

            var membershipplan = await _context.Membershipplans.FindAsync(id);
            if (membershipplan == null)
            {
                return NotFound();
            }
            return View(membershipplan);
        }

        // POST: Membershipplans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Planid,Planname,Price,Durationmonths,Details")] Membershipplan membershipplan)
        {
            if (id != membershipplan.Planid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipplan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipplanExists(membershipplan.Planid))
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
            return View(membershipplan);
        }

        // GET: Membershipplans/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Membershipplans == null)
            {
                return NotFound();
            }

            var membershipplan = await _context.Membershipplans
                .FirstOrDefaultAsync(m => m.Planid == id);
            if (membershipplan == null)
            {
                return NotFound();
            }

            return View(membershipplan);
        }

        // POST: Membershipplans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Membershipplans == null)
            {
                return Problem("Entity set 'ModelContext.Membershipplans'  is null.");
            }
            var membershipplan = await _context.Membershipplans.FindAsync(id);
            if (membershipplan != null)
            {
                _context.Membershipplans.Remove(membershipplan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipplanExists(decimal id)
        {
          return (_context.Membershipplans?.Any(e => e.Planid == id)).GetValueOrDefault();
        }
    }
}
