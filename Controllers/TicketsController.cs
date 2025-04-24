using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTickets.Data;
using GestionTickets.DTOs.Tickets;
using GestionTickets.Models.Tablas;

namespace GestionTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Método GET "Obtener Tickets"
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            //Hallamos todos los tickets
            var tickets = await _context.Tickets
            //Segun ID de la categoría
                .Include(t => t.Categoria)
            //A modo de lista
                .ToListAsync();
            
            //Detalle de como luce el ticket
            return tickets.Select(t => new TicketDTO
                {
                    TicketID = t.TicketID,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    Estado = t.Estado,
                    Prioridad = t.Prioridad,
                    FechaCreacion = t.FechaCreacion,
                    FechaCierre = t.FechaCierre,
                    CategoriaID = t.CategoriaID,
                    CategoriaDescripcion = t.Categoria?.Descripcion,
                }).ToList();
        }

        //Método GET "Obtener UN Ticket"
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(int id)
        {
            //Hallamos todos los tickets
            var ticket = await _context.Tickets
            //Segun el ID
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(t => t.TicketID == id);

            //Si no lo encuentra
            if (ticket == null)
                return NotFound();

            //Si lo encuentra, demuestra el listado
            return new TicketDTO
            {
                TicketID = ticket.TicketID,
                Titulo = ticket.Titulo,
                Descripcion = ticket.Descripcion,
                Estado = ticket.Estado,
                Prioridad = ticket.Prioridad,
                FechaCreacion = ticket.FechaCreacion,
                FechaCierre = ticket.FechaCierre,
                CategoriaID = ticket.CategoriaID,
                CategoriaDescripcion = ticket.Categoria?.Descripcion,
            };
        }

        //Método POST "Crear Ticket"
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> PostTicket(TicketCrearEditarDTO dto)
        {
            //Indicamos los campos del Ticket "a crear"
            var ticket = new Ticket
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado,
                Prioridad = dto.Prioridad,
                CategoriaID = dto.CategoriaID,
                FechaCreacion = DateTime.UtcNow
            };

            //Agregamos el Ticket
            _context.Tickets.Add(ticket);
            //Y guardamos los cambios
            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaID);

            //RESPUESTA TRAS HABER CREADO EL TICKET(UN CÓDIGO DE ESTADO HTTP 201(Created)
            //Mostramos el Ticket creado gracias al endpoint anterior de "GetTicket"
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketID }, 

            //Mostramos el Ticket en cuestion:
            new TicketDTO
            {
                TicketID = ticket.TicketID,
                Titulo = ticket.Titulo,
                Descripcion = ticket.Descripcion,
                Estado = ticket.Estado,
                Prioridad = ticket.Prioridad,
                FechaCreacion = ticket.FechaCreacion,
                CategoriaID = ticket.CategoriaID,
                CategoriaDescripcion = categoria?.Descripcion
            });
        }

        //Método PUT "Editar Ticket"
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketCrearEditarDTO dto)
        {
            //Hallamos el ticket segun su ID
            var ticket = await _context.Tickets.FindAsync(id);

            //Si no en cuentra
            if (ticket == null)
                return NotFound();

            //Si encuentra
            ticket.Titulo = dto.Titulo;
            ticket.Descripcion = dto.Descripcion;
            ticket.Estado = dto.Estado;
            ticket.Prioridad = dto.Prioridad;
            ticket.CategoriaID = dto.CategoriaID;

            //Si el estado del Ticket es "cerrado" o "cancelado", se establece la fecha de cierre
            if (ticket.Estado == EstadoEnum.Cerrado || ticket.Estado == EstadoEnum.Cancelado)
                ticket.FechaCierre = DateTime.UtcNow;
            else
                ticket.FechaCierre = null;

            //Guardamos los cambios
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Método DELETE "Eliminar un Ticket"
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {   
            //Hallamos el ticket segun su ID
            var ticket = await _context.Tickets.FindAsync(id);
            //Si no lo encuentra
            if (ticket == null)
                return NotFound();

            //Si lo encuentra, lo eliminamos
            _context.Tickets.Remove(ticket);
            //Y guardamos los cambios
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
