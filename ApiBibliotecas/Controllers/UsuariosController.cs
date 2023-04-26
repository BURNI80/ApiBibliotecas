using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Route("[action]/{dniusuario}")]
        public async Task<ActionResult<int>> NumComentariosUsuario(string dniusuario)
        {
            return await this.repo.NumComentariosUsuario(dniusuario);

        }


        [HttpGet]
        [Route("[action]/{dniusuario}")]
        public async Task<ActionResult<int>> NumReseñasUsuario(string dniusuario)
        {
            return await this.repo.NumReseñasUsuario(dniusuario);

        }




    }
}
