using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCentralDeComprasRepository : RepositoryBase<central_de_compras>
    {
        int idEmpresa = (int)Sessao.IdEmpresaUsuario;

        //CRIAR/GRAVAR CENTRAL de COMPRAS
        public DadosDaCentralComprasViewModel CriarCentralDeComprasNoSistema(central_de_compras obj)
        {
            try
            {
                //----------------------------------------------------------
                //VALIDAR EMPRESA para a criação da CENTRAL de COMPRAS - 
                //Obs: 1) EMPRESA não pode ser ADM de nenhuma CENTRAL com o mesmo RAMO de ATIVIDADES e 2) nem participar como membro de alguma outra CENTRAL do mesmo RAMO de ATIVIDADE
                //----------------------------------------------------------

                string status = "";
                string query = "";
                DadosDaCentralComprasViewModel dadosDaCentral = new DadosDaCentralComprasViewModel();

                // 1- VERIFICAR se a EMPRESA logada é ADM de alguma CENTRAL de COMPRAS com o mesmo RAMO de ATIVIDADE
                central_de_compras centralDeCompras =
                    _contexto.central_de_compras.FirstOrDefault(m => ((m.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS == obj.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS)
                    && (m.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS == obj.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS) && (m.ID_GRUPO_ATIVIDADES == obj.ID_GRUPO_ATIVIDADES)));

                if (centralDeCompras != null)
                {
                    dadosDaCentral.NOME_CENTRAL_COMPRAS = centralDeCompras.NOME_CENTRAL_COMPRAS;
                    dadosDaCentral.ID_CENTRAL_COMPRAS = centralDeCompras.ID_CENTRAL_COMPRAS;
                    dadosDaCentral.tipoParticipacao = "admin";
                    status = "jaParticipa";
                }

                if (status == "")
                {
                    // 2- VERIFICAR se a EMPRESA logada já faz parte de alguma CENTRAL de COMPRAS com o mesmo RAMO de ATIVIDADE

                    //empresas_participantes_central_de_compras participacaoDaEmpresa =
                    //    _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == obj.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS));

                    query = "SELECT EP.* FROM empresas_participantes_central_de_compras EP " +
                            "INNER JOIN central_de_compras CC ON(CC.ID_GRUPO_ATIVIDADES = " + obj.ID_GRUPO_ATIVIDADES + ") " +
                            "WHERE EP.ID_CODIGO_EMPRESA = " + obj.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS + " AND EP.ID_CENTRAL_COMPRAS IN(SELECT CC2.ID_CENTRAL_COMPRAS FROM central_de_compras CC2 WHERE CC2.ID_GRUPO_ATIVIDADES = " + obj.ID_GRUPO_ATIVIDADES + ")";

                    List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel> listaDeCentraisQueParticipa =
                        _contexto.Database.SqlQuery<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel>(query).ToList();

                    if (listaDeCentraisQueParticipa.Count > 0)
                    {
                        var idCentralCompras = listaDeCentraisQueParticipa[0].ID_CENTRAL_COMPRAS;

                        //DADOS da CENTRAL de COMPRAS da qual já é participante
                        central_de_compras centralDeComprasDaQualEParticipante =
                            _contexto.central_de_compras.FirstOrDefault(m => (m.ID_CENTRAL_COMPRAS == idCentralCompras));

                        if (centralDeComprasDaQualEParticipante != null)
                        {
                            dadosDaCentral.NOME_CENTRAL_COMPRAS = centralDeComprasDaQualEParticipante.NOME_CENTRAL_COMPRAS;
                            dadosDaCentral.ID_CENTRAL_COMPRAS = centralDeComprasDaQualEParticipante.ID_CENTRAL_COMPRAS;
                            dadosDaCentral.tipoParticipacao = "partic";
                            status = "jaParticipa";
                        }
                    }
                }

                if (status == "")
                {
                    //CRIAR/GRAVAR CENTRAL de COMPRAS
                    central_de_compras criarCentralCompras =
                        _contexto.central_de_compras.Add(obj);
                    _contexto.SaveChanges();

                    //POPULAR OBJ para RETORNO
                    dadosDaCentral.ID_CENTRAL_COMPRAS = criarCentralCompras.ID_CENTRAL_COMPRAS;
                    dadosDaCentral.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS = criarCentralCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    dadosDaCentral.tipoParticipacao = "criado";

                    //GRAVAR EMPRESA como MEMBRO da CENTRAL de COMPRAS
                    empresas_participantes_central_de_compras dadosEmpresaParticipante = new empresas_participantes_central_de_compras();

                    dadosEmpresaParticipante.ID_CENTRAL_COMPRAS = dadosDaCentral.ID_CENTRAL_COMPRAS;
                    dadosEmpresaParticipante.ID_CODIGO_EMPRESA = obj.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    dadosEmpresaParticipante.DATA_ADESAO_EMPRESA_CENTRAL_COMPRAS = DateTime.Now;
                    dadosEmpresaParticipante.DATA_ENCERRAMENTO_PARTICIPACAO_CENTRAL_COMPRAS = Convert.ToDateTime("1900-01-01");
                    dadosEmpresaParticipante.DATA_CONVITE_CENTRAL_COMPRAS = DateTime.Now;
                    dadosEmpresaParticipante.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS = true;

                    _contexto.empresas_participantes_central_de_compras.Add(dadosEmpresaParticipante);
                    _contexto.SaveChanges();
                }

                return dadosDaCentral;
            }
            catch (Exception erro)
            {
                Trace.Write(erro.ToString());

                throw erro;
            }
        }

        //CARREGA DADOS da CENTRAL de COMPRAS
        public central_de_compras CarregarDadosDaCentralDeCompras(int cCC)
        {
            central_de_compras dadosDaCC = _contexto.central_de_compras.Include("empresa_usuario").FirstOrDefault(m => (m.ID_CENTRAL_COMPRAS == cCC));

            return dadosDaCC;
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS do SISTEMA
        public List<ListaCentraisComprasViewModel> CarregarListaAutoCompleteCentraisDeComprasDoSistema(string term)
        {
            var query = "SELECT * FROM central_de_compras CC WHERE CC.NOME_CENTRAL_COMPRAS LIKE '%" + term + "%' ORDER BY NOME_CENTRAL_COMPRAS, CC.DATA_CRIACAO_CENTRAL_COMPRAS DESC";

            var result = _contexto.Database.SqlQuery<ListaCentraisComprasViewModel>(query).ToList();
            return result;
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS do SISTEMA
        public List<ListaCentraisComprasViewModel> BuscarListaDeCentraisDeComprasDoSistema()
        {
            var query = "SELECT TOP 20 CC.ID_CENTRAL_COMPRAS, CC.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS, CC.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS, " +
                        "CC.NOME_CENTRAL_COMPRAS AS nomeCentralCompras, CC.DATA_CRIACAO_CENTRAL_COMPRAS, CC.DATA_ENCERRAMENTO_CENTRAL_COMPRAS, " +
                        "CC.ID_GRUPO_ATIVIDADES AS idGrupoAtividades, GA.DESCRICAO_ATIVIDADE AS ramoAtividadeCentralCompras, CE.CIDADE_EMPRESA_USUARIO AS cidadeDaCentralCompras, " +
                        "EE.UF_EMPRESA_USUARIO AS ufDaCentralCompras " +
                        "FROM central_de_compras CC " +
                        "INNER JOIN grupo_atividades_empresa GA ON(GA.ID_GRUPO_ATIVIDADES = CC.ID_GRUPO_ATIVIDADES) " +
                        "INNER JOIN empresa_usuario EU ON(EU.ID_CODIGO_EMPRESA = CC.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS) " +
                        "INNER JOIN enderecos_empresa_usuario EN ON(EN.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EN.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario EE ON(EE.ID_ESTADOS_EMPRESA_USUARIO = CE.ID_ESTADOS_EMPRESA_USUARIO) " +
                        "ORDER BY CC.DATA_CRIACAO_CENTRAL_COMPRAS DESC ";

            var result = _contexto.Database.SqlQuery<ListaCentraisComprasViewModel>(query).ToList();
            return result;
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS
        public List<ListaCentraisComprasViewModel> CarregarListaAutoCompleteCentraisDeCompras(string term)
        {
            var query = "SELECT * FROM central_de_compras CC WHERE CC.NOME_CENTRAL_COMPRAS LIKE '%" + term + "%' AND ID_CENTRAL_COMPRAS IN(SELECT ID_CENTRAL_COMPRAS FROM empresas_participantes_central_de_compras WHERE ID_CODIGO_EMPRESA = " + idEmpresa + ")";

            var result = _contexto.Database.SqlQuery<ListaCentraisComprasViewModel>(query).ToList();
            return result;
        }

        //CONSULTAR DADOS sobre a CENTRAL de COMPRAS
        public central_de_compras ConsultarDadosGeraisSobreACentralDeComprasPeloID(int codCentralCompras)
        {
            central_de_compras dadosCC =
                _contexto.central_de_compras.FirstOrDefault(m => (m.ID_CENTRAL_COMPRAS == codCentralCompras));

            return dadosCC;
        }

        //EXCLUIR a CENTRAL de COMPRAS
        public void ExcluirACentralDeCompras(int codCentralCompras, int codEmpresaAdm)
        {
            central_de_compras centralDeComprasAExcluir =
                _contexto.central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == codCentralCompras) && (m.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS == codEmpresaAdm)));

            if (centralDeComprasAExcluir != null)
            {
                _contexto.central_de_compras.Remove(centralDeComprasAExcluir);
                _contexto.SaveChanges();
            }
        }

        //CARREGAR os DADOS da CENTRAL de COMPRAS
        public central_de_compras ConsultarDadosDaCentralDeCompras(int codCentralCompras, int codEmpresaAdm)
        {
            central_de_compras dadosDaCcentralDeCompras =
                _contexto.central_de_compras.FirstOrDefault(m => (m.ID_CENTRAL_COMPRAS == codCentralCompras) && (m.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS == codEmpresaAdm));

            return dadosDaCcentralDeCompras;
        }
    }
}
