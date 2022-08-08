using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;

namespace Discount.gRPC.Services;
public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    #region Ctor
    private readonly IDiscountRepository _discountRepository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;
    public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _logger = logger;
        _mapper = mapper;
    }

    #endregion
    #region GetDiscount
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        if (coupon == null) throw new RpcException(
            new Status(StatusCode.NotFound, $"discount with product name : {request.ProductName} not founded"));

        _logger.LogInformation("Discount is retrived for product name");
        return _mapper.Map<CouponModel>(coupon);
    }
    #endregion
    #region CreateDiscount
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        var res = await _discountRepository.CreateDiscount(coupon);

        _logger.LogInformation($"discount is successfully created for product {coupon.ProductName}");
        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }
    #endregion
    #region UpdateDiscount
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _discountRepository.UpdateDiscount(coupon);

        _logger.LogInformation($"discoiunt is succesfully updated for product name {coupon.ProductName}");
        return _mapper.Map<CouponModel>(coupon);
    }
    #endregion
    #region DeleteDiscount
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
        var response = new DeleteDiscountResponse { Succes = deleted };
        return response;
    }
    #endregion
}