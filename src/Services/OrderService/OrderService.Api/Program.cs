using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Persistence;
using OrderService.Application.UseCases;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderServiceDb")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<AddOrderLineUseCase>();
builder.Services.AddScoped<PayOrderUseCase>();
builder.Services.AddScoped<GetOrderByIdUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
