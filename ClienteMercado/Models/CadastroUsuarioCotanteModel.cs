using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class CadastroUsuarioCotante
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Entre com o CPF", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "* Dígitos insuficientes! Entre com o CPF completo.")]
        [Display(Name = "CPF: ")]
        public string CPF_USUARIO_COTANTE { get; set; }

        [Required(ErrorMessage = "* Entre com o Nome do Usuário", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome: ")]
        public string NOME_USUARIO_COTANTE { get; set; }

        [MaxLength(30)]
        [Display(Name = "Me chamem por: ")]
        public string NICK_NAME_USUARIO_COTANTE { get; set; }

        [Required(ErrorMessage = "* Entre com o e-mail 1", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail 1: ")]
        public string EMAIL1_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o Login")]
        [MaxLength(30)]
        [Display(Name = "Login: ")]
        public string LOGIN_USUARIO_COTANTE_LOGINS { get; set; }

        [Required(ErrorMessage = "* Informe a Senha")]
        [MaxLength(20)]
        [Display(Name = "Senha: ")]
        public string SENHA_USUARIO_COTANTE_LOGINS { get; set; }

        [Required(ErrorMessage = "* Confirme a Senha")]
        [MaxLength(20)]
        [Display(Name = "Confirme Senha: ")]
        [System.ComponentModel.DataAnnotations.Compare("SENHA_USUARIO_COTANTE_LOGINS", ErrorMessage = "* Senha e Confirme Senha devem ser iguais.")]
        public string CONFIRMAR_SENHA_USUARIO_COTANTE_LOGINS { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required(ErrorMessage = "* Informe o País onde está localizado")]
        [MaxLength(15)]
        [Display(Name = "País: ")]
        public string PAIS_USUARIO_COTANTE { get; set; }
        public List<SelectListItem> ListagemPaises { get; set; }

        [Required]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required(ErrorMessage = "* Entre com o Cep", AllowEmptyStrings = false)]
        [MaxLength(9), MinLength(9)]
        [Display(Name = "CEP: ")]
        public string CEP_SEQUENCIAL_ENDERECO { get; set; }

        [Required(ErrorMessage = "* Entre com o Endereço", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Endereço: ")]
        public string ENDERECO_USUARIO_COTANTE { get; set; }

        [MaxLength(100)]
        [Display(Name = "Complemento: ")]
        public string COMPLEMENTO_ENDERECO_USUARIO_COTANTE { get; set; }

        [Required(ErrorMessage = "* Entre com o Estado", AllowEmptyStrings = false)]
        [MaxLength(4)]
        [Display(Name = "UF: ")]
        public string NOME_ESTADO_USUARIO_COTANTE { get; set; }
        public List<SelectListItem> ListagemEstados { get; set; }

        [Required(ErrorMessage = "* Entre com a Cidade", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Cidade: ")]
        public string NOME_CIDADE_USUARIO_COTANTE { get; set; }

        [Required(ErrorMessage = "* Entre com o Bairro", AllowEmptyStrings = false)]
        [MaxLength(50)]
        [Display(Name = "Bairro: ")]
        public string NOME_BAIRRO_USUARIO_COTANTE { get; set; }

        [Required(ErrorMessage = "* Entre com o Fone Celular 1", AllowEmptyStrings = false)]
        [MaxLength(15)]
        [Display(Name = "Fone Celular 1: ")]
        public string TELEFONE1_USUARIO_COTANTE { get; set; }

        [MaxLength(15)]
        [Display(Name = "Telefone 2: ")]
        public string TELEFONE2_USUARIO_COTANTE { get; set; }

        public bool RECEBER_EMAILS_USUARIO_COTANTE { get; set; }
        public System.DateTime DATA_CADASTRO_USUARIO_COTANTE { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO_COTANTE { get; set; }
        public bool ATIVA_INATIVO_USUARIO_COTANTE { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_USUARIO_COTANTE { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }
    }
}