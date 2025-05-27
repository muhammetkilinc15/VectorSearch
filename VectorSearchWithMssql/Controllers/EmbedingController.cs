using Microsoft.AspNetCore.Mvc;
using VectorSearchWithMssql.Dtos;
using VectorSearchWithMssql.Services;

namespace VectorSearchWithMssql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddingController : ControllerBase
    {
        private readonly IOllamaEmbeddingService _service;
        private readonly ILogger<EmbeddingController> _logger;

        public EmbeddingController(IOllamaEmbeddingService service, ILogger<EmbeddingController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("generate")]
        [ProducesResponseType(typeof(EmbeddingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateEmbedding([FromBody] EmbeddingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new ErrorResponse("Text cannot be empty"));
            }

            try
            {
                _logger.LogInformation("Generating embedding for text: {Text}", request.Text[..Math.Min(50, request.Text.Length)] + "...");

                var embedding = await _service.GenerateEmbeddingAsync(request.Text);
                var response = new EmbeddingResponse(embedding, embedding.Length, request.Text);

                _logger.LogInformation("Successfully generated embedding with dimension: {Dimension}", embedding.Length);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embedding");
                return Problem(
                    detail: ex.Message,
                    title: "Embedding Generation Failed",
                    statusCode: 500
                );
            }
        }

        [HttpPost("generate-batch")]
        [ProducesResponseType(typeof(BatchEmbeddingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateBatchEmbeddings([FromBody] BatchEmbeddingRequest request)
        {
            if (request.Texts == null || !request.Texts.Any())
            {
                return BadRequest(new ErrorResponse("Texts list cannot be empty"));
            }

            if (request.Texts.Any(string.IsNullOrWhiteSpace))
            {
                return BadRequest(new ErrorResponse("All texts must be non-empty"));
            }

            try
            {
                _logger.LogInformation("Generating embeddings for {Count} texts", request.Texts.Count);

                var embeddings = await _service.GenerateEmbeddingsAsync(request.Texts);
                var responses = request.Texts.Select((text, i) =>
                    new EmbeddingResponse(embeddings[i], embeddings[i].Length, text)).ToList();

                var batchResponse = new BatchEmbeddingResponse(responses, embeddings.Count);

                _logger.LogInformation("Successfully generated {Count} embeddings", embeddings.Count);
                return Ok(batchResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating batch embeddings");
                return Problem(
                    detail: ex.Message,
                    title: "Batch Embedding Generation Failed",
                    statusCode: 500
                );
            }
        }
    }
}
