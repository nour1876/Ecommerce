using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data; // Ensure this namespace contains your StoreContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for EF Core

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init();

builder.Services.AddAutoMapper(typeof(MappingProfiles));
// Add services to the container.
builder.Services.AddControllers();

// Configure the DbContext with SQLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
//Added By NoB
builder.Services.AddApplicationServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//NoB
builder.Services.AddSwaggerDocumentations();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerfactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex){
        var logger = loggerfactory.CreateLogger<Program>();
        logger.LogError(ex, "An  Error occured during migration");

    }
}
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
    //errors
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
