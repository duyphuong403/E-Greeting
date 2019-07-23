using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Application.eGreeting.Controllers
{
    public class AdminController : BaseController
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
        public ActionResult ManageCard(int? page)
        {

            {
                if (Session["username"] != null && Session["role"] != null)
                {
                    if (Session["role"].ToString().ToLower() == "true")
                    {
                        if (page == null)
                        {
                            page = 1;
                        }
                        int pageSize = 3;
                        int pageNumber = (page ?? 1);

                        return View(CardDAO.GetAllCard.ToPagedList(pageNumber, pageSize));
                    }
                    Alert("You not permit to access that page", NotificationType.warning);
                    return RedirectToAction("Index", "Home");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Admin/ManageUser
        public ActionResult ManageUser(int? page)
        {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    if (page == null)
                    {
                        page = 1;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);

                    return View(UserDAO.GetAllUser.ToPagedList(pageNumber, pageSize));
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        //
        [HttpPost]
        public bool InsertFeedback(Feedback model)
        {
            try
            {
                return FeedbackDAO.Insert(model);
            }
            catch (Exception)
            {
                return false;
            }

        }

        [HttpPost]
        public bool UpdateFeedback(FeedbackModel model) {
            
            try
            {
                Feedback item = FeedbackDAO.GetById(model.Id);
                item.Content = model.Content;
                item.Subject = model.Subject;
                return FeedbackDAO.Update(item);
            }
            catch (Exception)
            {
                return false;
            }

        }

        [HttpPost]
        public bool DeleteFeedbackv2(int id)
        {

            try
            {
                return FeedbackDAO.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }

        }



        [HttpPost]
        public string getManagerFeedback(int page = 1 ) {
            IList<FeedbackModel> list = new List<FeedbackModel>();
            MyResponse<IList<FeedbackModel>> myResponse = new MyResponse<IList<FeedbackModel>>();
            
            if (IsAuthoration())
            {
                try
                {
                    list = FeedbackDAO.GetList(new Pagination() { currentPage = page});
                    myResponse.success = true;
                    myResponse.data = list;
                }
                catch (Exception ex)
                {
                    myResponse.message = ex.Message;
                }
                
            }
            return JsonConvert.SerializeObject(myResponse);
        }

        // GET: Admin/ManageFeedback
        public ActionResult ManageFeedback(int page = 1, int pageSize = 2)
        {
            if (IsAuthoration())
            {
                return View(FeedbackDAO.GetAllFeedbackPaging(page, pageSize));
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

       
        public ActionResult DetailFeedback(int id) {
            var s = FeedbackDAO.GetById(id);
            return View(s);
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
                        if (id > 0)
                        {
                            if (FeedbackDAO.Delete(id))
                            {
                                Alert("Delete feedback successfully", NotificationType.success);
                                return RedirectToAction("ManageFeedback");
                            }
                        }
                        return RedirectToAction("ManageFeedback");
                    }
                    
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
                            Alert("Delete Card Successfully", NotificationType.success);
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
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    var result = UserDAO.GetUser(id);
                    return View(result);
                }
                Alert("You not permit to access this page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            Alert("You need login to access this page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
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

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (Session["username"] != null && Session["role"] != null)
                {
                    if (Session["role"].ToString().ToLower() == "true")
                    {
                        if (id >= 0)
                        {
                            if (UserDAO.DeleteUser(id))
                            {
                                Alert("Delete User Successfully .", NotificationType.success);
                                return RedirectToAction("ManageUser");
                            }
                            else
                            {
                                Alert("Delete error, cannot find this User!!!", NotificationType.error);
                                return RedirectToAction("ManageUser");
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("ManageCard");
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
            info,
            question
        }
    }
}
