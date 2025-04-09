using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestionTickets.Models; //Desde donde traigo la clase del Usuario("ApplicationUser")


namespace GestionTickets.Data //en caso de precaución
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //Clase que toma propiedades de identity para manejar a mis usuarios("ApplicationUser"),es decir toda la lógica.
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) //constructor del contexto(donde se inyecta la configuración)
        : base(options)
        { 
        }
            //DbSets 
            //Acá van todas las clases, modelos, tablas, como se diga, ejemplo:
             public DbSet<Categoria> Categorias { get; set; }
             public DbSet<Ticket> Tickets { get; set; }
             public DbSet<ComentarioTicket> ComentarioTickets { get; set; }

    }
}