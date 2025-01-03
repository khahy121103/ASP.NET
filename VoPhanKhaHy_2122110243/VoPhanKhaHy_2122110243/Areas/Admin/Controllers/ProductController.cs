using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoPhanKhaHy_2122110243.Context;
using System.IO;
using System.Data.Entity;
using System.Security.AccessControl;
using System.Data.Common;
using System.Data;

namespace VoPhanKhaHy_2122110243.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebASPEntities db = new WebASPEntities();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var listProduct = db.Products.ToList();
            return View(listProduct);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var listCategory = db.Categories.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(listCategory);
            ViewBag.Category = new SelectList(listCategory, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product) 
        {
            try
            {
                if (product.ImageUpLoad != null && product.ImageUpLoad.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageUpLoad.FileName);
                    string extension = Path.GetExtension(product.ImageUpLoad.FileName);
                    fileName = fileName  + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    product.Avartar = fileName;
                    product.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }
                product.CreatedOnUtc = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var Product = db.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(Product);
        }
        

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var Product = db.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(Product);
        }
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            var Product = db.Products.Where(n => n.Id == product.Id).FirstOrDefault();
            db.Products.Remove(Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Product = db.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(Product);
        }
        [HttpPost]
        public ActionResult Edit(Product product,int id)
        {
            if (product.ImageUpLoad != null && product.ImageUpLoad.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageUpLoad.FileName);
                string extension = Path.GetExtension(product.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                product.Avartar = fileName;
                product.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}