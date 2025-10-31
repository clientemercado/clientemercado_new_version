using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClienteMercado.Data.Entities
{
    [Table("central_de_compras")]
    public partial class central_de_compras
    {
        public central_de_compras()
        {
            this.empresas_participantes_central_de_compras = new List<empresas_participantes_central_de_compras>();
            this.cotacao_master_central_compras = new List<cotacao_master_central_compras>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS { get; set; }

        [Required]
        [MaxLength(50)]
        public string NOME_CENTRAL_COMPRAS { get; set; }

        [Required]
        public System.DateTime DATA_CRIACAO_CENTRAL_COMPRAS { get; set; }

        public System.DateTime DATA_ENCERRAMENTO_CENTRAL_COMPRAS { get; set; }

        [Required]
        public int ID_GRUPO_ATIVIDADES { get; set; }

        [ForeignKey("ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS")]
        public virtual empresa_usuario empresa_usuario { get; set; }

        [ForeignKey("ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS")]
        public virtual usuario_empresa usuario_empresa { get; set; }

        [ForeignKey("ID_GRUPO_ATIVIDADES")]
        public virtual grupo_atividades_empresa grupo_atividades_empresa { get; set; }

        public virtual ICollection<empresas_participantes_central_de_compras> empresas_participantes_central_de_compras { get; set; }

        public virtual ICollection<cotacao_master_central_compras> cotacao_master_central_compras { get; set; }
    }
}
