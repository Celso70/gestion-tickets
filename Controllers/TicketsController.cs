// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using GestionTickets.Data;
// using GestionTickets.DTOs.Tickets;
// using GestionTickets.Models.Tablas;

// namespace GestionTickets.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class TicketsController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;

//         public TicketsController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // GET: api/Tickets
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
//         {
//             var tickets = await _context.Tickets
//                 .Include(t => t.Categoria)
//                 .Select(t => new TicketDTO
//                 {
//                     TicketID = t.TicketID,
//                     Titulo = t.Titulo,
//                     Descripcion = t.Descripcion,
//                     Estado = t.Estado,
//                     Prioridad = t.Prioridad,
//                     FechaCreacion = t.FechaCreacion,
//                     FechaCierre = t.FechaCierre,
//                     CategoriaID = t.CategoriaID,
//                     CategoriaDescripcion = t.Categoria.Descripcion,
//                     UsuarioClienteID = t.UsuarioClienteID
//                 }).ToListAsync();

//             return Ok(tickets);
//         }

//         // GET: api/Tickets/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<TicketDTO>> GetTicket(int id)
//         {
//             var ticket = await _context.Tickets
//                 .Include(t => t.Categoria)
//                 .FirstOrDefaultAsync(t => t.TicketID == id);

//             if (ticket == null)
//                 return NotFound();

//             var dto = new TicketDTO
//             {
//                 TicketID = ticket.TicketID,
//                 Titulo = ticket.Titulo,
//                 Descripcion = ticket.Descripcion,
//                 Estado = ticket.Estado,
//                 Prioridad = ticket.Prioridad,
//                 FechaCreacion = ticket.FechaCreacion,
//                 FechaCierre = ticket.FechaCierre,
//                 CategoriaID = ticket.CategoriaID,
//                 CategoriaDescripcion = ticket.Categoria?.Descripcion,
//                 UsuarioClienteID = ticket.UsuarioClienteID
//             };

//             return Ok(dto);
//         }

//         // POST: api/Tickets
//         [HttpPost]
//         public async Task<ActionResult<TicketDTO>> PostTicket(TicketCrearEditarDTO dto)
//         {
//             var ticket = new Ticket
//             {
//                 Titulo = dto.Titulo,
//                 Descripcion = dto.Descripcion,
//                 Estado = dto.Estado,
//                 Prioridad = dto.Prioridad,
//                 FechaCreacion = DateTime.UtcNow,
//                 FechaCierre = dto.FechaCierre,
//                 CategoriaID = dto.CategoriaID,
//                 UsuarioClienteID = dto.UsuarioClienteID
//             };

//             _context.Tickets.Add(ticket);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketID }, new TicketDTO
//             {
//                 TicketID = ticket.TicketID,
//                 Titulo = ticket.Titulo,
//                 Descripcion = ticket.Descripcion,
//                 Estado = ticket.Estado,
//                 Prioridad = ticket.Prioridad,
//                 FechaCreacion = ticket.FechaCreacion,
//                 FechaCierre = ticket.FechaCierre,
//                 CategoriaID = ticket.CategoriaID,
//                 UsuarioClienteID = ticket.UsuarioClienteID,
//                 CategoriaDescripcion = (await _context.Categorias.FindAsync(ticket.CategoriaID))?.Descripcion
//             });
//         }

//         // PUT: api/Tickets/5
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutTicket(int id, TicketCrearEditarDTO dto)
//         {
//             var ticket = await _context.Tickets.FindAsync(id);
//             if (ticket == null)
//                 return NotFound();

//             ticket.Titulo = dto.Titulo;
//             ticket.Descripcion = dto.Descripcion;
//             ticket.Estado = dto.Estado;
//             ticket.Prioridad = dto.Prioridad;
//             ticket.FechaCierre = dto.FechaCierre;
//             ticket.CategoriaID = dto.CategoriaID;
//             ticket.UsuarioClienteID = dto.UsuarioClienteID;

//             await _context.SaveChangesAsync();
//             return NoContent();
//         }

//         // DELETE: api/Tickets/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteTicket(int id)
//         {
//             var ticket = await _context.Tickets.FindAsync(id);
//             if (ticket == null)
//                 return NotFound();

//             _context.Tickets.Remove(ticket);
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }
//     }
// }
