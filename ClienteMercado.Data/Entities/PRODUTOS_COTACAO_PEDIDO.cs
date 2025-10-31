using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("PRODUTOS_COTACAO_PEDIDO")]
    public partial class PRODUTOS_COTACAO_PEDIDO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_produtos_cotacao_pedido { get; set; }
        public int id_produto_fornecedor { get; set; }
        public decimal preco_cotacao_produto_fornecedor { get; set; }

        [ForeignKey("id_produto_fornecedor")]
        public virtual PRODUTOS_FORNECEDOR produtos_fornecedor { get; set; }
    }
}
