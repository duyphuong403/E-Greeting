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
        // GET: Home
        public ActionResult Index(string name, int? page) 
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

        public ActionResult CreateTrans(int id)
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (id != 0)
                {
                    var search = CardDAO.GetCard(id);
                    if (search != null)
                    {
                        var model = new Transaction
                        {
                            NameCard = search.NameCard,
                            Username = Session["username"].ToString(),
                            ImageName = search.ImageName,
                            TimeSend = DateTime.Now
                        };
                        return View(model);
                    }
                }
                return RedirectToAction("Index");
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrans(Transaction newTrans)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (TransDAO.CreateTrans(newTrans))
                    {
                        Alert("Send eGreeting card successfully.", NotificationType.success);
                        return RedirectToAction("Index", "Home");
                    }
                    Alert("Send card failed. Please contact your Admin", NotificationType.error);
                }
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
                throw;
            }
        }
    }
}
