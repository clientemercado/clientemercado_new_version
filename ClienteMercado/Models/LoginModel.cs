using System.ComponentModel.DataAnnotations;

namespace ClienteMercado.Models
{
    public class Login
    {
        //[Required(ErrorMessage = "* Marque o tipo de Login que deseja", AllowEmptyStrings = false)]
        [MaxLength(1)]
        [Display(Name = "Como deseja acessar sua conta? ")]
        public string TIPO_LOGIN { get; set; }

        [Required(ErrorMessage = "* Digite E-mail ou Login", AllowEmptyStrings = false)]
        [MaxLength(40)]
        [Display(Name = "E-mail ou Login: ")]
        public string LOGIN_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Digite a Senha", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [MaxLength(40)]
        [Display(Name = "Senha: ")]
        public string SENHA_EMPRESA_USUARIO { get; set; }
    }
}