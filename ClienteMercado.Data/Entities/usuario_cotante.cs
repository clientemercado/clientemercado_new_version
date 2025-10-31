using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("usuario_cotante")]
    public partial class usuario_cotante
    {
        public usuario_cotante()
        {
            this.usuario_cotante_logins = new List<usuario_cotante_logins>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_USUARIO_COTANTE { get; set; }

        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(11)]
        public string CPF_USUARIO_COTANTE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_USUARIO_COTANTE { get; set; }

        [MaxLength(30)]
        public string NICK_NAME_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [MaxLength(100)]
        public string COMPLEMENTO_ENDERECO_USUARIO_COTANTE { get; set; }

        [Required]
        [MaxLength(15)]
        public string PAIS_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required]
        [MaxLength(15)]
        public string TELEFONE1_USUARIO_COTANTE { get; set; }

        [MaxLength(15)]
        public string TELEFONE2_USUARIO_COTANTE { get; set; }

        public bool RECEBER_EMAILS_USUARIO_COTANTE { get; set; }
        public System.DateTime DATA_CADASTRO_USUARIO_COTANTE { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO_COTANTE { get; set; }
        public bool ATIVA_INATIVO_USUARIO_COTANTE { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_USUARIO_COTANTE { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }

        public bool CADASTRO_CONFIRMADO { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_EMPRESA_USUARIO")]
        public virtual tipo_empresa_usuario tipo_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_ENDERECO_EMPRESA_USUARIO")]
        public virtual enderecos_empresa_usuario enderecos_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSAO")]
        public virtual profissoes_usuario profissoes_usuario { get; set; }

        public virtual ICollection<usuario_cotante_logins> usuario_cotante_logins { get; set; }
    }
}
