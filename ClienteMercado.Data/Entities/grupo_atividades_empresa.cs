using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("grupo_atividades_empresa")]
    public partial class grupo_atividades_empresa
    {
        public grupo_atividades_empresa()
        {
            this.produtos_servicos_empresa_profissional = new List<produtos_servicos_empresa_profissional>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        [MaxLength(150)]
        public string DESCRICAO_ATIVIDADE { get; set; }

        [Required]
        public int ID_CLASSIFICACAO_EMPRESA { get; set; }

        public virtual ICollection<produtos_servicos_empresa_profissional> produtos_servicos_empresa_profissional { get; set; }
    }
}
