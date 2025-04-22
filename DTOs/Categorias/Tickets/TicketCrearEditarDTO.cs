using GestionTickets.Models.Tablas;


namespace GestionTickets.DTOs.Tickets
{
    public class TicketCrearEditarDTO
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaID { get; set; }
        public EstadoEnum Estado { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public DateTime? FechaCierre { get; set; }
    }
}