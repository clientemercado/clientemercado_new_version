using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("chat_cotacao_central_compras")]
    public partial class chat_cotacao_central_compras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_CHAT_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_EMPRESA_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_EMPRESA_COTADA { get; set; }

        [Required]
        public System.DateTime DATA_CHAT_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        [MaxLength(200)]
        public string TEXTO_CHAT_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_DIALOGANDO { get; set; }

        [Required]
        public int ORDEM_EXIBICAO_CHAT_COTACAO_CENTRAL_COMPRAS { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS")]
        public virtual cotacao_filha_central_compras cotacao_filha_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_EMPRESA_COTANTE")]
        public virtual usuario_empresa usuario_empresa_cotante { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_EMPRESA_COTADA")]
        public virtual usuario_empresa usuario_empresa_cotada { get; set; }
    }
}
