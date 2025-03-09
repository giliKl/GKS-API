using GKD.Data;
using GKD.Data.Repositories;
using GKS.Core;
using GKS.Core.IRepositories;
using GKS.Core.IServices;
using GKS.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Service

builder.Services.AddScoped<IUserService, IUserService>();
builder.Services.AddScoped<IFileService, UserFileService>();



//Repository

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFileRepository, IUserFileRepository>();


//Data

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(@"Server=(localdb)\\MSSQLLocalDB;Database=GKS_db;Integrated Security=true");
});


//builder.Services.AddSingleton<DataContext>();
//

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ProfileMapping));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
