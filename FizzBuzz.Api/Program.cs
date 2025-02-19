using FizzBuzz.Api.Middleware;
using FizzBuzz.Appilcation.Implementation;
using FizzBuzz.Appilcation.Interface;
using FizzBuzz.Domain.Implementation;
using FizzBuzz.Domain.Interface;
using FizzBuzz.Infrastructure.Implementation;
using FizzBuzz.Infrastructure.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add Cors
var corsPolicy = "_myCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFizzBuzzApplication, FizzBuzzApplication>();
builder.Services.AddScoped<IFizzBuzzService, FizzBuzzService>();
builder.Services.AddSingleton<IFileWriter, FileWriter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
