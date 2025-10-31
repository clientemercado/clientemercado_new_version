using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_filha_negociacao_central_compras")]
    public partial class itens_cotacao_filha_negociacao_central_compras
    {
        public itens_cotacao_filha_negociacao_central_compras()
        {
            this.fotos_itens_alternativos_cotacao_filha_negociacao_central_compras = new List<fotos_itens_alternativos_cotacao_filha_negociacao_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_RESPOSTA_COTACAO { get; set; }

        [Required]
        public int ID_CLASSIFICACAO_TIPO_ITENS_COTACAO { get; set; }

        [MaxLength(100)]
        public string DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public decimal QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public bool PRODUTO_COTADO_CENTRAL_COMPRAS { get; set; }

        public bool ITEM_COTACAO_FILHA_EDITADO { get; set; }

        [Required]
        public decimal PRECO_UNITARIO_ITENS_TABELA_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public decimal PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS { get; set; }

        public decimal PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS")]
        public virtual cotacao_filha_central_compras cotacao_filha_central_compras { get; set; }

        [ForeignKey("ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS")]
        public virtual itens_cotacao_individual_empresa_central_compras itens_cotacao_individual_empresa_central_compras { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_RESPOSTA_COTACAO")]
        public virtual tipo_resposta_cotacao tipo_resposta_cotacao { get; set; }

        [ForeignKey("ID_CLASSIFICACAO_TIPO_ITENS_COTACAO")]
        public virtual classificacao_tipo_itens_cotacao classificacao_tipo_itens_cotacao { get; set; }

        public virtual ICollection<fotos_itens_alternativos_cotacao_filha_negociacao_central_compras> fotos_itens_alternativos_cotacao_filha_negociacao_central_compras { get; set; }
    }
}
