using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("usuario_historico_empresas")]
    public partial class usuario_historico_empresas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_HISTORICO_USUARIOS_EMPRESAS { get; set; }

        public int ID_CODIGO_USUARIO { get; set; }

        public System.DateTime DATA_INICIO_SISTEMA_USUARIO_EMPRESA { get; set; }
        public System.DateTime DATA_FIM_SISTEMA_USUARIO_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }
    }
}
