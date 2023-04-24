using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecasController : ControllerBase
    {
        private BibliotecasRepository repo;
        private readonly ILogger<BibliotecasController> _logger;

        public BibliotecasController(BibliotecasRepository repo, ILogger<BibliotecasController> logger)
        {
            this.repo = repo;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<List<Biblioteca>>> GetBibliotecas()
        {
            return await this.repo.GetBibliotecasAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Biblioteca>> FindBiblioteca(int id)
        {
            return await this.repo.FindBibliotecaAsync(id);
        }


        [HttpGet]
        [Route("[action]/{input}")]
        public async Task<ActionResult<List<Biblioteca>>> SearchBibliotecasNombre(string input)
        {
            return await this.repo.SearchBibliotecaAsync(input);
        }




    }
}
