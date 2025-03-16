using DotNetEnv;
using GKD.Data;
using GKD.Data.Repositories;
using GKS.Core;
using GKS.Core.IRepositories;
using GKS.Core.IServices;
using GKS.Service;
using GKS.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, UserFileService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService,RoleService>();
builder.Services.AddScoped<IPermissionService,PermissionService>();


// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFileRepository, UserFileRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();



//Data
builder.Services.AddScoped<IDataContext, DataContext>();

builder.Services.AddDbContext<DataContext>(options =>
{
    var conection = "Host=localhost;Port=5432;Database=GKS;Username=postgres;Password=gK215114760";
    options.UseNpgsql(conection);
});

builder.Services.AddScoped<FileStorageService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  // מאפשר לכל מקור לגשת
               .AllowAnyMethod()  // מאפשר כל שיטה (GET, POST וכו')
               .AllowAnyHeader(); // מאפשר כל כותרת
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});


Console.WriteLine($"AWS_REGION: {Environment.GetEnvironmentVariable("AWS_REGION")}");
Console.WriteLine($"AWS_ACCESS_KEY_ID: {Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")}");
Console.WriteLine($"AWS_SECRET_ACCESS_KEY: {Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")}");
Console.WriteLine($"AWS_BUCKET_NAME: {Environment.GetEnvironmentVariable("AWS_BUCKET_NAME")}");


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

//mapper
builder.Services.AddAutoMapper(typeof(ProfileMapping),typeof(PostModelProfileMapping));

//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

//Provisioning role permissions
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("Editor", "Admin"));
    options.AddPolicy("ViewerOnly", policy => policy.RequireRole("Viewer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
