using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> ListProductAsync();

        Task<Product> FindProductByName(string productName);

        void Update(Product product);

        void Delete(Product product);
    }
}