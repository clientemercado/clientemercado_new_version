using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("controle_sms_usuario_empresa")]
    public class controle_sms_usuario_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CONTROLE_SMS_USUARIO_EMPRESA { get; set; }

        [Required]
        //[MaxLength(15)]
        public string TELEFONE_DESTINO { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        public int MOTIVO_ENVIO { get; set; }

        [Required]
        public DateTime DATA_ENVIO { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }
    }
}
