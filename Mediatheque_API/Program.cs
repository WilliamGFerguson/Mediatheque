using Microsoft.EntityFrameworkCore;
using Mediatheque_DAL.Models;
using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGenreDAO, GenreMySqlDAO>();
builder.Services.AddScoped<IMediaGenreDAO, MediaGenreMySqlDAO>();
builder.Services.AddScoped<IMediaItemDAO, MediaItemMySqlDAO>();
builder.Services.AddScoped<IMediaStatusDAO, MediaStatusMySqlDAO>();
builder.Services.AddScoped<IUserDAO, UserMySqlDAO>();
builder.Services.AddScoped<DbInitializer>();

builder.Services.AddDbContext<MediaDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    initializer.SeedMedias();
    initializer.SeedUsers();
}

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
