using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresas_fabricantes_marcas")]
    public partial class empresas_fabricantes_marcas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }

        [Required]
        [MaxLength(20)]
        public string DESCRICAO_EMPRESA_FABRICANTE_MARCAS { get; set; }
    }
}
