using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("bairros_empresa_usuario")]
    public partial class bairros_empresa_usuario
    {
        public bairros_empresa_usuario()
        {
            this.enderecos_empresa_usuario = new List<enderecos_empresa_usuario>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_BAIRRO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string BAIRRO_CIDADE_EMPRESA_USUARIO { get; set; }

        public int ID_CIDADE_EMPRESA_USUARIO { get; set; }

        [ForeignKey("ID_CIDADE_EMPRESA_USUARIO")]
        public virtual cidades_empresa_usuario cidades_empresa_usuario { get; set; }

        public virtual ICollection<enderecos_empresa_usuario> enderecos_empresa_usuario { get; set; }
    }
}
