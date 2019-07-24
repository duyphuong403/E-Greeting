using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class UserController : BaseController
    {
        
        // GET: User
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                var result = UserDAO.GetUserByUsername(Session["username"].ToString());
                return View(result);
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
                var search = UserDAO.GetUser(id);
                var model = new ChangePassword
                {
                    UserId = id,
                    OldPassword = search.Password,
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                var searchUser = UserDAO.GetUser(changePassword.UserId);
                if (searchUser != null)
                {
                    if (searchUser.Password == changePassword.OldPassword)
                    {
                        var model = new User
                        {
                            UserId = changePassword.UserId,
                            Password = changePassword.NewPassword,
                        };
                        UserDAO.ChangePassword(model);

                        Alert("Change Password successfully!!", NotificationType.success);
                        return RedirectToAction("Index");
                    }
                    Alert("Old Password invalid.", NotificationType.warning);
                    return View();
                }
                Alert("Not found this user.", NotificationType.error);
                return View();
            }
            else
            {
                return View();
            }
        }


        // GET: User/CreateFeedback
        public ActionResult FeedbackIndex()
        {
            if (Session["username"] != null)
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
            if (IsLoggedIn())
            {
                var result = UserDAO.GetUser(id);
                if (result != null && result.IsVIP)
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
                    return RedirectToAction("Index");
                }
                Alert("Please Register Information to purchase to use this feature. Thanks", NotificationType.info);
                return RedirectToAction("Payment");
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

        //GET: User/SubscribeSend
        public ActionResult SubscribeSend()
        {
            // Check login or not ?
            if (Session["username"] != null)
            {
                var search = UserDAO.GetUserByUsername(Session["username"].ToString());
                // get info user
                if (search != null)
                {
                    // check user purchase or not ?
                    if (search.IsSubcribeSend)
                    {

                    }
                }
                var model = new Feedback
                {
                    Username = Session["username"].ToString(),
                };
                return View(model);
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }


        //GET: User/Payment
        public ActionResult Payment()
        {
            if (IsLoggedIn())
            {
                var search = UserDAO.GetUserByUsername(Session["username"].ToString());
                var model = new PaymentInfo
                {
                    UserName = search.UserName,
                };
                return View(model);
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
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
