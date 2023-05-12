using ApiBibliotecas.Helpers;
using ApiBibliotecas.Models;
using ApiBibliotecas.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuguetProyectoBibliotecas.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ApiBibliotecas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private BibliotecasRepository repo;
        private HelperOAuthToken helper;

        public AuthController(BibliotecasRepository repo,
            HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //NECESITAMOS UN METODO PARA VALIDAR A NUESTRO USUARIO
        //Y DEVOLVER EL TOKEN DE ACCESO
        //DICHO METODO SIEMPRE DEBE SER POST
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(UsuarioLogin model)
        {
            Usuario usuario = await this.repo.Login(model.DniUsuario, model.Contraseña);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                //EL TOKEN SE GENERA CON UNA CLASE Y DEBEMOS INDICAR
                //LOS DATOS QUE CONFORMAN DICHO TOKEN
                JwtSecurityToken token = new JwtSecurityToken
                        (
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(new
                {
                    response =
                    new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
    }

}
