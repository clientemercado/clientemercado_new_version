using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante")]
    public partial class fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_ARQUIVO_IMAGEM { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE")]
        public virtual itens_cotacao_filha_negociacao_usuario_cotante itens_cotacao_filha_negociacao_usuario_cotante { get; set; }
    }
}
