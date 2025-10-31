namespace ClienteMercado.Models
{
    public class FotosProdutosAlternativos
    {
        public FotosProdutosAlternativos(int _id_foto_produto_alternativo, string _foto1_produto_alternativo, string _foto2_produto_alternativo, string _foto3_produto_alternativo,
            string _foto4_produto_alternativo, string _foto5_produto_alternativo)
        {
            id_foto_produto_alternativo = _id_foto_produto_alternativo;
            foto1_produto_alternativo = _foto1_produto_alternativo;
            foto2_produto_alternativo = _foto2_produto_alternativo;
            foto3_produto_alternativo = _foto3_produto_alternativo;
            foto4_produto_alternativo = _foto4_produto_alternativo;
            foto5_produto_alternativo = _foto5_produto_alternativo;
        }

        public int id_foto_produto_alternativo { get; set; }

        public string foto1_produto_alternativo { get; set; }

        public string foto2_produto_alternativo { get; set; }

        public string foto3_produto_alternativo { get; set; }

        public string foto4_produto_alternativo { get; set; }

        public string foto5_produto_alternativo { get; set; }
    }
}
