namespace VectorSearchWithMssql.Services
{
    public interface IOllamaEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts);
        Task<bool> IsModelAvailableAsync();
    }
}
