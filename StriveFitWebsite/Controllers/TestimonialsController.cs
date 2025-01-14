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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Member);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialid,Memberid,Content,Status,Submitteddate,Rating")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Memberid);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Memberid);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Memberid,Content,Status,Submitteddate,Rating")] Testimonial testimonial)
        {
            if (id != testimonial.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
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
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Memberid);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult SubmitFeedback(string Content, int Rating)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            var testimonial = new Testimonial
            {
                Memberid = Convert.ToDecimal(userId),
                Content = Content,
                Submitteddate = DateTime.Now,
                Status = "Pending",
                Rating = Rating  // Save the Rating from the form
            };

            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();

            TempData["Message"] = "Your feedback has been submitted and is awaiting approval.";
            return RedirectToAction("About", "Home");
        }

        [HttpPost]
        public IActionResult ApproveTestimonial(decimal testimonialId)
        {
            var testimonial = _context.Testimonials.Find(testimonialId);
            if (testimonial != null)
            {
                testimonial.Status = "Approved"; // Change the status to "Approved"
                _context.SaveChanges();
            }

            return RedirectToAction("ManageTestimonials"); // Redirect to a page where all testimonials are listed
        }

        [HttpPost]
        public IActionResult RejectTestimonial(decimal testimonialId)
        {
            var testimonial = _context.Testimonials.Find(testimonialId);
            if (testimonial != null)
            {
                testimonial.Status = "Rejected"; // Change the status to "Rejected"
                _context.SaveChanges();
            }

            return RedirectToAction("ManageTestimonials"); // Redirect to a page where all testimonials are listed
        }

        public IActionResult ManageTestimonials()
        {
            var testimonials = _context.Testimonials.Include(t => t.Member).ToList();
            return View(testimonials);
        }
    }
}
