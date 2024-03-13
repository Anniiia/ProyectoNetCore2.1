using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Data;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS

//CREATE PROCEDURE SP_INSERTAR_COMPRA
//    @idusuario int,
//    @ideaccion int,
//    @precio int,
//    @cantidad int,
//    @total int
//AS
//BEGIN
//    INSERT INTO Compras (IDUSUARIO, IDEACCION, PRECIO, CANTIDAD, TOTAL)
//    VALUES (@idusuario, @ideaccion, @precio, @cantidad, @total)
//END

#endregion

namespace ProyectoNetCore.Repositories
{
    public class RepositoryCompras
    {
        private UsuariosContext context;
        private ComprasContext contextCompras;

        public RepositoryCompras(UsuariosContext context, ComprasContext contextCompras)
        {
            this.context = context;
            this.contextCompras = contextCompras;
        }


        public async Task<int> numeroComprasAsync(int idusuario)
        {
            var consulta = from datos in this.contextCompras.Compras where (datos.idUsuairo == idusuario) select datos.idUsuairo;

            //int CantidadTotal = consulta.Sum(x=>x.Cantidad);
            int cantidad = consulta.Count();

            return cantidad;
        }
        public async Task<int> cantidadComprasAsync(int idusuario)
        {
            var consulta = from datos in this.contextCompras.Compras where(datos.idUsuairo == idusuario) select datos.Cantidad;

            //int CantidadTotal = consulta.Sum(x=>x.Cantidad);
            int cantidad = consulta.Sum();

            return cantidad;
        }


        public async Task<double> totalComprasAsync(int idusuario)
        {
            var consulta = from datos in this.contextCompras.Compras where datos.idUsuairo == idusuario select datos.Total;

            double CantidadTotal = consulta.Sum();

            return CantidadTotal;
        }

        public async Task InsertarCompraAsync(Compra compra)
        {

            string sql  = "SP_INSERTAR_COMPRA @idusuario,@idaccion, @precio, @cantidad,@total";

            SqlParameter pamUsuario = new SqlParameter("@idusuario",compra.idUsuairo);
            SqlParameter pamIdAccion = new SqlParameter("@idaccion",compra.idAccion);
            SqlParameter pamPrecio = new SqlParameter("@precio",compra.Precio);
            SqlParameter pamCantidad = new SqlParameter("@cantidad",compra.Cantidad);
            SqlParameter pamTotal = new SqlParameter("@total",compra.Total);

            var consulta = this.contextCompras.Database.ExecuteSqlRaw(sql, pamUsuario, pamIdAccion, pamPrecio, pamCantidad, pamTotal);


        }

    }      
}
