using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using TextEditor.Models;

namespace TextEditor.Controllers
{
    public class AuthController : Controller
    {

        private IConfiguration _configuration;
        private SqlConnection _connection;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("Practice"));
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                _connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, _connection);

                // Convert StringValues to string
                string username = collection["Username"].ToString();
                string password = collection["Password"].ToString();
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                Console.WriteLine("Username: " + username);
                SqlDataReader reader = cmd.ExecuteReader();
                string userid = "";
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                    userid += reader.GetValue(0);
                }
                return RedirectToAction("Index", "Editor", new { userId = userid });
               
            }
            catch (SqlException ex)
            {
                ViewBag.ErrorMessage = "An error occurred during login";
                return View();
            }
            finally
            {
                _connection.Close();
            }
        }

        [HttpPost]
        public ActionResult Signup(IFormCollection collection)
        {
            try
            {
                _connection.Open();
                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                SqlCommand cmd = new SqlCommand(query, _connection);

                // Convert StringValues to string
                string username = collection["Username"].ToString();
                string password = collection["Password"].ToString();
                string email = collection["Email"].ToString();
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Email", email);

                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to create user";
                    return View();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.ErrorMessage = "An error occurred during signup";
                return View();
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
