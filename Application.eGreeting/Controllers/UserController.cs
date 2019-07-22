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
        public ActionResult Index(string name)
        {
            if (Session["username"] != null)
            {
                return View();
            }
            Alert("You need login to access this page", NotificationType.warning);
           
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
        public ActionResult Register()
        {
            if (Session["username"] == null)
            {
                return View();
            }
            else
            {
                Alert("You must logout first to register new account!", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (newUser.Password != newUser.RePassword)
                    {
                        Alert("RePassword does not match", NotificationType.error);
                        return View();
                    }
                    var search = UserDAO.GetUserByUsername(newUser.UserName);
                    if (search == null)
                    {
                        if (UserDAO.CreateUser(newUser))
                        {
                            Alert("Register Successfully!!", NotificationType.success);
                            return RedirectToAction("Index", "Home");
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
            if (id > 0)
            {
                var edi = UserDAO.GetUser(id);
                return View(edi);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User editU)
        {
            if (ModelState.IsValid)
            {
                UserDAO.EditUser(editU);
                Alert("Edited successfully!!", NotificationType.error);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public ActionResult ChangePassword(int id)
        {
            if (id >0)
            {
                var changePass = UserDAO.GetUser(id);
                return View(changePass);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(User changePassword)
        {
            if (ModelState.IsValid)
            {
                UserDAO.ChangePassword(changePassword);
                Alert("Change Password successfully!!", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        // GET: User/CreateFeedback
        public ActionResult FeedbackIndex()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                var model = new Feedback
                {
                    Username = Session["username"].ToString(),
                };
                return View(model);
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.DataCreated = DateTime.Now;
                if (FeedbackDAO.Insert(feedback))
                {
                    Alert("Send feedback successfully!", NotificationType.success);
                    return RedirectToAction("FeedbackIndex", "User");
                }
                else
                {
                    Alert("Send feedback failed!", NotificationType.error);
                    return View();
                }
            }
            return View();
        }

        //GET: User/CreateTrans
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

        //POST: User/CreateTrans
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
