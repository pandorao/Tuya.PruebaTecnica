using Microsoft.EntityFrameworkCore;
using Tuya.PruebaTecnica.Models.Models;
using Tuya.PruebaTecnica.ProductsService.Data;

namespace Tuya.PruebaTecnica.ProductsService.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(int[] ids = null);
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(Product model);
        Task AddAsync(Product model);
        Task RemoveAsync(Product model);
        bool AnyByIdAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProductRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Product>> GetAllAsync(int[] ids = null) 
        {
            if (ids == null || ids.Count() == 0)
            {
                return await _dbcontext.Products.AsNoTracking().ToListAsync();
            }

            return await _dbcontext.Products
                .Where(p => ids.Contains(p.Id))
                .AsNoTracking().
                ToListAsync();
        }

        public bool AnyByIdAsync(int id)
        {
            return _dbcontext.Products.Any(p => p.Id == id);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _dbcontext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Product model)
        {
            _dbcontext.Products.Update(model);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddAsync(Product model)
        {
            _dbcontext.Products.Add(model);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Product model)
        {
            _dbcontext.Products.Remove(model);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
