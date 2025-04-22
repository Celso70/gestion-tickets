using System.ComponentModel.DataAnnotations; //para los atributos de configuracion de modelos


namespace GestionTickets.Models.Tablas
{
    public class Categoria
    {
        [Key]
        public int CategoriaID { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        public bool Eliminado { get; set; }

        // Relación con Tickets
         public ICollection<Ticket>? Tickets { get; set; }
    }
}
