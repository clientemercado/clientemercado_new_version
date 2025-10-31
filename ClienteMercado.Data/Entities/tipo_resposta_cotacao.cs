using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("tipo_resposta_cotacao")]
    public partial class tipo_resposta_cotacao
    {
        [Key]
        public int ID_CODIGO_TIPO_RESPOSTA_COTACAO { get; set; }

        [Required]
        [MaxLength(20)]
        public string DESCRICAO_TIPO_RESPOSTA_COTACAO { get; set; }
    }
}