using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuguetProyectoBibliotecas.Models;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private BibliotecasRepository repo;

        public ReservasController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet]
        [Route("[action]/{idbiblioteca}")]
        public async Task<ActionResult<List<ReservaNLibro>>> GetReservasBiblio(int idbiblioteca)
        {
            return await this.repo.GetReservasBiblio(idbiblioteca);
        }


        [HttpGet]
        [Route("[action]/{idlibro}/{idbiblio}")]
        public async Task<ActionResult<List<Reserva>>> GetResrevasLibro(int idlibro, int idbiblio)
        {
            return await this.repo.GetResrevasLibro(idlibro,idbiblio);
        }


        [HttpGet]
        [Route("[action]/{idlibro}")]
        public async Task<ActionResult<List<BibliotecaSimple>>> GetLibroDisponible(int idlibro)
        {
            return await this.repo.GetLibroDisponible(idlibro);
        }


        [HttpPost]
        [Route("[action]/{reserva}")]
        public async Task CreateReserva(Reserva reserva)
        {
            await this.repo.CreateReserva(reserva);
        }


        [HttpDelete]
        [Route("[action]/{idreserva}")]
        public async Task DeleteReserva(int idreserva)
        {
            await this.repo.DeleteReserva(idreserva);
        }


        [HttpGet]
        [Route("[action]/{idprestamo}/{idbiblio}")]
        public async Task RecogerLibro(int idprestamo, int idbiblio)
        {
            await this.repo.RecogerLibro(idprestamo, idbiblio);
        }


        [HttpGet]
        [Route("[action]/{idprestamo}/{idbiblio}")]
        public async Task DevolverLibro(int idprestamo, int idbiblio)
        {
            await this.repo.DevolverLibro(idprestamo, idbiblio);
        }




    }
}
