using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClienteMercado.Models
{
    public class Lembrete
    {
        public bool retornoEnvio { get; set; }

        [MaxLength(1)]
        [Display(Name = "Qual o tipo da sua conta? ")]
        public string TIPO_LOGIN { get; set; }

        [Required(ErrorMessage = "* Informe o e-mail armazenado no seu cadastro")]
        [StringLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail cadastrado: ")]
        public string EMAIL_ENVIO_SENHA { get; set; }
    }
}