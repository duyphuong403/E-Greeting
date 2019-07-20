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
        private static eGreetingDB db = new eGreetingDB();
        // GET: User
        public ActionResult Index(string name)
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Login", "Home");
            }
            var model = db.Users.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                model = model.Where(p => p.UserName.ToUpper().Contains(name)
                                    || p.UserName.ToLower().Contains(name)).ToList();
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "You need login to access this page");
                return View(model);
            }
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

        // GET: User/CreateFeedback
        public ActionResult FeedbackIndex(int id)
        {
            if (Session["username"] != null)
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

        public ActionResult SaveFeedback(Feedback feedback)
        {
            feedback.DataCreated = DateTime.Now;
            if (FeedbackDAO.Insert(feedback))
            {
                Alert("Send feedback successfully!", NotificationType.success);
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                Alert("Send feedback failed!", NotificationType.error);
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
