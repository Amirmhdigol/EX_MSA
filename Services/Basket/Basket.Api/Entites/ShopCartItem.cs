namespace Basket.Api.Entites;
public class ShopCartItem
{
    public int Count { get; set; }
    public string Color { get; set; }
    public long Price { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
}