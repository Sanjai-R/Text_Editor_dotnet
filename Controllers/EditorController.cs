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
        // GET: Document
        public ActionResult Index()
        {
            List<DocumentModel> documents = new List<DocumentModel>();
            using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                string query = "SELECT * FROM Documents";
                SqlCommand cmd = new SqlCommand(query, con);
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
            }
            return View(documents);
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
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
                {
                    con.Open();
                    string query = "INSERT INTO Documents (Title, Content) VALUES (@Title, @Content)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Title", document.Title);
                    cmd.Parameters.AddWithValue("@Content", document.Content);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: Document/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentModel document = new DocumentModel();
            using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
            {
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
                }
                else
                {
                    return NotFound();
                }
                con.Close();
            }
            return View(document);
        }

        // POST: Document/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentModel document)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
                {
                    con.Open();
                    string query = "UPDATE Documents SET Title = @Title, Content = @Content WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", document.Id);
                    cmd.Parameters.AddWithValue("@Title", document.Title);
                    cmd.Parameters.AddWithValue("@Content", document.Content);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: Document/Delete/5
        public ActionResult Delete(int? id)
        {

            DocumentModel document = new DocumentModel();
            using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
            {
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
                }

                con.Close();
            }
            return View(document);
        }

        // POST: Editor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection("Data Source=5CG9445SKD;Initial Catalog=Document;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                string query = "DELETE FROM Documents WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }

    }
}