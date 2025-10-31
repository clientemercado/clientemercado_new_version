using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    //Busca Fornecedores na MINHA CIDADE e no MEU ESTADO (Direcionada)
    public class DFornecedoresRepository
    {
        int idEmpresa = Convert.ToInt32(Sessao.IdEmpresaUsuario);
        cliente_mercadoContext _contexto = new cliente_mercadoContext();

        public List<empresa_usuario> BuscarFornecedoresNaMinhaCidadeENoMeuEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            //Na mesma cidade do COTANTE
            List<empresa_usuario> empresasFornecedoras =
                _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada)) && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idCidadeCotante))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

            return empresasFornecedoras;
        }

        //Busca Fornecedores em OUTRA CIDADE no meu ESTADO (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmOutraCidadeENoMeuEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idOutraCidade, int idEstadoCotante, int idPaisCotante)
        {
            if (idOutraCidade != 1000000)
            {
                //Cidade específica do meu Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada)) && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idOutraCidade))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

                return empresasFornecedoras;
            }
            else
            {
                //Todas as Cidades do meu Estado (Direcionada)
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

                return empresasFornecedoras;
            }
        }

        //Buscar Fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA) (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmOutroEstadoDirecionada(int categoriaASerCotada, int quantFornecedores, int idOutroEstado, int idOutraCidadeOutroEstado, int idPaisCotante)
        {
            if (idOutraCidadeOutroEstado != 1000000)
            {
                //Cidade específica do outro Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idOutraCidadeOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

                return empresasFornecedoras;
            }
            else
            {
                //Todas as Cidades do outro Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

                return empresasFornecedoras;
            }
        }

        //Buscar Fornecedores em TODO PAÍS (Direcionada)
        public List<empresa_usuario> BuscarFornecedoresEmTodoOPaisDirecionada(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        {
            //Busca fornecedores em TODO o PAÍS
            List<empresa_usuario> empresasFornecedoras =
                _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                && (m.ID_CODIGO_EMPRESA != idEmpresa)).Take(quantFornecedores).ToList();

            return empresasFornecedoras;
        }

        //CARREGA LISTA de FORNECEDORES que RECEBERAM a COTAÇÃO
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDeFornecedoresQueReceberamACotacao(int iCM, string idsEmpresasCotadas)
        {
            var query = "";
            List<ListaEstilizadaDeEmpresasViewModel> listaDeEmpresasFornecedorasCotadas = new List<ListaEstilizadaDeEmpresasViewModel>();

            //BUSCANDO em TODAS as CIDADES do ESTADO da EMPRESA ADM
            query = "SELECT EU.*, CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, UF.UF_EMPRESA_USUARIO AS ufEmpresa, " +
                    "UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                    "FROM empresa_usuario EU " +
                    "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                    "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO) " +
                    "INNER JOIN estados_empresa_usuario UF ON(UF.ID_ESTADOS_EMPRESA_USUARIO = CE.ID_ESTADOS_EMPRESA_USUARIO) " +
                    "INNER JOIN paises_empresa_usuario P ON(P.ID_PAISES_EMPRESA_USUARIO = 4) " +
                    "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                    "WHERE EU.ID_CODIGO_EMPRESA IN (" + idsEmpresasCotadas + ") " +
                    "ORDER BY EU.NOME_FANTASIA_EMPRESA ";

            listaDeEmpresasFornecedorasCotadas = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();

            return listaDeEmpresasFornecedorasCotadas;
        }

        //CARREGA LISTA de FORNECEDORES conforme LOCAL SELECIONADO
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDePossiveisFornecedorasParaACentralDeCompras(int codRamoAtividade, int quantosFornecedores, int localBusca,
            int uFSelecionada, int codEAdmDaCC, int cCC)
        {
            //localBusca - 1 --> MEU ESTADO
            //localBusca - 2 --> OUTRO ESTADO
            //localBusca - 3 --> TODO o PAÍS

            var query = "";
            var empresasDaCC = " AND EU.ID_CODIGO_EMPRESA <> " + idEmpresa;
            var idsEmpresasDaCC = "";

            List<ListaEstilizadaDeEmpresasViewModel> listaDeEmpresasFornecedoras = new List<ListaEstilizadaDeEmpresasViewModel>();

            DEmpresasParticipantesCentralDeComprasRepository dEmpresasParticipantesCentralDeCompras = new DEmpresasParticipantesCentralDeComprasRepository();

            //EMPRESAS da CENTRAL de COMPRAS não podem receber a COTAÇÃO da CC que participam
            //----------------------------------------------------------------------------------------------------
            List<empresas_participantes_central_de_compras> listaEmpresasDestaCC =
                dEmpresasParticipantesCentralDeCompras.BuscarListaDeEmpresasConfirmadasComoParticipantesDaCC(cCC);

            for (int i = 0; i < listaEmpresasDestaCC.Count; i++)
            {
                if (idsEmpresasDaCC == "")
                {
                    idsEmpresasDaCC = listaEmpresasDestaCC[i].ID_CODIGO_EMPRESA.ToString();
                }
                else
                {
                    idsEmpresasDaCC = (idsEmpresasDaCC + "," + listaEmpresasDestaCC[i].ID_CODIGO_EMPRESA.ToString());
                }
            }

            if ((idsEmpresasDaCC != "") && (idsEmpresasDaCC != null))
            {
                empresasDaCC = " AND EU.ID_CODIGO_EMPRESA NOT IN (" + idsEmpresasDaCC + ")";
            }
            //----------------------------------------------------------------------------------------------------

            if (localBusca == 1)
            {
                //BUSCANDO em TODAS as CIDADES do ESTADO da EMPRESA ADM
                query = "SELECT TOP " + quantosFornecedores + " EU.*, CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, UF.UF_EMPRESA_USUARIO AS ufEmpresa, " +
                        "UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                        "FROM empresa_usuario EU " +
                        "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario UF ON(UF.ID_ESTADOS_EMPRESA_USUARIO = " + uFSelecionada + ") " +
                        "INNER JOIN paises_empresa_usuario P ON(P.ID_PAISES_EMPRESA_USUARIO = 4) " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                        "WHERE EU.ID_GRUPO_ATIVIDADES_ATACADO = " + codRamoAtividade + empresasDaCC + " " +
                        "ORDER BY NEWID() ";

                listaDeEmpresasFornecedoras = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();
            }
            else if (localBusca == 2)
            {
                //BUSCANDO em TODAS as CIDADES de OUTRO ESTADO
                query = "SELECT TOP " + quantosFornecedores + " EU.*, CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, UF.UF_EMPRESA_USUARIO AS ufEmpresa, " +
                        "UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                        "FROM empresa_usuario EU " +
                        "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO AND CE.ID_ESTADOS_EMPRESA_USUARIO = " + uFSelecionada + ") " +
                        "INNER JOIN estados_empresa_usuario UF ON(UF.ID_ESTADOS_EMPRESA_USUARIO = " + uFSelecionada + ") " +
                        "INNER JOIN paises_empresa_usuario P ON(P.ID_PAISES_EMPRESA_USUARIO = 4) " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                        "WHERE EU.ID_GRUPO_ATIVIDADES_ATACADO = " + codRamoAtividade + empresasDaCC + " " +
                        "ORDER BY NEWID() ";

                listaDeEmpresasFornecedoras = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();
            }
            else if (localBusca == 3)
            {
                //BUSCANDO em TODO o PAÍS
                query = "SELECT TOP " + quantosFornecedores + " EU.*, CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, UF.UF_EMPRESA_USUARIO AS ufEmpresa, " +
                        "UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                        "FROM empresa_usuario EU " +
                        "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario UF ON(UF.ID_ESTADOS_EMPRESA_USUARIO = CE.ID_ESTADOS_EMPRESA_USUARIO) " +
                        "INNER JOIN paises_empresa_usuario P ON(P.ID_PAISES_EMPRESA_USUARIO = 4) " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                        "WHERE EU.ID_GRUPO_ATIVIDADES_ATACADO = " + codRamoAtividade + empresasDaCC + " " +
                        "ORDER BY NEWID() ";

                listaDeEmpresasFornecedoras = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();
            }

            return listaDeEmpresasFornecedoras.OrderBy(m => (m.ufEmpresa)).ThenByDescending(m => m.cidadeEmpresa).ToList();
        }

        ////Buscar Fornecedores em OUTROS PAÍSES (Direcionada - Futuramente)
        //public List<empresa_usuario> BuscarFornecedoresEmOutrosPaises(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        //{
        //    cliente_mercadoContext _contexto = new cliente_mercadoContext();

        //    List<empresa_usuario> empresasFornecedoras =
        //        _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
        //        && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))).Take(quantFornecedores).ToList();

        //    return empresasFornecedoras;
        //}

        //Busca Fornecedores na MINHA CIDADE e no MEU ESTADO (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresNaMinhaCidadeENoMeuEstadoAvulsa(int categoriaASerCotada, int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            //Busca Fornecedores na MINHA CIDADE e no MEU ESTADO
            List<empresa_usuario> empresasFornecedoras =
                _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada)) && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idCidadeCotante))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

            return empresasFornecedoras;
        }

        //Busca Fornecedores em OUTRA CIDADE no meu ESTADO (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresEmOutraCidadeENoMeuEstadoAvulsa(int categoriaASerCotada, int idOutraCidade, int idEstadoCotante, int idPaisCotante)
        {
            if (idOutraCidade != 1000000)
            {
                //Cidade específica do meu Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada)) && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idOutraCidade))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

                return empresasFornecedoras;
            }
            else
            {
                //Todas as Cidades do meu Estado (Direcionada)
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstadoCotante))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

                return empresasFornecedoras;
            }
        }

        //Buscar Fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA) (Avulsa)
        public List<empresa_usuario> BuscarFornecedoresEmOutroEstadoAvulsa(int categoriaASerCotada, int idOutroEstado, int idOutraCidadeOutroEstado, int idPaisCotante)
        {
            if (idOutraCidadeOutroEstado != 1000000)
            {
                //Cidade específica do outro Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.Equals(idOutraCidadeOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

                return empresasFornecedoras;
            }
            else
            {
                //Todas as Cidades do outro Estado
                List<empresa_usuario> empresasFornecedoras =
                    _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.Equals(idOutroEstado))
                    && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                    && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

                return empresasFornecedoras;
            }
        }

        //Buscar fornecedores de TODO o país
        public List<empresa_usuario> BuscarFornecedoresEmTodoOPaisAvulsa(int categoriaASerCotada, int idPaisCotante)
        {
            //Busa fornecedores em TODO o país
            List<empresa_usuario> empresasFornecedoras =
                _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
                && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))
                && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

            return empresasFornecedoras;
        }

        ////Buscar Fornecedores em OUTROS PAÍSES ( Avulsa - Futuramente)
        //public List<empresa_usuario> BuscarFornecedoresEmOutrosPaises(int categoriaASerCotada, int quantFornecedores, int idPaisCotante)
        //{
        //    cliente_mercadoContext _contexto = new cliente_mercadoContext();

        //    List<empresa_usuario> empresasFornecedoras =
        //        _contexto.empresa_usuario.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(categoriaASerCotada))
        //        && (m.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.Equals(idPaisCotante))).Take(quantFornecedores).ToList();

        //    return empresasFornecedoras;
        //}

    }
}
