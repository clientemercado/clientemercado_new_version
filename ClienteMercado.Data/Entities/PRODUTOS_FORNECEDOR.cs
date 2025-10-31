using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("PRODUTOS_FORNECEDOR")]
    public partial class PRODUTOS_FORNECEDOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_produto_fornecedor { get; set; }
        public int id_empresa_fornecedor { get; set; }
        public string nome_produto_fornecedor { get; set; }
        public string refer_cod_barras_produto_fornecedor { get; set; }
        public string unidade_produto_fornecedor { get; set; }
        public Decimal preco_produto_fornecedor { get; set; }
        public bool eh_promocao_produto_fornecedor { get; set; }

        [ForeignKey("id_empresa_fornecedor")]
        public virtual EMPRESA_FORNECEDOR empresa_fornecedor { get; set; }
    }
}
