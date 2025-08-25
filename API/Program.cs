using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SERVICES //

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//scoped to lifetime of http request then it gets cleaned up(disposed of)
//need to specify the interface before the implementation class since i'm using the repository pattern
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// MIDDLEWARE //

// Configure the HTTP request pipeline.

app.MapControllers();


//seed our data
try
{
    //using means that any code we write using this variable (scope) will be disposed of after use by the framework
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    //applies any pending migrations and creates database if it does not exist
    await context.Database.MigrateAsync();
    //finally run the seed data method
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

app.Run();
