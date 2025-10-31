using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("forma_pagamento")]
    public partial class forma_pagamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FORMA_PAGAMENTO { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA { get; set; }
        public string DESCRICAO_FORMA_PAGAMENTO { get; set; }
        public int NUMERO_PARCELAS_FORMA_PAGAMENTO { get; set; }
        public int PRIMEIRO_INTERVALO_FORMA_PAGAMENTO { get; set; }
        public int INTERVALO_DEMAIS_PARCELAS_FORMA_PAGAMENTO { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }
    }
}
