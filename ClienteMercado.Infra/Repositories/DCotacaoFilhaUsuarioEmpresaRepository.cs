using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoFilhaUsuarioEmpresaRepository
    {
        //Consulta os dados da COTAÇÃO FILHA enviada pela EMPRESA, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaUsuarioEmpresaASerRespondida(cotacao_filha_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa buscarDadosDaCotacao =
                    _contexto.cotacao_filha_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA))
                    && (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA)) && (m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO)));

                return buscarDadosDaCotacao;
            }
        }

        //Gravar (criar) a COTAÇÃO FILHA, réplica da COTACAO_MASTER que será encaminhada aos FORNECEDORES
        public cotacao_filha_usuario_empresa GerarCotacaoFilhaUsuarioEmpresa(cotacao_filha_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa cotacaoFilhaUsuarioEmpresa =
                    _contexto.cotacao_filha_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return cotacaoFilhaUsuarioEmpresa;
            }
        }

        //Buscar os FORNECEDORES para os quais foram enviadas as COTAÇÕES
        public List<cotacao_filha_usuario_empresa> ConsultarFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            int idEmpresa = Convert.ToInt32(Sessao.IdEmpresaUsuario);

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_filha_usuario_empresa> buscarFornecedores =
                    _contexto.cotacao_filha_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster)) && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

                return buscarFornecedores;
            }
        }

        ////Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        //public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        //{
        //    using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
        //    {
        //        List<cotacao_filha_usuario_cotante> quantidadeDeFornecedores =
        //            _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))).ToList();

        //        return quantidadeDeFornecedores.Count;
        //    }
        //}

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_filha_usuario_empresa> ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_filha_usuario_empresa> cotacoesDirecionadasEnviadasAoUsuario = new List<cotacao_filha_usuario_empresa>();

                var cotacoesDirecionadas = (from cf in _contexto.cotacao_filha_usuario_empresa
                                            join cm in _contexto.cotacao_master_usuario_empresa on cf.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA equals cm.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA
                                            where ((cf.ID_CODIGO_EMPRESA == idEmpresa) && (cf.ID_CODIGO_USUARIO == idUsuarioEmpresa) && (cm.ID_CODIGO_TIPO_COTACAO == 1))
                                            select new
                                            {
                                                cf.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA,
                                                cf.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA,
                                                cf.ID_CODIGO_EMPRESA,
                                                cf.ID_CODIGO_USUARIO,
                                                cf.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA,
                                                cf.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA,
                                                cf.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA,
                                                cf.TIPO_DESCONTO,
                                                cf.PERCENTUAL_DESCONTO
                                            }).ToList();

                for (int i = 0; i < cotacoesDirecionadas.Count; i++)
                {
                    cotacao_filha_usuario_empresa listaDeCotacoesDirecionadas = new cotacao_filha_usuario_empresa(); //CONTINUAR NAS LINHAS COMENTADAS ABAIXO...

                    listaDeCotacoesDirecionadas.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = cotacoesDirecionadas[i].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA;
                    listaDeCotacoesDirecionadas.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA = cotacoesDirecionadas[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA;
                    listaDeCotacoesDirecionadas.ID_CODIGO_EMPRESA = cotacoesDirecionadas[i].ID_CODIGO_EMPRESA;
                    listaDeCotacoesDirecionadas.ID_CODIGO_USUARIO = cotacoesDirecionadas[i].ID_CODIGO_USUARIO;
                    listaDeCotacoesDirecionadas.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA = cotacoesDirecionadas[i].RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA;
                    listaDeCotacoesDirecionadas.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = cotacoesDirecionadas[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA;
                    listaDeCotacoesDirecionadas.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = cotacoesDirecionadas[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA;
                    listaDeCotacoesDirecionadas.TIPO_DESCONTO = cotacoesDirecionadas[i].TIPO_DESCONTO;
                    listaDeCotacoesDirecionadas.PERCENTUAL_DESCONTO = cotacoesDirecionadas[i].PERCENTUAL_DESCONTO;

                    cotacoesDirecionadasEnviadasAoUsuario.Add(listaDeCotacoesDirecionadas);
                }

                return cotacoesDirecionadasEnviadasAoUsuario;
            }
        }

        //BUSCANDO DADOS de TODAS as COTAÇÕES disparadas pelo USUÁRIO EMPRESA COTANTE
        public List<cotacao_filha_usuario_empresa> ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioEmpresaCotante(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_filha_usuario_empresa> cotacoesFilhaDisparadas =
                    _contexto.cotacao_filha_usuario_empresa.Where(m => m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster)).ToList();

                return cotacoesFilhaDisparadas;
            }
        }

        //Consulta os dados da COTAÇÃO FILHA enviada pelo USUÁRIO EMPRESA, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaUsuarioEmpresaCotanteASerRespondida(cotacao_filha_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa buscarDadosDaCotacao =
                    _contexto.cotacao_filha_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA))
                    && (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA)) && (m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO)));

                return buscarDadosDaCotacao;
            }
        }

        //Consulta a EXISTÊNCIA de COTAÇÃO FILHA para a COTAÇÃO MASTER em questão para este USUÁRIO EMPRESA (Obs: Isto só ocorrerá se em algum momento o usuário logado, com permissão de 
        //visualização e resposta a COTAÇÕES AVULSAS, clicou sobre a informada)
        public cotacao_filha_usuario_empresa ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(int idCotacaoMaster)
        {
            int idEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
            int idUsuarioLogado = (int)Sessao.IdUsuarioLogado;

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa buscarDadosDaCotacao =
                    _contexto.cotacao_filha_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster)
                    && (m.ID_CODIGO_EMPRESA.Equals(idEmpresaLogada))
                    && (m.ID_CODIGO_USUARIO.Equals(idUsuarioLogado))));

                return buscarDadosDaCotacao;
            }
        }

        //Gravar dados em RESPOSTA à COTAÇÃO FILHA enviada pelo USUÁRIO EMPRESA
        public cotacao_filha_usuario_empresa GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioEmpresa(cotacao_filha_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa cotacaoASerRespondida =
                    _contexto.cotacao_filha_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA)));

                if (cotacaoASerRespondida != null)
                {
                    cotacaoASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = obj.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA;
                    cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = obj.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA;
                    cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA = obj.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA;
                    cotacaoASerRespondida.ID_TIPO_FRETE = obj.ID_TIPO_FRETE;
                    cotacaoASerRespondida.TIPO_DESCONTO = obj.TIPO_DESCONTO;
                    cotacaoASerRespondida.PERCENTUAL_DESCONTO = obj.PERCENTUAL_DESCONTO;
                    cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA = true;
                    cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_EMPRESA = obj.OBSERVACAO_COTACAO_USUARIO_EMPRESA;
                    cotacaoASerRespondida.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA = obj.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA;

                    _contexto.SaveChanges();
                }

                return cotacaoASerRespondida;
            }
        }

        //Consultar Nº de COTAÇÕES que já FORAM RESPONDIDAS para o USUÁRIO COTANTE
        public double ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioEmpresa(int idCotacaoMaster)
        {
            DCotacaoMasterUsuarioEmpresaRepository dadosCotacaoMasterUsuarioEmpresa = new DCotacaoMasterUsuarioEmpresaRepository();

            double percentualRespondido = 0;

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                //Verifica quantas COTAÇÕES foram RESPONDIDAS
                List<cotacao_filha_usuario_empresa> cotacoesJaRespondidas =
                    _contexto.cotacao_filha_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster))
                    && (m.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA.Equals(true))).ToList();

                if (cotacoesJaRespondidas.Count > 0)
                {
                    //Verifica quantas COTAÇÕES foram ENVIADAS
                    List<cotacao_filha_usuario_empresa> cotacoesEnviadas =
                        _contexto.cotacao_filha_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster))).ToList();

                    //Atualizar STATUS da COTAÇÃO
                    dadosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(idCotacaoMaster, cotacoesJaRespondidas.Count);

                    //Calcula o PERCENTUAL já respondido
                    percentualRespondido = (((double)cotacoesJaRespondidas.Count / cotacoesEnviadas.Count) * 100);
                }

                return percentualRespondido;
            }
        }

        //Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_filha_usuario_empresa> quantidadeDeFornecedores =
                    _contexto.cotacao_filha_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster))).ToList();

                return quantidadeDeFornecedores.Count;
            }
        }

        //BUSCANDO DADOS da COTAÇÃO FILHA, pela EMPRESA COTANTE
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaPeloUsuarioEmpresaCotante(cotacao_filha_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_filha_usuario_empresa dadosConsultadosDaCotacaoFilhaUsuarioEmpresa =
                    _contexto.cotacao_filha_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA)));

                return dadosConsultadosDaCotacaoFilhaUsuarioEmpresa;
            }
        }

        //////Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS COTANTES a este Usuário
        ////public List<cotacao_filha_usuario_cotante> ConsultarCotacaoesAvulsasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        ////{
        ////    int idGrupoAtividades = 0;

        ////    usuario_empresa dadosUsuarioEmpresa = new usuario_empresa();

        ////    using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
        ////    {
        ////        //Verifica se o USUÁRIO VENDEDOR tem acesso às COTAÇÕES AVULSAS
        ////        dadosUsuarioEmpresa =
        ////            _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_USUARIO.Equals(idUsuarioEmpresa)));

        ////        if (dadosUsuarioEmpresa.VER_COTACAO_AVULSA)
        ////        {
        ////            idGrupoAtividades = dadosUsuarioEmpresa.empresa_usuario.ID_GRUPO_ATIVIDADES;
        ////        }
        ////    }

        ////    using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
        ////    {
        ////        List<cotacao_filha_usuario_cotante> buscarCotacoesAvulsasEnviadasAoUsuario =
        ////            _contexto.cotacao_master_usuario_cotante.Where(m => (m.ID_CODIGO_EMPRESA.Equals(idEmpresa)) && (m.ID_CODIGO_USUARIO.Equals(idUsuarioEmpresa)))
        ////            .OrderByDescending(m => m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE).ToList();

        ////        cotacoesDeEmpresasAvulsas =
        ////                _contexto.cotacao_master_empresa.Where(m => m.ID_GRUPO_ATIVIDADES.Equals(idGrupoAtividades)
        ////                && (m.ID_CODIGO_TIPO_COTACAO.Equals(2))
        ////                && (m.ID_CODIGO_STATUS_COTACAO != 3)).ToList();

        ////        return buscarCotacoesAvulsasEnviadasAoUsuario;
        ////    }
        ////}
    }
}
