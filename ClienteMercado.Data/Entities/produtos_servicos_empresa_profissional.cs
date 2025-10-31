using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("produtos_servicos_empresa_profissional")]
    public partial class produtos_servicos_empresa_profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        [MaxLength(150)]
        public string DESCRICAO_PRODUTO_SERVICO { get; set; }

        [Required]
        [MaxLength(1)]
        public string TIPO_PRODUTO_SERVICO { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }
    }
}
