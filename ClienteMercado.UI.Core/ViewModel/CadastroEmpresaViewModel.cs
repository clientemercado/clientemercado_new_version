using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class CadastroEmpresaViewModel
    {
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        public string CNPJ_CPF_EMPRESA_USUARIO { get; set; }

        public string RAZAO_SOCIAL_EMPRESA { get; set; }

        public string NOME_FANTASIA_EMPRESA { get; set; }

        public string NOME_USUARIO { get; set; }

        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        public string EMAIL1_EMPRESA { get; set; }

        public string EMAIL2_EMPRESA { get; set; }

        public string LOGIN_EMPRESA_USUARIO { get; set; }

        public string SENHA_EMPRESA_USUARIO { get; set; }

        public int PAIS_EMPRESA_USUARIO { get; set; }
        public List<SelectListItem> ListagemPaises { get; set; }

        public List<SelectListItem> ListagemGruposAtividadeEmpresa { get; set; }

        public int ID_RAMO_ATIVIDADE_EMPRESA { get; set; }
        public string RAMOS_ATIVIDADES_SELECIONADOS { get; set; }
        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS { get; set; }
        public string DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS_ORIGINAL { get; set; }

        public string DESCRICAO_RAMO_ATIVIDADE_EMPRESA { get; set; }

        public string DESCRICAO_RAMO_ATIVIDADE_EMPRESA_CONFORME_BANCO { get; set; }

        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        public string DESCRICAO_TIPO_CONTRATO_COTADA { get; set; }

        public int VALOR_PLANO_CONTRATADO { get; set; }

        public int ID_MEIO_PAGAMENTO { get; set; }

        public string CEP_SEQUENCIAL_ENDERECO { get; set; }

        public string ENDERECO_EMPRESA_USUARIO { get; set; }

        public string COMPLEMENTO_ENDERECO_EMPRESA_USUARIO { get; set; }

        public string NOME_ESTADO_EMPRESA_USUARIO { get; set; }

        public int ID_ESTADOS_EMPRESA_USUARIO { get; set; }

        public List<SelectListItem> ListagemEstados { get; set; }

        public string NOME_CIDADE_EMPRESA_USUARIO { get; set; }

        public string NOME_BAIRRO_EMPRESA_USUARIO { get; set; }

        public string TELEFONE1_EMPRESA_USUARIO { get; set; }

        public string TELEFONE2_EMPRESA_USUARIO { get; set; }

        public string PAGINA_HOME_EMPRESA { get; set; }

        public string PAGINA_FUN_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_EMPRESA { get; set; }

        public bool ACEITACAO_TERMOS_POLITICAS { get; set; }
        public System.DateTime DATA_CADASTRO_EMPRESA { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_EMPRESA { get; set; }
        public bool ATIVA_INATIVA_EMPRESA { get; set; }
        public bool EMPRESA_ADMISTRADORA { get; set; }

        public string CPF_USUARIO { get; set; }

        public string NOME_USUARIO_MASTER { get; set; }

        public bool USUARIO_CONFIRMADO_EMPRESA_CONFIGURADA { get; set; }

        public string APELIDO_USUARIO { get; set; }

        public string EMAIL1_USUARIO { get; set; }

        public string EMAIL2_USUARIO { get; set; }

        public string TELEFONE1_USUARIO_EMPRESA { get; set; }

        public string TELEFONE2_USUARIO_EMPRESA { get; set; }

        //Armazena o ID da empresa, em caso de o CNPJ já estiver cadastrado no banco para outra empresa
        public string ID_EMPRESA { get; set; }

        //Armazena o tipo de login, que será cobrado nas actions posteriores
        public int TIPO_LOGIN { get; set; }

        public List<SelectListItem> ListagemRamosComercioAtacadista { get; set; }

        public List<SelectListItem> ListagemRamosComercioVarejista { get; set; }

        public int? ID_GRUPO_ATIVIDADES_ATACADO { get; set; }

        public int? ID_GRUPO_ATIVIDADES_VAREJO { get; set; }
    }
}
