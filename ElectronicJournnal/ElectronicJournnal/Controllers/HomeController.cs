using ElectronicJournnal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicJournnal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string password)
        {
            if (password == "12345")
            {
                return RedirectToActionPermanent("TeacherIndex", "Teacher");
            }
            else
            {
                return RedirectToActionPermanent("Index", "Home");
            }
        }

        public ActionResult Dispatch(string res)
        {
            if (res == "Да")
                return RedirectToRoute(new { controller = "Home", action = "Subscription" });
            else
            {
                return RedirectToActionPermanent("ParentIndex", "Parent");
            }
        }

        // Форма для подписки.
        [HttpGet]
        public ActionResult Subscription()
        {
            List<PupilWithClass> pupils = PupilsDB.GetPupilsWithClassOrderByClass();
            ViewBag.Pupils = pupils;
            return View();
        }

        // Оформление подписки на рассылку сообщений.
        public ActionResult Subscription(List<int> Subscribe, string subscr, Parent parent)
        {
            if (Subscribe.Count != 0)
            {
                ParentsDB.AddParent(parent.Name, parent.Surname, parent.Patronymic, parent.Email);
                int parentID = ParentsDB.GetParentsLastID();

                for (int i = 0; i < Subscribe.Count; i++)
                {
                    ParentsDB.AddParenthood(parentID, Subscribe[i]);
                }
            }
            return RedirectToRoute(new { controller = "Parent", action = "ParentIndex" });
        }
    }
}