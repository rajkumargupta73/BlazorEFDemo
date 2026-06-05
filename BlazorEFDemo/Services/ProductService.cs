using BlazorEFDemo.Common;
using BlazorEFDemo.Data;
using BlazorEFDemo.DTOs;
using BlazorEFDemo.Interfaces;
using BlazorEFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) => _context = context;

        // ── Mapping helper ──────────────────────────────────────────
        private static ProductDto ToDto(Product p) => new(
            p.Id, p.Name, p.Description, p.Price, p.Stock,
            p.IsActive, p.CategoryId,
            p.Category?.Name ?? string.Empty,
            p.CreatedAt, p.UpdatedAt);

        public async Task<List<ProductDto>> GetAllAsync()
            => await _context.Products
                             .Include(p => p.Category)
                             .OrderBy(p => p.Name)
                             .Select(p => new ProductDto(
                                 p.Id, p.Name, p.Description,
                                 p.Price, p.Stock, p.IsActive,
                                 p.CategoryId,
                                 p.Category!.Name,
                                 p.CreatedAt, p.UpdatedAt))
                             .ToListAsync();

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefaultAsync(p => p.Id == id);
            return p == null ? null : ToDto(p);
        }

        public async Task<List<ProductDto>> GetByCategoryAsync(int categoryId)
            => await _context.Products
                             .Include(p => p.Category)
                             .Where(p => p.CategoryId == categoryId)
                             .Select(p => new ProductDto(
                                 p.Id, p.Name, p.Description,
                                 p.Price, p.Stock, p.IsActive,
                                 p.CategoryId, p.Category!.Name,
                                 p.CreatedAt, p.UpdatedAt))
                             .ToListAsync();

        public async Task<List<ProductDto>> GetActiveAsync()
            => await _context.Products
                             .Include(p => p.Category)
                             .Where(p => p.IsActive)
                             .Select(p => new ProductDto(
                                 p.Id, p.Name, p.Description,
                                 p.Price, p.Stock, p.IsActive,
                                 p.CategoryId, p.Category!.Name,
                                 p.CreatedAt, p.UpdatedAt))
                             .ToListAsync();

        public async Task<(List<ProductDto> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize,
            string? keyword = null,
            int? categoryId = null)
        {
            var query = _context.Products
                                .Include(p => p.Category)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p =>
                    p.Name.Contains(keyword) ||
                    (p.Description != null && p.Description.Contains(keyword)));

            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            int total = await query.CountAsync();
            var items = await query
                             .OrderBy(p => p.Name)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(p => new ProductDto(
                                 p.Id, p.Name, p.Description,
                                 p.Price, p.Stock, p.IsActive,
                                 p.CategoryId, p.Category!.Name,
                                 p.CreatedAt, p.UpdatedAt))
                             .ToListAsync();
            return (items, total);
        }

        public async Task<ServiceResult> CreateAsync(CreateProductDto dto)
        {
            if (dto.CategoryId == 0)
                return ServiceResult.Fail("Please select a category.");

            bool nameExists = await _context.Products
                                            .AnyAsync(p =>
                                                p.Name.ToLower() == dto.Name.ToLower());
            if (nameExists)
                return ServiceResult.Fail($"'{dto.Name}' already exists.");

            bool catExists = await _context.Categories
                                           .AnyAsync(c => c.Id == dto.CategoryId);
            if (!catExists)
                return ServiceResult.Fail("Selected category does not exist.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok("Product created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductDto dto)
        {
            if (dto.CategoryId == 0)
                return ServiceResult.Fail("Please select a category.");

            var product = await _context.Products.FindAsync(dto.Id);
            if (product == null)
                return ServiceResult.Fail("Product not found.");

            bool duplicate = await _context.Products
                                           .AnyAsync(p =>
                                               p.Name.ToLower() == dto.Name.ToLower()
                                               && p.Id != dto.Id);
            if (duplicate)
                return ServiceResult.Fail(
                    $"Another product named '{dto.Name}' already exists.");

            bool catExists = await _context.Categories
                                           .AnyAsync(c => c.Id == dto.CategoryId);
            if (!catExists)
                return ServiceResult.Fail("Selected category does not exist.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return ServiceResult.Ok("Product updated.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ServiceResult.Fail("Product not found.");

            if (product.IsActive)
                return ServiceResult.Fail(
                    $"'{product.Name}' is active. Deactivate it before deleting.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok($"'{product.Name}' deleted.");
        }
    }
}

