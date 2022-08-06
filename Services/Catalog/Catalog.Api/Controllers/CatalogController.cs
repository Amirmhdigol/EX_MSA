using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    #region Ctor
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }
    #endregion

    #region GetProducts
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return Ok(products);
    }
    #endregion

    #region GetProductById
    [HttpGet("{productId:length(24)}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Product>> GetProductById(string productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
        {
            _logger.LogError($"product with {productId} not found");
            return NotFound();
        }
        return Ok(product);
    }
    #endregion

    #region GetProductByCategoryName
    [HttpGet("[action]/{category}")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var product = await _productRepository.GetProductByCategory(category);
        if (product == null)
        {
            _logger.LogError($"product with {category} not found");
            return NotFound();
        }
        return Ok(product);
    }
    #endregion

    #region CreateProduct
    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await _productRepository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { productid = product.Id }, product);
    }
    #endregion

    #region UpdateProduct
    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> EditProduct([FromBody] Product product)
    {
        return Ok(await _productRepository.UpdateProduct(product));
    }
    #endregion

    #region DeleteProduct
    [HttpDelete("{productId:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> EditProduct(string productId)
    {
        return Ok(await _productRepository.DeleteProduct(productId));
    }
    #endregion
}