using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Communication;
using Resources;

namespace Domain.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ListAsync();

        Task<ProductResponse> SaveAsync(Product product);

        Task<ProductResponse> UpdateAsync(int id, Product product);

        Task<ProductResponse> DeleteAsync(int id);
    }
}