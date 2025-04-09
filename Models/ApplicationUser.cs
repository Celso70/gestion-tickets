using Microsoft.AspNetCore.Identity;


namespace GestionTickets.Models //utilizamos namespace por si en un futuro usamos una clase con el mismo nombre, para evitar conflictos
{

    
    public class ApplicationUser : IdentityUser // Creando clase usuario tomando propiedades del IdentityUser
    {
        // Agregamos 2 campos personalizados
        public string? NombreCompleto { get; set;}
        public DateTime FechaNacimiento { get; set;}
}
}
