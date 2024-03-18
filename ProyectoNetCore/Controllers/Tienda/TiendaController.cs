using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;
using ProyectoNetCore.Repositories;
using System;
using System.Globalization;
using System.Security.Claims;
using static ScrapySharp.Core.Token;
using LiveCharts;
using LiveCharts.Wpf;
using Blazor_ApexCharts;

namespace ProyectoNetCore.Controllers.Tienda
{
    public class TiendaController : Controller
    {
        private HelperAccion helperAccion;
        private RepositoryAcciones repo;
        private RepositoryUsuarios repoUser;
        private RepositoryCompras repoCompras;
        private IMemoryCache memoryCache;
        public TiendaController( HelperAccion helperAccion, RepositoryAcciones repo, RepositoryUsuarios repoUser, RepositoryCompras repoCompras, IMemoryCache memoryCache)
        {
            this.helperAccion = helperAccion;
            this.repo = repo;
            this.repoUser = repoUser;
            this.repoCompras = repoCompras;
            this.memoryCache = memoryCache;
        }

        //public async Task<IActionResult> _GraficoPrueba()
        //{
        //    var dt = new VisualizationDataTable();
        //    dt.AddColumn("Date", "string");
        //    dt.AddColumn("Events", "number");
        //    dt
        //        .NewRow("25-05-2016", 123)
        //        .NewRow("26-05-2016", 111)
        //        .NewRow("27-05-2016", 132)
        //        .NewRow("28-05-2016", 121)
        //        .NewRow("29-05-2016", 109)
        //        .NewRow("30-05-2016", 126)
        //        ;

        //    var chart = new ChartViewModel
        //    {
        //        Title = "Events",
        //        Subtitle = "per date",
        //        DataTable = dt
        //    };

        //    return Content(JsonConvert.SerializeObject(chart), "application/json");
        //}

        //https://www.youtube.com/watch?v=JGi6Uj5AaAg
        //https://www.highcharts.com/demo/highcharts/pie-legend

        public JsonResult DataPastel() {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
            List<SeriePastel> lista = this.repoCompras.GetDataDum(usuario);
            return Json(lista);
            //SeriePastel serie = new SeriePastel("",0);

            //return Json(serie.GetDataDum());
        }

        public async Task<IActionResult> _GraficoDineroInvertido()
        {
            return PartialView("_GraficoDineroInvertido");
        }
        public async Task<IActionResult> _GraficoDineroGanado()
        {
            return PartialView("_GraficoDineroGanado");
        }
        public async Task<IActionResult> _TotalGanancias()
        {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            //double TotalGananciasCompras = await this.repoCompras.totalComprasAsync(usuario);

            double TotalGananciasCompras = 1505.12;
            //int totalCompras = 3;
            return PartialView("_TotalGanancias", TotalGananciasCompras);
        }

        public async Task<IActionResult> _TotalInvertido()
        {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            double TotalInvertidoCompras = await this.repoCompras.totalComprasAsync(usuario);

            //int totalCompras = 3;
            return PartialView("_TotalInvertido", TotalInvertidoCompras);
        }

        public async Task<IActionResult> _ComprasRealizadas()
        {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            //int totalCompras = await this.repoCompras.totalComprasAsync(usuario);

            int totalNumeroCompras = await this.repoCompras.numeroComprasAsync(usuario);
            ViewData["TOTALNUMEROCOMPRAS"] = totalNumeroCompras;

            //int totalCompras = 3;
            return PartialView("_ComprasRealizadas",totalNumeroCompras);
        }

        public async Task<IActionResult> _TotalAcciones()
        {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            //int totalCompras = await this.repoCompras.totalComprasAsync(usuario);

            int totalCompras = await this.repoCompras.cantidadComprasAsync(usuario);
            ViewData["TOTALCOMPRAS"] = totalCompras;
            //int totalCompras = 3;
            return PartialView("_TotalAcciones",totalCompras);
        }


        public async Task<IActionResult> Resumen()
        {

            int usuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            double Totalcompras = await this.repoCompras.totalComprasAsync(usuario);

            //List<Compra> Orders = await this.repoCompras.GetCompras();

            //return StatusCode(StatusCodes.Status200OK, Totalcompras); 
             return View();
        }


        public async Task<IActionResult> CompraCompleta()
        {

            //string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["MENSAJE"] = "compra realizada";
            return View();
            
        }

        public IActionResult Pendientes()
        {
            if (HttpContext.Session.GetString("USUARIO") == null)
            {
                return RedirectToAction("AccesoDenegado", "Managed");
            }

            List<Accion> acciones =  new List<Accion>();
            if (this.memoryCache.Get<List<Accion>>("PENDIENTES") != null) 
            {
                acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES").ToList();
            }
           

            return View(acciones);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Pendientes(int? idaccion
/*             string? producto*/)
        {
            //COMPROBAMOS SI EXISTE NUESTRO USUARIO ANTES 
            //DE REALIZAR LA COMPRA
            if (HttpContext.Session.GetString("USUARIO") == null)
            {
                return RedirectToAction("AccesoDenegado", "Managed");
            }
            else
            {

                return RedirectToAction("PedidoFinal", idaccion);
            }
        }
        public async Task<IActionResult> Productos(int? addpendiente, int? ideliminar)
        {
            //comprobamos si existe el usuario para dejarle entrar o no 
            if (HttpContext.Session.GetString("USUARIO") == null)
            {
                return RedirectToAction("AccesoDenegado", "Managed");
            }
            List<Accion> acciones = await this.repo.PedirAccionesBBDD();
            //var acciones = this.helperAccion.PedirAccionesPag();


            if (addpendiente != null)
            {
                List<Accion> accionesPendientes;
                if (this.memoryCache.Get("PENDIENTES") == null)
                {
                    accionesPendientes = new List<Accion>();
                }
                else
                {
                    accionesPendientes = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                }
                Accion accionPendiente = await this.repo.FindAccionAsync(addpendiente.Value);
                accionesPendientes.Add(accionPendiente);
                this.memoryCache.Set("PENDIENTES", accionesPendientes);
                ViewData["FAVS"] = this.memoryCache.Get<List<Accion>>("PENDIENTES");

            }
            //List<Accion> accionesTodas = await this.repo.PedirAccionesBBDD();
            //return View(accionesTodas);
            if (ideliminar != null)
            {
                List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                 Accion accion = accionesP.Find(z => z.ID == ideliminar.Value);
                accionesP.Remove(accion);

                if (acciones.Count == 0)
                {
                    this.memoryCache.Remove("PENDIENTES");
                    this.memoryCache.Remove("TOTAL");
                }
                else
                {
                    this.memoryCache.Set("PENDIENTES", accionesP);
                }

            }
            if (this.memoryCache.Get("PENDIENTES") != null)
            {
                List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                int suma = accionesP.Count();
                this.memoryCache.Set("TOTAL", suma);
                ViewData["FAVS"] = this.memoryCache.Get<List<Accion>>("PENDIENTES");
            }
            string prueba = await this.repo.InsertarAccionDia();
            List<Accion> accionesTodas = await this.repo.PedirAccionesBBDD();
            return View(accionesTodas);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Productos(int? idaccion, int?addpendiente, int? ideliminar
/*             string? producto*/)
        {
            //COMPROBAMOS SI EXISTE NUESTRO USUARIO ANTES 
            //DE REALIZAR LA COMPRA
            if (HttpContext.Session.GetString("USUARIO") == null)
            {
                return RedirectToAction("AccesoDenegado", "Managed");
            }
            else
            {

                return RedirectToAction("PedidoFinal", idaccion);
            }
        }

        public async Task<IActionResult> PedidoFinal(int idaccion)
        {
            Accion accion = await this.repo.FindAccionAsync(idaccion);
            
            this.memoryCache.Set("ACCION", accion);

            return View(accion);
        }
        [HttpPost]
        public async Task<IActionResult> PedidoFinal(int idaccion, int precio, string cantidad, int total)
        {

            if (HttpContext.Session.GetString("USUARIO") == null && this.memoryCache.Get("ACCION") == null)
            {
                return RedirectToAction("ListaAcciones", "AccionGeneral");

            }
            else if (this.memoryCache.Get("ACCION") == null)
            {
                return RedirectToAction("Productos", "Tienda");
            }
            else
            {

                Compra compra = new Compra();
                Accion accion = ((Accion)this.memoryCache.Get("ACCION"));
                compra.idUsuairo = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
                compra.idAccion = accion.ID;
                //compra.Precio = double.Parse(accion.Ultimo);
                string texto = accion.Ultimo.Replace('.', ',');

                string textoOriginal = texto;
                int indiceComa = textoOriginal.IndexOf(',');
                string textoModificado = "";

                if (indiceComa != -1)
                {
                    textoModificado = textoOriginal.Remove(indiceComa, 1);
                    // textoModificado será "38722,69"
                }

                compra.Precio = double.Parse(textoModificado);
                compra.Cantidad = int.Parse(cantidad);
                compra.Total = double.Parse(textoModificado) * double.Parse(cantidad);

                this.memoryCache.Set("COMPRA", accion);

                await this.repoCompras.InsertarCompraAsync(compra);


                this.memoryCache.Remove("ACCION");
                //al realizarse la compra, se borra los datos en cache de esa accion y se bloque la entrada a esta vista, se redireccion a la lista de
                List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                Accion accioP = accionesP.Find(z => z.ID == accion.ID);
                accionesP.Remove(accioP);
                this.memoryCache.Set("PENDIENTES", accionesP);
                return RedirectToAction("CompraCompleta", "Tienda");

            }

        }

        public async Task<IActionResult> AccionesPendientes (int? ideliminar)
        {
            if (ideliminar != null)
            {
                List<Accion> acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                Accion accion = acciones.Find(z => z.ID == ideliminar.Value);
                acciones.Remove(accion);

                if (acciones.Count == 0)
                {
                    this.memoryCache.Remove("PENDIENTES");
                    this.memoryCache.Remove("TOTAL");
                }
                else
                {
                    this.memoryCache.Set("PENDIENTES", acciones);
                }

            }
            if (this.memoryCache.Get("PENDIENTES") != null)
            {
                List<Accion> acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES");
                int suma = acciones.Count();
                this.memoryCache.Set("TOTAL", suma);
            }

            return View();
        }

    }


}


