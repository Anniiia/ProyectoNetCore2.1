using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Data;
using ProyectoNetCore.Helpers;
using ProyectoNetCore.Models;
using System.Data;
using System.Diagnostics.Metrics;
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

//create PROCEDURE SP_NOMBRE_COMPR
//    @idaccion INT, @nombre nvarchar(100) OUT
//AS
//BEGIN
//    SELECT @nombre = Nombre
//    FROM accion
//    WHERE id = @idaccion;
//select @nombre
//END

//   DECLARE @nombre NVARCHAR(100);

//exec SP_NOMBRE_COMPR 21, @nombre

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

        public List<SeriePastel> GetDataDum(int idusuario)
        {
            //List<SeriePastel> lista = new List<SeriePastel>();
            var consulta = from datos in this.contextCompras.Compras where (datos.idUsuairo == idusuario) select datos;

            List<Compra> compras = consulta.ToList();

            List<SeriePastel> lista = new List<SeriePastel>();

            foreach (var col in compras)
            {
                SeriePastel li = new SeriePastel();
                li.name = "asd" + col.idAccion;
                li.y = col.Total;
                li.sliced = false;
                li.selected = false;
                lista.Add(li);
            }


            //lista.Add(new SeriePastel("Angular", 45));
            //lista.Add(new SeriePastel("Vue", 50));
            //lista.Add(new SeriePastel("React", 30));
            //lista.Add(new SeriePastel("Css", 20));

            return lista;
        }

        public async Task<List<Compra>> GetCompras() {

            return await this.contextCompras.Compras.ToListAsync();
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
