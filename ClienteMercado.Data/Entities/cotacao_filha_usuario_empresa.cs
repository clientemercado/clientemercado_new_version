using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("cotacao_filha_usuario_empresa")]
    public partial class cotacao_filha_usuario_empresa
    {
        public cotacao_filha_usuario_empresa()
        {
            this.itens_cotacao_negociacao_usuario_empresa = new List<itens_cotacao_filha_negociacao_usuario_empresa>();
            this.chat_cotacao_usuario_empresa = new List<chat_cotacao_usuario_empresa>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO { get; set; }

        [Required]
        public int ID_TIPO_FRETE { get; set; }

        [Required]
        public bool RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        [Required]
        public System.DateTime DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        [Required]
        [MaxLength(50)]
        public string FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA { get; set; }

        public int TIPO_DESCONTO { get; set; }

        public decimal PERCENTUAL_DESCONTO { get; set; }

        [Required]
        public decimal PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [MaxLength(300)]
        public string OBSERVACAO_COTACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public bool COTACAO_FILHA_USUARIO_EMPRESA_EDITADA { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA")]
        public virtual cotacao_master_usuario_empresa cotacao_master_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO")]
        public virtual usuario_empresa usuario_empresa { get; set; }

        [ForeignKey("ID_TIPO_FRETE")]
        public virtual tipos_frete tipo_frete { get; set; }

        public virtual ICollection<itens_cotacao_filha_negociacao_usuario_empresa> itens_cotacao_negociacao_usuario_empresa { get; set; }

        public virtual ICollection<chat_cotacao_usuario_empresa> chat_cotacao_usuario_empresa { get; set; }
    }
}
