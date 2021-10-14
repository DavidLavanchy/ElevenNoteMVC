using ElevenNote.Data;
using ElevenNote.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;
        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    new Category
                    {
                        Description = model.Description,
                        Title = model.Title  
                    };

                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
