using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models.CategoryModels
{
    public class CategoryCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "There are too many characters in this field. Title must be less than or equal to 50 characters")]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
    }
}
