using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("chat_cotacao_usuario_cotante")]
    public partial class chat_cotacao_usuario_cotante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_EMPRESA_COTADA { get; set; }

        [Required]
        public System.DateTime DATA_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        [MaxLength(200)]
        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE")]
        public virtual cotacao_filha_usuario_cotante cotacao_filha_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_COTANTE")]
        public virtual usuario_cotante usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_EMPRESA_COTADA")]
        public virtual usuario_empresa usuario_empresa_cotada { get; set; }
    }
}
