using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("profissional_usuario")]
    public partial class profissional_usuario
    {
        public profissional_usuario()
        {
            this.usuario_profissional = new List<usuario_profissional>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_EMPRESA_USUARIO { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        [MaxLength(11)]
        public string CPF_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        [MaxLength(200)]
        public string FOTO_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        [MaxLength(100)]
        public string NOME_COMERCIAL_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_ENDERECO_EMPRESA_USUARIO { get; set; }

        [MaxLength(100)]
        public string COMPLEMENTO_ENDERECO_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        [MaxLength(15)]
        public string PAIS_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_PROFISSAO { get; set; }

        [Required]
        [MaxLength(15)]
        public string TELEFONE1_PROFISSIONAL_USUARIO { get; set; }

        [MaxLength(15)]
        public string TELEFONE2_PROFISSIONAL_USUARIO { get; set; }

        public bool RECEBER_EMAILS_PROFISSIONAL_USUARIO { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_CONTRATO_COTADA { get; set; }

        public System.DateTime DATA_CADASTRO_PROFISSIONAL_USUARIO { get; set; }
        public System.DateTime DATA_ULTIMA_ATUALIZACAO_PROFISSIONAL_USUARIO { get; set; }
        public bool ATIVA_INATIVO_PROFISSIONAL_USUARIO { get; set; }
        public Nullable<System.DateTime> DATA_INATIVOU_PROFISSIONAL_USUARIO { get; set; }
        public Nullable<int> CODIGO_USUARIO_QUE_INATIVOU { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_EMPRESA_USUARIO")]
        public virtual tipo_empresa_usuario tipo_empresa_usuario { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        [ForeignKey("ID_CODIGO_ENDERECO_EMPRESA_USUARIO")]
        public virtual enderecos_empresa_usuario enderecos_empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_PROFISSAO")]
        public virtual profissoes_usuario profissoes_usuario { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_CONTRATO_COTADA")]
        public virtual tipos_contratos_servicos tipos_contrato_servicos { get; set; }

        public virtual ICollection<usuario_profissional> usuario_profissional { get; set; }

        public virtual ICollection<cards_usuario_profissional> cards_usuario_profissional { get; set; }

        public virtual ICollection<atividades_profissional> atividades_profissional { get; set; }
    }
}
