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
            return View();
        }

        //POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
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
            var service = CreateNoteService();
            var model = service.GetNoteById(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);

            var model =
                new NoteEdit
                {
                    Content = detail.Content,
                    NoteId = detail.NoteId,
                    Title = detail.Title
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
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

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}