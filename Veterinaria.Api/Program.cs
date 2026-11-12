using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos.Models;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Negocio;
using Veterinaria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------- Inyección de dependencias ----------
// DbContext (la cadena de conexión está en appsettings.Development.json, que no se sube al repositorio)
builder.Services.AddDbContext<VeterinariaContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("VeterinariaDB")));

// Repositorios (capa Datos)
builder.Services.AddScoped<IDuenoRepositorio, DuenoRepositorio>();
builder.Services.AddScoped<IMascotaRepositorio, MascotaRepositorio>();

// Servicios (capa Negocio)
builder.Services.AddScoped<IDuenoServicio, DuenoServicio>();
builder.Services.AddScoped<IMascotaServicio, MascotaServicio>();

// AutoMapper con el MappingProfile de la capa Negocio
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                       // documento OpenAPI en /openapi/v1.json
    app.UseSwaggerUI(opciones =>            // interfaz Swagger en /swagger
    {
        opciones.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
