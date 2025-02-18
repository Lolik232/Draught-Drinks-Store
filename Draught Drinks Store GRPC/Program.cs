using Draught_Drinks_Store_GRPC.Services.v1;
using DAL.EFCore.BackgroundServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.BussinesLogic.Di;
using DAL.Abstractions.Interfaces.Repositories;
using DAL.EFCore.Repositories;
using DAL.Neo4j.Neo4jDriver.Repository;
using DAL.EFCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using DAL.Abstractions.Interfaces.Payment;
using Payment.YooKassa;
using Draught_Drinks_Store_GRPC;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();


builder.Services.AddDbContext<ShopContext>(
    opts => opts.UseNpgsql($"Host={builder.Configuration["Configs:Postgres:Host"]!};" +
    $"Port={builder.Configuration["Configs:Postgres:Port"]!};" +
    $"Database={builder.Configuration["Configs:Postgres:Database"]!};" +
    $"Username={builder.Configuration["Configs:Postgres:Username"]!};" +
    $"Password={builder.Configuration["Configs:Postgres:Password"]!}")
    );


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IContactDataRepository, ContactDataRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<IPaymentService, YooKasssaPayment>();

builder.Services.AddBussinesLogicServices();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Access-Control-Allow-Headers", "Authrorization", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding", "X-Grpc-Web", "User-Agent");
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(
        o =>
        {
            var validator = new JwtTokenValidator();
            o.SecurityTokenValidators.Add(validator);
        }
        );

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors();

app.MapGrpcService<CategoryService>().EnableGrpcWeb().RequireCors("AllowAll");
app.MapGrpcService<ProductService>().EnableGrpcWeb().RequireCors("AllowAll");
app.MapGrpcService<UserService>().EnableGrpcWeb().RequireCors("AllowAll");
app.MapGrpcService<OrderService>().EnableGrpcWeb().RequireCors("AllowAll");

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.UseCors();
app.Run();
