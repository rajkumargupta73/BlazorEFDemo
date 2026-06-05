using BlazorEFDemo.Common;
using BlazorEFDemo.Data;
using BlazorEFDemo.DTOs;
using BlazorEFDemo.Interfaces;
using BlazorEFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFDemo.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
            => _context = context;

        public async Task<List<CategoryDto>> GetAllAsync()
            => await _context.Categories
                             .OrderBy(c => c.Name)
                             .Select(c => new CategoryDto(
                                 c.Id, c.Name, c.Description, 0))
                             .ToListAsync();

        public async Task<List<CategoryDto>> GetAllWithProductCountAsync()
            => await _context.Categories
                             .Include(c => c.Products)
                             .OrderBy(c => c.Name)
                             .Select(c => new CategoryDto(
                                 c.Id, c.Name, c.Description,
                                 c.Products.Count))
                             .ToListAsync();

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return null;
            return new CategoryDto(c.Id, c.Name, c.Description, 0);
        }

        public async Task<List<CategoryDto>> SearchAsync(string keyword)
            => await _context.Categories
                             .Where(c => c.Name.Contains(keyword) ||
                                         (c.Description != null &&
                                          c.Description.Contains(keyword)))
                             .Select(c => new CategoryDto(
                                 c.Id, c.Name, c.Description, 0))
                             .ToListAsync();

        public async Task<ServiceResult> CreateAsync(CreateCategoryDto dto)
        {
            bool exists = await _context.Categories
                                        .AnyAsync(c =>
                                            c.Name.ToLower() == dto.Name.ToLower());
            if (exists)
                return ServiceResult.Fail($"'{dto.Name}' already exists.");

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok("Category created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.Id);
            if (category == null)
                return ServiceResult.Fail("Category not found.");

            bool duplicate = await _context.Categories
                                           .AnyAsync(c =>
                                               c.Name.ToLower() == dto.Name.ToLower()
                                               && c.Id != dto.Id);
            if (duplicate)
                return ServiceResult.Fail(
                    $"Another category named '{dto.Name}' already exists.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            await _context.SaveChangesAsync();
            return ServiceResult.Ok("Category updated.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await _context.Categories
                                         .Include(c => c.Products)
                                         .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return ServiceResult.Fail("Category not found.");

            if (category.Products.Any())
                return ServiceResult.Fail(
                    $"Cannot delete '{category.Name}' — " +
                    $"it has {category.Products.Count} product(s). " +
                    "Remove them first.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok($"'{category.Name}' deleted.");
        }
    }
}
