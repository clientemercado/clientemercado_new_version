using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("classificacao_tipo_itens_cotacao")]
    public partial class classificacao_tipo_itens_cotacao
    {
        [Key]
        public int ID_CLASSIFICACAO_TIPO_ITENS_COTACAO { get; set; }

        [Required]
        [MaxLength(30)]
        public string DESCRICAO_CLASSIFICACAO_TIPO_ITENS_COTACAO { get; set; }
    }
}
