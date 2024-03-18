using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoNetCore.Models
{
    [Table("COMPRAS")]
    public class Compra
    {
        [Key]
        [Column("IDCOMPRA")]
        public int idCompra { get; set; }


        [Column("IDUSUARIO")]
        public int idUsuairo { get; set; }

        [Column("IDACCION")]
        public int idAccion{ get; set; }

        [Column("PRECIO")]
        public double Precio { get; set; }

        [Column("CANTIDAD")]
        public int Cantidad { get; set; }

        [Column("TOTAL")]
        public double Total { get; set; }


    }
}
