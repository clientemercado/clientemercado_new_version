using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Utils.ViewModel
{
    public class dadosProdutosImportacao
    {
        public string nome_produto { get; set; }
        public string codigo_barras_produto { get; set; }
        public string unidade_produto { get; set; }
        public Decimal preco_produto{ get; set; }
        public bool eh_promocao_produto { get; set; }
    }
}
