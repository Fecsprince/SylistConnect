using BusinessLayer.Implementations;
using BusinessLayer.Interfaces;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class SylistManagerController : Controller
    {

        private IRepository<Employee> repository = null;
        public SylistManagerController()
        {
            this.repository = new Repository<Employee>();
        }
        public SylistManagerController(IRepository<Employee> repository)
        {
            this.repository = repository;
        }



        // GET: SylistManager
        public ActionResult Index()
        {
            return View();
        }
    }
}