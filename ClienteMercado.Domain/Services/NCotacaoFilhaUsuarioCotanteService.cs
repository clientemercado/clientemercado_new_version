using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoFilhaUsuarioCotanteService
    {
        DCotacaoFilhaUsuarioCotanteRepository dcotacaofilhausuariocotante = new DCotacaoFilhaUsuarioCotanteRepository();

        //Gravar (criar) a COTAÇÃO FILHA, réplica da COTACAO_MASTER que será encaminhada aos FORNECEDORES
        public cotacao_filha_usuario_cotante GerarCotacaoFilhaUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            return dcotacaofilhausuariocotante.GerarCotacaoFilhaUsuarioCotante(obj);
        }

        //Buscar os FORNECEDORES para os quais foram enviadas as COTAÇÕES
        public List<cotacao_filha_usuario_cotante> ConsultarFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);
        }

        //Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);
        }

        //Buscar QUANTOS FORNECEDORES já responderam a COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueJaResponderamACotacao(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarQuantidadeDeFornecedoresQueJaResponderamACotacao(idCotacaoMaster);
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_filha_usuario_cotante> ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            return dcotacaofilhausuariocotante.ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(idEmpresa, idUsuarioEmpresa);
        }

        //Consulta os dados da COTAÇÃO FILHA enviada pelo USUÁRIO COTANTE, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_cotante ConsultarDadosDaCotacaoFilhaUsuarioCotanteASerRespondida(cotacao_filha_usuario_cotante obj)
        {
            return dcotacaofilhausuariocotante.ConsultarDadosDaCotacaoFilhaUsuarioCotanteASerRespondida(obj);
        }

        //Consulta a EXISTÊNCIA de COTAÇÃO FILHA para a COTAÇÃO MASTER em questão para este USUÁRIO EMPRESA (Obs: Isto só ocorrerá se em algum momento o usuário logado, com permissão de 
        //visualização e resposta a COTAÇÕES AVULSAS, clicou sobre a informada)
        public cotacao_filha_usuario_cotante ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(idCotacaoMaster);
        }

        //Gravar dados em RESPOSTA à COTAÇÃO FILHA enviada pelo USUÁRIO COTANTE
        public cotacao_filha_usuario_cotante GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            return dcotacaofilhausuariocotante.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioCotante(obj);
        }

        //Consultar Nº de COTAÇÕES que já FORAM RESPONDIDAS para o USUÁRIO COTANTE
        public double ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioCotante(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioCotante(idCotacaoMaster);
        }

        //BUSCANDO DADOS da COTAÇÃO FILHA, pela EMPRESA COTANTE
        public cotacao_filha_usuario_cotante ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(cotacao_filha_usuario_cotante obj)
        {
            return dcotacaofilhausuariocotante.ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(obj);
        }

        //BUSCANDO DADOS de TODAS as COTAÇÕES disparadas pelo USUÁRIO COTANTE
        public List<cotacao_filha_usuario_cotante> ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioCotante(int idCotacaoMaster)
        {
            return dcotacaofilhausuariocotante.ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioCotante(idCotacaoMaster);
        }
    }
}
