namespace Basket.Api.Entites;
public class ShopCart
{
    public ShopCart()
    {
    }
    public ShopCart(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
    public List<ShopCartItem> Items { get; set; }
    public decimal TotalPrice
    {
        get
        {
            decimal total = 0;
            if (Items != null && Items.Any())
            {
                foreach (var item in Items)
                {
                    total += item.Price * item.Count;
                }
            }
            return total;
        }
    }
}