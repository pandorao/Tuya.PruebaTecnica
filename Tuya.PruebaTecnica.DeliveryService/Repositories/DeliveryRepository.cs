using Microsoft.EntityFrameworkCore;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.DeliveryService.Data;

namespace Tuya.PruebaTecnica.DeliveryService.Repositories
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetAllAsync();
        Task<Delivery> GetByIdAsync(int id);
        Task AddAsync(Delivery model);
    }

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public DeliveryRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Delivery>> GetAllAsync() 
        {
            return await _dbcontext.Deliveries.AsNoTracking().ToListAsync();
        }

        public async Task<Delivery> GetByIdAsync(int id)
        {
            return await _dbcontext.Deliveries.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Delivery model)
        {
            _dbcontext.Deliveries.Add(model);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
