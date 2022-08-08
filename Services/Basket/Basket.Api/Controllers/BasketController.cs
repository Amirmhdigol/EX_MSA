using Basket.Api.Entites;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    #region Ctor
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _grpcService;
    public BasketController(IBasketRepository basketRepository, DiscountGrpcService gerpcService)
    {
        _basketRepository = basketRepository;
        _grpcService = gerpcService;
    }

    #endregion
    #region GetBasket
    [HttpGet("userName", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShopCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShopCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetUserBasket(userName);
        return Ok(basket ?? new ShopCart(userName));
    }

    #endregion
    #region UpdateBasket
    [HttpPost]
    [ProducesResponseType(typeof(ShopCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShopCart>> UpdateBasket([FromBody] ShopCart basket)
    {
        foreach (var item in basket.Items)
        {
            var coupon = await _grpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amout;
        }
        return Ok(await _basketRepository.UpdateBasket(basket));
    }
    #endregion
    #region DeleteBasket
    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _basketRepository.DelteBasket(userName);
        return Ok();
    }
    #endregion
}