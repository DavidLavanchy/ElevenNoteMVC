using ElevenNote.Data;
using ElevenNote.Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now,
                    Title = model.Title,
                };

            using(var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Notes
                    .Where(e=> e.OwnerId == _userId)
                    .Select(e =>
                    new NoteListItem
                    {
                        NoteId = e.NoteId,
                        CreatedUtc = e.CreatedUtc,
                        Title = e.Title
                    });

                return query.ToArray();
            }
        }

        public NoteDetail GetNoteById(int id)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Notes
                    .Single(e => id == e.NoteId && e.OwnerId == _userId);
                return
                    new NoteDetail
                    {
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc,
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                    };
            }
        }
    }
}
