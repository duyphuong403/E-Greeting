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
            if (IsLoggedIn())
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
            if (IsLoggedIn())
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
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User editU)
        {
            var search = UserDAO.GetUser(editU.UserId);
            if (ModelState.IsValid)
            {
                UserDAO.UserEditUser(editU);
                Alert("Edited successfully!!", NotificationType.success);
                return RedirectToAction("Index");
            }
            else
            {
                if (search != null)
                {
                    return View(search);
                }
                Alert("Cannot get User", NotificationType.error);
                return RedirectToAction("Index");
            }
        }

        //Phuc
        public ActionResult EditPaymentInfo()
        {
            if (IsLoggedIn())
            {
                var findPaymentInfo = PaymentDAO.GetPaymentByUsername(Session["username"].ToString());
                if (findPaymentInfo == null)
                {
                    Alert("Your Payment Info are not registered!!", NotificationType.warning);
                    return RedirectToAction("Index");
                }
                return View(findPaymentInfo);
            }
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaymentInfo(PaymentInfo edit)
        {
            if (ModelState.IsValid)
            {

                if (PaymentDAO.EditPaymentInfo(edit))
                {
                    Alert("Edited successfully!!", NotificationType.success);
                    var search = PaymentDAO.GetPayment(edit.PayId);
                    if (search != null)
                    {
                        return View(search);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    Alert("Edited failed!!", NotificationType.error);
                    return View();
                }
            }
            else
            {
                Alert("Edited failed!!", NotificationType.error);
                return View();
            }
        }
        // End Phuc

        public ActionResult ChangePassword(int id)
        {
            if (IsLoggedIn())
            {
                if (id > 0)
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
            Alert("You need Log in to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                if (changePassword.NewPassword != changePassword.ConfirmNewPassword)
                {
                    Alert("Confirm New Password not match.", NotificationType.error);
                    return View();
                }
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
            if (IsLoggedIn())
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
                    return RedirectToAction("Index", "Home");
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
                var Username = Session["username"].ToString();
                var searchUser = UserDAO.GetUserByUsername(Username);
                var searchPayment = PaymentDAO.GetPaymentByUsername(Username);
                if (searchPayment != null)
                {
                    if (searchUser == null)
                    {
                        Alert("You not Register Email List", NotificationType.error);
                        return RedirectToAction("SubscribeSend");
                    }
                    if (!searchPayment.IsActive)
                    {
                        Alert("Your Info Payment is not activated. Please contact Administrator by send feedback.", NotificationType.error);
                        return RedirectToAction("FeedbackIndex");
                    }
                    else
                    {
                        var searchCard = CardDAO.GetCard(id);
                        var searchEmailList = EmailListDAO.GetEmailListByUsername(searchUser.UserName);
                        if (searchEmailList == null)
                        {
                            Alert("You not register email list to send Card. Please click Subscribe Send Card to register email list.", NotificationType.error);
                            return RedirectToAction("Index");
                        }
                        if (searchCard != null)
                        {
                            var model = new Transaction
                            {
                                NameCard = searchCard.NameCard,
                                Username = Session["username"].ToString(),
                                ImageNameTrans = searchCard.ImageName,
                                Receiver = searchEmailList.ListEmail
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
        [ValidateAntiForgeryToken]
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
                    Alert("Send card failed. Please contact Administrator!", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
                return View(newTrans);
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
        [ValidateAntiForgeryToken]
        public ActionResult Payment(PaymentInfo addPayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //addPayment.DateCreated = DateTime.Now;
                    if (addPayment.DateExpire > DateTime.Now)
                    {
                        if (PaymentDAO.CreatePayment(addPayment))
                        {
                            Alert("Register payment account successfully. Please wait until we activate your payment. Thank you! ", NotificationType.success);
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
                var username = Session["username"].ToString();
                var searchPayment = PaymentDAO.GetPaymentByUsername(username);
                if (searchPayment == null)
                {
                    Alert("You must register Payment Info to use this feature.", NotificationType.error);
                    return RedirectToAction("Payment", "User");
                }
                var search = UserDAO.GetUserByUsername(username);
                if (search.IsSubcribeSend)
                {
                    return RedirectToAction("ChangeSubscribeSend");
                }
                if (!searchPayment.IsActive)
                {
                    Alert("You Payment Not Activate. Please Contact Administrator. Thank you.", NotificationType.error);
                    return RedirectToAction("FeedbackIndex", "User");
                }
                return View();
            }
            Alert("You need Log in to access this page!", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //GET: User/ChangeSubscribeSend/5
        public ActionResult ChangeSubscribeSend()
        {
            if (IsLoggedIn())
            {
                var search = UserDAO.GetUserByUsername(Session["username"].ToString());
                if (search != null)
                {
                    return View(search);
                }
                return RedirectToAction("Index", "Home");
            }
            Alert("You need Log in to access this page!", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/ChangeSubscribeSend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSubscribeSend(User search)
        {
            if (search != null)
            {
                if (UserDAO.UpdateSubscribeSend(search))
                {
                    var searchEmailList = EmailListDAO.GetEmailListByUsername(search.UserName);
                    if (searchEmailList != null)
                    {
                        if (EmailListDAO.Delete(searchEmailList.EmailId))
                        {
                            Alert("You are Unsubscribe Send Card successfully", NotificationType.success);
                            return RedirectToAction("Index", "Home");
                        }
                        Alert("Remove Email List failed", NotificationType.error);
                        return RedirectToAction("Index", "Home");
                    }
                    Alert("Not found Email List", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }
            Alert("Unsubscribe Send Card failed", NotificationType.error);
            return RedirectToAction("Index", "Home");
        }

        //GET: User/EditSubscribe/5
        public ActionResult EditSubscribe(int id)
        {
            if (IsLoggedIn())
            {
                if (id > 0)
                {
                    var search = UserDAO.GetUser(id);
                    if (search != null)
                    {
                        return View(search);
                    }
                    Alert("Not found User", NotificationType.error);
                    return RedirectToAction("Index");
                }
                Alert("UserId invalid", NotificationType.error);
                return RedirectToAction("Index");
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
                if (email.Length < 10 || email.Length > 20)
                {
                    Alert("You only input from 10 to 20 emails", NotificationType.error);
                    return RedirectToAction("SubscribeSend");
                }
                if (Session["username"] != null)
                {
                    addEmail.Username = Session["username"].ToString().ToLower();
                    var search = EmailListDAO.SearchEmailListByUsername(addEmail.Username);
                    if (search.Count == 0)
                    {
                        if (EmailListDAO.Create(addEmail))
                        {
                            var model = new User
                            {
                                UserName = addEmail.Username,
                                IsSubcribeSend = true
                            };
                            if (UserDAO.UpdateSubscribeSend(model))
                            {
                                Alert("You are Subscribe Send successfully", NotificationType.success);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                EmailListDAO.Delete(addEmail.EmailId);
                                Alert("Subscribe Send Failed. Please contact Administrator", NotificationType.error);
                                return RedirectToAction("SubscribeSend");
                            }
                        }
                    }
                    Alert("You has already register Email List", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }
            Alert("Please do not empty fields", NotificationType.error);
            return RedirectToAction("SubscribeSend");
        }

        //GET: User/EditEmailList/5
        public ActionResult EditEmailList(string username)
        {
            if (IsLoggedIn())
            {
                if (username != null)
                {
                    var search = EmailListDAO.GetEmailListByUsername(username);
                    if (search != null)
                    {
                        return View(search);
                    }
                    Alert("Not found Email List with this Username", NotificationType.error);
                    return RedirectToAction("Index");
                }
                Alert("Username was null", NotificationType.error);
                return View();
            }
            Alert("You need Log in to access this page!", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //POST: User/EditEmailList
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmailList(EmailList editEmailList)
        {
            if (editEmailList != null)
            {
                var search = EmailListDAO.GetEmailList(editEmailList.EmailId);
                if (search != null)
                {
                    editEmailList.ListEmail.Trim();
                    string[] ListEmail = editEmailList.ListEmail.Split('\n');
                    if (ListEmail.Length < 10 || ListEmail.Length > 20)
                    {
                        Alert("You must be enter from 10 to 20 emails.", NotificationType.error);
                        return View(search);
                    }
                    if (editEmailList == null)
                    {
                        if (EmailListDAO.Delete(editEmailList.EmailId))
                        {
                            Alert("Remove email list successfully", NotificationType.success);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            Alert("Remove email list failed", NotificationType.error);
                            return View(search);
                        }
                    }
                    else
                    {
                        editEmailList.ListEmail = editEmailList.ListEmail.ToString();
                        if (EmailListDAO.Edit(editEmailList))
                        {
                            Alert("Update email list successfully", NotificationType.success);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            Alert("Update email list failed", NotificationType.error);
                            return View(search);
                        }
                    }
                }
            }
            Alert("Model was null. Please try again", NotificationType.error);
            return RedirectToAction("Index");
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
                Alert("Your Payment is not activated. Please contact Administrator by send feedback.", NotificationType.error);
                return RedirectToAction("FeedbackIndex");
            }
            Alert("Sorry You not register Payment Information. Please register first!", NotificationType.error);
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
                    Alert("UnSubscribe daily receive new Cards successfully", NotificationType.success);
                    return RedirectToAction("Index", "Home");
                }
                Alert("UnSubscribe daily receive new Card failed", NotificationType.error);
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
