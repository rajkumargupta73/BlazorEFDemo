namespace BlazorEFDemo.DTOs
{
    // What the UI reads
    public record ProductDto(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        bool IsActive,
        int CategoryId,
        string CategoryName,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

}
