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
                Alert("Invalid Account", NotificationType.error);
            }
            return View();
        }

        //GET: Home/Logout
        public ActionResult Logout()
        {
            if (Session["username"] != null)
            {
                Session["username"] = null;
                Session["role"] = null;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User regisUser)
        {
            if (ModelState.IsValid)
            {
                if (UserDAO.Create(regisUser))
                {
                    //    if (Session["username"]!=null)
                    //    {
                    //        return View();
                    //    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Duplicate ID!!!");
            }
            return View();
        }


        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }

        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }

        public ActionResult SelectCard(int id)
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (id != 0)
                {
                    var search = CardDAO.GetCard(id);
                    if (search != null)
                    {
                        return View(search);
                    }
                }
                return RedirectToAction("Index");
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }
    }
}
