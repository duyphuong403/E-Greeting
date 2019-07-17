using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(CardDAO.GetAllCard);
        }
       
        //GET: Home/Login
        public ActionResult Login()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User login)
        {
            var model = new User
            {
                UserName = login.UserName,
                Password = login.Password
            };
            var search = UserDAO.CheckLogin(model);
            if (search != null)
            {
                Session["username"] = search.FullName;
                Session["role"] = search.Role.ToString().ToLower();
                if (Session["role"].ToString() == "true")
                {
                    return RedirectToAction("Index","Admin");
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid account");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        // POST: User/Register
        [HttpPost]
        public ActionResult Register(User regisUser)
        {
            if (ModelState.IsValid)
            {
                if (UserDAO.Create(regisUser))
                {
                    if (Session["username"]!=null)
                    {
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Duplicate ID!!!");
            }
            return View();
        }

    }
}
