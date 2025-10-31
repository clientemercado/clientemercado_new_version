using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cotacao_individual_empresa_central_compras")]
    public partial class cotacao_individual_empresa_central_compras
    {
        public cotacao_individual_empresa_central_compras()
        {
            this.itens_cotacao_individual_empresa_central_compras = new List<itens_cotacao_individual_empresa_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_COTACAO_MASTER_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_EMPRESA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_EMPRESA_CRIOU_COTACAO { get; set; }

        public System.DateTime DATA_CRIACAO_COTACAO_INDIVIDUAL { get; set; }

        public bool COTACAO_INDIVIDUAL_ANEXADA { get; set; }

        public bool SOLICITAR_CONFIRMACAO_COTACAO { get; set; }

        public bool NEGOCIACAO_COTACAO_ACEITA { get; set; }

        public bool NEGOCIACAO_COTACAO_REJEITADA { get; set; }

        [ForeignKey("ID_COTACAO_MASTER_CENTRAL_COMPRAS")]
        public virtual cotacao_master_central_compras cotacao_master_central_de_compras { get; set; }

        [ForeignKey("ID_EMPRESA_CENTRAL_COMPRAS")]
        public virtual empresas_participantes_central_de_compras empresas_participantes_central_de_compras { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_EMPRESA_CRIOU_COTACAO")]
        public virtual usuario_empresa usuario_empresa_criou_cotacao { get; set; }

        public virtual ICollection<itens_cotacao_individual_empresa_central_compras> itens_cotacao_individual_empresa_central_compras { get; set; }
    }
}
