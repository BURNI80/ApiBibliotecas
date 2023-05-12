using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private BibliotecasRepository repo;

        public UsuariosController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string dniUsu)
        {
            return await this.repo.GetUsuario(dniUsu);
        }


        [HttpPut]
        [Authorize]
        [Route("[action]")]
        public async Task UpdateUsuario(Usuario usu)
        {
            await this.repo.UpdateUsuario(usu);
        }


        [HttpGet]
        [Authorize]
        [Route("[action]/{dniusuario}")]
        public async Task<ActionResult<int>> NumComentariosUsuario(string dniusuario)
        {
            return await this.repo.NumComentariosUsuario(dniusuario);

        }


        [HttpGet]
        [Authorize]
        [Route("[action]/{dniusuario}")]
        public async Task<ActionResult<int>> NumReseñasUsuario(string dniusuario)
        {
            return await this.repo.NumReseñasUsuario(dniusuario);

        }


        [HttpPost]
        [Authorize]
        [Route("[action]/{dniUsu}/{idLibro}/{orden}")]
        public async Task AddListaLibro(string dniUsu, int idLibro, int orden)
        {
            await this.repo.AddListaLibro(dniUsu, idLibro, orden);
        }


        [HttpPost]
        [Authorize]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<int>> NLibrosLeidos(string dniUsu)
        {
            return await this.repo.NLibrosLeidos(dniUsu);
        }


        [HttpGet]
        [Authorize]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<List<ReservaUsuario>>> GetReservasUsuario(string dniUsu)
        {
            return await this.repo.GetReservasUsuario(dniUsu);
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<List<ComentarioBasico>>> GetComentariosUsuario(string dniUsu)
        {
            return await this.repo.GetComentariosUsuario(dniUsu);
        }


        [HttpGet]
        [Authorize]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<List<LibroDeseo>>> GetFavoritos(string dniUsu)
        {
            return await this.repo.GetFavoritos(dniUsu);
        }

        [HttpPost]
        [Route("[action]/{dniUsu}/{token}")]
        public async Task<ActionResult<Compartido>> SetGetToken(string dniUsu, string token)
        {
            return await this.repo.SetGetToken(dniUsu, token);
        }


        [HttpGet]
        [Route("[action]/{dniUsu}")]
        public async Task<ActionResult<Compartido>> GetShare(string dniUsu)
        {
            return await this.repo.GetShare(dniUsu);
        }


        [HttpGet]
        [Authorize]
        [Route("[action]/{idLibro}/{dniUsu}")]
        public async Task<ActionResult<int>> LibroDeseo(int idLibro, string dniUsu)
        {
            return this.repo.LibroDeseo(idLibro,dniUsu);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task Register(Usuario user)
        {
            await this.repo.Register(user);
        }

    }
}
