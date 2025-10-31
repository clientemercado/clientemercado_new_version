using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_pedido_central_compras")]
    public partial class itens_pedido_central_compras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITENS_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS { get; set; }
        public bool ITEM_PEDIDO_ENTREGUE { get; set; }
        public System.DateTime? DATA_ENTREGA_ITEM { get; set; }

        [ForeignKey("ID_CODIGO_PEDIDO_CENTRAL_COMPRAS")]
        public virtual pedido_central_compras pedido_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS")]
        public virtual itens_cotacao_filha_negociacao_central_compras itens_cotacao_filha_negociacao_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS")]
        public virtual itens_cotacao_individual_empresa_central_compras itens_cotacao_individual_empresa_central_compras { get; set; }
    }
}
