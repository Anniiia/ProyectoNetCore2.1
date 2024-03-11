using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Data;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;

namespace ProyectoNetCore.Repositories
{
    public class RepositoryUsuarios
    {
        private UsuariosContext context;

        public RepositoryUsuarios(UsuariosContext context)
        {
            this.context = context;
        }

        public async Task ActivateUserAsync(string token)
        {
            //buscamos el usuario por su token
           Usuario user = await this.context.Usuarios.FirstOrDefaultAsync(x => x.TokenMail == token);
            //user.Activo = true;
            //tmbn se puede poner a null
            user.TokenMail = "";
            await this.context.SaveChangesAsync();
        }
        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }
        public async Task<Usuario> RegisterUsuario(string nombre, string email, string password, string imagen)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Nombre = nombre;
            user.Email = email;
            //user.Imagen = imagen;
            //cada usuario tendra un salt distinto
            user.Salt = HelperTools.GenerateSalt();
            //guardamos el password en bytes[]
            user.Password = HelperCryptography.EncryptPassword(password, user.Salt);
            //user.Activo = false;
            user.TokenMail = HelperTools.GenerateTokenMail();
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();

            return user;

        }


        //necesitamos un metodo para validar al usuario, dicho metoedo es propio del usuario
        //pasword (12345)
        //1)Recuperar el user por su email
        //2)Recuperar salt usuario
        //3)Convertimos de nuevo el password con el salt 
        //4) Recuperamos el byte[] de password de la bbdd
        //5) comparamos los dos arrays (BBDD) y el generado en el codigo

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = user.Password;
                bool response = HelperTools.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
