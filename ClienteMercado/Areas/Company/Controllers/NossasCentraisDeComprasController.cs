using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Sms;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class NossasCentraisDeComprasController : Controller
    {
        //
        // GET: /Company/CentralDeCompras/

        public ActionResult Index()
        {
            try
            {
                if (Sessao.IdEmpresaUsuario > 0)
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    int mesDoAno = dataHoje.Month;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();

                    NossasCentraisDeComprasViewModel viewModelCC = new NossasCentraisDeComprasViewModel();

                    empresa_usuario dadosEmpresa = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresa = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesEmpresa =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosEmpresa.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacao = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //POPULAR VIEW MODEL
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;

                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacao[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacao[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresa.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.DESCRICAO_ATIVIDADE;
                    viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nossas Centrais de Compras";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult CotacoesDaCentralDeCompras(int cCC)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0))
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    int mesDoAno = dataHoje.Month;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();

                    NossasCentraisDeComprasViewModel viewModelCC = new NossasCentraisDeComprasViewModel();

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(cCC);

                    empresa_usuario dadosEmpresaADM =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaADM = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    empresa_usuario dadosEmpresaLogada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresaLogada = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesCC =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacaoEmpresaADM = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaADM.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //POPULAR VIEW MODEL
                    viewModelCC.cCC = cCC;
                    viewModelCC.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
                    viewModelCC.inNomeUsuarioLogado = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelCC.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    //viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();
                    viewModelCC.inListaUnidadesProdutosACotar = ListagemUnidadesDePesoEMedida();
                    viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();

                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                    //BUSCAR LISTA de EMPRESAS da CENTRAL de COMPRAS
                    List<empresas_participantes_central_de_compras> listaDeEmpresasParticipantesDaCC = negociosEmpresasParticipantesCC.BuscarListaDeEmpresasParticipantesDaCC(cCC);

                    //VERIFICAR se a EMPRESA em questâo já ESTÁ ATIVA na CENTRAL de COMPRAS
                    bool empresaAtivaCC = negociosEmpresasParticipantesCC.VerificarSeEmpresaParticipanteAceitouConviteParticipacaoCC(cCC);

                    if (empresaAtivaCC)
                    {
                        viewModelCC.inConviteAceitoCC = "sim";
                    }
                    else
                    {
                        viewModelCC.inConviteAceitoCC = "nao";
                    }

                    //VIEWBAGS
                    ViewBag.quantasEmpresas = (listaDeEmpresasParticipantesDaCC.Count - 2);
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nossas Centrais de Compras > Cotações da Central";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult NovaCotacao(int cCC, string tP, int iCM)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0))
                {
                    string nomeCotacaoMaster = "";
                    int idCotacaoIndividual = 0;
                    string cotacaoAnexada = "nao";
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    int mesDoAno = dataHoje.Month;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterDaCC = new NCotacaoMasterCentralDeComprasService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresaCC = new NCotacaoIndividualEmpresaCentralComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                    //VERIFICAR PARTICIPAÇÃO da EMPRESA na CENTRAL de COMPRAS
                    empresas_participantes_central_de_compras dadosParticipacaoEmpresaCC = negociosEmpresasParticipantesCC.ConsultarSeEmpresaParticipaDaCentralDeCompras(cCC);

                    NossasCentraisDeComprasViewModel viewModelCC = new NossasCentraisDeComprasViewModel();
                    cotacao_master_central_compras dadosDaCotacaoMaster = new cotacao_master_central_compras();

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(cCC);

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    if (iCM > 0)
                    {
                        nomeCotacaoMaster = negociosCotacaoMasterDaCC.CarregarNomeDaCotacaoMaster(iCM);
                    }

                    //CARREGAR dados da COTAÇÃO INDIVIDUAL gerada (Se a empresa logada possuir...)
                    cotacao_individual_empresa_central_compras dadosDaCotacaoIndividualDaEmpresa =
                        negociosCotacaoIndividualEmpresaCC.CarregarDadosDaCotacao(iCM, dadosParticipacaoEmpresaCC.ID_EMPRESA_CENTRAL_COMPRAS);

                    if (dadosDaCotacaoIndividualDaEmpresa != null)
                    {
                        idCotacaoIndividual = dadosDaCotacaoIndividualDaEmpresa.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS;

                        if (dadosDaCotacaoIndividualDaEmpresa.COTACAO_INDIVIDUAL_ANEXADA)
                        {
                            cotacaoAnexada = "sim";
                        }
                        else
                        {
                            cotacaoAnexada = "nao";
                        }
                    }

                    empresa_usuario dadosEmpresaADM =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaADM = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    empresa_usuario dadosEmpresaLogada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresaLogada = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesCC =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacaoEmpresaADM = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaADM.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    dadosDaCotacaoMaster = negociosCotacaoMasterDaCC.ConsultarDadosDaCotacaoMasterCC(iCM);

                    //POPULAR VIEW MODEL
                    viewModelCC.cCC = cCC;
                    viewModelCC.iCM = iCM;
                    viewModelCC.iCI = idCotacaoIndividual;
                    viewModelCC.iPCC = dadosParticipacaoEmpresaCC.ID_EMPRESA_CENTRAL_COMPRAS;
                    viewModelCC.inCotacaoAnexada = cotacaoAnexada;
                    viewModelCC.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelCC.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    //viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();
                    viewModelCC.inListaUnidadesProdutosACotar = ListagemUnidadesDePesoEMedida();
                    viewModelCC.nomeCotacaoMaster = nomeCotacaoMaster;
                    viewModelCC.inDataSugeridaEncerramentoCotacao = DateTime.Now.AddDays(5).ToShortDateString();
                    viewModelCC.inDataMinimaEncerramentoCotacao = DateTime.Now.AddDays(3).ToShortDateString();
                    viewModelCC.inDataMaximaEncerramentoCotacao = DateTime.Now.AddDays(6).ToShortDateString();
                    viewModelCC.inTipo = tP;

                    if (dadosDaCotacaoMaster != null)
                    {
                        if (dadosDaCotacaoMaster.COTACAO_ENVIADA_FORNECEDORES)
                        {
                            viewModelCC.inCotacaoMasterEnviada = "sim";
                        }
                        else
                        {
                            viewModelCC.inCotacaoMasterEnviada = "nao";
                        }
                    }

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nossas Centrais de Compras > Cotações da Central > Nova Cotação";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult VisualizarCotacao(int cCC, string tP, int iCM)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0))
                {
                    string nomeCotacaoMaster = "";
                    int idCotacaoIndividual = 0;
                    string cotacaoAnexada = "nao";
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    int mesDoAno = dataHoje.Month;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterDaCC = new NCotacaoMasterCentralDeComprasService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresaCC = new NCotacaoIndividualEmpresaCentralComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                    //VERIFICAR PARTICIPAÇÃO da EMPRESA na CENTRAL de COMPRAS
                    empresas_participantes_central_de_compras dadosParticipacaoEmpresaCC = negociosEmpresasParticipantesCC.ConsultarSeEmpresaParticipaDaCentralDeCompras(cCC);

                    NossasCentraisDeComprasViewModel viewModelCC = new NossasCentraisDeComprasViewModel();
                    cotacao_master_central_compras dadosDaCotacaoMaster = new cotacao_master_central_compras();

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(cCC);

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    if (iCM > 0)
                    {
                        nomeCotacaoMaster = negociosCotacaoMasterDaCC.CarregarNomeDaCotacaoMaster(iCM);
                    }

                    //CARREGAR dados da COTAÇÃO INDIVIDUAL gerada (Se a empresa logada possuir...)
                    cotacao_individual_empresa_central_compras dadosDaCotacaoIndividualDaEmpresa =
                        negociosCotacaoIndividualEmpresaCC.CarregarDadosDaCotacao(iCM, dadosParticipacaoEmpresaCC.ID_EMPRESA_CENTRAL_COMPRAS);

                    if (dadosDaCotacaoIndividualDaEmpresa != null)
                    {
                        idCotacaoIndividual = dadosDaCotacaoIndividualDaEmpresa.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS;

                        if (dadosDaCotacaoIndividualDaEmpresa.COTACAO_INDIVIDUAL_ANEXADA)
                        {
                            cotacaoAnexada = "sim";
                        }
                    }

                    empresa_usuario dadosEmpresaADM =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaADM = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    empresa_usuario dadosEmpresaLogada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresaLogada = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesCC =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacaoEmpresaADM = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaADM.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    dadosDaCotacaoMaster = negociosCotacaoMasterDaCC.ConsultarDadosDaCotacaoMasterCC(iCM);

                    //POPULAR VIEW MODEL
                    viewModelCC.cCC = cCC;
                    viewModelCC.iCM = iCM;
                    viewModelCC.iCI = idCotacaoIndividual;
                    viewModelCC.iPCC = dadosParticipacaoEmpresaCC.ID_EMPRESA_CENTRAL_COMPRAS;
                    viewModelCC.inCotacaoAnexada = cotacaoAnexada;
                    viewModelCC.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelCC.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    //viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();
                    viewModelCC.inListaUnidadesProdutosACotar = ListagemUnidadesDePesoEMedida();
                    viewModelCC.nomeCotacaoMaster = nomeCotacaoMaster;
                    viewModelCC.inDataSugeridaEncerramentoCotacao = DateTime.Now.AddDays(5).ToShortDateString();
                    viewModelCC.inDataMinimaEncerramentoCotacao = DateTime.Now.AddDays(3).ToShortDateString();
                    viewModelCC.inDataMaximaEncerramentoCotacao = DateTime.Now.AddDays(6).ToShortDateString();
                    viewModelCC.inTipo = tP;

                    if (dadosDaCotacaoMaster.COTACAO_ENVIADA_FORNECEDORES)
                    {
                        viewModelCC.inCotacaoMasterEnviada = "sim";
                    }
                    else
                    {
                        viewModelCC.inCotacaoMasterEnviada = "nao";
                    }

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nossas Centrais de Compras > Cotações da Central > Visualizar Cotação";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult AnalisarEnviarCotacao(int iCM, int cCC, string lib)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (iCM > 0))
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    int mesDoAno = dataHoje.Month;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterDaCC = new NCotacaoMasterCentralDeComprasService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesDaCC = new NEmpresasParticipantesCentralDeComprasService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualDasEmpresasParticipantesDaCC = new NCotacaoIndividualEmpresaCentralComprasService();

                    NossasCentraisDeComprasViewModel viewModelCC = new NossasCentraisDeComprasViewModel();
                    cotacao_master_central_compras dadosDaCotacaoMaster = new cotacao_master_central_compras();

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(cCC);

                    empresa_usuario dadosEmpresaADM =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaADM = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    empresa_usuario dadosEmpresaLogada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresaLogada = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesCC =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacaoEmpresaADM = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaADM.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    List<empresas_participantes_central_de_compras> dadosParticipantesDaCC = negociosEmpresasParticipantesDaCC.BuscarListaDeEmpresasParticipantesDaCC(cCC);
                    List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisDasEmpresasDaCC =
                        negociosCotacaoIndividualDasEmpresasParticipantesDaCC.BuscarListaDeCotacoesIndividuaisAnexadas(iCM);

                    empresas_participantes_central_de_compras dadosDaEmpresaLogadaNaCC =
                        dadosParticipantesDaCC.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == cCC) && (m.ID_CODIGO_EMPRESA == Sessao.IdEmpresaUsuario)));

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    dadosDaCotacaoMaster = negociosCotacaoMasterDaCC.ConsultarDadosDaCotacaoMasterCC(iCM);

                    TimeSpan date = (Convert.ToDateTime(dadosDaCotacaoMaster.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS) - DateTime.Now);

                    viewModelCC.cCC = cCC;
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelCC.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelCC.inCodEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
                    viewModelCC.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelCC.nomeCotacaoMaster = dadosDaCotacaoMaster.NOME_COTACAO_CENTRAL_COMPRAS;
                    viewModelCC.iCM = iCM;
                    viewModelCC.telefoneContato = dadosUsuarioEmpresaADM.TELEFONE1_USUARIO_EMPRESA + " / " + dadosEmpresaADM.TELEFONE1_EMPRESA_USUARIO;
                    viewModelCC.inIDUFEmpresaUsuario = dadosLocalizacaoEmpresaADM[0].ID_ESTADOS_EMPRESA_USUARIO;
                    viewModelCC.inListaDeRamosDeAtividadeParaComprasDaCC = ListagemDeDeRamosComercioAtacadista();
                    viewModelCC.inListaDeUFs = ListagemEstados();
                    viewModelCC.quantidadeEmpresasParticipantesDaCC = dadosParticipantesDaCC.Count;
                    viewModelCC.quantidadeEmpresasJahAnexaramCotacao = cotacoesIndividuaisDasEmpresasDaCC.Count;
                    viewModelCC.quantosFaltamAnexar = (dadosParticipantesDaCC.Count - cotacoesIndividuaisDasEmpresasDaCC.Count);
                    viewModelCC.inQuantosFornecedores = ListagemQuantiaDeFornecedores();
                    viewModelCC.ineACriptografado = MD5Crypt.Criptografar(dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS.ToString());
                    viewModelCC.quantosDiasFaltam = date.Days;
                    viewModelCC.inCotacaoAnexada =
                        negociosCotacaoIndividualDasEmpresasParticipantesDaCC.VerificarSeEmpresaPossuiCotacaoIndividualAnexada(iCM, dadosDaEmpresaLogadaNaCC.ID_EMPRESA_CENTRAL_COMPRAS);

                    var percentualAnexadoIdeal = ((dadosParticipantesDaCC.Count * 80) / 100);

                    if (cotacoesIndividuaisDasEmpresasDaCC.Count < percentualAnexadoIdeal)
                    {
                        viewModelCC.corStatusDaQuantidadeAnexada = "#FF0000";
                        viewModelCC.statusAlerta = "sim";
                    }
                    else
                    {
                        viewModelCC.corStatusDaQuantidadeAnexada = "#32CD32";
                        viewModelCC.statusAlerta = "nao";
                    }

                    if (dadosDaCotacaoMaster.COTACAO_ENVIADA_FORNECEDORES)
                    {
                        viewModelCC.inCotacaoMasterEnviada = "sim";
                    }
                    else
                    {
                        viewModelCC.inCotacaoMasterEnviada = "nao";
                    }

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nossas Centrais de Compras > Cotações da Central > Análise/Envio Cotação";

                    return View(viewModelCC);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //POPULAR LISTA de GRUPOS de ATIVIDADES
        private static List<SelectListItem> ListaDeGruposDeAtividades()
        {
            try
            {
                //Buscar GRUPOS de ATIVIDADES
                NGruposAtividadesEmpresaProfissionalService negociosGruposAtividades = new NGruposAtividadesEmpresaProfissionalService();
                List<grupo_atividades_empresa> listaGruposDeAtividades = negociosGruposAtividades.ListaGruposAtividadesEmpresaProfissional();

                List<SelectListItem> listRamosAtividades = new List<SelectListItem>();

                listRamosAtividades.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

                foreach (var grupoAtividades in listaGruposDeAtividades)
                {
                    listRamosAtividades.Add(new SelectListItem
                    {
                        Text = grupoAtividades.DESCRICAO_ATIVIDADE,
                        Value = grupoAtividades.ID_GRUPO_ATIVIDADES.ToString()
                    });
                }

                return listRamosAtividades;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //BUSCAR POSSÍVEIS PARCEIROS para compor a CENTRAL de COMPRAS
        public ActionResult BuscarListaDeNossasCentraisDeCompras(string descricaoFiltro, int idGrupoAtividadesFiltro)
        {
            try
            {
                var textoStatus = "";

                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                NCentralDeComprasService negociosCentraisDeCompras = new NCentralDeComprasService();
                NGruposAtividadesEmpresaProfissionalService negociosGruposAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();

                //CARREGA LISTA de CENTRAIS de COMPRAS das quais a EMPRESA LOGADA participa
                List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel> listaCentraisDeComprasEmQueParticipo =
                    negociosEmpresasParticipantesCC.BuscarListaDeCentraisDeComprasQueParticipo();

                //BUSCAR MAIS INFORMAÇÕES para popular alguns campos
                for (int i = 0; i < listaCentraisDeComprasEmQueParticipo.Count; i++)
                {
                    //DESCRIÇÃO da ATIVIDADE
                    central_de_compras dadosCC =
                         negociosCentraisDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(listaCentraisDeComprasEmQueParticipo[i].ID_CENTRAL_COMPRAS);
                    grupo_atividades_empresa dadosGA =
                         negociosGruposAtividades.ConsultarDadosGeraisSobreOGrupoDeAtividades(dadosCC.ID_GRUPO_ATIVIDADES);

                    listaCentraisDeComprasEmQueParticipo[i].nomeCentralCompras = dadosCC.NOME_CENTRAL_COMPRAS;
                    listaCentraisDeComprasEmQueParticipo[i].idGrupoAtividades = dadosCC.ID_GRUPO_ATIVIDADES;
                    listaCentraisDeComprasEmQueParticipo[i].ramoAtividadeCentralCompras = dadosGA.DESCRICAO_ATIVIDADE;

                    //QUANTIDADE de EMPRESAS
                    int quantidadeDeEmpresasCC =
                         negociosEmpresasParticipantesCC.BuscarQuantidadeDeEmpresasParticipantes(listaCentraisDeComprasEmQueParticipo[i].ID_CENTRAL_COMPRAS);

                    listaCentraisDeComprasEmQueParticipo[i].quantasEmpresas = quantidadeDeEmpresasCC;

                    //BUSCAR CIDADE da CENTRAL de COMPRAS
                    var cidadeCC = negociosEmpresa.ConsultarCidadeDaEmpresa(dadosCC.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS).Split('-');

                    listaCentraisDeComprasEmQueParticipo[i].cidadeDaCentralCompras = cidadeCC[0];
                    listaCentraisDeComprasEmQueParticipo[i].ufDaCentralCompras = cidadeCC[1];

                    //VERIFICAR STATUS da CENTRAL de COMPRAS
                    if (DateTime.Now <= dadosCC.DATA_ENCERRAMENTO_CENTRAL_COMPRAS)
                    {
                        textoStatus = "ATIVA";
                    }

                    if (listaCentraisDeComprasEmQueParticipo[i].CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS == false)
                    {
                        textoStatus = "CONVITE";
                    }

                    /*
                    OBS: Definir aqui outras condições para o STATUS.
                    */

                    listaCentraisDeComprasEmQueParticipo[i].statusCentralCompras = textoStatus;
                }

                //------------------------------------------------------------------
                //APLICAR FILTRO
                List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel> listaCentraisDeComprasEmQueParticipoRetorno =
                    new List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel>();

                if ((String.IsNullOrEmpty(descricaoFiltro) == false) || (idGrupoAtividadesFiltro > 0))
                {
                    if (String.IsNullOrEmpty(descricaoFiltro) == false)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasEmQueParticipo.Where(m => (m.nomeCentralCompras.Contains(descricaoFiltro))).ToList();
                    }

                    if (idGrupoAtividadesFiltro > 0)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasEmQueParticipo.Where(m => (m.idGrupoAtividades == idGrupoAtividadesFiltro)).ToList();
                    }
                }
                else
                {
                    listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasEmQueParticipo;
                }
                //------------------------------------------------------------------

                return Json(
                    new
                    {
                        rows = listaCentraisDeComprasEmQueParticipoRetorno,
                        current = 1,
                        rowCount = listaCentraisDeComprasEmQueParticipoRetorno.Count,
                        total = listaCentraisDeComprasEmQueParticipoRetorno.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CONSULTA do AUTOCOMPLETE da LISTA de CENTRAIS de COMPRAS
        [WebMethod]
        public JsonResult BuscarListaDescricaoCC(string term)
        {
            NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();

            //CARREGA LISTA de CENTRAIS de COMPRAS
            List<ListaCentraisComprasViewModel> listaCC = negociosCentraisCompras.CarregarListaAutoCompleteCentraisDeCompras(term);

            return Json(listaCC, JsonRequestBehavior.AllowGet);
        }

        //CONSULTA do AUTOCOMPLETE da LISTA de COTAÇÕES da CENTRAL de COMPRAS
        [WebMethod]
        public JsonResult BuscarListaDeNomesCotacaoesDaCC(string term)
        {
            //NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();
            NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();

            //CARREGA LISTA de COTAÇÕES da CENTRAL de COMPRAS
            List<cotacao_master_central_compras> listaCotacaoMasterCC = negociosCotacaoMaster.CarregarListaAutoCompleteDasCotacoesDaCC(term);

            return Json(listaCotacaoMasterCC, JsonRequestBehavior.AllowGet);
        }

        //CONSULTA da LISTA de COTAÇÕES MASTER da CENTRAIS de COMPRAS
        [WebMethod]
        public JsonResult BuscarListaDeCotacoesDaCentraisDeCompras(int cCC, string descricaoFiltro)
        {
            try
            {
                var textoStatus = "";
                bool cotacaoEnviada = false;

                NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresaCC = new NCotacaoIndividualEmpresaCentralComprasService();
                NCentralDeComprasService negociosCentraisDeCompras = new NCentralDeComprasService();
                NGruposAtividadesEmpresaProfissionalService negociosGruposAtividades = new NGruposAtividadesEmpresaProfissionalService();
                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                //CARREGA LISTA de COTAÇÕES MASTER da CENTRAL de COMPRAS selecionada (Obs: Funciona tbm como FILTRO)
                List<ListaDeCotacaoesDaCentralDeComprasViewModel> listaCotacaoesDaCentralCompras = negociosCotacaoMaster.ListaDeCotacoesDaCentralDeCompras(cCC);

                //BUSCAR MAIS INFORMAÇÕES para popular alguns campos
                for (int i = 0; i < listaCotacaoesDaCentralCompras.Count; i++)
                {
                    //DESCRIÇÃO da ATIVIDADE
                    central_de_compras dadosCC =
                        negociosCentraisDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(listaCotacaoesDaCentralCompras[i].ID_CENTRAL_COMPRAS);
                    grupo_atividades_empresa dadosGA =
                        negociosGruposAtividades.ConsultarDadosGeraisSobreOGrupoDeAtividades(dadosCC.ID_GRUPO_ATIVIDADES);

                    listaCotacaoesDaCentralCompras[i].nomeDaCotacao = listaCotacaoesDaCentralCompras[i].NOME_COTACAO_CENTRAL_COMPRAS;
                    listaCotacaoesDaCentralCompras[i].dataCriacaoDaCotacao = listaCotacaoesDaCentralCompras[i].DATA_CRIACAO_COTACAO_CENTRAL_COMPRAS.ToShortDateString();
                    listaCotacaoesDaCentralCompras[i].dataEncerramentoCotacao = listaCotacaoesDaCentralCompras[i].DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS.ToShortDateString();
                    listaCotacaoesDaCentralCompras[i].grupoAtividades = dadosGA.DESCRICAO_ATIVIDADE;

                    //QUANTIDotacaoADE de EMPRESAS respondendo a COTAÇÃO
                    int quantidadeDeEmpresasRespondendoACotacao =
                        negociosCotacaoFilhaCC.BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                    listaCotacaoesDaCentralCompras[i].quantasEmpresas = quantidadeDeEmpresasRespondendoACotacao;

                    empresas_participantes_central_de_compras dadosEmpresaPartcipanteCC =
                        negociosEmpresasParticipantesCC.ConsultarSeEmpresaParticipaDaCentralDeCompras(cCC);

                    //VERIFICANDO se TEM / NÃO TEM COTAÇÃO ANEXADA
                    listaCotacaoesDaCentralCompras[i].temCotacaoAnexada =
                        negociosCotacaoIndividualEmpresaCC.VerificarSeEmpresaPossuiCotacaoIndividualAnexada(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS, dadosEmpresaPartcipanteCC.ID_EMPRESA_CENTRAL_COMPRAS);

                    var teste = "";

                    //VERIFICAR STATUS da CENTRAL de COMPRAS
                    if ((quantidadeDeEmpresasRespondendoACotacao == 0) && (DateTime.Now.Date <= listaCotacaoesDaCentralCompras[i].DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS.Date))
                    {
                        if (listaCotacaoesDaCentralCompras[i].temCotacaoAnexada == "nao")
                        {
                            textoStatus = "NOVA";
                        }
                        else
                        {
                            textoStatus = "AGUARDE";
                        }
                    }
                    else if ((quantidadeDeEmpresasRespondendoACotacao > 0) && (DateTime.Now.Date <= listaCotacaoesDaCentralCompras[i].DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS.Date))
                    {
                        //VERIFICA se a COTAÇÃO já foi ENVIADA aos FORNECEDORES
                        cotacaoEnviada =
                            negociosCotacaoMaster.VerificarSeACotacaoJahFoiEnviadaAosFornecedores(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                        if (cotacaoEnviada)
                        {
                            textoStatus = "COTANDO";
                        }
                        else
                        {
                            textoStatus = "EM ANDAMENTO";
                        }
                    }
                    else if ((quantidadeDeEmpresasRespondendoACotacao > 0) && (DateTime.Now.Date > listaCotacaoesDaCentralCompras[i].DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS.Date))
                    {
                        textoStatus = "ENCERRADA";
                    }

                    ///*
                    // OBS: Definir aqui outras condições para o STATUS.
                    // */

                    listaCotacaoesDaCentralCompras[i].statusCotacao = textoStatus;

                    //QUANTIDADE de EMPRESAS participantes da CC
                    int quantidadeDeEmpresasCC =
                         negociosEmpresasParticipantesCC.BuscarQuantidadeDeEmpresasParticipantes(listaCotacaoesDaCentralCompras[i].ID_CENTRAL_COMPRAS);

                    //QUANTIDADE de EMPRESAS com COTAÇÃO ANEXADA na CC
                    int quantidadeDeEmpresasCotacaoAnexada =
                        negociosCotacaoIndividualEmpresaCC.BuscarQuantidadeEmpresasComCotacaoAnexada(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                    //VERIFICAR STATUS para ENVIO da COTAÇÃO
                    if ((quantidadeDeEmpresasCotacaoAnexada) >= (((quantidadeDeEmpresasCC / 2) + 1)))
                    {
                        //VERDE - LIVRE
                        listaCotacaoesDaCentralCompras[i].statusEnvioCotacao = "green";
                    }
                    else if (((quantidadeDeEmpresasCotacaoAnexada) < (quantidadeDeEmpresasCC / 2)) && (quantidadeDeEmpresasCotacaoAnexada > 0))
                    {
                        //AMARELO - PERTO de LIBERAR
                        listaCotacaoesDaCentralCompras[i].statusEnvioCotacao = "orange";
                    }
                    else if ((quantidadeDeEmpresasCotacaoAnexada <= 1))
                    {
                        //VERMELHO - NÃO É POSSÍVEL LIBERAR
                        listaCotacaoesDaCentralCompras[i].statusEnvioCotacao = "red";
                    }

                    listaCotacaoesDaCentralCompras[i].cotacaoJahEnviadaAosFornecedores = listaCotacaoesDaCentralCompras[i].COTACAO_ENVIADA_FORNECEDORES;
                }

                //------------------------------------------------------------------
                //APLICAR FILTRO
                List<ListaDeCotacaoesDaCentralDeComprasViewModel> listaDeCotacoesDaCentralDeComprasFiltro = new List<ListaDeCotacaoesDaCentralDeComprasViewModel>();

                if (String.IsNullOrEmpty(descricaoFiltro) == false)
                {
                    listaDeCotacoesDaCentralDeComprasFiltro =
                        listaCotacaoesDaCentralCompras.Where(m => (m.NOME_COTACAO_CENTRAL_COMPRAS.ToUpper().Contains(descricaoFiltro.ToUpper()))).ToList();
                }
                else
                {
                    listaDeCotacoesDaCentralDeComprasFiltro = listaCotacaoesDaCentralCompras;
                }
                //------------------------------------------------------------------

                return Json(
                    new
                    {
                        rows = listaDeCotacoesDaCentralDeComprasFiltro,
                        current = 1,
                        rowCount = listaDeCotacoesDaCentralDeComprasFiltro.Count,
                        total = listaDeCotacoesDaCentralDeComprasFiltro.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ACEITAR CONVITE CENTRAL de COMPRAS
        public JsonResult AceitarConviteCC(int cCC)
        {
            var resultado = new { conviteAceitoCC = "" };

            NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

            //REGISTRAR ACEITAÇÃO do CONVITE
            negociosEmpresasParticipantesCC.AceitarConviteDeParticipacaoNaCC(cCC);

            resultado = new { conviteAceitoCC = "Ok" };

            return Json(resultado, JsonRequestBehavior.AllowGet);
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

        //CARREGA a LISTA de ESTADOS (Obs: No momento carregrá todos os estados brasileiros. Depois vejo como ficará)
        private List<SelectListItem> ListagemEstados()
        {
            try
            {
                //Buscar lista de Estados brasileiros
                NEstadosService negocioEstados = new NEstadosService();

                List<estados_empresa_usuario> listaEstados = negocioEstados.ListaEstados();

                List<SelectListItem> listEstados = new List<SelectListItem>();

                listEstados.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

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
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega as QUANTIDADES de FORNECEDORES a serem trazidos pelo FILTRO de BUSCA dos mesms
        private static List<SelectListItem> ListagemQuantiaDeFornecedores()
        {
            List<SelectListItem> listaQuantidades = new List<SelectListItem>();

            listaQuantidades.Add(new SelectListItem { Text = "5", Value = "5" });
            listaQuantidades.Add(new SelectListItem { Text = "7", Value = "7" });
            listaQuantidades.Add(new SelectListItem { Text = "10", Value = "10" });
            listaQuantidades.Add(new SelectListItem { Text = "12", Value = "12" });
            listaQuantidades.Add(new SelectListItem { Text = "15", Value = "15" });

            return listaQuantidades;
        }

        //GERAR a COTAÇÃO MASTER
        public JsonResult GerarCotacaoMaster(string nomeCotacao, string dataEncerramento, int tipoCotacao, int cRamoAtividade, int cCC, int cadmCC)
        {
            try
            {
                string nomeUsuario = "";
                string corpoEmail = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";
                string dataAjustada = Convert.ToDateTime(dataEncerramento).Year.ToString() + "-" + Convert.ToDateTime(dataEncerramento).Month.ToString() + "-"
                    + Convert.ToDateTime(dataEncerramento).Day.ToString();
                var resultado = new { cotacaoMasterGerada = "", iCM = "" };

                NCotacaoMasterCentralDeComprasService negociosCotacaoMasterCC = new NCotacaoMasterCentralDeComprasService();
                cotacao_master_central_compras dadosCotacaoMasterCC = new cotacao_master_central_compras();

                //POPULAR OBJETO
                dadosCotacaoMasterCC.ID_CENTRAL_COMPRAS = cCC;
                dadosCotacaoMasterCC.ID_CODIGO_TIPO_COTACAO = tipoCotacao;
                dadosCotacaoMasterCC.ID_CODIGO_STATUS_COTACAO = 1; //STATUS: ABERTA
                dadosCotacaoMasterCC.ID_TIPO_FRETE = 3;
                dadosCotacaoMasterCC.ID_GRUPO_ATIVIDADES = cRamoAtividade;
                dadosCotacaoMasterCC.NOME_COTACAO_CENTRAL_COMPRAS = nomeCotacao;
                dadosCotacaoMasterCC.DATA_CRIACAO_COTACAO_CENTRAL_COMPRAS = DateTime.Now;
                dadosCotacaoMasterCC.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS = DateTime.Now.AddDays(2); //As EMPRESAS PARCEIRAS devem inserir suas cotações em 2 dias no máximo
                dadosCotacaoMasterCC.DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS = Convert.ToDateTime(dataAjustada);
                dadosCotacaoMasterCC.CONDICAO_PAGAMENTO_COTACAO = "";
                dadosCotacaoMasterCC.OBSERVACAO_COTACAO = "";
                dadosCotacaoMasterCC.PERCENTUAL_RESPONDIDA_COTACAO = 0;
                dadosCotacaoMasterCC.NEGOCIACAO_CONTRA_PROPOSTA = false;
                dadosCotacaoMasterCC.SOLICITAR_CONFIRMACAO_COTACAO = false;
                dadosCotacaoMasterCC.NEGOCIACAO_COTACAO_ACEITA = false;

                //GRAVAR a COTAÇÃO MASTER
                cotacao_master_central_compras cotacaoMasterGerada = negociosCotacaoMasterCC.GerarContacaoMasterDaCentralDeCompras(dadosCotacaoMasterCC);

                if (cotacaoMasterGerada != null)
                {
                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO E-MAILS
                    //---------------------------------------------------------------------------------------------
                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();

                    //BUSCAR DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCC = negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(cCC, cadmCC);

                    //BUSCAR LISTA de EMPRESAS PARTICIPANTES CONFIRMADAS na CENTRAL COMPRAS
                    List<empresas_participantes_central_de_compras> dadosEmpresasParticipantesCC =
                        negociosEmpresasParticipantesCC.BuscarListaDeEmpresasConfirmadasComoParticipantesDaCC(cCC);

                    if (dadosEmpresasParticipantesCC.Count > 0)
                    {
                        int[] empresasParticipantes = new int[dadosEmpresasParticipantesCC.Count];

                        for (int a = 0; a < dadosEmpresasParticipantesCC.Count; a++)
                        {
                            empresasParticipantes[a] = dadosEmpresasParticipantesCC[a].ID_CODIGO_EMPRESA;
                        }

                        //CARREGAR DADOS das EMPRESAS PARTICIPANTES CONFIRMADAS na CENTRAL COMPRAS
                        List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                            negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(empresasParticipantes);

                        EnviarEmailAvisosDeNovaCotacaoGeradaCC enviarEmailsAvisoNovaCotacao = new EnviarEmailAvisosDeNovaCotacaoGeradaCC();

                        for (int i = 0; i < dadosEmpresasParticipantesCC.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa))
                            {
                                nomeUsuario = dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa;
                            }
                            else
                            {
                                nomeUsuario = dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa;
                            }

                            corpoEmail = " Sr(a) " + nomeUsuario + "<br><br>" +
                                         " A Central de Compras " + dadosDaCC.NOME_CENTRAL_COMPRAS + " acabou de abrir uma NOVA COTAÇÃO, nomeada por:<br><br>" +
                                         "<b>" + nomeCotacao.ToUpper() + ".</b><br><br>" +
                                         " Acesse a COTAÇÃO criada e informe os PRODUTOS que deseja cotar.<br>" +
                                         " O PRAZO MÁXIMO é até o dia: " + DateTime.Now.AddDays(2).ToString("dd/MM/yyyy") + ".<br>" +
                                         " Lhe desejamos uma boa negociação e boas compras.<br><br><br>" +
                                         "<br><br><br>Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br>";

                            //bool emailConviteAEmpresasParceiras =
                            //    enviarEmailsAvisoNovaCotacao.EnviandoEmailAvisoNovaCotacaoCC(dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa, 
                            //        dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa, dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, assuntoEmail, corpoEmail);

                            //---------------------------------------------------------------------------------------------
                            //ENVIANDO SMS´s
                            //---------------------------------------------------------------------------------------------
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            smsMensagem = "ClienteMercado - Sua CENTRAL de COMPRAS abriu uma nova COTACAO. Informe os PRODUTOS que deseja cotar. Acesse www.clientemercado.com.br.";

                            for (int j = 0; j < dadosEmpresasSelecionadas.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa))
                                {
                                    //TELEFONE 1 do USUÁRIO ADM
                                    telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa, "[()-]", "");
                                    telefoneUsuarioADM = telefoneUsuarioADM.Replace(" ", "");
                                    urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                                    bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                                    if (smsUsuarioVendedor)
                                    {
                                        //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                        NControleSMSService negociosSMS = new NControleSMSService();
                                        controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                        controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa;
                                        controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                                        controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)
                                        controleEnvioSms.DATA_ENVIO = DateTime.Now;

                                        negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    }
                                }

                                if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa))
                                {
                                    //TELEFONE 2 do USUÁRIO ADM
                                    telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa, "[()-]", "");
                                    telefoneUsuarioADM = telefoneUsuarioADM.Replace(" ", "");
                                    urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                                    bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                                    if (smsUsuarioVendedor)
                                    {
                                        //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                        NControleSMSService negociosSMS = new NControleSMSService();
                                        controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                        controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa;
                                        controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                                        controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)

                                        negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    }
                                }
                            }
                        }
                    }
                }

                resultado = new { cotacaoMasterGerada = "Ok", iCM = cotacaoMasterGerada.ID_COTACAO_MASTER_CENTRAL_COMPRAS.ToString() };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CONSULTA AUTOCOMPLETE ITENS / PRODUTOS para COTAÇÕES
        public JsonResult AutoCompleteItensProdutosParaCotacoes(string term)
        {
            try
            {
                //BUSCAR LISTA de PRODUTOS conforme digitado
                NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresa = new NProdutosServicosEmpresaProfissionalService();

                List<produtos_servicos_empresa_profissional> listaProdutosEmpresa = negocioProdutosServicosEmpresa.ListaProdutosEmpresa(term);

                return Json(listaProdutosEmpresa, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Autocomplete EMBALAGENS
        public JsonResult AutoCompleteProdutoEmbalagens(string term, int codProduto)
        {
            try
            {
                //Buscar lista de Fabricantes & Marcas registrados no Sistema
                NEmpresasProdutosEmbalagensService negociosEmpresasProdutosEmbalagens = new NEmpresasProdutosEmbalagensService();
                List<ListaDeEmbalagensDosProdutosViewModel> listaEmbalagens = new List<ListaDeEmbalagensDosProdutosViewModel>();

                //TRAZ EMBALAGENS VINCULADAS AO PRODUTO REGISTRADO
                listaEmbalagens = negociosEmpresasProdutosEmbalagens.ListaDeEmbalagensVinculadasAoProduto(term, codProduto);

                return Json(listaEmbalagens, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Autocomplete Fabricantes & Marcas
        public JsonResult AutoCompleteFabricantesEMarcas(string term, int codProduto)
        {
            try
            {
                //Buscar lista de Fabricantes & Marcas registrados no Sistema
                NEmpresasFabricantesMarcasService negocioEmpresasFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                List<ListaDeEmpresasFabricantesEMarcasViewModel> listaEmpresasFabricantesMarcas = new List<ListaDeEmpresasFabricantesEMarcasViewModel>();

                if (codProduto > 0)
                {
                    //TRAZ MARCAS/FABRICANTES VINCULADAS AO PRODUTO REGISTRADO
                    listaEmpresasFabricantesMarcas = negocioEmpresasFabricantesMarcas.ListaDeFabricantesEMarcasVinculadasAoProduto(term, codProduto);

                    if (listaEmpresasFabricantesMarcas.Count == 0)
                    {
                        //TRAZ TODAS AS MARCAS/FABRICANTES QUANDO É UM PRODUTO NÃO CADATRADO NA BASE
                        listaEmpresasFabricantesMarcas = negocioEmpresasFabricantesMarcas.ListaDeFabricantesEMarcas(term);
                    }
                }
                else
                {
                    //TRAZ TODAS AS MARCAS/FABRICANTES QUANDO É UM PRODUTO NÃO CADATRADO NA BASE
                    listaEmpresasFabricantesMarcas = negocioEmpresasFabricantesMarcas.ListaDeFabricantesEMarcas(term);
                }

                return Json(listaEmpresasFabricantesMarcas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CARREGAR LISTA de ITENS da COTAÇÃO INDIVIDUAL
        public JsonResult BuscarListaDeItensDaCotacaoIndividual(int iCI)
        {
            try
            {
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividual = new NItensCotacaoIndividualEmpresaCentralComprasService();

                //CARREGAR LISTA de ITENS que compoem a COTAÇÂO INDIVUAL
                List<ListaDeItensDaCotacaoIndividualViewModel> listaDeItensDaCotacaoIndividual = negociosItensCotacaoIndividual.CarregarListaDeItensDaCotacaoIndividual(iCI);

                return Json(
                    new
                    {
                        rows = listaDeItensDaCotacaoIndividual,
                        current = 1,
                        rowCount = listaDeItensDaCotacaoIndividual.Count,
                        total = listaDeItensDaCotacaoIndividual.Count
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CARREGAR LISTA de ITENS AGRUPADOS da COTAÇÃO da CENTRAL de COMPRAS
        public JsonResult BuscarListaDeItensAgrupadosDaCotacaoDaCentral(int iCM)
        {
            try
            {
                var listaIdsCotacoesIndividuais = "";

                NCotacaoIndividualEmpresaCentralComprasService negociosContacaoIndividualEmpresaCC = new NCotacaoIndividualEmpresaCentralComprasService();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensDasCotacoesIndividuais = new NItensCotacaoIndividualEmpresaCentralComprasService();

                //CARREGAR LISTA de COTAÇÕES INDIVIDUAIS ANEXADAS
                List<ListaDeIdsDasCotacoesIndividuaisViewModel> listaCotacaoesIndividuais =
                    negociosContacaoIndividualEmpresaCC.BuscarListaDeCotacoesIndividuaisAnexadas(iCM);

                List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel> listaDeItensAgrupadosDasCotacoesDaCC =
                    new List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel>();

                //BUSCAR ITENS da COTAÇÃO INDIVIDUAL - AGRUPADOS
                if (listaCotacaoesIndividuais.Count > 0)
                {
                    for (int i = 0; i < listaCotacaoesIndividuais.Count; i++)
                    {
                        if (listaIdsCotacoesIndividuais == "")
                        {
                            listaIdsCotacoesIndividuais = listaCotacaoesIndividuais[i].ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS.ToString();
                        }
                        else
                        {
                            listaIdsCotacoesIndividuais = (listaIdsCotacoesIndividuais + "," + listaCotacaoesIndividuais[i].ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS.ToString());
                        }
                    }

                    listaDeItensAgrupadosDasCotacoesDaCC =
                        negociosItensDasCotacoesIndividuais.CarregarListaDeItensAgrupadosDasCotacoesIndividuais(listaIdsCotacoesIndividuais, iCM);
                }

                return Json(
                    new
                    {
                        rows = listaDeItensAgrupadosDasCotacoesDaCC.OrderBy(m => m.descricaoProdutoCotacaoIndividual),
                        current = 1,
                        rowCount = listaDeItensAgrupadosDasCotacoesDaCC.Count,
                        total = listaDeItensAgrupadosDasCotacoesDaCC.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //BUSCAR LISTA de FORNECEDORES
        [WebMethod]
        public JsonResult BuscarFornecedores(int codRamoAtividade, int quantosFornecedores, int localBusca, int uFSelecionada, int codEAdmDaCC, int cCC)
        {
            try
            {
                //localBusca = 1 --> meu ESTADO
                //localBusca = 2 --> outro ESTADO
                //localBusca = 3 --> todo o PAÍS

                NFornecedoresService negociosFornecedores = new NFornecedoresService();

                //CARREGA LISTA de FORNECEDORES conforme LOCAL SELECIONADO
                List<ListaEstilizadaDeEmpresasViewModel> dadosPossiveisFornecedores =
                    negociosFornecedores.BuscarListaDePossiveisFornecedorasParaACentralDeCompras(codRamoAtividade, quantosFornecedores, localBusca, uFSelecionada, codEAdmDaCC, cCC);

                return Json(
                    new
                    {
                        rows = dadosPossiveisFornecedores,
                        current = 1,
                        rowCount = dadosPossiveisFornecedores.Count,
                        total = dadosPossiveisFornecedores.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                Trace.Write(erro.ToString());

                throw erro;
            }
        }

        //BUSCAR FORNECEDORES que RECEBERAM a COTAÇÃO ENVIADA
        public JsonResult BuscarFornecedoresQueJahReceberamACotacao(int iCM)
        {
            try
            {
                string idsEmpresasCotadas = "";

                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilha = new NCotacaoFilhaCentralDeComprasService();
                NFornecedoresService negociosFornecedores = new NFornecedoresService();

                //CARREGAR LISTA de IDS de EMPRESAS que RECEBERAM a COTAÇÃO
                List<ListaIdsDeEmpresasFornecedorasCotadas> listaIdasDosFornecedoresCotados = negociosCotacaoFilha.BuscaListaDeIdsDosFornecedoresCotados(iCM);

                for (int i = 0; i < listaIdasDosFornecedoresCotados.Count; i++)
                {
                    if (idsEmpresasCotadas == "")
                    {
                        idsEmpresasCotadas = listaIdasDosFornecedoresCotados[i].ID_CODIGO_EMPRESA.ToString();
                    }
                    else
                    {
                        idsEmpresasCotadas = (idsEmpresasCotadas + "," + listaIdasDosFornecedoresCotados[i].ID_CODIGO_EMPRESA.ToString());
                    }
                }

                List<ListaEstilizadaDeEmpresasViewModel> dadosFornecedoresQueReceberamACotacao = new List<ListaEstilizadaDeEmpresasViewModel>();

                //CARREGA LISTA de FORNECEDORES que RECEBERAM a COTAÇÃO
                if (listaIdasDosFornecedoresCotados.Count > 0)
                {
                    dadosFornecedoresQueReceberamACotacao = negociosFornecedores.BuscarListaDeFornecedoresQueReceberamACotacao(iCM, idsEmpresasCotadas);
                }

                return Json(
                    new
                    {
                        rows = dadosFornecedoresQueReceberamACotacao,
                        current = 1,
                        rowCount = dadosFornecedoresQueReceberamACotacao.Count,
                        total = dadosFornecedoresQueReceberamACotacao.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //------------------------------------------------------------------
        //GRAVAR DADOS EDITADOS do ITEM da COTAÇÃO INDIVIDUAL <----- TESTAR
        public JsonResult GravarDadosEditadosDoItemDaCotacao(int codItemCotacao, string descricaoProduto, int idProduto, int iCM, int codMarcaFabricanteAnterior,
            int codMarcaFabricante, string descricaoMarcaFabricante, string quantidadeProduto, int codUnidadeProduto, int codEmbalagem, string descricaoEmbalagem,
            int codGrupoAtividades)
        {
            try
            {
                var resultado = new { itemEditadoDaCotacao = "" };

                //========================================================================================================================================
                produtos_servicos_empresa_profissional dadosProduto = new produtos_servicos_empresa_profissional();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividual = new NItensCotacaoIndividualEmpresaCentralComprasService();
                itens_cotacao_individual_empresa_central_compras dadosItensCotacaoIndividual = new itens_cotacao_individual_empresa_central_compras();

                //SE PRODUTO ainda não for registrado no sistema
                if (idProduto == 0)
                {
                    dadosProduto = RegistrarProdutoNaBaseDeDados(descricaoProduto, codGrupoAtividades);

                    if (dadosProduto != null)
                    {
                        idProduto = dadosProduto.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS;
                    }
                }

                //SE a MARCA/FABRICANTE ainda não forem registrados
                if (codMarcaFabricante == 0)
                {
                    codMarcaFabricante = RegistrarMarcaOuFabricanteNaBaseDeDados(descricaoMarcaFabricante, idProduto);
                }

                //SE a EMBALAGEM ainda não tiver sido registrada
                if (codEmbalagem == 0)
                {
                    codEmbalagem = RegistrarEmbalagemNaBaseDeDados(descricaoEmbalagem, idProduto);
                }
                //========================================================================================================================================

                //GRAVAR DADOS EDITADOS do ITEM
                var quantidadeItens = quantidadeProduto.Replace(".", ",");

                dadosItensCotacaoIndividual.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = codItemCotacao;
                dadosItensCotacaoIndividual.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = iCM;
                dadosItensCotacaoIndividual.ID_CODIGO_UNIDADE_PRODUTO = codUnidadeProduto;
                dadosItensCotacaoIndividual.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = codMarcaFabricante;
                dadosItensCotacaoIndividual.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS = Convert.ToDecimal(quantidadeItens);

                if (String.IsNullOrEmpty(descricaoEmbalagem))
                {
                    dadosItensCotacaoIndividual.ID_EMPRESAS_PRODUTOS_EMBALAGENS = 1;
                }
                else
                {
                    dadosItensCotacaoIndividual.ID_EMPRESAS_PRODUTOS_EMBALAGENS = codEmbalagem;
                }

                negociosItensCotacaoIndividual.GravarItemEditadoNaCotacaoIndividualEmpresa(dadosItensCotacaoIndividual);

                resultado = new { itemEditadoDaCotacao = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        //------------------------------------------------------------------

        //EXCLUIR ITEM da COTAÇÃO INDIVIDUAL
        public JsonResult ExcluirItemDaCotacao(int codItemCotacao, int iCM)
        {
            try
            {
                var resultado = new { itemExcluidoDaCotacao = "" };

                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividual = new NItensCotacaoIndividualEmpresaCentralComprasService();

                //EXCLUIR ITEM da COTAÇÃO
                negociosItensCotacaoIndividual.ExcluirItemDaCotacao(codItemCotacao, iCM);

                resultado = new { itemExcluidoDaCotacao = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //EXCLUIR ITEM da COTAÇÃO INDIVIDUAL
        public JsonResult ExcluirTodosOsProdutosNaCotacaoIndividual(int iCI)
        {
            try
            {
                var resultado = new { itemsExcluidosDaCotacao = "" };

                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividual = new NItensCotacaoIndividualEmpresaCentralComprasService();

                //EXCLUIR TODOS os ITEMS da COTAÇÃO
                negociosItensCotacaoIndividual.ExcluirTodosOsItemsDaCotacao(iCI);

                resultado = new { itemsExcluidosDaCotacao = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GERAR COTACAO INDIVUDIAL da EMPRESA / INSERIR ITEM na COTAÇÃO
        public JsonResult AdicionarProdutosNaCotacaoIndividual(int cCC, int iPCC, int iCM, int iCI, int codProduto, int codMarcaFabricante, string descricaoProduto,
            string descricaoMarcaFabricante, int codGrupoAtividades, int codUnidadeProduto, string quantidadeDoItem, string descricaoEmbalagem, int codEmbalagem)
        {
            try
            {
                var resultado = new { produtoAdicionadoNaCotacaoIndividual = "", idCotacaoIndividualEmpresa = 0 };
                var quantidadeItem = quantidadeDoItem.Replace(".", ",");

                produtos_servicos_empresa_profissional dadosProduto = new produtos_servicos_empresa_profissional();
                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresaCC = new NCotacaoIndividualEmpresaCentralComprasService();
                cotacao_individual_empresa_central_compras dadosCotacaoIndividualGerada = new cotacao_individual_empresa_central_compras();

                //SE PRODUTO ainda não for registrado no sistema
                if (codProduto == 0)
                {
                    dadosProduto = RegistrarProdutoNaBaseDeDados(descricaoProduto, codGrupoAtividades);

                    if (dadosProduto != null)
                    {
                        codProduto = dadosProduto.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS;
                    }
                }

                //SE a MARCA/FABRICANTE ainda não forem registrados
                if (codMarcaFabricante == 0)
                {
                    codMarcaFabricante = RegistrarMarcaOuFabricanteNaBaseDeDados(descricaoMarcaFabricante, codProduto);
                }

                //SE a EMBALAGEM ainda não tiver sido registrada
                if (codEmbalagem == 0)
                {
                    codEmbalagem = RegistrarEmbalagemNaBaseDeDados(descricaoEmbalagem, codProduto);
                }

                //GERAR REGISTRO de COTAÇÃO INDIVIDUAL (SE NÃO EXISTIR AINDA)
                if (iCI == 0)
                {
                    cotacao_individual_empresa_central_compras dadosCotacaoIndividual = new cotacao_individual_empresa_central_compras();

                    dadosCotacaoIndividual.ID_COTACAO_MASTER_CENTRAL_COMPRAS = iCM;
                    dadosCotacaoIndividual.ID_EMPRESA_CENTRAL_COMPRAS = iPCC;
                    dadosCotacaoIndividual.ID_CODIGO_USUARIO_EMPRESA_CRIOU_COTACAO = (int)Sessao.IdUsuarioLogado;
                    dadosCotacaoIndividual.DATA_CRIACAO_COTACAO_INDIVIDUAL = DateTime.Now;
                    dadosCotacaoIndividual.COTACAO_INDIVIDUAL_ANEXADA = false;
                    dadosCotacaoIndividual.SOLICITAR_CONFIRMACAO_COTACAO = false;
                    dadosCotacaoIndividual.NEGOCIACAO_COTACAO_ACEITA = false;

                    //GRAVAR a COTAÇÃO INDIVIDUAL
                    dadosCotacaoIndividualGerada = negociosCotacaoIndividualEmpresaCC.GerarRegistroDeCotacaoIndividual(dadosCotacaoIndividual);

                    if (dadosCotacaoIndividualGerada != null)
                    {
                        iCI = dadosCotacaoIndividualGerada.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS;
                    }
                }

                //INSERIR ITENS na COTAÇÃO INDIVIDUAL
                if (iCI > 0)
                {
                    NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividualCC = new NItensCotacaoIndividualEmpresaCentralComprasService();
                    itens_cotacao_individual_empresa_central_compras dadosItensCotacaoIndividualCC = new itens_cotacao_individual_empresa_central_compras();

                    //POPULAR OBJETO
                    dadosItensCotacaoIndividualCC.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = iCI;
                    dadosItensCotacaoIndividualCC.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = codProduto;
                    dadosItensCotacaoIndividualCC.ID_CODIGO_UNIDADE_PRODUTO = codUnidadeProduto;
                    dadosItensCotacaoIndividualCC.ID_EMPRESAS_PRODUTOS_EMBALAGENS = codEmbalagem;
                    dadosItensCotacaoIndividualCC.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = 0;
                    dadosItensCotacaoIndividualCC.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = codMarcaFabricante;
                    dadosItensCotacaoIndividualCC.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS = Convert.ToDecimal(quantidadeItem);

                    //GRAVAR ITENS da COTAÇÃO INDIVIDUAL
                    itens_cotacao_individual_empresa_central_compras produtoAdicionadoNaCotacaoIndividual =
                        negociosItensCotacaoIndividualCC.GravarItemNaCotacaoIndividualDaEmpresa(dadosItensCotacaoIndividualCC);

                    if (produtoAdicionadoNaCotacaoIndividual != null)
                    {
                        resultado = new { produtoAdicionadoNaCotacaoIndividual = "Ok", idCotacaoIndividualEmpresa = iCI };
                    }
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                Trace.Write(erro.ToString());

                throw erro;
            }
        }

        //GRAVAR PRODUTO na BASE de DADOS
        public produtos_servicos_empresa_profissional RegistrarProdutoNaBaseDeDados(string descricaoProduto, int codGrupoAtividades)
        {
            try
            {
                var resultado = new { produtoAdicionadoNaBase = "" };

                NProdutosServicosEmpresaProfissionalService negociosProdutos = new NProdutosServicosEmpresaProfissionalService();
                produtos_servicos_empresa_profissional dadosDoProduto = new produtos_servicos_empresa_profissional();

                //POPULAR OBJ para gravação do PRODUTO
                dadosDoProduto.ID_GRUPO_ATIVIDADES = codGrupoAtividades;
                dadosDoProduto.DESCRICAO_PRODUTO_SERVICO = descricaoProduto;
                dadosDoProduto.TIPO_PRODUTO_SERVICO = "P";

                //GRAVAR PRODUTO
                produtos_servicos_empresa_profissional produtoGravado = negociosProdutos.GravarNovoProdutoServicoEmpresaProfissional(dadosDoProduto);

                return produtoGravado;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GRAVAR MARCA/FABRICANTE na BASE de DADOS e VINCULAR ao PRODUTO
        public int RegistrarMarcaOuFabricanteNaBaseDeDados(string descricaoMarcaFabricante, int codProduto)
        {
            try
            {
                NEmpresasFabricantesMarcasService negociosEmpresasFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                NEmpresasProdutosMarcasService negociosEmpresasProdutosMarcas = new NEmpresasProdutosMarcasService();
                empresas_fabricantes_marcas dadosFabricanteMarcas = new empresas_fabricantes_marcas();
                empresas_produtos_marcas dadosEmpresaProdutosMarcas = new empresas_produtos_marcas();

                //POPULAR OBJ para gravação do FABRICANTE/MARCA
                dadosFabricanteMarcas.DESCRICAO_EMPRESA_FABRICANTE_MARCAS = descricaoMarcaFabricante;

                //GRAVAR FABRICANTE/MARCA
                empresas_fabricantes_marcas fabricanteMarcaGravado = negociosEmpresasFabricantesMarcas.GravarNovaEmpresaFabricanteEMarcas(dadosFabricanteMarcas);

                if (fabricanteMarcaGravado != null)
                {
                    //POPULAR OBJ para GRAVAR VÍNCULO da MARCA/FABRICANTE ao PRODUTO
                    dadosEmpresaProdutosMarcas.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = fabricanteMarcaGravado.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS;
                    dadosEmpresaProdutosMarcas.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = codProduto;

                    //GRAVAR VÍNCULO da MARCA ou FABRICANTE ao PRODUTO
                    negociosEmpresasProdutosMarcas.GravarVinculoDaMarcaOuFabricanteAoProduto(dadosEmpresaProdutosMarcas);
                }

                return fabricanteMarcaGravado.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GRAVAR MARCA/FABRICANTE na BASE de DADOS e VINCULAR ao PRODUTO
        public int RegistrarEmbalagemNaBaseDeDados(string descricaoEmbalagem, int codProduto)
        {
            try
            {
                NEmpresasProdutosEmbalagensService negociosProdutosEmbalagens = new NEmpresasProdutosEmbalagensService();
                empresas_produtos_embalagens dadosProdutosEmbalagens = new empresas_produtos_embalagens();

                //POPULAR OBJ para gravação da EMBALAGEM
                dadosProdutosEmbalagens.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = codProduto;
                dadosProdutosEmbalagens.DESCRICAO_PRODUTO_EMBALAGEM = descricaoEmbalagem;

                //GRAVAR VÍNCULO da EMBALAGEM ao PRODUTO
                empresas_produtos_embalagens embalagemProdutoGravado = negociosProdutosEmbalagens.GravarVinculoDaEmbalagemAoProduto(dadosProdutosEmbalagens);

                return embalagemProdutoGravado.ID_EMPRESAS_PRODUTOS_EMBALAGENS;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ANEXAR a COTAÇÃO na COTAÇÃO MASTER
        public JsonResult AnexarACotacaoIndividualNaCotacaoMaster(int iPCC, int iCM)
        {
            try
            {
                var resultado = new { cotacaoAnexada = "" };

                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresa = new NCotacaoIndividualEmpresaCentralComprasService();

                //SETAR como ANEXADO
                negociosCotacaoIndividualEmpresa.AnexarMinhaCotacaoNaCotacaoMaster(iPCC, iCM);

                resultado = new { cotacaoAnexada = "sim" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ANEXAR a COTAÇÃO na COTAÇÃO MASTER
        public JsonResult DesanexarACotacaoIndividualNaCotacaoMaster(int iPCC, int iCM)
        {
            try
            {
                var dataLimite = "";
                var resultado = new { cotacaoDesanexada = "", dataLimiteAnexar = "" };

                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresa = new NCotacaoIndividualEmpresaCentralComprasService();
                NCotacaoMasterCentralDeComprasService negociosCotacaoMasterCC = new NCotacaoMasterCentralDeComprasService();

                //SETAR como DESANEXADO
                negociosCotacaoIndividualEmpresa.DesanexarMinhaCotacaoNaCotacaoMaster(iPCC, iCM);

                //CONSULTAR DADOS da COTACAO MASTER
                cotacao_master_central_compras dadosCotacaoMasterCC = negociosCotacaoMasterCC.ConsultarDadosDaCotacaoMasterCC(iCM);

                dataLimite =
                    dadosCotacaoMasterCC.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Day.ToString() + "/" + dadosCotacaoMasterCC.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Month.ToString() + "/"
                    + dadosCotacaoMasterCC.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Year.ToString();

                resultado = new { cotacaoDesanexada = "sim", dataLimiteAnexar = dataLimite };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega lista de Ramos de Comércio Atacadistas
        private static List<SelectListItem> ListagemDeDeRamosComercioAtacadista()
        {
            //Buscar lista de CATEGORIAS ATACADISTAS
            NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();

            List<grupo_atividades_empresa> listaCategoriasAtacadistas = negociosGruposDeAtividades.CarregarListaDeCategoriasAtacadistas();

            List<SelectListItem> listCategorias = new List<SelectListItem>();

            listCategorias.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

            foreach (var grupoCategoarias in listaCategoriasAtacadistas)
            {
                listCategorias.Add(new SelectListItem
                {
                    Text = grupoCategoarias.DESCRICAO_ATIVIDADE,
                    Value = grupoCategoarias.ID_GRUPO_ATIVIDADES.ToString()
                });
            }

            return listCategorias;
        }

        //ENVIAR AVISO de PROxIMIDADE DE DATA DE ENVIO DE COTAÇÃO
        public ActionResult EnviarAvisoDeAlertaDeProximidadeDeEnvioDeCotacao(int cCC, string eA, int iCM)
        {
            string nomeUsuario = "";
            string smsMensagem = "";
            string telefoneUsuarioADM = "";
            string urlParceiroEnvioSms = "";
            int[] listaIdsEmpresasDaCC;
            string dataLimite = "";

            var resultado = new { avisosAnexarCotacoesEnviados = "", dataPrazoFinalEncerramento = "" };

            int codCentralCompras = Convert.ToInt32(cCC);
            int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

            NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();

            cotacao_master_central_compras dadosDaCotacaoMaster =
                negociosCotacaoMaster.ConsultarDadosDaCotacaoMasterCC(iCM);

            dataLimite =
                dadosDaCotacaoMaster.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Day.ToString() + "/"
                + dadosDaCotacaoMaster.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Month.ToString() + "/"
                + dadosDaCotacaoMaster.DATA_LIMITE_ANEXAR_COTACAO_CENTRAL_COMPRAS.Year.ToString();

            NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
            NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
            NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
            NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualEmpresas = new NCotacaoIndividualEmpresaCentralComprasService();

            //BUSCA DADOS da CENTRAL de COMPRAS
            central_de_compras dadoscentralDeCompras =
                negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);

            //BUSCAR EMPRESAS que ainda NÃO ANEXARAM suas COTAÇÕES
            List<ListaDeIdsDasEmpresasQueAindaNaoAnexaramCotacao> listaEmpresasDaCCQueAindaNaoAnexaramCotacao =
                negociosCotacaoIndividualEmpresas.BuscarEmpresasQueAindaNaoAnexaramSuasCotacoes(iCM);

            listaIdsEmpresasDaCC = new int[listaEmpresasDaCCQueAindaNaoAnexaramCotacao.Count];

            if (listaEmpresasDaCCQueAindaNaoAnexaramCotacao.Count > 0)
            {
                for (int i = 0; i < listaEmpresasDaCCQueAindaNaoAnexaramCotacao.Count; i++)
                {
                    listaIdsEmpresasDaCC[i] = listaEmpresasDaCCQueAindaNaoAnexaramCotacao[i].ID_CODIGO_EMPRESA;
                }
            }

            List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(listaIdsEmpresasDaCC);

            //DISPARAR AOS PARCEIROS DA CENTRAL de COMPRAS, AVISO DE PROXIMIDADE DE ENVIO DE COTAÇÃO
            //---------------------------------------------------------------------------------------------
            //ENVIANDO E-MAILS
            //---------------------------------------------------------------------------------------------
            EnviarEmailAvisoProximidadeDeCotacao enviarEmailAvisos = new EnviarEmailAvisoProximidadeDeCotacao();

            //DISPARA E-MAIL´s para todas as EMPRESAS a serem COTADAS
            for (int i = 0; i < dadosEmpresasSelecionadas.Count; i++)
            {
                if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa))
                {
                    nomeUsuario = dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa;
                }
                else
                {
                    nomeUsuario = dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa;
                }

                bool emailAvisoDeEnvioDaCotacao =
                    enviarEmailAvisos.EnviandoEmail(dadosEmpresasSelecionadas[i].nomeEmpresa, dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa,
                    dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa, dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa,
                    dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, dadoscentralDeCompras.NOME_CENTRAL_COMPRAS, dataLimite);
            }

            //---------------------------------------------------------------------------------------------
            //ENVIANDO SMS´s
            //---------------------------------------------------------------------------------------------
            EnviarSms enviarSmsMaster = new EnviarSms();

            smsMensagem = "ClienteMercado - Você tem até o dia " + dataLimite + " para informar os produtos que deseja cotar. Acesse www.clientemercado.com.br.";

            for (int j = 0; j < dadosEmpresasSelecionadas.Count; j++)
            {
                if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa))
                {
                    //TELEFONE 1 do USUÁRIO ADM
                    telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa, "[()-]", "");
                    urlParceiroEnvioSms =
                        "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                    //if (smsUsuarioVendedor)
                    //{
                    //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                    //    NControleSMSService negociosSMS = new NControleSMSService();
                    //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                    //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa;
                    //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                    //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)
                    //    controleEnvioSms.DATA_ENVIO = DateTime.Now;

                    //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                    //}
                }

                if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa))
                {
                    //TELEFONE 2 do USUÁRIO ADM
                    telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa, "[()-]", "");
                    urlParceiroEnvioSms =
                        "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                    //if (smsUsuarioVendedor)
                    //{
                    //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                    //    NControleSMSService negociosSMS = new NControleSMSService();
                    //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                    //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa;
                    //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                    //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)

                    //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                    //}
                }

                //---------------------------------------------------------------------------------------------
                //ENVIANDO NOTIFICAÇÃO CELULAR
                //---------------------------------------------------------------------------------------------
                //3 - Enviar ALERT ao aplicativo no celular
                /*
                    CODIFICAR...
                */

                resultado = new { avisosAnexarCotacoesEnviados = "Ok", dataPrazoFinalEncerramento = dataLimite };
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //ENVIAR COTAÇÃO da CENTRAL de COMPRAS
        public ActionResult EnviarACotacaoParaOsFornecedores(string cCC, string eA, string iCM, int[] empresasFornecedorasSelecionadas, string avisarParceirosNaoCotantes)
        {
            try
            {
                string nomeUsuario = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";
                string listaIdsCotacoesIndividuais = "";
                int idCotacaoFilha = 0;
                bool cotacaoFilhaGerada = false;

                var resultado = new { avisosCotacoesEnviadas = "", cotacaoMasterEnviada = "" };

                int codCentralCompras = Convert.ToInt32(cCC);
                int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                NCotacaoIndividualEmpresaCentralComprasService negociosCotacoesIndividuaisDasEmpresasParticipantesDaCC =
                    new NCotacaoIndividualEmpresaCentralComprasService();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividualEmpresaDaCC = new NItensCotacaoIndividualEmpresaCentralComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();

                //BUSCA DADOS da EMPRESA ADM
                empresa_usuario dadosEmpresaADM =
                    negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(codEmpresaAdm) });

                //BUSCA DADOS da CENTRAL de COMPRAS
                central_de_compras dadoscentralDeCompras =
                    negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);

                //BUSCA LISTA de IDS das COTAÇÕES INDIVIDUAIS
                List<ListaDeIdsDasCotacoesIndividuaisViewModel> listaDeIDsDasCotacoesIndividuaisDasEmpresasDaCC =
                    negociosCotacoesIndividuaisDasEmpresasParticipantesDaCC.BuscarListaDeCotacoesIndividuaisAnexadas(Convert.ToInt32(iCM));

                List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel> listaDeItensAgrupadosDasCotacoesDaCC =
                    new List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel>();

                //BUSCAR ITENS da COTAÇÃO INDIVIDUAL - AGRUPADOS
                if (listaDeIDsDasCotacoesIndividuaisDasEmpresasDaCC.Count > 0)
                {
                    for (int i = 0; i < listaDeIDsDasCotacoesIndividuaisDasEmpresasDaCC.Count; i++)
                    {
                        if (listaIdsCotacoesIndividuais == "")
                        {
                            listaIdsCotacoesIndividuais =
                                listaDeIDsDasCotacoesIndividuaisDasEmpresasDaCC[i].ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS.ToString();
                        }
                        else
                        {
                            listaIdsCotacoesIndividuais =
                                (listaIdsCotacoesIndividuais + "," + listaDeIDsDasCotacoesIndividuaisDasEmpresasDaCC[i].ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS.ToString());
                        }
                    }

                    //CARREGAR LISTA de ITENS
                    listaDeItensAgrupadosDasCotacoesDaCC =
                        negociosItensCotacaoIndividualEmpresaDaCC.CarregarListaDeItensAgrupadosDasCotacoesIndividuais(listaIdsCotacoesIndividuais, 0);
                }

                //CARREGAR DADOS das EMPRESAS SELECIONADAS para receber a COTAÇÃO
                List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                    negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaRecerACotacaoDaCentralDeCompras(empresasFornecedorasSelecionadas);

                cotacao_filha_central_compras dadosCotacaoFilhaCentralDeCompras = new cotacao_filha_central_compras();

                for (int e = 0; e < dadosEmpresasSelecionadas.Count; e++)
                {
                    //Gerar a COTAÇÃO FILHA (cópia da Cotação MASTER) para cada FORNECEDOR que foi selecionado para RECEBER a COTAÇÃO
                    dadosCotacaoFilhaCentralDeCompras.ID_COTACAO_MASTER_CENTRAL_COMPRAS = Convert.ToInt32(iCM);
                    dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_EMPRESA = dadosEmpresasSelecionadas[e].idEmpresa;
                    dadosCotacaoFilhaCentralDeCompras.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[e].idUsuarioContatoResponsavel;
                    dadosCotacaoFilhaCentralDeCompras.ID_TIPO_FRETE = 2;
                    dadosCotacaoFilhaCentralDeCompras.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS = false;
                    dadosCotacaoFilhaCentralDeCompras.DATA_RESPOSTA_COTACAO_FILHA_CENTRAL_COMPRAS = Convert.ToDateTime("01/01/1900");
                    dadosCotacaoFilhaCentralDeCompras.FORMA_PAGAMENTO_COTACAO_FILHA_CENTRAL_COMPRAS = "À Vista";
                    dadosCotacaoFilhaCentralDeCompras.TIPO_DESCONTO = 0;
                    dadosCotacaoFilhaCentralDeCompras.PERCENTUAL_DESCONTO = 0;
                    dadosCotacaoFilhaCentralDeCompras.PRECO_LOTE_ITENS_COTACAO_CENTRAL_COMPRAS = 0;
                    dadosCotacaoFilhaCentralDeCompras.OBSERVACAO_COTACAO_CENTRAL_COMPRAS = null;
                    //dadosCotacaoFilhaCentralDeCompras.COTACAO_FILHA_CENTRAL_COMPRAS_EDITADA = false;
                    dadosCotacaoFilhaCentralDeCompras.RECEBEU_CONTRA_PROPOSTA = false;
                    dadosCotacaoFilhaCentralDeCompras.ACEITOU_CONTRA_PROPOSTA = false;
                    dadosCotacaoFilhaCentralDeCompras.RECEBEU_PEDIDO = false;
                    dadosCotacaoFilhaCentralDeCompras.DATA_RECEBEU_PEDIDO = Convert.ToDateTime("01/01/1900");
                    dadosCotacaoFilhaCentralDeCompras.CONFIRMOU_PEDIDO = false;
                    dadosCotacaoFilhaCentralDeCompras.DATA_CONFIRMOU_PEDIDO = Convert.ToDateTime("01/01/1900");
                    dadosCotacaoFilhaCentralDeCompras.DATA_RECEBEU_COTACAO_CENTRAL_COMPRAS = DateTime.Now;
                    dadosCotacaoFilhaCentralDeCompras.SOLICITAR_CONFIRMACAO_ACEITE_COTACAO = false;

                    cotacao_filha_central_compras gerarCotacaoFilha =
                        negociosCotacaoFilhaCentralDeCompras.GerarCotacaoFilhaDaCC(dadosCotacaoFilhaCentralDeCompras);

                    if (gerarCotacaoFilha != null)
                    {
                        idCotacaoFilha = gerarCotacaoFilha.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS;

                        //Gravar ITENS da COTAÇÃO FILHA gerada para o USUÁRIO que optou em responder esta COTAÇÃO AVULSA
                        if (listaDeItensAgrupadosDasCotacoesDaCC.Count > 0)
                        {
                            NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaNegociacaoCentralDeCompras =
                                new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                            itens_cotacao_filha_negociacao_central_compras dadosItensCotacaoFilhaCentralDeCompras = new itens_cotacao_filha_negociacao_central_compras();

                            for (int a = 0; a < listaDeItensAgrupadosDasCotacoesDaCC.Count; a++)
                            {
                                dadosItensCotacaoFilhaCentralDeCompras.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = idCotacaoFilha;
                                dadosItensCotacaoFilhaCentralDeCompras.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS =
                                    listaDeItensAgrupadosDasCotacoesDaCC[a].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS;
                                dadosItensCotacaoFilhaCentralDeCompras.ID_CODIGO_TIPO_RESPOSTA_COTACAO = 1;
                                dadosItensCotacaoFilhaCentralDeCompras.ID_CLASSIFICACAO_TIPO_ITENS_COTACAO = 1;
                                dadosItensCotacaoFilhaCentralDeCompras.DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS =
                                    listaDeItensAgrupadosDasCotacoesDaCC[a].descricaoProdutoCotacaoIndividual;
                                dadosItensCotacaoFilhaCentralDeCompras.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS =
                                    listaDeItensAgrupadosDasCotacoesDaCC[a].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS;
                                dadosItensCotacaoFilhaCentralDeCompras.PRODUTO_COTADO_CENTRAL_COMPRAS = false;
                                dadosItensCotacaoFilhaCentralDeCompras.ITEM_COTACAO_FILHA_EDITADO = false;
                                dadosItensCotacaoFilhaCentralDeCompras.PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS = 0;
                                dadosItensCotacaoFilhaCentralDeCompras.PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS = 0;

                                //GRAVAR ITENS da COTAÇÃO FILHA da CENTRAL de COMPRAS
                                itens_cotacao_filha_negociacao_central_compras gravadoItensDaCotacaoFilhaDaCentralDeCompras =
                                    negociosItensCotacaoFilhaNegociacaoCentralDeCompras.GravarItensDaCotacaoFilhaDaCentralCompras(dadosItensCotacaoFilhaCentralDeCompras);

                                if (gravadoItensDaCotacaoFilhaDaCentralDeCompras != null)
                                {
                                    cotacaoFilhaGerada = true;
                                }
                            }
                        }
                    }
                }

                //DISPARAR AVISOS aos FORNECEDORES
                if (cotacaoFilhaGerada)
                {
                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO E-MAILS FORNECEDORES
                    //---------------------------------------------------------------------------------------------
                    EnviarEmailCotacao enviarEmailAvisos = new EnviarEmailCotacao();

                    //DISPARA E-MAIL´s para todas as EMPRESAS a serem COTADAS
                    for (int i = 0; i < dadosEmpresasSelecionadas.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa))
                        {
                            nomeUsuario = dadosEmpresasSelecionadas[i].nickNameUsuarioContatoEmpresa;
                        }
                        else
                        {
                            nomeUsuario = dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa;
                        }

                        bool emailAvisoDeEnvioDaCotacao =
                            enviarEmailAvisos.EnviandoEmail(dadosEmpresasSelecionadas[i].nomeEmpresa, dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa,
                            dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa, dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa,
                            dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, dadoscentralDeCompras.NOME_CENTRAL_COMPRAS);
                    }

                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO SMS´s FORNECEDORES
                    //---------------------------------------------------------------------------------------------
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    smsMensagem = "ClienteMercado - Responda a COTACAO DIRECIONADA. Nao perca vendas. Baixe nosso app e responda ou acesse www.clientemercado.com.br.";

                    for (int j = 0; j < dadosEmpresasSelecionadas.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 1 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            //if (smsUsuarioVendedor)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMSService negociosSMS = new NControleSMSService();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                            //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)
                            //    controleEnvioSms.DATA_ENVIO = DateTime.Now;

                            //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 2 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            //if (smsUsuarioVendedor)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMSService negociosSMS = new NControleSMSService();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                            //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)

                            //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        //---------------------------------------------------------------------------------------------
                        //ENVIANDO NOTIFICAÇÃO CELULAR
                        //---------------------------------------------------------------------------------------------
                        //3 - Enviar ALERT ao aplicativo no celular
                        /*
                            CODIFICAR...
                        */
                    }

                    NCotacaoMasterCentralDeComprasService negociosCotacaoMasterDaCC = new NCotacaoMasterCentralDeComprasService();

                    //SETAR COTAÇÃO MASTER como ENVIADA - JÁ DISPARADA para os FORNECEDORES
                    negociosCotacaoMasterDaCC.SetarCotacaoMasterComoEnviadaAosFornecedores(Convert.ToInt32(iCM));

                    resultado = new { avisosCotacoesEnviadas = "Ok", cotacaoMasterEnviada = "sim" };
                }

                //AVISAR aos PARCEIROS que não ANEXARAM COTAÇÕES
                if (avisarParceirosNaoCotantes == "sim")
                {
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                    //CARREGA LISTA de ID´s das EMPRESAS que ainda não ANEXARAM COTAÇÃO
                    List<ListaDeIdsDasEmpresasQueAindaNaoAnexaramCotacao> empresasParceirasNaoCotantes =
                        negociosCotacoesIndividuaisDasEmpresasParticipantesDaCC.BuscarEmpresasQueAindaNaoAnexaramSuasCotacoes(Convert.ToInt32(iCM));

                    var listaIdsEmpresasNaoCotantes = "";

                    for (int a = 0; a < empresasParceirasNaoCotantes.Count; a++)
                    {
                        if (listaIdsEmpresasNaoCotantes == "")
                        {
                            listaIdsEmpresasNaoCotantes = empresasParceirasNaoCotantes[a].ID_CODIGO_EMPRESA.ToString();
                        }
                        else
                        {
                            listaIdsEmpresasNaoCotantes = (listaIdsEmpresasNaoCotantes + "," + empresasParceirasNaoCotantes[a].ID_CODIGO_EMPRESA.ToString());
                        }
                    }

                    //CARREGAR DADOS das EMPRESAS que NÃO ANEXARAM COTAÇÃO
                    List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasParceirasDaCCNaoCotantes =
                        negociosEmpresaUsuario.BuscarListaDeEmpresasPeloCodigo(listaIdsEmpresasNaoCotantes);

                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO SMS´s para EMPRESAS PARCEIRAS que NÃO ANEXARAM COTAÇÃO
                    //---------------------------------------------------------------------------------------------
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    smsMensagem = "ClienteMercado - Prazo encerrado para anexar sua cotação. Mais informações, veja no app e ou acesse www.clientemercado.com.br.";

                    for (int j = 0; j < dadosEmpresasParceirasDaCCNaoCotantes.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dadosEmpresasParceirasDaCCNaoCotantes[j].celular1_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 1 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresasParceirasDaCCNaoCotantes[j].celular1_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            //if (smsUsuarioVendedor)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMSService negociosSMS = new NControleSMSService();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                            //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)
                            //    controleEnvioSms.DATA_ENVIO = DateTime.Now;

                            //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        if (!string.IsNullOrEmpty(dadosEmpresasParceirasDaCCNaoCotantes[j].celular2_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 2 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresasParceirasDaCCNaoCotantes[j].celular2_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            //if (smsUsuarioVendedor)
                            //{
                            //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            //    NControleSMSService negociosSMS = new NControleSMSService();
                            //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            //    controleEnvioSms.TELEFONE_DESTINO = dadosEmpresasSelecionadas[j].celular2_UsuarioContatoEmpresa;
                            //    controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresasSelecionadas[j].idUsuarioContatoResponsavel;
                            //    controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de Convite CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)

                            //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            //}
                        }

                        //---------------------------------------------------------------------------------------------
                        //ENVIANDO NOTIFICAÇÃO CELULAR
                        //---------------------------------------------------------------------------------------------
                        //3 - Enviar ALERT ao aplicativo no celular
                        /*
                            CODIFICAR...
                        */
                    }
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
