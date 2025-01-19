using Microsoft.AspNetCore.Mvc;
using StriveFitWebsite.Models;

namespace StriveFitWebsite.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        IWebHostEnvironment _webHostEnvironment;

        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Name,Email,Isactive,Balance,Imagepath,ImageFile")] User users, string password)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + users.ImageFile.FileName;
                string path = Path.Combine(wwwrootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await users.ImageFile.CopyToAsync(fileStream);
                }

                users.Imagepath = fileName;
                users.Balance = 900;

                _context.Add(users);
                await _context.SaveChangesAsync();

                Userlogin userlogin = new Userlogin();
                userlogin.Username = users.Name;
                userlogin.Passwordhash = password;
                userlogin.Userid = users.Userid;
                userlogin.Roleid = 3;
                _context.Add(userlogin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");

            }
            return View(users);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("Username,Passwordhash")] Userlogin userLogin)
        {

            var auth = _context.Userlogins.Where(x => x.Username == userLogin.Username && x.Passwordhash == userLogin.Passwordhash).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.Roleid)
                {
                    case 1://Admin
                        HttpContext.Session.SetString("AdminName", auth.Username);
                        HttpContext.Session.SetInt32("AdminId", (int)auth.Userid);
                        return RedirectToAction("Index", "Admin");

                    case 2://Trainer
                        HttpContext.Session.SetString("MemberName", auth.Username);
                        HttpContext.Session.SetInt32("UserId", (int)auth.Userid);
                        return RedirectToAction("Index", "Home");

                    case 3://Member 
                        HttpContext.Session.SetString("MemberName", auth.Username);
                        HttpContext.Session.SetInt32("UserId", (int)auth.Userid);
                        return RedirectToAction("Index", "Home");

                }

            }
            ViewBag.Message = "Invalid Username or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
