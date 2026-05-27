using BlazorEFDemo.Models;

namespace BlazorEFDemo.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetByCategoryAsync(int categoryId);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<List<Product>> SearchAsync(string keyword);
        Task<(List<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    }

}
