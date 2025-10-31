using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("classificacao_empresas_cotadas")]
    public partial class classificacao_empresas_cotadas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_CLASSIFICACAO_EMPRESA { get; set; }

        [Required]
        [MaxLength(30)]
        public string DESCRICAO_CLASSIFICACAO_EMPRESA { get; set; }

        [Required]
        public int PONTUACAO_CLASSIFICACAO_EMPRESA { get; set; }
    }
}
