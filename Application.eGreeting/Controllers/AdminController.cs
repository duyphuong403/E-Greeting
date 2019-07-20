using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class AdminController : Controller
    {
        // Manage Card
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View();
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // GET: Admin/ManageCard
        public ActionResult ManageCard()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View(CardDAO.GetAllCard);
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // GET: Admin/ManageUser
        public ActionResult ManageUser()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View(UserDAO.GetAllUser);
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // GET: Admin/ManageFeedback
        public ActionResult ManageFeedback()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View(FeedbackDAO.GetAllFeedback.OrderByDescending(o => o.Id));
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult DeleteFeedback(int id)
        {
            try
            {
                if (Session["username"] != null && Session["role"] != null)
                {
                    if (Session["role"].ToString().ToLower() == "true")
                    {
                        if (id >= 0)
                        {
                            if (FeedbackDAO.Delete(id))
                            {
                                Alert("Delete feedback successfully", NotificationType.success);
                                return RedirectToAction("ManageFeedback");
                            }
                        }
                        return RedirectToAction("ManageFeedback");
                    }
                    Alert("You not permit to access that page", NotificationType.warning);
                    return RedirectToAction("Index", "Home");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("ManageFeedback");
            }
        }


        // GET: Admin/CreateCard
        public ActionResult CreateCard()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View();
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // POST: Admin/CreateCard
        [HttpPost]
        public ActionResult CreateCard(Card newCard, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        if (CheckExtImg(ext))
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var imagePath = Server.MapPath("~/ImageCard/" + fileName);
                            newCard.ImageName = fileName;
                           
                            if (CardDAO.Create(newCard))
                            {
                                file.SaveAs(imagePath);
                                return RedirectToAction("ManageCard");
                            }
                            ModelState.AddModelError("", "Create new card failed.");
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Format image invalid");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please choose Image");
                        return View();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        List<string> ImageExtension = new List<string> { ".png", ".jpg", ".jpeg" };

        bool CheckExtImg(string ext)
        {
            return ImageExtension.Contains(ext.ToLower());
        }

        // GET: Admin/Edit/5
        public ActionResult EditCard(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult EditCard(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
              

        // GET: Admin/Delete/5
        public ActionResult DeleteCard(int id, string ImageName)
        {
            try
            {
                if (Session["username"] != null && Session["role"] != null)
                {
                    if (Session["role"].ToString().ToLower() == "true")
                    {
                        if (id >= 0)
                        {
                            if (CardDAO.DeleteCard(id))
                            {
                                if (ImageName != null)
                                {
                                    string PathImage = Request.MapPath("~/ImageCard/" + ImageName);
                                    if (System.IO.File.Exists(PathImage))
                                    {
                                        System.IO.File.Delete(PathImage);
                                    }
                                }
                            }
                        }
                        return RedirectToAction("ManageCard");
                    }
                    Alert("You not permit to access that page", NotificationType.warning);
                    return RedirectToAction("Index", "Home");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");               
            }
            catch
            {
                return RedirectToAction("ManageCard");
            }
        }


        // Manage User

          // GET: Admin/CreateCard
        public ActionResult CreateUser()
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    return View();
                }
               Alert("You not permit to access this page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You need login to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // POST: Admin/CreateCard
        [HttpPost]
        public ActionResult CreateUser(User newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (newUser.Password != newUser.RePassword)
                    {
                        Alert("RePassword not match!", NotificationType.error);
                        return View();
                    }                   
                    var search = UserDAO.GetUserByUsername(newUser.UserName);
                    if (search == null)
                    {
                        if (UserDAO.CreateUser(newUser))
                        {
                            Alert("Create User successfully!", NotificationType.success);
                            return RedirectToAction("ManageUser");
                        }                        
                    }
                    else
                    {
                        Alert("Username already exist!!", NotificationType.error);
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

        public ActionResult EditUser(int id)
        {
            var edi = UserDAO.GetUser(id);
            return View(edi);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult EditUser(User editU)
        {
            if (ModelState.IsValid)
            {
                UserDAO.EditUser(editU);
                Alert("Edit User successfully!", NotificationType.success);
                return RedirectToAction("ManageUser");
            }
            else
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult DeleteUser(int id)
        {
            if (UserDAO.DeleteUser(id))
            {
                Alert("User has been remove!", NotificationType.success);
                return RedirectToAction("ManageUser");
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
