using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NFornecedoresService
    {
        DFornecedoresRepository dfornecedores = new DFornecedoresRepository();

        //Busca Fornecedores na MINHA CIDADE e no MEU ESTADO (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresNaMinhaCidadeENoMeuEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresNaMinhaCidadeENoMeuEstadoDirecionada(categoriaASerCotada, quantFornecedores, idCidadeCotante, idEstadoCotante, idPaisCotante);
        }

        //Busca Fornecedores em OUTRA CIDADE no meu ESTADO (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmOutraCidadeENoMeuEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idOutraCidade, int idEstadoCotante, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmOutraCidadeENoMeuEstadoDirecionada(categoriaASerCotada, quantFornecedores, idOutraCidade, idEstadoCotante, idPaisCotante);
        }

        //Buscar Fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA) (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmOutroEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idOutroEstado, int idOutraCidadeOutroEstado, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmOutroEstadoDirecionada(categoriaASerCotada, quantFornecedores, idOutroEstado, idOutraCidadeOutroEstado, idPaisCotante);
        }

        //Buscar Fornecedores em TODO PAÍS (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmTodoOPaisDirecionada(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmTodoOPaisDirecionada(categoriaASerCotada, quantFornecedores, idPaisCotante);
        }

        ////Buscar Fornecedores em OUTROS PAÍSES  (Direcionada - Futuramente)
        //public List<empresa_usuario> BuscarFornecedoresEmOutrosPaises(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        //{
        //    DFornecedores dfornecedores = new DFornecedores();

        //    List<empresa_usuario> listaFornecedores = dfornecedores.BuscarFornecedoresEmOutrosPaises(categoriaASerCotada, quantFornecedores, idPaisCotante);

        //    return listaFornecedores;
        //}

        //Busca Fornecedores na MINHA CIDADE e no MEU ESTADO (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresNaMinhaCidadeENoMeuEstadoAvulsa(int categoriaASerCotada, int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresNaMinhaCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idCidadeCotante, idEstadoCotante, idPaisCotante);
        }

        //Busca Fornecedores em OUTRA CIDADE no meu ESTADO (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresEmOutraCidadeENoMeuEstadoAvulsa(int categoriaASerCotada, int idOutraCidade, int idEstadoCotante, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmOutraCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idOutraCidade, idEstadoCotante, idPaisCotante);
        }

        //Buscar Fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA) (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresEmOutroEstadoAvulsa(int categoriaASerCotada, int idOutroEstado, int idOutraCidadeOutroEstado, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmOutroEstadoAvulsa(categoriaASerCotada, idOutroEstado, idOutraCidadeOutroEstado, idPaisCotante);
        }

        //Buscar fornecedores de TODO o PAÍS
        public List<empresa_usuario> BuscarFornecedoresEmTodoOPaisAvulsa(int categoriaASerCotada, int idPaisCotante)
        {
            return dfornecedores.BuscarFornecedoresEmTodoOPaisAvulsa(categoriaASerCotada, idPaisCotante);
        }

        //CARREGA LISTA de FORNECEDORES conforme LOCAL SELECIONADO
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDePossiveisFornecedorasParaACentralDeCompras(int codRamoAtividade, int quantosFornecedores, int localBusca,
            int uFSelecionada, int codEAdmDaCC, int cCC)
        {
            List<ListaEstilizadaDeEmpresasViewModel> listaPossiveisFornecedores =
                dfornecedores.BuscarListaDePossiveisFornecedorasParaACentralDeCompras(codRamoAtividade, quantosFornecedores, localBusca, uFSelecionada, codEAdmDaCC, cCC);

            for (int i = 0; i < listaPossiveisFornecedores.Count; i++)
            {
                listaPossiveisFornecedores[i].numeroItem = (i + 1);
                listaPossiveisFornecedores[i].nomeEmpresa = listaPossiveisFornecedores[i].NOME_FANTASIA_EMPRESA;
                listaPossiveisFornecedores[i].idEmpresa = listaPossiveisFornecedores[i].ID_CODIGO_EMPRESA;
            }

            return listaPossiveisFornecedores;
        }

        //CARREGA LISTA de FORNECEDORES que RECEBERAM a COTAÇÃO
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDeFornecedoresQueReceberamACotacao(int iCM, string idsEmpresasCotadas)
        {
            List<ListaEstilizadaDeEmpresasViewModel> listaDeFornecedoresCotados =
                dfornecedores.BuscarListaDeFornecedoresQueReceberamACotacao(iCM, idsEmpresasCotadas);

            for (int i = 0; i < listaDeFornecedoresCotados.Count; i++)
            {
                listaDeFornecedoresCotados[i].numeroItem = (i + 1);
                listaDeFornecedoresCotados[i].nomeEmpresa = listaDeFornecedoresCotados[i].NOME_FANTASIA_EMPRESA;
                listaDeFornecedoresCotados[i].idEmpresa = listaDeFornecedoresCotados[i].ID_CODIGO_EMPRESA;
            }

            return listaDeFornecedoresCotados;
        }

        ////Buscar Fornecedores em OUTROS PAÍSES  (Avulsa - Futuramente)
        //public List<empresa_usuario> BuscarFornecedoresEmOutrosPaises(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        //{
        //    DFornecedores dfornecedores = new DFornecedores();

        //    List<empresa_usuario> listaFornecedores = dfornecedores.BuscarFornecedoresEmOutrosPaises(categoriaASerCotada, quantFornecedores, idPaisCotante);

        //    return listaFornecedores;
        //}
    }
}
