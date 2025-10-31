using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_usuario_cotante")]
    public partial class itens_cotacao_usuario_cotante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE { get; set; }

        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [Required]
        public int ID_CODIGO_UNIDADE_PRODUTO { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }

        public int ID_CODIGO_PEDIDO_USUARIO_COTANTE { get; set; }

        public decimal QUANTIDADE_ITENS_COTACAO { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE")]
        public virtual cotacao_master_usuario_cotante cotacao_master_usuario_cotante { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servicos_empresa_profissional { get; set; }

        [ForeignKey("ID_CODIGO_UNIDADE_PRODUTO")]
        public virtual unidades_produtos unidades_produtos { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_FABRICANTE_MARCAS")]
        public virtual empresas_fabricantes_marcas empresas_fabricantes_marcas { get; set; }
    }
}
