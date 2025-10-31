using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("financeiro_cobranca_faturamento_usuario_profissional")]
    public partial class financeiro_cobranca_faturamento_usuario_profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_COBRANCA_FATURAMENTO_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int ID_MEIO_PAGAMENTO { get; set; }

        [Required]
        public System.DateTime VENCIMENTO_FATURA_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO { get; set; }

        public bool FINANCEIRO_TITULO_GERADO { get; set; }

        [Required]
        public bool PARCELA_PAGA_COBRANCA_FATURAMENTO { get; set; }

        public System.DateTime DATA_PAGAMENTO_COBRANCA_FATURAMENTO { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_CONTRATO_COTADA")]
        public virtual tipos_contratos_servicos tipos_contratos_servicos { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_PROFISSIONAL")]
        public virtual usuario_profissional usuario_profissional { get; set; }

        [ForeignKey("ID_MEIO_PAGAMENTO")]
        public virtual meios_pagamento_fatura_servicos meios_pagamento_fatura_servicos { get; set; }
    }
}
