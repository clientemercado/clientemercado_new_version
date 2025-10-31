using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("empresas_participantes_central_de_compras")]
    public partial class empresas_participantes_central_de_compras
    {
        public empresas_participantes_central_de_compras()
        {
            this.cotacao_individual_empresa_central_compras = new List<cotacao_individual_empresa_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EMPRESA_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA { get; set; }

        [Required]
        public System.DateTime DATA_ADESAO_EMPRESA_CENTRAL_COMPRAS { get; set; }

        public System.DateTime DATA_ENCERRAMENTO_PARTICIPACAO_CENTRAL_COMPRAS { get; set; }

        public System.DateTime DATA_CONVITE_CENTRAL_COMPRAS { get; set; }

        public bool CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS { get; set; }

        public int? ID_CODIGO_USUARIO_ACEITOU { get; set; }

        public int? ID_CODIGO_USUARIO_ENCERROU { get; set; }

        [ForeignKey("ID_CENTRAL_COMPRAS")]
        public virtual central_de_compras central_de_compras { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        public virtual ICollection<cotacao_individual_empresa_central_compras> cotacao_individual_empresa_central_compras { get; set; }
    }
}
