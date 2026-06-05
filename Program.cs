using ComprasVentas;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().CreateBootstrapLogger();
builder.Services.AddDbContext<ComprasVentas.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
));
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

//validations
builder.Services.AddScoped<IUniqueNameChecker, UniqueNameChecker>();


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
}
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
