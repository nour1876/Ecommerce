using Infrastructure.Data; // Ensure this namespace contains your StoreContext
using Microsoft.EntityFrameworkCore; // Required for EF Core

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init();
// Add services to the container.
builder.Services.AddControllers();

// Configure the DbContext with SQLite
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
