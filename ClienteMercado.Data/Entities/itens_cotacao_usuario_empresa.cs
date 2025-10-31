using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("itens_cotacao_usuario_empresa")]
    public partial class itens_cotacao_usuario_empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA { get; set; }

        [Required]
        public int ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS { get; set; }

        [Required]
        public int ID_CODIGO_UNIDADE_PRODUTO { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }

        public int ID_CODIGO_PEDIDO_USUARIO_EMPRESA { get; set; }

        [Required]
        public decimal QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA { get; set; }

        [ForeignKey("ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA")]
        public virtual cotacao_master_usuario_empresa cotacao_master_usuario_empresa { get; set; }

        [ForeignKey("ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS")]
        public virtual produtos_servicos_empresa_profissional produtos_servico_empresa_profissional { get; set; }

        [ForeignKey("ID_CODIGO_UNIDADE_PRODUTO")]
        public virtual unidades_produtos unidades_produtos { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_FABRICANTE_MARCAS")]
        public virtual empresas_fabricantes_marcas empresa_fabricantes_marcas { get; set; }
    }
}
