using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cards_usuario_empresa")]
    public partial class cards_usuario_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CARDS_USUARIO_EMPRESA { get; set; }

        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string NUMERO_CARD_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_CARD_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string MES_EXPIRACAO_CARD_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string ANO_EXPIRACAO_CARD_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string CODIGO_SEGURANCA_CARD_USUARIO_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }
    }
}
