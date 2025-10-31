using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("COTACAO_PRODUTOS")]
    public partial class COTACAO_PRODUTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cotacao_produtos { get; set; }
        public DateTime data_cotacao_produtos { get; set; }
        public int id_usuario_comprador { get; set; }
        public bool virou_pedido { get; set; }
        public int id_empresa_fornecedor_pedido { get; set; }

        [ForeignKey("id_usuario_comprador")]
        public virtual USUARIO_COMPRADOR usuario_comprador { get; set; }

        [ForeignKey("id_empresa_fornecedor_pedido")]
        public virtual EMPRESA_FORNECEDOR empresa_fornecedor { get; set; }
    }
}
