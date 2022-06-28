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

        private IRepository<Shop> repository = null;
        public SylistManagerController()
        {
            this.repository = new Repository<Shop>();
        }
        public SylistManagerController(IRepository<Shop> repository)
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