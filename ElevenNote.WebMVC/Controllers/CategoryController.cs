using ElevenNote.Models.CategoryModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var query = service.GetCategories();
            return View(query);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            var service = CreateCategoryService();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (service.CreateCategory(model))
            {
                TempData["SaveResult"] = "Category successfully created";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var service = CreateCategoryService();

            var model = service.GetCategoryById(id);

            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateCategoryService();

            var entity = service.GetCategoryById(id);

            var model =
                new CategoryEdit
                {
                    CategoryId = entity.CategoryId,
                    Description = entity.Description,
                    Title = entity.Title
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryEdit model)
        {
            if(model.CategoryId != id)
            {
                ModelState.AddModelError("", "Entered ID and the model ID do not match");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var service = CreateCategoryService();
                service.UpdateCategory(model);
                TempData["SaveResult"] = "Category successfully updated";
                return RedirectToAction("Index");
            }

            TempData["SaveResult"] = "Updates could not be saved";
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var service = CreateCategoryService();

            var entity = service.GetCategoryById(id);

            var model =
                new CategoryDetail
                {
                    CategoryId = entity.CategoryId,
                    Description = entity.Description,
                    Title = entity.Title
                    
                };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            var service = CreateCategoryService();

            if (service.DeleteCategory(id))
            {
                TempData["SaveResult"] = "Category successfully deleted.";
                return RedirectToAction("Index");
            }

            TempData["SaveResult"] = "Could not delete category.";
            return View();
        }

        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }
    }
}