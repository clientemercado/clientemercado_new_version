using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("pedido_central_compras")]
    public partial class pedido_central_compras
    {
        public pedido_central_compras()
        {
            this.itens_pedido_central_compras = new List<itens_pedido_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_COTACAO_MASTER_CENTRAL_COMPRAS { get; set; }
        [Required]
        public int ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }
        [Required]
        public decimal VALOR_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public System.DateTime DATA_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public System.DateTime DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS { get; set; }
        [Required]
        public bool CONFIRMADO_PEDIDO_CENTRAL_COMPRAS { get; set; }
        public int? ID_FORMA_PAGAMENTO { get; set; }
        public int? ID_TIPO_FRETE { get; set; }
        public string COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS { get; set; }
        public bool PEDIDO_ENTREGUE_FINALIZADO { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_CENTRAL_COMPRAS")]
        public virtual cotacao_master_central_compras cotacao_master_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS")]
        public virtual cotacao_filha_central_compras cotacao_filha_central_compras { get; set; }

        [ForeignKey("ID_FORMA_PAGAMENTO")]
        public virtual forma_pagamento forma_pagamento { get; set; }

        [ForeignKey("ID_TIPO_FRETE")]
        public virtual tipos_frete tipo_frete { get; set; }

        public virtual ICollection<itens_pedido_central_compras> itens_pedido_central_compras { get; set; }
    }
}
