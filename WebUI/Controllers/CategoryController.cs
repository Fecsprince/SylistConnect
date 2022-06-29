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

        [HttpGet]
        public ActionResult Category()
        {
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Category(CategoryViewModel model)
        {
            string msg = "";


            if (model.Name == null)
            {
                msg = "Name cannot be null!";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else if (model.Name.Length > 50 || model.Description.Length > 100)
            {
                msg = "Name character(s) cannot be more than 50, also, Description character(s) cannot be more than 100!";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }


            try
            {
                if (model.Id > 0) //RECORD EXIST
                {
                    var dbObj = repository.GetRecordById(model.Id);
                    if (dbObj != null)
                    {
                        dbObj.Name = model.Name;
                        dbObj.Description = model.Description;

                        if (ModelState.IsValid)
                        {
                            var updateRec = repository.Update(dbObj);

                            if (updateRec != null)
                            {
                                msg = updateRec.Name + " record update successfully!";
                            }
                            else
                            {
                                msg = dbObj.Name + " record update successfully!";
                            }
                        }
                        else
                        {
                            msg = "Update failed: Error 100 --> Invalid entry!";
                        }
                    }
                    else
                    {
                        msg = "Record does not exist anymore!";
                    }

                }
                else //NEW CATEGORY
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
                            msg = objCate.Name + " added to the database!";
                        }
                        else
                        {
                            msg = objCate.Name + " was not added to the database!";
                        }
                    }
                    else
                    {
                        msg = "Update failed: Error 100 --> Invalid entry!";
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message != null)
                {
                    msg = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    msg = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddEditCategory(int ID)
        {
            string msg = "";

            CategoryViewModel model = new CategoryViewModel();
            try
            {
                if (ID > 0)
                {
                    var obj = repository.GetRecordById(ID);
                    if (obj != null)
                    {
                        model.Id = obj.Id;
                        model.Name = obj.Name;
                        model.Description = obj.Description;
                    }
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException.Message != null)
                {
                    msg = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                }
                else
                {
                    msg = "Please copy response and send to admin: \n" + ex.Message.ToString();
                }

                return RedirectToAction("Category");
            }
            ViewBag.Msg = msg;
            return PartialView("AddEditCategory", model);
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