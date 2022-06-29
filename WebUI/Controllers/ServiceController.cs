using BusinessLayer.Implementations;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace WebUI.Controllers
{
    public class ServiceController : Controller
    {
        private IRepository<Service> repository = null;
        public ServiceController()
        {
            this.repository = new Repository<Service>();
        }
        public ServiceController(IRepository<Service> repository)
        {
            this.repository = repository;
        }

        // GET: Service
        public ActionResult Index()
        {
            return View();
        }
    }
}