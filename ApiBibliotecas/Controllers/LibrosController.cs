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


        [HttpGet]
        [Route("[action]/{idbiblioteca}")]
        public async Task<ActionResult<List<LibroDisponibilidad>>> GetLibrosDispoBiblioteca(int idbiblioteca)
        {
            return await this.repo.GetLibrosBibliotecaAsync(idbiblioteca);
        }


        [HttpGet]
        [Route("[action]/{idbiblioteca}/{input}/{opcion}")]
        public async Task<ActionResult<List<LibroDisponibilidad>>> GetLibrosDispoBiblioteca(int idbiblioteca, string input, char opcion)
        {
            return await this.repo.SearchLibroBibliotecaAsync(idbiblioteca,input,opcion);
        }


        [HttpGet]
        [Route("[action]/{idautor}")]
        public async Task<ActionResult<List<LibroDefault>>> GetLibrosAutor(int idautor)
        {
            return await this.repo.GetLibrosAutor(idautor);
        }


        [HttpGet]
        [Route("[action]/{idautor}/{input}")]
        public async Task<ActionResult<List<LibroDefault>>> SearchLibroAutorNombre(int idautor, string input)
        {
            return await this.repo.SearchLibroAutorNombre(idautor,input);
        }




    }
}
