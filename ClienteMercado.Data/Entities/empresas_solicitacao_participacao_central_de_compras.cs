using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresas_solicitacao_participacao_central_de_compras")]
    public class empresas_solicitacao_participacao_central_de_compras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_SOLICITACAO_PARTICIPACAO { get; set; }

        [Required]
        public int ID_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_SOLICITANTE { get; set; }

        public bool SOLICITACAO_PARTICIPACAO_ACEITO { get; set; }

        public int? ID_CODIGO_USUARIO_ACEITOU_SOLICITACAO { get; set; }

        [ForeignKey("ID_CENTRAL_COMPRAS")]
        public virtual central_de_compras central_de_compras { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_SOLICITANTE")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_ACEITOU_SOLICITACAO")]
        public virtual usuario_empresa usuario_empresa { get; set; }
    }
}
