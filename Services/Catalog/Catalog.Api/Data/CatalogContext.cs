using Catalog.Api.Entities;
using MongoDB.Driver;
namespace Catalog.Api.Data;
public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString")); //Set Connection with Mongo
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName")); //Set Connection with Mongo Database
        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName")); //Set the collection (former entity)
        CatalogContextSeed.SeedData(Products);
    }
    public IMongoCollection<Product> Products { get; }
}
     