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
    public class ProductController : Controller
    {
        private const string V = ". ";
        private IRepository<Product> repository_Prod = null;
        private IRepository<Category> repository_Cate = null;
        public ProductController()
        {
            this.repository_Prod = new Repository<Product>();
            this.repository_Cate = new Repository<Category>();
        }
        public ProductController(IRepository<Product> repository, IRepository<Category> repositoryCate)
        {
            this.repository_Prod = repository;
            this.repository_Cate = repositoryCate;
        }


        #region PRODUCT CRUD
        public ActionResult Index()
        {

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            try
            {
                List<ProductViewModel> productViewModels = new List<ProductViewModel>();

                var allProducts = repository_Prod.GetAllRecords();
                if (allProducts.Count() > 0)
                {
                    //CREATE NEW LIST OF CATEGORY_VIEW_MODEL AND HAND OVER TO VIEW
                    foreach (var prod in allProducts)
                    {
                        //LOAD CATEGORY THAT MATCHES THE PRODUCT CATEGORY ID
                        string cateName = repository_Cate.GetRecordById(prod.CategoryID).Name;

                        ProductViewModel prodVm = new ProductViewModel()
                        {
                            Id = prod.Id,
                            Name = prod.Name,
                            Description = prod.Description,
                            CategoryID = prod.CategoryID,
                            CategoryName = cateName,
                            _Image1 = prod.Image1,
                            _Image2 = prod.Image2
                        };

                        productViewModels.Add(prodVm);
                    }


                    ViewBag.AllProducts = productViewModels;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message != null)
                {
                    TempData["Msg"] = ex.InnerException.Message.ToString();

                    ViewBag.Msg = TempData["Msg"].ToString();
                }
                else
                {
                    TempData["Msg"] = ex.Message.ToString();

                    ViewBag.Msg = TempData["Msg"].ToString();
                }
                return View();
            }
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            //LOAD ALL CATEGORIES
            var categories = repository_Cate.GetAllRecords();
            List<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();
            foreach (var cate in categories)
            {
                CategoryViewModel model = new CategoryViewModel()
                {
                    Id = cate.Id,
                    Name = cate.Name,
                    Description = cate.Description
                };


                categoryVMs.Add(model);
            }
            ViewBag.Categories = categoryVMs;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            string msg = "";
            //LOAD ALL CATEGORIES
            var categories = repository_Cate.GetAllRecords();
            List<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();
            foreach (var cate in categories)
            {
                CategoryViewModel category = new CategoryViewModel()
                {
                    Id = cate.Id,
                    Name = cate.Name,
                    Description = cate.Description
                };


                categoryVMs.Add(category);
            }

            try
            {
               
                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.Image1 == null || model.Image1.ContentLength < 0 ||
                    model.Image2 == null || model.Image2.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;
                    ViewBag.Category = categoryVMs;

                    return View(model);
                }
                else if (model.Image1 != null && model.Image1.ContentLength > 0 &&
                    model.Image1.FileName.ToLower().EndsWith("jpg") ||
                    model.Image1.FileName.ToLower().EndsWith("png") ||
                    model.Image2.ContentLength > 0 &&
                    model.Image2.FileName.ToLower().EndsWith("jpg") ||
                    model.Image2.FileName.ToLower().EndsWith("png"))
                {

                    //UPLOAD NOT EMPTY
                  

                    int postFix = 1;
                    string imgs = model.Image1.FileName;
                    char[] separator = { '.' };
                    int count = 2;

                    //CHANGE IMAGE 1 FILE NAME
                    string[] imgx = imgs.Split(separator, count, StringSplitOptions.None);

                    string _ProdNamex = model.Name + postFix.ToString();

                    string newImage1_Name = _ProdNamex + "." + imgx[1];

                    string imageName1 = String.Concat(newImage1_Name.Where(x => !Char.IsWhiteSpace(x)));


                    string pathx = Server.MapPath("~/Uploads/Products/" + imageName1);
                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);
                    model.Image1.SaveAs(pathx);

                    string imageName2 = "";
                    //CHANGE IMAGE 2 FILE NAME
                    if (model.Image2 != null)
                    {
                        string[] imgy = model.Image2.FileName.Split(separator, count, StringSplitOptions.None);

                        postFix++;
                        string _ProdNamey = model.Name + postFix.ToString();

                        string newImage2_Name = _ProdNamey + "." + imgy[1];

                        imageName2 = String.Concat(newImage2_Name.Where(x => !Char.IsWhiteSpace(x)));


                        string pathy = Server.MapPath("~/Uploads/Products/" + imageName2);
                        if (System.IO.File.Exists(pathy))
                            System.IO.File.Delete(pathy);
                        model.Image2.SaveAs(pathy);
                    }


                    #region CREATE NEW PRODUCT
                    Product objProd = new Product()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CategoryID = model.CategoryID,
                        Image1 = imageName1,
                        Image2 = imageName2,
                        Price = model.Price
                    };


                    if (ModelState.IsValid)
                    {
                        var addRec = repository_Prod.AddInToTable(objProd);
                        if (addRec != null)
                        {
                            TempData["Msg"] = objProd.Name + " added to the database!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Msg"] = objProd.Name + " was not added to the database!";
                        }
                    }
                    else
                    {
                        TempData["Msg"] = "Update failed: Error 100 --> Invalid entry!";
                    }
                    #endregion
                }
                else
                {
                    TempData["Msg"] = "Image 1 was not valid!";
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException.Message != null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    ViewBag.Categories = categoryVMs; return View();
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    ViewBag.Categories = categoryVMs;
                    return View();
                }
            }


            ViewBag.Categories = categoryVMs;

            ViewBag.Msg = TempData["Msg"].ToString();
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = repository_Prod.GetRecordById(id);

            if (model != null)
            {
                var product = new ProductViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    CategoryID = model.CategoryID,
                    Price = model.Price,
                    _Image1 = model.Image1,
                    _Image2 = model.Image2
                };

                //LOAD ALL CATEGORIES
                var categories = repository_Cate.GetAllRecords();
                List<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();
                foreach (var cate in categories)
                {
                    CategoryViewModel vmModel = new CategoryViewModel()
                    {
                        Id = cate.Id,
                        Name = cate.Name
                    };


                    categoryVMs.Add(vmModel);
                }
                ViewBag.Categories = categoryVMs;

                return View(product);
            }
            else
            {
                TempData["Msg"] = "RECORD NOT FOUND WITH SUPPLIED ID!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model)
        {
            string msg = "";


            //LOAD ALL CATEGORIES
            var categories = repository_Cate.GetAllRecords();
            List<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();
            foreach (var cate in categories)
            {
                CategoryViewModel vmModel = new CategoryViewModel()
                {
                    Id = cate.Id,
                    Name = cate.Name
                };


                categoryVMs.Add(vmModel);
            }


            // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
            if (model.Image1 == null || model.Image1.ContentLength < 0)
            {
                msg = "Please select a file, Try again!";
                ViewBag.Msg = msg;
                ViewBag.Categories = categoryVMs;

                return View(model);
            }
            else if (model.Image1 != null && model.Image1.ContentLength > 0 &&
                model.Image1.FileName.ToLower().EndsWith("jpg") ||
                model.Image1.FileName.ToLower().EndsWith("png") ||
                model.Image2.ContentLength > 0 &&
                model.Image2.FileName.ToLower().EndsWith("jpg") ||
                model.Image2.FileName.ToLower().EndsWith("png"))
            {

                //UPLOAD NOT EMPTY
                //string imageName1 = model.Name + imageId.ToString();


                int postFix = 1;
                string imgs = model.Image1.FileName;
                char[] separator = { '.' };
                int count = 2;

                //CHANGE IMAGE 1 FILE NAME
                string[] imgx = imgs.Split(separator, count, StringSplitOptions.None);

                string _ProdNamex = model.Name + postFix.ToString();

                string newImage1_Name = _ProdNamex + "." + imgx[1];

                string imageName1 = String.Concat(newImage1_Name.Where(x => !Char.IsWhiteSpace(x)));


                string pathx = Server.MapPath("~/Uploads/Products/" + imageName1);
                if (System.IO.File.Exists(pathx))
                    System.IO.File.Delete(pathx);
                model.Image1.SaveAs(pathx);


                string imageName2 = "";
                //CHANGE IMAGE 2 FILE NAME
                if (model.Image2 != null)
                {
                    string[] imgy = model.Image2.FileName.Split(separator, count, StringSplitOptions.None);

                    postFix++;
                    string _ProdNamey = model.Name + postFix.ToString();

                    string newImage2_Name = _ProdNamey + "." + imgy[1];

                    imageName2 = String.Concat(newImage2_Name.Where(x => !Char.IsWhiteSpace(x)));


                    string pathy = Server.MapPath("~/Uploads/Products/" + imageName2);
                    if (System.IO.File.Exists(pathy))
                        System.IO.File.Delete(pathy);
                    model.Image2.SaveAs(pathy);
                }


                #region UPDATE PRODUCT


                var objProd = repository_Prod.GetRecordById(model.Id);
                if (objProd != null)
                {
                    //UPDATE THE MODEL
                    objProd.Name = model.Name;
                    objProd.Description = model.Description;
                    objProd.Price = model.Price;
                    objProd.CategoryID = model.CategoryID;
                    objProd.Image1 = imageName1;
                    objProd.Image2 = imageName2;



                    if (ModelState.IsValid)
                    {
                        var addRec = repository_Prod.Update(objProd);
                        if (addRec != null)
                        {
                            TempData["Msg"] = objProd.Name + " has been updated successfully!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Msg"] = objProd.Name + " record update failed!";
                        }
                    }
                    else
                    {
                        TempData["Msg"] = "Update failed: Error 100 --> Invalid entry!";
                    }
                }
                else
                {
                    msg = "Your request could not be processed!";
                }


                #endregion
            }
            else
            {
                TempData["Msg"] = "Image 1 was not valid!";
            }


            ViewBag.Msg = TempData["Msg"].ToString();

            ViewBag.Categories = categoryVMs;

            return View(model);
        }


        public JsonResult DeleteProduct(int ID)
        {
            string msg = "";

            if (ID > 0)
            {
                int responseValue = repository_Prod.RemoveFromTable(ID);
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