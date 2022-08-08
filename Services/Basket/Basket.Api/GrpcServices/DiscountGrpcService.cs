using Discount.gRPC.Protos;

namespace Basket.Api.GrpcServices;
public class DiscountGrpcService
{
    #region Ctor
    private readonly DiscountProtoService.DiscountProtoServiceClient _service;
    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient serviceClient)
    {
        _service = serviceClient;
    }
    #endregion
    #region GetDiscount
    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountReq = new GetDiscountRequest { ProductName = productName };
        return await _service.GetDiscountAsync(discountReq);
    }
    #endregion
    #region CreateDiscount
    public async Task<CouponModel> CreateDiscount(CouponModel couponModel)
    {
        var discountReq = new CreateDiscountRequest { Coupon = couponModel };
        var createdDiscount = await _service.CreateDiscountAsync(discountReq);
        return createdDiscount;
    }
    #endregion
    #region UpdateDiscount
    public async Task<CouponModel> UpdateDiscount(CouponModel couponModel)
    {
        var discountReq = new UpdateDiscountRequest { Coupon = couponModel };
        var updatedDiscount = await _service.UpdateDiscountAsync(discountReq);
        return updatedDiscount;
    }
    #endregion
    #region DeleteDiscount
    public async Task<DeleteDiscountResponse> DeleteDiscount(string productName)
    {
        var discountReq = new DeleteDiscountRequest { ProductName = productName };
        return await _service.DeleteDiscountAsync(discountReq);
    }
    #endregion

}