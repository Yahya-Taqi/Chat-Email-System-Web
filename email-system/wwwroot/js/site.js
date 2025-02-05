// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {
    let userId = 1; // Change this dynamically based on logged-in user

    // Function to load received emails
    function loadReceivedEmails() {
        $.get(`/Email/GetReceivedEmails?userId=${userId}`, function (data) {
            $("#inbox-list").empty();
            if (data.length === 0) {
                $("#inbox-list").append("<p>No emails received.</p>");
            } else {
                data.forEach(email => {
                    $("#inbox-list").append(
                        `<div class='email-item'><strong>From:</strong> ${email.senderId}<br><strong>Message:</strong> ${email.content}</div><hr>`
                    );
                });
            }
        });
    }

    // Function to load sent emails
    function loadSentEmails() {
        $.get(`/Email/GetSentEmails?userId=${userId}`, function (data) {
            $("#sent-list").empty();
            if (data.length === 0) {
                $("#sent-list").append("<p>No emails sent.</p>");
            } else {
                data.forEach(email => {
                    $("#sent-list").append(
                        `<div class='email-item'><strong>To:</strong> ${email.receiverId}<br><strong>Message:</strong> ${email.content}</div><hr>`
                    );
                });
            }
        });
    }

    // Load emails when page loads
    loadReceivedEmails();
    loadSentEmails();

    // Send email
    $("#sendEmailForm").submit(function (event) {
        event.preventDefault();

        let receiverId = $("#receiverId").val();
        let content = $("#emailContent").val();

        $.ajax({
            url: "/Email/SendEmail",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ senderId: userId, receiverId: receiverId, content: content }),
            success: function (response) {
                alert(response);
                loadSentEmails();
            },
            error: function (xhr) {
                alert("Error: " + xhr.responseText);
            }
        });
    });
});

// Write your JavaScript code.
