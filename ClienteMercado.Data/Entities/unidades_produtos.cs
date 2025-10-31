using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("unidades_produtos")]
    public partial class unidades_produtos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_UNIDADE_PRODUTO { get; set; }

        [Required]
        [MaxLength(10)]
        public string DESCRICAO_UNIDADE_PRODUTO { get; set; }
    }
}
