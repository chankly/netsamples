using HSoft.CasbinProject.Api.Infrastructure.Filters;
using HSoft.CasbinProject.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSoft.CasbinProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly List<Product> _productos = new()
        {
            new Product { Id = 1, Nombre = "Leche Entera", Categoria = "leche", Precio = 1.20m },
            new Product { Id = 2, Nombre = "Leche Desnatada", Categoria = "leche", Precio = 1.30m },
            new Product { Id = 3, Nombre = "Pan Integral", Categoria = "pan", Precio = 0.80m }
        };

        [HttpGet("leche")]
        [CategoriaCasbin("listar", "leche")]
        public IActionResult ListarLeche()
        {
            var productosLeche = _productos.Where(p => p.Categoria == "leche").ToList();
            return Ok(productosLeche);
        }

        [HttpGet("leche/{id}")]
        [CategoriaCasbin("ver", "leche")]
        public IActionResult VerLeche(int id)
        {
            var producto = _productos.FirstOrDefault(p => p.Categoria == "leche" && p.Id == id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPut("leche/{id}")]
        [CategoriaCasbin("editar", "leche")]
        public IActionResult EditarLeche(int id, [FromBody] Product productoActualizado)
        {
            var producto = _productos.FirstOrDefault(p => p.Categoria == "leche" && p.Id == id);
            if (producto == null) return NotFound();

            producto.Nombre = productoActualizado.Nombre;
            producto.Precio = productoActualizado.Precio;

            return NoContent();
        }

        [HttpGet]
        [Authorize] // Solo usuarios autenticados
        public IActionResult ListarTodos()
        {
            // Este endpoint requiere verificación adicional en el código
            if (!User.IsInRole("admin"))
            {
                return Forbid();
            }
            return Ok(_productos);
        }
    }
}
