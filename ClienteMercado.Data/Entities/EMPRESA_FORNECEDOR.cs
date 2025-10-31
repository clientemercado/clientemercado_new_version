using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("EMPRESA_FORNECEDOR")]
    public partial class EMPRESA_FORNECEDOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_empresa_fornecedor { get; set; }
        public string cnpj_empresa_fornecedor { get; set; }
        public string nome_fantasia_empresa_fornecedor { get; set; }
        public DateTime data_cadastro_empresa_fornecedor { get; set; }
        public bool empresa_ativa_empresa_fornecedor { get; set; }
        public DateTime data_inativacao_empresa_fornecedor { get; set; }
        public string endereco_empresa_fornecedor { get; set; }
        public string complemento_empresa_fornecedor { get; set; }
        public string cep_empresa_fornecedor { get; set; }
        public string cidade_empresa_fornecedor { get; set; }
        public string uf_empresa_fornecedor { get; set; }
        public string bairro_empresa_fornecedor { get; set; }
        public string ramo_atividade_empresa_fornecedor { get; set; }
        public Boolean? emp_adm_soft { get; set; }
    }
}
