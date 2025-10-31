using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoFilhaUsuarioCotanteRepository : RepositoryBase<cotacao_filha_usuario_cotante>
    {
        //Gravar (criar) a COTAÇÃO FILHA, réplica da COTACAO_MASTER que será encaminhada aos FORNECEDORES
        public cotacao_filha_usuario_cotante GerarCotacaoFilhaUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            cotacao_filha_usuario_cotante cotacaoFilhaUsuarioCotante =
                _contexto.cotacao_filha_usuario_cotante.Add(obj);
            _contexto.SaveChanges();

            return cotacaoFilhaUsuarioCotante;
        }

        //Buscar os FORNECEDORES para os quais foram enviadas as COTAÇÕES
        public List<cotacao_filha_usuario_cotante> ConsultarFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            int idEmpresa = Convert.ToInt32(Sessao.IdEmpresaUsuario);

            List<cotacao_filha_usuario_cotante> buscarFornecedores =
                _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster)) && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();

            return buscarFornecedores;
        }

        //Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            List<cotacao_filha_usuario_cotante> quantidadeDeFornecedoresRespondendo =
                _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))).ToList();

            return quantidadeDeFornecedoresRespondendo.Count;
        }

        //Buscar QUANTOS FORNECEDORES já responderam a COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueJaResponderamACotacao(int idCotacaoMaster)
        {
            List<cotacao_filha_usuario_cotante> quantidadeDeFornecedoresQueResponderam =
                _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))
                && (m.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE == true)).ToList();

            return quantidadeDeFornecedoresQueResponderam.Count;
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_filha_usuario_cotante> ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            List<cotacao_filha_usuario_cotante> cotacoesDirecionadasEnviadasAoUsuario = new List<cotacao_filha_usuario_cotante>();

            var cotacoesDirecionadas = (from cf in _contexto.cotacao_filha_usuario_cotante
                                        join cm in _contexto.cotacao_master_usuario_cotante on cf.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE equals cm.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE
                                        where ((cf.ID_CODIGO_EMPRESA == idEmpresa) && (cf.ID_CODIGO_USUARIO == idUsuarioEmpresa) && (cm.ID_CODIGO_TIPO_COTACAO == 1))
                                        select new
                                        {
                                            cf.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                                            cf.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE,
                                            cf.ID_CODIGO_EMPRESA,
                                            cf.ID_CODIGO_USUARIO,
                                            cf.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE,
                                            cf.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE,
                                            cf.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE,
                                            cf.TIPO_DESCONTO,
                                            cf.PERCENTUAL_DESCONTO
                                        }).ToList();

            for (int i = 0; i < cotacoesDirecionadas.Count; i++)
            {
                cotacao_filha_usuario_cotante listaDeCotacoesDirecionadas = new cotacao_filha_usuario_cotante();

                listaDeCotacoesDirecionadas.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = cotacoesDirecionadas[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;
                listaDeCotacoesDirecionadas.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = cotacoesDirecionadas[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE;
                listaDeCotacoesDirecionadas.ID_CODIGO_EMPRESA = cotacoesDirecionadas[i].ID_CODIGO_EMPRESA;
                listaDeCotacoesDirecionadas.ID_CODIGO_USUARIO = cotacoesDirecionadas[i].ID_CODIGO_USUARIO;
                listaDeCotacoesDirecionadas.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE = cotacoesDirecionadas[i].RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE;
                listaDeCotacoesDirecionadas.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = cotacoesDirecionadas[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE;
                listaDeCotacoesDirecionadas.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = cotacoesDirecionadas[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE;
                listaDeCotacoesDirecionadas.TIPO_DESCONTO = cotacoesDirecionadas[i].TIPO_DESCONTO;
                listaDeCotacoesDirecionadas.PERCENTUAL_DESCONTO = cotacoesDirecionadas[i].PERCENTUAL_DESCONTO;

                cotacoesDirecionadasEnviadasAoUsuario.Add(listaDeCotacoesDirecionadas);
            }

            return cotacoesDirecionadasEnviadasAoUsuario;
        }

        //Consulta os dados da COTAÇÃO FILHA enviada pelo USUÁRIO COTANTE, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_cotante ConsultarDadosDaCotacaoFilhaUsuarioCotanteASerRespondida(cotacao_filha_usuario_cotante obj)
        {
            cotacao_filha_usuario_cotante buscarDadosDaCotacao =
                _contexto.cotacao_filha_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE))
                && (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA)) && (m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO)));

            return buscarDadosDaCotacao;
        }

        //Consulta a EXISTÊNCIA de COTAÇÃO FILHA para a COTAÇÃO MASTER em questão para este USUÁRIO EMPRESA (Obs: Isto só ocorrerá se em algum momento o usuário logado, com permissão de 
        //visualização e resposta a COTAÇÕES AVULSAS, clicou sobre a informada)
        public cotacao_filha_usuario_cotante ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(int idCotacaoMaster)
        {
            int idEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
            int idUsuarioLogado = (int)Sessao.IdUsuarioLogado;

            cotacao_filha_usuario_cotante buscarDadosDaCotacao =
                _contexto.cotacao_filha_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster)
                && (m.ID_CODIGO_EMPRESA.Equals(idEmpresaLogada))
                && (m.ID_CODIGO_USUARIO.Equals(idUsuarioLogado))));

            return buscarDadosDaCotacao;
        }

        //Gravar dados em RESPOSTA à COTAÇÃO FILHA enviada pelo USUÁRIO COTANTE
        public cotacao_filha_usuario_cotante GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            cotacao_filha_usuario_cotante cotacaoASerRespondida =
                _contexto.cotacao_filha_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE)));

            if (cotacaoASerRespondida != null)
            {
                cotacaoASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = obj.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE;
                cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = obj.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE;
                cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE = obj.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE;
                cotacaoASerRespondida.ID_TIPO_FRETE = obj.ID_TIPO_FRETE;
                cotacaoASerRespondida.TIPO_DESCONTO = obj.TIPO_DESCONTO;
                cotacaoASerRespondida.PERCENTUAL_DESCONTO = obj.PERCENTUAL_DESCONTO;
                cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE = true;
                cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_COTANTE = obj.OBSERVACAO_COTACAO_USUARIO_COTANTE;
                cotacaoASerRespondida.COTACAO_FILHA_USUARIO_COTANTE_EDITADA = obj.COTACAO_FILHA_USUARIO_COTANTE_EDITADA;

                _contexto.SaveChanges();
            }

            return cotacaoASerRespondida;
        }

        //Consultar Nº de COTAÇÕES que já FORAM RESPONDIDAS para o USUÁRIO COTANTE
        public double ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioCotante(int idCotacaoMaster)
        {
            DCotacaoMasterUsuarioCotanteRepository dadosCotacaoMasterUsuarioCotante = new DCotacaoMasterUsuarioCotanteRepository();

            double percentualRespondido = 0;

            //Verifica quantas COTAÇÕES foram RESPONDIDAS
            List<cotacao_filha_usuario_cotante> cotacoesJaRespondidas =
                _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))
                && (m.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE.Equals(true))).ToList();

            if (cotacoesJaRespondidas.Count > 0)
            {
                //Verifica quantas COTAÇÕES foram ENVIADAS
                List<cotacao_filha_usuario_cotante> cotacoesEnviadas =
                    _contexto.cotacao_filha_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))).ToList();

                //Atualizar STATUS da COTAÇÃO
                dadosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(idCotacaoMaster, cotacoesJaRespondidas.Count);

                //Calcula o PERCENTUAL já respondido
                percentualRespondido = (((double)cotacoesJaRespondidas.Count / cotacoesEnviadas.Count) * 100);
            }

            return percentualRespondido;
        }

        //BUSCANDO DADOS da COTAÇÃO FILHA, pela EMPRESA COTANTE
        public cotacao_filha_usuario_cotante ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            cotacao_filha_usuario_cotante dadosConsultadosDaCotacaoFilhaUsuarioCotante =
                _contexto.cotacao_filha_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(obj.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE)));

            return dadosConsultadosDaCotacaoFilhaUsuarioCotante;
        }

        //BUSCANDO DADOS de TODAS as COTAÇÕES disparadas pelo USUÁRIO COTANTE
        public List<cotacao_filha_usuario_cotante> ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioCotante(int idCotacaoMaster)
        {
            List<cotacao_filha_usuario_cotante> cotacoesFilhaDisparadas =
                _contexto.cotacao_filha_usuario_cotante.Where(m => m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster)).ToList();

            return cotacoesFilhaDisparadas;
        }

        ////Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS COTANTES a este Usuário
        //public List<cotacao_filha_usuario_cotante> ConsultarCotacaoesAvulsasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        //{
        //    int idGrupoAtividades = 0;

        //    usuario_empresa dadosUsuarioEmpresa = new usuario_empresa();

        //    using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
        //    {
        //        //Verifica se o USUÁRIO VENDEDOR tem acesso às COTAÇÕES AVULSAS
        //        dadosUsuarioEmpresa =
        //            _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_USUARIO.Equals(idUsuarioEmpresa)));

        //        if (dadosUsuarioEmpresa.VER_COTACAO_AVULSA)
        //        {
        //            idGrupoAtividades = dadosUsuarioEmpresa.empresa_usuario.ID_GRUPO_ATIVIDADES;
        //        }
        //    }

        //    using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
        //    {
        //        List<cotacao_filha_usuario_cotante> buscarCotacoesAvulsasEnviadasAoUsuario =
        //            _contexto.cotacao_master_usuario_cotante.Where(m => (m.ID_CODIGO_EMPRESA.Equals(idEmpresa)) && (m.ID_CODIGO_USUARIO.Equals(idUsuarioEmpresa)))
        //            .OrderByDescending(m => m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE).ToList();

        //        cotacoesDeEmpresasAvulsas =
        //                _contexto.cotacao_master_empresa.Where(m => m.ID_GRUPO_ATIVIDADES.Equals(idGrupoAtividades)
        //                && (m.ID_CODIGO_TIPO_COTACAO.Equals(2))
        //                && (m.ID_CODIGO_STATUS_COTACAO != 3)).ToList();

        //        return buscarCotacoesAvulsasEnviadasAoUsuario;
        //    }
        //}
    }
}
