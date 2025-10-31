using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("usuario_profissional")]
    public partial class usuario_profissional
    {
        public usuario_profissional()
        {
            this.profissional_usuario_logins = new List<profissional_usuario_logins>();
            this.controle_sms_usuario_profissional = new List<controle_sms_usuario_profissional>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_USUARIO_PROFISSIONAL { get; set; }

        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(11)]
        public string CPF_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [MaxLength(100)]
        public string COMPLEMENTO_ENDERECO_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        [MaxLength(15)]
        public string PAIS_USUARIO_PROFISSIONAL { get; set; }

        [Required]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required]
        [MaxLength(15)]
        public string TELEFONE1_USUARIO_PROFISSIONAL { get; set; }

        [MaxLength(15)]
        public string TELEFONE2_USUARIO_PROFISSIONAL { get; set; }

        public bool RECEBER_EMAILS_USUARIO_PROFISSIONAL { get; set; }
        public System.DateTime DATA_CADASTRO_USUARIO_PROFISSIONAL { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO_PROFISSIONAL { get; set; }
        public bool ATIVA_INATIVO_USUARIO_PROFISSIONAL { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_USUARIO_PROFISSIONAL { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }

        public bool CADASTRO_CONFIRMADO { get; set; }

        public int ID_CODIGO_PROFISSIONAL_USUARIO { get; set; }

        public bool USUARIO_MASTER { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_EMPRESA_USUARIO")]
        public virtual tipo_empresa_usuario tipo_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_ENDERECO_EMPRESA_USUARIO")]
        public virtual enderecos_empresa_usuario enderecos_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSAO")]
        public virtual profissoes_usuario profissoes_usuario { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSIONAL_USUARIO")]
        public virtual profissional_usuario profissional_usuario { get; set; }

        public virtual ICollection<profissional_usuario_logins> profissional_usuario_logins { get; set; }

        public virtual ICollection<controle_sms_usuario_profissional> controle_sms_usuario_profissional { get; set; }
    }
}
