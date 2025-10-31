using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_individual_empresa_central_compras")]
    public partial class itens_cotacao_individual_empresa_central_compras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }
        [Required]
        public int ID_CODIGO_UNIDADE_PRODUTO { get; set; }
        public int ID_CODIGO_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }
        [Required]
        public decimal QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS { get; set; }
        //public bool PEDIDO_ITENS_COTACAO_CENTRAL_COMPRAS { get; set; }
        public int? ID_EMPRESA_FORNECEDORA_PEDIDO { get; set; }
        [Required]
        public int ID_EMPRESAS_PRODUTOS_EMBALAGENS { get; set; }

        [ForeignKey("ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS")]
        public virtual cotacao_individual_empresa_central_compras cotacao_individual_empresa_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servicos_empresas_profissionais { get; set; }

        [ForeignKey("ID_CODIGO_UNIDADE_PRODUTO")]
        public virtual unidades_produtos unidade_produtos { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_FABRICANTE_MARCAS")]
        public virtual empresas_fabricantes_marcas empresas_fabricantes_marcas { get; set; }

        [ForeignKey("ID_EMPRESAS_PRODUTOS_EMBALAGENS")]
        public virtual empresas_produtos_embalagens empresas_produtos_embalagens { get; set; }
    }
}
