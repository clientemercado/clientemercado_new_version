using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("usuario_cotante_logins")]
    public partial class usuario_cotante_logins
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_USUARIO_COTANTE_LOGINS { get; set; }

        public int ID_CODIGO_USUARIO_COTANTE { get; set; }

        [Required]
        [MaxLength(30)]
        public string LOGIN_USUARIO_COTANTE_LOGINS { get; set; }

        [Required]
        [MaxLength(50)]
        public string SENHA_USUARIO_COTANTE_LOGINS { get; set; }

        [MaxLength(50)]
        public string LEMBRETE_USUARIO_COTANTE_LOGINS { get; set; }

        [Required]
        [MaxLength(50)]
        public string EMAIL1_USUARIO { get; set; }

        [MaxLength(50)]
        public string EMAIL2_USUARIO { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_COTANTE")]
        public virtual usuario_cotante usuario_cotante { get; set; }
    }
}
