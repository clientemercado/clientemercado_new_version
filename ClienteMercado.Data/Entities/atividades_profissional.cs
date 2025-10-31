using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("atividades_profissional")]
    public partial class atividades_profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ATIVIDADES_PROFISSIONAL { get; set; }

        public int ID_CODIGO_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSIONAL_USUARIO")]
        public virtual profissional_usuario profissional_usuario { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servicos_empresa_profissional { get; set; }
    }
}
