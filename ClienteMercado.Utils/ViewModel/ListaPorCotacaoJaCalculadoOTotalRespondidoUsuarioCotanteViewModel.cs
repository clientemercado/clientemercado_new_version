namespace ClienteMercado.Utils.ViewModel
{
    public class ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel
    {
        public ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel(int _id_cotacao_filha, string _empresa_que_respondeu, string _data_resposta, string _cotou_total_parcial,
            decimal _valor_total_cotacao_sem_desconto, decimal _percentual_desconto, string _tipo_desconto, decimal _valor_desconto, decimal _valor_total_cotacao_com_desconto,
            string _virou_pedido, string _menor_valor)
        {
            ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = _id_cotacao_filha;
            EMPRESA_QUE_RESPONDEU = _empresa_que_respondeu;
            DATA_RESPOSTA = _data_resposta;
            COTOU_TOTAL_PARCIAL = _cotou_total_parcial;
            VALOR_TOTAL_COTACAO_SEM_DESCONTO = _valor_total_cotacao_sem_desconto;
            PERCENTUAL_DESCONTO = _percentual_desconto;
            TIPO_DESCONTO = _tipo_desconto;
            VALOR_DESCONTO = _valor_desconto;
            VALOR_TOTAL_COTACAO_COM_DESCONTO = _valor_total_cotacao_com_desconto;
            VIROU_PEDIDO = _virou_pedido;
            menor_valor = _menor_valor;
        }

        public int ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        public string EMPRESA_QUE_RESPONDEU { get; set; }

        public string DATA_RESPOSTA { get; set; }

        public string COTOU_TOTAL_PARCIAL { get; set; }

        public decimal VALOR_TOTAL_COTACAO_SEM_DESCONTO { get; set; }

        public decimal PERCENTUAL_DESCONTO { get; set; }

        public string TIPO_DESCONTO { get; set; }

        public decimal VALOR_DESCONTO { get; set; }

        public decimal VALOR_TOTAL_COTACAO_COM_DESCONTO { get; set; }

        public string VIROU_PEDIDO { get; set; }

        public string menor_valor { get; set; }
    }
}
