using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("controle_sms_usuario_profissional")]
    public class controle_sms_usuario_profissional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CONTROLE_SMS_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        //[MaxLength(15)]
        public string TELEFONE_DESTINO { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int MOTIVO_ENVIO { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_PROFISSIONAL")]
        public virtual usuario_profissional usuario_profissional { get; set; }
    }
}
