using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ProyectoNetCore.Models;
using ScrapySharp.Extensions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Repositories;

namespace ProyectoNetCore.Controllers.Acciones
{
    public class AccionGeneralController : Controller
    {
        private HelperAccion helperAccion;
        private RepositoryAcciones repo;
        private RepositoryUsuarios repoUser;
        public AccionGeneralController(RepositoryAcciones repo,HelperAccion helperAccion, RepositoryUsuarios repoUser) {
            this.repo = repo;
            this.helperAccion = helperAccion;
            this.repoUser = repoUser;
        }
        public IActionResult ListaAcciones()
        {            
            //var acciones = this.helperAccion.InsertarAccionDia(url);
            var acciones = this.helperAccion.PedirAccionesPag();
            //this.repo.InsertarAccionDia();


            return View(acciones);
        }
        public async Task<IActionResult> Prueba() { 

            string prueba = await this.repo.InsertarAccionDia();
            List < Accion > acciones = await this.repo.PedirAccionesBBDD();
            ViewData["PRUEBA"] = prueba;

            return View(acciones);

        }
    }
}