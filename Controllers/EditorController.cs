using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Web;
using TextEditor.Models;
using Microsoft.AspNetCore.Mvc;

namespace TextEditor.Controllers
{
    public class EditorController : Controller
    {
        private IConfiguration _configuration;
        private SqlConnection con;

        public EditorController(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString("Practice"));
        }

        // GET: Document
        [Route("Editor/Index/{userId}")]
        public ActionResult Index(string userId)
        {
            List<DocumentModel> documents = new List<DocumentModel>();

            con.Open();
            string query = "SELECT * FROM Documents where UserId = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@UserId", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DocumentModel document = new DocumentModel();
                document.Id = Convert.ToInt32(reader["Id"]);
                document.Title = reader["Title"].ToString();
                document.Content = reader["Content"].ToString();
                documents.Add(document);
            }
            con.Close();
            ViewBag.documents = documents;
            ViewBag.userId = userId;
            return View();
        }

        // GET: Document/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Document/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentModel document)
        {
            con.Open();
            string query = "INSERT INTO Documents (Title, Content,UserId) VALUES (@Title, @Content,@UserId)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Title", document.Title);
            cmd.Parameters.AddWithValue("@Content", document.Content);
            cmd.Parameters.AddWithValue("@UserId", 1);
            cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", new { userId = 1 });


        }

        // GET: Document/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentModel document = new DocumentModel();

            con.Open();
            string query = "SELECT * FROM Documents WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                document.Id = Convert.ToInt32(reader["Id"]);
                document.Title = reader["Title"].ToString();
                document.Content = reader["Content"].ToString();
                document.UserId = reader["UserId"].ToString();
            }
            else
            {
                return NotFound();
            }
            con.Close();

            return View(document);
        }

        // POST: Document/Edit/5
        [HttpPost]
        public ActionResult Edit(DocumentModel document)
        {
            // Console.WriteLine($"Id: {document.Id}, Title: {document.Title}, Content: {document.Content}, UserId: {document.UserId}");
            try
            {
                con.Open();
                string query = "UPDATE Documents SET Title = @Title, Content = @Content WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", document.Id);
                cmd.Parameters.AddWithValue("@Title", document.Title);
                cmd.Parameters.AddWithValue("@Content", document.Content);
                int res = cmd.ExecuteNonQuery();
                Console.WriteLine(res);
                if (res > 0)
                {
                    Console.WriteLine("Updated");
                    return RedirectToAction("Index", new { userId = document.UserId });
                }
                else
                {
                    Console.WriteLine("Not Updated");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }




            return View(document);
        }

        // GET: Document/Delete/5
        public ActionResult Delete(int? id)
        {

            DocumentModel document = new DocumentModel();

            con.Open();
            string query = "SELECT * FROM Documents WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                document.Id = Convert.ToInt32(reader["Id"]);
                document.Title = reader["Title"].ToString();
                document.Content = reader["Content"].ToString();
                document.UserId = reader["UserId"].ToString();
            }

            con.Close();

            return View(document);
        }


        // POST: Editor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {


            con.Open();
            string query = "DELETE FROM Documents WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            int rowsAffected = cmd.ExecuteNonQuery();

            con.Close();

            if (rowsAffected > 0)
            {
                return RedirectToAction("Index", new { userId = 1 });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }



    }
}