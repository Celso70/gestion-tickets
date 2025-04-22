using System.ComponentModel.DataAnnotations;

namespace GestionTickets.DTOs.Categorias
{
    public class CategoriaCrearEditarDTO
    {
        [Required]
        public string Descripcion { get; set; }
    }
}