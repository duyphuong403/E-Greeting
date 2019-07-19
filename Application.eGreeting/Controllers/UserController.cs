using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.eGreeting.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                return View();
            }
            ModelState.AddModelError("", "You need login to access this page");
            return RedirectToAction("Login", "Home");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            var d = UserDAO.GetUser(id);
            if (d != null)
            {
                return View(d);
            }
            return View("Index");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (newUser.Password != newUser.RePassword)
                    {
                        ModelState.AddModelError("", "RePassword not match.");
                        return View();
                    }
                    //var model = new User
                    //{
                    //    UserName = newUser.UserName
                    //};
                    var search = UserDAO.GetUserByUsername(newUser.UserName);
                    if (search == null)
                    {
                        if (UserDAO.Create(newUser))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username is existed");
                        return View();
                    }
                }
                ModelState.AddModelError("", "Create new User failed .");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            var edi = UserDAO.GetUser(id);
            return View(edi);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User editU)
        {
            if (ModelState.IsValid)
            {
                UserDAO.EditUser(editU);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            if (UserDAO.DeleteUser(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Delete error, cannot find this User!!!";
                return View();
            }
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
