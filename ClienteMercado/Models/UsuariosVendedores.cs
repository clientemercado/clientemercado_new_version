namespace ClienteMercado.Models
{
    public class UsuariosVendedores
    {
        public UsuariosVendedores(int _id_codigo_usuario, int _id_codigo_empresa, string _nick_name_usuario, string _telefone1_usuario_empresa, string _telefone2_usuario_empresa)
        {
            ID_CODIGO_USUARIO = _id_codigo_usuario;
            ID_CODIGO_EMPRESA = _id_codigo_empresa;
            NICK_NAME_USUARIO = _nick_name_usuario;
            TELEFONE1_USUARIO_EMPRESA = _telefone1_usuario_empresa;
            TELEFONE2_USUARIO_EMPRESA = _telefone2_usuario_empresa;
        }

        public int ID_CODIGO_USUARIO { get; set; }

        public int ID_CODIGO_EMPRESA { get; set; }

        public string NICK_NAME_USUARIO { get; set; }

        public string TELEFONE1_USUARIO_EMPRESA { get; set; }

        public string TELEFONE2_USUARIO_EMPRESA { get; set; }
    }
}
