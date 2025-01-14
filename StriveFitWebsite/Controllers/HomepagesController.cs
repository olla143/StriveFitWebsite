using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveFitWebsite.Models;

namespace StriveFitWebsite.Controllers
{
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomepagesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Homepages
        public async Task<IActionResult> Index()
        {
              return _context.Homepages != null ? 
                          View(await _context.Homepages.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homepages'  is null.");
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pageid,Sectionname,ImageFile,Headings,Descriptions,Details")] Homepage homepage)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                string path = null;


                if (homepage.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFile.FileName;
                    path = Path.Combine(wwwrootPath + "/Images/", fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.ImageFile.CopyToAsync(fileStream);
                    }
                    homepage.Imageurl = fileName;
                }


                if (string.IsNullOrEmpty(homepage.Imageurl))
                {
                    homepage.Imageurl = null;
                }


                _context.Add(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homepage);
        }

        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage == null)
            {
                return NotFound();
            }
            return View(homepage);
        }

        // POST: Homepages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Pageid,Sectionname,ImageFile,Headings,Descriptions,Details")] Homepage homepage)
        {
            if (id != homepage.Pageid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if a new image file has been uploaded
                    if (homepage.ImageFile != null)
                    {
                        // Delete the old image if a new one is uploaded (optional step)
                        if (!string.IsNullOrEmpty(homepage.Imageurl))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath + "/Images/", homepage.Imageurl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath); // Delete the old image
                            }
                        }

                        // Save the new image
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await homepage.ImageFile.CopyToAsync(fileStream);
                        }
                        homepage.Imageurl = fileName; // Update the Imageurl with the new image name
                    }

                    _context.Update(homepage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepageExists(homepage.Pageid))
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
            return View(homepage);
        }

        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Pageid == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepages == null)
            {
                return Problem("Entity set 'ModelContext.Homepages'  is null.");
            }
            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage != null)
            {
                _context.Homepages.Remove(homepage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
          return (_context.Homepages?.Any(e => e.Pageid == id)).GetValueOrDefault();
        }
    }
}
