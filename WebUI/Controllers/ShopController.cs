using BusinessLayer.Implementations;
using BusinessLayer.Interfaces;
using DomainLayer.DbEFContext;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.ViewModels;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IRepository<Shop> repository = null;
        ApplicationDbContext context = new ApplicationDbContext();

        public ShopController()
        {
            this.repository = new Repository<Shop>();
        }
        public ShopController(IRepository<Shop> repository)
        {
            this.repository = repository;
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
                List<ShopViewModel> shopViewModels = new List<ShopViewModel>();

                var allShops = repository.GetAllRecords();
                if (allShops.Count() > 0)
                {
                    //CREATE NEW LIST OF CATEGORY_VIEW_MODEL AND HAND OVER TO VIEW
                    foreach (var shop in allShops)
                    {
                        //GET USER
                        string userName = null;
                        var user = context.Users.Where(x => x.Id == shop.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            userName = user.Name;
                        }

                        ShopViewModel shopVm = new ShopViewModel()
                        {
                            Id = shop.Id,
                            Name = shop.Name,
                            PermanantAddress = shop.PermanantAddress,
                            BookingDays = shop.BookingDays,
                            ClosingHour = shop.ClosingHour,
                            Contact1 = shop.Contact1,
                            Contact2 = shop.Contact2,
                            _Image1 = shop.Image1,
                            _Image2 = shop.Image2,
                            OpeningHour = shop.OpeningHour,
                            UserID = userName,
                        };

                        shopViewModels.Add(shopVm);
                    }


                    ViewBag.AllShops = shopViewModels;
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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShopViewModel model)
        {
            string msg = "";

            try
            {

                //GET USER

                string userID = null;
                var user = context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                if (user != null)
                {

                    model.UserID = user.Id;

                    // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                    if (model.Image1 == null || model.Image1.ContentLength < 0 ||
                        model.Image2 == null || model.Image2.ContentLength < 0)
                    {
                        msg = "Please select a file, Try again!";
                        ViewBag.Msg = msg;

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


                        string pathx = Server.MapPath("~/Uploads/Shops/" + imageName1);
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


                            string pathy = Server.MapPath("~/Uploads/Shops/" + imageName2);
                            if (System.IO.File.Exists(pathy))
                                System.IO.File.Delete(pathy);
                            model.Image2.SaveAs(pathy);
                        }


                     

                        #region CREATE NEW PRODUCT
                        Shop objShop = new Shop()
                        {
                            Name = model.Name,
                            PermanantAddress = model.PermanantAddress,
                            BookingDays = model.BookingDays,
                            ClosingHour = model.ClosingHour,
                            Contact1 = model.Contact1,
                            Contact2 = model.Contact2,
                            Image1 = imageName1,
                            Image2 = imageName2,
                            OpeningHour = model.OpeningHour,
                            UserID = model.UserID
                        };


                        if (ModelState.IsValid)
                        {
                            var addRec = repository.AddInToTable(objShop);
                            if (addRec != null)
                            {
                                TempData["Msg"] = objShop.Name + " added to the database!";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Msg"] = objShop.Name + " was not added to the database!";
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
                else
                {
                    TempData["Msg"] = "This operation is limited to registered users!";
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
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }

            ViewBag.Msg = TempData["Msg"].ToString();
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = repository.GetRecordById(id);

            if (model != null)
            {
                var shop = new ShopViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    PermanantAddress = model.PermanantAddress,
                    BookingDays = model.BookingDays,
                    ClosingHour = model.ClosingHour,
                    Contact1 = model.Contact1,
                    Contact2 = model.Contact2,
                    OpeningHour = model.OpeningHour,
                    UserID = model.UserID
                };

                return View(shop);
            }
            else
            {
                TempData["Msg"] = "RECORD NOT FOUND WITH SUPPLIED ID!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopViewModel model)
        {
            string msg = "";




            // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
            if (model.Image1 == null || model.Image1.ContentLength < 0)
            {
                msg = "Please select a file, Try again!";
                ViewBag.Msg = msg;

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


                string pathx = Server.MapPath("~/Uploads/Shops/" + imageName1);
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


                    string pathy = Server.MapPath("~/Uploads/Shops/" + imageName2);
                    if (System.IO.File.Exists(pathy))
                        System.IO.File.Delete(pathy);
                    model.Image2.SaveAs(pathy);
                }


                #region UPDATE PRODUCT


                var objShop = repository.GetRecordById(model.Id);
                if (objShop != null)
                {
                    //UPDATE THE MODEL
                    objShop.Name = model.Name;
                    objShop.Contact1 = model.Contact1;
                    objShop.Contact2 = model.Contact2;
                    objShop.BookingDays = model.BookingDays;
                    objShop.ClosingHour = model.ClosingHour;
                    objShop.PermanantAddress = model.PermanantAddress;
                    objShop.UserID = model.UserID;
                    objShop.Image1 = imageName1;
                    objShop.Image2 = imageName2;



                    if (ModelState.IsValid)
                    {
                        var addRec = repository.Update(objShop);
                        if (addRec != null)
                        {
                            TempData["Msg"] = objShop.Name + " has been updated successfully!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Msg"] = objShop.Name + " record update failed!";
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


            return View(model);
        }


        public JsonResult DeleteShop(int ID)
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