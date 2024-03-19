using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;
using ProyectoNetCore.Repositories;
using System.Numerics;
using System.Security.Claims;

namespace ProyectoNetCore.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuarios repo;
        private HelperPathProvider helperPathProvider;
        //private ISession session;

        public UsuariosController(RepositoryUsuarios repo,  HelperPathProvider helperPathProvider)
        {
            this.repo = repo;
            this.helperPathProvider = helperPathProvider;

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string email, string password, string imagen)
        {
            Usuario user = await this.repo.RegisterUsuario(nombre, email, password, imagen);
            string serverUrl = this.helperPathProvider.MapUrlServerPath();
            serverUrl = serverUrl + "/Usuarios/ActivateUser/" + user.TokenMail;
            string mensaje = "<h3>Usuario registrado</h3>";
            //mensaje += "<p>Debe activar su cuenta pulsando el siguiente enlace: </p>";
            //mensaje += "<p>" + serverUrl + "</p>";
            //mensaje += "<a href='" + serverUrl + "'>" + serverUrl + "</a><p>Muchas gracias</p>";
            ////await this.helperMails.SendMailAsync(email, "Registro usuario", mensaje);
            //ViewData["MENSAJE"] = "Usuario registrado correctamente. Hemos enviado un mail a su correo";
            //return View();

            return RedirectToAction("ActivateUser", new { token = user.TokenMail });
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario user = await this.repo.LogInUserAsync(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                
                return View();
            }
            else
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                //CREAMOS EL CLAIM PARA EL NOMBRE (APELLIDO)
                Claim claimName =
                    new Claim(ClaimTypes.Name, user.Email);
                identity.AddClaim(claimName);
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString());
                identity.AddClaim(claimId);
                //Claim claimEspecialidad =
                //    new Claim(ClaimTypes.Role, doctor.Especialidad);
                //identity.AddClaim(claimEspecialidad);

                //COMO POR AHORA NO VOY A UTILIZAR NI SE UTILIZAR ROLES
                //NO LO INCLUIMOS
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
                //LO VAMOS A LLEVAR A UNA VISTA QUE TODAVIA NO TENEMOS
                //QUE SERA EL PERFIL DEL EMPLEADO
                string controller = TempData["CONTROLLER"].ToString();
                string action = TempData["ACTION"].ToString();

                return RedirectToAction(action, controller);



                //HttpContext.Session.SetString("IDUSUARIO", user.IdUsuario.ToString());
                //HttpContext.Session.SetString("USUARIO", email);
                //return RedirectToAction("Productos", "Tienda");
                //return View(user);
            }
        }

        public async Task<IActionResult> ActivateUser(string token)
        {
            await this.repo.ActivateUserAsync(token);
            ViewData["MENSAJE"] = "Cuenta activada correctamente";

            return View();
        }

        public async Task<IActionResult> Logout() {

            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

            //HttpContext.Session.Remove("IDUSUARIO");
            //HttpContext.Session.Remove("USUARIO");
            //return View();
        }
    }
}
