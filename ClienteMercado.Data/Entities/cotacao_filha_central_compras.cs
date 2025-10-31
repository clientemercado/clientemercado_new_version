using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cotacao_filha_central_compras")]
    public partial class cotacao_filha_central_compras
    {
        public cotacao_filha_central_compras()
        {
            this.itens_cotacao_filha_negociacao_central_compras = new List<itens_cotacao_filha_negociacao_central_compras>();
            this.chat_cotacao_central_compras = new List<chat_cotacao_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_COTACAO_MASTER_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        public int ID_TIPO_FRETE { get; set; }

        [Required]
        public bool RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public DateTime DATA_RESPOSTA_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        public DateTime DATA_RECEBEU_COTACAO_CENTRAL_COMPRAS { get; set; }

        public bool SOLICITAR_CONFIRMACAO_ACEITE_COTACAO { get; set; }

        public string DESCRICAO_MOTIVO_REJEITOU_PEDIDO { get; set; }

        [Required]
        [MaxLength(50)]
        public string FORMA_PAGAMENTO_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        public int TIPO_DESCONTO { get; set; }

        public decimal PERCENTUAL_DESCONTO { get; set; }

        [Required]
        public decimal PRECO_LOTE_ITENS_COTACAO_CENTRAL_COMPRAS { get; set; }

        [MaxLength(300)]
        public string OBSERVACAO_COTACAO_CENTRAL_COMPRAS { get; set; }

        //[Required]
        //public bool COTACAO_FILHA_CENTRAL_COMPRAS_EDITADA { get; set; }

        public bool? RECEBEU_CONTRA_PROPOSTA { get; set; }

        public bool? ACEITOU_CONTRA_PROPOSTA { get; set; }

        public bool REJEITOU_CONTRA_PROPOSTA { get; set; }

        public int? ID_CODIGO_PEDIDO_CENTRAL_COMPRAS { get; set; }

        public bool RECEBEU_PEDIDO { get; set; }

        public DateTime DATA_RECEBEU_PEDIDO { get; set; }

        public bool CONFIRMOU_PEDIDO { get; set; }

        public DateTime DATA_CONFIRMOU_PEDIDO { get; set; }

        public bool REJEITOU_PEDIDO { get; set; }

        public DateTime DATA_REJEITOU_PEDIDO { get; set; }
        public DateTime? DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS { get; set; }
        public int? ID_FORMA_PAGAMENTO { get; set; }

        [ForeignKey("ID_COTACAO_MASTER_CENTRAL_COMPRAS")]
        public virtual cotacao_master_central_compras cotacao_master_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }

        [ForeignKey("ID_TIPO_FRETE")]
        public virtual tipos_frete tipo_frete { get; set; }

        [ForeignKey("ID_FORMA_PAGAMENTO")]
        public virtual forma_pagamento forma_pagamento { get; set; }

        //[ForeignKey("ID_CODIGO_PEDIDO_CENTRAL_COMPRAS")]
        //public virtual pedido_central_compras pedido_central_compras { get; set; }

        public virtual ICollection<itens_cotacao_filha_negociacao_central_compras> itens_cotacao_filha_negociacao_central_compras { get; set; }

        public virtual ICollection<chat_cotacao_central_compras> chat_cotacao_central_compras { get; set; }
    }
}
