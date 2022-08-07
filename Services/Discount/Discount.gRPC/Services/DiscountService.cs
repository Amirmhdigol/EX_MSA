using AutoMapper;
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
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        if (coupon == null) throw new RpcException(
            new Status(StatusCode.NotFound, $"discount with product name : {request.ProductName} not founded"));

        _logger.LogInformation("Discount is retrived for product name");
        return _mapper.Map<CouponModel>(coupon);
    }
}