using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NodesOfTrees.Abstractions.Interfaces;
using NodesOfTrees.Data;
using NodesOfTrees.Middlawares;
using NodesOfTrees.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddSwaggerGen();
var app = builder.Build();
Database.Migrate(app);

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
