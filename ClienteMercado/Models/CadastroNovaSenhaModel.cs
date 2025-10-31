using System.ComponentModel.DataAnnotations;

namespace ClienteMercado.Models
{
    public class CadastroNovaSenha
    {
        public int ID_CODIGO_USUARIO_LOGADO { get; set; }

        [Required(ErrorMessage = "* Informe a nova Senha")]
        [MaxLength(20)]
        [Display(Name = "Digite nova Senha: ")]
        public string SENHA_EMPRESA_USUARIO_LOGINS { get; set; }

        [Required(ErrorMessage = "* Confirme a nova Senha")]
        [MaxLength(20)]
        [Display(Name = "Confirme nova Senha: ")]
        [System.ComponentModel.DataAnnotations.Compare("SENHA_EMPRESA_USUARIO_LOGINS", ErrorMessage = "* Senha e Confirme Senha devem ser iguais.")]
        public string CONFIRMAR_SENHA_EMPRESA_USUARIO_LOGINS { get; set; }

        //Armazena o tipo de login, que será cobrado nas actions posteriores
        public int TIPO_LOGIN { get; set; }
    }
}