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
    public class TrainingtypesController : Controller
    {
        private readonly ModelContext _context;

        public TrainingtypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Trainingtypes
        public async Task<IActionResult> Index()
        {
              return _context.Trainingtypes != null ? 
                          View(await _context.Trainingtypes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Trainingtypes'  is null.");
        }

        // GET: Trainingtypes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Trainingtypes == null)
            {
                return NotFound();
            }

            var trainingtype = await _context.Trainingtypes
                .FirstOrDefaultAsync(m => m.Trainingtypeid == id);
            if (trainingtype == null)
            {
                return NotFound();
            }

            return View(trainingtype);
        }

        // GET: Trainingtypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainingtypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Trainingtypeid,Trainingtypename,Isactive")] Trainingtype trainingtype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingtype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainingtype);
        }

        // GET: Trainingtypes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Trainingtypes == null)
            {
                return NotFound();
            }

            var trainingtype = await _context.Trainingtypes.FindAsync(id);
            if (trainingtype == null)
            {
                return NotFound();
            }
            return View(trainingtype);
        }

        // POST: Trainingtypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Trainingtypeid,Trainingtypename,Isactive")] Trainingtype trainingtype)
        {
            if (id != trainingtype.Trainingtypeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingtype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingtypeExists(trainingtype.Trainingtypeid))
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
            return View(trainingtype);
        }

        // GET: Trainingtypes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Trainingtypes == null)
            {
                return NotFound();
            }

            var trainingtype = await _context.Trainingtypes
                .FirstOrDefaultAsync(m => m.Trainingtypeid == id);
            if (trainingtype == null)
            {
                return NotFound();
            }

            return View(trainingtype);
        }

        // POST: Trainingtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Trainingtypes == null)
            {
                return Problem("Entity set 'ModelContext.Trainingtypes'  is null.");
            }
            var trainingtype = await _context.Trainingtypes.FindAsync(id);
            if (trainingtype != null)
            {
                _context.Trainingtypes.Remove(trainingtype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingtypeExists(decimal id)
        {
          return (_context.Trainingtypes?.Any(e => e.Trainingtypeid == id)).GetValueOrDefault();
        }
    }
}
