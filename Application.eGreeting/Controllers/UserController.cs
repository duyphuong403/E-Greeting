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
                            Session["username"] = newUser.UserName;
                            Session["fullname"] = newUser.FullName;
                            Session["role"] = newUser.Role;
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
                Alert("Edited successfully!!", NotificationType.success);
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
        public ActionResult FeedbackIndex(Feedback feedback)
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
                var searchUser = UserDAO.GetUser(id);
                var searchPayment = PaymentDAO.GetPaymentByUsername(Session["username"].ToString());
                if (searchPayment != null)
                {
                    if (searchUser != null && !searchPayment.IsActive)
                    {
                        Alert("Your Info Payment not active. Please contact Administrator", NotificationType.error);
                        return RedirectToAction("FeedbackIndex");
                    }
                    else
                    {
                        var searchCard = CardDAO.GetCard(id);
                        if (searchCard != null)
                        {
                            var model = new Transaction
                            {
                                NameCard = searchCard.NameCard,
                                Username = Session["username"].ToString(),
                                ImageNameTrans = searchCard.ImageName,
                                //TimeSend = DateTime.Now
                            };
                            return View(model);
                        }
                        return RedirectToAction("Index");
                    }
                }
                Alert("Please purchase to use this feature. Thanks", NotificationType.info);
                return RedirectToAction("DescriptionPayment");
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/CreateTrans
        [HttpPost]
        public ActionResult CreateTrans(Transaction newTrans)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newTrans.TimeSend = DateTime.UtcNow;
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

        //GET: User/DescriptionPayment
        public ActionResult DescriptionPayment()
        {
            return View();
        }


        //GET: User/Payment
        public ActionResult Payment()
        {
            if (IsLoggedIn())
            {
                var searchUser = UserDAO.GetUserByUsername(Session["username"].ToString());
                var model = new PaymentInfo
                {
                    UserId = searchUser.UserId,
                    UserName = searchUser.UserName,
                };
                return View(model);
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/Payment
        [HttpPost]
        public ActionResult Payment(PaymentInfo addPayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    addPayment.DateCreated = DateTime.Now;
                    if (addPayment.DateExpire > addPayment.DateCreated)
                    {
                        if (PaymentDAO.CreatePayment(addPayment))
                        {
                            Alert("Register Payment Account Successfully. Please wait us active your payment. Thanks. ", NotificationType.success);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    Alert("Expire Date must be date in future", NotificationType.error);
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

        //==================================================== Subscribes ===========================================================================

        //GET: //User/SubscribeSend
        public ActionResult SubscribeSend()
        {
            if (IsLoggedIn())
            {
                return View();
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/AddEmailList
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmailList(EmailList addEmail)
        {
            if (addEmail.ListEmail != null)
            {
                string[] email = addEmail.ListEmail.Split('\n');
                if (email.Length < 10)
                {
                    Alert("You must be input at least 10 emails to send card.", NotificationType.error);
                    return RedirectToAction("SubscribeSend");
                }
                if (Session["username"] != null)
                {
                    addEmail.Username = Session["username"].ToString().ToLower();
                    if (EmailListDAO.Create(addEmail))
                    {
                        Alert("You're Subscribe Send successfully", NotificationType.success);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            Alert("Please do not empty fields", NotificationType.error);
            return RedirectToAction("SubscribeSend");
        }

        //GET: User/SubscribeReceive
        public ActionResult SubscribeReceive()
        {
            if (IsLoggedIn())
            {
                var searchUser = UserDAO.GetUserByUsername(Session["username"].ToString().ToLower());
                if (searchUser != null)
                {                   
                   return View(searchUser);
                }
                Alert("Not found this Username", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/SubscribeReceive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubscribeReceive(User editUser)
        {
            var search = PaymentDAO.GetPaymentByUsername(Session["username"].ToString().ToLower());
            if (search != null)
            {
                if (search.IsActive)
                {
                    var searchUser = UserDAO.GetUser(search.UserId);
                    if (searchUser != null)
                    {                        
                        searchUser.IsSubcribeReceive = true;
                        if (UserDAO.UpdateSubscribeReceive(searchUser))
                        {
                            Alert("Subscribe Daily Receive New Card Successfully", NotificationType.success);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    Alert("Not found Username", NotificationType.error);
                    return RedirectToAction("SubscribeReceive");
                }
                Alert("Sorry Your Payment not activate. Please contact Administrator", NotificationType.error);
                return RedirectToAction("FeedbackIndex");
            }
            Alert("Sorry You not Register Payment Info. Please Register Payment Info", NotificationType.error);
            return RedirectToAction("Payment");
        }

        //POST: User/UnSubscribeReceive
        public ActionResult UnSubscribeReceive(User editUser)
        {
            var search = UserDAO.GetUser(editUser.UserId);
            if (search != null)
            {
                search.IsSubcribeReceive = false;
                if (UserDAO.UpdateSubscribeReceive(search))
                {
                    Alert("UnSubscribe Daily Receive New Card Successfully", NotificationType.success);
                    return RedirectToAction("Index", "Home");
                }
                Alert("UnSubscribe Daily Receive New Card Failed", NotificationType.error);
                return RedirectToAction("SubscribeReceive");
            }
            Alert("Not found Username", NotificationType.error);
            return RedirectToAction("SubscribeReceive");
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
