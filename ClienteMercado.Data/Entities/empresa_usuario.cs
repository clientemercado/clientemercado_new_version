using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresa_usuario")]
    public partial class empresa_usuario
    {
        public empresa_usuario()
        {
            this.usuario_empresa = new List<usuario_empresa>();
            this.avaliacao_empresa_cotada = new List<avaliacao_empresa_cotada>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_EMPRESA { get; set; }

        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        [MaxLength(15)]
        public string CNPJ_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(100)]
        public string RAZAO_SOCIAL_EMPRESA { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_FANTASIA_EMPRESA { get; set; }

        [MaxLength(100)]
        public string LOGOMARCA_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [MaxLength(100)]
        public string COMPLEMENTO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [Required]
        //[MaxLength(15)]
        public int PAIS_EMPRESA_USUARIO { get; set; }

        [MaxLength(50)]
        public string PAGINA_HOME_EMPRESA { get; set; }

        [MaxLength(50)]
        public string PAGINA_FUN_EMPRESA { get; set; }

        [Required]
        [MaxLength(15)]
        public string TELEFONE1_EMPRESA_USUARIO { get; set; }

        [MaxLength(15)]
        public string TELEFONE2_EMPRESA_USUARIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string EMAIL1_EMPRESA { get; set; }

        [MaxLength(50)]
        public string EMAIL2_EMPRESA { get; set; }

        public bool RECEBER_EMAILS_EMPRESA { get; set; }
        public bool ACEITACAO_TERMOS_POLITICAS { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        public DateTime DATA_CADASTRO_EMPRESA { get; set; }
        public DateTime DATA_ULTIMA_ATUALIZACAO_EMPRESA { get; set; }
        public bool ATIVA_INATIVA_EMPRESA { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_EMPRESA { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }
        public bool EMPRESA_ADMISTRADORA { get; set; }

        public int? ID_GRUPO_ATIVIDADES_ATACADO { get; set; }
        public int? ID_GRUPO_ATIVIDADES_VAREJO { get; set; }
        public Boolean? emp_adm_soft { get; set; }

        public DateTime? DATA_FINAL_GRATUIDADE_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_EMPRESA_USUARIO")]
        public virtual tipo_empresa_usuario tipo_empresa_usuario { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES_ATACADO")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa_atacado { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES_VAREJO")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa_varejo { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_CONTRATO_COTADA")]
        public virtual tipos_contratos_servicos tipos_contrato_servicos { get; set; }

        [ForeignKey("ID_CODIGO_ENDERECO_EMPRESA_USUARIO")]
        public virtual enderecos_empresa_usuario enderecos_empresa_usuario { get; set; }

        public virtual ICollection<usuario_empresa> usuario_empresa { get; set; }

        public virtual ICollection<atividades_empresa> atividades_empresa { get; set; }

        public virtual ICollection<cards_empresa> cards_empresa { get; set; }

        public virtual ICollection<avaliacao_empresa_cotada> avaliacao_empresa_cotada { get; set; }
    }
}
