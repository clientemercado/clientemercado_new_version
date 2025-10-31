using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cotacao_master_usuario_cotante")]
    public partial class cotacao_master_usuario_cotante
    {
        public cotacao_master_usuario_cotante()
        {
            this.itens_cotacao_usuario_cotante = new List<itens_cotacao_usuario_cotante>();
            this.cotacao_filha_usuario_cotante = new List<cotacao_filha_usuario_cotante>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_STATUS_COTACAO { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_TIPO_COTACAO { get; set; }

        [Required]
        public int ID_TIPO_FRETE { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [MaxLength(30)]
        public string NOME_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public System.DateTime DATA_CRIACAO_COTACAO_USUARIO_COTANTE { get; set; }

        public System.DateTime DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE { get; set; }

        [MaxLength(30)]
        public string CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE { get; set; }

        [MaxLength(200)]
        public string OBSERVACAO_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public decimal PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE { get; set; }

        [ForeignKey("ID_CODIGO_STATUS_COTACAO")]
        public virtual status_cotacao status_cotacao { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_COTANTE")]
        public virtual usuario_cotante usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_TIPO_COTACAO")]
        public virtual tipos_cotacao tipos_cotacao { get; set; }

        [ForeignKey("ID_TIPO_FRETE")]
        public virtual tipos_frete tipos_frete { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        public virtual ICollection<itens_cotacao_usuario_cotante> itens_cotacao_usuario_cotante { get; set; }

        public virtual ICollection<cotacao_filha_usuario_cotante> cotacao_filha_usuario_cotante { get; set; }
    }
}
