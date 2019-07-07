using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

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

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            return BasicViewFoundNotFound(() => context.Find(Id));

            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(String Id)
        {
            return BasicFindHttpNotFound(
                () => context.Find(Id),
                (x) =>
                {
                    context.Delete(Id);
                    context.Commit();
                    return RedirectToHome;
                });
        }

        public ActionResult ExecuteOnFound<TFindType>
            (Func<TFindType> find, Func<TFindType, ActionResult> OnFound, Func<TFindType, ActionResult> OnNotFound)
            where TFindType : class
        {
            TFindType t = find();
            if(t == null)
            {
                return OnNotFound(t);
            }
            else
            {
                return OnFound(t);
            }
        }

        public ActionResult BasicFindHttpNotFound<TFindType>
            (Func<TFindType> find, Func<TFindType, ActionResult> OnFound) where TFindType : class =>
            ExecuteOnFound(
                find,
                OnFound,
                (x) => HttpNotFound());

        public ActionResult BasicViewFoundNotFound<TFindType>(Func<TFindType> find) where TFindType : class =>
            BasicFindHttpNotFound(find, (x) => View(x));

        public ActionResult RedirectToHome => RedirectToAction("Index");
    }
}

public static class FunctionalExt
{
    public struct Unit { }

    public static Unit ActionToFunc(this Action action)
    {
        action();
        return new Unit();
    }
}
