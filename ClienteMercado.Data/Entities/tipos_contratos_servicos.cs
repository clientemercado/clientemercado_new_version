using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("tipos_contratos_servicos")]
    public partial class tipos_contratos_servicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        [MaxLength(6)]
        public string CODIGO_REFERENCIA_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        [MaxLength(20)]
        public string DESCRICAO_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        public decimal VALOR_MENSAL_TIPO_CONTRATO_COTADA { get; set; }

        [Required]
        public bool PAGA_FATURA_EMPRESA { get; set; }

        [Required]
        public bool PAGA_FATURA_USUARIO { get; set; }

        public System.DateTime DATA_CADASTRO_TIPO_CONTRATO { get; set; }
        public bool ATIVO_TIPO_CONTRATO_COTADA { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_TIPO_CONTRATO_COTADA { get; set; }
        public Nullable<int> ID_USUARIO_INATIVOU { get; set; }
    }
}
