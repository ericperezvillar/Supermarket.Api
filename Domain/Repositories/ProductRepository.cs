using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> ListProductAsync()
        {
            return await GetList().Include(p => p.Category).ToListAsync();
        }
        
        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<Product> FindProductByName(string productName)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Name == productName);
        }
    }
}