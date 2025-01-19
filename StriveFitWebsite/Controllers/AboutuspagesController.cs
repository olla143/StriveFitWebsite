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
    public class AboutuspagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AboutuspagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Aboutuspages
        public async Task<IActionResult> Index()
        {
              return _context.Aboutuspages != null ? 
                          View(await _context.Aboutuspages.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Aboutuspages'  is null.");
        }

        // GET: Aboutuspages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // GET: Aboutuspages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutuspages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pageid,Title,Subheading,Descriptions,ImageFile")] Aboutuspage aboutuspage)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                string path = null;

                
                if (aboutuspage.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFile.FileName;
                    path = Path.Combine(wwwrootPath + "/Images/", fileName);

                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutuspage.ImageFile.CopyToAsync(fileStream);
                    }
                    aboutuspage.Imageurl = fileName;
                }

                
                if (string.IsNullOrEmpty(aboutuspage.Imageurl))
                {
                    aboutuspage.Imageurl = null; 
                }

                _context.Add(aboutuspage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutuspage);
        }

        // GET: Aboutuspages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage == null)
            {
                return NotFound();
            }
            return View(aboutuspage);
        }

        // POST: Aboutuspages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Pageid,Title,Subheading,Descriptions,ImageFile")] Aboutuspage aboutuspage)
        {
            if (id != aboutuspage.Pageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                        if (aboutuspage.ImageFile != null)
                        {
                            if (!string.IsNullOrEmpty(aboutuspage.Imageurl))
                            {
                                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath + "/Images/", aboutuspage.Imageurl);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath); 
                                }
                            }

                            string wwwRootPath = _webHostEnvironment.WebRootPath;
                            string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFile.FileName;
                            string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await aboutuspage.ImageFile.CopyToAsync(fileStream);
                            }
                            aboutuspage.Imageurl = fileName; 
                        }
                        _context.Update(aboutuspage);
                        await _context.SaveChangesAsync();
                    
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuspageExists(aboutuspage.Pageid))
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
            return View(aboutuspage);
        }

        // GET: Aboutuspages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // POST: Aboutuspages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutuspages == null)
            {
                return Problem("Entity set 'ModelContext.Aboutuspages'  is null.");
            }
            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage != null)
            {
                _context.Aboutuspages.Remove(aboutuspage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutuspageExists(decimal id)
        {
          return (_context.Aboutuspages?.Any(e => e.Pageid == id)).GetValueOrDefault();
        }
    }
}
