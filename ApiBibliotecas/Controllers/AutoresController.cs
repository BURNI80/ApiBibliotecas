using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private BibliotecasRepository repo;

        public AutoresController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> GetAutores()
        {
            return await this.repo.GetAutores();
        }


        [HttpGet]
        [Route("[action]/{input}")]
        public async Task<ActionResult<List<Autor>>> SearchAutorNombre(string input)
        {
            return await this.repo.SearchAutorNombre(input);
        }


        [HttpGet]
        [Route("[action]/{idautor}")]
        public async Task<ActionResult<Autor>> GetDatosAutor(int idautor)
        {
            return await this.repo.GetDatosAutor(idautor);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task AddAutor(Autor autor)
        {
            await this.repo.AddAutor(autor);
        }


        [HttpPut]
        [Route("[action]")]
        public async Task UpdateAutor(Autor autor)
        {
            await this.repo.UpdateAutor(autor);
        }


        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task DeleteAutor(int id)
        {
            await this.repo.DeleteAutor(id);
        }













    }
}
