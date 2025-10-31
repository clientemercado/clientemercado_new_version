using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{

    //[ComparaPropriedades("SENHA_EMPRESA_USUARIO_LOGINS", "CONFIRMAR_SENHA_EMPRESA_USUARIO_LOGINS", ErrorMessage = "A confirmação da senha não bate com a senha original")]

    public class CadastroUsuarioEmpresa
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o CPF", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "* Dígitos insuficientes! Entre com o CPF completo.")]
        [Display(Name = "CPF: ")]
        public string CPF_USUARIO_EMPRESA { get; set; }

        [Required(ErrorMessage = "* Entre com o Nome do Usuário", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome: ")]
        public string NOME_USUARIO { get; set; }

        [MaxLength(30)]
        [Display(Name = "Me chamem por: ")]
        public string NICK_NAME_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o e-mail 1", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail 1: ")]
        public string EMAIL1_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o Login")]
        [MaxLength(30)]
        [Display(Name = "Login: ")]
        public string LOGIN_EMPRESA_USUARIO_LOGINS { get; set; }

        [Required(ErrorMessage = "* Informe a Senha")]
        [MaxLength(20)]
        [Display(Name = "Senha: ")]
        public string SENHA_EMPRESA_USUARIO_LOGINS { get; set; }

        [Required(ErrorMessage = "* Confirme a Senha")]
        [MaxLength(20)]
        [Display(Name = "Confirme Senha: ")]
        [System.ComponentModel.DataAnnotations.Compare("SENHA_EMPRESA_USUARIO_LOGINS", ErrorMessage = "* Senha e Confirme Senha devem ser iguais.")]
        public string CONFIRMAR_SENHA_EMPRESA_USUARIO_LOGINS { get; set; }

        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o País onde está localizado")]
        [MaxLength(15)]
        [Display(Name = "País: ")]
        public string PAIS_USUARIO_EMPRESA { get; set; }
        public List<SelectListItem> ListagemPaises { get; set; }

        [Required(ErrorMessage = "* Selecione os Ramos de Atividade da Empresa")]
        [MaxLength(50)]
        [Display(Name = "Escolha dos Ramos de Atividade")]
        public string RAMOS_ATIVIDADES_EMPRESA { get; set; }

        [Required(ErrorMessage = "* Selecione um Plano de Assinatura")]
        [MaxLength(50)]
        [Display(Name = "Escolha do Plano Assinatura")]
        public string PLANOS_DE_ASSINATURA { get; set; }
        public List<SelectListItem> ListagemPlanosAssinatura { get; set; }

        [Required(ErrorMessage = "* Selecione uma Forma de Pagamento")]
        [MaxLength(50)]
        [Display(Name = "Modo de Pagamento")]
        public string MEIOS_DE_PAGAMENTO { get; set; }
        public List<SelectListItem> ListagemMeiosPagamento { get; set; }

        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required(ErrorMessage = "* Entre com o Cep", AllowEmptyStrings = false)]
        [MaxLength(9), MinLength(9)]
        [Display(Name = "CEP: ")]
        public string CEP_SEQUENCIAL_ENDERECO { get; set; }

        [Required(ErrorMessage = "* Entre com o Endereço", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Endereço: ")]
        public string ENDERECO_USUARIO_EMPRESA { get; set; }

        [MaxLength(100)]
        [Display(Name = "Complemento: ")]
        public string COMPLEMENTO_ENDERECO_USUARIO { get; set; }

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

        [Required(ErrorMessage = "* Entre com o Fone Celular 1", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [Display(Name = "Fone Celular 1: ")]
        public string TELEFONE1_USUARIO_EMPRESA { get; set; }

        [MaxLength(15)]
        [Display(Name = "Telefone 2: ")]
        public string TELEFONE2_USUARIO_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_USUARIO { get; set; }
        public System.DateTime DATA_CADASTRO_USUARIO { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO { get; set; }
        public bool ATIVA_INATIVO_USUARIO { get; set; }
        public int ID_CODIGO_EMPRESA { get; set; }
        public bool USUARIO_MASTER { get; set; }

        //Armazena o ID da empresa, em caso de o CNPJ já estiver cadastrado no banco para outra empresa (Vem do Cadastro de empresa. Receberá uma Session)
        public int ID_EMPRESA { get; set; }

    }

}