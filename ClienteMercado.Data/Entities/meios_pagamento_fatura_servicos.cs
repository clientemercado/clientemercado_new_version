using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("meios_pagamento_fatura_servicos")]
    public partial class meios_pagamento_fatura_servicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_MEIO_PAGAMENTO { get; set; }

        [Required]
        [MaxLength(50)]
        public string DESCRICAO_MEIO_PAGAMENTO { get; set; }

        [MaxLength(200)]
        public string OBSERVACAO_BOLETO_COBRANCA_PAGAMENTO { get; set; }

        public bool PAGO_POR_BOLETO { get; set; }
        public bool PAGO_CARTAO_CREDITO { get; set; }
        //public bool SEM_COBRANCA { get; set; }    //Descomentar para gerar no novo banco (Nãome lembro do motivo de ter criado isso aqui, mas no momento não faz sentido usar. Se permancer sem sentido, excluir.)
    }
}
