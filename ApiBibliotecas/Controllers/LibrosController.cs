using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private BibliotecasRepository repo;
        private readonly ILogger<BibliotecasController> _logger;

        public LibrosController(BibliotecasRepository repo, ILogger<BibliotecasController> logger)
        {
            this.repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroDefault>>> GetLibrosDefault()
        {
            return await this.repo.GetLibrosTodosAsync();
        }


        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            return await this.repo.GetDatosLibroAsync(id);
        }


        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<LibroDefault>> GetLibroDef(int id)
        {
            return await this.repo.GetDatosLibroDefAsync(id);
        }

    }
}
