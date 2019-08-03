using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (IsAdmin())
            {
                return View();
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }       

        //========================================================= Manage Feedback ===========================================================================
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
        public bool UpdateFeedback(FeedbackModel model)
        {
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
        public string GetManagerFeedback(int page = 1)
        {
            IList<FeedbackModel> list = new List<FeedbackModel>();
            MyResponse<IList<FeedbackModel>> myResponse = new MyResponse<IList<FeedbackModel>>();

            if (IsLoggedIn())
            {
                try
                {
                    list = FeedbackDAO.GetList(new Pagination() { currentPage = page });
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
        public ActionResult ManageFeedback(int? page)
        {
            if (IsAdmin())
            {
                if (page == null)
                {
                    page = 1;
                }
                int pageSize = 3;
                int pageNumber = (page ?? 1);

                return View(FeedbackDAO.GetAllFeedback.ToPagedList(pageNumber, pageSize));
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");


            //=============== khong xoa ============

            //if (IsLoggedIn())
            //{
            //    return View(FeedbackDAO.GetAllFeedbackPaging(page, pageSize));
            //}
            //Alert("You not permit to access that page", NotificationType.warning);
            //return RedirectToAction("Login", "Home");
        }


        public ActionResult DetailFeedback(int id)
        {
            var s = FeedbackDAO.GetById(id);
            return View(s);
        }

        [HttpPost]
        public ActionResult DeleteFeedback(int id)
        {
            try
            {
                if (IsAdmin())
                {
                    if (id > 0)
                    {
                        if (FeedbackDAO.Delete(id))
                        {
                            Alert("Delete feedback successfully", NotificationType.success);
                            return RedirectToAction("ManageFeedback");
                        }
                    }
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("ManageFeedback");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("ManageFeedback");
            }
        }

        //================================================ Manage Card ====================================================//

        // GET: Admin/ManageCard
        public ActionResult ManageCard(string pName,int? page)
        {
            var model = CardDAO.GetAllCard;
            if (IsAdmin())
            {
                if (!string.IsNullOrEmpty(pName))
                {
                    //seartch by name
                    model = model.Where(p => p.NameCard.ToUpper().Contains(pName)
                                        || p.NameCard.ToLower().Contains(pName)).ToList();
                    //items in page
                    int pageSize = 9;
                    int pageNumber = (page ?? 1);
                    return View(model.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    //items in page
                    int pageSize = 9;
                    int pageNumber = (page ?? 1);
                    return View(model.ToPagedList(pageNumber, pageSize));
                }
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Login", "Home");
        }

        // GET: Admin/CreateCard
        public ActionResult CreateCard()
        {
            if (IsAdmin())
            {
                return View();
            }
            Alert("You not permit to access that page", NotificationType.warning);
            return RedirectToAction("Index", "Home");
        }

        // POST: Admin/CreateCard
        [HttpPost]
        public ActionResult CreateCard(Card newCard, HttpPostedFileBase file)
        {
            try
            {
                if (IsAdmin())
                {
                    if (ModelState.IsValid)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            //var search = CardDAO.GetNameCard(newCard.NameCard);
                            if (CardDAO.GetNameCard(newCard.NameCard))
                            {
                                Alert("Namecard has been exist.", NotificationType.error);
                                return View();
                            }
                           
                            var ext = Path.GetExtension(file.FileName);
                            if (CheckExtImg(ext))
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var imagePath = Server.MapPath("~/ImageCard/" + fileName);
                                newCard.ImageName = fileName;
                                newCard.DateCreated = DateTime.Now;
                                if (CardDAO.GetImageCard(fileName))
                                {
                                    Alert("Image Name has been exist.", NotificationType.error);
                                    return View();
                                }
                                if (CardDAO.Create(newCard))
                                {
                                    file.SaveAs(imagePath);
                                    Alert("Create new Card successfully", NotificationType.success);
                                    return RedirectToAction("ManageCard");
                                }
                                Alert("Create new card failed.",NotificationType.error);
                                return View();
                            }
                            else
                            {
                                Alert("File format invalid. Please select file *.png/.jpg/.jpeg.", NotificationType.error);
                                return View();
                            }
                        }
                        else
                        {
                            Alert("Please choose Image", NotificationType.error);
                            return View();
                        }
                    }
                    return RedirectToAction("Index");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                Alert(e.Message,NotificationType.error);
                return View();
            }
        }

        //GET: Admin/EditCard/5
        public ActionResult EditCard(int id)
        {
            try
            {
                if (IsAdmin())
                {
                    var search = CardDAO.GetCard(id);
                    if (search != null)
                    {
                        return View(search);
                    }
                    Alert("Cannot get Card", NotificationType.error);
                    return RedirectToAction("Index");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");
                
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return RedirectToAction("Index");
                throw;
            }
        }

        //POST: Admin/EditCard/5
        [HttpPost]
        public ActionResult EditCard(Card editCard, HttpPostedFileBase file)
        {

            try
            {
                if (IsAdmin())
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
                                editCard.ImageName = fileName;
                                if (editCard.DateCreated == null)
                                {
                                    editCard.DateCreated = DateTime.Now;
                                }

                                if (CardDAO.EditCard(editCard))
                                {
                                    file.SaveAs(imagePath);
                                    Alert("Update Card successfully", NotificationType.success);
                                    return RedirectToAction("ManageCard");
                                }
                                Alert("Update card failed.", NotificationType.error);
                                return View();
                            }
                            else
                            {
                                Alert("File format invalid. Please select file *.png/.jpg/.jpeg.", NotificationType.error);
                                return View();
                            }
                        }
                        else
                        {
                            if (editCard.DateCreated == null)
                            {
                                editCard.DateCreated = DateTime.Now;
                            }
                            if (CardDAO.EditCard(editCard))
                            {
                                Alert("Update Card successfully", NotificationType.success);
                                return RedirectToAction("ManageCard");
                            }
                            Alert("Update card failed.", NotificationType.error);
                            return View();
                        }
                    }
                    return RedirectToAction("Index");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return View();
            }
        }

        private readonly List<string> ImageExtension = new List<string> { ".png", ".jpg", ".jpeg" };

        bool CheckExtImg(string ext)
        {
            return ImageExtension.Contains(ext.ToLower());
        }

        // GET: Admin/Edit/5
        //public ActionResult EditCard(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult EditCard(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        // GET: Admin/Delete/5
        public ActionResult DeleteCard(int id)
        {
            try
            {
                if (IsAdmin())
                {
                    if (id >= 0)
                    {
                        if (CardDAO.DeleteCard(id))
                        {
                            //if (ImageName != null)
                            //{
                            //    string PathImage = Request.MapPath("~/ImageCard/" + ImageName);
                            //    if (System.IO.File.Exists(PathImage))
                            //    {
                            //        System.IO.File.Delete(PathImage);
                            //    }
                            //}
                            //Alert("Delete Card Successfully", NotificationType.success);
                            return RedirectToAction("ManageCard");
                        }
                    }
                    Alert("Delete Card failed", NotificationType.error);
                    return RedirectToAction("ManageCard");
                }
                Alert("You not permit to access that page", NotificationType.warning);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("ManageCard");
            }
        }


        //================================================ Manage User ====================================================//

        // GET: Admin/ManageUser
        public ActionResult ManageUser(int? page)
        {
            if (IsAdmin())
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
            return RedirectToAction("Login", "Home");
        }

        // GET: Admin/CreateCard
        public ActionResult CreateUser()
        {
            if (IsAdmin())
            {
                return View();
            }
            Alert("You not permit to access this page", NotificationType.warning);
            return RedirectToAction("Index", "Home");
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

        // GET: Admin/EditUser
        public ActionResult EditUser(int id)
        {
            try
            {
                if (id != null)
                {
                    var search = UserDAO.GetUser(id);
                    if (search != null)
                    {
                        return View(search);
                    }
                    Alert("Not found user", NotificationType.error);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return RedirectToAction("Index");
                throw;
            }
        }

        // POST: Admin/EditUser
        [HttpPost]
        public ActionResult EditUser(User editU)
        {
            try
            {
                var searchUser = UserDAO.GetUser(editU.UserId);
                if (searchUser == null)
                {
                    Alert("Not found This User", NotificationType.error);
                    return View();
                }
                if (editU.Password == null || editU.RePassword == null)
                {
                    editU.Password = searchUser.Password;
                    editU.RePassword = searchUser.RePassword;
                }
                if (Session["username"].ToString() == "admin")
                {
                    editU.Role = true;
                }
                if (UserDAO.EditUser(editU))
                {
                    Alert("Edit User successfully!", NotificationType.success);
                    return RedirectToAction("ManageUser");
                }
                Alert("Edit User failed!", NotificationType.error);
                return View();
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return View();
                throw;
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (IsAdmin())
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("ManageCard");
            }
        }

        //================================================ Manage Payment ====================================================//
        // GET: /Admin/ManagePaymentInfo
        public ActionResult ManagePaymentInfo(int? page)
        {
            if (IsAdmin())
            {
                if (page == null)
                {
                    page = 1;
                }
                int pageSize = 3;
                int pageNumber = (page ?? 1);

                return View(PaymentDAO.GetAllPayment.ToPagedList(pageNumber, pageSize));
            }
            Alert("You not permit to access this page", NotificationType.error);
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult EditPayment(int id,string IsActive)
        {
            if (PaymentDAO.ChangeStatusActivation(id, Boolean.Parse(IsActive)))
            {
                Alert("Change status activation successfully", NotificationType.success);
                return RedirectToAction("ManagePaymentInfo");
            }
            Alert("Change status activation failed.", NotificationType.error);
            return View();
        }

        //POST: /Admin/DeletePayment
        [HttpPost]
        public ActionResult DeletePayment(int id)
        {
            try
            {
                if (PaymentDAO.DeletePayment(id))
                {
                    Alert("Delete Payment Successfully .", NotificationType.success);
                    return RedirectToAction("ManagePaymentInfo");
                }
                else
                {
                    Alert("Delete error, cannot find this User!!!", NotificationType.error);
                    return RedirectToAction("ManagePaymentInfo");
                }
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return RedirectToAction("ManagePaymentInfo");
                throw;
            }
        }
        
        //================================================ Manage Transaction====================================================//
        // GET: /Admin/ManageTrans

        public ActionResult ManageTrans(int? page)
        {
            if (IsAdmin())
            {
                if (page == null)
                {
                    page = 1;
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(TransDAO.GetAllTrans.ToPagedList(pageNumber, pageSize));
            }
            Alert("You not permit to access this page", NotificationType.error);
            return RedirectToAction("Login", "Home");
        }

        public ActionResult DeleteTrans(int id)
        {
            try
            {
                if (TransDAO.DeleteTrans(id))
                {
                    Alert("Delete Transaction Successfully .", NotificationType.success);
                    return RedirectToAction("ManageTrans");
                }
                else
                {
                    Alert("Delete Transaction error, cannot find this User!!!", NotificationType.error);
                    return RedirectToAction("ManageTrans");
                }
            }
            catch (Exception e)
            {
                Alert(e.Message, NotificationType.error);
                return RedirectToAction("ManageTrans");
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
            info,
            question
        }
    }
}
