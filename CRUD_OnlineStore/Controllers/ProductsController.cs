using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_OnlineStore.Models;
using System.IO;

namespace CRUD_OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Products
        public ActionResult Index()
        {
            var result = db.Products
                 .Select(x => new ProductViewModel
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     Price = x.Price,
                     Discount = x.Discount,
                     Thumbnail = x.Thumbnail,
                     ShortDescription = x.ShortDescription,
                     Description = x.Description,
                     CategoryId = x.CategoryId,
                     CategoryName = x.Category.CategoryName,
                     SortOrder = x.SortOrder,
                     Status = x.Status,
                 }).ToList();

            return View(result);

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            string category_parent_name = String.Empty;
            if (product.CategoryId != null)
            {
                Category category_parent = db.Categories.Find(product.CategoryId);
                category_parent_name = category_parent.CategoryName;
            }
            var viewmodel = new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Discount = product.Discount,
                Thumbnail = product.Thumbnail,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = category_parent_name,
                SortOrder = product.SortOrder,
                Status = product.Status,
            };
            return View(viewmodel);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();
            ViewBag.categoryId = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductName,Price,Discount,Thumbnail,ShortDescription,Description,CategoryId,SortOrder,Status,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt")] ProductViewModel viewmodel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {    // PNG, GIF, JPG
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".png" && extension != ".gif" && extension != ".jpg")
                    {
                        ViewBag.Notice = "Invalid file format. Please choice again.";

                        var categories1 = db.Categories.ToList();
                        ViewBag.categoryId = new SelectList(categories1, "CategoryId", "CategoryName");

                        return View();
                    }
                    // check size
                    if (file.ContentLength > 2097152)
                    {
                        ViewBag.Notice = "Invalid file size. Please choice again.";

                        var categories1 = db.Categories.ToList();
                        ViewBag.categoryId = new SelectList(categories1, "CategoryId", "CategoryName");

                        return View();
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    string filePath = "/uploads/" + Guid.NewGuid().ToString() + "/";

                    if (!Directory.Exists(Server.MapPath(filePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(filePath));
                    }
                    var path = Path.Combine(Server.MapPath(filePath), fileName);
                    file.SaveAs(path);

                    viewmodel.Thumbnail = filePath + fileName;
                }
                var product = new Product
                {
                    ProductName = viewmodel.ProductName,
                    Price = viewmodel.Price,
                    Discount = viewmodel.Discount,
                    Thumbnail = viewmodel.Thumbnail,
                    ShortDescription = viewmodel.ShortDescription,
                    Description = viewmodel.Description,
                    CategoryId = viewmodel.CategoryId,
                    SortOrder = viewmodel.SortOrder,
                    Status = viewmodel.Status,
                    CreatedBy = viewmodel.CreatedBy,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = viewmodel.ModifiedAt,
                    ModifiedBy = viewmodel.ModifiedBy
                };
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

            return View(viewmodel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new ProductViewModel
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Discount = product.Discount,
                Thumbnail = product.Thumbnail,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                CategoryId = product.CategoryId,
                SortOrder = product.SortOrder,
                Status = product.Status,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt,
                ModifiedAt = DateTime.Now,
                ModifiedBy = product.ModifiedBy
            };

            var categories = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", viewmodel.CategoryId);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,Price,Discount,Thumbnail,ShortDescription,Description,CategoryId,SortOrder,Status,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt")] ProductViewModel viewmodel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {    //check PNG, GIF, JPG
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension != ".png" && extension != ".gif" && extension != ".jpg")
                        {
                            ViewBag.Notice = "Invalid file format. Please choice again.";

                            var categories1 = db.Categories.ToList();
                            ViewBag.categoryId = new SelectList(categories1, "CategoryId", "CategoryName");

                            return View();
                        }
                        // check size
                        if (file.ContentLength > 2097152)
                        {
                            ViewBag.Notice = "Invalid file size. Please choice again.";

                            var categories1 = db.Categories.ToList();
                            ViewBag.categoryId = new SelectList(categories1, "CategoryId", "CategoryName");

                            return View();
                        }

                        var fileName = Path.GetFileName(file.FileName);
                        string filePath = "/uploads/" + Guid.NewGuid().ToString() + "/";

                        if (!Directory.Exists(Server.MapPath(filePath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(filePath));
                        }
                        var path = Path.Combine(Server.MapPath(filePath), fileName);
                        file.SaveAs(path);

                        viewmodel.Thumbnail = filePath + fileName;
                    }
                }
                //var product = new Product
                //{
                //    ProductName = viewmodel.ProductName,
                //    Price = viewmodel.Price,
                //    Discount = viewmodel.Discount,
                //    Thumbnail = viewmodel.Thumbnail,
                //    ShortDescription = viewmodel.ShortDescription,
                //    Description = viewmodel.Description,
                //    CategoryId = viewmodel.CategoryId,
                //    SortOrder = viewmodel.SortOrder,
                //};
                var product = db.Products.Find(viewmodel.Id);
                product.ProductName = viewmodel.ProductName;
                product.Price = viewmodel.Price;
                product.Discount = viewmodel.Discount;
                product.ShortDescription = viewmodel.ShortDescription;
                product.Description = viewmodel.Description;
                product.CategoryId = viewmodel.CategoryId;
                product.SortOrder = viewmodel.SortOrder;
                if (viewmodel.Thumbnail != null)
                    product.Thumbnail = viewmodel.Thumbnail;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", viewmodel.CategoryId);

            return View(viewmodel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new ProductViewModel
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Discount = product.Discount,
                Thumbnail = product.Thumbnail,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                CategoryId = product.CategoryId,
                SortOrder = product.SortOrder,
                Status = product.Status,
                CreatedBy = product.CreatedBy,
                CreatedAt = product.CreatedAt,
                ModifiedAt = DateTime.Now,
                ModifiedBy = product.ModifiedBy
            };
            return View(viewmodel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
