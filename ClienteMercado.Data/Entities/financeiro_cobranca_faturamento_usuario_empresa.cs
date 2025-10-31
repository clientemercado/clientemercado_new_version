using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("financeiro_cobranca_faturamento_usuario_empresa")]
    public partial class financeiro_cobranca_faturamento_usuario_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_COBRANCA_FATURAMENTO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        public int ID_MEIO_PAGAMENTO { get; set; }

        [Required]
        public System.DateTime VENCIMENTO_FATURA_USUARIO_EMPRESA { get; set; }

        [Required]
        public int USUARIO_PAGARA_COBRANCA_FATURAMENTO { get; set; }

        [Required]
        public int DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO { get; set; }

        public bool FINANCEIRO_TITULO_GERADO { get; set; }

        [Required]
        public bool PARCELA_PAGA_COBRANCA_FATURAMENTO { get; set; }

        public System.DateTime DATA_PAGAMENTO_COBRANCA_FATURAMENTO { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_CONTRATO_COTADA")]
        public virtual tipos_contratos_servicos tipos_contratos_servicos { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }

        [ForeignKey("ID_MEIO_PAGAMENTO")]
        public virtual meios_pagamento_fatura_servicos meios_pagamento_fatura_servicos { get; set; }
    }
}
