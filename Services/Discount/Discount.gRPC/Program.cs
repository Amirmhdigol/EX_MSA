using Discount.gRPC.Repositories;
using Discount.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IDiscountRepository,DiscountRepository>();
builder.Services.AddGrpc();
var app = builder.Build();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();
