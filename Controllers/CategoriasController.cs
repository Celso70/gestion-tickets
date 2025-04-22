using GestionTickets.Data;
using GestionTickets.DTOs.Categorias;
using GestionTickets.Models.Tablas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GestionTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Método GET "Obtener Categorias"
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            //Buscamos las categorias existentes
            var categorias = await _context.Categorias
                //Que NO esten eliminadas
                .Where(c => !c.Eliminado)
                //Entonces mostramos un listado de DTOCategorias
                .Select(c => new CategoriaDTO
                {
                    CategoriaID = c.CategoriaID,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();

            return Ok(categorias);
        }

        //Método GET "Obtener UNA Categoria"
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            //Buscamos las categorias existentes
            var categoria = await _context.Categorias
            //Que coincidan con el ID que estamos utilizando y que NO esten eliminadas
                .Where(c => c.CategoriaID == id && !c.Eliminado)
                .FirstOrDefaultAsync();

            //Si no la encuentra
            if (categoria == null)
                return NotFound();

            //Si la encuentra, entonces mostramos el DTOCategorias
            return Ok(new CategoriaDTO
            {
                CategoriaID = categoria.CategoriaID,
                Descripcion = categoria.Descripcion
            });
        }

        //Método POST "Crear una Categoria"
        [HttpPost]
        public async Task<ActionResult> CrearCategoria(CategoriaCrearEditarDTO dto)
        {
            //Indicamos los campos de la Categoria "a crear"
            var categoria = new Categoria
            {
                Descripcion = dto.Descripcion,
                Eliminado = false
            };

            //La añadimos
            _context.Categorias.Add(categoria);
            //Y guardamos los cambios
            await _context.SaveChangesAsync();

            //RESPUESTA TRAS HABER CREADO LA CATEGORÍA(UN CÓDIGO DE ESTADO HTTP 201(Created)
            //Mostramos la Categoria creada gracias al endpoint anterior de "GetCategoria"
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.CategoriaID }, 
            //Mostramos la categoría "en cuestión"
            new CategoriaDTO
            {
                CategoriaID = categoria.CategoriaID,
                Descripcion = categoria.Descripcion
            });
        }

        //Método PUT "Editar Categoria"
        [HttpPut("{id}")]
        public async Task<ActionResult> EditarCategoria(int id, CategoriaCrearEditarDTO dto)
        {
            //Hallamos la Categoría que queremos editar, mediante su ID
            var categoria = await _context.Categorias.FindAsync(id);
            //Si la categoría no existe, o no se encuentra
            if (categoria == null || categoria.Eliminado)
                return NotFound();

            //Al encontrarla, editamos sus campos(descripcion)
            categoria.Descripcion = dto.Descripcion;
            //Y guardamos los cambios
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Método DELETE "Eliminar Categoria"
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCategoria(int id)
        {
            //Hallamos la Categoría que queremos eliminar, mediante su ID
            var categoria = await _context.Categorias.FindAsync(id);
            //Si la categoría no existe o ya se eliminó
            if (categoria == null || categoria.Eliminado)
                return NotFound();

            //Al encontrarla, la marcamos como "Eliminada"
            categoria.Eliminado = true;
            //Y guardamos los cambios
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}