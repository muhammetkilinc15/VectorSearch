
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using VectorSearchWithMssql.Context;
using VectorSearchWithMssql.Dtos;
using VectorSearchWithMssql.Helper;
using VectorSearchWithMssql.Models;
using VectorSearchWithMssql.Services;

namespace VectorSearchWithMssql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly IOllamaEmbeddingService _ollamaEmbeddingService;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger, IOllamaEmbeddingService ollamaEmbeddingService)
        {
            _context = context;
            _logger = logger;
            _ollamaEmbeddingService = ollamaEmbeddingService;
        }

        [HttpPost]
        [Route("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            Product newProduct = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                EmbedingVector = await _ollamaEmbeddingService.GenerateEmbeddingAsync($"{product.Name} {product.Description} {product.Category}")
            };

            await _context.Products.AddAsync(newProduct, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok("Product added successfully");
        }
        [HttpPost]
        [Route("add-products")]
        public async Task<IActionResult> AddProducts([FromBody] List<CreateProductDto> products, CancellationToken cancellationToken)
        {
            var newProducts = new List<Product>();
            foreach (var product in products)
            {
                Product newProduct = new()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Category = product.Category,
                    EmbedingVector = await _ollamaEmbeddingService.GenerateEmbeddingAsync($"{product.Name} {product.Description} {product.Category}")
                };
                newProducts.Add(newProduct);
            }
            await _context.Products.AddRangeAsync(newProducts, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok("Products added successfully");
        }

        [HttpGet]
        [Route("get-products")]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _context
                .Products
                .AsNoTracking()
                .AsQueryable()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.Category
                })
                .ToListAsync(cancellationToken);

            return Ok(products);
        }

        [HttpGet]
        [Route("get-products-by-similarity")]
        public async Task<IActionResult> GetProductWithCosineSimilarity(string text, int similaritType, CancellationToken cancellationToken)
        {
            // Creating embedding vector for the input text
            float[] embeddingFloatArray = await _ollamaEmbeddingService.GenerateEmbeddingAsync(text);

            // get all products from the database and their embedding vectors
            var products = await _context.Products
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Price,
                    x.Category,
                    Vector = x.EmbedingVector
                })
                .ToListAsync(cancellationToken);

            var closestProduct = products
                .Select(p => new
                {
                    Product = p,
                    Similarity = similaritType switch
                    {
                        1 => SimilartyCalculater.CalculateManhattanDistance(embeddingFloatArray, p.Vector),
                        2 => SimilartyCalculater.CalculateEuclideanDistance(embeddingFloatArray, p.Vector),
                        _ => SimilartyCalculater.CalculateCosineSimilarity(embeddingFloatArray, p.Vector)
                    }
                })
                .OrderByDescending(x => x.Similarity);

            var result = new List<dynamic>();

            foreach (var product in closestProduct)
            {
                if (product.Similarity > 0.5f)
                {
                    result.Add(new
                    {
                        product.Product.Id,
                        product.Product.Name,
                        product.Product.Description,
                        product.Product.Price,
                        product.Product.Category,
                        SimilarityScore = product.Similarity
                    });
                }
            }

            return Ok(result);
        }



    }

}
