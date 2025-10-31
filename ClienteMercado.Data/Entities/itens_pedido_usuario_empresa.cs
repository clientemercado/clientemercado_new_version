using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_pedido_usuario_empresa")]
    public partial class itens_pedido_usuario_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITENS_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_PEDIDO_USUARIO_EMPRESA")]
        public virtual pedido_usuario_empresa pedido_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA")]
        public virtual itens_cotacao_filha_negociacao_usuario_empresa itens_cotacao_filha_negociacao_usuario_empresa { get; set; }
    }
}
