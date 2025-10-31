using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("avaliacao_empresa_cotada")]
    public partial class avaliacao_empresa_cotada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_AVALIACAO_EMPRESA_COTADA { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_AVALIADA { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_AVALIOU { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_AVALIOU { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER { get; set; }

        [Required]
        public int ID_CODIGO_CLASSIFICACAO_EMPRESA { get; set; }

        [Required]
        public System.DateTime DATA_AVALIACAO_EMPRESA_COTADA { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_AVALIADA")]
        public virtual empresa_usuario empresa_usuario_avaliada { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_AVALIOU")]
        public virtual empresa_usuario empresa_usuario_avaliou { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_AVALIOU")]
        public virtual usuario_empresa usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER")]
        public virtual cotacao_master_usuario_empresa cotacao_master_empresa { get; set; }

        [ForeignKey("ID_CODIGO_CLASSIFICACAO_EMPRESA")]
        public virtual classificacao_empresas_cotadas classificacao_empresas_cotadas { get; set; }
    }
}
