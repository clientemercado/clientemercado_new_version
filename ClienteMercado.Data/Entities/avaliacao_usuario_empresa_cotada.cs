using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("avaliacao_usuario_empresa_cotada")]
    public partial class avaliacao_usuario_empresa_cotada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_AVALIACAO_USUARIO_EMPRESA_COTADA { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_AVALIADO { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_AVALIOU { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER { get; set; }

        [Required]
        public int ID_CODIGO_CLASSIFICACAO_USUARIO { get; set; }

        [Required]
        public System.DateTime DATA_AVALIACAO_USUARIO_EMPRESA_COTADA { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_AVALIADO")]
        public virtual usuario_empresa usuario_empresa_avaliado { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_AVALIOU")]
        public virtual usuario_empresa usuario_empresa_avaliou { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER")]
        public virtual cotacao_master_usuario_empresa cotacao_master_empresa { get; set; }

        [ForeignKey("ID_CODIGO_CLASSIFICACAO_USUARIO")]
        public virtual classificacao_usuario_empresas_cotadas classificacao_usuario_empresas_cotadas { get; set; }
    }
}
