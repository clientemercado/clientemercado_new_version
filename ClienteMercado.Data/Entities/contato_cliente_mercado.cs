using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("contato_cliente_mercado")]
    public partial class contato_cliente_mercado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CONTATO_CLIENTE_MERCADO { get; set; }

        [Required]
        [MaxLength(50)]
        public string NOME_CONTATO_CLIENTE_MERCADO { get; set; }

        [Required]
        [MaxLength(50)]
        public string EMAIL_CONTATO_CLIENTE_MERCADO { get; set; }

        [Required]
        public int ASSUNTO_CONTATO_CLIENTE_MERCADO { get; set; }

        [Required]
        [MaxLength(5000)]
        public string MENSAGEM_CONTATO_CLIENTE_MERCADO { get; set; }

    }
}
