using Application.eGreeting.DataAccess;
using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class SlideCardController : Controller
    {
        // GET: HomeCard
        public ActionResult Index()
        {
            SlideCard slidecard = new SlideCard();

            slidecard.ListBirthday = CardDAO.GetCardsByCategory("birthday").Take(9).ToList();
            slidecard.ListNewYear = CardDAO.GetCardsByCategory("newyear").Take(9).ToList();
            slidecard.ListFestival = CardDAO.GetCardsByCategory("festival").Take(9).ToList();

            return View("Index", slidecard);
        }
    }
}