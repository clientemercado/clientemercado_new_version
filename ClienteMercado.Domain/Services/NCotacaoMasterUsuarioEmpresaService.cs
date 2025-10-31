using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoMasterUsuarioEmpresaService
    {
        DCotacaoMasterUsuarioEmpresaRepository dcotacaomasterusuarioempresa = new DCotacaoMasterUsuarioEmpresaRepository();

        //Buscar a quantidade de Cotações do Usuário Cotante para montar o nome default da cotação
        public List<cotacao_master_usuario_empresa> VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(int idEmpresa)
        {
            return dcotacaomasterusuarioempresa.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(idEmpresa);
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas pelo Usuário da Empresa
        public List<cotacao_master_usuario_empresa> CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa()
        {
            return dcotacaomasterusuarioempresa.CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa();
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas pelo Usuário da Empresa
        public List<cotacao_master_usuario_empresa> CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa()
        {
            return dcotacaomasterusuarioempresa.CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa();
        }

        //Gravar (criar) a COTAÇÃO MASTER da EMPRESA que está enviando a COTACAO
        public cotacao_master_usuario_empresa GerarCotacaoMasterUsuarioEmpresa(cotacao_master_usuario_empresa obj)
        {
            return dcotacaomasterusuarioempresa.GerarCotacaoMasterUsuarioEmpresa(obj);
        }

        //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário da empresa
        public cotacao_master_usuario_empresa BuscarCotacaoMasterDoUsuarioEmpresa(int idCotacaoMaster)
        {
            return dcotacaomasterusuarioempresa.BuscarCotacaoMasterDoUsuarioEmpresa(idCotacaoMaster);
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS de EMPRESAS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_master_usuario_empresa> ConsultarCotacoesAvulsasEnviadasPorEmpresasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            return dcotacaomasterusuarioempresa.ConsultarCotacoesAvulsasEnviadasPorEmpresasParaOUsuarioEmpresa(idEmpresa, idUsuarioEmpresa);
        }

        //Atualizar o STATUS da COTAÇÃO MASTER
        public cotacao_master_usuario_empresa AtualizarStatusDaCotacao(int idCotacaoMaster, int quantidadeDeCotacaoesRespondidas)
        {
            return dcotacaomasterusuarioempresa.AtualizarStatusDaCotacao(idCotacaoMaster, quantidadeDeCotacaoesRespondidas);
        }
    }
}
