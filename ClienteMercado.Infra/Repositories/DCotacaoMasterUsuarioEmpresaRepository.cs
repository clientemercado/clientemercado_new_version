using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoMasterUsuarioEmpresaRepository
    {
        //Buscar a quantidade de Cotações do Usuário Cotante para montar o nome default da cotação
        public List<cotacao_master_usuario_empresa> VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(int idEmpresa)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_empresa> quantasCotacoesParaOUsuarioEmpresa =
                    _contexto.cotacao_master_usuario_empresa.Where(m => m.ID_CODIGO_EMPRESA.Equals(idEmpresa)).ToList();

                return quantasCotacoesParaOUsuarioEmpresa;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas pelo Usuário da Empresa
        public List<cotacao_master_usuario_empresa> CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa()
        {
            int idUsuario = Convert.ToInt32(Sessao.IdUsuarioLogado);

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_empresa> cotacoesDoUsuario =
                    _contexto.cotacao_master_usuario_empresa.Where(m => (m.ID_CODIGO_USUARIO.Equals(idUsuario))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(1))).OrderByDescending(m => m.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA).ToList();

                return cotacoesDoUsuario;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas pelo Usuário da Empresa
        public List<cotacao_master_usuario_empresa> CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa()
        {
            int idUsuario = Convert.ToInt32(Sessao.IdUsuarioLogado);

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_empresa> cotacoesDoUsuario =
                    _contexto.cotacao_master_usuario_empresa.Where(m => (m.ID_CODIGO_USUARIO.Equals(idUsuario))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(2))).OrderByDescending(m => m.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA).ToList();

                return cotacoesDoUsuario;
            }
        }

        //Gravar (criar) a COTAÇÃO MASTER da EMPRESA que está enviando a COTACAO
        public cotacao_master_usuario_empresa GerarCotacaoMasterUsuarioEmpresa(cotacao_master_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_master_usuario_empresa cotacaoMasterUsuarioEmpresa =
                    _contexto.cotacao_master_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return cotacaoMasterUsuarioEmpresa;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS de EMPRESAS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_master_usuario_empresa> ConsultarCotacoesAvulsasEnviadasPorEmpresasParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            int idGrupoAtividades = 0;

            usuario_empresa dadosUsuarioEmpresa = new usuario_empresa();
            List<cotacao_master_usuario_empresa> cotacoesAvulsasEnviadasPeloUsuarioEmpresa = new List<cotacao_master_usuario_empresa>();

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                //Verifica se o USUÁRIO VENDEDOR tem acesso às COTAÇÕES AVULSAS
                dadosUsuarioEmpresa =
                    _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_USUARIO.Equals(idUsuarioEmpresa)));

                //Traz as COTAÇÕES AVULSAS, se o USUARIO responsável por responder a COTAÇÃO AVULSA tiver permissão para tal.
                //if (dadosUsuarioEmpresa.VER_COTACAO_AVULSA)
                //{
                idGrupoAtividades = dadosUsuarioEmpresa.empresa_usuario.ID_GRUPO_ATIVIDADES;

                //Busca COTAÇÕES AVULSAS habilitadas para a categoria do USUÁRIO logado (COTACÃO MASTER)
                cotacoesAvulsasEnviadasPeloUsuarioEmpresa =
                    _contexto.cotacao_master_usuario_empresa.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(idGrupoAtividades))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(2)) && (m.ID_CODIGO_EMPRESA != idEmpresa)).ToList();
                //}

                return cotacoesAvulsasEnviadasPeloUsuarioEmpresa;
            }
        }

        //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário da empresa
        public cotacao_master_usuario_empresa BuscarCotacaoMasterDoUsuarioEmpresa(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_master_usuario_empresa cotacaoMasterDoUsuario =
                    _contexto.cotacao_master_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster)));

                return cotacaoMasterDoUsuario;
            }
        }

        //Atualizar o STATUS da COTAÇÃO MASTER
        public cotacao_master_usuario_empresa AtualizarStatusDaCotacao(int idCotacaoMaster, int quantidadeDeCotacaoesRespondidas)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                System.DateTime dataDeHoje = DateTime.Now;

                cotacao_master_usuario_empresa atualizarStatusDaCotacaoMaster =
                    _contexto.cotacao_master_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster)));

                if (atualizarStatusDaCotacaoMaster != null)
                {
                    if ((quantidadeDeCotacaoesRespondidas > 0))
                    {
                        if ((dataDeHoje.Date <= atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 1))
                        {
                            //Atualiza o STATUS para 'EM ANDAMENTO'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 2;

                            _contexto.SaveChanges();
                        }
                        else if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 1))
                        {
                            //Atualiza o STATUS para 'ENCERRADA'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 3;

                            _contexto.SaveChanges();
                        }
                        else if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 2))
                        {
                            //Atualiza o STATUS para 'ENCERRADA'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 3;

                            _contexto.SaveChanges();
                        }
                    }
                    else
                    {
                        if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO != 3))
                        {
                            //Atualiza o STATUS para 'ENCERRADA'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 3;

                            _contexto.SaveChanges();
                        }
                    }
                }

                return atualizarStatusDaCotacaoMaster;
            }
        }

    }
}
