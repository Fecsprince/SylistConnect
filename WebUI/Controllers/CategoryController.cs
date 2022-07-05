using BusinessLayer.Implementations;
using BusinessLayer.Interfaces;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private IRepository<Category> repository = null;
        public CategoryController()
        {
            this.repository = new Repository<Category>();
        }
        public CategoryController(IRepository<Category> repository)
        {
            this.repository = repository;
        }
        #region CATEGORY CRUD
        public ActionResult Index()
        {

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            try
            {
                List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();

                var allCategories = repository.GetAllRecords();
                if (allCategories.Count() > 0)
                {
                    //CREATE NEW LIST OF CATEGORY_VIEW_MODEL AND HAND OVER TO VIEW
                    foreach (var cate in allCategories)
                    {
                        CategoryViewModel catVm = new CategoryViewModel()
                        {
                            Id = cate.Id,
                            Name = cate.Name,
                            Description = cate.Description
                        };

                        categoryViewModels.Add(catVm);
                    }


                    ViewBag.AllCategories = categoryViewModels;
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();
                return View();
            }
            return View();
        }




        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CategoryViewModel model)
        {
            string msg = "";
            try
            {
                Category objCate = new Category()
                {
                    Name = model.Name,
                    Description = model.Description
                };


                if (ModelState.IsValid)
                {
                    var addRec = repository.AddInToTable(objCate);
                    if (addRec != null)
                    {
                        TempData["Msg"] = objCate.Name + " added to the database!";
                    }
                    else
                    {
                        TempData["Msg"] = objCate.Name + " was not added to the database!";
                    }
                }
                else
                {
                    TempData["Msg"] = "Update failed: Error 100 --> Invalid entry!";
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException.Message != null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    msg = TempData["Msg"].ToString();
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    msg = TempData["Msg"].ToString();
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }

            msg = TempData["Msg"].ToString();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = repository.GetRecordById(id);

            if (model != null)
            {
                var category = new CategoryViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                return View(category);
            }
            else
            {
                TempData["Msg"] = "RECORD NOT FOUND WITH SUPPLIED ID!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel model)
        {


            var objCate = new Category()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            if (ModelState.IsValid)
            {
                var updateRecord = repository.Update(objCate);
                if (updateRecord != null)
                {
                    TempData["Msg"] = objCate.Name + " has been updated succssfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Msg"] = objCate.Name + " was not updated succssfully!";
                }
            }
            else
            {
                TempData["Msg"] = "Invalid Entries";
            }


            ViewBag.Msg = TempData["Msg"].ToString();
            return View();
        }



        public JsonResult DeleteCategory(int ID)
        {
            string msg = "";

            if (ID > 0)
            {
                int responseValue = repository.RemoveFromTable(ID);
                if (responseValue == 1)
                {
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}