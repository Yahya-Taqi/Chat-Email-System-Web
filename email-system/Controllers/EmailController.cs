using Microsoft.AspNetCore.Mvc;
using MufaApp.Models;
using System.Linq;

namespace MufaApp.Controllers
{
    public class EmailRequest
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }

    public class EmailController : Controller
    {
        private readonly AppDbContext _context;

        public EmailController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] EmailRequest request)
        {
            Console.WriteLine($"Raw Request - SenderId: {request.SenderId}, ReceiverId: {request.ReceiverId}, Content: '{request.Content}'");

            // Validate Sender
            var sender = _context.Users.Find(request.SenderId);
            if (sender == null)
            {
                Console.WriteLine($"SenderId {request.SenderId} does not exist in Users table.");
                return BadRequest("Sender does not exist.");
            }

            // Validate Receiver
            var receiver = _context.Users.Find(request.ReceiverId);
            if (receiver == null)
            {
                Console.WriteLine($"ReceiverId {request.ReceiverId} does not exist in Users table.");
                return BadRequest("Receiver does not exist.");
            }

            // Check Content
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Email content cannot be empty.");
            }

            // Create Email Entry
            var email = new Email
            {
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content
            };

            try
            {
                _context.Emails.Add(email);
                _context.SaveChanges();
                Console.WriteLine("Email saved successfully.");
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving email: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpGet]
        public IActionResult GetSentEmails(int userId)
        {
            var sentEmails = _context.Emails
                .Where(e => e.SenderId == userId)
                .ToList();

            return Json(sentEmails);
        }

        [HttpGet]
        public IActionResult GetReceivedEmails(int userId)
        {
            var receivedEmails = _context.Emails
                .Where(e => e.ReceiverId == userId)
                .ToList();

            return Json(receivedEmails);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
