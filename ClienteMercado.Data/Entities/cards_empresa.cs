using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cards_empresa")]
    public partial class cards_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CARDS_EMPRESA { get; set; }

        public int ID_CODIGO_EMPRESA { get; set; }

        [Required]
        [MaxLength(50)]
        public string NUMERO_CARD_EMPRESA { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_CARD_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string MES_EXPIRACAO_CARD_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string ANO_EXPIRACAO_CARD_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string CODIGO_SEGURANCA_CARD_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }
    }
}
