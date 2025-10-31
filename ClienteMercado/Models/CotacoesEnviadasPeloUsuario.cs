namespace ClienteMercado.Models
{
    public class CotacoesEnviadasPeloUsuario
    {
        //Classe para montagem da lista de Cotações ENVIADAS pelo usuário
        public CotacoesEnviadasPeloUsuario(int _idCotacaoMaster, string _nomeDaCotacao, string _dataEnvioDaCotacao, string _dataEncerramentoDaCotacao, string _descricaoCategoria,
            int _numeroParticipantes, int _quantosFornedoresResponderam, string _descricaoStatus, bool _virouPedido)
        {
            idCotacaoMaster = _idCotacaoMaster;
            nomeDaCotacao = _nomeDaCotacao;
            dataEnvioDaCotacao = _dataEnvioDaCotacao;
            descricaoCategoria = _descricaoCategoria;
            numeroParticipantes = _numeroParticipantes;
            quantosFornedoresResponderam = _quantosFornedoresResponderam;
            descricaoStatus = _descricaoStatus;
        }

        public int idCotacaoMaster { get; set; }

        public string nomeDaCotacao { get; set; }

        public string dataEnvioDaCotacao { get; set; }

        public string dataEncerramentoDaCotacao { get; set; }

        public string descricaoCategoria { get; set; }

        public int numeroParticipantes { get; set; }

        public int quantosFornedoresResponderam { get; set; }

        public string descricaoStatus { get; set; }

        public bool virouPedido { get; set; }
    }
}
