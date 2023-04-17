using Microsoft.AspNetCore.Mvc;
using TextEditor.Models;
namespace TextEditor.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Perform login logic here, e.g. verify credentials, set authentication cookie, etc.

            // Redirect to home page on successful login
            return RedirectToAction("Index", "Home");
        }
    }
}
