namespace BlazorEFDemo.DTOs
{
    // What the UI reads
    public record CategoryDto(
        int Id,
        string Name,
        string? Description,
        int ProductCount
    );

}
