using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoMasterUsuarioCotanteService
    {
        DCotacaoMasterUsuarioCotanteRepository dcotacaomasterusuariocotante = new DCotacaoMasterUsuarioCotanteRepository();

        //Buscar a quantidade de Cotações do Usuário Cotante para montar o nome default da cotação
        public List<cotacao_master_usuario_cotante> VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(int idUsuarioLogado)
        {
            return dcotacaomasterusuariocotante.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(idUsuarioLogado);
        }

        //Gravar (criar) a COTAÇÃO MASTER do USUÁRIO COTANTE que está enviando a COTACAO
        public cotacao_master_usuario_cotante GerarCotacaoMasterUsuarioCotante(cotacao_master_usuario_cotante obj)
        {
            return dcotacaomasterusuariocotante.GerarCotacaoMasterUsuarioCotante(obj);
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas pelo Usuário Cotante
        public List<cotacao_master_usuario_cotante> CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante()
        {
            return dcotacaomasterusuariocotante.CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante();
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas pelo Usuário Cotante
        public List<cotacao_master_usuario_cotante> CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante()
        {
            return dcotacaomasterusuariocotante.CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante();
        }

        //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário COTANTE
        public cotacao_master_usuario_cotante BuscarCotacaoMasterDoUsuarioCotante(int idCotacaoMaster)
        {
            return dcotacaomasterusuariocotante.BuscarCotacaoMasterDoUsuarioCotante(idCotacaoMaster);
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_master_usuario_cotante> ConsultarCotacoesAvulsasEnviadasPorUsuariosCotantesParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            return dcotacaomasterusuariocotante.ConsultarCotacoesAvulsasEnviadasPorUsuariosCotantesParaOUsuarioEmpresa(idEmpresa, idUsuarioEmpresa);
        }

        //Atualizar o STATUS da COTAÇÃO MASTER
        public cotacao_master_usuario_cotante AtualizarStatusDaCotacao(int idCotacaoMaster, int quantidadeDeCotacaoesRespondidas)
        {
            return dcotacaomasterusuariocotante.AtualizarStatusDaCotacao(idCotacaoMaster, quantidadeDeCotacaoesRespondidas);
        }
    }
}
