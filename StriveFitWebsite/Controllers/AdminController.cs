using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StriveFitWebsite.Models;

namespace StriveFitWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");

            decimal adminRoleId = 1; 
            decimal trainerRoleId = 2;

            var user = _context.Users.FirstOrDefault(u => u.Userid == 1);
            ViewBag.AdminImage = "~/Images/" + user.Imagepath;

            var memberCount = _context.Userlogins
                                     .Where(ul => ul.Roleid != adminRoleId && ul.Roleid != trainerRoleId)
                                     .Count();

            var activeSubscriptionCount = _context.Subscriptions
               .Where(sub => sub.Paymentstatus == "Paid" && sub.Enddate > DateTime.Now)
               .Count();

            var trainerCount = _context.Userlogins
                .Where(ul => ul.Roleid == trainerRoleId)
                .Count();

            var totalRevenue = _context.Payments
                .Where(p => p.Paymentstatus == "Completed")
                .Sum(p => p.Amount);

            ViewBag.MemberCount = memberCount;
            ViewBag.ActiveSubscriptionCount = activeSubscriptionCount;
            ViewBag.TrainerCount = trainerCount;
            ViewBag.TotalRevenue = totalRevenue;


            var subscriptions = _context.Subscriptions.Include(s => s.User).Include(s => s.Plan).ToList();


            //chart
            var monthlySubscriptions = _context.Subscriptions
                .Where(sub => sub.Startdate.Year == DateTime.Now.Year) 
                .GroupBy(sub => sub.Startdate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToList();


            var annualSubscriptions = _context.Subscriptions
                .GroupBy(sub => sub.Startdate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ToList();


            var years = annualSubscriptions.Select(y => y.Year.ToString()).ToList();
            var yearCounts = annualSubscriptions.Select(y => y.Count).ToList();
            var months = monthlySubscriptions.Select(m => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Month)).ToList();
            var counts = monthlySubscriptions.Select(m => m.Count).ToList();

            ViewBag.Years = years;
            ViewBag.AnnualCounts = yearCounts;
            ViewBag.Months = months;
            ViewBag.MonthlyCounts = counts;

            return View(subscriptions);
        }

        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId"); 

            if (adminId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister"); 
            }

            var admin = _context.Users
                .Where(u => u.Userid == adminId)
                .SingleOrDefault();

            if (admin == null)
            {
                return NotFound(); 
            }

            return View(admin); 
        }

        public IActionResult Edit()
        {
            var userId = HttpContext.Session.GetInt32("AdminId");

            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            var user = _context.Users
                .Where(u => u.Userid == userId)
                .SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Userid,Name,Email,Balance,ImageFile")] User user)
        {
            

            var userId = HttpContext.Session.GetInt32("AdminId");
            if (userId == null || (decimal)userId != user.Userid)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            var existingUser = _context.Users
                .Where(u => u.Userid == user.Userid)
                .SingleOrDefault();

            if (existingUser == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Balance = user.Balance;

            if (user.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;

                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                existingUser.Imagepath = fileName;
                
            }

            _context.Update(existingUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult SearchSubscriptions(DateTime? startDate, DateTime? endDate)
        {
            var allSubscriptions = _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Plan)
                .AsQueryable();

            if (startDate != null)
            {
                allSubscriptions = allSubscriptions.Where(s => s.Startdate >= startDate);
            }
            if (endDate != null)
            {
                allSubscriptions = allSubscriptions.Where(s => s.Startdate <= endDate);
            }

            var subscriptions = allSubscriptions.ToList();

            ViewBag.Months = subscriptions
                .GroupBy(s => s.Startdate.Month)
                .Select(g => g.Key.ToString())
                .ToList();

            ViewBag.MonthlyCounts = subscriptions
                .GroupBy(s => s.Startdate.Month)
                .Select(g => g.Count())
                .ToList();

            ViewBag.Years = subscriptions
                .GroupBy(s => s.Startdate.Year)
                .Select(g => g.Key.ToString())
                .ToList();

            ViewBag.AnnualCounts = subscriptions
                .GroupBy(s => s.Startdate.Year)
                .Select(g => g.Count())
                .ToList();

            return PartialView("_SubscriptionsPartial", subscriptions);
        }


        public IActionResult SubscriptionDetails()
        {
            var subscriptionDetails = _context.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Plan)
                .Include(s => s.User.Schedules)
                .ThenInclude(s => s.Trainer) 
                .Include(s => s.User.WorkoutplanMembers) 
                .Select(s => new
                {
                    MemberName = s.User.Name, 
                    
                    TrainerName = s.User.WorkoutplanMembers
                                    .FirstOrDefault(wp => wp.Scheduleid.HasValue) != null
                                    ? s.User.WorkoutplanMembers
                                        .FirstOrDefault(wp => wp.Scheduleid.HasValue)
                                        .Schedule
                                        .Trainer
                                        .Name
                                    : null, 
                    PlanName = s.Plan.Planname, 
                    Balance = s.User.Balance, 
                    StartDate = s.Startdate,
                    EndDate = s.Enddate 
                })
                .ToList();

            var memberCount = _context.Userlogins
                                     .Where(ul => ul.Roleid != 1)
                                     .Count();                 

            var totalUsers = _context.Users
                .Where(u => u.Isactive == "True" && u.Userlogins.All(l => l.Roleid != 1))
                .Count();

            var enrolledUsers = subscriptionDetails
                .Where(s => s.TrainerName != null) 
                .Count();

            var totalRevenue = _context.Payments
                .Where(p => p.Paymentstatus == "Completed")
                .Sum(p => p.Amount);

            var activeSubscriptionCount = _context.Subscriptions
               .Where(sub => sub.Paymentstatus == "Paid" && sub.Enddate > DateTime.Now)
               .Count();

            ViewBag.TotalUsers = memberCount;
            ViewBag.EnrolledUsers = enrolledUsers;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.ActiveSubscription = activeSubscriptionCount;

            ViewBag.SubscriptionDetails = subscriptionDetails;

            return View(subscriptionDetails);
        }




    }


}


