using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("estados_empresa_usuario")]
    public partial class estados_empresa_usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ESTADOS_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_PAISES_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(2)]
        public string UF_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string ESTADO_EMPRESA_USUARIO { get; set; }

        [ForeignKey("ID_PAISES_EMPRESA_USUARIO")]
        public virtual paises_empresa_usuario paises_empresa_usuario { get; set; }
    }
}
