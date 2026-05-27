using BlazorEFDemo.Data;
using BlazorEFDemo.Interfaces;
using BlazorEFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFDemo.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories
                             .OrderBy(c => c.Name)
                             .ToListAsync();

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories
                             .Include(c => c.Products) // Eager loading
                             .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string name)
            => await _context.Categories.AnyAsync(c => c.Name == name);
    }

}
