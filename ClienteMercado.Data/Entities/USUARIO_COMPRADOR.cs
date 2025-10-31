using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Data.Entities
{
    [Table("USUARIO_COMPRADOR")]
    public partial class USUARIO_COMPRADOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_usuario_comprador { get; set; }
        public string cpf_usuario_comprador { get; set; }
        public string nome_usuario_comprador { get; set; }
        public string endereco_usuario_comprador { get; set; }
        public string complemento_usuario_comprador { get; set; }
        public string bairro_usuario_comprador { get; set; }
        public string cep_usuario_comprador { get; set; }
        public string cidade_usuario_comprador { get; set; }
    }
}
