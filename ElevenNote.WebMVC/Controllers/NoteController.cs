using ElevenNote.Data;
using ElevenNote.Models.NoteModels;
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
    public class NoteController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Note
        public ActionResult Index()
        {

            var service = CreateNoteService();
            var model = service.GetNotes();
            return View(model);
        }

        //GET: Note/Create
        public ActionResult Create()
        {
            ViewData["Categories"] = _db.Categories.Select(category => new SelectListItem
            {
                Text = category.Title,
                Value = category.CategoryId.ToString(),
            });

            return View();
        }

        //POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            ViewData["Categories"] = _db.Categories.Select(category => new SelectListItem
            {
                Text = category.Title,
                Value = category.CategoryId.ToString(),
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateNoteService();

            if (service.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created";
                return RedirectToAction("Index");
            }

            return View(model);
           
        }

        public ActionResult Details(int id)
        {
            ViewData["Categories"] = _db.Categories.Select(category => new SelectListItem
            {
                Text = category.Title,
                Value = category.CategoryId.ToString(),
            });

            var service = CreateNoteService();
            var model = service.GetNoteById(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {

            var detail = _db.Notes.Find(id);

            var model = new NoteEdit
            {
                Content = detail.Content,
                NoteId = detail.NoteId,
                Title = detail.Title,
                CategoryId = detail.CategoryId
            };

            ViewData["Categories"] = _db.Categories.Select(category => new SelectListItem
            {
                Text = category.Title,
                Value = category.CategoryId.ToString(),
            });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            ViewData["Categories"] = _db.Categories.Select(category => new SelectListItem
            {
                Text = category.Title,
                Value = category.CategoryId.ToString(),
            });

            if (ModelState.IsValid)
            {
                var service = CreateNoteService();

                service.EditNote(model);
                TempData["SaveResult"] = "Note successfully edited";
                return RedirectToAction("Index");
            }

            if(model.NoteId != id)
            {
                ModelState.AddModelError("", "Id mismatch");
                return View(model);
            }

            ModelState.AddModelError("", "Your note could not be updated");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);

            new NoteDetail
            {
                Content = model.Content,
                CreatedUtc = model.CreatedUtc,
                ModifiedUtc = model.ModifiedUtc,
                NoteId = model.NoteId,
                Title = model.Title
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNote(int id)
        {
            var service = CreateNoteService();

            if (service.DeleteNote(id))
            {
                TempData["SaveResult"] = "Note successfully deleted";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Note could not be deleted");
            return View();
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}