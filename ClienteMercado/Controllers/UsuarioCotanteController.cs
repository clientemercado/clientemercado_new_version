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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Controllers
{
    public class UsuarioCotanteController : Controller
    {
        private object lista;

        //
        // GET: /Usuario/
        //[Authorize]
        public ActionResult PerfilUsuarioCotante(string nmU, string cloG, string cbaiR, string cciD, string cesT, string ccouT)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                PerfilUsuarioCotante dadosEPerfilUsuarioCotante = new PerfilUsuarioCotante();

                dadosEPerfilUsuarioCotante.ListagemCategoriaAtividadesACotarDirecionada = ListagemGruposAtividades();
                dadosEPerfilUsuarioCotante.ListagemCategoriaAtividadesACotarAvulsa = ListagemGruposAtividades();
                dadosEPerfilUsuarioCotante.ListagemUnidadesProdutosACotarDirecionada = ListagemUnidadesDePesoEMedida();
                dadosEPerfilUsuarioCotante.ListagemUnidadesProdutosACotarAvulsa = ListagemUnidadesDePesoEMedida();
                dadosEPerfilUsuarioCotante.ListagemOutrasCidadesACotarDirecionada = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioCotante.ListagemOutrosEstadosACotarDirecionada = ListagemEstados();
                dadosEPerfilUsuarioCotante.ListagemOutrasCidadesACotarAvulsa = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioCotante.ListagemOutrosEstadosACotarAvulsa = ListagemEstados();
                dadosEPerfilUsuarioCotante.ListagemOutrasCidadesOutroEstadoACotarDirecionada = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioCotante.ListagemOutrasCidadesOutroEstadoACotarAvulsa = ListagemCidadesPorEstado(Convert.ToInt32(MD5Crypt.Descriptografar(cesT)));
                dadosEPerfilUsuarioCotante.listagemDasCotacoesDirecionadasEnviadasPeloUsuario = ListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante();
                dadosEPerfilUsuarioCotante.listagemDasCotacoesAvulsasEnviadasPeloUsuario = ListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante();

                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));

                TempData["CotacoesDirecionadasEnviadasPeloUsuario"] = (dadosEPerfilUsuarioCotante.listagemDasCotacoesDirecionadasEnviadasPeloUsuario.Count);

                TempData["CotacoesAvulsasEnviadasPeloDoUsuario"] = (dadosEPerfilUsuarioCotante.listagemDasCotacoesAvulsasEnviadasPeloUsuario.Count);

                return View(dadosEPerfilUsuarioCotante);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Acionada quando o Usuário Cotante deseja ver as ocorrências (Ações) relacionadas à COTAÇÃO selecionada no Grid
        public ActionResult AcompanharCotacoesEnviadasUsuarioCotante(string nmU, string cloG, string cbaiR, string cciD, string cesT, string ccouT, string ccM)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                int idCotacaoMaster = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));
                string categoriaCotacao = "";
                string tipoDaCotacao = "";
                string statusDaCotacao = "ABERTA";
                string dataEncerramento = "__/__/____";

                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                cotacao_master_usuario_cotante dadosDaCotacaoMasterDoUsuarioCotante = new cotacao_master_usuario_cotante();

                AcompanharCotacaoEnviada dadosASeremExibidosNaView = new AcompanharCotacaoEnviada();

                //Consulta os dados da COTAÇÃO MASTER, enviada pelo Usuário COTANTE
                dadosDaCotacaoMasterDoUsuarioCotante = negociosCotacaoMasterUsuarioCotante.BuscarCotacaoMasterDoUsuarioCotante(idCotacaoMaster);

                if (dadosDaCotacaoMasterDoUsuarioCotante != null)
                {
                    //Buscar dados do GRUPO de ATIVIDADES selecionado para esta COTAÇÃO
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                    grupo_atividades_empresa dadosDaConsultaDoGrupoDeAtividades = new grupo_atividades_empresa();

                    dadosDaConsultaDoGrupoDeAtividades.ID_GRUPO_ATIVIDADES = dadosDaCotacaoMasterDoUsuarioCotante.ID_GRUPO_ATIVIDADES;

                    grupo_atividades_empresa dadosDoGrupoDeAtividadesPesquisado =
                        negociosGrupoDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(dadosDaConsultaDoGrupoDeAtividades);

                    if (dadosDoGrupoDeAtividadesPesquisado != null)
                    {
                        categoriaCotacao = dadosDoGrupoDeAtividadesPesquisado.ID_GRUPO_ATIVIDADES.ToString() + " - " + dadosDoGrupoDeAtividadesPesquisado.DESCRICAO_ATIVIDADE;
                    }

                    //Buscar dados do TIPO de COTAÇÃO
                    NTiposDeCotacaoService negociosTiposCotacao = new NTiposDeCotacaoService();
                    tipos_cotacao dadosDaConsultaDoTipoDeCotacao = new tipos_cotacao();

                    dadosDaConsultaDoTipoDeCotacao.ID_CODIGO_TIPO_COTACAO = dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_TIPO_COTACAO;

                    tipos_cotacao dadosDoTipoDaCotacao = negociosTiposCotacao.ConsultarDadosDoTipoDeCotacao(dadosDaConsultaDoTipoDeCotacao);

                    if (dadosDoTipoDaCotacao != null)
                    {
                        tipoDaCotacao = dadosDoTipoDaCotacao.DESCRICAO_TIPO_COTACAO;
                    }

                    //Montando os dados a serem exibidos na View
                    dadosASeremExibidosNaView.NOME_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE;
                    dadosASeremExibidosNaView.CATEGORIA_COTACAO_ENVIADA = categoriaCotacao;
                    dadosASeremExibidosNaView.TIPO_COTACAO_ENVIADA = tipoDaCotacao;
                    dadosASeremExibidosNaView.DATA_CRIACAO_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                    dadosASeremExibidosNaView.PERCENTUAL_RESPONDIDA_COTACAO_ENVIADA = dadosDaCotacaoMasterDoUsuarioCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE;

                    dadosASeremExibidosNaView.ListagemProdutosDaCotacaoEnviada = BuscarProdutosDaCotacao(idCotacaoMaster); //Buscar PRODUTOS ligados a esta COTAÇÃO
                    dadosASeremExibidosNaView.ListagemFornecedoresDaCotacaoEnviada = BuscarFornecedoresDaCotacao(idCotacaoMaster); //Buscar Fornecedores que receberam esta COTAÇÃO

                    //Buscar o STATUS da COTACAO (ABERTA, EM ANDAMENTO ou ENCERRADA)
                    if (dadosASeremExibidosNaView.ListagemFornecedoresDaCotacaoEnviada.Count > 0)
                    {
                        if (dadosDaCotacaoMasterDoUsuarioCotante.ID_CODIGO_STATUS_COTACAO != 3)
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
                            dataEncerramento = dadosDaCotacaoMasterDoUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString();
                        }
                    }

                    dadosASeremExibidosNaView.STATUS_COTACAO_ENVIADA = statusDaCotacao;
                    dadosASeremExibidosNaView.DATA_ENCERRAMENTO_COTACAO_ENVIADA = dataEncerramento;
                }

                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.idLogradouro = Convert.ToInt32(MD5Crypt.Descriptografar(cloG));
                ViewBag.idBairro = Convert.ToInt32(MD5Crypt.Descriptografar(cbaiR));
                ViewBag.idCidade = Convert.ToInt32(MD5Crypt.Descriptografar(cciD));
                ViewBag.idEstado = Convert.ToInt32(MD5Crypt.Descriptografar(cesT));
                ViewBag.idPais = Convert.ToInt32(MD5Crypt.Descriptografar(ccouT));
                ViewBag.idCotacao = Convert.ToInt32(MD5Crypt.Descriptografar(ccM));

                ViewBag.deOndeVim = "Início";
                ViewBag.ondeEstou = "Acompanhar Cotações Enviadas";

                return View(dadosASeremExibidosNaView);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Carrega os Grupos de Atividades para montagem da Cotação
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

        //Carregar COTAÇÕES DIRECIONADAS enviadas pelo Usuário Cotante
        private static List<CotacoesEnviadasPeloUsuario> ListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante()
        {
            int fornecedoresCotados = 0;
            int quantidadeCotacoesRespondidas = 0;

            try
            {
                //Buscar as COTAÇÕES DIRECIONADAS do Usuário
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                List<CotacoesEnviadasPeloUsuario> cotacoesDirecionadasEnviadasPeloUsuario = new List<CotacoesEnviadasPeloUsuario>();
                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();

                List<cotacao_master_usuario_cotante> listCotacoesDirecionadasDoUsuarioCotante =
                    negociosCotacaoMasterUsuarioCotante.CarregarListaDeCotacoesDirecionadasEnviadasPeloUsuarioCotante();

                //Monta a lista a ser exibida
                if (listCotacoesDirecionadasDoUsuarioCotante.Count > 0)
                {
                    //Monta a Lista das COTAÇÕES enviadas pelo Usuário Cotante
                    for (int i = 0; i < listCotacoesDirecionadasDoUsuarioCotante.Count; i++)
                    {
                        //Buscar dados do Grupo de Atividades
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesDirecionadasDoUsuarioCotante[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa descricaoGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Buscas dados do Status da Cotacao
                        //statusDaCotacao.ID_CODIGO_STATUS_COTACAO = listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_STATUS_COTACAO;
                        status_cotacao descricaoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_STATUS_COTACAO);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        //Buscar QUANTIDADE de COTAÇÕES que já foram respondidas
                        quantidadeCotacoesRespondidas = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueJaResponderamACotacao(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        cotacoesDirecionadasEnviadasPeloUsuario.Add(new CotacoesEnviadasPeloUsuario(listCotacoesDirecionadasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE,
                            listCotacoesDirecionadasDoUsuarioCotante[i].NOME_COTACAO_USUARIO_COTANTE, listCotacoesDirecionadasDoUsuarioCotante[i].DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString(),
                            listCotacoesDirecionadasDoUsuarioCotante[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString(), descricaoGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados,
                            quantidadeCotacoesRespondidas, descricaoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false));
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
        private static List<CotacoesEnviadasPeloUsuario> ListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante()
        {
            int fornecedoresCotados = 0;
            int quantidadeCotacoesRespondidas = 0;

            try
            {
                //Buscar as COTAÇÕES AVULSAS do Usuário
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                List<CotacoesEnviadasPeloUsuario> cotacoesAvulsasEnviadasPeloUsuario = new List<CotacoesEnviadasPeloUsuario>();
                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NStatusCotacaoService negociosStatusDaCotacao = new NStatusCotacaoService();
                grupo_atividades_empresa grupoDeAtividades = new grupo_atividades_empresa();
                status_cotacao statusDaCotacao = new status_cotacao();

                List<cotacao_master_usuario_cotante> listCotacoesAvulsasDoUsuarioCotante =
                    negociosCotacaoMasterUsuarioCotante.CarregarListaDeCotacoesAvulsasEnviadasPeloUsuarioCotante();

                //Monta a lista a ser exibida
                if (listCotacoesAvulsasDoUsuarioCotante.Count > 0)
                {
                    //Monta a Lista das COTAÇÕES enviadas pelo Usuário Cotante
                    for (int i = 0; i < listCotacoesAvulsasDoUsuarioCotante.Count; i++)
                    {
                        //Buscar dados do Grupo de Atividades
                        grupoDeAtividades.ID_GRUPO_ATIVIDADES = listCotacoesAvulsasDoUsuarioCotante[i].ID_GRUPO_ATIVIDADES;
                        grupo_atividades_empresa descricaoGrupoDeAtividades = negociosGruposDeAtividades.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupoDeAtividades);

                        //Buscas dados do Status da Cotacao
                        //statusDaCotacao.ID_CODIGO_STATUS_COTACAO = listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_STATUS_COTACAO;
                        status_cotacao descricaoStatusDaCotacao = negociosStatusDaCotacao.ConsultarDadosDoStatusDaCotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_STATUS_COTACAO);

                        //Buscar QUANTIDADE de FORNECEDORES PARTICIPANTES em responder a cotação
                        fornecedoresCotados = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueEstaoRespondendoACotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        //Buscar QUANTIDADE de COTAÇÕES que já foram respondidas
                        quantidadeCotacoesRespondidas = negociosCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeFornecedoresQueJaResponderamACotacao(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE);

                        cotacoesAvulsasEnviadasPeloUsuario.Add(new CotacoesEnviadasPeloUsuario(listCotacoesAvulsasDoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE,
                            listCotacoesAvulsasDoUsuarioCotante[i].NOME_COTACAO_USUARIO_COTANTE, listCotacoesAvulsasDoUsuarioCotante[i].DATA_CRIACAO_COTACAO_USUARIO_COTANTE.ToShortDateString(),
                            listCotacoesAvulsasDoUsuarioCotante[i].DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE.ToShortDateString(), descricaoGrupoDeAtividades.DESCRICAO_ATIVIDADE, fornecedoresCotados,
                            quantidadeCotacoesRespondidas, descricaoStatusDaCotacao.DESCRICAO_STATUS_COTACAO, false));
                    }
                }

                return cotacoesAvulsasEnviadasPeloUsuario;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega Lista de PRODUTOS da COTAÇÃO
        private List<ProdutosDaCotacao> BuscarProdutosDaCotacao(int idCotacaoMaster)
        {
            string quantidadeItens;
            string valorDoProduto = string.Format("{0:0,0.00}", 0.00);
            string produtoCotado = "nao";

            //Buscar os ITENS da COTAÇÃO
            NItensCotacaoUsuarioCotanteService negociosItensCotacaoUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
            List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

            List<itens_cotacao_usuario_cotante> itensDaCotacaoUsuarioCotante =
                negociosItensCotacaoUsuarioCotante.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoMaster);

            if (itensDaCotacaoUsuarioCotante.Count > 0)
            {
                for (int i = 0; i < itensDaCotacaoUsuarioCotante.Count; i++)
                {
                    //Buscar dados do PRODUTO
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    produtos_servicos_empresa_profissional dadosDoProduto =
                        negociosProdutos.ConsultarDadosDoProdutoDaCotacao(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                    //Buscar dados da MARCA do PRODUTO
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    empresas_fabricantes_marcas dadosFabrincanteOuMarca =
                        negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                    //Buscar dados da UNIDADE do PRODUTO
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();
                    unidades_produtos dadosDaUnidadeProduto =
                        negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_UNIDADE_PRODUTO);

                    quantidadeItens = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO); //Formata a quantidade para exibição

                    produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE, 0,
                        itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, itensDaCotacaoUsuarioCotante[i].ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE,
                        dadosDoProduto.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItens, "0",
                        dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, "", 0, "", 0, "", "", 0, 0, "", "", "", "", 0));
                }
            }

            return produtosDaCotacao;
        }

        //Carrega Lista de FORNECEDORES da COTAÇÃO
        private List<FornecedoresCotados> BuscarFornecedoresDaCotacao(int idCotacaoMaster)
        {
            int valor = 0;
            string mensagem = "";
            string respondidaSimOuNao = "NÃO";

            //Buscar os FORNECEDORES da COTAÇÃO
            NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
            List<FornecedoresCotados> fornecedoresRespondendoACotacao = new List<FornecedoresCotados>();

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
                    NChatCotacaoUsuarioCotanteService negociosChatCotacaoUsuarioCotante = new NChatCotacaoUsuarioCotanteService();
                    List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                    //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                    List<chat_cotacao_usuario_cotante> listaConversasApuradasNoChat =
                        negociosChatCotacaoUsuarioCotante.BuscarChatEntreUsuarioCOtanteEFornecedor(dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE);

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
                        dadosDoEstadoUFDaEmpresa.UF_EMPRESA_USUARIO, dadosDoUsuario.NICK_NAME_USUARIO, dadosDaCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE, mensagem,
                        respondidaSimOuNao));
                }
            }

            return fornecedoresRespondendoACotacao;
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

        //Registra a Confirmação do Cadastro do Usuário Cotante
        public ActionResult ConfirmarCadastroUsuario(string login, string senha)
        {
            try
            {
                //Login para Usuário Cotante
                NLoginService negocioLoginCotante = new NLoginService();
                usuario_cotante_logins novoLoginCotante = new usuario_cotante_logins();

                novoLoginCotante.LOGIN_USUARIO_COTANTE_LOGINS = login;
                novoLoginCotante.SENHA_USUARIO_COTANTE_LOGINS = senha;

                usuario_cotante_logins usuarioCotanteLogins =
                    negocioLoginCotante.ConsultarLoginUsuarioCotante(novoLoginCotante);

                //Se campo CADASTRO_CONFIRMADO não estiver setado como verdadeiro, então inicia-se o processo de 
                //confirmação de cadastro
                if (usuarioCotanteLogins.usuario_cotante.CADASTRO_CONFIRMADO != true)
                {
                    NUsuarioCotanteService negocioUsuarioCotante = new NUsuarioCotanteService();
                    usuario_cotante confirmaCadastroUsuarioCotante = new usuario_cotante();

                    confirmaCadastroUsuarioCotante.ID_CODIGO_USUARIO_COTANTE =
                        usuarioCotanteLogins.ID_CODIGO_USUARIO_COTANTE;

                    usuario_cotante usuarioCotante =
                        negocioUsuarioCotante.ConfirmarCadastroUsuarioCotante(confirmaCadastroUsuarioCotante);

                    if (usuarioCotante != null)
                    {
                        Sessao.IdUsuarioLogado = usuarioCotanteLogins.usuario_cotante.ID_CODIGO_USUARIO_COTANTE;
                    }
                }

                /*Esse redirecionamento ocorrerá de qualquer forma. Se o cadastro e existência do usuário forem confirmados no
                sistema, o método PerfilUsuarioCotante permitirá o acesso, caso contrário, será direcionado para a página de Login*/
                return RedirectToAction("PerfilUsuarioCotante", "UsuarioCotante",
                             new
                             {
                                 nmU = MD5Crypt.Criptografar(ManipulacaoStrings.pegarParteNomeUsuario(usuarioCotanteLogins.usuario_cotante.NICK_NAME_USUARIO_COTANTE))
                             });
            }
            catch (Exception erro)
            {
                throw erro;
            }
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
        [HttpPost]
        public ActionResult PerfilUsuarioCotanteCotacaoDirecionada(PerfilUsuarioCotante cotacaoDirecionada, FormCollection form)
        {
            //Definindo as variáveis pertinentes ao método em questão
            string[] listaIDsProdutosServicos, listaDescricaoDosProdutosOuServicosASeremCotados, listaIDsUnidadesDeMedida, listaIDsFornecedores, listaIDsUsuariosVendedores, listaQuantidadeCadaItem,
                listaIDsMarcasFabricantes, listaDescricaoMarcasFabricantes;

            ArrayList listaIDsItensCotacaoUsuarioCotante = new ArrayList();
            ArrayList listaEmailsVendedoresQueReceberaoACotacao = new ArrayList();

            listaIDsProdutosServicos = cotacaoDirecionada.LISTA_IDS_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoDosProdutosOuServicosASeremCotados = cotacaoDirecionada.DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA_ORIGINAL.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsUnidadesDeMedida = cotacaoDirecionada.LISTA_IDS_UNIDADES_MEDIDA_PRODUTOS_ASEREM_COTADOS_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsFornecedores = cotacaoDirecionada.LISTA_IDS_FORNECEDORES_ARECEBER_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsUsuariosVendedores = cotacaoDirecionada.LISTA_IDS_VENDEDORES_FORNECEDORES_ARECEBER_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaQuantidadeCadaItem = cotacaoDirecionada.LISTA_QUANTIDADES_CADA_ITEM_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsMarcasFabricantes = cotacaoDirecionada.LISTA_IDS_MARCAS_FABRICANTES_ITEM_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoMarcasFabricantes = cotacaoDirecionada.LISTA_DESCRICAO_MARCAS_FABRICANTES_ITEM_COTACAO_DIRECIONADA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

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
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                cotacao_master_usuario_cotante dadosDaCotacaoMasterUsuarioCotante = new cotacao_master_usuario_cotante();

                itens_cotacao_usuario_cotante gravandoItensDaCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();

                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_STATUS_COTACAO = 1; //Por default já inicia como ABERTA (1-ABERTA, 2-EM ANDAMENTO, 3-ENCERRADA)
                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_USUARIO_COTANTE = Convert.ToInt32(Sessao.IdUsuarioLogado);
                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_TIPO_COTACAO = 1; //Tipo DIRECIONADA (1-DIRECIONADA, 2-AVULSA, 3-CENTRAL DE COMPRAS)

                //Monta o Nome da Cotação Direcionada
                if ((cotacaoDirecionada.NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA != null) && (cotacaoDirecionada.NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA != ""))
                {
                    dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE = cotacaoDirecionada.NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA;
                }
                else
                {
                    //Busca quantidade de registros de cotação do Usuário Cotante, para montar a cotação
                    List<cotacao_master_usuario_cotante> registrosDeCotacaoUsuarioCotante = new List<cotacao_master_usuario_cotante>();

                    registrosDeCotacaoUsuarioCotante =
                        negociosCotacaoMasterUsuarioCotante.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(Convert.ToInt32(Sessao.IdUsuarioLogado));

                    if (registrosDeCotacaoUsuarioCotante.Count > 0)
                    {
                        dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE =
                            "0000" + (registrosDeCotacaoUsuarioCotante.Count + 1).ToString();
                    }
                    else
                    {
                        dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE = "00001";
                    }
                }

                dadosDaCotacaoMasterUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE = DateTime.Now;

                if (cotacaoDirecionada.DATA_ENCERRAMENTO_COTACAO_DIRECIONADA != null)
                {
                    //Armazena data informada pelo USUÁRIO
                    dadosDaCotacaoMasterUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE = Convert.ToDateTime(cotacaoDirecionada.DATA_ENCERRAMENTO_COTACAO_DIRECIONADA);
                }
                else
                {
                    //O USUÁRIO não informou uma data de ENCERRAMENTO, então o sistema sugere a data atual acrescida de mais 1 dia
                    var dataSugerida = DateTime.Now.AddDays(1);
                    dadosDaCotacaoMasterUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE = dataSugerida;
                }

                dadosDaCotacaoMasterUsuarioCotante.ID_TIPO_FRETE = 2; //Por default FOB (1-CIF (Fornecedor paga) , 2-FOB (Cliente paga))
                dadosDaCotacaoMasterUsuarioCotante.ID_GRUPO_ATIVIDADES = cotacaoDirecionada.ID_CATEGORIA_ATIVIDADES_ACOTAR_DIRECIONADA;
                dadosDaCotacaoMasterUsuarioCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE = 0; //Inicia por default com 0

                //Gerando a COTAÇÃO MASTER
                cotacao_master_usuario_cotante gerarCotacaoMaster =
                    negociosCotacaoMasterUsuarioCotante.GerarCotacaoMasterUsuarioCotante(dadosDaCotacaoMasterUsuarioCotante);

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
                            novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = cotacaoDirecionada.ID_CATEGORIA_ATIVIDADES_ACOTAR_DIRECIONADA;
                            novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoDosProdutosOuServicosASeremCotados[i];

                            if ((cotacaoDirecionada.TIPO_COTACAO_PRODUTO_DIRECIONADA == "P") && (cotacaoDirecionada.TIPO_COTACAO_SERVICO_DIRECIONADA == "S"))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((cotacaoDirecionada.TIPO_COTACAO_PRODUTO_DIRECIONADA == "P") && (cotacaoDirecionada.TIPO_COTACAO_SERVICO_DIRECIONADA == ""))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((cotacaoDirecionada.TIPO_COTACAO_PRODUTO_DIRECIONADA == "") && (cotacaoDirecionada.TIPO_COTACAO_SERVICO_DIRECIONADA == "S"))
                            {
                                //Define como SERVIÇO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "S";
                            }

                            //Grava o novo Produto / Serviço
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

                        //Salvando os itens da Cotação (ITENS_COTACAO_USUARIO_COTANTE)
                        NItensCotacaoUsuarioCotanteService negociosItensCotacaoUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                        itens_cotacao_usuario_cotante dadosItensCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();

                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE;
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = Convert.ToInt32(listaIDsProdutosServicos[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_UNIDADE_PRODUTO = Convert.ToInt32(listaIDsUnidadesDeMedida[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = Convert.ToInt32(listaIDsMarcasFabricantes[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_PEDIDO_USUARIO_COTANTE = 0; //Na geração da COTAÇÃO ainda não existe um PEDIDO. Coloquei o valor por default pq o campo é de preenchimento obrigatório
                        dadosItensCotacaoUsuarioCotante.QUANTIDADE_ITENS_COTACAO = Convert.ToDecimal(listaQuantidadeCadaItem[i], new CultureInfo("en-US"));

                        //Gravar Itens que estarão ligado à COTACAO_MASTER_USUARIO_COTANTE
                        gravandoItensDaCotacaoUsuarioCotante =
                            negociosItensCotacaoUsuarioCotante.GravarItensDaCotacaoMasterDoUsuarioCotante(dadosItensCotacaoUsuarioCotante);

                        if (gravandoItensDaCotacaoUsuarioCotante != null)
                        {
                            //Armazenar o ID dos itens da COTAÇÃO MASTER, para serem replicados na geração da COTAÇÃO FILHA
                            listaIDsItensCotacaoUsuarioCotante.Add(gravandoItensDaCotacaoUsuarioCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE.ToString());
                        }
                    }

                    //GERAR a COTAÇÃO FILHA e ENVIAR a COTAÇÃO para os FORNECEDORES
                    if ((gerarCotacaoMaster != null) && (gravandoItensDaCotacaoUsuarioCotante != null))
                    {
                        //Gerar a COTAÇÃO FILHA (COTACAO_FILHA_USUARIO_COTANTE), 1 registro para cada FORNECEDOR
                        NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                        cotacao_filha_usuario_cotante dadosCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();

                        int contadorCotacoesFilha = 0; //Conta quantidade de cotações filha geradas.

                        for (int a = 0; a < listaIDsFornecedores.Length; a++)
                        {
                            dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE;
                            dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA = Convert.ToInt32(listaIDsFornecedores[a]);
                            dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO = Convert.ToInt32(listaIDsUsuariosVendedores[a]);
                            dadosCotacaoFilhaUsuarioCotante.ID_TIPO_FRETE = 2;
                            dadosCotacaoFilhaUsuarioCotante.RESPONDIDA_COTACAO_FILHA_USUARIO_COTANTE = false;
                            dadosCotacaoFilhaUsuarioCotante.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE = Convert.ToDateTime("01/01/1900");
                            dadosCotacaoFilhaUsuarioCotante.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE = "À Vista";
                            dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO = 0;
                            dadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO = 0;
                            dadosCotacaoFilhaUsuarioCotante.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE = 0;
                            dadosCotacaoFilhaUsuarioCotante.OBSERVACAO_COTACAO_USUARIO_COTANTE = null;
                            dadosCotacaoFilhaUsuarioCotante.COTACAO_FILHA_USUARIO_COTANTE_EDITADA = false;

                            cotacao_filha_usuario_cotante gerarCotacaoFilha =
                                negociosCotacaoFilhaUsuarioCotante.GerarCotacaoFilhaUsuarioCotante(dadosCotacaoFilhaUsuarioCotante);

                            //Armazenar os Itens ligados ou pertencentes à COTAÇÃO FILHA (ITENS_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE)
                            if (gerarCotacaoFilha != null)
                            {
                                //Salvando os itens da COTAÇÃO FILHA (ITENS_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE)
                                NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilhaNegociacaoUsuarioCotante =
                                    new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                                itens_cotacao_filha_negociacao_usuario_cotante dadosItensCotacaoFilhaUsuarioCotante = new itens_cotacao_filha_negociacao_usuario_cotante();

                                for (int b = 0; b < listaIDsItensCotacaoUsuarioCotante.Count; b++)
                                {
                                    dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE;
                                    dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE = Convert.ToInt32(listaIDsItensCotacaoUsuarioCotante[b]);
                                    dadosItensCotacaoFilhaUsuarioCotante.ID_CODIGO_TIPO_RESPOSTA_COTACAO = 1;
                                    dadosItensCotacaoFilhaUsuarioCotante.ID_CLASSIFICACAO_TIPO_ITENS_COTACAO = 1;
                                    dadosItensCotacaoFilhaUsuarioCotante.DESCRICAO_PRODUTO_EDITADO_COTADA_USUARIO_COTANTE = listaDescricaoDosProdutosOuServicosASeremCotados[b];
                                    dadosItensCotacaoFilhaUsuarioCotante.PRECO_ITENS_COTACAO_USUARIO_COTANTE = 0;
                                    dadosItensCotacaoFilhaUsuarioCotante.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE = Convert.ToDecimal(listaQuantidadeCadaItem[b], new CultureInfo("en-US"));
                                    dadosItensCotacaoFilhaUsuarioCotante.PRODUTO_COTADO_USUARIO_COTANTE = false;
                                    dadosItensCotacaoFilhaUsuarioCotante.ITEM_COTACAO_FILHA_EDITADO = false;

                                    itens_cotacao_filha_negociacao_usuario_cotante gravandoItensDaCotacaoFilhaUsuarioCotante =
                                        negociosItensCotacaoFilhaNegociacaoUsuarioCotante.GravarItensDaCotacaoFilhaUsuarioCotante(dadosItensCotacaoFilhaUsuarioCotante);
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

                            /*
                            REDIRECIONAR PARA TELA COM MENSAGEM INFORMANDO QUE 'A COTAÇÃO FOI ENVIADA COM SUCESSO' --> VER SE O LOCAL É AQUI MESMO
                            */
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

        //Enviar Cotação Avulsa
        [HttpPost]
        public ActionResult PerfilUsuarioCotanteCotacaoAvulsa(PerfilUsuarioCotante cotacaoAvulsa, FormCollection form)
        {
            //Definindo as variáveis pertinentes ao método em questão
            string[] listaIDsProdutosServicos, listaDescricaoDosProdutosOuServicosASeremCotados, listaIDsUnidadesDeMedida, listaQuantidadeCadaItem, listaIDsMarcasFabricantes,
                listaDescricaoMarcasFabricantes;

            ArrayList listaIDsItensCotacaoUsuarioCotante = new ArrayList();
            ArrayList listaEmailsVendedoresQueReceberaoACotacao = new ArrayList();

            listaIDsProdutosServicos = cotacaoAvulsa.LISTA_IDS_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoDosProdutosOuServicosASeremCotados = cotacaoAvulsa.DESCRICAO_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA_ORIGINAL.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsUnidadesDeMedida = cotacaoAvulsa.LISTA_IDS_UNIDADES_MEDIDA_PRODUTOS_ASEREM_COTADOS_COTACAO_AVULSA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaQuantidadeCadaItem = cotacaoAvulsa.LISTA_QUANTIDADES_CADA_ITEM_COTACAO_AVULSA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaIDsMarcasFabricantes = cotacaoAvulsa.LISTA_IDS_MARCAS_FABRICANTES_ITEM_COTACAO_AVULSA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            listaDescricaoMarcasFabricantes = cotacaoAvulsa.LISTA_DESCRICAO_MARCAS_FABRICANTES_ITEM_COTACAO_AVULSA.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            try
            {
                //Gerar a COTAÇÂO MASTER (COTAÇAO_MASTER_USUARIO_COTANTE)
                NCotacaoMasterUsuarioCotanteService negociosCotacaoMasterUsuarioCotante = new NCotacaoMasterUsuarioCotanteService();
                cotacao_master_usuario_cotante dadosDaCotacaoMasterUsuarioCotante = new cotacao_master_usuario_cotante();

                itens_cotacao_usuario_cotante gravandoItensDaCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();

                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_STATUS_COTACAO = 1; //Por default já inicia como ABERTA (1-ABERTA, 2-EM ANDAMENTO, 3-ENCERRADA)
                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_USUARIO_COTANTE = Convert.ToInt32(Sessao.IdUsuarioLogado);
                dadosDaCotacaoMasterUsuarioCotante.ID_CODIGO_TIPO_COTACAO = 2; //Tipo AVULSA (1-DIRECIONADA, 2-AVULSA, 3-CENTRAL DE COMPRAS)

                //Monta o Nome da Cotação Direcionada
                if ((cotacaoAvulsa.NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA != null) && (cotacaoAvulsa.NOME_COTACAO_USUARIO_COTANTE_DIRECIONADA != ""))
                {
                    dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE = cotacaoAvulsa.NOME_COTACAO_USUARIO_COTANTE_AVULSA;
                }
                else
                {
                    //Busca quantidade de registros de cotação do Usuário Cotante, para montar a cotação
                    List<cotacao_master_usuario_cotante> registrosDeCotacaoUsuarioCotante = new List<cotacao_master_usuario_cotante>();

                    registrosDeCotacaoUsuarioCotante =
                        negociosCotacaoMasterUsuarioCotante.VerificarAQuantidadeDeCotacoesExistentesParaMontagemDoNomeDefaultDaCotacao(Convert.ToInt32(Sessao.IdUsuarioLogado));

                    if (registrosDeCotacaoUsuarioCotante.Count > 0)
                    {
                        dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE =
                            "0000" + (registrosDeCotacaoUsuarioCotante.Count + 1).ToString();
                    }
                    else
                    {
                        dadosDaCotacaoMasterUsuarioCotante.NOME_COTACAO_USUARIO_COTANTE = "00001";
                    }
                }

                dadosDaCotacaoMasterUsuarioCotante.DATA_CRIACAO_COTACAO_USUARIO_COTANTE = DateTime.Now;

                if (cotacaoAvulsa.DATA_ENCERRAMENTO_COTACAO_AVULSA != null)
                {
                    //Armazena data informada pelo USUÁRIO
                    dadosDaCotacaoMasterUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE = Convert.ToDateTime(cotacaoAvulsa.DATA_ENCERRAMENTO_COTACAO_AVULSA);
                }
                else
                {
                    //O USUÁRIO não informou uma data de ENCERRAMENTO, então o sistema sugere a data atual acrescida de mais 1 dia
                    var dataSugerida = DateTime.Now.AddDays(1);
                    dadosDaCotacaoMasterUsuarioCotante.DATA_ENCERRAMENTO_COTACAO_USUARIO_COTANTE = dataSugerida;
                }

                dadosDaCotacaoMasterUsuarioCotante.ID_TIPO_FRETE = 2; //Por default FOB (1-CIF (Fornecedor paga) , 2-FOB (Cliente paga))
                dadosDaCotacaoMasterUsuarioCotante.ID_GRUPO_ATIVIDADES = cotacaoAvulsa.ID_CATEGORIA_ATIVIDADES_ACOTAR_AVULSA;
                dadosDaCotacaoMasterUsuarioCotante.PERCENTUAL_RESPONDIDA_COTACAO_USUARIO_COTANTE = 0; //Inicia por default com 0

                //Gerando a COTAÇÃO MASTER
                cotacao_master_usuario_cotante gerarCotacaoMaster =
                    negociosCotacaoMasterUsuarioCotante.GerarCotacaoMasterUsuarioCotante(dadosDaCotacaoMasterUsuarioCotante);

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
                            novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = cotacaoAvulsa.ID_CATEGORIA_ATIVIDADES_ACOTAR_AVULSA;
                            novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoDosProdutosOuServicosASeremCotados[i];

                            if ((cotacaoAvulsa.TIPO_COTACAO_PRODUTO_AVULSA == "P") && (cotacaoAvulsa.TIPO_COTACAO_SERVICO_AVULSA == "S"))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((cotacaoAvulsa.TIPO_COTACAO_PRODUTO_AVULSA == "P") && (cotacaoAvulsa.TIPO_COTACAO_SERVICO_AVULSA == ""))
                            {
                                //Define como PRODUTO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "P";
                            }
                            else if ((cotacaoAvulsa.TIPO_COTACAO_PRODUTO_AVULSA == "") && (cotacaoAvulsa.TIPO_COTACAO_SERVICO_AVULSA == "S"))
                            {
                                //Define como SERVIÇO
                                novoProdutoServicoEmpresaProfissional.TIPO_PRODUTO_SERVICO = "S";
                            }

                            //Grava o novo Produto / Serviço
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

                        //Salvando os itens da Cotação (ITENS_COTACAO_USUARIO_COTANTE)
                        NItensCotacaoUsuarioCotanteService negociosItensCotacaoUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                        itens_cotacao_usuario_cotante dadosItensCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();

                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = gerarCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE;
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = Convert.ToInt32(listaIDsProdutosServicos[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_UNIDADE_PRODUTO = Convert.ToInt32(listaIDsUnidadesDeMedida[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = Convert.ToInt32(listaIDsMarcasFabricantes[i]);
                        dadosItensCotacaoUsuarioCotante.ID_CODIGO_PEDIDO_USUARIO_COTANTE = 0; //Na geração da COTAÇÃO ainda não existe um PEDIDO. Coloquei o valor por default pq o campo é de preenchimento obrigatório
                        dadosItensCotacaoUsuarioCotante.QUANTIDADE_ITENS_COTACAO = Convert.ToDecimal(listaQuantidadeCadaItem[i], new CultureInfo("en-US"));

                        //Gravar Itens que estarão ligado à COTACAO_MASTER_USUARIO_COTANTE
                        gravandoItensDaCotacaoUsuarioCotante =
                            negociosItensCotacaoUsuarioCotante.GravarItensDaCotacaoMasterDoUsuarioCotante(dadosItensCotacaoUsuarioCotante);
                    }

                    //Dispara avisos aos USUÁRIOS VENDEDORES de EMPRESAS COTADAS habilitadas para serem avisadas de COTAÇÃO AVULSA
                    if ((gerarCotacaoMaster != null) && (gravandoItensDaCotacaoUsuarioCotante != null))
                    {
                        //Atribuir / Converter valores dos parâmetros em valores utilizáveis
                        var categoriaASerCotada = Convert.ToInt32(cotacaoAvulsa.ID_CATEGORIA_ATIVIDADES_ACOTAR_AVULSA);
                        var tipoPorCidade = Convert.ToInt32(cotacaoAvulsa.TIPO_BUSCA_FORNECEDORES_POR_CIDADE_AVULSA);
                        var tipoPorUF = Convert.ToInt32(cotacaoAvulsa.TIPO_BUSCA_FORNECEDORES_POR_UF_PAIS_AVULSA);
                        var idOutraCidade = Convert.ToInt32(cotacaoAvulsa.NOME_OUTRA_CIDADE_ACOTAR_AVULSA);
                        var idOutroEstado = Convert.ToInt32(cotacaoAvulsa.NOME_OUTRO_ESTADO_ACOTAR_AVULSA);
                        var idOutraCidadeOutroEstado = Convert.ToInt32(cotacaoAvulsa.NOME_OUTRA_CIDADE_OUTRO_ESTADO_ACOTAR_AVULSA);
                        var idCidadeCotante = Convert.ToInt32(form["ID_CIDADE_USUARIO"]);
                        var idEstadoCotante = Convert.ToInt32(form["ID_ESTADO_USUARIO"]);
                        var idPaisCotante = Convert.ToInt32(form["ID_PAIS_USUARIO"]);

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
                            //Envio de e-mail informando ao usuário VENDEDOR que ele possui uma nova COTAÇÂO (Tipo 7)
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

            }
            catch (Exception erro)
            {

                throw erro;
            }

            return null;
        }

        //Armazena na tabela CHAT_COTACAO_USUARIO_COTANTE a conversa entre o COTANTE e o FORNECEDOR
        [WebMethod]
        public ActionResult EnviarComunicacaoAoFornecedorNaCotacao(int idCotacaoFilha, int idEmpresaCotada, string textoPerguntaOuResposta)
        {
            try
            {
                string retornoGravacao = "nok";

                NChatCotacaoUsuarioCotanteService negociosChatUsuarioCotante = new NChatCotacaoUsuarioCotanteService();
                chat_cotacao_usuario_cotante dadosChatCotacao = new chat_cotacao_usuario_cotante();

                int numeroDeOrdemNaExibicao = (negociosChatUsuarioCotante.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha) + 1);

                //Preparando os dados a serem gravados
                dadosChatCotacao.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                dadosChatCotacao.ID_CODIGO_USUARIO_COTANTE = (int)Sessao.IdUsuarioLogado;
                dadosChatCotacao.ID_CODIGO_USUARIO_EMPRESA_COTADA = idEmpresaCotada;
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
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoMasterUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    itens_cotacao_usuario_cotante dadosItemCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();

                    NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                    cotacao_filha_usuario_cotante dadosCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    //BUSCAR dados da COTAÇÃO FILHA
                    dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                    cotacao_filha_usuario_cotante dadosConsultadosCotacaoFilhaUsuarioCotante =
                        negociosCotacaoFilhaUsuarioCotante.ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(dadosCotacaoFilhaUsuarioCotante);

                    if (dadosConsultadosCotacaoFilhaUsuarioCotante != null)
                    {
                        pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA.ToString();
                        pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO.ToString();
                    }

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

                            //BUSCAR as IMAGENS armazenadas para PRODUTOS ALTERNATIVOS (Obs: Enviadas pela EMPRESA que respondeu a cotação)
                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;
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
                                if ((dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 0) || (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 1) || (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);
                                }
                                else if (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((dadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO * valorDoProdutoSemDesconto) / 100);
                                    valorDoProdutoComDesconto = (valorDoProdutoSemDesconto - valorDesconto);

                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                }
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE > 0)
                            {
                                produtoCotado = "sim";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE, idCotacaoFilha,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE,
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

                            //BUSCAR as IMAGENS armazenadas para PRODUTOS ALTERNATIVOS (Obs: Enviadas pela EMPRESA que respondeu a cotação)
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
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA,
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
                throw;
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

                NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                cotacao_filha_usuario_cotante dadosCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();
                NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                    new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                //BUSCAR dados da COTAÇÃO FILHA
                dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                cotacao_filha_usuario_cotante dadosConsultadosCotacaoFilhaUsuarioCotante =
                    negociosCotacaoFilhaUsuarioCotante.ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(dadosCotacaoFilhaUsuarioCotante);

                if (dadosConsultadosCotacaoFilhaUsuarioCotante != null)
                {
                    pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA.ToString();
                    pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO.ToString();
                }

                //CARREGAR FOTOS ANEXADAS se houver
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> imagensArmazenadas =
                    negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(idCotacaoFilhaNegociacao);

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

            return null;
        }

        //Carrega DIÁLOGOS entre COTANTE e FORNECEDOR
        public JsonResult CarregarDialogoEntreCotanteEFornecedor(int idCotacaoFilha)
        {
            try
            {
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

        //Carregar RESPOSTAS da COTAÇÃO para os PRODUTOS cotados pelo FORNECEDOR (Obs: Para efeito de ANÁLISE)
        public JsonResult CarregarItensCotadosPorFornecedorParaAnaliseEPedido(int idCotacaoFilha, string tipoCotacao)
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
                decimal somaValorDoProdutoSemDesconto = 0;
                decimal somaValorDoProdutoComDesconto = 0;
                string valorASerExibidoNaColunaDesconto = "0,00";
                string listaFotosProdutosAlternativos = "";
                int quantidadeFotosAnexadas = 0;
                string pastaCodigoEmpresa = "";
                string pastaCodigoUsuario = "";
                string tipoDeFrete = "";

                List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

                if (tipoCotacao == "uc")
                {
                    //Carrega RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoMasterUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    itens_cotacao_usuario_cotante dadosItemCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();

                    NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                    cotacao_filha_usuario_cotante dadosCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    //BUSCAR dados da COTAÇÃO FILHA
                    dadosCotacaoFilhaUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = idCotacaoFilha;
                    cotacao_filha_usuario_cotante dadosConsultadosCotacaoFilhaUsuarioCotante =
                        negociosCotacaoFilhaUsuarioCotante.ConsultarDadosDaCotacaoFilhaPeloUsuarioCotante(dadosCotacaoFilhaUsuarioCotante);

                    if (dadosConsultadosCotacaoFilhaUsuarioCotante != null)
                    {
                        pastaCodigoEmpresa = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_EMPRESA.ToString();
                        pastaCodigoUsuario = dadosConsultadosCotacaoFilhaUsuarioCotante.ID_CODIGO_USUARIO.ToString();
                    }

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

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE); //Formata a quantidade para exibição

                            quantidadeItensReal = itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE) > 0)
                            {
                                //Calcula o VALOR TOTAL dos PRODUTOS sem desconto aplicado (Obs: Pra o caso de ocorrer DESCONTO na COTAÇÃO respondida)
                                valorDoProdutoSemDesconto = (itensDaCotacaoUsuarioCotante[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE * itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE);
                                somaValorDoProdutoSemDesconto = (somaValorDoProdutoSemDesconto + valorDoProdutoSemDesconto);

                                valorDoProduto = string.Format("{0:0,0.00}", itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((dadosConsultadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 0) || (dadosConsultadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 1))
                                {
                                    //SEM DESCONTO APLICADO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);
                                }
                                else if (dadosConsultadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);

                                    var valorDesconto = ((dadosConsultadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO * valorDoProdutoSemDesconto) / 100);
                                    valorDoProdutoComDesconto = (valorDoProdutoSemDesconto - valorDesconto);

                                    valorASerExibidoNaColunaDesconto = string.Format("{0:0,0.00}", valorDoProdutoComDesconto);
                                    somaValorDoProdutoComDesconto = (somaValorDoProdutoComDesconto + valorDoProdutoComDesconto);
                                }
                                else
                                {
                                    //DESCONTO aplicado SOMENTE no LOTE
                                    somaValorDoProdutoComDesconto = 0;
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valorDoProdutoSemDesconto);

                                    if (somaValorDoProdutoComDesconto == 0)
                                    {
                                        var valorDesconto = ((dadosConsultadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO
                                            * dadosConsultadosCotacaoFilhaUsuarioCotante.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE) / 100);

                                        somaValorDoProdutoComDesconto = (dadosConsultadosCotacaoFilhaUsuarioCotante.PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE - valorDesconto);
                                    }
                                }
                            }

                            //Seta um marcador pra manter o checkBox do produto respondido CHECKED ou NÃO CHECKED
                            if (itensDaCotacaoUsuarioCotante[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE > 0)
                            {
                                produtoCotado = "sim";
                            }

                            //DEFININDO o TIPO de FRETE
                            if (dadosConsultadosCotacaoFilhaUsuarioCotante.ID_TIPO_FRETE == 1)
                            {
                                tipoDeFrete = "CIF - FORNECEDOR PAGA";
                            }
                            else if (dadosConsultadosCotacaoFilhaUsuarioCotante.ID_TIPO_FRETE == 2)
                            {
                                tipoDeFrete = "FOB - CLIENTE PAGA";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(itensDaCotacaoUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE, idCotacaoFilha,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, "", quantidadeItensFormatado,
                                quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado, valorTotalDoProduto,
                                dadosConsultadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO, valorASerExibidoNaColunaDesconto, quantidadeFotosAnexadas,
                                listaFotosProdutosAlternativos, "", somaValorDoProdutoSemDesconto, somaValorDoProdutoComDesconto,
                                dadosConsultadosCotacaoFilhaUsuarioCotante.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE.ToShortDateString(),
                                dadosConsultadosCotacaoFilhaUsuarioCotante.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE,
                                dadosConsultadosCotacaoFilhaUsuarioCotante.OBSERVACAO_COTACAO_USUARIO_COTANTE, tipoDeFrete, 0));

                            quantidadeFotosAnexadas = 0;
                            valorDoProduto = "0,00";
                            valorTotalDoProduto = "0,00";
                            listaFotosProdutosAlternativos = "";
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
                string tipoDeFrete = "";

                List<ProdutosDaCotacao> produtosDaCotacao = new List<ProdutosDaCotacao>();

                if (tipoCotacao == "uc")
                {
                    //Carrega RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE
                    //Buscar os ITENS da COTAÇÃO
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoMasterUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    itens_cotacao_usuario_cotante dadosItemCotacaoUsuarioCotante = new itens_cotacao_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilha = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                    NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                    NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                    NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    empresa_usuario dadosEmpresaUsuario = new empresa_usuario();
                    empresa_usuario dadosDaEmpresaCotada = new empresa_usuario();

                    NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                    cotacao_filha_usuario_cotante dadosCotacaoFilhaUsuarioCotante = new cotacao_filha_usuario_cotante();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService negociosFotosProdutosAlternativos =
                        new NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService();

                    //BUSCAR dados da COTAÇÃO FILHA
                    List<cotacao_filha_usuario_cotante> dadosConsultadosCotacaoFilhaUsuarioCotante =
                        negociosCotacaoFilhaUsuarioCotante.ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioCotante(idCotacaoMaster);

                    if (dadosConsultadosCotacaoFilhaUsuarioCotante.Count > 0)
                    {
                        for (int a = 0; a < dadosConsultadosCotacaoFilhaUsuarioCotante.Count; a++)
                        {
                            if (listaIdsCotacoesEnviadas == "")
                            {
                                listaIdsCotacoesEnviadas = dadosConsultadosCotacaoFilhaUsuarioCotante[a].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.ToString();
                            }
                            else
                            {
                                listaIdsCotacoesEnviadas =
                                    (listaIdsCotacoesEnviadas + "," + dadosConsultadosCotacaoFilhaUsuarioCotante[a].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.ToString());
                            }
                        }

                        //BUSCA e CALCULA o VALOR TOTAL por PRODUTO em cada COTAÇÃO RESPONDIDA
                        List<ListaPorItemCotadoJaCalculadoUsuarioCotanteViewModel> valoresCotadosPorProduto =
                              negociosItensCotacaoFilha.BuscarValorTotalPorProdutoDestaCotacao(listaIdsCotacoesEnviadas, idCodigoProduto);

                        //for (int i = 0; i < dadosConsultadosCotacaoFilhaUsuarioCotante.Count; i++)
                        for (int i = 0; i < valoresCotadosPorProduto.Count; i++)
                        {
                            if (valoresCotadosPorProduto.Count > 0)
                            {
                                pastaCodigoEmpresa = valoresCotadosPorProduto[i].ID_CODIGO_EMPRESA.ToString();
                                pastaCodigoUsuario = valoresCotadosPorProduto[i].ID_CODIGO_USUARIO.ToString();

                                //Consultar EMPRESA cotada
                                dadosEmpresaUsuario.ID_CODIGO_EMPRESA = valoresCotadosPorProduto[i].ID_CODIGO_EMPRESA;

                                dadosDaEmpresaCotada = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(dadosEmpresaUsuario);
                            }

                            dadosItemCotacaoUsuarioCotante.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE = valoresCotadosPorProduto[i].ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE;

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

                            //BUSCAR as IMAGENS armazenadas para PRODUTOS ALTERNATIVOS (Obs: Enviadas pela EMPRESA que respondeu a cotação)
                            List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> imagensArmazenadas =
                                negociosFotosProdutosAlternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(valoresCotadosPorProduto[i].ID_PRODUTO_COTADO);

                            if (imagensArmazenadas.Count > 0)
                            {
                                quantidadeFotosAnexadas = imagensArmazenadas.Count;
                            }

                            quantidadeItensFormatado = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE); //Formata a quantidade para exibição

                            quantidadeItensReal = valoresCotadosPorProduto[i].QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE; //Armazena o valor real, para cálculo

                            //Valor do PRODUTO para EDIÇÃO
                            if (Convert.ToDecimal(valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE) > 0)
                            {
                                valorDoProduto = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE);

                                //APLICAR o DESCONTO conforme o TIPO respondido na COTAÇÃO
                                if ((dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 0) || (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 1)
                                    || (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 3))
                                {
                                    //SEM DESCONTO APLICADO ou DESCONTO APLICADO SOMENTE nos PRODUTOS da COTAÇÃO
                                    valorTotalDoProduto = string.Format("{0:0,0.00}", valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO);
                                }
                                else if (dadosCotacaoFilhaUsuarioCotante.TIPO_DESCONTO == 2)
                                {
                                    //DESCONTO APLICADO SOMENTE NO LOTE
                                    var valorDesconto = ((dadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO * valoresCotadosPorProduto[i].PRECO_FINAL_CALCULADO_DO_PRODUTO) / 100);
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
                            if (valoresCotadosPorProduto[i].PRECO_ITENS_COTACAO_USUARIO_COTANTE > 0)
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

                            //DEFININDO o TIPO de FRETE
                            if (valoresCotadosPorProduto[i].ID_TIPO_FRETE == 1)
                            {
                                tipoDeFrete = "CIF - FORNECEDOR PAGA";
                            }
                            else if (valoresCotadosPorProduto[i].ID_TIPO_FRETE == 2)
                            {
                                tipoDeFrete = "FOB - CLIENTE PAGA";
                            }

                            produtosDaCotacao.Add(new ProdutosDaCotacao(valoresCotadosPorProduto[i].ID_PRODUTO_COTADO, valoresCotadosPorProduto[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                                dadosDoProdutoDaCotacaoMaster.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE, dadosDoProdutoDaCotacaoMaster.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE,
                                dadosDoProdutoEmSi.DESCRICAO_PRODUTO_SERVICO, dadosFabrincanteOuMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS, dadosDaEmpresaCotada.NOME_FANTASIA_EMPRESA,
                                quantidadeItensFormatado, quantidadeItensReal.ToString(), dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO, valorDoProduto, produtoCotado,
                                valorTotalDoProduto, dadosCotacaoFilhaUsuarioCotante.PERCENTUAL_DESCONTO, valorDoProdutoComDescontoParaExibicao, quantidadeFotosAnexadas,
                                listaFotosProdutosAlternativos, itemComMenorPreco, 0, 0, valoresCotadosPorProduto[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE.ToShortDateString(),
                                valoresCotadosPorProduto[i].FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE, valoresCotadosPorProduto[i].OBSERVACAO_COTACAO_USUARIO_COTANTE, tipoDeFrete, 0));

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

        //Carregar RESPOSTAS da COTAÇÃO, respondida pelos FORNECEDORES (Obs: Para efeito de ANÁLISE)
        [WebMethod]
        public ActionResult CarregarEmpresasCotadasPeloUsuarioCotante(int idCotacaoMaster, string tipoCotacao)
        {
            try
            {
                int quantidadeDeItensASeremCotados = 0;
                int quantidadeDeItensCotados = 0;
                decimal valorDoProdutoComDesconto = 0;
                decimal valorDoDesconto = 0;
                string cotadosParcialTotal = "";
                string tipoDeDesconto = "";
                string virouPedido = "";
                //int totalDeRegistros = 0;

                List<ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel> listaTotalPorCotacaoRespondida =
                    new List<ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel>();

                if (tipoCotacao == "uc")
                {
                    //Carrega RESPOSTA da COTAÇÃO ao USUÁRIO COTANTE
                    NItensCotacaoUsuarioCotanteService negociosItensCotacaoMasterUsuarioCotante = new NItensCotacaoUsuarioCotanteService();
                    NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosItensCotacaoFilhaUsuarioCotante = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                    NCotacaoFilhaUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NCotacaoFilhaUsuarioCotanteService();
                    NPedidoUsuarioCotanteService negociosPedidoUsuarioCotante = new NPedidoUsuarioCotanteService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    empresa_usuario dadosEmpresaUsuario = new empresa_usuario();
                    empresa_usuario dadosDaEmpresaCotada = new empresa_usuario();

                    //BUSCAR Qtde ITENS COTADOS
                    List<itens_cotacao_usuario_cotante> itensDaCotacaoMasterUsuarioCotante =
                        negociosItensCotacaoMasterUsuarioCotante.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoMaster);

                    if (itensDaCotacaoMasterUsuarioCotante.Count > 0)
                    {
                        quantidadeDeItensASeremCotados = itensDaCotacaoMasterUsuarioCotante.Count;
                    }

                    //BUSCAR dados da COTAÇÃO FILHA
                    List<cotacao_filha_usuario_cotante> dadosConsultadosCotacaoFilhaUsuarioCotante =
                        negociosCotacaoFilhaUsuarioCotante.ConsultarTodasAsCotacoesFilhasEnviadasParaUmaDeterminadaCotacaoMasterPeloUsuarioCotante(idCotacaoMaster);

                    for (int i = 0; i < dadosConsultadosCotacaoFilhaUsuarioCotante.Count; i++)
                    {
                        //BUSCAR DADOS da EMPRESA que RESPONDEU a COTAÇÃO
                        dadosEmpresaUsuario.ID_CODIGO_EMPRESA = dadosConsultadosCotacaoFilhaUsuarioCotante[i].ID_CODIGO_EMPRESA;
                        dadosDaEmpresaCotada = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(dadosEmpresaUsuario);

                        //VERIFICAR QUANTIDADE de ITENS COTADOS
                        quantidadeDeItensCotados =
                            negociosItensCotacaoFilhaUsuarioCotante.ConsultarQuantidadeDeItensRespondidosPeloUsuarioCotante(dadosConsultadosCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE);

                        if (quantidadeDeItensCotados == quantidadeDeItensASeremCotados)
                        {
                            cotadosParcialTotal = "Total";
                        }
                        else if (quantidadeDeItensCotados < quantidadeDeItensASeremCotados)
                        {
                            cotadosParcialTotal = "Parcial";
                        }
                        else if (quantidadeDeItensCotados == 0)
                        {
                            cotadosParcialTotal = "Não Cotou";
                        }

                        //DEFINE o TIPO de DESCONTO
                        if (dadosConsultadosCotacaoFilhaUsuarioCotante[i].TIPO_DESCONTO == 1)
                        {
                            tipoDeDesconto = "não houve";
                        }
                        else if (dadosConsultadosCotacaoFilhaUsuarioCotante[i].TIPO_DESCONTO == 2)
                        {
                            tipoDeDesconto = "p/ produto";
                        }
                        else
                        {
                            tipoDeDesconto = "no lote";
                        }

                        //APURAR TOTAL da COTAÇÃO menos o DESCONTO
                        if ((dadosConsultadosCotacaoFilhaUsuarioCotante[i].TIPO_DESCONTO == 2) || (dadosConsultadosCotacaoFilhaUsuarioCotante[i].TIPO_DESCONTO == 3))
                        {
                            valorDoDesconto = ((dadosConsultadosCotacaoFilhaUsuarioCotante[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE *
                                dadosConsultadosCotacaoFilhaUsuarioCotante[i].PERCENTUAL_DESCONTO) / 100);
                            valorDoProdutoComDesconto = (dadosConsultadosCotacaoFilhaUsuarioCotante[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE -
                                valorDoDesconto);
                        }
                        else
                        {
                            valorDoProdutoComDesconto = dadosConsultadosCotacaoFilhaUsuarioCotante[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE;
                        }

                        //VERIFICA se a COTAÇÃO em questão virou PEDIDO
                        virouPedido = negociosPedidoUsuarioCotante.VerificarSeExistePedidoParaEstaCotacao(idCotacaoMaster, dadosConsultadosCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE);

                        listaTotalPorCotacaoRespondida.Add(new ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel(dadosConsultadosCotacaoFilhaUsuarioCotante[i].ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE,
                            dadosDaEmpresaCotada.NOME_FANTASIA_EMPRESA, dadosConsultadosCotacaoFilhaUsuarioCotante[i].DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE.ToString(),
                            cotadosParcialTotal, dadosConsultadosCotacaoFilhaUsuarioCotante[i].PRECO_LOTE_ITENS_COTACAO_USUARIO_COTANTE,
                            dadosConsultadosCotacaoFilhaUsuarioCotante[i].PERCENTUAL_DESCONTO, tipoDeDesconto, valorDoDesconto, valorDoProdutoComDesconto, virouPedido, ""));
                    }
                }

                //ORGANIZA a LISTA de COTAÇÕES RESPONDIDAS considerando a de menor valor como sendo a primeira
                List<ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel> listaTotalPorCotacaoRespondidaSetadaMenorValor =
                    listaTotalPorCotacaoRespondida.OrderBy(m => m.VALOR_TOTAL_COTACAO_COM_DESCONTO).ThenBy(m => m.VALOR_TOTAL_COTACAO_SEM_DESCONTO).ToList();

                listaTotalPorCotacaoRespondidaSetadaMenorValor[0].menor_valor = "sim"; //SETA o primeiro item da lista

                return Json(listaTotalPorCotacaoRespondidaSetadaMenorValor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Acionada quando o Usuário Cotante deseja ver as ocorrências (Ações) relacionadas à COTAÇÃO selecionada no Grid
        public ActionResult FazerPedidoEmpresas(int tipoPedido, string idCotacaoMaster, string idCotacaoFilha, string idProduto, string valorPedido)
        {
            try
            {
                var resultado = new { pedidoFeito = "nao" };
                var idItemPedido = 0;

                valorPedido = valorPedido.Replace('.', ',');

                NPedidoUsuarioCotanteService negociosPedidoUsuarioCotante = new NPedidoUsuarioCotanteService();
                NItensPedidoUsuarioCotanteService negociosItensPedidoUsuarioCotante = new NItensPedidoUsuarioCotanteService();
                pedido_usuario_cotante dadosPedidoUsuarioCotante = new pedido_usuario_cotante();
                itens_pedido_usuario_cotante dadosItenPedidoUsuarioCotante = new itens_pedido_usuario_cotante();

                //GERAR o PEDIDO feito pelo USUÁRIO COTANTE (Independente do Pedido ser TOTAL ou PARCIAL)
                dadosPedidoUsuarioCotante.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE = Convert.ToInt32(idCotacaoMaster);
                dadosPedidoUsuarioCotante.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = Convert.ToInt32(idCotacaoFilha);
                dadosPedidoUsuarioCotante.VALOR_PEDIDO_USUARIO_COTANTE = Convert.ToDecimal(valorPedido);
                dadosPedidoUsuarioCotante.DATA_PEDIDO_USUARIO_COTANTE = DateTime.Now;
                dadosPedidoUsuarioCotante.DATA_ENTREGA_PEDIDO_USUARIO_COTANTE = Convert.ToDateTime("1900-01-01");
                dadosPedidoUsuarioCotante.CONFIRMADO_PEDIDO_USUARIO_COTANTE = false;

                int idPedidoGeradoUsuarioCotante = negociosPedidoUsuarioCotante.GravarPedidoUsuarioCotante(dadosPedidoUsuarioCotante);

                if (tipoPedido == 1)
                {
                    //PEDIDO de TODOS os PRODUTOS DA COTAÇÃO - INTEGRAL
                    if (idPedidoGeradoUsuarioCotante > 0)
                    {
                        NItensCotacaoFilhaNegociacaoUsuarioCotanteService negociosCotacaoFilhaUsuarioCotante = new NItensCotacaoFilhaNegociacaoUsuarioCotanteService();
                        List<itens_cotacao_filha_negociacao_usuario_cotante> dadosItensRespondidosNaCotacaoFilha = new List<itens_cotacao_filha_negociacao_usuario_cotante>();

                        dadosItensRespondidosNaCotacaoFilha = negociosCotacaoFilhaUsuarioCotante.ConsultarItensDaCotacaoDoUsuarioCotante(Convert.ToInt32(idCotacaoFilha));

                        if (dadosItensRespondidosNaCotacaoFilha.Count > 0)
                        {
                            //GRAVAR ITENS do PEDIDO
                            for (int i = 0; i < dadosItensRespondidosNaCotacaoFilha.Count; i++)
                            {
                                dadosItenPedidoUsuarioCotante.ID_CODIGO_PEDIDO_USUARIO_COTANTE = idPedidoGeradoUsuarioCotante;
                                dadosItenPedidoUsuarioCotante.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE =
                                    dadosItensRespondidosNaCotacaoFilha[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE;

                                idItemPedido = negociosItensPedidoUsuarioCotante.GravarItemDoPedido(dadosItenPedidoUsuarioCotante);
                            }

                            if (idItemPedido > 0)
                            {
                                resultado = new
                                {
                                    pedidoFeito = "sim"
                                };

                                //===========================================================================
                                /*
                                DISPARAR AQUI PARA O VENDEDOR:                        
                                    - E-MAILS, SMS, INFORMAÇÕES no SISTEMA sobre O PEDIDO
                                */
                                //===========================================================================
                            }
                        }
                    }
                }
                else if (tipoPedido == 2)
                {
                    //PEDIDO de PRODUTO AVULSO
                    if (idPedidoGeradoUsuarioCotante > 0)
                    {
                        //GRAVAR ITEM do PEDIDO
                        dadosItenPedidoUsuarioCotante.ID_CODIGO_PEDIDO_USUARIO_COTANTE = idPedidoGeradoUsuarioCotante;
                        dadosItenPedidoUsuarioCotante.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE = Convert.ToInt32(idProduto);

                        idItemPedido = negociosItensPedidoUsuarioCotante.GravarItemDoPedido(dadosItenPedidoUsuarioCotante);

                        if (idItemPedido > 0)
                        {
                            resultado = new
                            {
                                pedidoFeito = "sim"
                            };

                            //===========================================================================
                            /*
                            DISPARAR AQUI PARA O VENDEDOR:                        
                                - E-MAILS, SMS, INFORMAÇÕES no SISTEMA sobre O PEDIDO
                            */
                            //===========================================================================
                        }
                    }
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }
    }
}
