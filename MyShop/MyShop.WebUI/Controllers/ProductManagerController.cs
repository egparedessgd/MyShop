using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using static FunctionalExtensions.FunctionalHelpers;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;

        public ProductManagerController()
        {
            context = new ProductRepository();
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToHome;
            }
        }

        public ActionResult Edit(string Id)
        {
            return ShowViewOrHttpNotFound(() => context.Find(Id));
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            return FindOrHttpNotFound(
                () => context.Find(Id),
                (productToEdit) =>
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();

                    return RedirectToHome;
                });
        }

        public ActionResult Delete(string Id)
        {
            return ShowViewOrHttpNotFound(() => context.Find(Id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(String Id)
        {
            return FindOrHttpNotFound(
                () => context.Find(Id),
                (x) =>
                {
                    context.Deletee(Id);
                    context.Commit();
                    return RedirectToHome;
                });
        }

        public ActionResult FindOrHttpNotFound<TFindType>
            (Func<TFindType> valueFunction, Func<TFindType, ActionResult> OnFound) where TFindType : class =>
            MapNullable(
                valueFunction,
                OnFound,
                (x) => HttpNotFound());

        
        public ActionResult ShowViewOrHttpNotFound<TFindType>(Func<TFindType> valueFunction) where TFindType : class =>
            FindOrHttpNotFound(valueFunction, (x) => View(x));

        public ActionResult RedirectToHome => RedirectToAction("Index");
    }
}

