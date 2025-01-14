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
    public class ContactmessagesController : Controller
    {
        private readonly ModelContext _context;

        public ContactmessagesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactmessages
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Contactmessages.Include(c => c.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Contactmessages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactmessages == null)
            {
                return NotFound();
            }

            var contactmessage = await _context.Contactmessages
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Messageid == id);
            if (contactmessage == null)
            {
                return NotFound();
            }

            return View(contactmessage);
        }

        // GET: Contactmessages/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Contactmessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Messageid,Userid,Username,Useremail,Message,Submissiondate,Status")] Contactmessage contactmessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactmessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", contactmessage.Userid);
            return View(contactmessage);
        }

        // GET: Contactmessages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contactmessages == null)
            {
                return NotFound();
            }

            var contactmessage = await _context.Contactmessages.FindAsync(id);
            if (contactmessage == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", contactmessage.Userid);
            return View(contactmessage);
        }

        // POST: Contactmessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Messageid,Userid,Username,Useremail,Message,Submissiondate,Status")] Contactmessage contactmessage)
        {
            if (id != contactmessage.Messageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactmessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactmessageExists(contactmessage.Messageid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", contactmessage.Userid);
            return View(contactmessage);
        }

        // GET: Contactmessages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactmessages == null)
            {
                return NotFound();
            }

            var contactmessage = await _context.Contactmessages
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Messageid == id);
            if (contactmessage == null)
            {
                return NotFound();
            }

            return View(contactmessage);
        }

        // POST: Contactmessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactmessages == null)
            {
                return Problem("Entity set 'ModelContext.Contactmessages'  is null.");
            }
            var contactmessage = await _context.Contactmessages.FindAsync(id);
            if (contactmessage != null)
            {
                _context.Contactmessages.Remove(contactmessage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactmessageExists(decimal id)
        {
          return (_context.Contactmessages?.Any(e => e.Messageid == id)).GetValueOrDefault();
        }
    }
}
