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
    public class ShopController : Controller
    {
        private IRepository<Shop> repository = null;
        public ShopController()
        {
            this.repository = new Repository<Shop>();
        }
        public ShopController(IRepository<Shop> repository)
        {
            this.repository = repository;
        }


        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }
    }
}