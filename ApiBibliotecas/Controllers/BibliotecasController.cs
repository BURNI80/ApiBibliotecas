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
        [Route("[action]")]
        public async Task<ActionResult<List<Biblioteca>>> GetBibliotecas()
        {
            return await this.repo.GetBibliotecasAsync();
        }


        [HttpGet]
        [Route("[action]/{id}")]
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


        [HttpPost]
        [Route("[action]")]
        public async Task AddBiblio(Biblioteca biblio)
        {
            await this.repo.AddBiblio(biblio);
        }


        [HttpPut]
        [Route("[action]")]
        public async Task UpdateBiblio(Biblioteca biblio)
        {
            await this.repo.UpdateBiblio(biblio);
        }


        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task DeleteBiblioteca(int id)
        {
            await this.repo.DeleteBiblioteca(id);
        }


        [HttpGet]
        [Route("[action]/{dniUsuario}")]
        public async Task<ActionResult<List<BibliotecaSimple>>> GetBibliotecasEditables(string dniUsuario)
        {
            return this.repo.GetBibliotecasEditables(dniUsuario);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<BibliotecaSimple>>> GetBibliotecasSimples()
        {
            return await this.repo.GetBibliotecasSimples();
        }



    }
}
