using System.Text;
using ComprasVentas;
using ComprasVentas.Middleware;
using ComprasVentas.Services.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().CreateBootstrapLogger();
builder.Services.AddDbContext<ComprasVentas.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//Serilog
builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());
// Add services to the container.
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<PermisoRepository>();
builder.Services.AddScoped<IPermisoService, PermisoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotaService, NotaService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<EncryptionService>();
//validations
builder.Services.AddScoped<IUniqueNameChecker, UniqueNameChecker>();
// Seeders
builder.Services.AddScoped<PermisoSeeder>();
builder.Services.AddScoped<RolSeeder>();
builder.Services.AddScoped<UsuarioSeeder>();
builder.Services.AddScoped<CategoriaSeeder>();
builder.Services.AddScoped<ProductoSeeder>();
builder.Services.AddScoped<SucursalSeeder>();
builder.Services.AddScoped<AlmacenSeeder>();
builder.Services.AddScoped<ClienteProveedorSeeder>();
builder.Services.AddScoped<DataSeeder>();
//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});

builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("crear_categoria", policy => policy.Requirements.Add(new PermissionRequirement("crear_categoria")));
    options.AddPolicy("editar_categoria", policy => policy.Requirements.Add(new PermissionRequirement("editar_categoria")));
    options.AddPolicy("eliminar_categoria", policy => policy.Requirements.Add(new PermissionRequirement("eliminar_categoria")));
    options.AddPolicy("leer:categorias", policy => policy.Requirements.Add(new PermissionRequirement("leer:categorias")));
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
//Add global error handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

//Log with serilog
app.UseSerilogRequestLogging(option =>
{
    option.MessageTemplate = "HTTP {RequestMethod} {RequestPath} response {StatusCode} in {Elapsed:0:0000} ms";
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API Compras Ventas .NET");
    });
    //Configure Data Seeding
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await context.SeedAsync();
    }
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
// app.UseMiddleware<EncryptionMiddleware>();
app.MapControllers();

app.Run();
