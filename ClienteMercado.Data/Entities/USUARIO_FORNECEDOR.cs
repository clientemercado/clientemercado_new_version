using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("USUARIO_FORNECEDOR")]
    public partial class USUARIO_FORNECEDOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_usuario_fornecedor { get; set; }
        public int id_empresa_fornecedor { get; set; }
        public string cpf_usuario_fornecedor { get; set; }
        public string nome_usuario_fornecedor { get; set; }
        public bool eh_master_usuario_fornecedor { get; set; }
        public bool usuario_ativo_usuario_fornecedor { get; set; }
        public DateTime data_inativacao_usuario_fornecedor { get; set; }
        public DateTime data_cadastro_empresa_fornecedor { get; set; }
        public string login_usuario_empresa_fornecedor { get; set; }
        public string passw_usuario_empresa_fornecedor { get; set; }
        public string email_usuario_empresa_fornecedor { get; set; }

        [ForeignKey("id_empresa_fornecedor")]
        public virtual EMPRESA_FORNECEDOR empresa_fornecedor { get; set; }
    }
}
