using ClienteMercado.Data.Entities;
using System;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class DadosEmpresaEUsuarioViewModel : empresa_usuario
    {
        public int cCC { get; set; }

        public int ID_CODIGO_USUARIO { get; set; }

        public int ID_CODIGO_PROFISSAO { get; set; }

        public string CPF_USUARIO_EMPRESA { get; set; }

        public string NOME_USUARIO { get; set; }

        public string NICK_NAME_USUARIO { get; set; }

        public string COMPLEMENTO_ENDERECO_USUARIO { get; set; }

        public string PAIS_USUARIO_EMPRESA { get; set; }

        public string TELEFONE1_USUARIO_EMPRESA { get; set; }

        public string TELEFONE2_USUARIO_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_USUARIO { get; set; }

        public DateTime DATA_CADASTRO_USUARIO { get; set; }

        public DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO { get; set; }

        public bool ATIVA_INATIVO_USUARIO { get; set; }

        public DateTime? DATA_INATIVOU_USUARIO { get; set; }

        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }

        public bool CADASTRO_CONFIRMADO { get; set; }

        public bool VER_COTACAO_AVULSA { get; set; }

        public bool USUARIO_MASTER { get; set; }
    }
}
