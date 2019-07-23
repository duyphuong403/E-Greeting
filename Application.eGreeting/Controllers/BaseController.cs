using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.eGreeting.Controllers
{
    public class BaseController : Controller
    {
        public bool IsAdmin() {
            if (Session["username"] != null && Session["role"] != null)
            {
                if (Session["role"].ToString().ToLower() == "true")
                {
                    //return View(FeedbackDAO.GetAllFeedback.OrderByDescending(o => o.Id));
                    return true;
                }
            }
            return false;
        }

        public bool IsLoggedIn()
        {
            if (Session["username"] != null)
            {
                return true;                
            }
            return false;
        }

    }
}