using INTERNAL_SOURCE_LOAD;
using INTERNAL_SOURCE_LOAD.Models;
using INTERNAL_SOURCE_LOAD.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); 
builder.Services.AddTransient(typeof(IJsonToModelTransformer<>), typeof(JsonToModelTransformer<>));

builder.Services.AddSingleton<IDatabaseExecutor, MariaDbExecutor>(sp =>
    new MariaDbExecutor(builder.Configuration.GetConnectionString("DefaultConnection")));


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