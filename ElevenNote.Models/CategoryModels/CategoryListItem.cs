using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models.CategoryModels
{
    public class CategoryListItem
    {
        public int CategoryId { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
    }
}
