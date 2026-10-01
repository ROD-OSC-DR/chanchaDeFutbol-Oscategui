using CanchaFutbol.Models;

namespace CanchaFutbol.Data;

/// <summary>
/// Fuente de consulta de la cancha. La API expone únicamente recursos de lectura.
/// </summary>
public sealed class CanchaRepository
{
    public IReadOnlyList<Horario> Horarios { get; } = CrearHorarios();

    public IReadOnlyList<Promocion> Promociones { get; } =
    [
        new("Noche de campeones", "Reserva una cancha de fútbol 5 desde las 8:00 p. m. y recibe una bebida isotónica por jugador.", "GOLNOCHE", "Válido de lunes a jueves, sujeto a disponibilidad."),
        new("Plan de equipo", "Separa tres horarios de entrenamiento y obtén 10% de descuento en el tercero.", "EQUIPO10", "Válido para reservas anticipadas de la semana.")
    ];

    public IReadOnlyList<Servicio> Servicios { get; } =
    [
        new("Iluminación LED", "Juega de noche con iluminación uniforme para seguir cada jugada.", "bi-lightbulb"),
        new("Vestuarios", "Espacios cómodos para que tu equipo se prepare antes y después del partido.", "bi-person-arms-up"),
        new("Estacionamiento", "Zona de estacionamiento para que llegues con tranquilidad.", "bi-p-square")
    ];

    private static IReadOnlyList<Horario> CrearHorarios()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var horas = new[] { "09:00", "11:00", "16:00", "18:00", "20:00" };
        var horarios = new List<Horario>();

        for (var dia = 0; dia < 7; dia++)
        {
            for (var turno = 0; turno < horas.Length; turno++)
            {
                var id = dia * horas.Length + turno + 1;
                var esFutbolCinco = (dia + turno) % 2 == 0;
                var tipoCancha = esFutbolCinco ? "Fútbol 5" : "Fútbol 7";
                var disponible = (dia + turno) % 4 != 2;
                var imagen = esFutbolCinco
                    ? "https://images.unsplash.com/photo-1575361204480-aadea25e6e68?auto=format&fit=crop&w=900&q=80"
                    : "https://images.unsplash.com/photo-1553778263-73a83bab9b0c?auto=format&fit=crop&w=900&q=80";

                horarios.Add(new Horario(
                    id,
                    $"HOR-{id:000}",
                    hoy.AddDays(dia),
                    horas[turno],
                    tipoCancha,
                    esFutbolCinco ? (turno >= 3 ? 100.00m : 80.00m) : (turno >= 3 ? 150.00m : 120.00m),
                    disponible,
                    esFutbolCinco ? "60 min" : "90 min",
                    disponible ? "Disponible" : "Reservado",
                    esFutbolCinco
                        ? "Cancha sintética techada, ideal para partidos dinamicos entre amigos."
                        : "Espacio amplio de césped sintético para equipos y entrenamientos.",
                    imagen));
            }
        }

        return horarios;
    }
}
