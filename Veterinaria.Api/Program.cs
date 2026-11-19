using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Veterinaria.Datos.Models;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Negocio;
using Veterinaria.Negocio.Seguridad;
using Veterinaria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI con el esquema Bearer para que Swagger muestre el botón "Authorize"
builder.Services.AddOpenApi(opciones =>
{
    opciones.AddDocumentTransformer((documento, contexto, cancelacion) =>
    {
        documento.Components ??= new OpenApiComponents();
        documento.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        documento.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Pega solo el token (sin la palabra Bearer)."
        };

        documento.Security ??= new List<OpenApiSecurityRequirement>();
        documento.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", documento)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});

// ---------- Configuración JWT (sección "Jwt") ----------
var seccionJwt = builder.Configuration.GetSection("Jwt");
var jwt = seccionJwt.Get<JwtOpciones>()
          ?? throw new InvalidOperationException("Falta la sección Jwt en la configuración.");
if (string.IsNullOrWhiteSpace(jwt.Clave) || jwt.Clave.Length < 32)
    throw new InvalidOperationException("Jwt:Clave debe tener al menos 32 caracteres.");

builder.Services.Configure<JwtOpciones>(seccionJwt);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.MapInboundClaims = false;   // conserva los nombres sub, email y rol
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,               // emisor
            ValidIssuer = jwt.Emisor,
            ValidateAudience = true,             // audiencia
            ValidAudience = jwt.Audiencia,
            ValidateLifetime = true,             // vencimiento
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,     // firma
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            NameClaimType = "email",
            RoleClaimType = "rol"                // [Authorize(Roles = "...")] lee este claim
        };
    });

builder.Services.AddAuthorization();

// ---------- Inyección de dependencias ----------
builder.Services.AddDbContext<VeterinariaContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("VeterinariaDB")));

builder.Services.AddScoped<IDuenoRepositorio, DuenoRepositorio>();
builder.Services.AddScoped<IMascotaRepositorio, MascotaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<IDuenoServicio, DuenoServicio>();
builder.Services.AddScoped<IMascotaServicio, MascotaServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1");
        opciones.EnablePersistAuthorization();   // mantiene el token al recargar Swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();   // primero: ¿quién eres? (lee y valida el token)
app.UseAuthorization();    // después: ¿qué puedes hacer? (revisa [Authorize] y roles)

app.MapControllers();

app.Run();
