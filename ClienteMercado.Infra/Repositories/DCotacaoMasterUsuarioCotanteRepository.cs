using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoMasterUsuarioCotanteRepository
    {
        //Buscar a quantidade de Cotações do Usuário Cotante para montar o nome default da cotação
        public List<cotacao_master_usuario_cotante> VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(int idUsuarioLogado)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_cotante> quantasCotacoesParaOUsuarioCotante =
                    _contexto.cotacao_master_usuario_cotante.Where(m => m.ID_CODIGO_USUARIO_COTANTE.Equals(idUsuarioLogado)).ToList();

                return quantasCotacoesParaOUsuarioCotante;
            }
        }

        //Gravar (criar) a COTAÇÃO MASTER da DO usuário cotante que está enviando a COTACAO
        public cotacao_master_usuario_cotante GerarCotacaoMasterUsuarioCotante(cotacao_master_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_master_usuario_cotante cotacaoMasterUsuarioCotante =
                    _contexto.cotacao_master_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return cotacaoMasterUsuarioCotante;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas pelo Usuário Cotante
        public List<cotacao_master_usuario_cotante> CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante()
        {
            int idUsuario = Convert.ToInt32(Sessao.IdUsuarioLogado);

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_cotante> cotacoesDoUsuario =
                    _contexto.cotacao_master_usuario_cotante.Where(m => (m.ID_CODIGO_USUARIO_COTANTE.Equals(idUsuario))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(1))).OrderByDescending(m => m.DATA_CRIACAO_COTACAO_USUARIO_COTANTE).ToList();

                return cotacoesDoUsuario;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES DIRECIONADAS enviadas pelo Usuário Cotante
        public List<cotacao_master_usuario_cotante> CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante()
        {
            int idUsuario = Convert.ToInt32(Sessao.IdUsuarioLogado);

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<cotacao_master_usuario_cotante> cotacoesDoUsuario =
                    _contexto.cotacao_master_usuario_cotante.Where(m => (m.ID_CODIGO_USUARIO_COTANTE.Equals(idUsuario))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(2))).OrderByDescending(m => m.DATA_CRIACAO_COTACAO_USUARIO_COTANTE).ToList();

                return cotacoesDoUsuario;
            }
        }

        //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário COTANTE
        public cotacao_master_usuario_cotante BuscarCotacaoMasterDoUsuarioCotante(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cotacao_master_usuario_cotante cotacaoMasterDoUsuario =
                    _contexto.cotacao_master_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster)));

                return cotacaoMasterDoUsuario;
            }
        }

        //Carrega a Lista com todas as COTAÇÕES AVULSAS enviadas por USUÁRIOS COTANTES ao USUÁRIO EMPRESA
        public List<cotacao_master_usuario_cotante> ConsultarCotacoesAvulsasEnviadasPorUsuariosCotantesParaOUsuarioEmpresa(int idEmpresa, int idUsuarioEmpresa)
        {
            int idGrupoAtividades = 0;

            usuario_empresa dadosUsuarioEmpresa = new usuario_empresa();
            List<cotacao_master_usuario_cotante> cotacoesAvulsasEnviadasPeloUsuarioCotante = new List<cotacao_master_usuario_cotante>();

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
                cotacoesAvulsasEnviadasPeloUsuarioCotante =
                    _contexto.cotacao_master_usuario_cotante.Where(m => (m.ID_GRUPO_ATIVIDADES.Equals(idGrupoAtividades))
                    && (m.ID_CODIGO_TIPO_COTACAO.Equals(2))).ToList();
                //}

                return cotacoesAvulsasEnviadasPeloUsuarioCotante;
            }
        }

        //Atualizar o STATUS da COTAÇÃO MASTER
        public cotacao_master_usuario_cotante AtualizarStatusDaCotacao(int idCotacaoMaster, int quantidadeDeCotacaoesRespondidas)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                System.DateTime dataDeHoje = DateTime.Now;

                cotacao_master_usuario_cotante atualizarStatusDaCotacaoMaster =
                    _contexto.cotacao_master_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster)));

                if (atualizarStatusDaCotacaoMaster != null)
                {
                    if ((quantidadeDeCotacaoesRespondidas > 0))
                    {
                        if ((dataDeHoje.Date <= atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 1))
                        {
                            //Atualiza o STATUS para 'EM ANDAMENTO'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 2;

                            _contexto.SaveChanges();
                        }
                        else if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 1))
                        {
                            //Atualiza o STATUS para 'ENCERRADA'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 3;

                            _contexto.SaveChanges();
                        }
                        else if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.Date)
                            && (atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO == 2))
                        {
                            //Atualiza o STATUS para 'ENCERRADA'
                            atualizarStatusDaCotacaoMaster.ID_CODIGO_STATUS_COTACAO = 3;

                            _contexto.SaveChanges();
                        }
                    }
                    else
                    {
                        if ((dataDeHoje.Date > atualizarStatusDaCotacaoMaster.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.Date)
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
