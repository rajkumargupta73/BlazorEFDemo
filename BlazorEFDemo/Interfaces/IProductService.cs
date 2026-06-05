using BlazorEFDemo.Common;
using BlazorEFDemo.DTOs;
using BlazorEFDemo.Models;

namespace BlazorEFDemo.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<List<ProductDto>> GetByCategoryAsync(int categoryId);
        Task<List<ProductDto>> GetActiveAsync();
        Task<(List<ProductDto> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize,
            string? keyword = null,
            int? categoryId = null);
        Task<ServiceResult> CreateAsync(CreateProductDto dto);
        Task<ServiceResult> UpdateAsync(UpdateProductDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }


}
