using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("pedido_usuario_empresa")]
    public partial class pedido_usuario_empresa
    {
        public pedido_usuario_empresa()
        {
            this.itens_pedido_usuario_empresa = new List<itens_pedido_usuario_empresa>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        [Required]
        public decimal VALOR_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public System.DateTime DATA_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public System.DateTime DATA_ENTREGA_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public bool CONFIRMADO_PEDIDO_USUARIO_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA")]
        public virtual cotacao_master_usuario_empresa cotacao_master_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA")]
        public virtual cotacao_filha_usuario_empresa cotacao_filha_usuario_empresa { get; set; }

        public virtual ICollection<itens_pedido_usuario_empresa> itens_pedido_usuario_empresa { get; set; }
    }
}
