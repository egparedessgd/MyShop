using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static FunctionalExtensions.FunctionalHelpers;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        ProductCategoryRepository context;

        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductCategory product = new ProductCategory();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory product)
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
        public ActionResult Edit(ProductCategory product, string Id)
        {
            return FindOrHttpNotFound(
                () => context.Find(Id),
                (productToEdit) =>
                {
                    productToEdit.Category = product.Category;

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
                    context.Delete(Id);
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
