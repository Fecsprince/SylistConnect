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
    public class ProductController : Controller
    {

        private IRepository<Product> repository = null;
        public ProductController()
        {
            this.repository = new Repository<Product>();
        }
        public ProductController(IRepository<Product> repository)
        {
            this.repository = repository;
        }


        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
    }
}