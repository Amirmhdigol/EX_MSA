using Basket.Api.Entites;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories;
public interface IBasketRepository
{
    Task<ShopCart> GetUserBasket(string userName);
    Task<ShopCart> UpdateBasket(ShopCart shopCart);
    Task DelteBasket(string userName);
}

public class BasketRepository : IBasketRepository
{
    #region Ctor
    private readonly IDistributedCache _distributedCache;
    public BasketRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    #endregion
    public async Task DelteBasket(string userName)
    {
        await _distributedCache.RemoveAsync(userName);
    }
    public async Task<ShopCart> GetUserBasket(string userName)
    {
        var basket = await _distributedCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(basket))
            return null;

        return JsonConvert.DeserializeObject<ShopCart>(basket);
    }
    public async Task<ShopCart> UpdateBasket(ShopCart shopCart)
    {
        await _distributedCache.SetStringAsync(shopCart.UserName, JsonConvert.SerializeObject(shopCart));
        return await GetUserBasket(shopCart.UserName);
    }
}