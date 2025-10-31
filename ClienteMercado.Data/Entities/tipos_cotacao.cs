using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("tipos_cotacao")]
    public partial class tipos_cotacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_TIPO_COTACAO { get; set; }

        [Required]
        [MaxLength(30)]
        public string DESCRICAO_TIPO_COTACAO { get; set; }
    }
}
