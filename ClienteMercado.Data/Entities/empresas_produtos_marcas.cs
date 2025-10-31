using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresas_produtos_marcas")]
    public partial class empresas_produtos_marcas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EMPRESAS_PRODUTOS_MARCAS { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }

        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_FABRICANTE_MARCAS")]
        public virtual empresas_fabricantes_marcas empresas_fabricantes_marcas { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servicos_empresa_profissional { get; set; }
    }
}
