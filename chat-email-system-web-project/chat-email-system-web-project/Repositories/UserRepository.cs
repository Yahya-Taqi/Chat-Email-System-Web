using chat_email_system_web_project.Models.SignupLogin;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace chat_email_system_web_project.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        // ✅ Use Dependency Injection for IConfiguration
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChatSystemDB");
        }

        public User? GetUserByEmail(string email)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    return db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
                return null;
            }
        }

        public bool RegisterUser(User user)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                    int rowsAffected = db.Execute(sql, user);

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("✅ User registered successfully!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("❌ No rows were inserted.");
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"🚨 SQL Error: {ex.Message}");
                Console.WriteLine($"🚨 Error Code: {ex.Number}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 General Error: {ex.Message}");
                return false;
            }
        }

    }
}
