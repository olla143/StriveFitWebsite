using System.Diagnostics;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StriveFitWebsite.Models;
using StriveFitWebsite.Models.ViewModels;

namespace StriveFitWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {

            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;
            var homeContent = _context.Homepages.ToList();
            var plan = _context.Membershipplans.ToList();
            var viewModel = new HomePageViewModel
            {
                Homepages = homeContent,
                plans = plan
                .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {

            var aboutUsContent = _context.Aboutuspages.ToList();

            // Fetch testimonials
            var testimonials = _context.Testimonials
                .Include(t => t.Member)
                .ToList();


            var trainers = _context.Userlogins
               .Include(u => u.User)
               .Where(u => u.Role.Rolename == "Trainer") // Adjust RoleName to match your database value
               .Select(u => new
               {
                   u.User.Name,
                   u.User.Imagepath
               })
               .ToList();

            // Combine into ViewModel
            var viewModel = new AboutPageViewModel
            {
                AboutusPage = aboutUsContent,
                Testimonials = testimonials,
                Trainers = trainers.Select(t => new TrainerViewModel
                {
                    Name = t.Name,
                    ImagePath = t.Imagepath
                }).ToList()
            };

            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            return View(viewModel);
        }

        public IActionResult Contact()
        {
            var contactContent = _context.Contactuspages.ToList();
            var viewModel = new ContactPageViewModel
            {
                Contactpages = contactContent                
                .ToList()
            };

            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;
            return View(viewModel);
        }

        public IActionResult Schedule()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Redirect to login if user is not logged in
                return RedirectToAction("Login", "LoginAndRegister");
            }

            var user = _context.Userlogins.Include(u => u.Role)
                        .FirstOrDefault(u => u.Userid == userId.Value);

            if (user == null)
            {
                // Handle case where user doesn't exist
                return RedirectToAction("Index", "Home");
            }

            if (user.Role.Rolename == "Trainer")
            {
                // Trainer: Allow them to create a schedule
                return RedirectToAction("Index", "Schedules");
            }
            else if (user.Role.Rolename == "Member")
            {
                var schedules = _context.Schedules
                    .Include(s => s.Trainer)  
                    .Select(s => new ScheduleViewModel
                    {
                        ScheduleId = s.Scheduleid,
                        Starttime = s.Starttime,
                        Endtime = s.Endtime,
                        Classtype = s.Classtype,
                        Exercisroutine = s.Exercisroutine,                        
                        TrainerName = s.Trainer.Name 
                        })
                        .ToList();
                // Member: Optionally show a message or redirect them to another page
                return View(schedules);
            }

            

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Profile()
        {
            ViewBag.Profile = HttpContext.Session.GetString("MemberName");
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;
            // Check if the session is active
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            // Retrieve user information
            var user = _context.Users
                .Where(u => u.Userid == userId)
                .SingleOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            return View(user); // Pass the user data to the Profile view

        }


        public IActionResult Edit(decimal id)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;
            // Check if the user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId != id)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            // Retrieve user information for editing
            var user = _context.Users
                .Where(u => u.Userid == id)
                .SingleOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            return View(user); // Pass the user data to the Edit view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Userid,Name,Email,Balance,ImageFile")] User user)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserId") != null;

            // Check if the user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId != user.Userid)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            // Find the user in the database
            var existingUser = _context.Users
                .Where(u => u.Userid == user.Userid)
                .SingleOrDefault();

            if (existingUser == null)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            // Update user data
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Balance = user.Balance;

            // Check if the user uploaded a new image
            if (user.ImageFile != null)
            {
                // Get the path to the web root (wwwroot)
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Create a unique file name for the uploaded image
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;

                // Combine the file name with the path to the Images folder
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                // Save the uploaded file to the server
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(fileStream);
                }

                // Update the user's image path in the database
                existingUser.Imagepath = fileName;
            }

            // Save the changes to the database
            _context.Update(existingUser);
            await _context.SaveChangesAsync();

            // Redirect to the profile page after the update
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult Enroll(decimal scheduleId)
        {
            
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            
            var subscription = _context.Subscriptions
                .FirstOrDefault(s => s.Userid == userId && s.Enddate >= DateTime.Now);

            if (subscription == null)
            {
                TempData["Message"] = "You do not have a valid subscription.";
                return RedirectToAction("Schedule");
            }

          
            var schedule = _context.Schedules
                .Include(s => s.Trainer)
                .FirstOrDefault(s => s.Scheduleid == scheduleId);

            if (schedule == null)
            {
                TempData["Message"] = "Schedule not found.";
                return RedirectToAction("Schedule");
            }

            var existingEnrollment = _context.Workoutplans
        .FirstOrDefault(w => w.Scheduleid == scheduleId && w.Memberid == userId.Value);

            if (existingEnrollment != null)
            {
                TempData["Message"] = "You are already enrolled in this schedule.";
                return RedirectToAction("Schedule");
            }


            var workoutPlan = new Workoutplan
            {
                Scheduleid = scheduleId,
                Memberid = userId.Value,
                Trainerid = schedule.Trainerid
            };

            _context.Workoutplans.Add(workoutPlan);
            _context.SaveChanges();

            TempData["Message"] = "You have successfully enrolled in the workout plan.";

            return RedirectToAction("Schedule");
        }




    }
}
