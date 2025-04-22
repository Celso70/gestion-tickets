using System.ComponentModel.DataAnnotations;//para los atributos de configuracion de modelos
using System.ComponentModel.DataAnnotations.Schema;//especificar nombre exacto de tablas/claves foraneas/not mappeds,etc
using GestionTickets.Models.Usuario;

namespace GestionTickets.Models.Tablas
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Titulo { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public EstadoEnum Estado { get; set; }

        [Required]
        public PrioridadEnum Prioridad { get; set; }

        public DateTime FechaCreacion { get; set; }
        
        public DateTime? FechaCierre { get; set; }

//RELACIONES


    //Categoria 
        //traemos clave foranea categoria
        [ForeignKey("Categoria")]
        public int CategoriaID { get; set; }

        //traemos el objeto completo de categoria
        public Categoria? Categoria { get; set; }


    //Usuario Cliente
        //traemos clave foranea usuario
        [ForeignKey("UsuarioCliente")]
        public string? UsuarioClienteID { get; set; }
        //traemos el objeto completo de usuario
        public ApplicationUser? UsuarioCliente { get; set; }

    //Comentarios
     public List<ComentarioTicket>? Comentarios { get; set; }
    }

//RELACIONES

//CAMPOS ENUMERADOS
        public enum EstadoEnum
    {
        Abierto,
        EnProceso,
        Cerrado,
        Cancelado
    }

    public enum PrioridadEnum
    {
        Baja,
        Media,
        Alta
    }
//CAMPOS ENUMERADOS
}

