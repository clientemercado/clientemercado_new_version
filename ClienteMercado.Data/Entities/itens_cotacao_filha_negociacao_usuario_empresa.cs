using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_filha_negociacao_usuario_empresa")]
    public partial class itens_cotacao_filha_negociacao_usuario_empresa
    {
        public itens_cotacao_filha_negociacao_usuario_empresa()
        {
            this.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa = new List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_RESPOSTA_COTACAO { get; set; }

        [Required]
        public int ID_CLASSIFICACAO_TIPO_ITENS_COTACAO { get; set; }

        [Required]
        [MaxLength(100)]
        public string DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_EMPRESA { get; set; }

        [Required]
        public decimal PRECO_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public decimal QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public bool PRODUTO_COTADO_USUARIO_EMPRESA { get; set; }

        public bool ITEM_COTACAO_FILHA_EDITADO { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA")]
        public virtual cotacao_filha_usuario_empresa cotacao_filha_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA")]
        public virtual itens_cotacao_usuario_empresa itens_cotacao_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_RESPOSTA_COTACAO")]
        public virtual tipo_resposta_cotacao tipo_resposta_cotacao { get; set; }

        [ForeignKey("ID_CLASSIFICACAO_TIPO_ITENS_COTACAO")]
        public virtual classificacao_tipo_itens_cotacao classificacao_tipo_itens_cotacao { get; set; }

        public virtual ICollection<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa { get; set; }
    }
}
