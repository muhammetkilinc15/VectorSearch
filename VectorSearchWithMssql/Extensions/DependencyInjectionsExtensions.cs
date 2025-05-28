using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.SemanticKernel;
using VectorSearchWithMssql.Services;

namespace VectorSearchWithMssql.Extensions
{
    public static class DependencyInjectionsExtensions
    {
        public static IServiceCollection AddOllamaEmbedding(this IServiceCollection services, IConfiguration configuration)
        {

            // Microsoft bu metotu değişebilir işaretlediği için uyarı koyduk
            #pragma warning disable SKEXP0070
            services.AddKernel().
                AddOllamaEmbeddingGenerator(
                modelId: configuration["Ollama:Model"],
                endpoint: new Uri(configuration["Ollama:Endpoint"])
            );
            #pragma warning restore SKEXP0070


            services.AddScoped<IOllamaEmbeddingService, OllamaEmbeddingService>();

            return services;
        }
    }
}
