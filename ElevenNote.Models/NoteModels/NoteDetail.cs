using ElevenNote.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Models.NoteModels
{
    public class NoteDetail
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }
        [Display(Name = "Modified")]
        public DateTimeOffset? ModifiedUtc { get; set; }
        public int CategoryId { get; set; }
        public string Categories { get; set; }
        public bool IsStarred { get; set; }
    }
}
