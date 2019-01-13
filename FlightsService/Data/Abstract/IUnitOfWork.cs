using System.Threading.Tasks;

namespace FlightsService.Data.Abstract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
