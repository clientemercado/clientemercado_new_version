using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class PerfilUsuarioCotante
    {
        public string NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA { get; set; }
        public string DATA_ENCERRAMENTO_COTACAO_DIRECIONADA { get; set; }

        //Das Cotações Direcionadas
        [Required(ErrorMessage = "* Informe a Categoria em que deseja buscar preços")]
        public List<SelectListItem> ListagemCategoriaAtividadesACotarDirecionada { get; set; }
        public int ID_CATEGORIA_ATIVIDADES_ACOTAR_DIRECIONADA { get; set; }

        [MaxLength(1)]
        public string TIPO_COTACAO_PRODUTO_DIRECIONADA { get; set; }

        [MaxLength(1)]
        public string TIPO_COTACAO_SERVICO_DIRECIONADA { get; set; }

        [MaxLength(100)]
        public string ITEM_PRODUTO_COTACAO_DIRECIONADA { get; set; }
        public int ID_ITEM_PRODUTO_COTACAO_DIRECIONADA { get; set; }
        public string LISTA_IDS_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA { get; set; }

        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA { get; set; }
        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA_ORIGINAL { get; set; }
        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA_CONFORME_BANCO { get; set; }

        public string QUANTIDADE_ITEM_COTACAO_DIRECIONADA { get; set; }
        public string LISTA_QUANTIDADES_CADA_ITEM_COTACAO_DIRECIONADA { get; set; }

        public int ID_CODIGO_UNIDADE_PRODUTO_DIRECIONADA { get; set; }
        public List<SelectListItem> ListagemUnidadesProdutosACotarDirecionada { get; set; }
        public string LISTA_IDS_UNIDADES_MEDIDA_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA { get; set; }

        [MaxLength(50)]
        public string DESCRICAO_EMPRESA_FABRICANTE_MARCAS_DIRECIONADA { get; set; }
        public int ID_EMPRESA_FABRICANTE_MARCAS_DIRECIONADA { get; set; }
        public string LISTA_IDS_MARCAS_FABRICANTES_ITEM_COTACAO_DIRECIONADA { get; set; }
        public string LISTA_DESCRICAO_MARCAS_FABRICANTES_ITEM_COTACAO_DIRECIONADA { get; set; }

        public int TIPO_BUSCA_FORNECEDORES_POR_CIDADE_DIRECIONADA { get; set; }
        public int TIPO_BUSCA_FORNECEDORES_POR_UF_PAIS_DIRECIONADA { get; set; }

        public string NOME_OUTRA_CIDADE_ACOTAR_DIRECIONADA { get; set; }
        public List<SelectListItem> ListagemOutrasCidadesACotarDirecionada { get; set; }

        public string NOME_OUTRO_ESTADO_ACOTAR_DIRECIONADA { get; set; }
        public List<SelectListItem> ListagemOutrosEstadosACotarDirecionada { get; set; }

        public string NOME_OUTRA_CIDADE_OUTRO_ESTADO_ACOTAR_DIRECIONADA { get; set; }
        public List<SelectListItem> ListagemOutrasCidadesOutroEstadoACotarDirecionada { get; set; }

        public string LISTA_IDS_FORNECEDORES_ARECEBER_COTACAO_DIRECIONADA { get; set; }

        public string LISTA_IDS_VENDEDORES_FORNECEDORES_ARECEBER_COTACAO_DIRECIONADA { get; set; }

        //Das Cotações Avulsas
        public string NOME_COTACAO_USUARIO_COTANTE_AVULSA { get; set; }
        public string DATA_ENCERRAMENTO_COTACAO_AVULSA { get; set; }

        [Required(ErrorMessage = "* Informe a Categoria em que deseja buscar preços")]
        public int ID_CATEGORIA_ATIVIDADES_ACOTAR_AVULSA { get; set; }
        public List<SelectListItem> ListagemCategoriaAtividadesACotarAvulsa { get; set; }

        [MaxLength(1)]
        public string TIPO_COTACAO_PRODUTO_AVULSA { get; set; }

        [MaxLength(1)]
        public string TIPO_COTACAO_SERVICO_AVULSA { get; set; }

        [MaxLength(100)]
        public string ITEM_PRODUTO_COTACAO_AVULSA { get; set; }
        public int ID_ITEM_PRODUTO_COTACAO_AVULSA { get; set; }
        public string LISTA_IDS_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA { get; set; }

        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA { get; set; }
        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA_ORIGINAL { get; set; }
        public string DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA_CONFORME_BANCO { get; set; }

        public string QUANTIDADE_ITEM_COTACAO_AVULSA { get; set; }
        public string LISTA_QUANTIDADES_CADA_ITEM_COTACAO_AVULSA { get; set; }

        public int ID_CODIGO_UNIDADE_PRODUTO_AVULSA { get; set; }
        public List<SelectListItem> ListagemUnidadesProdutosACotarAvulsa { get; set; }
        public string LISTA_IDS_UNIDADES_MEDIDA_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA { get; set; }

        [MaxLength(50)]
        public string DESCRICAO_EMPRESA_FABRICANTE_MARCAS_AVULSA { get; set; }
        public int ID_EMPRESA_FABRICANTE_MARCAS_AVULSA { get; set; }
        public string LISTA_IDS_MARCAS_FABRICANTES_ITEM_COTACAO_AVULSA { get; set; }
        public string LISTA_DESCRICAO_MARCAS_FABRICANTES_ITEM_COTACAO_AVULSA { get; set; }

        public int TIPO_BUSCA_FORNECEDORES_POR_CIDADE_AVULSA { get; set; }
        public int TIPO_BUSCA_FORNECEDORES_POR_UF_PAIS_AVULSA { get; set; }

        public string NOME_OUTRA_CIDADE_ACOTAR_AVULSA { get; set; }
        public List<SelectListItem> ListagemOutrasCidadesACotarAvulsa { get; set; }

        public string NOME_OUTRO_ESTADO_ACOTAR_AVULSA { get; set; }
        public List<SelectListItem> ListagemOutrosEstadosACotarAvulsa { get; set; }

        public string NOME_OUTRA_CIDADE_OUTRO_ESTADO_ACOTAR_AVULSA { get; set; }
        public List<SelectListItem> ListagemOutrasCidadesOutroEstadoACotarAvulsa { get; set; }

        //public string LISTA_IDS_FORNECEDORES_ARECEBER_COTACAO_AVULSA { get; set; }

        //public string LISTA_IDS_VENDEDORES_FORNECEDORES_ARECEBER_COTACAO_AVULSA { get; set; }

        public int QUANTIDADE_FORNECEDORES_ARECEBER_COTACAO_AVULSA { get; set; }

        //Demais campos
        public int ID_CODIGO_EMPRESA_FABRICANTE_MARCAS { get; set; }
        public int QUANTIDADE_FORNECEDORES_APESQUISAR { get; set; }
        public string LISTA_IDS_COTACOES_SELECIONADADAS { get; set; }
        public List<CotacoesEnviadasPeloUsuario> listagemDasCotacoesDirecionadasEnviadasPeloUsuario { get; set; }
        public List<CotacoesEnviadasPeloUsuario> listagemDasCotacoesAvulsasEnviadasPeloUsuario { get; set; }

        //Ver campo para foto do Produto, caso queira idêntico
    }
}
