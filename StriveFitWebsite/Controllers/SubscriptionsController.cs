using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StriveFitWebsite.Models;


using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Net;
using System.Net.Mail;

namespace StriveFitWebsite.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ModelContext _context;

        private readonly IConfiguration _configuration;
        public SubscriptionsController(ModelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Subscriptions.Include(s => s.Plan).Include(s => s.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Subscriptionid == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["Planid"] = new SelectList(_context.Membershipplans, "Planid", "Planid");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subscriptionid,Userid,Planid,Renewalstatus,Paymentstatus,Startdate,Enddate")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Planid"] = new SelectList(_context.Membershipplans, "Planid", "Planid", subscription.Planid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", subscription.Userid);
            return View(subscription);
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["Planid"] = new SelectList(_context.Membershipplans, "Planid", "Planid", subscription.Planid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", subscription.Userid);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Subscriptionid,Userid,Planid,Renewalstatus,Paymentstatus,Startdate,Enddate")] Subscription subscription)
        {
            if (id != subscription.Subscriptionid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.Subscriptionid))
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
            ViewData["Planid"] = new SelectList(_context.Membershipplans, "Planid", "Planid", subscription.Planid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", subscription.Userid);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Subscriptions == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.Plan)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Subscriptionid == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Subscriptions == null)
            {
                return Problem("Entity set 'ModelContext.Subscriptions'  is null.");
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(decimal id)
        {
            return (_context.Subscriptions?.Any(e => e.Subscriptionid == id)).GetValueOrDefault();
        }

        public IActionResult CheckPlan(decimal price, string planname)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "LoginAndRegister"); 
            }

            
            var user = _context.Users.SingleOrDefault(u => u.Userid == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "LoginAndRegister"); 
            }

            var activeSubscription = _context.Subscriptions
              .SingleOrDefault(s => s.Userid == user.Userid && s.Enddate > DateTime.Now);

            if (activeSubscription != null)
            {
                TempData["Message"] = "You cannot choose another subscription until your current subscription ends.";
                return RedirectToAction("Index", "Home");
            }

            
            if (user.Balance == null || user.Balance < price)
            {
                TempData["Message"] = "You do not have enough balance to subscribe to this plan.";
                return RedirectToAction("Index", "Home"); 
            }

            
            user.Balance -= price;
            var plan = _context.Membershipplans
                    .AsEnumerable() 
                    .SingleOrDefault(p => p.Planname.Trim().Equals(planname.Trim(), StringComparison.OrdinalIgnoreCase)); if (plan == null)
            {
                TempData["Message"] = "The selected plan does not exist.";
                return RedirectToAction("Index", "Home"); 
            }

            
            var subscription = new Subscription
            {
                Userid = user.Userid,
                Planid = plan.Planid,
                Startdate = DateTime.Now,
                Enddate = DateTime.Now.AddMonths((int)plan.Durationmonths),
                Paymentstatus = "Paid",
                Renewalstatus = "Active"
            };

            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            var payment = new Payment
            {
                Subscriptionid = subscription.Subscriptionid,
                Amount = plan.Price,
                Paymentdate = DateTime.Now,
                Paymentmethod = "Paypal",
                Paymentstatus = "Completed"
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            if (payment.Paymentstatus == "Completed")
            {
                string content = $"Dear {user.Name},\n\n" +
                    $"Your payment has been successfully completed for the {subscription.Plan.Planname} subscription on StriveFit.\n\n" +
                    "Thank you for choosing us!\n\n" +
                    "Best Regards,\nStriveFit Team";

                
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Content().Column(column =>
                        {
                            column.Item().Text("Payment Confirmation").FontSize(20).Bold().AlignCenter();
                            column.Item().Text(content).FontSize(12);
                        });
                    });
                });

                using (var memoryStream = new MemoryStream())
                {
                    document.GeneratePdf(memoryStream);
                    var pdfBytes = memoryStream.ToArray();

                    var smtpServer = _configuration["EmailSettings:SMTPServer"];
                    var smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                    var senderEmail = _configuration["EmailSettings:SenderEmail"];
                    var senderPassword = _configuration["EmailSettings:SenderPassword"];
                    try
                    {
                        using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtpClient.EnableSsl = true;

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(senderEmail),
                            Subject = "Payment Confirmation",
                            Body = $"Dear {user.Name},\n\nYour payment for {subscription.Plan.Planname} has been confirmed.\n\nBest Regards,\nStriveFit Team",
                            IsBodyHtml = false
                        };

                        mailMessage.To.Add(user.Email);
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "PaymentConfirmation.pdf"));

                        smtpClient.Send(mailMessage);

                    }
                }
                    catch (Exception ex)
                    {
                    TempData["Message"] = $"Payment successful, but the email could not be sent. Error: {ex.Message}";
                }
                TempData["Message"] = "Payment completed and confirmation email sent.";
                    return File(pdfBytes, "application/pdf", "PaymentConfirmation.pdf");
                }

                
            }
            TempData["Message"] = "You have successfully subscribed to the plan.";
            return RedirectToAction("Index", "Home");
        }

    }
}
