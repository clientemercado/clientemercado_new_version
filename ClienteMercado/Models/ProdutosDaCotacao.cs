namespace ClienteMercado.Models
{
    public class ProdutosDaCotacao
    {
        public ProdutosDaCotacao(int _id_codigo_cotacao_filha_negociacao, int _id_cotacao_filha, int _id_codigo_cotacao_master, int _id_codigo_item, string _nome_produto,
            string _marca_produto, string _empresa_cotada, string _quantidade_produto_exibicao, string _quantidade_real_produto, string _unidade_produto, string _valor_produto,
            string _produto_cotado, string _total_por_produto, decimal _percentual_desconto, string _total_por_produto_com_desconto, int _quantidade_imagens_anexadas,
            string _fotos_produtos_alternativos, string _menor_valor, decimal _total_produtos_sem_desconto, decimal _total_produtos_com_desconto, string _data_resposta,
            string _forma_pagamento, string _observacao, string _tipoFrete, decimal _valorFrete)
        {
            id_codigo_cotacao_filha_negociacao = _id_codigo_cotacao_filha_negociacao;
            id_cotacao_filha = _id_cotacao_filha;
            id_codigo_cotacao_master = _id_codigo_cotacao_master;
            id_codigo_item = _id_codigo_item;
            nome_produto = _nome_produto;
            marca_produto = _marca_produto;
            empresa_cotada = _empresa_cotada;
            quantidade_produto_exibicao = _quantidade_produto_exibicao;
            quantidade_real_produto = _quantidade_real_produto;
            unidade_produto = _unidade_produto;
            valor_produto = _valor_produto;
            produto_cotado = _produto_cotado;
            total_por_produto = _total_por_produto;
            percentual_desconto = _percentual_desconto;
            total_por_produto_com_desconto = _total_por_produto_com_desconto;
            quantidade_imagens_anexadas = _quantidade_imagens_anexadas;
            fotos_produtos_alternativos = _fotos_produtos_alternativos;
            menor_valor = _menor_valor;
            total_produtos_sem_desconto = _total_produtos_sem_desconto;
            total_produtos_com_desconto = _total_produtos_com_desconto;
            data_resposta = _data_resposta;
            forma_pagamento = _forma_pagamento;
            observacao = _observacao;
            tipoFrete = _tipoFrete;
            valorFrete = _valorFrete;
        }

        public int id_codigo_cotacao_filha_negociacao { get; set; }

        public int id_cotacao_filha { get; set; }

        public int id_codigo_cotacao_master { get; set; }

        public int id_codigo_item { get; set; }

        public string nome_produto { get; set; }

        public string marca_produto { get; set; }

        public string empresa_cotada { get; set; }

        public string quantidade_produto_exibicao { get; set; }

        public string quantidade_real_produto { get; set; }

        public string unidade_produto { get; set; }

        public string valor_produto { get; set; }

        public string produto_cotado { get; set; }

        public string total_por_produto { get; set; }

        public decimal percentual_desconto { get; set; }

        public string total_por_produto_com_desconto { get; set; }

        public int quantidade_imagens_anexadas { get; set; }

        public string fotos_produtos_alternativos { get; set; }

        public string menor_valor { get; set; }

        public decimal total_produtos_sem_desconto { get; set; }

        public decimal total_produtos_com_desconto { get; set; }

        public string data_resposta { get; set; }

        public string forma_pagamento { get; set; }

        public string observacao { get; set; }

        public string tipoFrete { get; set; }

        public decimal valorFrete { get; set; }
    }
}
