using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T resource);

        Task<T> FindByIdAsync(int id);

    }
}