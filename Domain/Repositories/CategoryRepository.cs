using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        
        public void Update(Category category)
        {
	        _context.Categories.Update(category);
        }

        public void Delete(Category category)
        {
	        _context.Categories.Remove(category);
        }

        public async Task<Category> FindCategoryByName(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);
        }
    }
}