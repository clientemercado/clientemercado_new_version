using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoFilhaUsuarioEmpresaService
    {
        DCotacaoFilhaUsuarioEmpresaRepository dcotacaofilhausuarioempresa = new DCotacaoFilhaUsuarioEmpresaRepository();

        //Consulta os dados da COTAÇÃO FILHA enviada pela EMPRESA, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaUsuarioEmpresaASerRespondida(cotacao_filha_usuario_empresa obj)
        {
            return dcotacaofilhausuarioempresa.ConsultarDadosDaCotacaoFilhaUsuarioEmpresaASerRespondida(obj);
        }

        //Gravar (criar) a COTAÇÃO FILHA, réplica da COTACAO_MASTER que será encaminhada aos FORNECEDORES
        public cotacao_filha_usuario_empresa GerarCotacaoFilhaUsuarioEmpresa(cotacao_filha_usuario_empresa obj)
        {
            return dcotacaofilhausuarioempresa.GerarCotacaoFilhaUsuarioEmpresa(obj);
        }

        //Buscar os FORNECEDORES para os quais foram enviadas as COTAÇÕES
        public List<cotacao_filha_usuario_empresa> ConsultarFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            return dcotacaofilhausuarioempresa.ConsultarFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);
        }

        ////Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        //public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        //{
        //    DCotacaoFilhaUsuarioCotante dcotacaofilhausuariocotante = new DCotacaoFilhaUsuarioCotante();

        //    return dcotacaofilhausuariocotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);
        //}

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_filha_usuario_empresa> ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            return dcotacaofilhausuarioempresa.ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(idEmpresa, idUsuarioEmpresa);
        }

        //Consulta os dados da COTAÇÃO FILHA enviada pelo USUÁRIO EMPRESA, a ser respondida pelo FORNECEDOR
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaUsuarioEmpresaCotanteASerRespondida(cotacao_filha_usuario_empresa obj)
        {
            return dcotacaofilhausuarioempresa.ConsultarDadosDaCotacaoFilhaUsuarioEmpresaCotanteASerRespondida(obj);
        }

        //Consulta a EXISTÊNCIA de COTAÇÃO FILHA para a COTAÇÃO MASTER em questão para este USUÁRIO EMPRESA (Obs: Isto só ocorrerá se em algum momento o usuário logado, com permissão de 
        //visualização e resposta a COTAÇÕES AVULSAS, clicou sobre a informada)
        public cotacao_filha_usuario_empresa ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(int idCotacaoMaster)
        {
            return dcotacaofilhausuarioempresa.ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(idCotacaoMaster);
        }

        //Gravar dados em RESPOSTA à COTAÇÃO FILHA enviada pelo USUÁRIO EMPRESA
        public cotacao_filha_usuario_empresa GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioEmpresa(cotacao_filha_usuario_empresa obj)
        {
            return dcotacaofilhausuarioempresa.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioEmpresa(obj);
        }

        //Consultar Nº de COTAÇÕES que já FORAM RESPONDIDAS para o USUÁRIO COTANTE
        public double ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioEmpresa(int idCotacaoMaster)
        {
            return dcotacaofilhausuarioempresa.ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioEmpresa(idCotacaoMaster);
        }

        //Buscar QUANTIDADE de FORNECEDORES que estao respondendo uma determinada COTAÇÃO
        public int ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(int idCotacaoMaster)
        {
            return dcotacaofilhausuarioempresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);
        }

        //BUSCANDO DADOS da COTAÇÃO FILHA, pela EMPRESA COTANTE
        public cotacao_filha_usuario_empresa ConsultarDadosDaCotacaoFilhaPeloUsuarioEmpresaCotante(cotacao_filha_usuario_empresa obj)
        {
            return dcotacaofilhausuarioempresa.ConsultarDadosDaCotacaoFilhaPeloUsuarioEmpresaCotante(obj);
        }

        //BUSCANDO DADOS de TODAS as COTAÇÕES disparadas pelo USUÁRIO EMPRESA COTANTE
        public List<cotacao_filha_usuario_empresa> ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioEmpresaCotante(int idCotacaoMaster)
        {
            return dcotacaofilhausuarioempresa.ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioEmpresaCotante(idCotacaoMaster);
        }
    }
}
