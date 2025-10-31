using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class GerarFinanceiro
    {
        //Dados armazenamento cartão crédito
        [Required(ErrorMessage = "* Digite o nº do cartão", AllowEmptyStrings = false)]
        [MaxLength(16)]
        [Display(Name = "Número do cartão:")]
        public string DADOS_NUMERO_CARTAO { get; set; }

        [Required(ErrorMessage = "* Digite o nome do titular igual ao cartão", AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome no cartão: ")]
        public string DADOS_NOME_NO_CARTAO { get; set; }

        [Required(ErrorMessage = "* Selecione o Mês de vencimento", AllowEmptyStrings = false)]
        [MaxLength(2)]
        [Display(Name = "Vencimento:")]
        public string DADOS_MES_EXPIRACAO_CARTAO { get; set; }

        //public List<SelectListItem> ListagemMesesCobranca { get; set; }

        [Required(ErrorMessage = "* Selecione o Ano de vencimento", AllowEmptyStrings = false)]
        [MaxLength(4)]
        //[Display(Name = "Ano expiração")]
        public string DADOS_ANO_EXPIRACAO_CARTAO { get; set; }

        //public List<SelectListItem> ListagemAnosCobranca { get; set; }

        [Required(ErrorMessage = "* Digite o código de segurança", AllowEmptyStrings = false)]
        [MaxLength(3)]
        [Display(Name = "Cód. Segurança:")]
        public string DADOS_CODIGO_SEGURANCA { get; set; }

        [Display(Name = "Pagamento")]
        public int ID_MEIO_PAGAMENTO { get; set; }

        //Referente aos dados da Empresa, caso haja opção para que seja a pagadora da fatura
        public string NOME_EMPRESA_PAGADOR { get; set; }
        public string EMAIL_EMPRESA_PAGADOR { get; set; }
        public string CEP_EMPRESA_PAGADOR { get; set; }
        public string TELEFONE_EMPRESA_PAGADOR { get; set; }
        public string LOGRADOURO_EMPRESA_PAGADOR { get; set; }
        public string NUMERO_LOGRADOURO_EMPRESA_PAGADOR { get; set; }
        public string BAIRRO_EMPRESA_PAGADOR { get; set; }
        public string CIDADE_EMPRESA_PAGADOR { get; set; }
        public string ESTADO_EMPRESA_PAGADOR { get; set; }

        //Referente aos dados do Usuário Master, caso haja opção para que seja o pagador da fatura
        public string NOME_USUARIO_PAGADOR { get; set; }
        public string EMAIL_USUARIO_PAGADOR { get; set; }
        public string CEP_USUARIO_PAGADOR { get; set; }
        public string TELEFONE_USUARIO_PAGADOR { get; set; }
        public string LOGRADOURO_USUARIO_PAGADOR { get; set; }
        public string NUMERO_LOGRADOURO_USUARIO_PAGADOR { get; set; }
        public string BAIRRO_USUARIO_PAGADOR { get; set; }
        public string CIDADE_USUARIO_PAGADOR { get; set; }
        public string ESTADO_USUARIO_PAGADOR { get; set; }

        //Emissão da cobrança em Cartão ou Boleto (para Empresa ou Usuário)
        [Required(ErrorMessage = "* Selecione um titular para a cobrança", AllowEmptyStrings = false)]
        [Display(Name = "Emitir a cobrança para:")]
        public int RESPONSAVEL_EMISSAO_COBRANCA { get; set; }
        public List<SelectListItem> ListagemResponsaveisCobranca { get; set; }

        public int ANO_CORRENTE { get; set; }

    }
}