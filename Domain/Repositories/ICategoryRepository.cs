using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> ListAsync(); 
	    void Update(Category category);
        void Delete(Category category);
        Task<Category> FindCategoryByName(string categoryName);
    } 
}