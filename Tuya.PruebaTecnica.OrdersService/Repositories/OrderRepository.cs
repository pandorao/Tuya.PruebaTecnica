using Microsoft.EntityFrameworkCore;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.OrderService.Data;

namespace Tuya.PruebaTecnica.OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order model);
        Task UpdateAsync(Order model);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public OrderRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Order>> GetAllAsync() 
        {
            return await _dbcontext.Orders.AsNoTracking()
                .Include(p => p.OrderProducts).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _dbcontext.Orders
                .Include(p => p.OrderProducts)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Order model)
        {
            _dbcontext.Orders.Add(model);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order model)
        {
            _dbcontext.Orders.Update(model);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
