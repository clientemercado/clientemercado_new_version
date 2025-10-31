using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresa_usuario_logins")]
    public partial class empresa_usuario_logins
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_EMPRESA_USUARIO_LOGINS { get; set; }

        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        [MaxLength(30)]
        public string LOGIN_EMPRESA_USUARIO_LOGINS { get; set; }

        [Required]
        [MaxLength(50)]
        public string SENHA_EMPRESA_USUARIO_LOGINS { get; set; }

        [MaxLength(50)]
        public string LEMBRETE_EMPRESA_USUARIO_LOGINS { get; set; }

        [Required]
        [MaxLength(50)]
        public string EMAIL1_USUARIO { get; set; }

        [MaxLength(50)]
        public string EMAIL2_USUARIO { get; set; }

        //[MaxLength(50)]
        //public string PAGINA_HOME_USUARIO { get; set; }

        //[MaxLength(50)]
        //public string PAGINA_FUN_USUARIO { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }
    }
}
