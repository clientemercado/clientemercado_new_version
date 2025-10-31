using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("tipos_frete")]
    public partial class tipos_frete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_TIPO_FRETE { get; set; }

        [Required]
        [MaxLength(40)]
        public string DESCRICAO_TIPO_FRETE { get; set; }
    }
}
