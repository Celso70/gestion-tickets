using System.ComponentModel.DataAnnotations;
using GestionTickets.Models.Tablas;


namespace GestionTickets.DTOs.Tickets
{
    public class TicketCrearEditarDTO
    {   
        [Required]
        public string Titulo { get; set; } = string.Empty;
        
        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]        
        public EstadoEnum Estado { get; set; }

        [Required]
        public PrioridadEnum Prioridad { get; set; }

        [Required]
        public int CategoriaID { get; set; }
    }
}