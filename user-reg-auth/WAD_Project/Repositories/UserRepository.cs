using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using Dapper;
using WAD_Project.Models;

namespace WAD_Project.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ChatSystemDB"].ConnectionString;

        public User GetUserByEmail(string email)
        {
            using (var db = new SqlConnection(connectionString))
            {
                return db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
            }
        }

        public void RegisterUser(User user)
        {
            using (var db = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                db.Execute(sql, user);
            }
        }
    }
}