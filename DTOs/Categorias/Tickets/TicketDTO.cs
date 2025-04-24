using GestionTickets.Models.Tablas;


namespace GestionTickets.DTOs.Tickets

{
    public class TicketDTO
    {
        public int TicketID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public EstadoEnum Estado { get; set; }
        public PrioridadEnum Prioridad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }

        //Info relacionada
        public int CategoriaID { get; set; }
        public string? CategoriaDescripcion { get; set;}
    }
    
}