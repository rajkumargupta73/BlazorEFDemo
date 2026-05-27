using BlazorEFDemo.Data;
using BlazorEFDemo.Interfaces;
using BlazorEFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) => _context = context;

        public async Task<List<Product>> GetAllAsync()
            => await _context.Products.Include(p => p.Category)
                             .OrderBy(p => p.Name).ToListAsync();

        public async Task<List<Product>> GetByCategoryAsync(int categoryId)
            => await _context.Products.Include(p => p.Category)
                             .Where(p => p.CategoryId == categoryId).ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products.Include(p => p.Category)
                             .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> CreateAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> SearchAsync(string keyword)
            => await _context.Products.Include(p => p.Category)
                             .Where(p => p.Name.Contains(keyword) ||
                                         p.Description!.Contains(keyword))
                             .ToListAsync();

        public async Task<(List<Product> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();
            int total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize).ToListAsync();
            return (items, total);
        }
    }

}
