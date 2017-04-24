using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_OnlineStore.Models;
using System.Data.Entity.Validation;
using System.IO;

namespace CRUD_OnlineStore.Controllers
{
    public class CategoriesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Categories
        public ActionResult Index()
        {
            var result = db.Categories
                 .Select(x => new CategoryViewModel
                 {
                     CategoryId = x.CategoryId,
                     CategoryName = x.CategoryName,
                     Description = x.Description,
                     Thumbnail = x.Thumbnail,
                     ParentId = x.ParentId,
                     TotalItems = x.TotalItems,
                     SortOrder = x.SortOrder,
                     // ParentString = db.Categories.Find(x.ParentId).CategoryName,
                 }).ToList();

            return View(result);

        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            string category_parent_name = String.Empty;
            if (category.ParentId != null)
            {
                Category category_parent = db.Categories.Find(category.ParentId);
                category_parent_name = category_parent.CategoryName;
            }

            var viewmodel = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Thumbnail = category.Thumbnail,
                ParentId = category.ParentId,
                ParentString = category_parent_name,
                TotalItems = category.TotalItems,
                SortOrder = category.SortOrder,
            };


            return View(viewmodel);
        }


        // GET: Categories/Create
        public ActionResult Create()
        {
            var parent = db.Categories.ToList();
            ViewBag.ParentList = new SelectList(parent, "CategoryId", "CategoryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,Description,Thumbnail,ParentId,TotalItems,SortOrder,Status,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt")] CategoryViewModel viewmodel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {    //check PNG, GIF, JPG
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".png" && extension != ".gif" && extension != ".jpg")
                    {
                        ViewBag.Notice = "Invalid file format. Please choice again.";

                        var parent1 = db.Categories.ToList();
                        ViewBag.CategoryList = new SelectList(parent1, "CategoryId", "CategoryName");

                        return View();
                    }
                    // check size
                    if (file.ContentLength > 2097152)
                    {
                        ViewBag.Notice = "Invalid file size. Please choice again.";

                        var parent1 = db.Categories.ToList();
                        ViewBag.categoryList = new SelectList(parent1, "CategoryId", "CategoryName");

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

                var category = new Category
                {
                    CategoryName = viewmodel.CategoryName,
                    Description = viewmodel.Description,
                    Thumbnail = viewmodel.Thumbnail,
                    ParentId = viewmodel.ParentId,
                    TotalItems = viewmodel.TotalItems,
                    SortOrder = viewmodel.SortOrder,
                    Status = viewmodel.Status,
                    CreatedBy = viewmodel.CreatedBy,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = viewmodel.ModifiedAt,
                    ModifiedBy = viewmodel.ModifiedBy
                };
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            var parent = db.Categories.ToList();
            ViewBag.ParentList = new SelectList(parent, "CategoryId", "CategoryName");

            return View(viewmodel);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Thumbnail = category.Thumbnail,
                ParentId = category.ParentId,
                TotalItems = category.TotalItems,
                SortOrder = category.SortOrder,
                Status = category.Status,
                CreatedBy = category.CreatedBy,
                CreatedAt = category.CreatedAt,
                ModifiedAt = category.ModifiedAt,
                ModifiedBy = category.ModifiedBy
            };

            var parent = db.Categories.ToList();
            ViewBag.ParentList = new SelectList(parent, "CategoryId", "CategoryName", viewmodel.ParentId);

            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,Description,Thumbnail,ParentId,TotalItems,SortOrder,Status,CreatedBy,CreatedAt,ModifiedBy,ModifiedAt")] CategoryViewModel viewmodel, HttpPostedFileBase file)
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

                            var parent1 = db.Categories.ToList();
                            ViewBag.CategoryList = new SelectList(parent1, "CategoryId", "CategoryName");

                            return View();
                        }
                        // check size
                        if (file.ContentLength > 2097152)
                        {
                            ViewBag.Notice = "Invalid file size. Please choice again.";

                            var parent1 = db.Categories.ToList();
                            ViewBag.categoryList = new SelectList(parent1, "CategoryId", "CategoryName");

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

                var category = db.Categories.Find(viewmodel.CategoryId);
                category.CategoryName = viewmodel.CategoryName;
                category.Description = viewmodel.Description;
                category.ParentId = viewmodel.ParentId;
                category.TotalItems = viewmodel.TotalItems;
                category.SortOrder = viewmodel.SortOrder;
                category.Status = viewmodel.Status;

                if (viewmodel.Thumbnail != null)
                    category.Thumbnail = viewmodel.Thumbnail;

                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var parent = db.Categories.ToList();
            ViewBag.ParentList = new SelectList(parent, "CategoryId", "CategoryName", viewmodel.ParentId);
            return View(viewmodel);
        }


        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Thumbnail = category.Thumbnail,
                ParentId = category.ParentId,
                TotalItems = category.TotalItems,
                SortOrder = category.SortOrder,
                Status = category.Status,
            };
            return View(viewmodel);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Category category = db.Categories.Find(id);
                var viewmodel = new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    Thumbnail = category.Thumbnail,
                    ParentId = category.ParentId,
                    TotalItems = category.TotalItems,
                    SortOrder = category.SortOrder,
                    Status = category.Status,
                };
                ViewBag.String = "Delete failed";
                return View( viewmodel);
            }

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
