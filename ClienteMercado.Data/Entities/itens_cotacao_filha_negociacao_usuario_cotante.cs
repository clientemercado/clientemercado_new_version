using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_filha_negociacao_usuario_cotante")]
    public partial class itens_cotacao_filha_negociacao_usuario_cotante
    {
        public itens_cotacao_filha_negociacao_usuario_cotante()
        {
            this.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante = new List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_RESPOSTA_COTACAO { get; set; }

        [Required]
        public int ID_CLASSIFICACAO_TIPO_ITENS_COTACAO { get; set; }

        [Required]
        [MaxLength(100)]
        public string DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_COTANTE { get; set; }

        [Required]
        public decimal PRECO_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public decimal QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public bool PRODUTO_COTADO_USUARIO_COTANTE { get; set; }

        public bool ITEM_COTACAO_FILHA_EDITADO { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE")]
        public virtual cotacao_filha_usuario_cotante cotacao_filha_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE")]
        public virtual itens_cotacao_usuario_cotante itens_cotacao_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_RESPOSTA_COTACAO")]
        public virtual tipo_resposta_cotacao tipo_resposta_cotacao { get; set; }

        [ForeignKey("ID_CLASSIFICACAO_TIPO_ITENS_COTACAO")]
        public virtual classificacao_tipo_itens_cotacao classificacao_tipo_itens_cotacao { get; set; }

        public virtual ICollection<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante { get; set; }
    }
}
