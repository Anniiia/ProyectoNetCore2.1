using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Data;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using static ScrapySharp.Core.Token;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Principal;

#region PROCEDIMIENTOS ALMACENADOS

//CREATE TABLE ACCION(

//    [ID][int] IDENTITY(1,1) NOT NULL,

//    [NOMBRE] [nvarchar] (50) NULL,

//    [ULTIMO] [nvarchar] (50) NULL,

//    [MAXIMO] [nvarchar] (50) NULL,

//    [MINIMO] [nvarchar] (50) NULL,

//    [CAMBIO] [nvarchar] (50) NULL,

//    [CAMBIOPORC] [nvarchar] (50) NULL,

//    [FECHA] [datetime] default getdate())


//alter procedure SP_FECHA_ULTIMO_REGISTRO
//as 
//declare @maximoid int;
//select @maximoid = max(id) from accion;
//select convert(date, fecha) from accion where id = @maximoid

//DECLARE @fecha date;
//EXEC SP_FECHA_ULTIMO_REGISTRO;
//SELECT @fecha AS UltimaFecha;



#endregion

namespace ProyectoNetCore.Repositories
{
    public class RepositoryAcciones
    {
        private AccionesContext context;
        private HelperAccion helperAccion;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryAcciones(AccionesContext context, HelperAccion helperAccion) { 
            this.context = context;
            this.helperAccion = helperAccion;
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=PROYECTO;Persist Security Info=True;User ID=sa;Password=MCSD2023; Trust Server Certificate=True";
            string sql = "select * from accion";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
        }

        public async Task<string> InsertarAccionDia() {

            var acciones = this.helperAccion.PedirAccionesPag();


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_FECHA_ULTIMO_REGISTRO";
            this.cn.Open();
            var consulta = await this.com.ExecuteScalarAsync();
            string respuesta = consulta.ToString();
            string[] cadena = respuesta.Split(' ');
            string fechaBBDD = cadena[0];
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");


            if (fechaBBDD != fechaHoy)
            {
                foreach (var accion in acciones)
                {
                    this.context.Acciones.Add(accion);

                }
                this.context.SaveChanges();

            }

            return fechaBBDD;
        }

        public async Task<List<Accion>> PedirAccionesBBDD()
        {
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaBusqueda = DateTime.ParseExact(fechaHoy, "dd/MM/yyyy", null);
            var consulta = from datos in this.context.Acciones where datos.Fecha.Date==fechaBusqueda select datos;

            return consulta.ToList();
        }

        public async Task<Accion> FindAccionAsync(int idaccion)
        {
            return await this.context.Acciones.FirstOrDefaultAsync(z => z.ID == idaccion);
        }
    }
}
