namespace ClienteMercado.Models
{
    public class CotacoesRecebidasPeloUsuario
    {
        //Classe para montagem da lista de Cotações RECEBIDAS pelo usuário
        public CotacoesRecebidasPeloUsuario(string _tipoCotacao, int _idCotacaoFilha, int _idCotacaoMaster, string _nomeDaCotacao, string _cotacaoRecebida, string _dataEnvioDaCotacao, string _dataEncerramentoDaCotacao,
            string _descricaoCategoria, int _numeroParticipantes, bool _cotacaoRespondida, int _quantosFornedoresResponderam, string _dataRespostaDaCotacao, string _statusDaCotacao, bool _venceuCotacao,
            string _virouPedido)
        {
            tipoCotacao = _tipoCotacao;
            idCotacaoFilha = _idCotacaoFilha;
            idCotacaoMaster = _idCotacaoMaster;
            nomeDaCotacao = _nomeDaCotacao;
            cotacaoRecebida = _cotacaoRecebida;
            dataEnvioDaCotacao = _dataEnvioDaCotacao;
            dataEncerramentoDaCotacao = _dataEncerramentoDaCotacao;
            descricaoCategoria = _descricaoCategoria;
            numeroParticipantes = _numeroParticipantes;
            cotacaoRespondida = _cotacaoRespondida;
            quantosFornedoresResponderam = _quantosFornedoresResponderam;
            dataRespostaDaCotacao = _dataRespostaDaCotacao;
            statusDaCotacao = _statusDaCotacao;
            venceuCotacao = _venceuCotacao;
            virouPedido = _virouPedido;
        }

        public int idCotacaoFilha { get; set; }

        public int idCotacaoMaster { get; set; }

        public string nomeDaCotacao { get; set; }

        public string cotacaoRecebida { get; set; }

        public string dataEnvioDaCotacao { get; set; }

        public string dataEncerramentoDaCotacao { get; set; }

        public string descricaoCategoria { get; set; }

        public int numeroParticipantes { get; set; }

        public bool cotacaoRespondida { get; set; }

        public int quantosFornedoresResponderam { get; set; }

        public string dataRespostaDaCotacao { get; set; }

        public string statusDaCotacao { get; set; }

        public bool venceuCotacao { get; set; }

        public string virouPedido { get; set; }

        public string tipoCotacao { get; set; }
    }
}
