using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Logging;

namespace VectorSearchWithMssql.Services;

public class OllamaEmbeddingService : IOllamaEmbeddingService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingService;
    private readonly ILogger<OllamaEmbeddingService> _logger;

    public OllamaEmbeddingService(
        IEmbeddingGenerator<string, Embedding<float>> embeddingService,
        ILogger<OllamaEmbeddingService> logger)
    {
        _embeddingService = embeddingService;
        _logger = logger;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        _logger.LogDebug("Generating embedding for text of length {Length}", text.Length);

        var embedding = await _embeddingService.GenerateAsync(text);
        return embedding.Vector.ToArray();
    }

    public async Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts)
    {
        _logger.LogDebug("Generating embeddings for {Count} texts", texts.Count);

        var results = await Task.WhenAll(texts.Select(GenerateEmbeddingAsync));
        return results.ToList();
    }

}
