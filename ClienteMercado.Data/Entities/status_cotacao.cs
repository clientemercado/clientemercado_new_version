using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("status_cotacao")]
    public partial class status_cotacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_STATUS_COTACAO { get; set; }

        [Required]
        [MaxLength(30)]
        public string DESCRICAO_STATUS_COTACAO { get; set; }

    }
}
