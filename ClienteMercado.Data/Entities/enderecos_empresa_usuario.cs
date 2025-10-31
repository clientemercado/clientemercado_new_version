using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ClienteMercado.Data.Entities
{
    [Table("enderecos_empresa_usuario")]
    public partial class enderecos_empresa_usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required]
        public Int64 CEP_ENDERECO_EMPRESA_USUARIO { get; set; }

        //[Required]
        public DbGeography LATITUDE_LONGITUDE_CEP_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(150)]
        public string LOGRADOURO_CEP_EMPRESA_USUARIO { get; set; }

        [MaxLength(30)]
        public string TIPO_LOGRADOURO_EMPRESA_USUARIO { get; set; }

        [MaxLength(150)]
        public string COMPLEMENTO_ENDERECO_EMPRESA_USUARIO { get; set; }

        public int ID_CIDADE_EMPRESA_USUARIO { get; set; }

        public int ID_BAIRRO_EMPRESA_USUARIO { get; set; }

        [ForeignKey("ID_CIDADE_EMPRESA_USUARIO")]
        public virtual cidades_empresa_usuario cidades_empresa_usuario { get; set; }

        [ForeignKey("ID_BAIRRO_EMPRESA_USUARIO")]
        public virtual bairros_empresa_usuario bairros_empresa_usuario { get; set; }
    }
}
