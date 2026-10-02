using Reservas.Domain.Errores;

namespace Reservas.Domain
{
    public sealed class Reserva
    {
        public Guid Id { get; private set; }
        public Guid SalaId { get; private set; }
        public RangoHorario Rango { get; private set; }
        private Reserva(Guid id, Guid salaId, RangoHorario rango)
        {
            Id = id;
            SalaId = salaId;
            Rango = rango;
        }
        public static Reserva Crear(Guid salaId, DateTime inicio, DateTime fin)
        {
            if (salaId == Guid.Empty)
            {
                throw new SalaRequeridaException();
            }

            DateTime ahora = DateTime.Now;

            if (inicio < ahora)
            {
                throw new ReservaEnElPasadoException(inicio, ahora);
            }

            RangoHorario rangoHoraio = RangoHorario.Crear(inicio, fin);

            return new Reserva(Guid.CreateVersion7(), salaId, rangoHoraio);
        }
        public bool SeTranlapaCon(Reserva otra)
        {
            ArgumentNullException.ThrowIfNull(otra);
            if (Id == otra.Id)
            {
                return false;
            }
            return SalaId == otra.SalaId && Rango.SeTrasladaCon(otra.Rango);
        }
    }
}
1:09:57