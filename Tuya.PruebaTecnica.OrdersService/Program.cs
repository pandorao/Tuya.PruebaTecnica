using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tuya.PruebaTecnica.OrderService.Data;
using Tuya.PruebaTecnica.OrderService.Repositories;
using Tuya.PruebaTecnica.SDK.Models;
using Tuya.PruebaTecnica.SDK.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServicesEndPointOptions>(auth =>
{
    auth.ProductServiceEndPoint = builder.Configuration.GetConnectionString("ProductServiceEndPoint");
    auth.DeliveryServiceEndPoint = builder.Configuration.GetConnectionString("DeliveryServiceEndPoint");
});

builder.Services.AddDbContext<ApplicationDbContext>(
    x => x.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IDeliveriesServices, DeliveriesServices>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("apiv1", new OpenApiInfo
    {
        Title = "API Docs",
        Description = "Documentacion de las APIs",
        Version = "v1"
    });

    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    foreach (var fi in dir.EnumerateFiles("*.xml"))
    {
        c.IncludeXmlComments(fi.FullName);
    }

    c.EnableAnnotations();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/apiv1/swagger.json", "Apis");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
