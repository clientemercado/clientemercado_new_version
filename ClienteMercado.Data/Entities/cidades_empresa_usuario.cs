using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cidades_empresa_usuario")]
    public partial class cidades_empresa_usuario
    {
        public cidades_empresa_usuario()
        {
            this.bairros_empresa_usuario = new List<bairros_empresa_usuario>();
            this.enderecos_empresa_usuario = new List<enderecos_empresa_usuario>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CIDADE_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_ESTADOS_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(100)]
        public string CIDADE_EMPRESA_USUARIO { get; set; }

        //[Required]
        //[MaxLength(2)]
        //public string UF_CIDADE_EMPRESA_USUARIO { get; set; }

        //public float CODIGO_IBGE_CIDADE { get; set; }
        //public float AREA_IBGE { get; set; }

        [ForeignKey("ID_ESTADOS_EMPRESA_USUARIO")]
        public virtual estados_empresa_usuario estados_empresa_usuario { get; set; }

        public virtual ICollection<bairros_empresa_usuario> bairros_empresa_usuario { get; set; }
        public virtual ICollection<enderecos_empresa_usuario> enderecos_empresa_usuario { get; set; }
    }
}
