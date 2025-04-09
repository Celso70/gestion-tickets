using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionTickets.Models
{
    public class ComentarioTicket
    {
        [Key]
        public int ComentarioID { get; set; }

        [Required]
        public string? Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }
    
//RELACIONES


    //Ticket
        //traemos clave foranea ticket
        [ForeignKey("Ticket")]
        public int TicketID { get; set; }

        //traemos el objeto completo de ticket
        public Ticket? Ticket{ get; set; }


    //Usuario
        //traemos clave foranea ticket
        [ForeignKey("Usuario")]
        public string? UsuarioID { get; set; }

        //traemos el objeto completo de usuario
        public ApplicationUser? Usuario { get; set; }
        }


//RELACIONES

}
