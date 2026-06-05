using BlazorEFDemo.Common;
using BlazorEFDemo.DTOs;
using BlazorEFDemo.Models;

namespace BlazorEFDemo.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<List<CategoryDto>> GetAllWithProductCountAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<List<CategoryDto>> SearchAsync(string keyword);
        Task<ServiceResult> CreateAsync(CreateCategoryDto dto);
        Task<ServiceResult> UpdateAsync(UpdateCategoryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }


}
