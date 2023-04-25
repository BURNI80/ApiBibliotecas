using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosValoracionesController : ControllerBase
    {
        private BibliotecasRepository repo;

        public ComentariosValoracionesController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        [Route("[action]/{idlibro}")]
        public ActionResult<int> GetValoracionesCount(int idlibro)
        {
            return this.repo.GetValoraciones(idlibro);
        }


        [HttpGet]
        [Route("[action]/{dniusuario}/{idlibro}")]
        public async Task<ActionResult<List<Comentario>>> GetComentariosLibroLikeUsuario(string dniusuario,int idlibro)
        {
            return await this.repo.GetComentariosLikeAsync(idlibro,dniusuario);
        }


        [HttpGet]
        [Route("[action]/{dniusuario}/{idcomentario}/{orden}")]
        public IActionResult LikeComentario(string dniusuario, int idcomentario, int orden)
        {
            this.repo.LikeComentario(orden, idcomentario, dniusuario);

            return Ok();
        }



    }
}
