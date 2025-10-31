namespace ClienteMercado.Models
{
    public class ChatEntreUsuarioEFornecedor
    {
        public ChatEntreUsuarioEFornecedor(int _id_cotacaoFilha, int _id_codigo_usuario_empresa_cotada, string _autor_dialogo, string _data_chat, string _texto_chat,
            int _ordem_exibicao)
        {
            id_cotacaoFilha = _id_cotacaoFilha;
            id_codigo_usuario_empresa_cotada = _id_codigo_usuario_empresa_cotada;
            autor_dialogo = _autor_dialogo;
            data_chat = _data_chat;
            texto_chat = _texto_chat;
            ordem_exibicao = _ordem_exibicao;
        }

        public int id_cotacaoFilha { get; set; }

        public int id_codigo_usuario_empresa_cotada { get; set; }

        public string autor_dialogo { get; set; }

        public string data_chat { get; set; }

        public string texto_chat { get; set; }

        public int ordem_exibicao { get; set; }
    }
}
