namespace VectorSearchWithMssql.Dtos
{
    public record CreateProductDto(string Name, decimal Price,string Description, string Category);
    public record UpdateProductDto(int Id, string Name, decimal Price, string Description, string Category);
    public record DeleteProductDto(int Id);
    public record ProductDto(int Id, string Name, decimal Price, string Description, string Category, string ImageUrl, string Vector);
}
