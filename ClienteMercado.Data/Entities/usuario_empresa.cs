using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("usuario_empresa")]
    public partial class usuario_empresa
    {
        public usuario_empresa()
        {
            this.empresa_usuario_logins = new List<empresa_usuario_logins>();
            this.avaliacao_usuario_empresa_cotada = new List<avaliacao_usuario_empresa_cotada>();
            //this.cards_usuario_empresa = new List<cards_usuario_empresa>();
            //this.usuario_historico_empresas = new List<usuario_historico_empresas>();
            this.controle_sms_usuario_empresa = new List<controle_sms_usuario_empresa>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required]
        [MaxLength(11)]
        public string CPF_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_USUARIO { get; set; }

        [MaxLength(30)]
        public string NICK_NAME_USUARIO { get; set; }

        [MaxLength(100)]
        public string COMPLEMENTO_ENDERECO_USUARIO { get; set; }

        [Required]
        //[MaxLength(15)]
        public int PAIS_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(15)]
        public string TELEFONE1_USUARIO_EMPRESA { get; set; }

        [MaxLength(15)]
        public string TELEFONE2_USUARIO_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_USUARIO { get; set; }
        public System.DateTime DATA_CADASTRO_USUARIO { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_USUARIO { get; set; }
        public bool ATIVA_INATIVO_USUARIO { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_USUARIO { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }

        public bool CADASTRO_CONFIRMADO { get; set; }

        public int ID_CODIGO_EMPRESA { get; set; }

        public bool VER_COTACAO_AVULSA { get; set; }

        public bool USUARIO_MASTER { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_EMPRESA_USUARIO")]
        public virtual tipo_empresa_usuario tipo_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_ENDERECO_EMPRESA_USUARIO")]
        public virtual enderecos_empresa_usuario enderecos_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSAO")]
        public virtual profissoes_usuario profissoes_usuario { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        public virtual ICollection<empresa_usuario_logins> empresa_usuario_logins { get; set; }

        public virtual ICollection<cards_usuario_empresa> cards_usuario_empresa { get; set; }

        public virtual ICollection<avaliacao_usuario_empresa_cotada> avaliacao_usuario_empresa_cotada { get; set; }

        public virtual ICollection<usuario_historico_empresas> usuario_historico_empresas { get; set; }

        public virtual ICollection<controle_sms_usuario_empresa> controle_sms_usuario_empresa { get; set; }
    }
}
