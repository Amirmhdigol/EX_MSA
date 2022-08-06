using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories;
public class ProductRepository : IProductRepository
{
    #region Ctor
    private readonly ICatalogContext _catalogContext;
    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    #endregion

    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    { //FlterDefenition is like lambda filter expression but for more complex filterings
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(a => a.Id, id); //Eq is Same as '==' - *** NOTE *** => Eq and Equal are different Equal means if the current obj(product) is equal to the given parameter
        DeleteResult result = await _catalogContext.Products.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0; //IsAcknowledge means the result is confirmed or not
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string category)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(n => n.Category, category);
        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    public async Task<Product> GetProductById(string id)
    {
        return await _catalogContext.Products.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(n => n.Name, name);
        return await _catalogContext.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catalogContext.Products.Find(a => true).ToListAsync();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _catalogContext.Products.ReplaceOneAsync(a => a.Id == product.Id, product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
}