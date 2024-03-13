using System.Data;
using api;
using api.Middleware;
using ConsoleApp1.JsonFileExtractor;
using infrastructure;
using infrastructure.mySqlRepositories;
using infrastructure.repositories;
using MySql.Data.MySqlClient;
using service.accountservice;
using service.allergenService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
    
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}


// Register the connection string as a singleton service so it can be used in repoes.
builder.Services.AddSingleton(provider => Utilities.MySqlConnectionString);

builder.Services.AddSingleton(provider => new MySQLRepo(provider.GetRequiredService<string>()));
builder.Services.AddSingleton(provider => new FoodJsonExtractorRepository(provider.GetRequiredService<string>()));
builder.Services.AddSingleton(provider => new ProductRepo(provider.GetRequiredService<string>()));
builder.Services.AddSingleton(provider => new UserRepo(provider.GetRequiredService<string>()));
builder.Services.AddSingleton(provider => new PasswordHashRepo(provider.GetRequiredService<string>()));
builder.Services.AddSingleton(provider => new AllergenRepo(provider.GetRequiredService<string>()));

builder.Services.AddSingleton<PasswordHashRepository>();
builder.Services.AddSingleton<UserRepository>();

builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<AllergenDbCreatorService>();

builder.Services.AddJwtService();
builder.Services.AddSwaggerGenWithBearerJWT();

var allowedOrigins = new[] { "http://localhost:4200", "https://localhost:4200" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

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

app.UseCors("AllowSpecificOrigins");
app.UseMiddleware<JwtBearerHandler>();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();