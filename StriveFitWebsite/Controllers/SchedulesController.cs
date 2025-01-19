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
    public class SchedulesController : Controller
    {
        private readonly ModelContext _context;

        public SchedulesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public IActionResult Index()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var schedules = _context.Schedules
                .Include(s => s.Trainer)
                .Include(s => s.Training)
                .Where(s => s.Trainerid == userId.Value)
                .ToList();

            return View(schedules);  
        }


        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Trainer)
                .Include(s => s.Training)
                .FirstOrDefaultAsync(m => m.Scheduleid == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            var trainerName = _context.Users
                .Where(u => u.Userid == userId)
                .Select(u => u.Name)
                .FirstOrDefault();

            if (trainerName == null)
            {
                return NotFound();
            }

            ViewBag.TrainerName = trainerName;
            ViewBag.TrainerId = userId;

            ViewBag.Trainingid = new SelectList(
                _context.Trainingtypes.Select(t => new { Trainingtypeid = (decimal)t.Trainingtypeid, t.Trainingtypename }),
                "Trainingtypeid",
                "Trainingtypename"
            );
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleForm form)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    Trainerid = (decimal)userId.Value,
                    Starttime = form.Starttime,
                    Endtime = form.Endtime,
                    Capacity = form.Capacity ?? 1,
                    Goal = form.Goal ?? "N/A",
                    Lectuerstime = form.Lectuerstime,
                    Exercisroutine = form.Exercisroutine,
                    Classtype = form.Classtype,
                    Trainingid = form.Trainingid, 
                    Schedulestatus = "Approved"
                };

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Trainingid = new SelectList(
                _context.Trainingtypes.Select(t => new { Trainingtypeid = (decimal)t.Trainingtypeid, t.Trainingtypename }),
                "Trainingtypeid",
                "Trainingtypename"
            );

            return View(form);
        }



        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", schedule.Trainerid);
            ViewData["Trainingid"] = new SelectList(_context.Trainingtypes, "Trainingtypeid", "Trainingtypeid", schedule.Trainingid);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, ScheduleForm scheduleForm)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    schedule.Starttime = scheduleForm.Starttime;
                    schedule.Endtime = scheduleForm.Endtime;
                    schedule.Capacity = scheduleForm.Capacity;
                    schedule.Goal = scheduleForm.Goal;
                    schedule.Lectuerstime = scheduleForm.Lectuerstime;
                    schedule.Exercisroutine = scheduleForm.Exercisroutine;
                    schedule.Classtype = scheduleForm.Classtype;
                    schedule.Trainingid = scheduleForm.Trainingid;

                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(id))
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

            ViewData["Trainingid"] = new SelectList(_context.Trainingtypes, "Trainingtypeid", "Trainingtypeid", scheduleForm.Trainingid);
            return View(scheduleForm);
        }


        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Trainer)
                .Include(s => s.Training)
                .FirstOrDefaultAsync(m => m.Scheduleid == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'ModelContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(decimal id)
        {
          return (_context.Schedules?.Any(e => e.Scheduleid == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> EnrolledMembers(decimal id)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            var schedule = await _context.Schedules
                .Include(s => s.Trainer)
                .Include(s => s.Training)
                .FirstOrDefaultAsync(m => m.Scheduleid == id);

            if (schedule == null)
            {
                return NotFound();
            }

            var enrolledMembers = _context.Users.Where(e => e.Userid == id).ToList();

            return View(enrolledMembers);
        }

       



    }
}
