using Reservas.Domain.Errores;
using System;
namespace Reservas.Domain
{
    public sealed record RangoHorario
    {
        public static readonly TimeSpan DuracionMinima = TimeSpan.FromMinutes(30);
        public static readonly TimeSpan DuracionMaxima = TimeSpan.FromHours(4);

        public DateTime Inicio { get; }
        public DateTime Fin { get; }

        private RangoHorario(DateTime inicio, DateTime fin)
        {
            Inicio = inicio;
            Fin = fin;
        }

        public TimeSpan Duracion => Fin - Inicio;

        public static RangoHorario Crear(DateTime inicio, DateTime fin)
        {
            var inicioUtc = ANormalizacionUtc(inicio);
            var finUtc = ANormalizacionUtc(fin);

            if (finUtc <= inicioUtc)
            {
                throw new RangoInvertidoException(inicioUtc, finUtc);
            }
            var duracion = finUtc - inicioUtc;

            if (duracion < DuracionMinima || duracion > DuracionMaxima)
            {
                throw new DuraccionFueraDeRangoException(duracion, DuracionMinima, DuracionMaxima);
            }

            return new RangoHorario(inicio, fin);
        }

        public bool SeTrasladaCon(RangoHorario otro)
        {
            ArgumentNullException.ThrowIfNull(otro);
            return Inicio < otro.Fin && otro.Inicio < Fin;
        }

        private static DateTime ANormalizacionUtc(DateTime valor) => valor.Kind switch
        { DateTimeKind.Utc => valor, DateTimeKind.Local => valor.ToUniversalTime(), _ => DateTime.SpecifyKind(valor, DateTimeKind.Utc) };
    }
}
