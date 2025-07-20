using apiReservas.Identity;
using apiReservas.Data;
using apiReservas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region 1. Configuración fuerte‑tipada de JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
#endregion

#region 2. DbContext (SQL Server)
builder.Services.AddDbContext<ReservasContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("cnReservas")));
#endregion

#region 3. Identity (solo API, sin cookies)
builder.Services
    .AddIdentityCore<ApplicationUser>(opt =>
    {
        opt.SignIn.RequireConfirmedAccount = false;
        opt.Password.RequireNonAlphanumeric = false;          // ajusta a tu política
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ReservasContext>()
    .AddDefaultTokenProviders();
#endregion

#region 4. Autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero          // sin margen extra
    };
    options.SaveToken = true;                             // expone token en HttpContext
});
#endregion

builder.Services.AddAuthorization();

#region 5. Servicios de dominio
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IIngresoService, IngresoService>();
builder.Services.AddScoped<IEgresoService, EgresoService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
#endregion

#region 6. Controladores + System.Text.Json
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
#endregion

#region 7. Swagger con JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiReservas", Version = "v1" });

    // Definición del esquema Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe:  Bearer {token}"
    });
    // Requisito global
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Comentarios XML (opcional, activa <GenerateDocumentationFile> en .csproj)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});
#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

#region 8. Migraciones y seeding (idempotentes)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReservasContext>();
    await IdentityDataSeeder.SeedAsync(scope.ServiceProvider);

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Aplicando migraciones…");
        if (context.Database.GetPendingMigrations().Any())
            await context.Database.MigrateAsync();
        logger.LogInformation("Base de datos lista.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al migrar o sembrar la BD.");
        throw; // detiene el arranque si es crítico
    }
}
#endregion

#region 9. Middleware HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();    //  ← antes de Authorization
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();

/// <summary>
/// Modelo fuerte‑tipado para la sección "Jwt" de appsettings.json.
/// </summary>
public record JwtSettings(string Key, string Issuer, string Audience);
