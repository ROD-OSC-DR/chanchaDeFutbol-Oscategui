using CanchaFutbol.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CanchaRepository>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();

var api = app.MapGroup("/api");

api.MapGet("/horarios", (DateOnly? fecha, string? tipoCancha, bool? disponible, decimal? precioMaximo, CanchaRepository repository) =>
{
    var horarios = repository.Horarios.AsEnumerable();

    if (fecha.HasValue)
        horarios = horarios.Where(horario => horario.Fecha == fecha.Value);

    if (!string.IsNullOrWhiteSpace(tipoCancha))
        horarios = horarios.Where(horario => string.Equals(horario.TipoCancha, tipoCancha.Trim(), StringComparison.OrdinalIgnoreCase));

    if (disponible.HasValue)
        horarios = horarios.Where(horario => horario.Disponible == disponible.Value);

    if (precioMaximo.HasValue)
        horarios = horarios.Where(horario => horario.Precio <= precioMaximo.Value);

    return Results.Ok(horarios.ToArray());
});

api.MapGet("/horarios/{id:int}", (int id, CanchaRepository repository) =>
{
    var horario = repository.Horarios.FirstOrDefault(horario => horario.Id == id);
    return horario is null
        ? Results.NotFound(new { mensaje = "No se encontró el horario solicitado." })
        : Results.Ok(horario);
});

api.MapGet("/tipos-cancha", (CanchaRepository repository) =>
    Results.Ok(repository.Horarios
        .Select(horario => horario.TipoCancha)
        .Distinct()
        .OrderBy(tipo => tipo)));

api.MapGet("/promociones", (CanchaRepository repository) =>
    Results.Ok(repository.Promociones));

api.MapGet("/servicios", (CanchaRepository repository) =>
    Results.Ok(repository.Servicios));

app.MapFallbackToFile("index.html");

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
