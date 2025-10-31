namespace ClienteMercado.Models
{
    public class FornecedoresCotados
    {
        public FornecedoresCotados(int _id_Empresa, string _nome_empresa_fornecedor, string _cidade_empresa_fornecedor, string _estado_empresa_fornecedor,
            string _usuario_respondendo_empresa_fornecedor, int _id_Cotacao_Filha, string _tem_nao_tem_notificacao, string _respondido_empresa_fornecedor)
        {
            id_Empresa = _id_Empresa;
            nome_empresa_fornecedor = _nome_empresa_fornecedor;
            cidade_empresa_fornecedor = _cidade_empresa_fornecedor;
            estado_empresa_fornecedor = _estado_empresa_fornecedor;
            usuario_respondendo_empresa_fornecedor = _usuario_respondendo_empresa_fornecedor;
            id_Cotacao_Filha = _id_Cotacao_Filha;
            tem_nao_tem_notificacao = _tem_nao_tem_notificacao;
            respondido_empresa_fornecedor = _respondido_empresa_fornecedor;
        }

        public int id_Empresa { get; set; }

        public string nome_empresa_fornecedor { get; set; }

        public string cidade_empresa_fornecedor { get; set; }

        public string estado_empresa_fornecedor { get; set; }

        public string usuario_respondendo_empresa_fornecedor { get; set; }

        public int id_Cotacao_Filha { get; set; }

        public string tem_nao_tem_notificacao { get; set; }

        public string respondido_empresa_fornecedor { get; set; }
    }
}
