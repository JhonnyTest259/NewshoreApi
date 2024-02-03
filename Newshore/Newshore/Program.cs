using Newshore.Repository;
using Newshore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Repositorio / repositiry
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

//Inyeccion de Http para consumir apis externas
builder.Services.AddHttpClient<IFlightService, FlightService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["BaseUrlFlights"]);
});

builder.Services.AddControllers();
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
