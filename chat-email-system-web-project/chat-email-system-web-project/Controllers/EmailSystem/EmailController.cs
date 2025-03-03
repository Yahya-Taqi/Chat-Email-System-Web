using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System;

namespace chat_email_system_web_project.Controllers.EmailSystem
{
    public class EmailController : Controller
    {
        private readonly string _connectionString;

        // ✅ Correctly retrieve the connection string using the right key
        public EmailController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChatSystemDB");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing or invalid.");
            }
        }

        public IActionResult EmailPage()
        {
            // ✅ Ensure user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open(); // ✅ Ensure the connection is opened

                // ✅ Fetch received messages (Inbox)
                var inboxMessages = connection.Query<MessageModel>(
                    @"SELECT m.Id, u.UserName AS SenderName, m.MessageText, m.Timestamp 
                      FROM Messages m 
                      JOIN Users u ON m.SenderId = u.Id 
                      WHERE m.ReceiverId = @UserId 
                      ORDER BY m.Timestamp DESC",
                    new { UserId = userId });

                // ✅ Fetch sent messages
                var sentMessages = connection.Query<MessageModel>(
                    @"SELECT m.Id, u.UserName AS ReceiverName, m.MessageText, m.Timestamp 
                      FROM Messages m 
                      JOIN Users u ON m.ReceiverId = u.Id 
                      WHERE m.SenderId = @UserId 
                      ORDER BY m.Timestamp DESC",
                    new { UserId = userId });

                ViewData["Inbox"] = inboxMessages;
                ViewData["SentMessages"] = sentMessages;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(string receiverEmail, string messageText)
        {
            var senderId = HttpContext.Session.GetInt32("UserId");
            if (senderId == null)
                return RedirectToAction("Login", "Auth");

            using (var connection = new SqlConnection(_connectionString))
            {
                // ✅ Check if the recipient exists
                var receiver = connection.QueryFirstOrDefault<UserModel>(
                    "SELECT Id FROM Users WHERE Email = @Email", new { Email = receiverEmail });

                if (receiver == null)
                {
                    ViewData["Error"] = "Recipient email is not registered!";
                    return View("EmailPage");
                }

                // ✅ Insert message into database
                connection.Execute(
                    @"INSERT INTO Messages (SenderId, ReceiverId, MessageText) 
                      VALUES (@SenderId, @ReceiverId, @MessageText)",
                    new { SenderId = senderId, ReceiverId = receiver.Id, MessageText = messageText });

                ViewData["Success"] = "Message sent successfully!";
            }

            return RedirectToAction("EmailPage");
        }
    }

    // ✅ Models for data retrieval
    public class UserModel
    {
        public int Id { get; set; }
    }

    public class MessageModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
