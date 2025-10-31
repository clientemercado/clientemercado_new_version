using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("pedido_usuario_cotante")]
    public partial class pedido_usuario_cotante
    {
        public pedido_usuario_cotante()
        {
            this.itens_pedido_usuario_cotante = new List<itens_pedido_usuario_cotante>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        [Required]
        public decimal VALOR_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public System.DateTime DATA_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public System.DateTime DATA_ENTREGA_PEDIDO_USUARIO_COTANTE { get; set; }

        [Required]
        public bool CONFIRMADO_PEDIDO_USUARIO_COTANTE { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE")]
        public virtual cotacao_master_usuario_cotante cotacao_master_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE")]
        public virtual cotacao_filha_usuario_cotante cotacao_filha_usuario_cotante { get; set; }

        public virtual ICollection<itens_pedido_usuario_cotante> itens_pedido_usuario_cotante { get; set; }
    }
}
