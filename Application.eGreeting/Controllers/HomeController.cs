using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Application.eGreeting.Controllers
{
    public class HomeController : Controller
    {
        [HandleError]
        // GET: Home
        public ActionResult Index(int? page) 
        {
            if (page==null)
            {
                page = 1;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
           
            return View(CardDAO.GetAllCard.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Search(string txtSearch, int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (string.IsNullOrEmpty(txtSearch))
            {
                return View("Index", CardDAO.GetAllCard.ToPagedList(pageNumber, pageSize));
            }
            return View("Index", CardDAO.GetCards(txtSearch).ToPagedList(pageNumber, pageSize));
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
                Session["username"] = search.UserName;
                Session["fullname"] = search.FullName;
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
    }
}
