namespace CanchaFutbol.Models;

public sealed record Horario(
    int Id,
    string Codigo,
    DateOnly Fecha,
    string Hora,
    string TipoCancha,
    decimal Precio,
    bool Disponible,
    string Duracion,
    string Estado,
    string Descripcion,
    string ImagenUrl);
