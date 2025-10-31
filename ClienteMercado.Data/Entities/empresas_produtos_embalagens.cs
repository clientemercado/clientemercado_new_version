using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresas_produtos_embalagens")]
    public partial class empresas_produtos_embalagens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EMPRESAS_PRODUTOS_EMBALAGENS { get; set; }

        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [Required]
        public string DESCRICAO_PRODUTO_EMBALAGEM { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servicos_empresa { get; set; }
    }
}
