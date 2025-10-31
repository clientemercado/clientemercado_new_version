using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_pedido_usuario_cotante")]
    public partial class itens_pedido_usuario_cotante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITENS_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE { get; set; }

        [ForeignKey("ID_CODIGO_PEDIDO_USUARIO_COTANTE")]
        public virtual pedido_usuario_cotante pedido_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE")]
        public virtual itens_cotacao_filha_negociacao_usuario_cotante itens_cotacao_filha_negociacao_usuario_cotante { get; set; }
    }
}
