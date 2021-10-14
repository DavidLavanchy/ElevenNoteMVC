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

        public IEnumerable<CategoryListItem> GetCategories()
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Categories
                    .Where(e => _userId == e.OwnerId)
                    .Select(e =>
                    new CategoryListItem
                    {
                        CategoryId = e.CategoryId,
                        Title = e.Title,
                    });

                return query.ToArray();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Categories
                    .Single(e => _userId == e.OwnerId && id == e.CategoryId);
                return
                new CategoryDetail
                {
                    CategoryId = entity.CategoryId,
                    Title = entity.Title,
                    Description = entity.Description
                };

            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Categories
                    .Single(e => _userId == e.OwnerId && model.CategoryId == e.CategoryId);

                entity.CategoryId = model.CategoryId;
                entity.Description = model.Description;
                entity.Title = model.Title;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int id)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Categories
                    .Single(e => e.CategoryId == id & _userId == e.OwnerId);

                ctx.Categories.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
