using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
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
            return View(UserDAO.GetAllUser);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                ModelState.AddModelError("", "You not permit to access this page");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "You need login to access this page");
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
                    if (file != null)
                    {

                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
                ModelState.AddModelError("", "You not permit to access this page");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "You need login to access this page");
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
                        ModelState.AddModelError("", "RePassword not match.");
                        return View();
                    }
                    var model = new User
                    {
                        UserName = newUser.UserName
                    };
                    var search = UserDAO.GetUserByUsername(model);
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

    }
}
