using CRM.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        IRepository repository;
        public TasksController(IRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult MenuTasks()
        {
            return PartialView(repository.Tasks.OrderBy(x=>x.id));
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}