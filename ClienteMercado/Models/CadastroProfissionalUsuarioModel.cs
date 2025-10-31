using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class CadastroProfissionalUsuario
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o CPF", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "* Dígitos insuficientes! Entre com o CPF completo.")]
        [Display(Name = "CPF: ")]
        public string CPF_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Nome do Profissional", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome: ")]
        public string NOME_PROFISSIONAL_USUARIO { get; set; }

        //[Required(ErrorMessage = "* É necessário indicar uma foto a ser exibida em seu perfil profissional", AllowEmptyStrings = false)]
        //[MaxLength(200)]
        public string FOTO_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Nome Comercial", AllowEmptyStrings = false)]
        [Display(Name = "Nome Comercial: ")]
        public string NOME_COMERCIAL_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o e-mail 1", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "* E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail 1: ")]
        public string EMAIL1_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o Login")]
        [MaxLength(30)]
        [Display(Name = "Login: ")]
        public string LOGIN_PROFISSIONAL_USUARIO_LOGINS { get; set; }

        [Required(ErrorMessage = "* Informe a Senha")]
        [MaxLength(20)]
        [Display(Name = "Senha: ")]
        public string SENHA_PROFISSIONAL_USUARIO_LOGINS { get; set; }

        [Required(ErrorMessage = "* Confirme a Senha")]
        [MaxLength(20)]
        [Display(Name = "Confirme Senha: ")]
        [System.ComponentModel.DataAnnotations.Compare("SENHA_PROFISSIONAL_USUARIO_LOGINS", ErrorMessage = "* Senha e Confirme Senha devem ser iguais.")]
        public string CONFIRMAR_SENHA_PROFISSIONAL_USUARIO_LOGINS { get; set; }

        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o País onde está localizado")]
        [MaxLength(15)]
        [Display(Name = "País: ")]
        public string PAIS_PROFISSIONAL_USUARIO { get; set; }
        public List<SelectListItem> ListagemPaises { get; set; }

        [Required(ErrorMessage = "* Selecione o Grupo de Atividade do Profissional")]
        [Display(Name = "Ramo de Atividades do Profissional:")]
        public int ID_GRUPO_ATIVIDADES { get; set; }
        public List<SelectListItem> ListagemGruposAtividadeEmpresa { get; set; }

        [Required(ErrorMessage = "* Selecione os Ramos de Atividade do Profissional")]
        [Display(Name = "Atividades Profissionais")]
        public int ID_RAMO_ATIVIDADE_PROFISSIONAL { get; set; }
        public string RAMOS_ATIVIDADES_SELECIONADOS { get; set; }
        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS { get; set; }

        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS_ORIGINAL { get; set; }

        [Display(Name = "Selecione as Atividades Profissionais: ")]
        [MaxLength(150)]
        public string DESCRICAO_RAMO_ATIVIDADE_PROFISSIONAL { get; set; }

        public string DESCRICAO_RAMO_ATIVIDADE_PROFISSIONAL_CONFORME_BANCO { get; set; }

        public int VALOR_PLANO_CONTRATADO { get; set; }

        [Required(ErrorMessage = "* Selecione um Plano de Assinatura")]
        [Display(Name = "Plano Assinatura")]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        public string DESCRICAO_TIPO_CONTRATO_COTADA { get; set; }

        [Required(ErrorMessage = "* Selecione uma Forma de Pagamento")]
        [Display(Name = "Modo de Pagamento")]
        public int ID_MEIO_PAGAMENTO { get; set; }

        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required(ErrorMessage = "* Entre com o Cep", AllowEmptyStrings = false)]
        [MaxLength(9), MinLength(9)]
        [Display(Name = "CEP: ")]
        public string CEP_SEQUENCIAL_ENDERECO { get; set; }

        [Required(ErrorMessage = "* Entre com o Endereço", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Endereço: ")]
        public string ENDERECO_PROFISSIONAL_USUARIO { get; set; }

        [MaxLength(100)]
        [Display(Name = "Complemento: ")]
        public string COMPLEMENTO_ENDERECO_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Estado", AllowEmptyStrings = false)]
        [MaxLength(4)]
        [Display(Name = "UF: ")]
        public string NOME_ESTADO_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com a Cidade", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Cidade: ")]
        public string NOME_CIDADE_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Bairro", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Bairro: ")]
        public string NOME_BAIRRO_PROFISSIONAL_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o Fone Celular 1", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [Display(Name = "Fone Celular 1: ")]
        public string TELEFONE1_PROFISSIONAL_USUARIO { get; set; }

        [MaxLength(15)]
        [Display(Name = "Telefone 2: ")]
        public string TELEFONE2_PROFISSIONAL_USUARIO { get; set; }

        public bool RECEBER_EMAILS_PROFISSIONAL_USUARIO { get; set; }
        public System.DateTime DATA_CADASTRO_PROFISSIONAL_USUARIO { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_PROFISSIONAL_USUARIO { get; set; }
        public bool ATIVA_INATIVO_PROFISSIONAL_USUARIO { get; set; }

        public bool USUARIO_MASTER { get; set; }

        //Armazena o tipo de login, que será cobrado nas actions posteriores
        public int TIPO_LOGIN { get; set; }
    }
}