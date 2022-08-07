using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories;
public interface IDiscountRepository
{
    Task<Coupon> GetDiscount(string productName);
    Task<bool> CreateDiscount(Coupon coupon);
    Task<bool> UpdateDiscount(Coupon coupon);
    Task<bool> DeleteDiscount(string productName);
}

public class DiscountRepository : IDiscountRepository
{
    #region Ctor
    private readonly IConfiguration _configuration;
    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var executed = await connection.ExecuteAsync("insert into Coupon(ProductName,Description,Amout)" +
            " values (@ProductName,@Description,@Amout)"
            , new { ProductName = coupon.ProductName, Description = coupon.Description, Amout = coupon.Amout });

        if (executed == 0) return false;
        return true;
    }
    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var executed = await connection.ExecuteAsync("delete from Coupon where ProductName=@ProductName", new { ProductName = productName });
        if (executed == 0) return false;
        return true;
    }
    public async Task<Coupon> GetDiscount(string productName)
    {
         using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName=@ProductName", new { ProductName = productName });
        if (coupon == null)
        {
            return new Coupon { ProductName = "No discount", Amout = 0, Description = "No Discount Description" };
        }
        return coupon;
    }
    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var executed = await connection.ExecuteAsync("update Coupon Set ProductName=@ProductName,Description=@Description,Amout=@Amout where Id=@CouponId",
            new { coupon.ProductName, coupon.Description, coupon.Amout ,CouponId=coupon.Id});

        if (executed == 0) return false;
        return true;
    }
}