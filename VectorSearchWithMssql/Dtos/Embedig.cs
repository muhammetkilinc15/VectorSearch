using System.ComponentModel.DataAnnotations;

namespace VectorSearchWithMssql.Dtos
{
    // DTOs
    public record EmbeddingRequest([Required] string Text);
    public record BatchEmbeddingRequest([Required] List<string> Texts);
    public record EmbeddingResponse(float[] Embedding, int Dimension, string Text);
    public record BatchEmbeddingResponse(List<EmbeddingResponse> Embeddings, int TotalCount);
    public record HealthResponse(string Status, string Model, int? Dimension = null);
    public record ErrorResponse(string Message, string? Details = null);

}
