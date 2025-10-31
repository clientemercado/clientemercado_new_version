using System.ComponentModel.DataAnnotations;

namespace ClienteMercado.Models
{
    public class CadastroEmpresa
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "Entre com a CNPJ/CPF", AllowEmptyStrings = false)]
        [MaxLength(15)]
        public string CNPJ_CPF_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "Entre com a Razão Social", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Empresa (Rz. Social) / Usuário (Nome): ")]
        public string RAZAO_SOCIAL_EMPRESA { get; set; }

        [Required(ErrorMessage = "Entre com o Nome Fantasia", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome Fantasia: ")]
        public string NOME_FANTASIA_EMPRESA { get; set; }

        [MaxLength(100)]
        public string LOGOMARCA_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "Entre com o e-mail 1", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "E-Mail 1: ")]
        public string EMAIL1_EMPRESA { get; set; }

        [MaxLength(50)]
        [Display(Name = "E-Mail 2: ")]
        public string EMAIL2_EMPRESA { get; set; }

        [MaxLength(50)]
        [Display(Name = "Home Page: ")]
        public string PAGINA_HOME_EMPRESA { get; set; }

        [MaxLength(50)]
        [Display(Name = "Fun Page: ")]
        public string PAGINA_FUN_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_EMPRESA { get; set; }
    }
}