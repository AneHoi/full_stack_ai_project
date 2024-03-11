using api;
using api.Middleware;
using infrastructure;
using infrastructure.repositories;
using service.accountservice;

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

builder.Services.AddSingleton<PasswordHashRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<AccountService>();

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

var repo = new MySQLRepo();
repo.Test();

app.Run();