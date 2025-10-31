using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("tipo_empresa_usuario")]
    public partial class tipo_empresa_usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(100)]
        public string DESCRICAO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(1)]
        public string FISICA_JURIDICA_EMPRESA_USUARIO { get; set; }
    }
}
