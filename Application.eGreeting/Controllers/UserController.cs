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
            return View(UserDAO.GetAllUser);
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
            if (ModelState.IsValid)
            {
                if (UserDAO.CreateUser(newUser))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Duplicate ID!!!");
            }
            return View();
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

    }
}
