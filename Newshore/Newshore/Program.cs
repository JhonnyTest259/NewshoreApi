using Newshore.Repository;
using Newshore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IFlightService, FlightService>();

//Repositorio / repositiry
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

//http inject to call a external api
builder.Services.AddHttpClient<IFlightService, FlightService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["BaseUrlFlights"]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(["http://localhost:4200", "https://heroic-melba-a7c0af.netlify.app/"]));
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
