using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("profissoes_usuario")]
    public partial class profissoes_usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required]
        [MaxLength(100)]
        public string DESCRICAO_PROFISSAO { get; set; }
    }
}
