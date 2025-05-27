using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;

namespace VectorSearchWithMssql.Services
{
    public class OllamaEmbeddingService : IOllamaEmbeddingService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingService;
        private readonly ILogger<OllamaEmbeddingService> _logger;
        private readonly IConfiguration _configuration;
        public OllamaEmbeddingService(ILogger<OllamaEmbeddingService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            var builder = Kernel.CreateBuilder();
#pragma warning disable SKEXP0070
            builder.AddOllamaEmbeddingGenerator(
                modelId: _configuration["Ollama:Model"],
                endpoint: new Uri(_configuration["Ollama:Endpoint"])
            );
#pragma warning restore SKEXP0070

            var kernel = builder.Build();
            _embeddingService = kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            try
            {
                _logger.LogInformation("Generating embedding for text: {TextLength} characters", text.Length);

                var embedding = await _embeddingService.GenerateAsync(text);
                var result = embedding.Vector.ToArray();

                _logger.LogInformation("Generated embedding with dimension: {Dimension}", result.Length);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embedding for text");
                throw;
            }
        }

        public async Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts)
        {
            try
            {
                _logger.LogInformation("Generating embeddings for {Count} texts", texts.Count);

                var tasks = texts.Select(text => GenerateEmbeddingAsync(text));
                var results = await Task.WhenAll(tasks);

                _logger.LogInformation("Generated {Count} embeddings", results.Length);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embeddings for texts");
                throw;
            }
        }

        public async Task<bool> IsModelAvailableAsync()
        {
            try
            {
                _logger.LogInformation("Checking model availability");

                // Test embedding ile model kontrolü
                var testEmbedding = await _embeddingService.GenerateAsync("test");
                var isAvailable = testEmbedding?.Vector.Length > 0;

                _logger.LogInformation("Model availability: {IsAvailable}", isAvailable);
                return isAvailable;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Model availability check failed");
                return false;
            }
        }
    }
}