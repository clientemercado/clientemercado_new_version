namespace ClienteMercado.Models
{
    public class FornecedoresASeremCotadosAvulsa
    {
        public FornecedoresASeremCotadosAvulsa(int _id_codigo_empresa, int _id_grupo_atividades, string _nome_fantasia_empresa, string _cidade_localizacao_empresa_fornecedor, string _estado_localizacao_empresa_fornecedor,
            string _logomarca_empresa_usuario, int _id_codigo_endereco_empresa_usuario, int _id_codigo_usuario_vendedor, string _nome_usuario_vendedor)
        {
            ID_CODIGO_EMPRESA = _id_codigo_empresa;
            ID_GRUPO_ATIVIDADES = _id_grupo_atividades;
            NOME_FANTASIA_EMPRESA = _nome_fantasia_empresa;
            CIDADE_LOCALIZACAO_EMPRESA_FORNECEDOR = _cidade_localizacao_empresa_fornecedor;
            ESTADO_LOCALIZACAO_EMPRESA_FORNECEDOR = _estado_localizacao_empresa_fornecedor;
            LOGOMARCA_EMPRESA_USUARIO = _logomarca_empresa_usuario;
            ID_CODIGO_ENDERECO_EMPRESA_USUARIO = _id_codigo_endereco_empresa_usuario;
            ID_CODIGO_USUARIO_VENDEDOR = _id_codigo_usuario_vendedor;
            NOME_USUARIO_VENDEDOR = _nome_usuario_vendedor;
        }

        public int ID_CODIGO_EMPRESA { get; set; }

        public int ID_GRUPO_ATIVIDADES { get; set; }

        public string NOME_FANTASIA_EMPRESA { get; set; }

        public string CIDADE_LOCALIZACAO_EMPRESA_FORNECEDOR { get; set; }

        public string ESTADO_LOCALIZACAO_EMPRESA_FORNECEDOR { get; set; }

        public string LOGOMARCA_EMPRESA_USUARIO { get; set; }

        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        public int ID_CODIGO_USUARIO_VENDEDOR { get; set; }

        public string NOME_USUARIO_VENDEDOR { get; set; }
    }
}
