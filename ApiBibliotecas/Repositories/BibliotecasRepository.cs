using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuguetProyectoBibliotecas.Models;
using NuguetProyectoBibliotecas.Helpers;
using ApiBibliotecas.Data;
using System.Data;
using System.Data.Common;
using System.Net;
//using Newtonsoft.Json.Linq;

namespace ApiBibliotecas.Repositorys
{
    public class BibliotecasRepository
    {
        private BibliotecasContext context;

        public BibliotecasRepository(BibliotecasContext context)
        {
            this.context = context;
        }


       
        //AUTH
        public async Task<Usuario> Login(string dni, string pass)
        {
            Usuario usu = this.context.Usuarios.FirstOrDefault(z => z.DNI_USUARIO == dni);
            if (usu == null)
            {
                return null;
            }
            else
            {
                byte[] passBBDD = usu.PASSWORD;
                byte[] passInput = HelperCryptography.EncryptPassword(pass, usu.SALT);
                bool res = HelperCryptography.ComapreArrays(passBBDD, passInput);
                if (res == true)
                {
                    return usu;
                }
                else
                {
                    return null;
                }
            }
        }



        //BIBLIOTECAS
        public async Task<List<Biblioteca>> GetBibliotecasAsync()
        {
            return await this.context.Bibliotecas.ToListAsync();
        }

        public async Task<List<Biblioteca>> SearchBibliotecaAsync(string input)
        {
            string sql = "SP_BUSCARBIBLIOTECAS @INPUT";
            SqlParameter p = new SqlParameter("@INPUT", input);
            var consulta = this.context.Bibliotecas.FromSqlRaw(sql, p);
            return await consulta.ToListAsync();
        }

        public async  Task<Biblioteca> FindBibliotecaAsync(int id)
        {
            var consulta = from data in this.context.Bibliotecas
                           where data.ID_BIBLIOTECA == id
                           select data;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task AddBiblio(Biblioteca biblio)
        {
            biblio.ID_BIBLIOTECA = this.context.Bibliotecas.Any() ? this.context.Bibliotecas.Max(x => x.ID_BIBLIOTECA) + 1 : 1;
            this.context.Bibliotecas.Add(biblio);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateBiblio(Biblioteca biblio)
        {
            Biblioteca b = await FindBibliotecaAsync(biblio.ID_BIBLIOTECA);
            b.NOMBRE = biblio.NOMBRE;
            b.DIRECCION = biblio.DIRECCION;
            b.TELEFONO = biblio.TELEFONO;
            b.WEB = biblio.WEB;
            b.IMAGEN = biblio.IMAGEN;
            b.HORA_APERTURA = biblio.HORA_APERTURA;
            b.HORA_CIERRE = biblio.HORA_CIERRE;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteBiblioteca(int id)
        {
            Biblioteca b = this.context.Bibliotecas.Where(x => x.ID_BIBLIOTECA == id).FirstOrDefault();
            this.context.Bibliotecas.Remove(b);
            await this.context.SaveChangesAsync();
        }

        public List<BibliotecaSimple> GetBibliotecasEditables(string id)
        {
            var consulta = from editorBiblioteca in context.EditorBiblioteca
                           join biblioteca in context.Bibliotecas
                           on editorBiblioteca.ID_BIBLIOTECA equals biblioteca.ID_BIBLIOTECA
                           where editorBiblioteca.DNI_USUARIO == id
                           select new
                           {
                               biblioteca.ID_BIBLIOTECA,
                               biblioteca.NOMBRE,
                           };
            List<BibliotecaSimple> biblios = consulta.ToList().ConvertAll(x => new BibliotecaSimple
            {
                ID_BIBLIOTECA = x.ID_BIBLIOTECA,
                NOMBRE = x.NOMBRE,
            });
            return biblios;
        }

        public async Task<List<BibliotecaSimple>> GetBibliotecasSimples()
        {
            List<Biblioteca> biblios = await this.context.Bibliotecas.ToListAsync();
            List<BibliotecaSimple> bibliotecasSimples = biblios.Select(b =>
                    new BibliotecaSimple
                    {
                        ID_BIBLIOTECA = b.ID_BIBLIOTECA,
                        NOMBRE = b.NOMBRE
                    }).ToList();
            return bibliotecasSimples;
        }



        //LIBROS
        public async Task<List<LibroDefault>> GetLibrosTodosAsync()
        {
            return await this.context.LibrosDef.ToListAsync();
        }

        public async Task<Libro> GetDatosLibroAsync(int id)
        {
            string sql = "SP_DETALLESLIBRO @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            var consulta = this.context.Libros.FromSqlRaw(sql, p1);
            return consulta.AsEnumerable().FirstOrDefault();
        }

        public async Task<LibroDefault> GetDatosLibroDefAsync(int id)
        {
            return await this.context.LibrosDef.Where(x => x.ID_LIBRO == id).FirstOrDefaultAsync();
        }

        public async Task<List<LibroDisponibilidad>> GetLibrosBibliotecaAsync(int id)
        {
            string sql = "SP_BUSCARLIBRO @ID_BIBLIOTECA";
            SqlParameter p = new SqlParameter("@ID_BIBLIOTECA", id);
            var consulta = this.context.LibrosDisponibilidad.FromSqlRaw(sql, p);
            return await consulta.ToListAsync();
        }

        public async Task<List<LibroDisponibilidad>> SearchLibroBibliotecaAsync(int id, string input, char option)
        {
            string sql = "";
            if (input == null ||  input == "null")
            {
                sql = "SP_LIBROSDISPO";
                var consulta = this.context.LibrosDisponibilidad.FromSqlRaw(sql);
                return await consulta.ToListAsync();

            }
            if (option == 'T')
            {
                sql = "SP_BUSCARLIBRONOMBRE @INPUT, @ID_BIBLIOTECA";
                SqlParameter p1 = new SqlParameter("@INPUT", input);
                SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", id);
                var consulta = this.context.LibrosDisponibilidad.FromSqlRaw(sql, p1, p2);
                return await consulta.ToListAsync();

            }
            else if (option == 'A')
            {
                sql = "SP_BUSCARLIBROAUTOR @INPUT, @ID_BIBLIOTECA";
                SqlParameter p1 = new SqlParameter("@INPUT", input);
                SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", id);
                var consulta = this.context.LibrosDisponibilidad.FromSqlRaw(sql, p1, p2);
                return await consulta.ToListAsync();

            }
            return null;

        }

        public async Task<List<LibroDefault>> GetLibrosAutor(int id)
        {
            return await this.context.LibrosDef.Where(x => x.ID_AUTOR == id).ToListAsync();
        }

        public async Task<List<LibroDefault>> SearchLibroAutorNombre(int id, string input)
        {
            string sql = "SP_SEARCHLIBRODEAUTOR @ID_AUTOR, @INPUT";
            SqlParameter p1 = new SqlParameter("@ID_AUTOR", id);
            SqlParameter p2 = new SqlParameter("@INPUT", input);
            var consulta = this.context.LibrosDef.FromSqlRaw(sql, p1, p2);
            return await consulta.ToListAsync();
        }

        public async Task<List<LibroDefault>> GetLibrosNotInBiblioteca(int id)
        {
            var consulta = from libro in this.context.LibrosDef
                           where !(from lb in this.context.LibrosBiblio
                                   where lb.ID_BIBLIOTECA == id
                                   select lb.ID_LIBRO)
                                   .Contains(libro.ID_LIBRO)
                           select libro;

            return await consulta.ToListAsync();
        }

        public async Task AddLibro(LibroDefault libro)
        {
            libro.ID_LIBRO = this.context.LibrosDef.Any() ? this.context.LibrosDef.Max(x => x.ID_LIBRO) + 1 : 1;
            this.context.LibrosDef.Add(libro);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateLibro(LibroDefault libro)
        {
            LibroDefault b = await GetDatosLibroDefAsync(libro.ID_LIBRO);
            b.NOMBRE = libro.NOMBRE;
            b.NUM_PAGINAS = libro.NUM_PAGINAS;
            b.IMAGEN = libro.IMAGEN;
            b.URL_COMPRA = libro.URL_COMPRA;
            b.DESCRIPCION = libro.DESCRIPCION;
            b.IDIOMA = libro.IDIOMA;
            b.FECHA_PUBLICACION = libro.FECHA_PUBLICACION;
            b.ID_AUTOR = libro.ID_AUTOR;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteLibro(int id)
        {
            LibroDefault l = this.context.LibrosDef.Where(x => x.ID_LIBRO == id).FirstOrDefault();
            this.context.LibrosDef.Remove(l);
            await this.context.SaveChangesAsync();
        }

        public async Task AddLibroBiblio(int idBiblio, int idLibro)
        {
            string sql = "SP_ADDLIBROBIBLIOTECA @ID_LIBRO ,@ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            int rowsAffected = await this.context.Database.ExecuteSqlRawAsync(sql, p1, p2);
        }

        public async Task DeleteLibroBiblio(int idBiblio, int idLibro)
        {
            string sql = "SP_DELETELIBROBIBLIOTECA @ID_LIBRO ,@ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            int rowsAffected = await this.context.Database.ExecuteSqlRawAsync(sql, p1, p2);
        }




        //VALORACIONES & COMENTARIOS
        public int GetValoraciones(int id)
        {
            var consulta = from data in this.context.Valoraciones.AsEnumerable()
                           where data.ID_LIBRO == id
                           select data;
            int cuenta = consulta.ToList().Count();
            return cuenta;
        }

        public async Task<List<Comentario>> GetComentariosLikeAsync(int id, string dni)
        {
            string sql = "SP_GETCOMENTARIOSLIBRO @ID_LIBRO ,@DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", dni ?? (object)DBNull.Value);
            var consulta = this.context.Comentarios.FromSqlRaw(sql, p1, p2);
            return await consulta.ToListAsync();
        }

        public void LikeComentario(int orden, int idComentario, string dni)
        {
            string sql = "SP_LIKECOMENTARIO @ID_COMENTARIO, @IDENT,@DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@ID_COMENTARIO", idComentario);
            SqlParameter p2 = new SqlParameter("@IDENT", orden);
            SqlParameter p3 = new SqlParameter("@DNI_USUARIO", dni);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3);
        }

        public void PostComentario(Comentario com, int rating)
        {
            string sql = "SP_CREATECOMENTARIORESENIA @ID_LIBRO, @DNI_USUARIO, @FECHA_COMENTARIO, @MENSAJE, @PUNTUACION";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", com.ID_LIBRO);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", com.USUARIO);
            SqlParameter p3 = new SqlParameter("@FECHA_COMENTARIO", com.FECHA_COMENTARIO);
            SqlParameter p4 = new SqlParameter("@MENSAJE", com.MENSAJE ?? (object)DBNull.Value);
            SqlParameter p5 = new SqlParameter("@PUNTUACION", rating);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3, p4, p5);
        }

        public void DeleteComentario(int id)
        {
            string sql = "SP_DELETECOMENTARIO @ID_COMENTARIO";
            SqlParameter p1 = new SqlParameter("@ID_COMENTARIO", id);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1);
        }



        //AUTORES
        public async Task<List<Autor>> GetAutores()
        {
            return await this.context.Autores.ToListAsync();
        }

        public async Task<List<Autor>> SearchAutorNombre(string input)
        {
            string sql = "SP_BUSCARAUTOR @INPUT";
            SqlParameter p = new SqlParameter("@INPUT", input);
            var consulta = this.context.Autores.FromSqlRaw(sql, p);
            return await consulta.ToListAsync();
        }

        public async Task<Autor> GetDatosAutor(int id)
        {
            return await this.context.Autores.Where(x => x.ID_AUTOR == id).FirstOrDefaultAsync();
        }

        public async Task AddAutor(Autor autor)
        {
            autor.ID_AUTOR = this.context.Autores.Any() ? this.context.Autores.Max(x => x.ID_AUTOR) + 1 : 1;
            this.context.Autores.Add(autor);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAutor(Autor autor)
        {
            Autor b = await GetDatosAutor(autor.ID_AUTOR);
            b.NOMBRE = autor.NOMBRE;
            b.NACIONALIDAD = autor.NACIONALIDAD;
            b.FECHA_NACIMIENTO = autor.FECHA_NACIMIENTO;
            b.IMAGEN = autor.IMAGEN;
            b.HISTORIA = autor.HISTORIA;
            b.NUM_LIBROS = autor.NUM_LIBROS;
            b.WIKI = autor.WIKI;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAutor(int id)
        {
            Autor l = this.context.Autores.Where(x => x.ID_AUTOR == id).FirstOrDefault();
            this.context.Autores.Remove(l);
            await this.context.SaveChangesAsync();
        }


        //USUARIO

        public async Task<Usuario> GetUsuario(string dni)
        {
            return await this.context.Usuarios.Where(x => x.DNI_USUARIO.Equals(dni)).FirstOrDefaultAsync();
        }

        public async Task UpdateUsuario(Usuario usu)
        {
            Usuario user = this.context.Usuarios.Where(x => x.DNI_USUARIO.Equals(usu.DNI_USUARIO)).FirstOrDefault();
            user.NOMBRE = usu.NOMBRE;
            user.APELLIDO = usu.APELLIDO;
            user.EMAIL = usu.EMAIL;
            user.TELEFONO = usu.TELEFONO;
            user.USUARIO = usu.USUARIO;
            await this.context.SaveChangesAsync();
        }

        public async Task<int> NumComentariosUsuario(string id)
        {
            return await this.context.ComentariosBasico.Where(x => x.DNI_USUARIO.Equals(id)).CountAsync();
        }

        public async Task<int> NumReseñasUsuario(string id)
        {
            return await this.context.Valoraciones.Where(x => x.DNI_USUARIO.Equals(id)).CountAsync();
        }

        public async Task AddListaLibro(string dni, int idLibro, int orden)
        {
            DateTime fecha = DateTime.Now;
            string sql = "SP_LISTADESEOS @ORDEN , @DNI_USUARIO, @ID_LIBRO, @FECHA";
            SqlParameter p1 = new SqlParameter("@ORDEN", orden);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p4 = new SqlParameter("@FECHA", fecha);
            int rowsAffected = await this.context.Database.ExecuteSqlRawAsync(sql, p1, p2, p3, p4);
        }

        public async Task<int> NLibrosLeidos(string id)
        {
            return await this.context.ListaDeseos.Where(x => x.DNI_USUARIO.Equals(id) && x.LEIDO == 1).CountAsync();
        }

        public async Task<List<ReservaUsuario>> GetReservasUsuario(string id)
        {
            string sql = "SP_PRESTAMOSUSUARIO @DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", id);
            var consulta = this.context.ReservasUsuario.FromSqlRaw(sql, p1);
            return await consulta.ToListAsync();
        }

        public async Task<List<ComentarioBasico>> GetComentariosUsuario(string id)
        {
            return await this.context.ComentariosBasico.Where(x => x.DNI_USUARIO.Equals(id)).ToListAsync();
        }

        public async Task<List<LibroDeseo>> GetFavoritos(string id)
        {
            string sql = "SP_LIBROSLISTA @DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", id);
            var consulta = this.context.LibrosDeseo.FromSqlRaw(sql, p1);
            return await consulta.ToListAsync();
        }

        public async Task<Compartido> SetGetToken(string dni, string token)
        {
            //return await this.context.Share.Where(x => x.TOKEN == token && x.DNI_USUARIO == dni).FirstOrDefaultAsync();
            string sql = "SP_COMPARTIR @DNI_USUARIO , @TOKEN";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p2 = new SqlParameter("@TOKEN", token);
            return this.context.Share.FromSqlRaw(sql, p1, p2).AsEnumerable().FirstOrDefault();
        }

        public async Task<Compartido> GetShare(string id)
        {
            return await this.context.Share.Where(x => x.DNI_USUARIO.Equals(id)).FirstOrDefaultAsync();
        }

        public int LibroDeseo(int idLibro, string dni)
        {
            var consulta = from data in this.context.ListaDeseos
                           where data.ID_LIBRO == idLibro && data.DNI_USUARIO.Equals(dni)
                           select data.LEIDO;

            if (!consulta.Any())
            {
                return -1;
            }
            else
            {
                return consulta.FirstOrDefault();
            }
        }

        public async Task Register(Usuario user)
        {
            user.ROL = "USUARIO";
            user.SALT = HelperCryptography.GenerateSalt();
            user.PASSWORD = HelperCryptography.EncryptPassword(user.CONTRASEÑA, user.SALT);

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }


        //RESERVAS
        public async Task<List<ReservaNLibro>> GetReservasBiblio(int id)
        {
            string sql = "SP_GETPRESTAMOBIBLIOTECA @ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_BIBLIOTECA", id);
            List<ReservaNLibro> consulta = await this.context.ReservaNLibros.FromSqlRaw(sql, p1).ToListAsync();
            return consulta;
        }

        public async Task<List<Reserva>> GetResrevasLibro(int id , int idBiblio)
        {
            return await this.context.Reservas.Where(x => x.ID_LIBRO == id && x.ID_BIBLIOTECA == idBiblio).ToListAsync();
        }

        public async Task<List<BibliotecaSimple>> GetLibroDisponible(int id)
        {
            string sql = "SP_LIBROBIBLIOTECADISPONIBLE @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            List<BibliotecaSimple> consulta = await this.context.BibliotecasSimples.FromSqlRaw(sql, p1).ToListAsync();
            return consulta;
        }
        
        public async Task CreateReserva(Reserva reserva)
        {
            int nuevoId = this.context.Reservas.Any() ? this.context.Reservas.Max(x => x.ID_PRESTAMO) + 1 : 1;
            reserva.ID_PRESTAMO = nuevoId;
            reserva.DEVUELTO = true;
            reserva.COMPLETADO = false;
            this.context.Reservas.Add(reserva);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteReserva(int id)
        {
            Reserva r = this.context.Reservas.Where(x => x.ID_PRESTAMO == id).FirstOrDefault();
            this.context.Reservas.Remove(r);
            await this.context.SaveChangesAsync();
        }

        public async Task RecogerLibro(int idPrestamo, int idBiblio)
        {
            string sql = "SP_PRESTARLIBRO @ID_PRESTAMO ,@ID_BIBLIOTECA, @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_PRESTAMO", idPrestamo);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            Reserva r = await this.context.Reservas.Where(x => x.ID_PRESTAMO == idPrestamo).FirstOrDefaultAsync();
            int idLibro = r.ID_LIBRO;
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3);
        }

        public async Task DevolverLibro(int idPrestamo, int idBiblio)
        {
            string sql = "SP_RECIVIRLIBRO @ID_PRESTAMO ,@ID_BIBLIOTECA, @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_PRESTAMO", idPrestamo);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            Reserva r = await this.context.Reservas.Where(x => x.ID_PRESTAMO == idPrestamo).FirstOrDefaultAsync();
            int idLibro = r.ID_LIBRO;
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3);
        }




        //OTORS
    
        public List<string> GetDaysBetween(DateTime fecha_inicio, DateTime fecha_fin)
        {
            var days = new List<string>();
            for (DateTime date = fecha_inicio; date <= fecha_fin; date = date.AddDays(1))
            {
                days.Add(date.ToString("dd/MM/yyyy"));
            }
            return days;
        }

        public string GenerateToken()
        {
            var token = Guid.NewGuid().ToString("N") + new Random().Next(1000, 9999).ToString();
            return token;
        }
    }
}
