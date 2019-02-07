using FlightsService.Data.Abstract;
using FlightsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlightsService.Data
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Flight> Flights { get; set; }

        async Task<int> IUnitOfWork.SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
