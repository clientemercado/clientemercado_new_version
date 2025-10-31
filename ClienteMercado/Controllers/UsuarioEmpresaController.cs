using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Sms;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Controllers
{
    public class UsuarioEmpresaController : Controller
    {
        private object lista;

        //
        // GET: /UsuarioEmpresa/
        //[Authorize]
        public ActionResult PerfilUsuarioEmpresa(string nmU, string nmE, string cloG, string cbaiR, string cciD, string cesT, string ccouT)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                NUsuarioEmpresaService negociosusuarioEmpresa = new NUsuarioEmpresaService();
                usuario_empresa dadosUsuarioEmpresa = new usuario_empresa();
                PerfilUsuarioEmpresa dadosEPerfilUsuarioEmpresa = new PerfilUsuarioEmpresa();

                //BUSCA informações sobre o USUÁRIO LOGADO
                dadosUsuarioEmpresa = negociosusuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Session["IdUsuarioLogado"]));

                dadosEPerfilUsuarioEmpresa.ListagemCategoriaAtividadesACotarDirecionada = ListagemGruposAtividades();
                dadosEPerfilUsuarioEmpresa.ListagemCategoriaAtividadesACotarAvulsa = ListagemGruposAtividades();
                dadosEPerfilUsuarioEmpresa.ListagemUnidadesProdutosACotarDirecionada = ListagemUnidadesDePesoEMedida();
                dadosEPerfilUsuarioEmpresa.ListagemUnidadesProdutosACotarAvulsa = ListagemUnidadesDePesoEMedida();
                dadosEPerfilUsuarioEmpresa.ListagemOutrasCidadesACotarDirecionada = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioEmpresa.ListagemOutrosEstadosACotarDirecionada = ListagemEstados();
                dadosEPerfilUsuarioEmpresa.ListagemOutrasCidadesACotarAvulsa = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioEmpresa.ListagemOutrosEstadosACotarAvulsa = ListagemEstados();
                dadosEPerfilUsuarioEmpresa.ListagemOutrasCidadesOutroEstadoACotarDirecionada = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioEmpresa.ListagemOutrasCidadesOutroEstadoACotarAvulsa = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioEmpresa.listagemDasCotacoesDirecionadasEnviadasPeloUsuario = ListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa();
                dadosEPerfilUsuarioEmpresa.listagemDasCotacoesAvulsasEnviadasPeloUsuario = ListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa();
                dadosEPerfilUsuarioEmpresa.listagemDasCotacoesDirecionadasRecebidasPeloUsuarioEmpresa = ListaDeCotacoesDirecionadasRecebidasPeloUsuarioEmpresa();
                dadosEPerfilUsuarioEmpresa.listagemDasCotacoesAvulsasRecebidasPeloUsuarioEmpresa = ListaDeCotacoesAvulsasRecebidasPeloUsuarioEmpresa();

                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);

                ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));

                if (dadosUsuarioEmpresa.VER_COTACAO_AVULSA)
                {
                    ViewBag.permissaoParaCotacaoAvulsa = "sim";
                }
                else
                {
                    ViewBag.permissaoParaCotacaoAvulsa = "nao";
                }

                TempData["CotacoesDirecionadasRecebidasPeloUsuarioEmpresa"] = (dadosEPerfilUsuarioEmpresa.listagemDasCotacoesDirecionadasRecebidasPeloUsuarioEmpresa.Count);
                TempData["CotacoesAvulsasRecebidasPeloUsuarioEmpresa"] = (dadosEPerfilUsuarioEmpresa.listagemDasCotacoesAvulsasRecebidasPeloUsuarioEmpresa.Count);

                return View(dadosEPerfilUsuarioEmpresa);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Acionada quando o Usuário Cotante deseja ver as ocorrências (Ações) relacionadas à COTAÇÃO selecionada no Grid
        public ActionResult AcompanharCotacoesEnviadasUsuarioEmpresa(string nmU, string nmE, string cloG, string cbaiR, string cciD, string cesT,
            string ccouT, string ccM)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                int idCotacaoMaster = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));
                string categoriaCotacao = "";
                string tipoDaCotacao = "";
                string tipoCotacao = "ec";
                string statusDaCotacao = "ABERTA";
                string dataEncerramento = "__/__/____";

                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresaCotante = new NCotacaoMasterUsuarioEmpresaService();
                cotacao_master_usuario_empresa dadosDaCotacaoMasterDoUsuarioEmpresaCotante = new cotacao_master_usuario_empresa();

                AcompanharCotacaoEnviada dadosASeremExibidosNaView = new AcompanharCotacaoEnviada();

                //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário COTANTE
                dadosDaCotacaoMasterDoUsuarioEmpresaCotante = negociosCotacaoMasterUsuarioEmpresaCotante.BuscarCotacaoMasterDoUsuarioEmpresa(idCotacaoMaster);

                if (dadosDaCotacaoMasterDoUsuarioEmpresaCotante != null)
                {
                    //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                    grupo_atividades_empresa dadosDaConsultaDoGrupoDeAtividades = new grupo_atividades_empresa();

                    dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.ID_GRUPO_ATIVIDADES;

                    grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                        negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                    if (dadosDoGrupoDeAtividadesPesquisado != null)
                    {
                        categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                    }

                    //Buscar dados do TIPO de COTAÇÃO
                    NTiposDeCotacaoService negociosTiposCotacao = new NTiposDeCotacaoService();
                    tipos_cotacao dadosDaConsultaDoTipoDeCotacao = new tipos_cotacao();

                    dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.ID_CODIGO_TIPO_COTACAO;

                    tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                    if (dadosDoTipoDaCotacao != null)
                    {
                        tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                    }

                    //Montando os dados a serem exibidos na View
                    dadosASeremExibidosNaView.NOME_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.NOME_COTACAO_USUARIO_EMPRESA;
                    dadosASeremExibidosNaView.CATEGORIA_COTACAO_ENVIADA = categoriaCotacao;
                    dadosASeremExibidosNaView.TIPO_COTACAO_ENVIADA = tipoDaCotacao;
                    dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                    dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_EMPRESA;

                    dadosASeremExibidosNaView.ListagemProdutosDaCotacaoEnviada = BuscarProdutosDaCotacaoEnviada(idCotacaoMaster); //Buscar PRODUTOS ligados a esta COTAÇÃO

                    dadosASeremExibidosNaView.ListagemFornecedoresDaCotacaoEnviada = BuscarFornecedoresDaCotacao(idCotacaoMaster, tipoCotacao); //Buscar Fornecedores que receberam esta COTAÇÃO

                    //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                    if (dadosASeremExibidosNaView.ListagemProdutosDaCotacaoEnviada.Count > 0)
                    {
                        if (dadosDaCotacaoMasterDoUsuarioEmpresaCotante.ID_CODIGO_STATUS_COTACAO != 3)
                        {
                            for (int i = 0; i < dadosASeremExibidosNaView.ListagemFornecedoresDaCotacaoEnviada.Count; i++)
                            {
                                if (dadosASeremExibidosNaView.ListagemFornecedoresDaCotacaoEnviada[i].respondido_empresa_fornecedor == "SIM")
                                {
                                    statusDaCotacao = "EM ANDAMENTO";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            statusDaCotacao = "ENCERRADA";
                            dataEncerramento = dadosDaCotacaoMasterDoUsuarioEmpresaCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                        }
                    }

                    dadosASeremExibidosNaView.STATUS_COTACAO_ENVIADA = statusDaCotacao;
                    dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_ENVIADA = dataEncerramento;
                }

                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));
                ViewBag.idCotacao = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));

                ViewBag.deOndeVim = "Início";
                ViewBag.ondeEstou = "Acompanhar Cotações";

                return View(dadosASeremExibidosNaView);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Acionada quando o Usuário Cotante deseja ver as ocorrências (Ações) relacionadas à COTAÇÃO selecionada no Grid
        public ActionResult AcompanharCotacoesDirecionadasRecebidasUsuarioEmpresa(string nmU, string nmE, string cloG, string cbaiR, string cciD,
            string cesT, string ccouT, string tC, string ccF, string ccM)
        {
            try
            {
                if (Session["IdUsuarioLogado"] != null)
                {
                    int idCotacaoFilha = Convert.ToInt32(MD5Crypt.Descriptografar(ccF));
                    int idCotacaoMaster = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));
                    int idUsuarioCotante = 0;
                    int tipoDesconto = 0;
                    decimal percentualDesconto = 0;
                    decimal valorTotalDoLote = 0;
                    string categoriaCotacao = "";
                    string tipoDaCotacao = "";
                    string statusDaCotacao = "";
                    string dataEncerramento = "__/__/____";
                    string tipoCotacao = MD5Crypt.Descriptografar(tC);
                    string notificacao = "";
                    string cotacaoRespondida = "Nao";

                    NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                    NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterCentralDeCompras = new NCotacaoMasterCentralDeComprasService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                    grupo_atividades_empresa dadosDaConsultaDoGrupoDeAtividades = new grupo_atividades_empresa();
                    NTiposDeCotacaoService negociosTiposCotacao = new NTiposDeCotacaoService();
                    tipos_cotacao dadosDaConsultaDoTipoDeCotacao = new tipos_cotacao();

                    ResponderCotacaoRecebida dadosASeremExibidosNaView = new ResponderCotacaoRecebida();

                    if (tipoCotacao == "uc")
                    {
                        //Cotação de USUÁRIO COTANTE
                        NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                        cotacao_filha_usuario_cotante dadosDaCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();

                        //Consultar dados da COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
                        dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                        dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                        dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;

                        cotacao_filha_usuario_cotante cotacaoASerRespondida =
                            negociosCotacaoFilhaUsuarioCotante.ConsultarDadosDaCotacaoFilhaUsuarioCotanteASerRespondida(dadosDaCotacaoFilhaUsuarioCotante);

                        if (cotacaoASerRespondida != null)
                        {
                            //Buscar dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                            cotacao_master_usuario_cotante dadosDaCotacaoMasterDoUsuarioCotante =
                                negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(cotacaoASerRespondida.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                            idUsuarioCotante = dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_USUARIO_COTANTE;
                            tipoDesconto = cotacaoASerRespondida.TIPO_DESCONTO;
                            percentualDesconto = cotacaoASerRespondida.PERCENTUAL_DESCONTO;

                            //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                            dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioCotante.ID_GRUPO_ATIVIDADES;

                            grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                                negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                            if (dadosDoGrupoDeAtividadesPesquisado != null)
                            {
                                categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                            }

                            //Buscar dados do TIPO de COTAÇÃO
                            dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_TIPO_COTACAO;

                            tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                            if (dadosDoTipoDaCotacao != null)
                            {
                                tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                            }

                            //Montando os dados a serem exibidos na View
                            dadosASeremExibidosNaView.NOME_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE;
                            dadosASeremExibidosNaView.CATEGORIA_COTACAO_RECEBIDA = categoriaCotacao;
                            dadosASeremExibidosNaView.TIPO_COTACAO_RECEBIDA = tipoDaCotacao;
                            dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                            dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE;

                            //Buscar PRODUTOS ligados a esta COTAÇÃO
                            dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida = BuscarProdutosDaCotacao(idCotacaoFilha, tipoCotacao, tipoDesconto, percentualDesconto);

                            //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO (OBS: Para desabilitar os labels de DESCONTO)
                            dadosASeremExibidosNaView.IDS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DA_COTACAO = BuscarIdsDosProdutosDaCotacao(idCotacaoFilha, tipoCotacao);

                            //Buscar FORNECEDORES que também receberam a COTAÇÃO
                            dadosASeremExibidosNaView.ListagemFornecedoresDaCotacao = BuscarFornecedoresDaCotacao(idCotacaoMaster, tipoCotacao);

                            //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                            if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 1)
                            {
                                statusDaCotacao = "ABERTA";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 2)
                            {
                                statusDaCotacao = "EM ANDAMENTO";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 3)
                            {
                                statusDaCotacao = "ENCERRADA";
                                dataEncerramento = dadosDaCotacaoMasterDoUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                            }

                            dadosASeremExibidosNaView.STATUS_COTACAO_RECEBIDA = statusDaCotacao;
                            dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_RECEBIDA = dataEncerramento;
                            dadosASeremExibidosNaView.OBSERVACAO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_COTANTE;

                            if (cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE != "")
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE;
                            }
                            else
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = "À Vista";
                            }

                            dadosASeremExibidosNaView.ID_TIPO_DE_FRETE = cotacaoASerRespondida.ID_TIPO_FRETE;
                            dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR_SEM_DESCONTO = cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE;

                            //Verifica se há PERCENTUAL de DESCONTO a aplicar
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = cotacaoASerRespondida.PERCENTUAL_DESCONTO.ToString();
                            }
                            else
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = "";
                            }

                            //Verifica se existe PERCENTUAL de DESCONTO declarado
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                if (cotacaoASerRespondida.TIPO_DESCONTO == 1)
                                {
                                    //SEM DESCONTO APLICADO
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO POR PRODUTO COTADO
                                    if (dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count > 0)
                                    {
                                        for (int i = 0; i < dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count; i++)
                                        {
                                            var valor_produto = Regex.Replace(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto, "[.]", "");
                                            valorTotalDoLote = (valorTotalDoLote + Convert.ToDecimal(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto));
                                        }
                                    }

                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", valorTotalDoLote);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 3)
                                {
                                    //DESCONTO APLICADO no LOTE
                                    var valorDesconto = ((percentualDesconto * cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE) / 100);
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", (cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE - valorDesconto));
                                }
                            }
                            else
                            {
                                dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE);
                            }

                            dadosASeremExibidosNaView.APLICACAO_DO_DESCONTO = cotacaoASerRespondida.TIPO_DESCONTO;

                            //Verifica se a COTAÇÃO já FOI RESPONDIDA
                            if (cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE == true)
                            {
                                cotacaoRespondida = "Sim";
                            }

                            /*
                             VER SE EXISTE ALGUM OUTRO CAMPO DA VIEW A SER CARREGADO AQUI...
                             */

                            //Buscar os TIPOS de FRETES cadastrados
                            dadosASeremExibidosNaView.ListagemTiposDeFrete = ListagemTiposDeFrete();

                            //Buscar o CHAT da COTAÇÃO
                            notificacao = ChecarSeHaDialogoEntreCotanteEFornecedorParaNotificacao(idCotacaoFilha, tipoCotacao);
                        }
                    }
                    else if (tipoCotacao == "ec")
                    {
                        //Cotação enviada por EMPRESAS
                        NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                        cotacao_filha_usuario_empresa dadosDaCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();

                        //Consultar dados da COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
                        dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                        dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                        dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;

                        cotacao_filha_usuario_empresa cotacaoASerRespondida =
                            negociosCotacaoFilhaUsuarioEmpresa.ConsultarDadosDaCotacaoFilhaUsuarioEmpresaASerRespondida(dadosDaCotacaoFilhaUsuarioEmpresa);

                        if (cotacaoASerRespondida != null)
                        {
                            //Buscar dados da COTAÇÃO MASTER do USUÁRIO EMPRESA
                            cotacao_master_usuario_empresa dadosDaCotacaoMasterDoUsuarioEmpresa =
                                negociosCotacaoMasterUsuarioEmpresa.BuscarCotacaoMasterDoUsuarioEmpresa(cotacaoASerRespondida.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                            idUsuarioCotante = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_EMPRESA;
                            tipoDesconto = cotacaoASerRespondida.TIPO_DESCONTO;
                            percentualDesconto = cotacaoASerRespondida.PERCENTUAL_DESCONTO;

                            //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                            dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_GRUPO_ATIVIDADES;

                            grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                                negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                            if (dadosDoGrupoDeAtividadesPesquisado != null)
                            {
                                categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                            }

                            //Buscar dados do TIPO de COTAÇÃO
                            dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_TIPO_COTACAO;

                            tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                            if (dadosDoTipoDaCotacao != null)
                            {
                                tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                            }

                            //Montando os dados a serem exibidos na View
                            dadosASeremExibidosNaView.NOME_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA;
                            dadosASeremExibidosNaView.CATEGORIA_COTACAO_RECEBIDA = categoriaCotacao;
                            dadosASeremExibidosNaView.TIPO_COTACAO_RECEBIDA = tipoDaCotacao;
                            dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                            dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_EMPRESA;

                            //Buscar PRODUTOS ligados a esta COTAÇÃO
                            dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida = BuscarProdutosDaCotacao(idCotacaoFilha, tipoCotacao, tipoDesconto, percentualDesconto);

                            //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO (OBS: Para desabilitar os labels de DESCONTO)
                            dadosASeremExibidosNaView.IDS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DA_COTACAO = BuscarIdsDosProdutosDaCotacao(idCotacaoFilha, tipoCotacao);

                            //Buscar FORNECEDORES que também receberam a COTAÇÃO
                            dadosASeremExibidosNaView.ListagemFornecedoresDaCotacao = BuscarFornecedoresDaCotacao(idCotacaoMaster, tipoCotacao);

                            //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                            if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 1)
                            {
                                statusDaCotacao = "ABERTA";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 2)
                            {
                                statusDaCotacao = "EM ANDAMENTO";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 3)
                            {
                                statusDaCotacao = "ENCERRADA";
                                dataEncerramento = dadosDaCotacaoMasterDoUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                            }

                            dadosASeremExibidosNaView.STATUS_COTACAO_RECEBIDA = statusDaCotacao;
                            dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_RECEBIDA = dataEncerramento;
                            dadosASeremExibidosNaView.OBSERVACAO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_EMPRESA;

                            if (cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA != "")
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA;
                            }
                            else
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = "À Vista";
                            }

                            dadosASeremExibidosNaView.ID_TIPO_DE_FRETE = cotacaoASerRespondida.ID_TIPO_FRETE;
                            dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR_SEM_DESCONTO = cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA;

                            //Verifica se há PERCENTUAL de DESCONTO a aplicar
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = cotacaoASerRespondida.PERCENTUAL_DESCONTO.ToString();
                            }
                            else
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = "";
                            }

                            //Verifica se existe PERCENTUAL de DESCONTO declarado
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                if (cotacaoASerRespondida.TIPO_DESCONTO == 1)
                                {
                                    //SEM DESCONTO APLICADO
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO POR PRODUTO COTADO
                                    if (dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count > 0)
                                    {
                                        for (int i = 0; i < dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count; i++)
                                        {
                                            var valor_produto = Regex.Replace(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto, "[.]", "");
                                            valorTotalDoLote = (valorTotalDoLote + Convert.ToDecimal(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto));
                                        }
                                    }

                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", valorTotalDoLote);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 3)
                                {
                                    //DESCONTO APLICADO no LOTE
                                    var valorDesconto = ((percentualDesconto * cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA) / 100);
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", (cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA - valorDesconto));
                                }
                            }
                            else
                            {
                                dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA);
                            }

                            dadosASeremExibidosNaView.APLICACAO_DO_DESCONTO = cotacaoASerRespondida.TIPO_DESCONTO;

                            //Verifica se a COTAÇÃO já FOI RESPONDIDA
                            if (cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA == true)
                            {
                                cotacaoRespondida = "Sim";
                            }

                            /*
                             VER SE EXISTE ALGUM OUTRO CAMPO DA VIEW A SER CARREGADO AQUI...
                             */

                            //Buscar os TIPOS de FRETES cadastrados
                            dadosASeremExibidosNaView.ListagemTiposDeFrete = ListagemTiposDeFrete();

                            //Buscar o CHAT da COTAÇÃO
                            notificacao = ChecarSeHaDialogoEntreCotanteEFornecedorParaNotificacao(idCotacaoFilha, tipoCotacao);
                        }
                    }
                    else if (tipoCotacao == "cc")
                    {
                        //Cotação enviada de CENTRAIS de COMPRAS
                        NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();
                        cotacao_filha_central_compras dadosCotacaoFilhaCentralDeCompras = new cotacao_filha_central_compras();

                        //Consultar dados da COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
                        dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                        dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                        dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = idCotacaoFilha;

                        cotacao_filha_central_compras cotacaoASerRespondida =
                            negociosCotacaoFilhaCentralDeCompras.ConsultarDadosDaCotacaoFilhaCentralDeComprasASerRespondida(dadosCotacaoFilhaCentralDeCompras);

                        /*
                        CONTINUAR DAQUI... 
                        */
                    }

                    //Montar ViewBag´s
                    ViewBag.idEmpresa = Sessao.IdEmpresaUsuario;
                    ViewBag.idUsuarioLogado = Sessao.IdUsuarioLogado;
                    ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                    ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                    ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                    ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                    ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                    ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                    ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));
                    ViewBag.idCotacaoFilha = idCotacaoFilha;
                    ViewBag.idCotacaoMaster = idCotacaoMaster;
                    ViewBag.idUsuarioCotante = idUsuarioCotante;
                    ViewBag.temaNaoTemNotificacao = notificacao;
                    ViewBag.tipoDesconto = tipoDesconto;
                    ViewBag.percentualDesconto = percentualDesconto;
                    ViewBag.tipoCotacaoRecebida = tipoCotacao;
                    ViewBag.cotacaoRespondida = cotacaoRespondida;
                    ViewBag.deOndeVim = "Início";
                    ViewBag.ondeEstou = "Acompanhar Cotações Recebidas";

                    return View(dadosASeremExibidosNaView);
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Acionada quando o Usuário Cotante deseja ver as ocorrências (Ações) relacionadas à COTAÇÃO selecionada no Grid
        public ActionResult AcompanharCotacoesAvulsasRecebidasUsuarioEmpresa(string nmU, string nmE, string cloG, string cbaiR, string cciD,
            string cesT, string ccouT, string tC, string ccF, string ccM)
        {
            try
            {
                if (Session["IdUsuarioLogado"] != null)
                {
                    int idCotacaoFilha = Convert.ToInt32(MD5Crypt.Descriptografar(ccF));
                    int idCotacaoMaster = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));
                    int idUsuarioCotante = 0;
                    int tipoDesconto = 0;
                    decimal percentualDesconto = 0;
                    decimal valorTotalDoLote = 0;
                    string categoriaCotacao = "";
                    string tipoDaCotacao = "";
                    string statusDaCotacao = "";
                    string dataEncerramento = "__/__/____";
                    string tipoCotacao = MD5Crypt.Descriptografar(tC);
                    string descricaoDoProduto = "";
                    string notificacao = "";
                    string cotacaoRespondida = "Nao";

                    NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                    NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    NItensCotacaoUsuarioEmpresaService negociosItensCotacaoUsuarioEmpresa = new NItensCotacaoUsuarioEmpresaService();
                    NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                    NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();

                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterCentralDeCompras = new NCotacaoMasterCentralDeComprasService();

                    itens_cotacao_usuario_cotante dadosItensCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();
                    itens_cotacao_usuario_empresa dadosItensCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();

                    cotacao_filha_usuario_cotante dadosDaCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();
                    cotacao_filha_usuario_empresa dadosDaCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();

                    NGruposAtividadesEmpresaProfissionalService negociosGrupoDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                    grupo_atividades_empresa dadosDaConsultaDoGrupoDeAtividades = new grupo_atividades_empresa();

                    NTiposDeCotacaoService negociosTiposCotacao = new NTiposDeCotacaoService();
                    tipos_cotacao dadosDaConsultaDoTipoDeCotacao = new tipos_cotacao();

                    ResponderCotacaoRecebida dadosASeremExibidosNaView = new ResponderCotacaoRecebida();

                    if (tipoCotacao == "uc")
                    {
                        //Cotação de USUÁRIO COTANTE
                        if (idCotacaoFilha == 0)
                        {
                            //Gerar a COTAÇÃO FILHA (COTACAO_FILHA_USUARIO_COTANTE) para o FORNECEDOR que se interessou e clicou na COTAÇÃO AVULSA
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = idCotacaoMaster;
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA = (int)Sessao.IdEmpresaUsuario;
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO = (int)Sessao.IdUsuarioLogado;
                            dadosDaCotacaoFilhaUsuarioCotante.ID_TIPO_FRETE = 2;
                            dadosDaCotacaoFilhaUsuarioCotante.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE = false;
                            dadosDaCotacaoFilhaUsuarioCotante.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = Convert.ToDateTime("01/01/1900");
                            dadosDaCotacaoFilhaUsuarioCotante.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE = "À Vista";
                            dadosDaCotacaoFilhaUsuarioCotante.TIPO_DESCONTO = 0;
                            dadosDaCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO = 0;
                            dadosDaCotacaoFilhaUsuarioCotante.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = 0;
                            dadosDaCotacaoFilhaUsuarioCotante.OBSERVACAO_COTACAO_USUARIO_COTANTE = null;
                            dadosDaCotacaoFilhaUsuarioCotante.COTACAO_FILHA_USUARIO_COTANTE_EDITADA = false;

                            cotacao_filha_usuario_cotante gerarCotacaoFilha =
                                negociosCotacaoFilhaUsuarioCotante.GerarCotacaoFilhaUsuarioCotante(dadosDaCotacaoFilhaUsuarioCotante);

                            if (gerarCotacaoFilha != null)
                            {
                                idCotacaoFilha = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;

                                //Buscar ITENS da COTAÇÃO do USUARIO COTANTE para gerar ITENS na COTAÇÃO FILHA
                                List<itens_cotacao_usuario_cotante> dadosItensDaCotacaoDoUsuarioCotante =
                                    negociosItensCotacaoUsuarioCotante.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoMaster);

                                //Gravar ITENS da COTAÇÃO FILHA gerada para o USUÁRIO que optou em responder esta COTAÇÃO AVULSA
                                if (dadosItensDaCotacaoDoUsuarioCotante.Count > 0)
                                {
                                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilhaNegociacaoUsuarioCotante =
                                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                                    itens_cotacao_filha_negociacao_usuario_cotante dadosItensCotacaoFilhaUsuarioCotante = new itens_cotacao_filha_negociacao_usuario_cotante();

                                    for (int a = 0; a < dadosItensDaCotacaoDoUsuarioCotante.Count; a++)
                                    {
                                        //Busca a DESCRIÇÃO ou NOME do PRODUTO
                                        produtos_servicos_empresa_profissional dadosDoProdutoDaCotacao = negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosItensDaCotacaoDoUsuarioCotante[a].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);
                                        if (dadosDoProdutoDaCotacao != null)
                                        {
                                            descricaoDoProduto = dadosDoProdutoDaCotacao.DESCRICAO_PRODUTO_SERVICO;
                                        }

                                        dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;
                                        dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE = dadosItensDaCotacaoDoUsuarioCotante[a].ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE;
                                        dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_TIPO_RESPOSTA_COTACAO = 1;
                                        dadosItensCotacaoFilhaUsuarioCotante.ID_CLASSIFICACAO_TIPO_ITENS_COTACAO = 1;
                                        dadosItensCotacaoFilhaUsuarioCotante.DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_COTANTE = descricaoDoProduto;
                                        dadosItensCotacaoFilhaUsuarioCotante.PRECO_ITENS_COTACAO_USUARIO_COTANTE = 0;
                                        dadosItensCotacaoFilhaUsuarioCotante.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE = Convert.ToDecimal(dadosItensDaCotacaoDoUsuarioCotante[a].QUANTIDADE_ITENS_COTACAO, new CultureInfo("en-US"));
                                        dadosItensCotacaoFilhaUsuarioCotante.PRODUTO_COTADO_USUARIO_COTANTE = false;
                                        dadosItensCotacaoFilhaUsuarioCotante.ITEM_COTACAO_FILHA_EDITADO = false;

                                        itens_cotacao_filha_negociacao_usuario_cotante gravandoItensDaCotacaoFilhaUsuarioCotante =
                                            negociosItensCotacaoFilhaNegociacaoUsuarioCotante.GravarItensDaCotacaoFilhaUsuarioCotante(dadosItensCotacaoFilhaUsuarioCotante);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Consultar dados da COTAÇÃO FILHA, enviada como AVULSA pelo USUÁRIO COTANTE
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                            dadosDaCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                        }

                        cotacao_filha_usuario_cotante cotacaoASerRespondida =
                            negociosCotacaoFilhaUsuarioCotante.ConsultarDadosDaCotacaoFilhaUsuarioCotanteASerRespondida(dadosDaCotacaoFilhaUsuarioCotante);

                        if (cotacaoASerRespondida != null)
                        {
                            //Buscar dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                            cotacao_master_usuario_cotante dadosDaCotacaoMasterDoUsuarioCotante =
                                negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(cotacaoASerRespondida.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                            idUsuarioCotante = dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_USUARIO_COTANTE;
                            tipoDesconto = cotacaoASerRespondida.TIPO_DESCONTO;
                            percentualDesconto = cotacaoASerRespondida.PERCENTUAL_DESCONTO;

                            //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                            dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioCotante.ID_GRUPO_ATIVIDADES;

                            grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                                negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                            if (dadosDoGrupoDeAtividadesPesquisado != null)
                            {
                                categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                            }

                            //Buscar dados do TIPO de COTAÇÃO
                            dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_TIPO_COTACAO;

                            tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                            if (dadosDoTipoDaCotacao != null)
                            {
                                tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                            }

                            //Montando os dados a serem exibidos na View
                            dadosASeremExibidosNaView.NOME_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE;
                            dadosASeremExibidosNaView.CATEGORIA_COTACAO_RECEBIDA = categoriaCotacao;
                            dadosASeremExibidosNaView.TIPO_COTACAO_RECEBIDA = tipoDaCotacao;
                            dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                            dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE;

                            //Buscar PRODUTOS ligados a esta COTAÇÃO
                            dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida = BuscarProdutosDaCotacao(idCotacaoFilha, tipoCotacao, tipoDesconto, percentualDesconto);

                            //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO (OBS: Para desabilitar os labels de DESCONTO)
                            dadosASeremExibidosNaView.IDS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DA_COTACAO = BuscarIdsDosProdutosDaCotacao(idCotacaoFilha, tipoCotacao);

                            //Buscar FORNECEDORES que também receberam a COTAÇÃO
                            dadosASeremExibidosNaView.ListagemFornecedoresDaCotacao = BuscarFornecedoresDaCotacao(idCotacaoMaster, tipoCotacao);

                            //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                            if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 1)
                            {
                                statusDaCotacao = "ABERTA";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 2)
                            {
                                statusDaCotacao = "EM ANDAMENTO";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO == 3)
                            {
                                statusDaCotacao = "ENCERRADA";
                                dataEncerramento = dadosDaCotacaoMasterDoUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                            }

                            dadosASeremExibidosNaView.STATUS_COTACAO_RECEBIDA = statusDaCotacao;
                            dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_RECEBIDA = dataEncerramento;
                            dadosASeremExibidosNaView.OBSERVACAO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_COTANTE;

                            //Buscar os TIPOS de FRETES cadastrados
                            dadosASeremExibidosNaView.ListagemTiposDeFrete = ListagemTiposDeFrete();

                            //Buscar o CHAT da COTAÇÃO
                            notificacao = ChecarSeHaDialogoEntreCotanteEFornecedorParaNotificacao(idCotacaoFilha, tipoCotacao);

                            if (cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE != "")
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE;
                            }
                            else
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = "À Vista";
                            }

                            dadosASeremExibidosNaView.ID_TIPO_DE_FRETE = cotacaoASerRespondida.ID_TIPO_FRETE;
                            dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR_SEM_DESCONTO = cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE;

                            //Verifica se há PERCENTUAL de DESCONTO a aplicar
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = cotacaoASerRespondida.PERCENTUAL_DESCONTO.ToString();
                            }
                            else
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = "";
                            }

                            //Verifica se existe PERCENTUAL de DESCONTO declarado
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                if (cotacaoASerRespondida.TIPO_DESCONTO == 0)
                                {
                                    //SEM DESCONTO APLICADO
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO POR PRODUTO COTADO
                                    if (dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count > 0)
                                    {
                                        for (int i = 0; i < dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count; i++)
                                        {
                                            var valor_produto = Regex.Replace(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto, "[.]", "");
                                            valorTotalDoLote = (valorTotalDoLote + Convert.ToDecimal(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto));
                                        }
                                    }

                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", valorTotalDoLote);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 3)
                                {
                                    //DESCONTO APLICADO no LOTE
                                    var valorDesconto = ((percentualDesconto * cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE) / 100);
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", (cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE - valorDesconto));
                                }
                            }
                            else
                            {
                                dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE);
                            }

                            dadosASeremExibidosNaView.APLICACAO_DO_DESCONTO = cotacaoASerRespondida.TIPO_DESCONTO;

                            //Verifica se a COTAÇÃO já FOI RESPONDIDA
                            if (cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE == true)
                            {
                                cotacaoRespondida = "Sim";
                            }
                        }
                    }
                    else if (tipoCotacao == "ec")
                    {
                        //Cotação de USUÁRIO EMPRESA
                        if (idCotacaoFilha == 0)
                        {
                            /*
                            TESTAR A FUNCIONALIDADE...
                            */

                            //Gerar a COTAÇÃO FILHA (COTACAO_FILHA_USUARIO_COTANTE) para o FORNECEDOR que se interessou e clicou na COTAÇÃO AVULSA
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA = idCotacaoMaster;
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA = (int)Sessao.IdEmpresaUsuario;
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO = (int)Sessao.IdUsuarioLogado;
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_TIPO_FRETE = 2;
                            dadosDaCotacaoFilhaUsuarioEmpresa.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA = false;
                            dadosDaCotacaoFilhaUsuarioEmpresa.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = Convert.ToDateTime("01/01/1900");
                            dadosDaCotacaoFilhaUsuarioEmpresa.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA = "À Vista";
                            dadosDaCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO = 0;
                            dadosDaCotacaoFilhaUsuarioEmpresa.PERCENTUAL_DESCONTO = 0;
                            dadosDaCotacaoFilhaUsuarioEmpresa.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = 0;
                            dadosDaCotacaoFilhaUsuarioEmpresa.OBSERVACAO_COTACAO_USUARIO_EMPRESA = null;
                            dadosDaCotacaoFilhaUsuarioEmpresa.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA = false;

                            cotacao_filha_usuario_empresa gerarCotacaoFilha =
                                negociosCotacaoFilhaUsuarioEmpresa.GerarCotacaoFilhaUsuarioEmpresa(dadosDaCotacaoFilhaUsuarioEmpresa);

                            if (gerarCotacaoFilha != null)
                            {
                                idCotacaoFilha = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA;

                                //Buscar ITENS da COTAÇÃO do USUARIO COTANTE para gerar ITENS na COTAÇÃO FILHA
                                List<itens_cotacao_usuario_empresa> dadosItensDaCotacaoDoUsuarioEmpresa =
                                    negociosItensCotacaoUsuarioEmpresa.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoMaster);

                                //Gravar ITENS da COTAÇÃO FILHA gerada para o USUÁRIO que optou em responder esta COTAÇÃO AVULSA
                                if (dadosItensDaCotacaoDoUsuarioEmpresa.Count > 0)
                                {
                                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilhaNegociacaoUsuarioEmpresa =
                                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                                    itens_cotacao_filha_negociacao_usuario_empresa dadosItensCotacaoFilhaUsuarioEmpresa = new itens_cotacao_filha_negociacao_usuario_empresa();

                                    for (int a = 0; a < dadosItensDaCotacaoDoUsuarioEmpresa.Count; a++)
                                    {
                                        //Busca a DESCRIÇÃO ou NOME do PRODUTO
                                        produtos_servicos_empresa_profissional dadosDoProdutoDaCotacao = negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosItensDaCotacaoDoUsuarioEmpresa[a].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);
                                        if (dadosDoProdutoDaCotacao != null)
                                        {
                                            descricaoDoProduto = dadosDoProdutoDaCotacao.DESCRICAO_PRODUTO_SERVICO;
                                        }

                                        dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = dadosItensDaCotacaoDoUsuarioEmpresa[a].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_TIPO_RESPOSTA_COTACAO = 1;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.ID_CLASSIFICACAO_TIPO_ITENS_COTACAO = 1;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_EMPRESA = descricaoDoProduto;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = 0;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(dadosItensDaCotacaoDoUsuarioEmpresa[a].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA, new CultureInfo("en-US"));
                                        dadosItensCotacaoFilhaUsuarioEmpresa.PRODUTO_COTADO_USUARIO_EMPRESA = false;
                                        dadosItensCotacaoFilhaUsuarioEmpresa.ITEM_COTACAO_FILHA_EDITADO = false;

                                        itens_cotacao_filha_negociacao_usuario_empresa gravandoItensDaCotacaoFilhaUsuarioEmpresa =
                                            negociosItensCotacaoFilhaNegociacaoUsuarioEmpresa.GravarItensDaCotacaoFilhaUsuarioEmpresa(dadosItensCotacaoFilhaUsuarioEmpresa);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Consultar dados da COTAÇÃO FILHA, enviada como AVULSA pelo USUÁRIO COTANTE
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                            dadosDaCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                        }

                        cotacao_filha_usuario_empresa cotacaoASerRespondida =
                            negociosCotacaoFilhaUsuarioEmpresa.ConsultarDadosDaCotacaoFilhaUsuarioEmpresaASerRespondida(dadosDaCotacaoFilhaUsuarioEmpresa);

                        if (cotacaoASerRespondida != null)
                        {
                            //Buscar dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                            cotacao_master_usuario_empresa dadosDaCotacaoMasterDoUsuarioEmpresa =
                                negociosCotacaoMasterUsuarioEmpresa.BuscarCotacaoMasterDoUsuarioEmpresa(cotacaoASerRespondida.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                            idUsuarioCotante = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_USUARIO;
                            tipoDesconto = cotacaoASerRespondida.TIPO_DESCONTO;
                            percentualDesconto = cotacaoASerRespondida.PERCENTUAL_DESCONTO;

                            //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                            dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_GRUPO_ATIVIDADES;

                            grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                                negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                            if (dadosDoGrupoDeAtividadesPesquisado != null)
                            {
                                categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                            }

                            //Buscar dados do TIPO de COTAÇÃO
                            dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_TIPO_COTACAO;

                            tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                            if (dadosDoTipoDaCotacao != null)
                            {
                                tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                            }

                            //Montando os dados a serem exibidos na View
                            dadosASeremExibidosNaView.NOME_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA;
                            dadosASeremExibidosNaView.CATEGORIA_COTACAO_RECEBIDA = categoriaCotacao;
                            dadosASeremExibidosNaView.TIPO_COTACAO_RECEBIDA = tipoDaCotacao;
                            dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                            dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_RECEBIDA = dadosDaCotacaoMasterDoUsuarioEmpresa.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_EMPRESA;

                            //Buscar PRODUTOS ligados a esta COTAÇÃO
                            dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida = BuscarProdutosDaCotacao(idCotacaoFilha, tipoCotacao, tipoDesconto, percentualDesconto);

                            //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO (OBS: Para desabilitar os labels de DESCONTO)
                            dadosASeremExibidosNaView.IDS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DA_COTACAO = BuscarIdsDosProdutosDaCotacao(idCotacaoFilha, tipoCotacao);

                            //Buscar FORNECEDORES que também receberam a COTAÇÃO
                            dadosASeremExibidosNaView.ListagemFornecedoresDaCotacao = BuscarFornecedoresDaCotacao(idCotacaoMaster, tipoCotacao);

                            //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                            if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 1)
                            {
                                statusDaCotacao = "ABERTA";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 2)
                            {
                                statusDaCotacao = "EM ANDAMENTO";
                            }
                            else if (dadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO == 3)
                            {
                                statusDaCotacao = "ENCERRADA";
                                dataEncerramento = dadosDaCotacaoMasterDoUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString();
                            }

                            dadosASeremExibidosNaView.STATUS_COTACAO_RECEBIDA = statusDaCotacao;
                            dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_RECEBIDA = dataEncerramento;
                            dadosASeremExibidosNaView.OBSERVACAO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.OBSERVACAO_COTACAO_USUARIO_EMPRESA;

                            //Buscar os TIPOS de FRETES cadastrados
                            dadosASeremExibidosNaView.ListagemTiposDeFrete = ListagemTiposDeFrete();

                            //Buscar o CHAT da COTAÇÃO
                            notificacao = ChecarSeHaDialogoEntreCotanteEFornecedorParaNotificacao(idCotacaoFilha, tipoCotacao);

                            if (cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA != "")
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = cotacaoASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA;
                            }
                            else
                            {
                                dadosASeremExibidosNaView.CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE = "À Vista";
                            }

                            dadosASeremExibidosNaView.ID_TIPO_DE_FRETE = cotacaoASerRespondida.ID_TIPO_FRETE;
                            dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR_SEM_DESCONTO = cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA;

                            //Verifica se há PERCENTUAL de DESCONTO a aplicar
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = cotacaoASerRespondida.PERCENTUAL_DESCONTO.ToString();
                            }
                            else
                            {
                                dadosASeremExibidosNaView.PERCENTUAL_DESCONTO_ASER_APLICADO = "";
                            }

                            //Verifica se existe PERCENTUAL de DESCONTO declarado
                            if (cotacaoASerRespondida.PERCENTUAL_DESCONTO > 0)
                            {
                                if (cotacaoASerRespondida.TIPO_DESCONTO == 0)
                                {
                                    //SEM DESCONTO APLICADO
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO POR PRODUTO COTADO
                                    if (dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count > 0)
                                    {
                                        for (int i = 0; i < dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida.Count; i++)
                                        {
                                            var valor_produto = Regex.Replace(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto, "[.]", "");
                                            valorTotalDoLote = (valorTotalDoLote + Convert.ToDecimal(dadosASeremExibidosNaView.ListagemProdutosDaCotacaoRecebida[i].total_por_produto));
                                        }
                                    }

                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", valorTotalDoLote);
                                }
                                else if (cotacaoASerRespondida.TIPO_DESCONTO == 3)
                                {
                                    //DESCONTO APLICADO no LOTE
                                    var valorDesconto = ((percentualDesconto * cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA) / 100);
                                    dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", (cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA - valorDesconto));
                                }
                            }
                            else
                            {
                                dadosASeremExibidosNaView.VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR = string.Format("{0:0,0.00}", cotacaoASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA);
                            }

                            dadosASeremExibidosNaView.APLICACAO_DO_DESCONTO = cotacaoASerRespondida.TIPO_DESCONTO;

                            //Verifica se a COTAÇÃO já FOI RESPONDIDA
                            if (cotacaoASerRespondida.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA == true)
                            {
                                cotacaoRespondida = "Sim";
                            }
                        }
                    }
                    else if (tipoCotacao == "cc")
                    {
                        ////Cotação enviada de CENTRAIS de COMPRAS
                        //NCotacaoFilhaCentralDeCompras negociosCotacaoFilhaCentralDeCompras = new NCotacaoFilhaCentralDeCompras();
                        //cotacao_filha_central_compras dadosCotacaoFilhaCentralDeCompras = new cotacao_filha_central_compras();

                        ////Consultar dados da COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
                        //dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                        //dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                        //dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = idCotacaoFilha;

                        //cotacao_filha_central_compras cotacaoASerRespondida =
                        //    negociosCotacaoFilhaCentralDeCompras.ConsultarDadosDaCotacaoFilhaCentralDeComprasASerRespondida(dadosCotacaoFilhaCentralDeCompras);

                        /*
                        CONTINUAR DAQUI... 
                        */
                    }

                    //Montar ViewBag´s
                    ViewBag.idEmpresa = Sessao.IdEmpresaUsuario;
                    ViewBag.idUsuarioLogado = Sessao.IdUsuarioLogado;
                    ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                    ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                    ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                    ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                    ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                    ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                    ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));
                    ViewBag.idCotacaoFilha = idCotacaoFilha;
                    ViewBag.idCotacaoMaster = idCotacaoMaster;
                    ViewBag.idUsuarioCotante = idUsuarioCotante;
                    ViewBag.temaNaoTemNotificacao = notificacao;
                    ViewBag.tipoDesconto = tipoDesconto;
                    ViewBag.percentualDesconto = percentualDesconto;
                    ViewBag.tipoCotacaoRecebida = tipoCotacao;
                    ViewBag.cotacaoRespondida = cotacaoRespondida;
                    ViewBag.deOndeVim = "Início";
                    ViewBag.ondeEstou = "Acompanhar Cotações Recebidas";

                    return View(dadosASeremExibidosNaView);
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        //Acionada em caso de primeiro acesso, onde o usuário configura Atividades, Assinatura e modo de Pagamento,
        //para o uso do sistema
        public ActionResult Configuracoes(int tpL, string nmU, string nmE)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                //Carrega listas a serem exibidas na View
                CadastroEmpresa empresa = new CadastroEmpresa();

                var listaGruposAtividades = ListagemGruposAtividades();

                ViewBag.tipoLogin = tpL;
                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);

                empresa.ListagemGruposAtividadeEmpresa = listaGruposAtividades;

                return View(empresa);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Carrega os Grupos de Atividades
        private static List<SelectListItem> ListagemGruposAtividades()
        {
            //Buscar os Grupos de Atividades da empresariais
            NGruposAtividadesEmpresaProfissionalService negocioGruposAtividadesEmpresaProfissional =
                new NGruposAtividadesEmpresaProfissionalService();
            List<grupo_atividades_empresa> listaGruposAtividades =
                negocioGruposAtividadesEmpresaProfissional.ListaGruposAtividadesEmpresaProfissional();

            List<SelectListItem> listGruposAtividades = new List<SelectListItem>();

            listGruposAtividades.Add(new SelectListItem { Text = "Selecione...", Value = "" });

            foreach (var gruposAtividades in listaGruposAtividades)
            {
                listGruposAtividades.Add(new SelectListItem
                {
                    Text = gruposAtividades.DESCRICAO_ATIVIDADE,
                    Value = gruposAtividades.ID_GRUPO_ATIVIDADES.ToString()
                });
            }

            return listGruposAtividades;
        }

        //Carrega as siglas relacionadas às Unidades de peso e medida para os produtos da Cotação
        private static List<SelectListItem> ListagemUnidadesDePesoEMedida()
        {
            //Buscar Unidades de peso e medida
            NUnidadesProdutosService negocioUnidadesProdutos = new NUnidadesProdutosService();
            List<unidades_produtos> listaUnidadesProdutos = negocioUnidadesProdutos.ListaUnidadesProdutos();

            List<SelectListItem> listUnidades = new List<SelectListItem>();

            listUnidades.Add(new SelectListItem { Text = "...", Value = "0" });

            foreach (var unidadesProdutos in listaUnidadesProdutos)
            {
                listUnidades.Add(new SelectListItem
                {
                    Text = unidadesProdutos.DESCRICAO_UNIDADE_PRODUTO,
                    Value = unidadesProdutos.ID_CODIGO_UNIDADE_PRODUTO.ToString()
                });
            }

            return listUnidades;
        }

        //Carrega a lista de Estados (Obs: No momento carregrá todos os estados brasileiros.)
        private List<SelectListItem> ListagemEstados()
        {
            //Buscar lista de Estados brasileiros
            NEstadosService negocioEstados = new NEstadosService();

            List<estados_empresa_usuario> listaEstados = negocioEstados.ListaEstados();

            List<SelectListItem> listEstados = new List<SelectListItem>();

            listEstados.Add(new SelectListItem { Text = "Selecione outro Estado", Value = "" });

            foreach (var grupoEstados in listaEstados)
            {
                listEstados.Add(new SelectListItem
                {
                    Text = grupoEstados.UF_EMPRESA_USUARIO,
                    Value = grupoEstados.ID_ESTADOS_EMPRESA_USUARIO.ToString()
                });
            }

            return listEstados;
        }

        //Carrega lista de Cidades por Estado selecionado
        public JsonResult BuscaCidadesEstadoSelecionado(int doEstado)
        {
            //Buscar as Cidades do referido Estado
            NEnderecoEmpresaUsuarioService negocioEnderecosEmpresaUsuario = new NEnderecoEmpresaUsuarioService();
            List<cidades_empresa_usuario> listaCidadesPorEstado = negocioEnderecosEmpresaUsuario.ConsultarEMontarListaDeCidadesPorEstado(doEstado);

            List<SelectListItem> listCidadesPorEstado = new List<SelectListItem>();

            listCidadesPorEstado.Add(new SelectListItem { Text = "Selecione outra Cidade", Value = "" });
            listCidadesPorEstado.Add(new SelectListItem { Text = "Todas as cidades do Estado", Value = "1000000" });

            foreach (var cidadesDoEstado in listaCidadesPorEstado)
            {
                listCidadesPorEstado.Add(new SelectListItem
                {
                    Text = cidadesDoEstado.CIDADE_EMPRESA_USUARIO,
                    Value = cidadesDoEstado.ID_CIDADE_EMPRESA_USUARIO.ToString()
                });
            }

            return Json(listCidadesPorEstado, JsonRequestBehavior.AllowGet);
        }

        //Carrega Lista de Cidades por Estado
        private static List<SelectListItem> ListagemCidadesPorEstado(int idEstado)
        {
            //Buscar as Cidades dos Estados
            NEnderecoEmpresaUsuarioService negocioEnderecosEmpresaUsuario = new NEnderecoEmpresaUsuarioService();
            List<cidades_empresa_usuario> listaCidadesPorEstado = negocioEnderecosEmpresaUsuario.ConsultarEMontarListaDeCidadesPorEstado(idEstado);

            List<SelectListItem> listCidadesPorEstado = new List<SelectListItem>();

            listCidadesPorEstado.Add(new SelectListItem { Text = "Selecione outra Cidade", Value = "" });
            listCidadesPorEstado.Add(new SelectListItem { Text = "Todas as cidades do Estado", Value = "1000000" });

            foreach (var cidadesDoEstado in listaCidadesPorEstado)
            {
                listCidadesPorEstado.Add(new SelectListItem
                {
                    Text = cidadesDoEstado.CIDADE_EMPRESA_USUARIO,
                    Value = cidadesDoEstado.ID_CIDADE_EMPRESA_USUARIO.ToString()
                });
            }

            return listCidadesPorEstado;
        }

        //Autocomplete Itens/Produtos para Cotações
        public JsonResult AutoCompleteItensProdutosParaCotacoes(string term, int idCategoriaAtividades, string produtosCotacao, string servicosCotacao)
        {
            //Buscar lista de atividades ligadas ao grupo de atividades da empresa, conforme tipo setado (Produto / Serviço)
            NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();

            List<produtos_servicos_empresa_profissional> listaAtividadesEProdutosEmpresa =
                negocioProdutosServicosEmpresaProfissional.ListaAtividadesEmpresaProfissional(idCategoriaAtividades);

            if (produtosCotacao != "" && servicosCotacao == "")
            {
                lista = (from t in listaAtividadesEProdutosEmpresa
                         where t.DESCRICAO_PRODUTO_SERVICO.ToLower().Contains(term.ToLower()) && t.TIPO_PRODUTO_SERVICO == produtosCotacao
                         select new { t.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS, t.DESCRICAO_PRODUTO_SERVICO }).Distinct().ToList();
            }
            else if (servicosCotacao != "" && produtosCotacao == "")
            {
                lista = (from t in listaAtividadesEProdutosEmpresa
                         where t.DESCRICAO_PRODUTO_SERVICO.ToLower().Contains(term.ToLower()) && t.TIPO_PRODUTO_SERVICO == servicosCotacao
                         select new { t.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS, t.DESCRICAO_PRODUTO_SERVICO }).Distinct().ToList();
            }
            else if (produtosCotacao != "" && servicosCotacao != "")
            {
                lista = (from t in listaAtividadesEProdutosEmpresa
                         where t.DESCRICAO_PRODUTO_SERVICO.ToLower().Contains(term.ToLower()) && (t.TIPO_PRODUTO_SERVICO == produtosCotacao || t.TIPO_PRODUTO_SERVICO == servicosCotacao)
                         select new { t.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS, t.DESCRICAO_PRODUTO_SERVICO }).Distinct().ToList();
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //Autocomplete Fabricantes & Marcas
        public JsonResult AutoCompleteFabricantesEMarcas(string term)
        {
            //Buscar lista de Fabricantes & Marcas registrados no Sistema
            NEmpresasFabricantesMarcasService negocioEmpresasFabricantesMarcas = new NEmpresasFabricantesMarcasService();

            List<ListaDeEmpresasFabricantesEMarcasViewModel> listaEmpresasFabricantesMarcas =
                negocioEmpresasFabricantesMarcas.ListaDeFabricantesEMarcas(term);

            lista = (from t in listaEmpresasFabricantesMarcas
                     where t.DESCRICAO_EMPRESA_FABRICANTE_MARCAS.ToLower().Contains(term.ToLower())
                     select new { t.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS, t.DESCRICAO_EMPRESA_FABRICANTE_MARCAS }).Distinct().ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //Carrega lista de Fornecedores para Cotações Direcionadas
        public JsonResult BuscaFornecedoresCotacaoDirecionada(int categoriaASerCotada, int quantFornecedores, int tipoPorCidade, int tipoPorUF, int idOutraCidade, int idOutroEstado, int idOutraCidadeOutroEstado,
            int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            try
            {
                NFornecedoresService negocioFornecedores = new NFornecedoresService();
                List<FornecedoresASeremCotadosDirecionada> listaFornecedoresASeremCotados = new List<FornecedoresASeremCotadosDirecionada>();
                List<empresa_usuario> listFornecedores = new List<empresa_usuario>();

                if ((tipoPorCidade == 1) && (tipoPorUF == 1))
                {
                    //Buscar fornecedores na MINHA CIDADE e no MEU ESTADO
                    listFornecedores = negocioFornecedores.BuscarFornecedoresNaMinhaCidadeENoMeuEstadoDirecionada(categoriaASerCotada, quantFornecedores, idCidadeCotante, idEstadoCotante,
                        idPaisCotante);
                }
                else if ((tipoPorCidade == 2) && (tipoPorUF == 1))
                {
                    //Buscar fornecedores em OUTRA CIDADE e no MEU ESTADO
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutraCidadeENoMeuEstadoDirecionada(categoriaASerCotada, quantFornecedores, idOutraCidade, idEstadoCotante,
                        idPaisCotante);
                }
                else if ((tipoPorCidade == 0) && (tipoPorUF == 2))
                {
                    //Buscar fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA)
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutroEstadoDirecionada(categoriaASerCotada, quantFornecedores, idOutroEstado, idOutraCidadeOutroEstado,
                        idPaisCotante);
                }
                else if ((tipoPorCidade == 0) && (tipoPorUF == 3))
                {
                    //Buscar fornecedores de TODO o PAÍS
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmTodoOPaisDirecionada(categoriaASerCotada, quantFornecedores, idPaisCotante);
                }

                if (listFornecedores != null)
                {
                    //Montagem da lista de Fornecedores
                    for (int a = 0; a < listFornecedores.Count; a++)
                    {
                        listaFornecedoresASeremCotados.Add(
                            new FornecedoresASeremCotadosDirecionada(
                                listFornecedores[a].ID_CODIGO_EMPRESA,
                                listFornecedores[a].ID_GRUPO_ATIVIDADES,
                                listFornecedores[a].NOME_FANTASIA_EMPRESA,
                                listFornecedores[a].enderecos_empresa_usuario.cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO,
                                listFornecedores[a].enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.UF_EMPRESA_USUARIO,
                                listFornecedores[a].LOGOMARCA_EMPRESA_USUARIO,
                                listFornecedores[a].ID_CODIGO_ENDERECO_EMPRESA_USUARIO,
                                listFornecedores[a].usuario_empresa.ElementAt(0).ID_CODIGO_USUARIO,
                                listFornecedores[a].usuario_empresa.ElementAt(0).NICK_NAME_USUARIO
                                )
                            );
                    }
                }

                return Json(listaFornecedoresASeremCotados, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Traz a lista de Vendedores ligados à EMPRESA cotada que satisfaçam os requisitos pré-determinados para a busca
        //Corrigir para evitar sobrepor no DropDown
        public JsonResult BuscaUsuariosOuVendedoresDaEmpresa(int idDaEmpresa)
        {
            try
            {
                NUsuarioEmpresaService negociosUsuario = new NUsuarioEmpresaService();
                usuario_empresa usuariosRecebedores = new usuario_empresa();

                List<SelectListItem> listaUsuariosVendedores = new List<SelectListItem>();

                usuariosRecebedores.ID_CODIGO_EMPRESA = idDaEmpresa;

                List<usuario_empresa> listUsuariosRecebedores = negociosUsuario.ConsultarUsuariosLigadosAEmpresa(usuariosRecebedores);

                if (listUsuariosRecebedores != null)
                {
                    //Montagem da lista de Vendedores da Empresa Cotada
                    foreach (var vendedores in listUsuariosRecebedores)
                    {
                        listaUsuariosVendedores.Add(new SelectListItem
                        {
                            Text = vendedores.NICK_NAME_USUARIO,
                            Value = vendedores.ID_CODIGO_USUARIO.ToString()
                        });
                    }
                }

                return Json(listaUsuariosVendedores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega lista de Fornecedores para Cotações Avulsa
        public JsonResult BuscaFornecedoresCotacaoAvulsa(int categoriaASerCotada, int tipoPorCidade, int tipoPorUF, int idOutraCidade, int idOutroEstado, int idOutraCidadeOutroEstado,
            int idCidadeCotante, int idEstadoCotante, int idPaisCotante)
        {
            try
            {
                NFornecedoresService negocioFornecedores = new NFornecedoresService();
                List<FornecedoresASeremCotadosDirecionada> listaFornecedoresASeremCotados = new List<FornecedoresASeremCotadosDirecionada>();
                List<empresa_usuario> listFornecedores = new List<empresa_usuario>();

                if ((tipoPorCidade == 1) && (tipoPorUF == 1))
                {
                    //Buscar fornecedores na MINHA CIDADE e no MEU ESTADO
                    listFornecedores = negocioFornecedores.BuscarFornecedoresNaMinhaCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idCidadeCotante, idEstadoCotante, idPaisCotante);
                }
                else if ((tipoPorCidade == 2) && (tipoPorUF == 1))
                {
                    //Buscar fornecedores em OUTRA CIDADE e no MEU ESTADO
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutraCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idOutraCidade, idEstadoCotante, idPaisCotante);
                }
                else if ((tipoPorCidade == 0) && (tipoPorUF == 2))
                {
                    //Buscar fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA)
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutroEstadoAvulsa(categoriaASerCotada, idOutroEstado, idOutraCidadeOutroEstado, idPaisCotante);
                }
                else if ((tipoPorCidade == 0) && (tipoPorUF == 3))
                {
                    //Buscar fornecedores de TODO o PAÍS
                    listFornecedores = negocioFornecedores.BuscarFornecedoresEmTodoOPaisAvulsa(categoriaASerCotada, idPaisCotante);
                }

                //Resultado a ser retornado
                var resultado = new
                {
                    quantidadeFornecedores = listFornecedores.Count
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Processo para o envio da COTAÇÃO DIRECIONADA
        [WebMethod]
        public ActionResult EnviarCotacaoDirecionadaUsuarioEmpresa(string nomeDaCotacao, string dataEncerramentoDaCotacaoDirecionada, string idCategoriaDaAtividadeACotar, string tipoCotacaoProdutoDirecionada,
            string tipoCotacaoServicoDirecionada, string idsProdutosServicos, string descricaoDosProdutosOuServicosASeremCotados, string idsUnidadesDeMedida, string idsFornecedores,
            string idsUsuariosVendedores, string quantidadeCadaItem, string idsMarcasFabricantes, string descricaoMarcasFabricantes)
        {
            //Definindo as variáveis pertinentes ao método em questão
            string cotacaoEnviada = "nok";
            string[] listaIDsProdutosServicos, listaDescricaoDosProdutosOuServicosASeremCotados, listaIDsUnidadesDeMedida, listaIDsFornecedores, listaIDsUsuariosVendedores, listaQuantidadeCadaItem,
                listaIDsMarcasFabricantes, listaDescricaoMarcasFabricantes;

            ArrayList listaIDsItensCotacaoUsuarioEmpresa = new ArrayList();
            ArrayList listaEmailsVendedoresQueReceberaoACotacao = new ArrayList();

            listaIDsProdutosServicos = idsProdutosServicos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoDosProdutosOuServicosASeremCotados = descricaoDosProdutosOuServicosASeremCotados.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsUnidadesDeMedida = idsUnidadesDeMedida.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsFornecedores = idsFornecedores.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsUsuariosVendedores = idsUsuariosVendedores.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaQuantidadeCadaItem = quantidadeCadaItem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsMarcasFabricantes = idsMarcasFabricantes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoMarcasFabricantes = descricaoMarcasFabricantes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Gerar a COTAÇÃO
            try
            {
                //Buscar e-mails dos USUÁRIOS vendedores
                NLoginService negociosLogin = new NLoginService();
                listaEmailsVendedoresQueReceberaoACotacao = negociosLogin.ConsultarEmailsDosVendedoresQueReceberaoAvisoDeCotacao(listaIDsUsuariosVendedores);

                //Buscar telefones dos USUÁRIOS vendedores
                NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                List<usuario_empresa> listaDadosUsuariosVendedoresQueReceberaoACotacao = new List<usuario_empresa>();

                listaDadosUsuariosVendedoresQueReceberaoACotacao =
                    negociosUsuarioEmpresa.ConsultarDadosDosUsuariosVendedoresQueReceberaoAvisoDeCotacao(listaIDsUsuariosVendedores);

                //Gerar a COTAÇÂO MASTER (COTAÇAO_MASTER_USUARIO_COTANTE)
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                cotacao_master_usuario_empresa dadosDaCotacaoMasterUsuarioEmpresa = new cotacao_master_usuario_empresa();

                itens_cotacao_usuario_empresa gravandoItensDaCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();

                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO = 1; //Por default já inicia como ABERTA (1-ABERTA, 2-EM ANDAMENTO, 3-ENCERRADA)
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_TIPO_COTACAO = 1; //Tipo DIRECIONADA (1-DIRECIONADA, 2-AVULSA, 3-CENTRAL DE COMPRAS)

                //Monta o Nome da Cotação Direcionada
                if ((nomeDaCotacao != null) && (nomeDaCotacao != ""))
                {
                    dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA = nomeDaCotacao;
                }
                else
                {
                    //Busca quantidade de registros de cotação do Usuário Cotante, para montar a cotação
                    List<cotacao_master_usuario_empresa> registrosDeCotacaoUsuarioCotante = new List<cotacao_master_usuario_empresa>();

                    registrosDeCotacaoUsuarioCotante =
                        negociosCotacaoMasterUsuarioEmpresa.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    if (registrosDeCotacaoUsuarioCotante.Count > 0)
                    {
                        dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA =
                            "0000" + (registrosDeCotacaoUsuarioCotante.Count + 1).ToString();
                    }
                    else
                    {
                        dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA = "00001";
                    }
                }

                dadosDaCotacaoMasterUsuarioEmpresa.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA = DateTime.Now;

                if (dataEncerramentoDaCotacaoDirecionada != null)
                {
                    //Armazena data informada pelo USUÁRIO
                    dadosDaCotacaoMasterUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA = Convert.ToDateTime(dataEncerramentoDaCotacaoDirecionada);
                }
                else
                {
                    //O USUÁRIO não informou uma data de ENCERRAMENTO, então o sistema sugere a data atual acrescida de mais 1 dia
                    var dataSugerida = DateTime.Now.AddDays(1);
                    dadosDaCotacaoMasterUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA = dataSugerida;
                }

                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_TIPO_FRETE = 2; //Por default FOB (1-CIF (Fornecedor paga) , 2-FOB (Cliente paga))
                dadosDaCotacaoMasterUsuarioEmpresa.ID_GRUPO_ATIVIDADES = Convert.ToInt32(idCategoriaDaAtividadeACotar);
                dadosDaCotacaoMasterUsuarioEmpresa.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_EMPRESA = 0; //Inicia por default com 0

                //Gerando a COTAÇÃO MASTER
                cotacao_master_usuario_empresa gerarCotacaoMaster =
                    negociosCotacaoMasterUsuarioEmpresa.GerarCotacaoMasterUsuarioEmpresa(dadosDaCotacaoMasterUsuarioEmpresa);

                //Depois de gerada a COTAÇÃO MASTER, gravar os ITENS pertencentes a cotação
                if (gerarCotacaoMaster != null)
                {
                    //Inserir novos Produtos / Serviços vinculados a um Grupo de Atividades
                    NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();
                    produtos_servicos_empresa_profissional novoProdutoServicoEmpresaProfissional = new produtos_servicos_empresa_profissional();

                    //Inserir novas Marcas ou Fabricantes no sistema
                    NEmpresasFabricantesMarcasService negociosEmpresasFabricantesEMarcas = new NEmpresasFabricantesMarcasService();
                    empresas_fabricantes_marcas novaEmpresaFabricanteOuMarca = new empresas_fabricantes_marcas();

                    //Armazenar os Itens ligados ou pertencentes à COTAÇÃO MASTER (ITENS_COTACAO_USUARIO_COTANTE)
                    for (int i = 0; i < listaIDsProdutosServicos.LongLength; i++)
                    {
                        //Salvando os novos Produtos/Serviços na tabela PRODUTOS_SERVICOS_EMPRESA_PROFISSIONAL, se ainda não existirem no banco
                        if (listaIDsProdutosServicos[i].IndexOf('a') > 0)
                        {
                            novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = Convert.ToInt32(idCategoriaDaAtividadeACotar);
                            novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoDosProdutosOuServicosASeremCotados[i];

                            if ((tipoCotacaoProdutoDirecionada == "P") && (tipoCotacaoServicoDirecionada == "S"))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((tipoCotacaoProdutoDirecionada == "P") && (tipoCotacaoServicoDirecionada == ""))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((tipoCotacaoProdutoDirecionada == "") && (tipoCotacaoServicoDirecionada == "S"))
                            {
                                //Define como SERVIÇO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "S";
                            }

                            //Grava o novo PRODUTO / SERVIÇO
                            produtos_servicos_empresa_profissional produtosServicosEmpresaProfissional =
                                negocioProdutosServicosEmpresaProfissional.GravarNovoProdutoServicoEmpresaProfissional(novoProdutoServicoEmpresaProfissional);

                            if (produtosServicosEmpresaProfissional != null)
                            {
                                //Troca ID temporário pelo ID real, obtido na gravação do Produto/Serviço
                                listaIDsProdutosServicos[i] = produtosServicosEmpresaProfissional.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS.ToString();
                            }
                        }

                        //Salvando as novas Marcas / Fabricantes na tabela EMPRESAS_FABRICANTES_MARCAS, se ainda não existirem no banco
                        if (listaIDsMarcasFabricantes[i].IndexOf('a') > 0)
                        {
                            if ((listaDescricaoMarcasFabricantes[i] != "") && (listaDescricaoMarcasFabricantes[i] != null))
                            {
                                novaEmpresaFabricanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS = listaDescricaoMarcasFabricantes[i];

                                //Grava a nova Marca / Fabricante do produto ou serviço
                                empresas_fabricantes_marcas novaEmpresaFabricanteMarcas =
                                    negociosEmpresasFabricantesEMarcas.GravarNovaEmpresaFabricanteEMarcas(novaEmpresaFabricanteOuMarca);

                                if (novaEmpresaFabricanteMarcas != null)
                                {
                                    //Troca ID temporário pelo ID real, obtido na gravação do Fabricante / Marca
                                    listaIDsMarcasFabricantes[i] = novaEmpresaFabricanteMarcas.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS.ToString();
                                }
                            }
                        }

                        //Salvando os itens da Cotação (ITENS_COTACAO_USUARIO_EMPRESA)
                        NItensCotacaoUsuarioEmpresaService negociosItensCotacaoUsuarioEmpresa = new NItensCotacaoUsuarioEmpresaService();
                        itens_cotacao_usuario_empresa dadosItensCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();

                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA;
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = Convert.ToInt32(listaIDsProdutosServicos[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_UNIDADE_PRODUTO = Convert.ToInt32(listaIDsUnidadesDeMedida[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = Convert.ToInt32(listaIDsMarcasFabricantes[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_PEDIDO_USUARIO_EMPRESA = 0; //Na geração da COTAÇÃO ainda não existe um PEDIDO. Coloquei o valor por default pq o campo é de preenchimento obrigatório
                        dadosItensCotacaoUsuarioEmpresa.QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(listaQuantidadeCadaItem[i], new CultureInfo("en-US"));

                        //Gravar Itens que estarão ligado à COTACAO_MASTER_USUARIO_EMPRESA
                        gravandoItensDaCotacaoUsuarioEmpresa =
                            negociosItensCotacaoUsuarioEmpresa.GravarItensDaCotacaoMasterDoUsuarioEmpresa(dadosItensCotacaoUsuarioEmpresa);

                        if (gravandoItensDaCotacaoUsuarioEmpresa != null)
                        {
                            //Armazenar o ID dos itens da COTAÇÃO MASTER, para serem replicados na geração da COTAÇÃO FILHA
                            listaIDsItensCotacaoUsuarioEmpresa.Add(gravandoItensDaCotacaoUsuarioEmpresa.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA.ToString());
                        }
                    }

                    //GERAR a COTAÇÃO FILHA e ENVIAR a COTAÇÃO para os FORNECEDORES
                    if ((gerarCotacaoMaster != null) && (gravandoItensDaCotacaoUsuarioEmpresa != null))
                    {
                        //Gerar a COTAÇÃO FILHA (COTACAO_FILHA_USUARIO_COTANTE), 1 registro para cada FORNECEDOR
                        NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                        cotacao_filha_usuario_empresa dadosCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();

                        int contadorCotacoesFilha = 0; //Conta quantidade de cotações filha geradas.

                        for (int a = 0; a < listaIDsFornecedores.Length; a++)
                        {
                            dadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA;
                            dadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(listaIDsFornecedores[a]);
                            dadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(listaIDsUsuariosVendedores[a]);
                            dadosCotacaoFilhaUsuarioEmpresa.ID_TIPO_FRETE = 2;
                            dadosCotacaoFilhaUsuarioEmpresa.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA = false;
                            dadosCotacaoFilhaUsuarioEmpresa.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = Convert.ToDateTime("01/01/1900");
                            dadosCotacaoFilhaUsuarioEmpresa.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA = "À Vista";
                            dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO = 0;
                            dadosCotacaoFilhaUsuarioEmpresa.PERCENTUAL_DESCONTO = 0;
                            dadosCotacaoFilhaUsuarioEmpresa.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = 0;
                            dadosCotacaoFilhaUsuarioEmpresa.OBSERVACAO_COTACAO_USUARIO_EMPRESA = null;
                            dadosCotacaoFilhaUsuarioEmpresa.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA = false;

                            cotacao_filha_usuario_empresa gerarCotacaoFilha =
                                negociosCotacaoFilhaUsuarioEmpresa.GerarCotacaoFilhaUsuarioEmpresa(dadosCotacaoFilhaUsuarioEmpresa);

                            //Armazenar os Itens ligados ou pertencentes à COTAÇÃO FILHA (ITENS_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA)
                            if (gerarCotacaoFilha != null)
                            {
                                //Salvando os itens da COTAÇÃO FILHA (ITENS_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE)
                                NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilhaNegociacaoUsuarioEmpresa =
                                    new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                                itens_cotacao_filha_negociacao_usuario_empresa dadosItensCotacaoFilhaUsuarioEmpresa = new itens_cotacao_filha_negociacao_usuario_empresa();

                                for (int b = 0; b < listaIDsItensCotacaoUsuarioEmpresa.Count; b++)
                                {
                                    //dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToInt32(listaIDsItensCotacaoUsuarioEmpresa[b]);
                                    dadosItensCotacaoFilhaUsuarioEmpresa.ID_CODIGO_TIPO_RESPOSTA_COTACAO = 1;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.ID_CLASSIFICACAO_TIPO_ITENS_COTACAO = 1;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_EMPRESA = listaDescricaoDosProdutosOuServicosASeremCotados[b];
                                    dadosItensCotacaoFilhaUsuarioEmpresa.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = 0;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(listaQuantidadeCadaItem[b], new CultureInfo("en-US"));
                                    dadosItensCotacaoFilhaUsuarioEmpresa.PRODUTO_COTADO_USUARIO_EMPRESA = false;
                                    dadosItensCotacaoFilhaUsuarioEmpresa.ITEM_COTACAO_FILHA_EDITADO = false;

                                    itens_cotacao_filha_negociacao_usuario_empresa gravandoItensDaCotacaoFilhaUsuarioCotante =
                                        negociosItensCotacaoFilhaNegociacaoUsuarioEmpresa.GravarItensDaCotacaoFilhaUsuarioEmpresa(dadosItensCotacaoFilhaUsuarioEmpresa);
                                }
                            }

                            contadorCotacoesFilha++; //Conta para quantos fornecedores a COTAÇÃO foi enviada, para posterior comparação
                        }

                        //Enviar os AVISOS DE COTAÇÃO via SMS´s, E-MAIL´S e PELO APLICATIVO
                        if (contadorCotacoesFilha == listaIDsFornecedores.Length)
                        {
                            //Disparar e-mails de avisos / SMS de aviso ao Usuário Vendedor, em caso de nova COTAÇÃO direcionada a ele
                            NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
                            empresa_usuario buscarDadosDaEmpresa = new empresa_usuario();

                            EnviarEmailCotacao enviarEmailAvisos = new EnviarEmailCotacao();
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            //Dispara avisos ao USUÁRIO VENDEDOR
                            if (listaEmailsVendedoresQueReceberaoACotacao.Count == listaIDsUsuariosVendedores.Length)
                            {
                                //1 - Enviar os e-mails
                                //Envio de e-mail informando ao usuário VENDEDOR que ele possui uma nova COTAÇÂO (Tipo 7)
                                int tipoEmail = 7;

                                for (int c = 0; c < listaEmailsVendedoresQueReceberaoACotacao.Count; c++)
                                {
                                    //Busca dados da EMPRESA que está recebendo COTAÇÃO
                                    buscarDadosDaEmpresa.ID_CODIGO_EMPRESA = listaDadosUsuariosVendedoresQueReceberaoACotacao[c].ID_CODIGO_EMPRESA;

                                    empresa_usuario dadosDaEmpresa = negociosEmpresa.ConsultarDadosDaEmpresa(buscarDadosDaEmpresa);

                                    //bool emailUsuarioEmpresaVendedor = enviarEmailAvisos.EnviandoEmail(dadosDaEmpresa.RAZAO_SOCIAL_EMPRESA, listaDadosUsuariosVendedoresQueReceberaoACotacao[c].NICK_NAME_USUARIO,
                                    //    listaEmailsVendedoresQueReceberaoACotacao[c].ToString(), tipoEmail);

                                    cotacaoEnviada = "ok";
                                }

                                //2 - Enviar SMS
                                //Envio de SMS informando ao usuário VENDEDOR que ele possui uma nova COTAÇÃO
                                string smsMensagem = "ClienteMercado - Você tem uma nova COTACAO. Nao perca vendas. Responda pelo aplicativo no seu celular ou acesse www.clientemercado.com.br.";

                                for (int d = 0; d < listaDadosUsuariosVendedoresQueReceberaoACotacao.Count; d++)
                                {
                                    string telefoneUsuarioVendedor = Regex.Replace(listaDadosUsuariosVendedoresQueReceberaoACotacao[d].TELEFONE1_USUARIO_EMPRESA, "[()-]", "");
                                    string urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioVendedor;

                                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuariovendedor));

                                    //if (smsUsuarioVendedor)
                                    //{
                                    //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                    //    NControleSMS negociosSMS = new NControleSMS();
                                    //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                    //    controleEnvioSms.TELEFONE_DESTINO = listaDadosUsuariosVendedoresQueReceberaoACotacao[d].TELEFONE1_USUARIO_EMPRESA;
                                    //    controleEnvioSms.ID_CODIGO_USUARIO = listaDadosUsuariosVendedoresQueReceberaoACotacao[d].ID_CODIGO_USUARIO;
                                    //    controleEnvioSms.MOTIVO_ENVIO = 2; //Valor default. 2 - Envio de Cotação (Criar uma tabela com esses valores para referência/leitura)

                                    //    controle_sms_usuario_empresa controleSmsUsuarioEmpresa = negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    //}
                                }

                                //3 - Enviar ALERT ao aplicativo no celular
                                /*
                                    CODIFICAR...
                                */
                            }

                            //Retorna confirmação do envio da cotação
                            var resultado = new
                            {
                                cotacaoFoiEnviada = cotacaoEnviada
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                /*
                 ENVIAR RETORNO JSON INFORMANDO SUCESSO NO ENVIO DA COTAÇÃO AVULSA...
                */
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //Processo para o envio da COTAÇÃO AVULSA
        [WebMethod]
        public ActionResult EnviarCotacaoAvulsaUsuarioEmpresa(string nomeDaCotacao, string dataEncerramentoDaCotacaoDirecionada, string idCategoriaDaAtividadeACotar, string tipoCotacaoProdutoDirecionada,
            string tipoCotacaoServicoDirecionada, string idsProdutosServicos, string descricaoDosProdutosOuServicosASeremCotados, string idsUnidadesDeMedida, string idsFornecedores,
            string idsUsuariosVendedores, string quantidadeCadaItem, string idsMarcasFabricantes, string descricaoMarcasFabricantes, string filtroPorCidade, string filtroPorUF,
            string idDaOutraCidade, string idDeOutroEstado, string idDeOutraCidadeOutroEstado, string idDaCidadeCotante, string idDoEstadoCotante, string idDoPaisCotante)
        {
            //Definindo as variáveis pertinentes ao método em questão
            string cotacaoEnviada = "nok";
            string[] listaIDsProdutosServicos, listaDescricaoDosProdutosOuServicosASeremCotados, listaIDsUnidadesDeMedida, listaQuantidadeCadaItem, listaIDsMarcasFabricantes,
                listaDescricaoMarcasFabricantes;

            ArrayList listaIDsItensCotacaoUsuarioEmpresa = new ArrayList();
            ArrayList listaEmailsVendedoresQueReceberaoACotacao = new ArrayList();

            listaIDsProdutosServicos = idsProdutosServicos.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoDosProdutosOuServicosASeremCotados = descricaoDosProdutosOuServicosASeremCotados.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsUnidadesDeMedida = idsUnidadesDeMedida.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaQuantidadeCadaItem = quantidadeCadaItem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaIDsMarcasFabricantes = idsMarcasFabricantes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoMarcasFabricantes = descricaoMarcasFabricantes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            try
            {
                //Gerar a COTAÇÃO MASTER (COTAÇAO_MASTER_USUARIO_COTANTE)
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                cotacao_master_usuario_empresa dadosDaCotacaoMasterUsuarioEmpresa = new cotacao_master_usuario_empresa();

                itens_cotacao_usuario_empresa gravandoItensDaCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();

                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO = 1; //Por default já inicia como ABERTA (1-ABERTA, 2-EM ANDAMENTO, 3-ENCERRADA)
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_TIPO_COTACAO = 2; //Tipo DIRECIONADA (1-DIRECIONADA, 2-AVULSA, 3-CENTRAL DE COMPRAS)

                //Monta o Nome da Cotação Direcionada
                if ((nomeDaCotacao != null) && (nomeDaCotacao != ""))
                {
                    dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA = nomeDaCotacao;
                }
                else
                {
                    //Busca quantidade de registros de cotação do Usuário Cotante, para montar a cotação
                    List<cotacao_master_usuario_empresa> registrosDeCotacaoUsuarioCotante = new List<cotacao_master_usuario_empresa>();

                    registrosDeCotacaoUsuarioCotante =
                        negociosCotacaoMasterUsuarioEmpresa.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    if (registrosDeCotacaoUsuarioCotante.Count > 0)
                    {
                        dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA =
                            "0000" + (registrosDeCotacaoUsuarioCotante.Count + 1).ToString();
                    }
                    else
                    {
                        dadosDaCotacaoMasterUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA = "00001";
                    }
                }

                dadosDaCotacaoMasterUsuarioEmpresa.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA = DateTime.Now;

                if (dataEncerramentoDaCotacaoDirecionada != null)
                {
                    //Armazena data informada pelo USUÁRIO
                    dadosDaCotacaoMasterUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA = Convert.ToDateTime(dataEncerramentoDaCotacaoDirecionada);
                }
                else
                {
                    //O USUÁRIO não informou uma data de ENCERRAMENTO, então o sistema sugere a data atual acrescida de mais 1 dia
                    var dataSugerida = DateTime.Now.AddDays(1);
                    dadosDaCotacaoMasterUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA = dataSugerida;
                }

                dadosDaCotacaoMasterUsuarioEmpresa.ID_CODIGO_TIPO_FRETE = 2; //Por default FOB (1-CIF (Fornecedor paga) , 2-FOB (Cliente paga))
                dadosDaCotacaoMasterUsuarioEmpresa.ID_GRUPO_ATIVIDADES = Convert.ToInt32(idCategoriaDaAtividadeACotar);
                dadosDaCotacaoMasterUsuarioEmpresa.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_EMPRESA = 0; //Inicia por default com 0

                //Gerando a COTAÇÃO MASTER
                cotacao_master_usuario_empresa gerarCotacaoMaster =
                    negociosCotacaoMasterUsuarioEmpresa.GerarCotacaoMasterUsuarioEmpresa(dadosDaCotacaoMasterUsuarioEmpresa);

                //Depois de gerada a COTAÇÃO MASTER, gravar os ITENS pertencentes a cotação     <=== OBS: VERIFICAR TODO ESTE TRECHO A PARTIR DAQUI...
                if (gerarCotacaoMaster != null)
                {
                    //Inserir novos Produtos / Serviços vinculados a um Grupo de Atividades
                    NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();
                    produtos_servicos_empresa_profissional novoProdutoServicoEmpresaProfissional = new produtos_servicos_empresa_profissional();

                    //Inserir novas Marcas ou Fabricantes no sistema
                    NEmpresasFabricantesMarcasService negociosEmpresasFabricantesEMarcas = new NEmpresasFabricantesMarcasService();
                    empresas_fabricantes_marcas novaEmpresaFabricanteOuMarca = new empresas_fabricantes_marcas();

                    //Armazenar os Itens ligados ou pertencentes à COTAÇÃO MASTER (ITENS_COTACAO_USUARIO_COTANTE)
                    for (int i = 0; i < listaIDsProdutosServicos.LongLength; i++)
                    {
                        //Salvando os novos Produtos/Serviços na tabela PRODUTOS_SERVICOS_EMPRESA_PROFISSIONAL, se ainda não existirem no banco
                        if (listaIDsProdutosServicos[i].IndexOf('a') > 0)
                        {
                            novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = Convert.ToInt32(idCategoriaDaAtividadeACotar);
                            novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoDosProdutosOuServicosASeremCotados[i];

                            if ((tipoCotacaoProdutoDirecionada == "P") && (tipoCotacaoServicoDirecionada == "S"))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((tipoCotacaoProdutoDirecionada == "P") && (tipoCotacaoServicoDirecionada == ""))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((tipoCotacaoProdutoDirecionada == "") && (tipoCotacaoServicoDirecionada == "S"))
                            {
                                //Define como SERVIÇO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "S";
                            }

                            //Grava o novo PRODUTO / SERVIÇO
                            produtos_servicos_empresa_profissional produtosServicosEmpresaProfissional =
                                negocioProdutosServicosEmpresaProfissional.GravarNovoProdutoServicoEmpresaProfissional(novoProdutoServicoEmpresaProfissional);

                            if (produtosServicosEmpresaProfissional != null)
                            {
                                //Troca ID temporário pelo ID real, obtido na gravação do Produto/Serviço
                                listaIDsProdutosServicos[i] = produtosServicosEmpresaProfissional.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS.ToString();
                            }
                        }

                        //Salvando as novas Marcas / Fabricantes na tabela EMPRESAS_FABRICANTES_MARCAS, se ainda não existirem no banco
                        if (listaIDsMarcasFabricantes[i].IndexOf('a') > 0)
                        {
                            if ((listaDescricaoMarcasFabricantes[i] != "") && (listaDescricaoMarcasFabricantes[i] != null))
                            {
                                novaEmpresaFabricanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS = listaDescricaoMarcasFabricantes[i];

                                //Grava a nova Marca / Fabricante do produto ou serviço
                                empresas_fabricantes_marcas novaEmpresaFabricanteMarcas =
                                    negociosEmpresasFabricantesEMarcas.GravarNovaEmpresaFabricanteEMarcas(novaEmpresaFabricanteOuMarca);

                                if (novaEmpresaFabricanteMarcas != null)
                                {
                                    //Troca ID temporário pelo ID real, obtido na gravação do Fabricante / Marca
                                    listaIDsMarcasFabricantes[i] = novaEmpresaFabricanteMarcas.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS.ToString();
                                }
                            }
                        }

                        //Salvando os itens da Cotação (ITENS_COTACAO_USUARIO_EMPRESA)
                        NItensCotacaoUsuarioEmpresaService negociosItensCotacaoUsuarioEmpresa = new NItensCotacaoUsuarioEmpresaService();
                        itens_cotacao_usuario_empresa dadosItensCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();

                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA;
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = Convert.ToInt32(listaIDsProdutosServicos[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_UNIDADE_PRODUTO = Convert.ToInt32(listaIDsUnidadesDeMedida[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = Convert.ToInt32(listaIDsMarcasFabricantes[i]);
                        dadosItensCotacaoUsuarioEmpresa.ID_CODIGO_PEDIDO_USUARIO_EMPRESA = 0; //Na geração da COTAÇÃO ainda não existe um PEDIDO. Coloquei o valor por default pq o campo é de preenchimento obrigatório
                        dadosItensCotacaoUsuarioEmpresa.QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(listaQuantidadeCadaItem[i], new CultureInfo("en-US"));

                        //Gravar Itens que estarão ligado à COTACAO_MASTER_USUARIO_EMPRESA
                        gravandoItensDaCotacaoUsuarioEmpresa =
                            negociosItensCotacaoUsuarioEmpresa.GravarItensDaCotacaoMasterDoUsuarioEmpresa(dadosItensCotacaoUsuarioEmpresa);
                    }

                    //Dispara avisos aos USUÁRIOS VENDEDORES de EMPRESAS COTADAS habilitadas para serem avisadas de COTAÇÃO AVULSA
                    if ((gerarCotacaoMaster != null) && (gravandoItensDaCotacaoUsuarioEmpresa != null))
                    {
                        //Atribuir / Converter valores dos parâmetros em valores utilizáveis
                        var categoriaASerCotada = Convert.ToInt32(idCategoriaDaAtividadeACotar);
                        var tipoPorCidade = Convert.ToInt32(filtroPorCidade);
                        var tipoPorUF = Convert.ToInt32(filtroPorUF);
                        var idOutraCidade = Convert.ToInt32(idDaOutraCidade); //Foi aqui o erro...
                        var idOutroEstado = Convert.ToInt32(idDeOutroEstado);
                        var idOutraCidadeOutroEstado = Convert.ToInt32(idDeOutraCidadeOutroEstado);
                        var idCidadeCotante = Convert.ToInt32(idDaCidadeCotante);
                        var idEstadoCotante = Convert.ToInt32(idDoEstadoCotante);
                        var idPaisCotante = Convert.ToInt32(idDoPaisCotante);

                        NFornecedoresService negocioFornecedores = new NFornecedoresService();
                        List<FornecedoresASeremCotadosDirecionada> listaFornecedoresASeremCotados = new List<FornecedoresASeremCotadosDirecionada>();
                        List<empresa_usuario> listFornecedores = new List<empresa_usuario>();

                        if ((tipoPorCidade == 1) && (tipoPorUF == 1))
                        {
                            //Buscar fornecedores na MINHA CIDADE e no MEU ESTADO
                            listFornecedores = negocioFornecedores.BuscarFornecedoresNaMinhaCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idCidadeCotante, idEstadoCotante, idPaisCotante);
                        }
                        else if ((tipoPorCidade == 2) && (tipoPorUF == 1))
                        {
                            //Buscar fornecedores em OUTRA CIDADE e no MEU ESTADO
                            listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutraCidadeENoMeuEstadoAvulsa(categoriaASerCotada, idOutraCidade, idEstadoCotante, idPaisCotante);
                        }
                        else if ((tipoPorCidade == 0) && (tipoPorUF == 2))
                        {
                            //Buscar fornecedores de OUTRO ESTADO (TODAS as cidades do estado ou CIDADE ESPECÍFICA)
                            listFornecedores = negocioFornecedores.BuscarFornecedoresEmOutroEstadoAvulsa(categoriaASerCotada, idOutroEstado, idOutraCidadeOutroEstado, idPaisCotante);
                        }
                        else if ((tipoPorCidade == 0) && (tipoPorUF == 3))
                        {
                            //Buscar fornecedores de TODO o PAÍS
                            listFornecedores = negocioFornecedores.BuscarFornecedoresEmTodoOPaisAvulsa(categoriaASerCotada, idPaisCotante);
                        }

                        //Montar avisos de COTAÇÃO AVULSA para usuários habilitados para responder a mesma
                        if (listFornecedores.Count > 0)
                        {
                            //Disparar e-mails de avisos / SMS de aviso ao Usuário Vendedor, em caso de nova COTAÇÃO direcionada a ele
                            NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
                            empresa_usuario buscarDadosDaEmpresa = new empresa_usuario();

                            EnviarEmailCotacao enviarEmailAvisos = new EnviarEmailCotacao();
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            ArrayList listaIdsEmpresasFornecedorasQueReceberaoAvisoDeCotacaoAvulsa = new ArrayList();
                            ArrayList listaEmailsVendedoresQueReceberaoAvisoDeCotacaoAvulsa = new ArrayList();
                            ArrayList listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa = new ArrayList();

                            //Carregar lista com ID´s dos Usuários Vendedores que serão acionados
                            for (int a = 0; a < listFornecedores.Count; a++)
                            {
                                listaIdsEmpresasFornecedorasQueReceberaoAvisoDeCotacaoAvulsa.Add(listFornecedores[a].ID_CODIGO_EMPRESA);
                                listaEmailsVendedoresQueReceberaoAvisoDeCotacaoAvulsa.Add(listFornecedores[a].usuario_empresa.ElementAt(0).empresa_usuario_logins.ElementAt(0).EMAIL1_USUARIO);
                                listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa.Add(listFornecedores[a].usuario_empresa.ElementAt(0).TELEFONE1_USUARIO_EMPRESA);
                            }

                            //1 - Enviar os e-mails
                            //Envio de e-mail informando ao usuário VENDEDOR que ele possui uma nova COTAÇÃO (Tipo 7)
                            if (listaEmailsVendedoresQueReceberaoAvisoDeCotacaoAvulsa.Count > 0)
                            {
                                int tipoEmail = 7;

                                for (int c = 0; c < listaIdsEmpresasFornecedorasQueReceberaoAvisoDeCotacaoAvulsa.Count; c++)
                                {
                                    //Busca dados da EMPRESA qque está recebendo COTAÇÃO
                                    buscarDadosDaEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(listaIdsEmpresasFornecedorasQueReceberaoAvisoDeCotacaoAvulsa[c]);

                                    empresa_usuario dadosDaEmpresa = negociosEmpresa.ConsultarDadosDaEmpresa(buscarDadosDaEmpresa);

                                    //bool emailUsuarioEmpresaVendedor = enviarEmailAvisos.EnviandoEmail(dadosDaEmpresa.RAZAO_SOCIAL_EMPRESA, listFornecedores[c].usuario_empresa.ElementAt(0).NICK_NAME_USUARIO,
                                    //    listaEmailsVendedoresQueReceberaoAvisoDeCotacaoAvulsa[c].ToString(), tipoEmail);
                                }

                                cotacaoEnviada = "ok";
                            }

                            //2 - Enviar SMS
                            //Envio de SMS informando ao usuário VENDEDOR que ele possui uma nova COTAÇÃO
                            if (listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa.Count > 0)
                            {
                                string smsMensagem = "ClienteMercado - COTACAO AVULSA disponível. Nao perca vendas. Responda pelo aplicativo no seu celular ou acesse www.clientemercado.com.br.";

                                for (int d = 0; d < listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa.Count; d++)
                                {
                                    string telefoneUsuariovendedor = Regex.Replace(listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa[d].ToString(), "[()-]", "");
                                    string urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuariovendedor;

                                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuariovendedor));

                                    //if (smsUsuarioVendedor)
                                    //{
                                    //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                    //    NControleSMS negociosSMS = new NControleSMS();
                                    //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                    //    controleEnvioSms.TELEFONE_DESTINO = listaTelefonesVendedoresQueReceberaoAvisoDeCotacaoAvulsa[d].ToString();
                                    //    controleEnvioSms.ID_CODIGO_USUARIO = listFornecedores[d].usuario_empresa.ElementAt(0).ID_CODIGO_USUARIO;
                                    //    controleEnvioSms.MOTIVO_ENVIO = 2; //Valor default. 2 - Envio de Cotação (Criar uma tabela com esses valores para referência/leitura)

                                    //    controle_sms_usuario_empresa controleSmsUsuarioEmpresa = negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    //}
                                }
                            }

                            //3 - Enviar ALERT ao aplicativo no celular
                            /*
                                CODIFICAR...
                            */

                        }
                    }
                }

                //Retorna confirmação do envio da cotação
                var resultado = new
                {
                    cotacaoFoiEnviada = cotacaoEnviada
                };

                return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Armazena na tabela CHAT_COTACAO_USUARIO_COTANTE a conversa entre o COTANTE e o FORNECEDOR
        [WebMethod]
        public ActionResult EnviarComunicacaoAoFornecedorNaCotacao(int idCotacaoFilha, int idEmpresaCotada, string textoPerguntaOuResposta)
        {
            try
            {
                string retornoGravacao = "nok";

                NChatCotacaoUsuarioEmpresaService negociosChatUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                chat_cotacao_usuario_empresa dadosChatCotacao = new chat_cotacao_usuario_empresa();

                int numeroDeOrdemNaExibicao = (negociosChatUsuarioEmpresa.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha) + 1);

                //Preparando os dados a serem gravados
                dadosChatCotacao.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTANTE = (int)Sessao.IdUsuarioLogado;
                dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTADA = idEmpresaCotada;
                dadosChatCotacao.DATA_CHAT_COTACAO_USUARIO_EMPRESA = DateTime.Now;
                dadosChatCotacao.TEXTO_CHAT_COTACAO_USUARIO_EMPRESA = textoPerguntaOuResposta;
                dadosChatCotacao.ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_EMPRESA = numeroDeOrdemNaExibicao;

                //Gravar CONVERSA no CHAT
                chat_cotacao_usuario_empresa gravarPerguntaOuRespostaDoChat = negociosChatUsuarioEmpresa.GravarConversaNoChat(dadosChatCotacao, idEmpresaCotada, textoPerguntaOuResposta);

                if (gravarPerguntaOuRespostaDoChat != null)
                {
                    retornoGravacao = "ok";
                }

                //Resultado a ser retornado
                var resultado = new
                {
                    gravacaoChat = retornoGravacao,
                    texto = textoPerguntaOuResposta
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carregar RESPOSTAS da COTAÇÃO respondida pelo FORNECEDOR
        public JsonResult CarregarRespostaDaCotacao(int idCotacaoFilha, string tipoCotacao)
        {
            try
            {
                decimal quantidadeItensReal;
                string quantidadeItensFormatado;
                string valorDoProduto = "0,00";
                string produtoCotado = "nao";
                string valorTotalDoProduto = "0,00";
                decimal valorDoProdutoSemDesconto = 0;
                decimal valorDoProdutoComDesconto = 0;
                string listaFotosProdutosAlternativos = "";
                int quantidadeFotosAnexadas = 0;
                string pastaCodigoEmpresa = "";
                string pastaCodigoUsuario = "";

                List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

                if (tipoCotacao == "uc")
                {
                    //Carrega RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE

                }
                else if (tipoCotacao == "ec")
                {
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioEmpresaService negociosItensCotacaoMasterUsuarioEmpresa = new NItensCotacaoUsuarioEmpresaService();
                    itens_cotacao_usuario_empresa dadosItemCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();

                    NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                    cotacao_filha_usuario_empresa dadosCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                    //BUSCAR dados da COTAÇÃO FILHA
                    dadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                    cotacao_filha_usuario_empresa dadosConsultadosCotacaoFilhaUsuarioEmpresa =
                        negociosCotacaoFilhaUsuarioEmpresa.ConsultarDadosDaCotacaoFilhaPeloUsuarioEmpresaCotante(dadosCotacaoFilhaUsuarioEmpresa);

                    if (dadosConsultadosCotacaoFilhaUsuarioEmpresa != null)
                    {
                        pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA.ToString();
                        pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO.ToString();
                    }

                    List<itens_cotacao_filha_negociacao_usuario_empresa> itensDaCotacaoUsuarioEmpresa = negociosItensCotacaoFilha.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoFilha);

                    if (itensDaCotacaoUsuarioEmpresa.Count > 0)
                    {
                        for (int i = 0; i < itensDaCotacaoUsuarioEmpresa.Count; i++)
                        {
                            dadosItemCotacaoUsuarioEmpresa.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA;

                            //Buscar dados dos itens na COTAÇÃO MASTER
                            itens_cotacao_usuario_empresa dadosDoProdutoDaCotacaoMaster = negociosItensCotacaoMasterUsuarioEmpresa.ConsultarDadosDosItensDaCotacaoFilha(dadosItemCotacaoUsuarioEmpresa);

                            //Buscar dados do PRODUTO
                            produtos_servicos_empresa_profissional dadosDoProdutoEmSi =
                                negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                            //Buscar dados da MARCA do PRODUTO
                            empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                                negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                            //Buscar dados da UNIDADE do PRODUTO
                            unidades_produtos dadosDaUnidadeProduto =
                                negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_UNIDADE_PRODUTO);

                            //TESTAR
                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;
                            }

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA); //Formata a quantidade para exibição

                            quantidadeItensReal = itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA) > 0)
                            {
                                //Calcula o VALOR TOTAL dos PRODUTOS sem desconto aplicado (Obs: Pra o caso de ocorrer DESCONTO na COTAÇÃO respondida)
                                valorDoProdutoSemDesconto = (itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA * itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA);

                                valorDoProduto = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 0) || (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 1) || (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);
                                }
                                else if (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((dadosCotacaoFilhaUsuarioEmpresa.PERCENTUAL_DESCONTO * valorDoProdutoSemDesconto) / 100);
                                    valorDoProdutoComDesconto = (valorDoProdutoSemDesconto - valorDesconto);

                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                }
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA > 0)
                            {
                                produtoCotado = "sim";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA, idCotacaoFilha,
                                dadosConsultadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItensFormatado,
                                quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, valorTotalDoProduto, 0, "",
                                quantidadeFotosAnexadas, listaFotosProdutosAlternativos, "", 0, 0, "", "", "", "", 0));

                            quantidadeFotosAnexadas = 0;
                            valorDoProduto = "0,00";
                            valorTotalDoProduto = "0,00";
                            listaFotosProdutosAlternativos = "";
                        }
                    }
                }
                else if (tipoCotacao == "cc")
                {
                    /*
                     IMPLEMENTAR FUTURAMENTE
                     */
                }

                return Json(produtosDaCotacao, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carregar FOTOS dos PRODUTOS ALTERNATIVOS enviados pelo FORNECEDOR como alternativa de compra (caso não tenha o produto solicitado),  da COTAÇÃO respondida pelo FORNECEDOR
        public JsonResult CarregarFotosDosProdutosAlternativos(int idCotacaoFilhaNegociacao, int idCotacaoFilha, string tipoCotacao)
        {
            try
            {
                string listaFotosProdutosAlternativos = "";
                int quantidadeFotosAnexadas = 0;
                string pastaCodigoEmpresa = "";
                string pastaCodigoUsuario = "";
                var resultado = new { fotos_produtos_alternativos = "", quantidade_imagens_anexadas = 0 };

                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                cotacao_filha_usuario_empresa dadosCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();
                NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosFotosProdutosAlternativos =
                    new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                //BUSCAR dados da COTAÇÃO FILHA
                dadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                cotacao_filha_usuario_empresa dadosConsultadosCotacaoFilhaUsuarioEmpresa =
                    negociosCotacaoFilhaUsuarioEmpresa.ConsultarDadosDaCotacaoFilhaPeloUsuarioEmpresaCotante(dadosCotacaoFilhaUsuarioEmpresa);

                if (dadosConsultadosCotacaoFilhaUsuarioEmpresa != null)
                {
                    pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_EMPRESA.ToString();
                    pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioEmpresa.ID_CODIGO_USUARIO.ToString();
                }

                //CARREGAR FOTOS ANEXADAS se houver
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> imagensArmazenadas =
                    negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(idCotacaoFilhaNegociacao);

                if (imagensArmazenadas.Count > 0)
                {
                    quantidadeFotosAnexadas = imagensArmazenadas.Count;

                    for (int a = 0; a < imagensArmazenadas.Count; a++)
                    {
                        if (listaFotosProdutosAlternativos == "")
                        {
                            listaFotosProdutosAlternativos = pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                        }
                        else
                        {
                            listaFotosProdutosAlternativos = listaFotosProdutosAlternativos + "," + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                        }
                    }
                }

                resultado = new
                {
                    fotos_produtos_alternativos = listaFotosProdutosAlternativos,
                    quantidade_imagens_anexadas = quantidadeFotosAnexadas
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega DIÁLOGOS entre COTANTE e FORNECEDOR
        public JsonResult CarregarDialogoEntreCotanteEFornecedor(int idCotacaoFilha, string tipoCotacao)
        {
            if (tipoCotacao == "uc")
            {
                try
                {
                    //Carrega CHAT entre USUÁRIO COTANTE e FORNECEDOR
                    NChatCotacaoUsuarioCotanteService negociosChatCotacaoUsuarioCotante = new NChatCotacaoUsuarioCotanteService();
                    List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                    //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                    List<chat_cotacao_usuario_cotante> listaConversasApuradasNoChat = negociosChatCotacaoUsuarioCotante.BuscarChatEntreUsuarioCOtanteEFornecedor(idCotacaoFilha);

                    if (listaConversasApuradasNoChat != null)
                    {
                        //Montagem da lista de Fornecedores
                        for (int a = 0; a < listaConversasApuradasNoChat.Count; a++)
                        {
                            listaDeConversasEntreCotanteEFornecedorNoChat.Add(
                                new ChatEntreUsuarioEFornecedor(
                                    listaConversasApuradasNoChat[a].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                                    listaConversasApuradasNoChat[a].ID_CODIGO_USUARIO_EMPRESA_COTADA,
                                    "",
                                    listaConversasApuradasNoChat[a].DATA_CHAT_COTACAO_USUARIO_COTANTE.ToString(),
                                    listaConversasApuradasNoChat[a].TEXTO_CHAT_COTACAO_USUARIO_COTANTE,
                                    listaConversasApuradasNoChat[a].ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_COTANTE
                                    )
                                );
                        }
                    }

                    return Json(listaDeConversasEntreCotanteEFornecedorNoChat, JsonRequestBehavior.AllowGet);
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoCotacao == "ec")
            {
                try
                {
                    //Carrega CHAT entre USUÁRIO EMPRESA e FORNECEDOR
                    NChatCotacaoUsuarioEmpresaService negociosChatCotacaoUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                    List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                    //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                    List<chat_cotacao_usuario_empresa> listaConversasApuradasNoChat = negociosChatCotacaoUsuarioEmpresa.BuscarChatEntreUsuarioEmpresaEFornecedor(idCotacaoFilha);

                    if (listaConversasApuradasNoChat != null)
                    {
                        //Montagem da lista de Fornecedores
                        for (int a = 0; a < listaConversasApuradasNoChat.Count; a++)
                        {
                            listaDeConversasEntreCotanteEFornecedorNoChat.Add(
                                new ChatEntreUsuarioEFornecedor(
                                    listaConversasApuradasNoChat[a].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA,
                                    listaConversasApuradasNoChat[a].ID_CODIGO_USUARIO_EMPRESA_COTADA,
                                    "",
                                    listaConversasApuradasNoChat[a].DATA_CHAT_COTACAO_USUARIO_EMPRESA.ToString(),
                                    listaConversasApuradasNoChat[a].TEXTO_CHAT_COTACAO_USUARIO_EMPRESA,
                                    listaConversasApuradasNoChat[a].ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_EMPRESA
                                    )
                                );
                        }
                    }

                    return Json(listaDeConversasEntreCotanteEFornecedorNoChat, JsonRequestBehavior.AllowGet);
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoCotacao == "cc")
            {
                try
                {
                    /*
                    IMPLEMENTAR FUTURAMENTE 
                    */
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }

            return null;
        }

        //Carregar RESPOSTAS da COTAÇÃO para o PRODUTO, respondida pelo FORNECEDOR (Obs: Para efeito de ANÁLISE)
        public JsonResult CarregarRespostaDaCotacaoPorProduto(int idCotacaoMaster, int idCodigoProduto, string tipoCotacao)
        {
            try
            {
                decimal quantidadeItensReal;
                string quantidadeItensFormatado;
                string valorDoProduto = "Não Respondeu";
                string produtoCotado = "nao";
                string valorTotalDoProduto = "Não Respondeu";
                decimal valorDoProdutoComDesconto = 0;
                string valorDoProdutoComDescontoParaExibicao = "";
                string listaFotosProdutosAlternativos = "";
                int quantidadeFotosAnexadas = 0;
                string pastaCodigoEmpresa = "";
                string pastaCodigoUsuario = "";
                string itemComMenorPreco = "nao";
                string listaIdsCotacoesEnviadas = "";
                string menorPrecoJaDefinido = "";

                List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

                if (tipoCotacao == "ec")
                {
                    //Carrega RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioEmpresaService negociosItensCotacaoMasterUsuarioEmpresaCotante = new NItensCotacaoUsuarioEmpresaService();
                    itens_cotacao_usuario_empresa dadosItemCotacaoUsuarioEmpresaCotante = new itens_cotacao_usuario_empresa();
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    empresa_usuario dadosEmpresaUsuario = new empresa_usuario();
                    empresa_usuario dadosDaEmpresaCotada = new empresa_usuario();

                    NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresaCotante = new NCotacaoFilhaUsuarioEmpresaService();
                    cotacao_filha_usuario_empresa dadosCotacaoFilhaUsuarioEmpresa = new cotacao_filha_usuario_empresa();
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                    //BUSCAR dados da COTAÇÃO FILHA
                    List<cotacao_filha_usuario_empresa> dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante =
                        negociosCotacaoFilhaUsuarioEmpresaCotante.ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioEmpresaCotante(idCotacaoMaster);

                    if (dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante.Count > 0)
                    {
                        for (int a = 0; a < dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante.Count; a++)
                        {
                            if (listaIdsCotacoesEnviadas == "")
                            {
                                listaIdsCotacoesEnviadas = dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[a].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.ToString();
                            }
                            else
                            {
                                listaIdsCotacoesEnviadas =
                                    (listaIdsCotacoesEnviadas + "," + dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[a].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.ToString());
                            }
                        }

                        //BUSCA e CALCULA o VALOR TOTAL po PRODUTO em cada COTAÇÃO RESPONDIDA
                        List<ListaPorItemCotadoJaCalculadoUsuarioEmpresaCotanteViewModel> valoresCotadosPorProduto =
                              negociosItensCotacaoFilha.BuscarValorTotalPorProdutoDestaCotacao(listaIdsCotacoesEnviadas, idCodigoProduto);

                        for (int i = 0; i < dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante.Count; i++)
                        {
                            if (dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante != null)
                            {
                                pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_EMPRESA.ToString();
                                pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_USUARIO.ToString();

                                //Consultar EMPRESA cotada
                                dadosEmpresaUsuario.ID_CODIGO_EMPRESA = dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_EMPRESA;

                                dadosDaEmpresaCotada = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(dadosEmpresaUsuario);
                            }

                            dadosItemCotacaoUsuarioEmpresaCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = valoresCotadosPorProduto[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA;

                            //Buscar dados dos itens na COTAÇÃO MASTER
                            itens_cotacao_usuario_empresa dadosDoProdutoDaCotacaoMaster = negociosItensCotacaoMasterUsuarioEmpresaCotante.ConsultarDadosDosItensDaCotacaoFilha(dadosItemCotacaoUsuarioEmpresaCotante);

                            //Buscar dados do PRODUTO
                            produtos_servicos_empresa_profissional dadosDoProdutoEmSi =
                                negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                            //Buscar dados da MARCA do PRODUTO
                            empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                                negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                            //Buscar dados da UNIDADE do PRODUTO
                            unidades_produtos dadosDaUnidadeProduto =
                                negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_UNIDADE_PRODUTO);

                            //BUSCAR as IMAGENS armazenadas para PRODUTOS ALTERNATIVOS (Obs: Enviadas pela EMPRESA que respondeu a cotação)
                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(valoresCotadosPorProduto[i].ID_DACOTACAO);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;
                            }

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA); //Formata a quantidade para exibição

                            quantidadeItensReal = valoresCotadosPorProduto[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA) > 0)
                            {
                                valorDoProduto = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 0) || (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 1)
                                    || (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO);
                                }
                                else if (dadosCotacaoFilhaUsuarioEmpresa.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((dadosCotacaoFilhaUsuarioEmpresa.PERCENTUAL_DESCONTO * valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO) / 100);
                                    valorDoProdutoComDesconto = (valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO - valorDesconto);

                                    valorDoProdutoComDescontoParaExibicao = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                }
                            }

                            //PARA que o DESCONTO tenha valor a ser exibido, mesmo que zerado
                            if (valorDoProdutoComDescontoParaExibicao == "")
                            {
                                valorDoProdutoComDescontoParaExibicao = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO);
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA > 0)
                            {
                                produtoCotado = "sim";
                            }

                            if (i == 0)
                            {
                                if (valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO > 0)
                                {
                                    itemComMenorPreco = "sim";
                                    menorPrecoJaDefinido = "sim";
                                }
                                else
                                {
                                    itemComMenorPreco = "Não Respondeu";
                                }
                            }
                            else
                            {
                                if (valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO > 0)
                                {
                                    if (menorPrecoJaDefinido == "")
                                    {
                                        itemComMenorPreco = "sim";
                                        menorPrecoJaDefinido = "sim";
                                    }
                                }
                                else
                                {
                                    itemComMenorPreco = "Não Respondeu";
                                }
                            }

                            if ((valorDoProduto == "Não Respondeu") && (valorTotalDoProduto == "Não Respondeu"))
                            {
                                valorDoProdutoComDescontoParaExibicao = "Não Respondeu";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(valoresCotadosPorProduto[i].ID_DACOTACAO, dadosConsultadosCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, dadosDaEmpresaCotada.NOME_FANTASIA_EMPRESA,
                                quantidadeItensFormatado, quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado,
                                valorTotalDoProduto, dadosCotacaoFilhaUsuarioEmpresa.PERCENTUAL_DESCONTO, valorDoProdutoComDescontoParaExibicao, quantidadeFotosAnexadas,
                                listaFotosProdutosAlternativos, itemComMenorPreco, 0, 0, "", "", "", "", 0));

                            quantidadeFotosAnexadas = 0;
                            valorDoProduto = "Não Respondeu";
                            valorTotalDoProduto = "Não Respondeu";
                            listaFotosProdutosAlternativos = "";
                            itemComMenorPreco = "nao";
                            valorDoProdutoComDescontoParaExibicao = "";
                        }
                    }
                }

                return Json(produtosDaCotacao, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega os TIPOS de FRETE
        private static List<SelectListItem> ListagemTiposDeFrete()
        {
            //Buscar os TIPOS de FRETE
            NTiposFreteService negociosTiposDeFrete = new NTiposFreteService();
            List<tipos_frete> listaDeTiposDeFretes = negociosTiposDeFrete.ListaDeTiposDeFrete();

            List<SelectListItem> listaFretes = new List<SelectListItem>();

            listaFretes.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

            foreach (var tiposDeFretes in listaDeTiposDeFretes)
            {
                listaFretes.Add(new SelectListItem
                {
                    Text = tiposDeFretes.DESCRICAO_TIPO_FRETE,
                    Value = tiposDeFretes.ID_TIPO_FRETE.ToString()
                });
            }

            return listaFretes;
        }

        //Carregar COTAÇÕES DIRECIONADAS enviadas pelo Usuário Cotante
        private static List<CotacoesEnviadasPeloUsuario> ListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa()
        {
            int fornecedoresCotados = 0;

            try
            {
                //Buscar as COTAÇÕES DIRECIONADAS do Usuário
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                List<CotacoesEnviadasPeloUsuario> cotacoesDirecionadasEnviadasPeloUsuario = new List<CotacoesEnviadasPeloUsuario>();
                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();

                List<cotacao_master_usuario_empresa> listCotacoesDirecionadasDoUsuarioEmpresa =
                    negociosCotacaoMasterUsuarioEmpresa.CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioEmpresa();

                //Monta a lista a ser exibida
                if (listCotacoesDirecionadasDoUsuarioEmpresa.Count > 0)
                {
                    //Monta a Lista das COTAÇÕES enviadas pelo Usuário Cotante
                    for (int i = 0; i < listCotacoesDirecionadasDoUsuarioEmpresa.Count; i++)
                    {
                        //Buscar dados do Grupo de Atividades
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa descricaoGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioEmpresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                        //Buscas dados do Status da Cotacao
                        status_cotacao descricaoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_STATUS_COTACAO);

                        cotacoesDirecionadasEnviadasPeloUsuario.Add(new CotacoesEnviadasPeloUsuario(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA,
                            listCotacoesDirecionadasDoUsuarioEmpresa[i].NOME_COTACAO_USUARIO_EMPRESA, listCotacoesDirecionadasDoUsuarioEmpresa[i].DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString(),
                            listCotacoesDirecionadasDoUsuarioEmpresa[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString(), descricaoGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, 0,
                            descricaoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false));
                    }
                }

                return cotacoesDirecionadasEnviadasPeloUsuario;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carregar COTAÇÕES AVULSAS enviadas pelo Usuário Cotante
        private static List<CotacoesEnviadasPeloUsuario> ListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa()
        {
            int fornecedoresCotados = 0;

            try
            {
                //Buscar as COTAÇÕES AVULSAS do Usuário
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioEmpresaService();
                List<CotacoesEnviadasPeloUsuario> cotacoesAvulsasEnviadasPeloUsuario = new List<CotacoesEnviadasPeloUsuario>();
                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();

                List<cotacao_master_usuario_empresa> listCotacoesAvulsasDoUsuarioEmpresa =
                    negociosCotacaoMasterUsuarioCotante.CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioEmpresa();

                //Monta a lista a ser exibida
                if (listCotacoesAvulsasDoUsuarioEmpresa.Count > 0)
                {
                    //Monta a Lista das COTAÇÕES enviadas pelo Usuário Cotante
                    for (int i = 0; i < listCotacoesAvulsasDoUsuarioEmpresa.Count; i++)
                    {
                        //Buscar dados do Grupo de Atividades
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesAvulsasDoUsuarioEmpresa[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa descricaoGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //====================================================================================== //RESOLVER E CONFERIR AQUI. O NÚMERO DE FORNECEDORES TRAZIDOS PODE ESTAR ERRADO
                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioEmpresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);
                        //====================================================================================== 

                        //Buscas dados do Status da Cotacao
                        //statusDaCotacao.ID_CODIGO_STATUS_COTACAO = listCotacoesAvulsasDoUsuarioEmpresa[i].ID_CODIGO_STATUS_COTACAO;
                        status_cotacao descricaoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(listCotacoesAvulsasDoUsuarioEmpresa[i].ID_CODIGO_STATUS_COTACAO);

                        cotacoesAvulsasEnviadasPeloUsuario.Add(new CotacoesEnviadasPeloUsuario(listCotacoesAvulsasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA,
                            listCotacoesAvulsasDoUsuarioEmpresa[i].NOME_COTACAO_USUARIO_EMPRESA, listCotacoesAvulsasDoUsuarioEmpresa[i].DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString(),
                            listCotacoesAvulsasDoUsuarioEmpresa[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString(), descricaoGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, 0,
                            descricaoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false));
                    }
                }

                return cotacoesAvulsasEnviadasPeloUsuario;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }


        //Carregar COTAÇÕES DIRECIONADAS recebidas pelo Usuário Cotante
        private static List<CotacoesRecebidasPeloUsuario> ListaDeCotacoesDirecionadasRecebidasPeloUsuarioEmpresa()
        {
            string respondidaNãoRespondida = "";
            int fornecedoresCotados = 0;
            int quantidadeCotacoesRespondidas = 0;

            try
            {
                //Buscar as COTAÇÕES DIRECIONADAS ao Usuário
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();

                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();
                List<CotacoesRecebidasPeloUsuario> listaDeCotacoesDirecionadasRecebidasPeloUsuario = new List<CotacoesRecebidasPeloUsuario>();

                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();
                cotacao_master_usuario_cotante atualizarStatusCotacaoMasterUsuarioCotante = new cotacao_master_usuario_cotante();
                cotacao_master_usuario_empresa atualizarStatusCotacaoMasterUsuarioEmpresa = new cotacao_master_usuario_empresa();
                cotacao_master_central_compras atualizarStatusCotacaoMasterCentralDeCompras = new cotacao_master_central_compras();

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES DIRECIONADAS enviadas pelo USUARIO_COTANTE
                //----------------------------------------------------------------------------
                List<cotacao_filha_usuario_cotante> listCotacoesDirecionadasDoUsuarioCotante =
                    negociosCotacaoFilhaUsuarioCotante.ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario), Convert.ToInt32(Sessao.IdUsuarioLogado));

                if (listCotacoesDirecionadasDoUsuarioCotante.Count > 0)
                {
                    for (int i = 0; i < listCotacoesDirecionadasDoUsuarioCotante.Count; i++)
                    {
                        cotacao_master_usuario_cotante buscarDadosDaCotacaoMasterDoUsuarioCotante =
                            negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        //Buscar GRUPO de ATIVIDADES da COTAÇÃO recebida
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = buscarDadosDaCotacaoMasterDoUsuarioCotante.ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa buscarGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Verifica se a COTAÇÃO FILHA já foi RESPONDIDA ou NÃO
                        if (listCotacoesDirecionadasDoUsuarioCotante[i].RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE)
                        {
                            respondidaNãoRespondida = "respondida";
                            quantidadeCotacoesRespondidas = (quantidadeCotacoesRespondidas + 1);

                            //Atualizar o STATUS da COTAÇÃO MASTER
                            if (quantidadeCotacoesRespondidas > 0)
                            {
                                atualizarStatusCotacaoMasterUsuarioCotante = negociosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, quantidadeCotacoesRespondidas);
                            }
                        }
                        else
                        {
                            //Atualizar o STATUS da COTAÇÃO MASTER
                            if (quantidadeCotacoesRespondidas == 0)
                            {
                                atualizarStatusCotacaoMasterUsuarioCotante = negociosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, quantidadeCotacoesRespondidas);
                            }
                        }

                        if (atualizarStatusCotacaoMasterUsuarioCotante != null)
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = atualizarStatusCotacaoMasterUsuarioCotante.ID_CODIGO_STATUS_COTACAO;
                        }
                        else
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = buscarDadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO;
                        }

                        status_cotacao dadosDoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(statusDaCotacao.ID_CODIGO_STATUS_COTACAO);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        /*
                         uc --> Cotaçao originada de USUÁRIO COTANTE
                        */

                        listaDeCotacoesDirecionadasRecebidasPeloUsuario.Add(new CotacoesRecebidasPeloUsuario("uc", listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                            listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, buscarDadosDaCotacaoMasterDoUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE, respondidaNãoRespondida,
                            buscarDadosDaCotacaoMasterDoUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString(), buscarDadosDaCotacaoMasterDoUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString(),
                            buscarGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, listCotacoesDirecionadasDoUsuarioCotante[i].RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE, 0,
                            listCotacoesDirecionadasDoUsuarioCotante[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE.ToShortDateString(), dadosDoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false, ""));

                        respondidaNãoRespondida = "";
                        quantidadeCotacoesRespondidas = 0;
                    }
                }

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES DIRECIONADAS enviadas pelo USUARIO_EMPRESA
                //----------------------------------------------------------------------------
                List<cotacao_filha_usuario_empresa> listCotacoesDirecionadasDoUsuarioEmpresa =
                    negociosCotacaoFilhaUsuarioEmpresa.ConsultarCotacoesDirecionadasEnviadasParaOUsuarioEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario), Convert.ToInt32(Sessao.IdUsuarioLogado));

                if (listCotacoesDirecionadasDoUsuarioEmpresa.Count > 0)
                {
                    for (int i = 0; i < listCotacoesDirecionadasDoUsuarioEmpresa.Count; i++)
                    {
                        cotacao_master_usuario_empresa buscarDadosDaCotacaoMasterDoUsuarioEmpresa =
                            negociosCotacaoMasterUsuarioEmpresa.BuscarCotacaoMasterDoUsuarioEmpresa(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                        //Buscar GRUPO de ATIVIDADES da COTAÇÃO recebida
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = buscarDadosDaCotacaoMasterDoUsuarioEmpresa.ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa buscarGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Verifica se a COTAÇÃO FILHA já foi RESPONDIDA ou NÃO
                        if (listCotacoesDirecionadasDoUsuarioEmpresa[i].RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA)
                        {
                            respondidaNãoRespondida = "respondida";
                            quantidadeCotacoesRespondidas = (quantidadeCotacoesRespondidas + 1);

                            //Atualizar o STATUS da COTAÇÃO MASTER
                            if (quantidadeCotacoesRespondidas > 0) //CONTINUAR DAQUI...
                            {
                                atualizarStatusCotacaoMasterUsuarioEmpresa = negociosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, quantidadeCotacoesRespondidas);
                            }
                        }
                        else
                        {
                            //Atualizar o STATUS da COTAÇÃO MASTER
                            if (quantidadeCotacoesRespondidas == 0)
                            {
                                atualizarStatusCotacaoMasterUsuarioEmpresa = negociosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, quantidadeCotacoesRespondidas);
                            }
                        }

                        if (atualizarStatusCotacaoMasterUsuarioEmpresa != null)
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = atualizarStatusCotacaoMasterUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO;
                        }
                        else
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = buscarDadosDaCotacaoMasterDoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO;
                        }

                        status_cotacao dadosDoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(statusDaCotacao.ID_CODIGO_STATUS_COTACAO);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioEmpresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                        /*
                         ue --> Cotaçao originada de USUÁRIO EMPRESA
                        */

                        listaDeCotacoesDirecionadasRecebidasPeloUsuario.Add(new CotacoesRecebidasPeloUsuario("ec", listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA,
                            listCotacoesDirecionadasDoUsuarioEmpresa[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, buscarDadosDaCotacaoMasterDoUsuarioEmpresa.NOME_COTACAO_USUARIO_EMPRESA, respondidaNãoRespondida,
                            buscarDadosDaCotacaoMasterDoUsuarioEmpresa.DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString(), buscarDadosDaCotacaoMasterDoUsuarioEmpresa.DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString(),
                            buscarGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, listCotacoesDirecionadasDoUsuarioEmpresa[i].RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA, 0,
                            listCotacoesDirecionadasDoUsuarioEmpresa[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA.ToShortDateString(), dadosDoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false, ""));

                        respondidaNãoRespondida = "";
                        quantidadeCotacoesRespondidas = 0;
                    }
                }

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES DIRECIONADAS enviadas por empresas admnistradoras de CENTRAL DE COMPRAS
                //----------------------------------------------------------------------------

                //----------------------------------------------------------------------------

                return listaDeCotacoesDirecionadasRecebidasPeloUsuario;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carregar COTAÇÕES AVULSAS recebidas pelo Usuário Cotante
        private static List<CotacoesRecebidasPeloUsuario> ListaDeCotacoesAvulsasRecebidasPeloUsuarioEmpresa()
        {
            int codigoCotacaoFilha = 0;
            string respondidaNãoRespondida = "";
            int fornecedoresCotados = 0;
            int quantidadeCotacoesRespondidas = 0;

            try
            {
                //Buscar as COTAÇÕES AVULSAS do SISTEMA
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();

                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresa = new NCotacaoFilhaUsuarioEmpresaService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();
                List<CotacoesRecebidasPeloUsuario> listaDeCotacoesAvulsasParaOGrupoDeAtividadesDoUsuario = new List<CotacoesRecebidasPeloUsuario>();

                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();
                cotacao_master_usuario_cotante atualizarStatusCotacaoUsuarioCotante = new cotacao_master_usuario_cotante();
                cotacao_master_usuario_empresa atualizarStatusCotacaoUsuarioEmpresa = new cotacao_master_usuario_empresa();

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES AVULSAS enviadas pelo USUARIO_COTANTE
                //----------------------------------------------------------------------------
                List<cotacao_master_usuario_cotante> listCotacoesAvulsasDoUsuarioCotante =
                    negociosCotacaoMasterUsuarioCotante.ConsultarCotacoesAvulsasEnviadasPorUsuariosCotantesParaOUsuarioEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario), Convert.ToInt32(Sessao.IdUsuarioLogado));

                if (listCotacoesAvulsasDoUsuarioCotante.Count > 0)
                {
                    for (int i = 0; i < listCotacoesAvulsasDoUsuarioCotante.Count; i++)
                    {
                        atualizarStatusCotacaoUsuarioCotante =
                            negociosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, quantidadeCotacoesRespondidas);

                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesAvulsasDoUsuarioCotante[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa buscarGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Busca a COTAÇÃO FILHA da COTAÇÃO MASTER em questão, para o USUÁRIO LOGADO
                        cotacao_filha_usuario_cotante cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioCotante =
                            negociosCotacaoFilhaUsuarioCotante.ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        //Atualiza o número da COTAÇÃO FILHA, se tiver sido gerada pelo interesse do USUÁRIO EMPRESA que está respondendo em clicar na COTAÇÃO AVULSA
                        if (cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioCotante != null)
                        {
                            codigoCotacaoFilha = cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;

                            //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                            fornecedoresCotados = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                            //Verifica se a COTAÇÃO FILHA já foi RESPONDIDA ou NÃO
                            if ((cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioCotante.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE)
                                && (atualizarStatusCotacaoUsuarioCotante.ID_CODIGO_STATUS_COTACAO < 3))
                            {
                                respondidaNãoRespondida = "respondida";
                                quantidadeCotacoesRespondidas = (quantidadeCotacoesRespondidas + 1);

                                //Atualizar o STATUS da COTAÇÃO MASTER
                                if (quantidadeCotacoesRespondidas > 0)
                                {
                                    atualizarStatusCotacaoUsuarioCotante = negociosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, quantidadeCotacoesRespondidas);
                                }
                            }
                            else
                            {
                                //Atualizar o STATUS da COTAÇÃO MASTER
                                if (quantidadeCotacoesRespondidas == 0)
                                {
                                    atualizarStatusCotacaoUsuarioCotante = negociosCotacaoMasterUsuarioCotante.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, quantidadeCotacoesRespondidas);
                                }
                            }
                        }

                        //Consultar STATUS setado para a COTAÇÃO MASTER
                        if ((atualizarStatusCotacaoUsuarioCotante != null) && (atualizarStatusCotacaoUsuarioCotante.ID_CODIGO_STATUS_COTACAO > 0))
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = atualizarStatusCotacaoUsuarioCotante.ID_CODIGO_STATUS_COTACAO;
                        }
                        else
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_STATUS_COTACAO;
                        }

                        status_cotacao dadosDoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(statusDaCotacao.ID_CODIGO_STATUS_COTACAO);

                        /*
                         uc --> Cotaçao originada de USUÁRIO COTANTE
                        */

                        listaDeCotacoesAvulsasParaOGrupoDeAtividadesDoUsuario.Add(new CotacoesRecebidasPeloUsuario("uc", codigoCotacaoFilha, listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE,
                            listCotacoesAvulsasDoUsuarioCotante[i].NOME_COTACAO_USUARIO_COTANTE, respondidaNãoRespondida, listCotacoesAvulsasDoUsuarioCotante[i].DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString(),
                            listCotacoesAvulsasDoUsuarioCotante[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString(), buscarGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, false, 0, "1900-0-01",
                            dadosDoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false, ""));
                    }

                    codigoCotacaoFilha = 0;
                }

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES AVULSAS enviadas pelo USUARIO_EMPRESA
                //----------------------------------------------------------------------------
                List<cotacao_master_usuario_empresa> listCotacoesAvulsasDoUsuarioEmpresaCotante =
                    negociosCotacaoMasterUsuarioEmpresa.ConsultarCotacoesAvulsasEnviadasPorEmpresasParaOUsuarioEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario), Convert.ToInt32(Sessao.IdUsuarioLogado));

                if (listCotacoesAvulsasDoUsuarioEmpresaCotante.Count > 0)
                {
                    for (int i = 0; i < listCotacoesAvulsasDoUsuarioEmpresaCotante.Count; i++)
                    {
                        atualizarStatusCotacaoUsuarioEmpresa = negociosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, quantidadeCotacoesRespondidas);

                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa buscarGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Busca a COTAÇÃO FILHA da COTAÇÃO MASTER em questão, para o USUÁRIO LOGADO
                        cotacao_filha_usuario_empresa cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioEmpresaCotante =
                                    negociosCotacaoFilhaUsuarioEmpresa.ConsultarAExistenciaDeCotacaoFilhaParaACotacaoMasterEmQuestao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioEmpresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                        //Atualiza o número da COTAÇÃO FILHA, se tiver sido gerada pelo interesse do USUÁRIO EMPRESA que está respondendo em clicar na COTAÇÃO AVULSA
                        if (cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioEmpresaCotante != null)
                        {
                            codigoCotacaoFilha = cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioEmpresaCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA;

                            //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                            fornecedoresCotados = negociosCotacaoFilhaUsuarioEmpresa.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA);

                            //Verifica se a COTAÇÃO FILHA já foi RESPONDIDA ou NÃO
                            if ((cotacacaoFilhaGeradaPelaOpcaoEmResponderACotacaoAvulsaDoUsuarioEmpresaCotante.RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA)
                                && (atualizarStatusCotacaoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO < 3))
                            {
                                respondidaNãoRespondida = "respondida";
                                quantidadeCotacoesRespondidas = (quantidadeCotacoesRespondidas + 1);

                                //Atualizar o STATUS da COTAÇÃO MASTER
                                if (quantidadeCotacoesRespondidas > 0)
                                {
                                    atualizarStatusCotacaoUsuarioEmpresa = negociosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, quantidadeCotacoesRespondidas);
                                }
                            }
                            else
                            {
                                //Atualizar o STATUS da COTAÇÃO MASTER
                                if (quantidadeCotacoesRespondidas == 0)
                                {
                                    atualizarStatusCotacaoUsuarioEmpresa = negociosCotacaoMasterUsuarioEmpresa.AtualizarStatusDaCotacao(listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, quantidadeCotacoesRespondidas);
                                }
                            }
                        }

                        //Consultar STATUS setado para a COTAÇÃO MASTER
                        if ((atualizarStatusCotacaoUsuarioEmpresa != null) && (atualizarStatusCotacaoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO > 0))
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = atualizarStatusCotacaoUsuarioEmpresa.ID_CODIGO_STATUS_COTACAO;
                        }
                        else
                        {
                            statusDaCotacao.ID_CODIGO_STATUS_COTACAO = listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_STATUS_COTACAO;
                        }

                        status_cotacao dadosDoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(statusDaCotacao.ID_CODIGO_STATUS_COTACAO);

                        /*
                         uc --> Cotaçao originada de USUÁRIO EMPRESA
                        */

                        listaDeCotacoesAvulsasParaOGrupoDeAtividadesDoUsuario.Add(new CotacoesRecebidasPeloUsuario("ec", codigoCotacaoFilha, listCotacoesAvulsasDoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA,
                            listCotacoesAvulsasDoUsuarioEmpresaCotante[i].NOME_COTACAO_USUARIO_EMPRESA, respondidaNãoRespondida, listCotacoesAvulsasDoUsuarioEmpresaCotante[i].DATA_CRIACAO_COTACAO_USUARIO_EMPRESA.ToShortDateString(),
                            listCotacoesAvulsasDoUsuarioEmpresaCotante[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_EMPRESA.ToShortDateString(), buscarGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados, false, 0, "1900-0-01",
                            dadosDoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false, ""));
                    }

                    codigoCotacaoFilha = 0;
                }

                //----------------------------------------------------------------------------
                //Busca as COTAÇÕES AVULSAS enviadas pelo USUARIO_EMPRESA
                //----------------------------------------------------------------------------
                /*
                FAZER... 
                */
                //----------------------------------------------------------------------------

                return listaDeCotacoesAvulsasParaOGrupoDeAtividadesDoUsuario;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //=============================================================================
        //Carrega Lista de PRODUTOS da COTAÇÃO
        private List<ProdutosDaCotacao> BuscarProdutosDaCotacaoEnviada(int idCotacaoMaster)
        {
            string quantidadeItens;
            string valorDoProduto = string.Format("{0:0,0.00}", 0.00);
            string produtoCotado = "nao";

            //Buscar os ITENS da COTAÇÃO
            NItensCotacaoUsuarioEmpresaService negociosItensCotacaoUsuarioEmpresaCotante = new NItensCotacaoUsuarioEmpresaService();
            List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

            List<itens_cotacao_usuario_empresa> itensDaCotacaoUsuarioEmpresaCotante =
                negociosItensCotacaoUsuarioEmpresaCotante.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoMaster);

            if (itensDaCotacaoUsuarioEmpresaCotante.Count > 0)
            {
                for (int i = 0; i < itensDaCotacaoUsuarioEmpresaCotante.Count; i++)
                {
                    //Buscar dados do PRODUTO
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    produtos_servicos_empresa_profissional dadosDoProduto =
                        negociosProdutos.ConsultarDadosDoProdutoDaCotacao(itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                    //Buscar dados da MARCA do PRODUTO
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                        negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                    //Buscar dados da UNIDADE do PRODUTO
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();
                    unidades_produtos dadosDaUnidadeProduto =
                        negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_UNIDADE_PRODUTO);

                    quantidadeItens = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioEmpresaCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA); //Formata a quantidade para exibição

                    produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA, 0,
                        itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA,
                        itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA,
                        dadosDoProduto.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItens, "0",
                        dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, "", 0, "", 0, "", "", 0, 0, "", "", "", "", 0));
                }
            }

            return produtosDaCotacao;
        }
        //=============================================================================

        //Carrega Lista de PRODUTOS da COTAÇÃO, conforme o TIPO da COTAÇÃO
        private List<ProdutosDaCotacao> BuscarProdutosDaCotacao(int idCotacaoFilha, string tipoCotacao, int tipoDesconto, decimal percentualDesconto)
        {
            try
            {
                decimal quantidadeItensReal;
                string quantidadeItensFormatado;
                string valorDoProduto = "0,00";
                string produtoCotado = "nao";
                string valorTotalDoProduto = "0,00";
                decimal valorDoProdutoSemDesconto = 0;
                decimal valorDoProdutoComDesconto = 0;
                string listaFotosProdutosAlternativos = "";
                int quantidadeFotosAnexadas = 0;

                List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

                if (tipoCotacao == "uc")
                {
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoMasterUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    itens_cotacao_usuario_cotante dadosItemCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();

                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    List<itens_cotacao_filha_negociacao_usuario_cotante> itensDaCotacaoUsuarioCotante = negociosItensCotacaoFilha.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoFilha);

                    if (itensDaCotacaoUsuarioCotante.Count > 0)
                    {
                        for (int i = 0; i < itensDaCotacaoUsuarioCotante.Count; i++)
                        {
                            dadosItemCotacaoUsuarioCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE = itensDaCotacaoUsuarioCotante[i].ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE;

                            //Buscar dados dos itens na COTAÇÃO MASTER
                            itens_cotacao_usuario_cotante dadosDoProdutoDaCotacaoMaster = negociosItensCotacaoMasterUsuarioCotante.ConsultarDadosDosItensDaCotacaoFilha(dadosItemCotacaoUsuarioCotante);

                            //Buscar dados do PRODUTO
                            produtos_servicos_empresa_profissional dadosDoProdutoEmSi =
                                negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                            //Buscar dados da MARCA do PRODUTO
                            empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                                negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                            //Buscar dados da UNIDADE do PRODUTO
                            unidades_produtos dadosDaUnidadeProduto =
                                negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_UNIDADE_PRODUTO);

                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;

                                for (int a = 0; a < imagensArmazenadas.Count; a++)
                                {
                                    if (listaFotosProdutosAlternativos == "")
                                    {
                                        listaFotosProdutosAlternativos = imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                                    }
                                    else
                                    {
                                        listaFotosProdutosAlternativos = listaFotosProdutosAlternativos + "," + imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                                    }
                                }
                            }

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE); //Formata a quantidade para exibição

                            quantidadeItensReal = itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE) > 0)
                            {
                                //Calcula o VALOR TOTAL dos PRODUTOS sem desconto aplicado (Obs: Pra o caso de ocorrer DESCONTO na COTAÇÃO respondida)
                                valorDoProdutoSemDesconto = (itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE * itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE);

                                valorDoProduto = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((tipoDesconto == 1) || (tipoDesconto == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);
                                }
                                else if (tipoDesconto == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((percentualDesconto * valorDoProdutoSemDesconto) / 100);
                                    valorDoProdutoComDesconto = (valorDoProdutoSemDesconto - valorDesconto);

                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                }
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE > 0)
                            {
                                produtoCotado = "sim";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE, 0,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItensFormatado,
                                quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, valorTotalDoProduto, 0, "",
                                quantidadeFotosAnexadas, listaFotosProdutosAlternativos, "", 0, 0, "", "", "", "", 0));

                            quantidadeFotosAnexadas = 0;
                            valorDoProduto = "0,00";
                            valorTotalDoProduto = "0,00";
                        }
                    }
                }
                else if (tipoCotacao == "ec")
                {
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioEmpresaService negociosItensCotacaoMasterUsuarioEmpresa = new NItensCotacaoUsuarioEmpresaService();
                    itens_cotacao_usuario_empresa dadosItemCotacaoUsuarioEmpresa = new itens_cotacao_usuario_empresa();
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();

                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                    List<itens_cotacao_filha_negociacao_usuario_empresa> itensDaCotacaoUsuarioEmpresa = negociosItensCotacaoFilha.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoFilha);

                    if (itensDaCotacaoUsuarioEmpresa.Count > 0)
                    {
                        for (int i = 0; i < itensDaCotacaoUsuarioEmpresa.Count; i++)
                        {
                            dadosItemCotacaoUsuarioEmpresa.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA;

                            //Buscar dados dos itens na COTAÇÃO MASTER
                            itens_cotacao_usuario_empresa dadosDoProdutoDaCotacaoMaster = negociosItensCotacaoMasterUsuarioEmpresa.ConsultarDadosDosItensDaCotacaoFilha(dadosItemCotacaoUsuarioEmpresa);

                            //Buscar dados do PRODUTO
                            produtos_servicos_empresa_profissional dadosDoProdutoEmSi =
                                negociosProdutos.ConsultarDadosDoProdutoDaCotacao(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                            //Buscar dados da MARCA do PRODUTO
                            empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                                negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                            //Buscar dados da UNIDADE do PRODUTO
                            unidades_produtos dadosDaUnidadeProduto =
                                negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(dadosDoProdutoDaCotacaoMaster.ID_CODIGO_UNIDADE_PRODUTO);

                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;

                                for (int a = 0; a < imagensArmazenadas.Count; a++)
                                {
                                    if (listaFotosProdutosAlternativos == "")
                                    {
                                        listaFotosProdutosAlternativos = imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                                    }
                                    else
                                    {
                                        listaFotosProdutosAlternativos = listaFotosProdutosAlternativos + "," + imagensArmazenadas[a].NOME_ARQUIVO_IMAGEM;
                                    }
                                }
                            }

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA); //Formata a quantidade para exibição

                            quantidadeItensReal = itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA) > 0)
                            {
                                //Calcula o VALOR TOTAL dos PRODUTOS sem desconto aplicado (Obs: Pra o caso de ocorrer DESCONTO na COTAÇÃO respondida)
                                valorDoProdutoSemDesconto = (itensDaCotacaoUsuarioEmpresa[i].QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA * itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA);

                                valorDoProduto = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((tipoDesconto == 1) || (tipoDesconto == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);
                                }
                                else if (tipoDesconto == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((percentualDesconto * valorDoProdutoSemDesconto) / 100);
                                    valorDoProdutoComDesconto = (valorDoProdutoSemDesconto - valorDesconto);

                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                }
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (itensDaCotacaoUsuarioEmpresa[i].PRECO_ITENS_COTACAO_USUARIO_EMPRESA > 0)
                            {
                                produtoCotado = "sim";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioEmpresa[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA, 0,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItensFormatado,
                                quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, valorTotalDoProduto, 0, "",
                                quantidadeFotosAnexadas, listaFotosProdutosAlternativos, "", 0, 0, "", "", "", "", 0));

                            quantidadeFotosAnexadas = 0;
                            valorDoProduto = "0,00";
                            valorTotalDoProduto = "0,00";
                        }
                    }
                }
                else if (tipoCotacao == "cc")
                {
                    /*
                    CENTRAL DE COMPRAS 
                    */
                }

                return produtosDaCotacao;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega apenas os ID´s dos PRODUTOS da COTAÇÃO
        private string BuscarIdsDosProdutosDaCotacao(int idCotacaoFilha, string tipoCotacao)
        {
            //Buscar os ITENS da COTAÇÃO
            string listaIdsProdutosDaCotacao = "";

            if (tipoCotacao == "uc")
            {
                //USUÁRIO COTANTE
                NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();

                List<itens_cotacao_filha_negociacao_usuario_cotante> itensDaCotacaoUsuarioCotante = negociosItensCotacaoFilha.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoFilha);

                if (itensDaCotacaoUsuarioCotante.Count > 0)
                {
                    //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO
                    for (int i = 0; i < itensDaCotacaoUsuarioCotante.Count; i++)
                    {
                        //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO
                        listaIdsProdutosDaCotacao = (listaIdsProdutosDaCotacao + "," + itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE.ToString());
                    }
                }
            }
            else if (tipoCotacao == "ec")
            {
                //USUÁRIO EMPRESA
                NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();

                List<itens_cotacao_filha_negociacao_usuario_empresa> itensDaCotacaoUsuarioEmpresaCotante = negociosItensCotacaoFilha.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoFilha);

                if (itensDaCotacaoUsuarioEmpresaCotante.Count > 0)
                {
                    //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO
                    for (int i = 0; i < itensDaCotacaoUsuarioEmpresaCotante.Count; i++)
                    {
                        //Montando STRINGÃO de ID´s dos PRODUTOS da COTAÇÃO
                        listaIdsProdutosDaCotacao = (listaIdsProdutosDaCotacao + "," + itensDaCotacaoUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA.ToString());
                    }
                }
            }

            return listaIdsProdutosDaCotacao;
        }

        //Carrega Lista de FORNECEDORES da COTAÇÃO, conforme o TIPO da COTAÇÃO
        private List<FornecedoresCotados> BuscarFornecedoresDaCotacao(int idCotacaoMaster, string tipoCotacao)
        {
            int valor = 0;
            string respondidaSimOuNao = "NÃO";
            string mensagem = "";

            List<FornecedoresCotados> fornecedoresRespondendoACotacao = new List<FornecedoresCotados>();

            if (tipoCotacao == "uc")
            {
                //Buscar os FORNECEDORES da COTAÇÃO
                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();

                List<cotacao_filha_usuario_cotante> dadosDaCotacaoFilhaUsuarioCotante =
                    negociosCotacaoFilhaUsuarioCotante.ConsultarFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);

                if (dadosDaCotacaoFilhaUsuarioCotante.Count > 0)
                {
                    for (int i = 0; i < dadosDaCotacaoFilhaUsuarioCotante.Count; i++)
                    {
                        //Buscar dados da EMPRESA do FORNECEDOR
                        NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
                        empresa_usuario dadosDaEmpresaParaConsulta = new empresa_usuario();

                        dadosDaEmpresaParaConsulta.ID_CODIGO_EMPRESA = dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_EMPRESA;
                        empresa_usuario dadosDaEmpresa = negociosEmpresa.ConsultarDadosDaEmpresa(dadosDaEmpresaParaConsulta);

                        //Buscar dados da localização da EMPRESA do FORNECEDOR - PARTE 1
                        NEnderecoEmpresaUsuarioService negociosEndereco = new NEnderecoEmpresaUsuarioService();
                        enderecos_empresa_usuario dadosDoEnderecoParaConsulta = new enderecos_empresa_usuario();

                        dadosDoEnderecoParaConsulta.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = dadosDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                        enderecos_empresa_usuario dadosDeLocalizacaoDaEmpresaForcecedora = negociosEndereco.ConsultarDadosEnderecoEmpresaUsuario(dadosDoEnderecoParaConsulta);

                        //Buscar dados da localização da EMPRESA do FORNECEDOR - PARTE 2
                        NCidadesService negociosCidade = new NCidadesService();
                        cidades_empresa_usuario dadosDaCidade = negociosCidade.ConsultarDadosDaCidade(dadosDeLocalizacaoDaEmpresaForcecedora.ID_CIDADE_EMPRESA_USUARIO);
                        NEstadosService negociosEstados = new NEstadosService();
                        estados_empresa_usuario dadosDoEstadoUFDaEmpresa = negociosEstados.ConsultarDadosDoEstado(dadosDaCidade.ID_ESTADOS_EMPRESA_USUARIO);

                        //Verifica se existe NOVA MENSAGEM no CHAT, para setar o balão indicativo no front end
                        NChatCotacaoUsuarioEmpresaService negociosChatCotacaoUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                        List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreEmpresaCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                        //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                        List<chat_cotacao_usuario_empresa> listaConversasApuradasNoChat =
                            negociosChatCotacaoUsuarioEmpresa.BuscarChatEntreUsuarioEmpresaEFornecedor(dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE);

                        valor = listaConversasApuradasNoChat.Count;

                        if (valor > 0)
                        {
                            if (valor % 2 == 0)
                            {
                                //Par - TEM NOVA MENSAGEM
                                mensagem = "tem";
                            }
                            else
                            {
                                //Impar - NÃO TEM NOVA MENSAGEM
                                mensagem = "naoTem";
                            }
                        }
                        else
                        {
                            mensagem = "naoTem";
                        }

                        //Buscar dados do Usuário que responderá ou está respondendo a COTAÇÃO
                        NUsuarioEmpresaService negociosUsuario = new NUsuarioEmpresaService();
                        usuario_empresa dadosDoUsuario = negociosUsuario.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_USUARIO);

                        if (dadosDaCotacaoFilhaUsuarioCotante[i].RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE)
                        {
                            respondidaSimOuNao = "SIM";
                        }

                        fornecedoresRespondendoACotacao.Add(new FornecedoresCotados(dadosDaEmpresa.ID_CODIGO_EMPRESA, dadosDaEmpresa.NOME_FANTASIA_EMPRESA, dadosDaCidade.CIDADE_EMPRESA_USUARIO,
                            dadosDoEstadoUFDaEmpresa.UF_EMPRESA_USUARIO, dadosDoUsuario.NICK_NAME_USUARIO, dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                            mensagem, respondidaSimOuNao));

                        respondidaSimOuNao = "NÃO";
                    }
                }
            }
            else if (tipoCotacao == "ec")
            {
                //Buscar os FORNECEDORES da COTAÇÃO
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilhaUsuarioEmpresaCotante = new NCotacaoFilhaUsuarioEmpresaService();

                List<cotacao_filha_usuario_empresa> dadosDaCotacaoFilhaUsuarioEmpresaCotante =
                    negociosCotacaoFilhaUsuarioEmpresaCotante.ConsultarFornecedoresQueEstaoRespondendoACotacao(idCotacaoMaster);

                if (dadosDaCotacaoFilhaUsuarioEmpresaCotante.Count > 0)
                {
                    for (int i = 0; i < dadosDaCotacaoFilhaUsuarioEmpresaCotante.Count; i++)
                    {
                        //Buscar dados da EMPRESA do FORNECEDOR
                        NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
                        empresa_usuario dadosDaEmpresaParaConsulta = new empresa_usuario();

                        dadosDaEmpresaParaConsulta.ID_CODIGO_EMPRESA = dadosDaCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_EMPRESA;
                        empresa_usuario dadosDaEmpresa = negociosEmpresa.ConsultarDadosDaEmpresa(dadosDaEmpresaParaConsulta);

                        //Buscar dados da localização da EMPRESA do FORNECEDOR - PARTE 1
                        NEnderecoEmpresaUsuarioService negociosEndereco = new NEnderecoEmpresaUsuarioService();
                        enderecos_empresa_usuario dadosDoEnderecoParaConsulta = new enderecos_empresa_usuario();

                        dadosDoEnderecoParaConsulta.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = dadosDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                        enderecos_empresa_usuario dadosDeLocalizacaoDaEmpresaForcecedora = negociosEndereco.ConsultarDadosEnderecoEmpresaUsuario(dadosDoEnderecoParaConsulta);

                        //Buscar dados da localização da EMPRESA do FORNECEDOR - PARTE 2
                        NCidadesService negociosCidade = new NCidadesService();
                        cidades_empresa_usuario dadosDaCidade = negociosCidade.ConsultarDadosDaCidade(dadosDeLocalizacaoDaEmpresaForcecedora.ID_CIDADE_EMPRESA_USUARIO);
                        NEstadosService negociosEstados = new NEstadosService();
                        estados_empresa_usuario dadosDoEstadoUFDaEmpresa = negociosEstados.ConsultarDadosDoEstado(dadosDaCidade.ID_ESTADOS_EMPRESA_USUARIO);

                        //Verifica se existe NOVA MENSAGEM no CHAT, para setar o balão indicativo no front end
                        NChatCotacaoUsuarioEmpresaService negociosChatCotacaoUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                        List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreEmpresaCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                        //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                        List<chat_cotacao_usuario_empresa> listaConversasApuradasNoChat =
                            negociosChatCotacaoUsuarioEmpresa.BuscarChatEntreUsuarioEmpresaEFornecedor(dadosDaCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA);

                        valor = listaConversasApuradasNoChat.Count;

                        if (valor > 0)
                        {
                            if (valor % 2 == 0)
                            {
                                //Par - TEM NOVA MENSAGEM
                                mensagem = "tem";
                            }
                            else
                            {
                                //Impar - NÃO TEM NOVA MENSAGEM
                                mensagem = "naoTem";
                            }
                        }
                        else
                        {
                            mensagem = "naoTem";
                        }

                        //Buscar dados do Usuário que responderá ou está respondendo a COTAÇÃO
                        NUsuarioEmpresaService negociosUsuario = new NUsuarioEmpresaService();
                        usuario_empresa dadosDoUsuario = negociosUsuario.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_USUARIO);

                        if (dadosDaCotacaoFilhaUsuarioEmpresaCotante[i].RESPONDIDA_COTACAO_FILHA_USUARIO_EMPRESA)
                        {
                            respondidaSimOuNao = "SIM";
                        }

                        fornecedoresRespondendoACotacao.Add(new FornecedoresCotados(dadosDaEmpresa.ID_CODIGO_EMPRESA, dadosDaEmpresa.NOME_FANTASIA_EMPRESA, dadosDaCidade.CIDADE_EMPRESA_USUARIO,
                            dadosDoEstadoUFDaEmpresa.UF_EMPRESA_USUARIO, dadosDoUsuario.NICK_NAME_USUARIO, dadosDaCotacaoFilhaUsuarioEmpresaCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA,
                            mensagem, respondidaSimOuNao));

                        respondidaSimOuNao = "NÃO";
                    }
                }
            }
            else if (tipoCotacao == "cc")
            {
                /*
                Central de Compras 
                */
            }

            return fornecedoresRespondendoACotacao;
        }

        //Confirmação de Cadastro de Usuário Empresa
        public ActionResult ConfirmarCadastroUsuario(string login, string senha)
        {
            try
            {
                //Booleano q indica se é ou nao o primeiro acesso. Valor inicial indica q não é.
                bool primeiroAcesso = false;

                //Login para Usuário de Empresa
                NLoginService negocioLoginEmpresa = new NLoginService();
                empresa_usuario_logins confirmaCadastroLoginEmpresa = new empresa_usuario_logins();

                confirmaCadastroLoginEmpresa.LOGIN_EMPRESA_USUARIO_LOGINS = login;
                confirmaCadastroLoginEmpresa.SENHA_EMPRESA_USUARIO_LOGINS = senha;

                empresa_usuario_logins empresaUsuarioLogins =
                    negocioLoginEmpresa.ConsultarLoginUsuarioEmpresa(confirmaCadastroLoginEmpresa);

                //Se campo CADASTRO_CONFIRMADO não estiver setado como verdadeiro, então inicia-se o processo de configuração da conta
                //e em seguida a confirmação do cadastro
                if (empresaUsuarioLogins.usuario_empresa.CADASTRO_CONFIRMADO == false && empresaUsuarioLogins.usuario_empresa.USUARIO_MASTER)
                {
                    Sessao.IdUsuarioLogado = empresaUsuarioLogins.ID_CODIGO_USUARIO;
                    Sessao.IdEmpresaUsuario = empresaUsuarioLogins.usuario_empresa.empresa_usuario.ID_CODIGO_EMPRESA;

                    primeiroAcesso = true;
                }

                //Esse redirecionamento ocorrerá de qualquer forma. Se o cadastro e existência do usuário forem confirmados no
                //sistema, o método PerfilUsuarioEmpresa permitirá o acesso, caso contrário, será direcionado para a página de Login

                if (primeiroAcesso)
                {
                    return RedirectToAction("Configuracoes", "UsuarioEmpresa",
                             new
                             {
                                 tpL = 1,
                                 nmU = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.NICK_NAME_USUARIO),
                                 nmE = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.empresa_usuario.NOME_FANTASIA_EMPRESA)
                             });
                }

                return RedirectToAction("PerfilUsuarioEmpresa", "UsuarioEmpresa",
                        new
                        {
                            nmU = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.NICK_NAME_USUARIO),
                            nmE = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.empresa_usuario.NOME_FANTASIA_EMPRESA)
                        });
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Autocomplete
        public JsonResult AutoCompleteRamosAtividade(string term, int idRamoAtividade)
        {
            //Buscar lista de atividades ligadas ao grupo de atividades da empresa
            NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();

            List<produtos_servicos_empresa_profissional> listaAtividadesEmpresa =
                negocioProdutosServicosEmpresaProfissional.ListaAtividadesEmpresaProfissional(idRamoAtividade);

            var lista = (from t in listaAtividadesEmpresa
                         where t.DESCRICAO_PRODUTO_SERVICO.ToLower().Contains(term.ToLower())
                         select new { t.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS, t.DESCRICAO_PRODUTO_SERVICO }).Distinct().ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //Salvar informações da configuração
        [HttpPost]
        public ActionResult Configuracoes(CadastroEmpresa configurarPerfil, FormCollection form)
        {
            int responsavelCobranca = 0;
            int idRegistroFinanceiro = 0;
            string[] listaIDsProdutosServicos, listaDescricaoProdutosServicos;

            listaIDsProdutosServicos = configurarPerfil.RAMOS_ATIVIDADES_SELECIONADOS.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            listaDescricaoProdutosServicos =
                configurarPerfil.DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS_ORIGINAL.Split(",".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

            //Salvando os dados da configuração para a EMPRESA através do USUÁRIO MASTER
            try
            {
                //Inserir novos Produtos / Serviços vinculados a um Grupo de Atividades
                NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();
                produtos_servicos_empresa_profissional novoProdutoServicoEmpresaProfissional = new produtos_servicos_empresa_profissional();

                //Atividades, produtos e serviços praticados, vendidos ou prestados pela empresa
                NAtividadesEmpresaService negocioAtividadesEmpresa = new NAtividadesEmpresaService();
                atividades_empresa novasAtividadesEmpresa = new atividades_empresa();

                //Configurar atividades
                for (int i = 0; i < listaIDsProdutosServicos.LongLength; i++)
                {
                    //Salvando os novos Produtos/Serviços na tabela PRODUTOS_SERVICOS_EMPRESA_PROFISSIONAL, se ainda não existirem no banco
                    if (listaIDsProdutosServicos[i].IndexOf('a') > 0)
                    {
                        novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = configurarPerfil.ID_GRUPO_ATIVIDADES;
                        novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoProdutosServicos[i];

                        produtos_servicos_empresa_profissional produtosServicosEmpresaProfissional =
                            negocioProdutosServicosEmpresaProfissional.GravarNovoProdutoServicoEmpresaProfissional(novoProdutoServicoEmpresaProfissional);

                        if (produtosServicosEmpresaProfissional != null)
                        {
                            //Troca ID temporário pelo ID real, obtido na gravação do Produto/Serviço
                            listaIDsProdutosServicos[i] = produtosServicosEmpresaProfissional.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS.ToString();
                        }
                    }

                    //Salvando as atividades, produtos e serviços pela empresa, na tabela ATIVIDADES_EMPRESA
                    novasAtividadesEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]);
                    novasAtividadesEmpresa.ID_GRUPO_ATIVIDADES = configurarPerfil.ID_GRUPO_ATIVIDADES;
                    novasAtividadesEmpresa.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = Convert.ToInt32(listaIDsProdutosServicos[i]);

                    atividades_empresa atividadesEmpresa =
                        negocioAtividadesEmpresa.GravarAtividadeProdutoServicoEmpresa(novasAtividadesEmpresa);
                }

                //Salvar dados financeiros
                if (configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA != 1) //Trata dos tipos de contrato que exijam pagamento
                {
                    //Salvando e gerando o registro financeiro da cobrança para a empresa / usuário
                    NFinanceiroCobrancaFaturamentoUsuarioEmpresaService negocioFinanceiroCobrancaFaturamentoUsuarioEmpresa =
                        new NFinanceiroCobrancaFaturamentoUsuarioEmpresaService();
                    financeiro_cobranca_faturamento_usuario_empresa novoCobrancaFaturamentoUsuarioEmpresa =
                        new financeiro_cobranca_faturamento_usuario_empresa();

                    //Atribui o responsável(quem será cobrado) de acordo com o tipo de contrato (1-Empresa / 2-Usuário Master)
                    if (configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA == 2)
                    {
                        //Usuários cadastrados
                        responsavelCobranca = 2;
                    }
                    else if ((configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA == 3) || (configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA == 4))
                    {
                        //Empresa
                        responsavelCobranca = 1;
                    }

                    novoCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA =
                        configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA;
                    novoCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_EMPRESA =
                        Convert.ToInt32(Session["IdEmpresaUsuario"]);
                    novoCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Session["IdUsuarioLogado"]);
                    novoCobrancaFaturamentoUsuarioEmpresa.ID_MEIO_PAGAMENTO = configurarPerfil.ID_MEIO_PAGAMENTO;
                    novoCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA = DateTime.Now;
                    novoCobrancaFaturamentoUsuarioEmpresa.DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO = 5;
                    novoCobrancaFaturamentoUsuarioEmpresa.PARCELA_PAGA_COBRANCA_FATURAMENTO = false;
                    novoCobrancaFaturamentoUsuarioEmpresa.DATA_PAGAMENTO_COBRANCA_FATURAMENTO =
                        Convert.ToDateTime("01/01/1900");   //Estou inserindo a data assim pois não sei ainda pq esta data está como requerida na tabela.
                    novoCobrancaFaturamentoUsuarioEmpresa.USUARIO_PAGARA_COBRANCA_FATURAMENTO = responsavelCobranca;

                    financeiro_cobranca_faturamento_usuario_empresa financeiroCobrancaFaturamentoUsuarioEmpresa =
                        negocioFinanceiroCobrancaFaturamentoUsuarioEmpresa.GravaCobrancaUsuarioEmpresa(
                            novoCobrancaFaturamentoUsuarioEmpresa);

                    if (financeiroCobrancaFaturamentoUsuarioEmpresa != null)
                    {
                        //Armazenda o identificador do registro financeiro gerado para planos cobrados
                        idRegistroFinanceiro = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_COBRANCA_FATURAMENTO_USUARIO_EMPRESA;
                    }

                    //Altera o tipo de contrato na Empresa (que por default foi cadastrada como Empresa Cotante)
                    NEmpresaUsuarioService negocioEmpresaUsuario = new NEmpresaUsuarioService();
                    empresa_usuario atualizaTipoDeContrato = new empresa_usuario();

                    atualizaTipoDeContrato.ID_CODIGO_TIPO_CONTRATO_COTADA = configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA;
                    atualizaTipoDeContrato.ID_GRUPO_ATIVIDADES = configurarPerfil.ID_GRUPO_ATIVIDADES;
                    atualizaTipoDeContrato.ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]);

                    empresa_usuario atualizandoTipoDeContratoNaEmpresa =
                        negocioEmpresaUsuario.AtualizarTipoDeContratoNaEmpresa(atualizaTipoDeContrato);

                    //Confirma o cadastro do Usuário após especificados os dados de configurações de Atividades / Assinatura
                    usuario_empresa confirmaCadastroUsuarioEmpresa = new usuario_empresa();

                    confirmaCadastroUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    usuario_empresa confirmandoCadastro =
                        negocioEmpresaUsuario.ConfirmarCadastroUsuarioEmpresa(confirmaCadastroUsuarioEmpresa);

                    //Redireciona para a emissão da cobrança
                    if (confirmandoCadastro != null)
                    {
                        return RedirectToAction("ValidarAssinatura", "FinanceiroClienteMercado",
                             new
                             {
                                 tpL = configurarPerfil.TIPO_LOGIN,
                                 nmU = MD5Crypt.Criptografar(configurarPerfil.NOME_USUARIO_MASTER),
                                 nmE = MD5Crypt.Criptografar(configurarPerfil.NOME_FANTASIA_EMPRESA),
                                 dPc = MD5Crypt.Criptografar(configurarPerfil.DESCRICAO_TIPO_CONTRATO_COTADA),
                                 vPc = MD5Crypt.Criptografar(configurarPerfil.VALOR_PLANO_CONTRATADO.ToString() + "00"),
                                 iRf = MD5Crypt.Criptografar(idRegistroFinanceiro.ToString())
                             });
                    }

                }
                else if (configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA == 1)  //Trata o tipo de contrato que não exige pagamento
                {
                    //Confirma o cadastro do Usuário após especificados os dados de configurações financeiras e afins
                    NEmpresaUsuarioService negocioEmpresaUsuario = new NEmpresaUsuarioService();
                    usuario_empresa confirmaCadastroUsuarioEmpresa = new usuario_empresa();

                    confirmaCadastroUsuarioEmpresa.ID_CODIGO_USUARIO = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    usuario_empresa confirmandoCadastro =
                        negocioEmpresaUsuario.ConfirmarCadastroUsuarioEmpresa(confirmaCadastroUsuarioEmpresa);

                    if (confirmandoCadastro != null)
                    {
                        return RedirectToAction("PerfilUsuarioEmpresa", "UsuarioEmpresa",
                             new
                             {
                                 nmU = MD5Crypt.Criptografar(configurarPerfil.NOME_USUARIO_MASTER),
                                 nmE = MD5Crypt.Criptografar(configurarPerfil.NOME_FANTASIA_EMPRESA)
                             });
                    }
                }
            }
            catch (Exception erro)
            {
                //throw erro;
                ModelState.AddModelError("", "Ocorreu um erro ao gravar os dados de configurações (Atividades / Planos Assinatura) da empresa!");
            }

            return null;
        }

        //UPLOAD de arquivos - FOTOS dos PRODUTOS ALTERNATIVOS
        [HttpPost]
        public ContentResult UploadFiles()
        {
            try
            {
                var r = new List<UploadFilesResult>();
                string validacaoImagem = "";
                string novoNomeDoArquivo = "";
                string caminhoFisicoDaImagem = "";

                string pastaCodigoEmpresa = Sessao.IdEmpresaUsuario.ToString();
                string pastaCodigoUsuario = Sessao.IdUsuarioLogado.ToString();
                string caminhoFotosProdutosAlternativos = System.AppDomain.CurrentDomain.BaseDirectory.ToString(); //Pega o caminho físico do PROJETO, para ser usado na montagem do caminho real de armaz.

                //Montando o caminho de armazenamento a ser confirmada existência
                caminhoFotosProdutosAlternativos = (caminhoFotosProdutosAlternativos + "Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa);

                //Verifica se a estrutura de pastas de armazenamento está criada. Se não existir, cria imediatamente
                DirectoryInfo dirEmpresa = new DirectoryInfo(caminhoFotosProdutosAlternativos);

                //Cria as PASTAS
                if (dirEmpresa.Exists == false)
                {
                    //Criando a pasta da EMPRESA LOGADA, que ainda não existe
                    dirEmpresa.Create();

                    //Criando a pasta que armazenará as imagens alternativas do USUÁRIO que está respondendo a COTAÇÃO, para todas as cotações respondidas por ele
                    caminhoFotosProdutosAlternativos = (caminhoFotosProdutosAlternativos + "/" + pastaCodigoUsuario);

                    DirectoryInfo dirUsuario = new DirectoryInfo(caminhoFotosProdutosAlternativos);

                    if (dirUsuario.Exists == false)
                    {
                        //Criando a pasta do USUÁRIO LOGADO, que ainda não existe
                        dirUsuario.Create();
                    }
                }

                //Realizando o UPLOAD
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                        continue;

                    // VALIDAR a extensão da imagem do upload
                    string extensao = Path.GetExtension(hpf.FileName);
                    string[] extensoesValidas = new string[] { ".jpg", ".jpeg", ".png", ".JPG", ".JPEG", ".PNG" };

                    if (!extensoesValidas.Contains(extensao))
                    {
                        //IMAGEM de upload NÃO É VÁLIDA
                        validacaoImagem = "nok";
                    }
                    else
                    {
                        //IMAGEM de upload É VÁLIDA
                        validacaoImagem = "ok";

                        //Salva o arquivo na pasta
                        string savedFileName = Path.Combine(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario), Path.GetFileName(hpf.FileName));
                        hpf.SaveAs(savedFileName);

                        //Comprimindo a IMAGEM (mantém a qualidade e ocupa menos espaço físico no disco)
                        using (Bitmap img = new Bitmap(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + hpf.FileName)))
                        {
                            //CodecInfo para imagens Jpeg
                            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().First(enc => enc.FormatID == ImageFormat.Jpeg.Guid);
                            //EncoderParameters que vai setar o nível de qualidade (compressão)
                            EncoderParameters imgParams = new EncoderParameters(1);
                            //Qualidade em 30L = bom nível de qualidade e compressão
                            imgParams.Param = new[] { new EncoderParameter(Encoder.Quality, 30L) };

                            //Renomear arquivo, codificando pra evitar nomes repetidos (padrão Facebook)
                            novoNomeDoArquivo = CodificarNomeDeArquivo.renomearComHash(hpf.FileName);

                            //Salvar a imagem comprimida
                            img.Save(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + novoNomeDoArquivo), codec, imgParams);
                        }

                        //Deve EXCLUIR o arquivo original, pra ficar só o comprimido
                        FileInfo arquivoADeletar = new FileInfo(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + hpf.FileName));
                        arquivoADeletar.Delete();

                        caminhoFisicoDaImagem = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + novoNomeDoArquivo);
                    }

                    r.Add(new UploadFilesResult()
                    {
                        Name = novoNomeDoArquivo,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        caminhoDaImagem = caminhoFisicoDaImagem,
                        aceitouImagem = validacaoImagem
                    });
                }

                //Retorno JSon
                return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\",\"caminhoImg\":\"" + r[0].caminhoDaImagem + "\",\"imagemAceita\":\"" + r[0].aceitouImagem + "\"}", "application/json");
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Grava as IMAGENS dos PRODUTOS ALTERNATIVOS em tabela
        [WebMethod]
        public ActionResult GravarImagemProdutoAlternativo(int idProdutoCotacao, string nomeDaImagem, string tipoCotacao)
        {
            string gravado = "nok";

            try
            {
                if (tipoCotacao == "uc")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosProdutosAlternativosUsuarioCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();
                    fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante dadosImagensProdutosAlternativosUsuarioCotante =
                        new fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante();

                    //Gravação das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO COTANTE
                    dadosImagensProdutosAlternativosUsuarioCotante.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE = idProdutoCotacao;
                    dadosImagensProdutosAlternativosUsuarioCotante.NOME_ARQUIVO_IMAGEM = nomeDaImagem;

                    fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante itemAlternativoGravado =
                        negociosProdutosAlternativosUsuarioCotante.GravarProdutoAlternativoNaCotacaoDoUsuarioCotante(dadosImagensProdutosAlternativosUsuarioCotante);

                    if (itemAlternativoGravado != null)
                    {
                        gravado = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itemGravado = gravado,
                        idDaImagemProduto = itemAlternativoGravado.ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                }
                else if (tipoCotacao == "ec")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosProdutosAlternativosUsuarioEmpresaCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();
                    fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa dadosImagensProdutosAlternativosUsuarioEmpresaCotante =
                        new fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa();

                    //Gravação das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO COTANTE
                    dadosImagensProdutosAlternativosUsuarioEmpresaCotante.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA = idProdutoCotacao;
                    dadosImagensProdutosAlternativosUsuarioEmpresaCotante.NOME_ARQUIVO_IMAGEM = nomeDaImagem;

                    fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa itemAlternativoGravado =
                        negociosProdutosAlternativosUsuarioEmpresaCotante.GravarProdutoAlternativoNaCotacaoDoUsuarioEmpresa(dadosImagensProdutosAlternativosUsuarioEmpresaCotante);

                    if (itemAlternativoGravado != null)
                    {
                        gravado = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itemGravado = gravado,
                        idDaImagemProduto = itemAlternativoGravado.ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                }

                return null;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //EXCLUI da tabela do banco e fisicamente as IMAGENS selecionadas, dos PRODUTOS ALTERNATIVOS
        [WebMethod]
        public ActionResult ExcluirImagemProdutoAlternativo(int idImagemPRodutoAlternativo, string nomeDaImagem, string tipoCotacao)
        {
            string excluido = "nok";
            string pastaCodigoEmpresa = Sessao.IdEmpresaUsuario.ToString();
            string pastaCodigoUsuario = Sessao.IdUsuarioLogado.ToString();

            try
            {
                if (tipoCotacao == "uc")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosProdutosAlternativosUsuarioCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    //Exclusão das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO COTANTE
                    bool exclusao = negociosProdutosAlternativosUsuarioCotante.ExcluirProdutoAlternativoNaCotacaoDoUsuarioCotante(idImagemPRodutoAlternativo);

                    if (exclusao)
                    {
                        //EXCLUIR fisicamnete a IMAGEM selecionada
                        FileInfo arquivoADeletar = new FileInfo(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + nomeDaImagem));
                        arquivoADeletar.Delete();

                        excluido = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itemExcluido = excluido
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
                else if (tipoCotacao == "ec")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosProdutosAlternativosUsuarioEmpresaCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                    //Exclusão das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO EMPRESA
                    bool exclusao = negociosProdutosAlternativosUsuarioEmpresaCotante.ExcluirProdutoAlternativoNaCotacaoDoUsuarioEmpresa(idImagemPRodutoAlternativo);

                    if (exclusao)
                    {
                        //EXCLUIR fisicamnete a IMAGEM selecionada
                        FileInfo arquivoADeletar = new FileInfo(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + nomeDaImagem));
                        arquivoADeletar.Delete();

                        excluido = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itemExcluido = excluido
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //EXCLUI da tabela do banco e fisicamente as IMAGENS selecionadas, dos PRODUTOS ALTERNATIVOS
        [WebMethod]
        public ActionResult ExcluirTodasAsImagensProdutoAlternativo(int idProdutoCotacao, string listaDeNomesDasImagens, string tipoCotacao)
        {
            string excluidos = "nok";
            string pastaCodigoEmpresa = Sessao.IdEmpresaUsuario.ToString();
            string pastaCodigoUsuario = Sessao.IdUsuarioLogado.ToString();

            try
            {
                if (tipoCotacao == "uc")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosProdutosAlternativosUsuarioCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    //Exclusão das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO COTANTE
                    string imagensExclusao = negociosProdutosAlternativosUsuarioCotante.ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(idProdutoCotacao);

                    if (imagensExclusao != null)
                    {
                        string[] nomesImagensExclusao = imagensExclusao.Split(',');

                        for (int i = 0; i < nomesImagensExclusao.Length; i++)
                        {
                            //EXCLUIR fisicamnete a IMAGEM selecionada
                            FileInfo arquivoADeletar = new FileInfo(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + nomesImagensExclusao[i]));
                            arquivoADeletar.Delete();
                        }

                        excluidos = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itensExcluidos = excluidos
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
                else if (tipoCotacao == "ec")
                {
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosProdutosAlternativosUsuarioEmpresaCotante =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();

                    //Exclusão das IMAGENS dos PRODUTOS ALTERNATIVOS para resposta de COTAÇÃO para o USUÁRIO EMPRESA
                    string imagensExclusao = negociosProdutosAlternativosUsuarioEmpresaCotante.ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(idProdutoCotacao);

                    if (imagensExclusao != null)
                    {
                        string[] nomesImagensExclusao = imagensExclusao.Split(',');

                        for (int i = 0; i < nomesImagensExclusao.Length; i++)
                        {
                            //EXCLUIR fisicamnete a IMAGEM selecionada
                            FileInfo arquivoADeletar = new FileInfo(Server.MapPath("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + nomesImagensExclusao[i]));
                            arquivoADeletar.Delete();
                        }

                        excluidos = "ok";
                    }

                    //Montando o parâmetro a ser retornado
                    var resultado = new
                    {
                        itensExcluidos = excluidos
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //Armazena as imagens de PRODUTOS ALTERNATIVOS às opções solicitadas na COTAÇÃO
        [WebMethod]
        public ActionResult CarregarImagensDeProdutosAlternativosAnexadas(int idProdutoCotacao, string tipoCotacao)
        {
            string pastaCodigoEmpresa = Sessao.IdEmpresaUsuario.ToString();
            string pastaCodigoUsuario = Sessao.IdUsuarioLogado.ToString();

            try
            {
                if (tipoCotacao == "uc")
                {
                    //Armanzena imagens dos PRODUTOS ALTERNATIVOS em resposta ao USUÁRIO COTANTE
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();
                    List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> fotosAnexadasDeProdutosAlternativos =
                        new List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante>();

                    List<FotosProdutosAlternativos> fotosProdutosAlternativosAnexados = new List<FotosProdutosAlternativos>();

                    //Carregando as IMAGENS de PRODUTOS ALTERNATIVOS
                    fotosAnexadasDeProdutosAlternativos = negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(idProdutoCotacao);

                    if (fotosAnexadasDeProdutosAlternativos.Count > 0)
                    {
                        if (fotosAnexadasDeProdutosAlternativos.Count == 1)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo2 = "",
                                idDaImagemProduto2 = "",
                                imagemProdutoAlternativo3 = "",
                                idDaImagemProduto3 = "",
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 2)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo3 = "",
                                idDaImagemProduto3 = "",
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = "",
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 3)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = "",
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 4)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo4 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[3].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto4 = fotosAnexadasDeProdutosAlternativos[3].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 5)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo4 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[3].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto4 = fotosAnexadasDeProdutosAlternativos[3].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE,
                                imagemProdutoAlternativo5 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[4].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto5 = fotosAnexadasDeProdutosAlternativos[4].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else if (tipoCotacao == "ec")
                {
                    //Armanzena imagens dos PRODUTOS ALTERNATIVOS em resposta ao USUÁRIO EMPRESA
                    NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService();
                    List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> fotosAnexadasDeProdutosAlternativos =
                        new List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa>();

                    List<FotosProdutosAlternativos> fotosProdutosAlternativosAnexados = new List<FotosProdutosAlternativos>();

                    //Carregando as IMAGENS de PRODUTOS ALTERNATIVOS
                    fotosAnexadasDeProdutosAlternativos = negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(idProdutoCotacao);

                    if (fotosAnexadasDeProdutosAlternativos.Count > 0)
                    {
                        if (fotosAnexadasDeProdutosAlternativos.Count == 1)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo2 = "",
                                idDaImagemProduto2 = "",
                                imagemProdutoAlternativo3 = "",
                                idDaImagemProduto3 = "",
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 2)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo3 = "",
                                idDaImagemProduto3 = "",
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = "",
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 3)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo4 = "",
                                idDaImagemProduto4 = "",
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = "",
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 4)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo4 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[3].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto4 = fotosAnexadasDeProdutosAlternativos[3].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo5 = "",
                                idDaImagemProduto5 = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                        else if (fotosAnexadasDeProdutosAlternativos.Count == 5)
                        {
                            var resultado = new
                            {
                                imagemProdutoAlternativo1 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[0].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto1 = fotosAnexadasDeProdutosAlternativos[0].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo2 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[1].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto2 = fotosAnexadasDeProdutosAlternativos[1].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo3 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[2].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto3 = fotosAnexadasDeProdutosAlternativos[2].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo4 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[3].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto4 = fotosAnexadasDeProdutosAlternativos[3].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA,
                                imagemProdutoAlternativo5 = @Url.Content("~/Content/imagensProdutosAlternativos/" + pastaCodigoEmpresa + "/" + pastaCodigoUsuario + "/" + fotosAnexadasDeProdutosAlternativos[4].NOME_ARQUIVO_IMAGEM),
                                idDaImagemProduto5 = fotosAnexadasDeProdutosAlternativos[4].ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //Método RESPONDE a COTAÇÃO enviada pelo USUÁRIO COTANTE
        [WebMethod]
        public ActionResult EnviarARespostaDaCotacao(int idCotacaoMaster, int idCotacaoFilha, string idsProdutosSelecionadosResposta, string valoresUnitariosRespostaDaCotacao, decimal valorTotalDoLoteSemDesconto,
            string formaDePagamento, string tipoDeFrete, string tipoDeDesconto, string percentualDeDesconto, string observacao, string tipoDaResposta)
        {
            string itemRespondido = "nok";
            string[] listaIdsProdutosSelecionados, listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante;
            double percentualDeCotacoesRespondidas = 0;

            listaIdsProdutosSelecionados = idsProdutosSelecionadosResposta.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante = valoresUnitariosRespostaDaCotacao.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            if (tipoDaResposta == "uc")
            {
                //RESPONDER a COTAÇÃO enviada pelo USUÁRIO COTANTE
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilha = new NCotacaoFilhaUsuarioCotanteService();
                NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                NUsuarioCotanteService negociosUsuarioCotante = new NUsuarioCotanteService();
                cotacao_filha_usuario_cotante cotacaoFilhaASerRespondida = new cotacao_filha_usuario_cotante();
                itens_cotacao_filha_negociacao_usuario_cotante itensDaCotacaoNegociacaoASerRespondidos = new itens_cotacao_filha_negociacao_usuario_cotante();
                usuario_cotante dadosUsuarioCotante = new usuario_cotante();

                try
                {
                    //Busca dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                    cotacao_master_usuario_cotante dadosCotacaoMaster = negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(idCotacaoMaster);

                    if (listaIdsProdutosSelecionados.Length > 0)
                    {
                        //Gravado os valores relacionados ao VALOR TOTAL do LOTE de mercadorias e DESCONTOS (TIPO e PERCENTUAL)
                        cotacaoFilhaASerRespondida.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                        cotacaoFilhaASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = DateTime.Now;
                        cotacaoFilhaASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE = formaDePagamento;
                        cotacaoFilhaASerRespondida.ID_TIPO_FRETE = Convert.ToInt32(tipoDeFrete);
                        cotacaoFilhaASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = valorTotalDoLoteSemDesconto;
                        cotacaoFilhaASerRespondida.TIPO_DESCONTO = Convert.ToInt32(tipoDeDesconto);
                        cotacaoFilhaASerRespondida.PERCENTUAL_DESCONTO = Convert.ToDecimal(percentualDeDesconto);
                        cotacaoFilhaASerRespondida.OBSERVACAO_COTACAO_USUARIO_COTANTE = observacao;
                        cotacaoFilhaASerRespondida.COTACAO_FILHA_USUARIO_COTANTE_EDITADA = false;

                        cotacao_filha_usuario_cotante respondendoCotacaoUsuarioCotante1 =
                            negociosCotacaoFilha.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioCotante(cotacaoFilhaASerRespondida);

                        //Gravar RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE nos ITENS da COTAÇÃO
                        for (int i = 0; i < listaIdsProdutosSelecionados.Length; i++)
                        {
                            //Gravando os PREÇOS dos itens da COTAÇÃO
                            itensDaCotacaoNegociacaoASerRespondidos.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE = Convert.ToInt32(listaIdsProdutosSelecionados[i]);
                            itensDaCotacaoNegociacaoASerRespondidos.PRECO_ITENS_COTACAO_USUARIO_COTANTE = Convert.ToDecimal(listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante[i], new CultureInfo("en-US"));

                            itens_cotacao_filha_negociacao_usuario_cotante respondendoCotacaoUsuarioCotante2 =
                                negociosItensCotacaoFilha.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioCotante(itensDaCotacaoNegociacaoASerRespondidos, idCotacaoFilha, 0);

                            if (respondendoCotacaoUsuarioCotante2 != null)
                            {
                                itemRespondido = "ok";
                            }
                        }

                        //Checar que PERCENTUAL da COTAÇÃO já foi respondido / ATUALIZAR STATUS da COTAÇÃO
                        percentualDeCotacoesRespondidas = negociosCotacaoFilha.ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioCotante(idCotacaoMaster);

                        if (percentualDeCotacoesRespondidas >= 50.01)
                        {
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            //Buscar dados do USUÁRIO COTANTE para avisar sobre percentual de respostas
                            dadosUsuarioCotante.ID_CODIGO_USUARIO_COTANTE = dadosCotacaoMaster.ID_CODIGO_USUARIO_COTANTE;

                            usuario_cotante dadosParaComunicacaoAoUsuarioCotante = negociosUsuarioCotante.ConsultarDadosDoUsuarioCotante(dadosUsuarioCotante);

                            //1 - Enviar SMS
                            //Envio de SMS informando ao USUÁRIO COTANTE que ele possui pouco mais da METADE das COTAÇÕES enviadas RESPONDIDAS (diz respeito a esta COTAÇÃO MASTER)
                            string smsMensagem = "ClienteMercado - Sua COTACAO foi respondida por metade dos FORNECEDORES. Veja o aplicativo no celular ou acesse www.clientemercado.com.br.";

                            string telefoneUsuarioCotante = Regex.Replace(dadosParaComunicacaoAoUsuarioCotante.TELEFONE1_USUARIO_COTANTE, "[()-]", "");
                            string urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioCotante;

                            //bool smsUsuarioCotante = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioCotante));

                            //if (smsUsuarioCotante)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMS negociosSMS = new NControleSMS();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosParaComunicacaoAoUsuarioCotante.TELEFONE1_USUARIO_COTANTE;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosCotacaoMaster.ID_CODIGO_USUARIO_COTANTE;
                            //    controleEnvioSms.MOTIVO_ENVIO = 3; //Valor default. 2 - Informar que Cotações foram respondidas (Criar uma tabela com esses valores para referência/leitura)

                            //    controle_sms_usuario_empresa controleSmsUsuarioEmpresa = negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        //Retorna dados do Centro de Custo
                        var resultado = new
                        {
                            itemRespondido = itemRespondido
                        };

                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoDaResposta == "ec")
            {
                //RESPONDER a COTAÇÃO enviada pelo EMPRESA COTANTE
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilha = new NCotacaoFilhaUsuarioEmpresaService();
                NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                cotacao_filha_usuario_empresa cotacaoFilhaASerRespondida = new cotacao_filha_usuario_empresa();
                itens_cotacao_filha_negociacao_usuario_empresa itensDaCotacaoNegociacaoASerRespondidos = new itens_cotacao_filha_negociacao_usuario_empresa();
                usuario_empresa dadosUsuarioEmpresaCotante = new usuario_empresa();

                try
                {
                    //Busca dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                    cotacao_master_usuario_empresa dadosCotacaoMaster = negociosCotacaoMasterUsuarioEmpresa.BuscarCotacaoMasterDoUsuarioEmpresa(idCotacaoMaster);

                    if (listaIdsProdutosSelecionados.Length > 0)
                    {
                        //Gravado os valores relacionados ao VALOR TOTAL do LOTE de mercadorias e DESCONTOS (TIPO e PERCENTUAL)
                        cotacaoFilhaASerRespondida.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                        cotacaoFilhaASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = DateTime.Now;
                        cotacaoFilhaASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA = formaDePagamento;
                        cotacaoFilhaASerRespondida.ID_TIPO_FRETE = Convert.ToInt32(tipoDeFrete);
                        cotacaoFilhaASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = valorTotalDoLoteSemDesconto;
                        cotacaoFilhaASerRespondida.TIPO_DESCONTO = Convert.ToInt32(tipoDeDesconto);
                        cotacaoFilhaASerRespondida.PERCENTUAL_DESCONTO = Convert.ToDecimal(percentualDeDesconto);
                        cotacaoFilhaASerRespondida.OBSERVACAO_COTACAO_USUARIO_EMPRESA = observacao;
                        cotacaoFilhaASerRespondida.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA = false;

                        cotacao_filha_usuario_empresa respondendoCotacaoUsuarioEmpresa1 =
                            negociosCotacaoFilha.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioEmpresa(cotacaoFilhaASerRespondida);

                        //Gravar RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE nos ITENS da COTAÇÃO
                        for (int i = 0; i < listaIdsProdutosSelecionados.Length; i++)
                        {
                            //Gravando os PREÇOS dos itens da COTAÇÃO
                            itensDaCotacaoNegociacaoASerRespondidos.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA = Convert.ToInt32(listaIdsProdutosSelecionados[i]);
                            itensDaCotacaoNegociacaoASerRespondidos.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante[i], new CultureInfo("en-US"));

                            itens_cotacao_filha_negociacao_usuario_empresa respondendoCotacaoUsuarioEmpresa2 =
                                negociosItensCotacaoFilha.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioEmpresa(itensDaCotacaoNegociacaoASerRespondidos, idCotacaoFilha, 0);

                            if (respondendoCotacaoUsuarioEmpresa2 != null)
                            {
                                itemRespondido = "ok";
                            }
                        }

                        //Checar que PERCENTUAL da COTAÇÃO já foi respondido / ATUALIZAR STATUS da COTAÇÃO
                        percentualDeCotacoesRespondidas = negociosCotacaoFilha.ConsultarPercentualJaRespondidoDestaCotacaoAoUsuarioEmpresa(idCotacaoMaster);

                        if (percentualDeCotacoesRespondidas >= 50.01)
                        {
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            //Buscar dados do USUÁRIO EMPRESA para avisar sobre percentual de respostas
                            dadosUsuarioEmpresaCotante.ID_CODIGO_USUARIO = dadosCotacaoMaster.ID_CODIGO_USUARIO;

                            usuario_empresa dadosParaComunicacaoAoUsuarioEmpresa =
                                negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosUsuarioEmpresaCotante.ID_CODIGO_USUARIO);

                            //1 - Enviar SMS
                            //Envio de SMS informando ao USUÁRIO EMPRESA que ele possui pouco mais da METADE das COTAÇÕES enviadas RESPONDIDAS (diz respeito a esta COTAÇÃO MASTER)
                            string smsMensagem = "ClienteMercado - Sua COTACAO foi respondida por metade dos FORNECEDORES. Veja o aplicativo no celular ou acesse www.clientemercado.com.br.";

                            string telefoneUsuarioCotante = Regex.Replace(dadosParaComunicacaoAoUsuarioEmpresa.TELEFONE1_USUARIO_EMPRESA, "[()-]", "");
                            string urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioCotante;

                            //bool smsUsuarioEmpresaCotante = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioCotante));

                            //if (smsUsuarioEmpresaCotante)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMS negociosSMS = new NControleSMS();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosParaComunicacaoAoUsuarioEmpresa.TELEFONE1_USUARIO_EMPRESA;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosCotacaoMaster.ID_CODIGO_USUARIO;
                            //    controleEnvioSms.MOTIVO_ENVIO = 3; //Valor default. 2 - Informar que Cotações foram respondidas (Criar uma tabela com esses valores para referência/leitura)

                            //    controle_sms_usuario_empresa controleSmsUsuarioEmpresa = negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        //Retorna dados do Centro de Custo
                        var resultado = new
                        {
                            itemRespondido = itemRespondido
                        };

                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception erro)
                {
                    throw erro;
                }

            }
            else if (tipoDaResposta == "cc")
            {
                /*
                CENTRAL DE COMPRAS... 
                */
            }

            return null;
        }

        //Método EDITAR a RESPOSTA da COTAÇÃO enviada pelo USUÁRIO COTANTE
        [WebMethod]
        public ActionResult EditarARespostaDaCotacao(int idCotacaoMaster, int idCotacaoFilha, string idsProdutosSelecionadosResposta, string valoresUnitariosRespostaDaCotacao, decimal valorTotalDoLoteSemDesconto,
            string formaDePagamento, string tipoDeFrete, string tipoDeDesconto, string percentualDeDesconto, string observacao, string tipoDaResposta)
        {
            string itemRespondido = "nok";
            string[] listaIdsProdutosSelecionados, listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante;
            double percentualDeCotacoesRespondidas = 0;

            listaIdsProdutosSelecionados = idsProdutosSelecionadosResposta.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante = valoresUnitariosRespostaDaCotacao.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            if (tipoDaResposta == "uc")
            {
                //EDITAR a COTAÇÃO enviada pelo USUÁRIO COTANTE
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilha = new NCotacaoFilhaUsuarioCotanteService();
                NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                NUsuarioCotanteService negociosUsuarioCotante = new NUsuarioCotanteService();
                cotacao_filha_usuario_cotante cotacaoFilhaASerRespondida = new cotacao_filha_usuario_cotante();
                itens_cotacao_filha_negociacao_usuario_cotante itensDaCotacaoNegociacaoASerRespondidos = new itens_cotacao_filha_negociacao_usuario_cotante();
                usuario_cotante dadosUsuarioCotante = new usuario_cotante();

                try
                {
                    //Busca dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                    cotacao_master_usuario_cotante dadosCotacaoMaster = negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(idCotacaoMaster);

                    if (listaIdsProdutosSelecionados.Length > 0)
                    {
                        //Gravado os valores relacionados ao VALOR TOTAL do LOTE de mercadorias e DESCONTOS (TIPO e PERCENTUAL)
                        cotacaoFilhaASerRespondida.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                        cotacaoFilhaASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = DateTime.Now;
                        cotacaoFilhaASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE = formaDePagamento;
                        cotacaoFilhaASerRespondida.ID_TIPO_FRETE = Convert.ToInt32(tipoDeFrete);
                        cotacaoFilhaASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = valorTotalDoLoteSemDesconto;
                        cotacaoFilhaASerRespondida.TIPO_DESCONTO = Convert.ToInt32(tipoDeDesconto);
                        cotacaoFilhaASerRespondida.PERCENTUAL_DESCONTO = Convert.ToDecimal(percentualDeDesconto);
                        cotacaoFilhaASerRespondida.OBSERVACAO_COTACAO_USUARIO_COTANTE = observacao;
                        cotacaoFilhaASerRespondida.COTACAO_FILHA_USUARIO_COTANTE_EDITADA = true;

                        //Editar a COTAÇÃO FILHA
                        cotacao_filha_usuario_cotante respondendoCotacaoUsuarioCotante1 =
                            negociosCotacaoFilha.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioCotante(cotacaoFilhaASerRespondida);

                        //Gravar RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE nos ITENS da COTAÇÃO
                        for (int i = 0; i < listaIdsProdutosSelecionados.Length; i++)
                        {
                            //Gravando os PREÇOS EDITADOS dos itens da COTAÇÃO
                            itensDaCotacaoNegociacaoASerRespondidos.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE = Convert.ToInt32(listaIdsProdutosSelecionados[i]);
                            itensDaCotacaoNegociacaoASerRespondidos.PRECO_ITENS_COTACAO_USUARIO_COTANTE = Convert.ToDecimal(listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante[i], new CultureInfo("en-US"));

                            itens_cotacao_filha_negociacao_usuario_cotante respondendoCotacaoUsuarioCotante2 =
                                negociosItensCotacaoFilha.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioCotante(itensDaCotacaoNegociacaoASerRespondidos, idCotacaoFilha, 1);

                            if (respondendoCotacaoUsuarioCotante2 != null)
                            {
                                itemRespondido = "ok";
                            }
                        }

                        //Retorna dados do Centro de Custo
                        var resultado = new
                        {
                            itemRespondido = itemRespondido
                        };

                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoDaResposta == "ec")
            {
                //EDITAR a COTAÇÃO enviada pela EMPRESA COTANTE
                NCotacaoMasterUsuarioEmpresaService negociosCotacaoMasterUsuarioEmpresa = new NCotacaoMasterUsuarioEmpresaService();
                NCotacaoFilhaUsuarioEmpresaService negociosCotacaoFilha = new NCotacaoFilhaUsuarioEmpresaService();
                NItensCotacaoFilhaNegociacaoUsuarioEmpresaService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioEmpresaService();
                NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                cotacao_filha_usuario_empresa cotacaoFilhaASerRespondida = new cotacao_filha_usuario_empresa();
                itens_cotacao_filha_negociacao_usuario_empresa itensDaCotacaoNegociacaoASerRespondidos = new itens_cotacao_filha_negociacao_usuario_empresa();
                usuario_empresa dadosUsuarioCotante = new usuario_empresa();

                try
                {
                    //Busca dados da COTAÇÃO MASTER do USUÁRIO COTANTE
                    cotacao_master_usuario_empresa dadosCotacaoMaster = negociosCotacaoMasterUsuarioEmpresa.BuscarCotacaoMasterDoUsuarioEmpresa(idCotacaoMaster);

                    if (listaIdsProdutosSelecionados.Length > 0)
                    {
                        //Gravado os valores relacionados ao VALOR TOTAL do LOTE de mercadorias e DESCONTOS (TIPO e PERCENTUAL)
                        cotacaoFilhaASerRespondida.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                        cotacaoFilhaASerRespondida.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_EMPRESA = DateTime.Now;
                        cotacaoFilhaASerRespondida.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_EMPRESA = formaDePagamento;
                        cotacaoFilhaASerRespondida.ID_TIPO_FRETE = Convert.ToInt32(tipoDeFrete);
                        cotacaoFilhaASerRespondida.PRECO_LOTE_ITENS_COTACAO_USUARIO_EMPRESA = valorTotalDoLoteSemDesconto;
                        cotacaoFilhaASerRespondida.TIPO_DESCONTO = Convert.ToInt32(tipoDeDesconto);
                        cotacaoFilhaASerRespondida.PERCENTUAL_DESCONTO = Convert.ToDecimal(percentualDeDesconto);
                        cotacaoFilhaASerRespondida.OBSERVACAO_COTACAO_USUARIO_EMPRESA = observacao;
                        cotacaoFilhaASerRespondida.COTACAO_FILHA_USUARIO_EMPRESA_EDITADA = true;

                        //Editar a COTAÇÃO FILHA
                        cotacao_filha_usuario_empresa respondendoCotacaoUsuarioEmpresaCotante1 =
                            negociosCotacaoFilha.GravarDadosEmRespostaACotacaoFilhaEnviadaPeloUsuarioEmpresa(cotacaoFilhaASerRespondida);

                        //Gravar RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE nos ITENS da COTAÇÃO
                        for (int i = 0; i < listaIdsProdutosSelecionados.Length; i++)
                        {
                            //Gravando os PREÇOS EDITADOS dos itens da COTAÇÃO
                            itensDaCotacaoNegociacaoASerRespondidos.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA = Convert.ToInt32(listaIdsProdutosSelecionados[i]);
                            itensDaCotacaoNegociacaoASerRespondidos.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = Convert.ToDecimal(listaValoresUnitariosRespostaDaCotacaoAoUsuarioCotante[i], new CultureInfo("en-US"));

                            itens_cotacao_filha_negociacao_usuario_empresa respondendoCotacaoUsuarioEmpresaCotante2 =
                                negociosItensCotacaoFilha.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioEmpresa(itensDaCotacaoNegociacaoASerRespondidos, idCotacaoFilha, 1);

                            if (respondendoCotacaoUsuarioEmpresaCotante2 != null)
                            {
                                itemRespondido = "ok";
                            }
                        }

                        //Retorna dados do Centro de Custo
                        var resultado = new
                        {
                            itemRespondido = itemRespondido
                        };

                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoDaResposta == "cc")
            {
                try
                {
                    /*
                    IMPLEMENTAR FUTURAMENTE 
                    */
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }

            return null;
        }

        //Verifica se existe ou não NOVA MENSAGEM no CHAT, para setar o balão indicativo no front end
        [WebMethod]
        public string ChecarSeHaDialogoEntreCotanteEFornecedorParaNotificacao(int idCotacaoFilha, string tipoCotacao)
        {
            int valor = 0;
            string mensagem = "";

            if (tipoCotacao == "uc")
            {
                try
                {
                    //Carrega CHAT do USUÁRIO COTANTE e FORNECEDOR
                    NChatCotacaoUsuarioCotanteService negociosChatCotacaoUsuarioCotante = new NChatCotacaoUsuarioCotanteService();
                    List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                    //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                    List<chat_cotacao_usuario_cotante> listaConversasApuradasNoChat = negociosChatCotacaoUsuarioCotante.BuscarChatEntreUsuarioCOtanteEFornecedor(idCotacaoFilha);

                    valor = listaConversasApuradasNoChat.Count;

                    if (valor % 2 == 0)
                    {
                        //Par - NÃO TEM NOVA MENSAGEM
                        mensagem = "naoTem";
                    }
                    else
                    {
                        //Impar - TEM NOVA MENSAGEM
                        mensagem = "tem";
                    }

                    return mensagem;
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoCotacao == "ec")
            {
                try
                {
                    //Carrega CHAT do USUÁRIO EMPRESA COTANTE e FORNECEDOR
                    NChatCotacaoUsuarioEmpresaService negociosChatCotacaoUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                    List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreEmpresaCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                    //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                    List<chat_cotacao_usuario_empresa> listaConversasApuradasNoChat = negociosChatCotacaoUsuarioEmpresa.BuscarChatEntreUsuarioEmpresaEFornecedor(idCotacaoFilha);

                    valor = listaConversasApuradasNoChat.Count;

                    if (valor % 2 == 0)
                    {
                        //Par - NÃO TEM NOVA MENSAGEM
                        mensagem = "naoTem";
                    }
                    else
                    {
                        //Impar - TEM NOVA MENSAGEM
                        mensagem = "tem";
                    }

                    return mensagem;
                }
                catch (Exception erro)
                {
                    throw erro;
                }
            }
            else if (tipoCotacao == "ec")
            {
                try
                {
                    /*
                    IMPLEMENTAR FUTURAMENTE 
                    */
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return null;
        }

        //Armazena na tabela CHAT_COTACAO_USUARIO_COTANTE a conversa entre o COTANTE e o FORNECEDOR
        [WebMethod]
        public ActionResult EnviarRespostaParaAPerguntaDoCompradorNaCotacao(int idCotacaoFilha, int idEmpresaCotada, int idUsuarioCotante, string textoPerguntaOuResposta,
            string tipoCotacao)
        {
            string retornoGravacao = "nok";

            var resultado = new { gravacaoChat = "" };

            try
            {
                if (tipoCotacao == "uc")
                {
                    //USUÁRIO COTANTE
                    NChatCotacaoUsuarioCotanteService negociosChatUsuarioCotante = new NChatCotacaoUsuarioCotanteService();
                    chat_cotacao_usuario_cotante dadosChatCotacao = new chat_cotacao_usuario_cotante();

                    int numeroDeOrdemNaExibicao = (negociosChatUsuarioCotante.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha) + 1);

                    //Preparando os dados a serem gravados
                    dadosChatCotacao.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                    dadosChatCotacao.ID_CODIGO_USUARIO_COTANTE = idUsuarioCotante;
                    dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTADA = (int)Sessao.IdUsuarioLogado;
                    dadosChatCotacao.DATA_CHAT_COTACAO_USUARIO_COTANTE = DateTime.Now;
                    dadosChatCotacao.TEXTO_CHAT_COTACAO_USUARIO_COTANTE = textoPerguntaOuResposta;
                    dadosChatCotacao.ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_COTANTE = numeroDeOrdemNaExibicao;

                    //Gravar CONVERSA no CHAT
                    chat_cotacao_usuario_cotante gravarPerguntaOuRespostaDoChat = negociosChatUsuarioCotante.GravarConversaNoChat(dadosChatCotacao);

                    if (gravarPerguntaOuRespostaDoChat != null)
                    {
                        retornoGravacao = "ok";
                    }

                    //Resultado a ser retornado
                    resultado = new
                    {
                        gravacaoChat = retornoGravacao
                    };
                }
                else if (tipoCotacao == "ec")
                {
                    //USUÁRIO EMPRESA COTANTE
                    NChatCotacaoUsuarioEmpresaService negociosChatUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();
                    chat_cotacao_usuario_empresa dadosChatCotacao = new chat_cotacao_usuario_empresa();

                    int numeroDeOrdemNaExibicao = (negociosChatUsuarioEmpresa.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha) + 1);

                    //Preparando os dados a serem gravados
                    dadosChatCotacao.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA = idCotacaoFilha;
                    dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTANTE = idUsuarioCotante;
                    dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTADA = (int)Sessao.IdUsuarioLogado;
                    dadosChatCotacao.DATA_CHAT_COTACAO_USUARIO_EMPRESA = DateTime.Now;
                    dadosChatCotacao.TEXTO_CHAT_COTACAO_USUARIO_EMPRESA = textoPerguntaOuResposta;
                    dadosChatCotacao.ORDEM_EXIBICAO_CHAT_COTACAO_USUARIO_EMPRESA = numeroDeOrdemNaExibicao;

                    //Gravar CONVERSA no CHAT
                    chat_cotacao_usuario_empresa gravarPerguntaOuRespostaDoChat = negociosChatUsuarioEmpresa.GravarConversaNoChat(dadosChatCotacao, idEmpresaCotada, textoPerguntaOuResposta);

                    if (gravarPerguntaOuRespostaDoChat != null)
                    {
                        retornoGravacao = "ok";
                    }

                    //Resultado a ser retornado
                    resultado = new
                    {
                        gravacaoChat = retornoGravacao
                    };
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

    }
}
