using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cotacao_master_central_compras")]
    public class cotacao_master_central_compras
    {
        public cotacao_master_central_compras()
        {
            this.cotacao_individual_empresa_central_compras = new List<cotacao_individual_empresa_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_COTACAO_MASTER_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_COTACAO { get; set; }

        [Required]
        public int ID_CODIGO_STATUS_COTACAO { get; set; }

        [Required]
        public int ID_TIPO_FRETE { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [Required]
        [MaxLength(30)]
        public string NOME_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public System.DateTime DATA_CRIACAO_COTACAO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public System.DateTime DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS { get; set; }

        public System.DateTime DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS { get; set; }

        [MaxLength(30)]
        public string CONDICAO_PAGAMENTO_COTACAO { get; set; }

        [MaxLength(200)]
        public string OBSERVACAO_COTACAO { get; set; }

        [Required]
        public decimal PERCENTUAL_RESPONDIDA_COTACAO { get; set; }

        public bool COTACAO_ENVIADA_FORNECEDORES { get; set; }

        public int? ID_EMPRESA_FORNECEDORA_APROVACAO { get; set; }

        public bool SOLICITAR_CONFIRMACAO_COTACAO { get; set; }

        public bool NEGOCIACAO_CONTRA_PROPOSTA { get; set; }

        public bool NEGOCIACAO_COTACAO_ACEITA { get; set; }

        public int? ID_EMPRESA_FORNECEDORA_APROVADA { get; set; }

        [ForeignKey("ID_CENTRAL_COMPRAS")]
        public virtual central_de_compras central_de_compras { get; set; }

        [ForeignKey("ID_CODIGO_STATUS_COTACAO")]
        public virtual status_cotacao status_cotacao { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_COTACAO")]
        public virtual tipos_cotacao tipos_cotacao { get; set; }

        [ForeignKey("ID_TIPO_FRETE")]
        public virtual tipos_frete tipos_frete { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        public virtual ICollection<cotacao_individual_empresa_central_compras> cotacao_individual_empresa_central_compras { get; set; }
    }
}
