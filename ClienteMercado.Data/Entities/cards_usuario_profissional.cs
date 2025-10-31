using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cards_usuario_profissional")]
    public partial class cards_usuario_profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CARDS_USUARIO_PROFISSIONAL { get; set; }

        public int ID_CODIGO_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string NUMERO_CARD_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_CARD_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(30)]
        public string MES_EXPIRACAO_CARD_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(30)]
        public string ANO_EXPIRACAO_CARD_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(30)]
        public string CODIGO_SEGURANCA_CARD_USUARIO_PROFISSIONAL { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSIONAL_USUARIO")]
        public virtual profissional_usuario profissional_usuario { get; set; }
    }
}
