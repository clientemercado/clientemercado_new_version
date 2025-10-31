using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class CadastroEmpresa
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o CNPJ", AllowEmptyStrings = false)]
        [MaxLength(18)]
        [StringLength(18, MinimumLength = 18)]
        [Display(Name = "CNPJ: ")]
        public string CNPJ_CPF_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com a Razão Social", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Razão Social: ")]
        public string RAZAO_SOCIAL_EMPRESA { get; set; }

        [Required(ErrorMessage = "* Entre com o Nome Fantasia", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome Fantasia: ")]
        public string NOME_FANTASIA_EMPRESA { get; set; }

        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o e-mail 1", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "* E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail 1: ")]
        public string EMAIL1_EMPRESA { get; set; }

        [MaxLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "* E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail 2: ")]
        public string EMAIL2_EMPRESA { get; set; }

        [Required(ErrorMessage = "* Informe o País onde está localizado")]
        [MaxLength(15)]
        [Display(Name = "País: ")]
        public string PAIS_EMPRESA_USUARIO { get; set; }
        public List<SelectListItem> ListagemPaises { get; set; }

        [Display(Name = "Ramo de Atividades da Empresa:")]
        public int ID_GRUPO_ATIVIDADES { get; set; }
        public List<SelectListItem> ListagemGruposAtividadeEmpresa { get; set; }

        [Display(Name = "Ramos de Atividade")]
        public int ID_RAMO_ATIVIDADE_EMPRESA { get; set; }
        public string RAMOS_ATIVIDADES_SELECIONADOS { get; set; }
        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS { get; set; }
        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS_ORIGINAL { get; set; }

        [Display(Name = "Selecione os Ramos de Atividade: ")]
        [MaxLength(150)]
        public string DESCRICAO_RAMO_ATIVIDADE_EMPRESA { get; set; }

        public string DESCRICAO_RAMO_ATIVIDADE_EMPRESA_CONFORME_BANCO { get; set; }

        [Display(Name = "Plano Assinatura")]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        public string DESCRICAO_TIPO_CONTRATO_COTADA { get; set; }

        public int VALOR_PLANO_CONTRATADO { get; set; }

        [Display(Name = "Modo de Pagamento")]
        public int ID_MEIO_PAGAMENTO { get; set; }

        [Required(ErrorMessage = "* Entre com o Cep", AllowEmptyStrings = false)]
        [MaxLength(9), MinLength(9)]
        [Display(Name = "CEP: ")]
        public string CEP_SEQUENCIAL_ENDERECO { get; set; }

        [Required(ErrorMessage = "* Entre com o Endereço", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Endereço: ")]
        public string ENDERECO_EMPRESA_USUARIO { get; set; }

        [MaxLength(100)]
        [Display(Name = "Complemento: ")]
        public string COMPLEMENTO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Estado", AllowEmptyStrings = false)]
        [MaxLength(4)]
        [Display(Name = "UF: ")]
        public string NOME_ESTADO_EMPRESA_USUARIO { get; set; }
        public List<SelectListItem> ListagemEstados { get; set; }

        [Required(ErrorMessage = "* Entre com a Cidade", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Cidade: ")]
        public string NOME_CIDADE_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Bairro", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Bairro: ")]
        public string NOME_BAIRRO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Telefone 1", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [Display(Name = "Telefone 1: ")]
        public string TELEFONE1_EMPRESA_USUARIO { get; set; }

        [MaxLength(15)]
        [Display(Name = "Telefone 2: ")]
        public string TELEFONE2_EMPRESA_USUARIO { get; set; }

        [MaxLength(50)]
        [Display(Name = "Home page: ")]
        public string PAGINA_HOME_EMPRESA { get; set; }

        [MaxLength(50)]
        [Display(Name = "Fun page: ")]
        public string PAGINA_FUN_EMPRESA { get; set; }

        [Display(Name = "Aceito receber e-mails do Cliente & Mercado.")]
        public bool RECEBER_EMAILS_EMPRESA { get; set; }

        public bool ACEITACAO_TERMOS_POLITICAS { get; set; }
        public System.DateTime DATA_CADASTRO_EMPRESA { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_EMPRESA { get; set; }
        public bool ATIVA_INATIVA_EMPRESA { get; set; }
        public bool EMPRESA_ADMISTRADORA { get; set; }

        public string NOME_USUARIO_MASTER { get; set; }
        public bool USUARIO_CONFIRMADO_EMPRESA_CONFIGURADA { get; set; }

        //Armazena o ID da empresa, em caso de o CNPJ já estiver cadastrado no banco para outra empresa
        public string ID_EMPRESA { get; set; }

        //Armazena o tipo de login, que será cobrado nas actions posteriores
        public int TIPO_LOGIN { get; set; }
    }
}