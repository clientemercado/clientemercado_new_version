using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("paises_empresa_usuario")]
    public partial class paises_empresa_usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_PAISES_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string PAIS_EMPRESA_USUARIO { get; set; }
    }
}
