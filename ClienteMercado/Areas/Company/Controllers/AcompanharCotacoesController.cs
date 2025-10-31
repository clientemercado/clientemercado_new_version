using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Sms;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class AcompanharCotacoesController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Sessao.IdEmpresaUsuario > 0)
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)     <== ESTE CÓDIGO FOI COPIADO e EXISTE GRANDE PROBABILIDADE DE SER MODIFICAO
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

                    AcompanharCotacaoesViewModel viewModelCC = new AcompanharCotacaoesViewModel();

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
                    viewModelCC.inListaDeUFs = ListagemEstados();

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Acompanhar Cotações";

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
                    ViewBag.ondeEstouAgora = "Acompanhar Cotações > Cotações da Central";

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
            try
            {
                NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();

                //CARREGA LISTA de CENTRAIS de COMPRAS
                List<ListaCentraisComprasViewModel> listaCC = negociosCentraisCompras.CarregarListaAutoCompleteCentraisDeCompras(term);

                return Json(listaCC, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult MapaDaCotacao(int cCC, int iCM)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0) && (iCM > 0))
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();

                    DadosDaCotacaoViewModel viewModelMapaCotacao = new DadosDaCotacaoViewModel();

                    //CARREGAR EMPRESAS COTANTES desta COTAÇÃO da CENTRAL COMPRAS
                    List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisDasEmpresasDaCC =
                        negociosCotacaoIndividual.BuscarListaDeCotacoesIndividuaisAnexadas(iCM);

                    //CARREGAR LISTA de EMPRESAS que JÁ REGISTRARAM o ACEITE dos VALORES COTADOS
                    List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor =
                        negociosCotacaoIndividual.BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(iCM);

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

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    cotacao_master_central_compras dadosDaCotacaoMaster = negociosCotacaoMaster.ConsultarDadosDaCotacaoMasterCC(iCM);

                    //POPULAR VIEW MODEL
                    viewModelMapaCotacao.cCC = cCC;
                    viewModelMapaCotacao.iCM = iCM;
                    viewModelMapaCotacao.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelMapaCotacao.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelMapaCotacao.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelMapaCotacao.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelMapaCotacao.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelMapaCotacao.inNomeUsuarioLogado = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelMapaCotacao.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelMapaCotacao.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelMapaCotacao.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    viewModelMapaCotacao.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelMapaCotacao.quantidadeEmpresasParticipantesDestaCotacao = cotacoesIndividuaisDasEmpresasDaCC.Count;
                    viewModelMapaCotacao.quantidadeEmpresasQueRegistraramOAceiteDosValoresCotados = cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor.Count;
                    viewModelMapaCotacao.listaDeEmpresasCotadas = ListaDeEmpresasCotadasParaFornecimento(iCM);
                    viewModelMapaCotacao.listaDeProdutosCotados = ListaDeProdutosDaCotacao(iCM);

                    if (dadosDaCotacaoMaster.SOLICITAR_CONFIRMACAO_COTACAO)
                    {
                        viewModelMapaCotacao.existeSolicitacaoDeConfirmacaoAprovandoRespostaDosFornecedores = "sim";
                        viewModelMapaCotacao.inCodEmpresaFornecedoraComRespostaEmAnalise = Convert.ToInt32(dadosDaCotacaoMaster.ID_EMPRESA_FORNECEDORA_APROVACAO);
                    }
                    else
                    {
                        viewModelMapaCotacao.existeSolicitacaoDeConfirmacaoAprovandoRespostaDosFornecedores = "nao";
                    }

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Acompanhar Cotações > Cotações da Central > Mapa da Cotação";

                    return View(viewModelMapaCotacao); 
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

        public ActionResult AnalisarRespostaDoFornecedor(int cCC, int iCM, int iCCF)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0) && (iCM > 0) && (iCCF > 0))
                {
                    int valor = 0;
                    decimal percentualIdealConfirmado = 0;
                    string mensagem = "";
                    string idsPedidos = "";

                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;
                    string usuarioFornecedor = "";
                    string textoMsgStatus = "PEDIDO FEITO&nbsp;";
                    var dataEntregaPedido = "";

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NLoginService negociosLoginUsuarioEmpresa = new NLoginService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();
                    NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
                    NChatCotacaoCentralComprasService negociosChatCotacaoCentralCompras = new NChatCotacaoCentralComprasService();
                    NPedidoCentralComprasService negociosPedidosCentralCompras = new NPedidoCentralComprasService();
                    NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaCentralCompras = new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                    NItensPedidoCentralComprasService negociosItensPedidosCentralCompras = new NItensPedidoCentralComprasService();

                    DadosDaCotacaoViewModel viewModelAnalisarResposta = new DadosDaCotacaoViewModel();

                    //CARREGAR DADOS da COTAÇÃO MASTER
                    cotacao_master_central_compras dadosDaCotacaoMaster = negociosCotacaoMaster.ConsultarDadosDaCotacaoMasterCC(iCM);

                    //PEGAR ID da EMPRESA LOGADA na CENTRAL de COMPRAS
                    empresas_participantes_central_de_compras dadosEmpresaParticipante =
                        negociosEmpresasParticipantesCC.BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(cCC, Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    //CARREGAR DADOS da COTAÇÃO INDIVIDUAL
                    cotacao_individual_empresa_central_compras dadosDaCotacaoIndividualEmpresaLogada =
                        negociosCotacaoIndividual.CarregarDadosDaCotacao(iCM, dadosEmpresaParticipante.ID_EMPRESA_CENTRAL_COMPRAS);

                    //CARREGAR EMPRESAS COTANTES desta COTAÇÃO da CENTRAL COMPRAS
                    List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisDasEmpresasDaCC =
                        negociosCotacaoIndividual.BuscarListaDeCotacoesIndividuaisAnexadas(iCM);

                    //CARREGAR LISTA de EMPRESAS que JÁ REGISTRARAM o ACEITE dos VALORES COTADOS
                    List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor =
                        negociosCotacaoIndividual.BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(iCM);

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosGeraisSobreACentralDeComprasPeloID(cCC);

                    empresa_usuario dadosEmpresaADM =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaADM =
                        negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    empresa_usuario dadosEmpresaLogada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresaLogada =
                        negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                    grupo_atividades_empresa dadosGruposAtividadesCC =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacaoEmpresaADM =
                        negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaADM.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //CONSULTAR DADOS da COTAÇÃO FILHA
                    cotacao_filha_central_compras dadosCotacaoFilha = negociosCotacaoFilhaCC.ConsultarDadosDaCotacaoFilhaCC(iCM, iCCF);

                    //CONSULTAR LISTA de ITENS da COTACAO FILHA
                    List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacaoFilha = 
                        negociosItensCotacaoFilhaCentralCompras.ConsultarItensDaCotacaoDaCC(iCCF);

                    //CONSULTAR DADOS da EMPRESA COTADA
                    empresa_usuario dadosDaEmpresaCotada =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosCotacaoFilha.ID_CODIGO_EMPRESA });
                    usuario_empresa dadosDoUsuarioDaEmpresaFornecedora =
                        negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(dadosDaEmpresaCotada.ID_CODIGO_EMPRESA);
                    empresa_usuario_logins dadosContatoUsuarioEmpresaFornecedora =
                        negociosLoginUsuarioEmpresa.ConsultarDadosDeContatoDoUsuarioDaEmpresaCotada(Sessao.IdEmpresaUsuario);

                    if (dadosDoUsuarioDaEmpresaFornecedora.NICK_NAME_USUARIO != null)
                    {
                        usuarioFornecedor = dadosDoUsuarioDaEmpresaFornecedora.NICK_NAME_USUARIO + " / " + dadosDoUsuarioDaEmpresaFornecedora.TELEFONE1_USUARIO_EMPRESA.Replace(" ", "-");
                    }
                    else
                    {
                        usuarioFornecedor = dadosDoUsuarioDaEmpresaFornecedora.NOME_USUARIO + " / " + dadosDoUsuarioDaEmpresaFornecedora.TELEFONE1_USUARIO_EMPRESA.Replace(" ", "-");
                    }

                    usuarioFornecedor = (usuarioFornecedor + " / " + dadosContatoUsuarioEmpresaFornecedora.EMAIL1_USUARIO);

                    //Busca os dados do CHAT entre a EMPRESA COTANTE e a EMPRESA FORNECEDORA
                    List<chat_cotacao_central_compras> listaConversasApuradasNoChat =
                        negociosChatCotacaoCentralCompras.BuscarChatEntreEmpresaCotanteEFornecedor(dadosCotacaoFilha.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS);

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

                    //POPULAR VIEW MODEL
                    viewModelAnalisarResposta.cCC = cCC;
                    viewModelAnalisarResposta.iCM = iCM;
                    viewModelAnalisarResposta.iCCF = iCCF;
                    viewModelAnalisarResposta.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelAnalisarResposta.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelAnalisarResposta.inCodUsuariomEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS;
                    viewModelAnalisarResposta.ineACriptografado = MD5Crypt.Criptografar(dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS.ToString());
                    viewModelAnalisarResposta.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelAnalisarResposta.inCodEmpresaLogada = (int)Sessao.IdEmpresaUsuario;
                    viewModelAnalisarResposta.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelAnalisarResposta.inNomeUsuarioLogado = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelAnalisarResposta.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelAnalisarResposta.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelAnalisarResposta.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    //viewModelAnalisarResposta.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelAnalisarResposta.idEmpresaFornecedoraCotada = dadosDaEmpresaCotada.ID_CODIGO_EMPRESA;
                    viewModelAnalisarResposta.idUsuarioEmpresaFornecedoraCotada = dadosDoUsuarioDaEmpresaFornecedora.ID_CODIGO_USUARIO;
                    viewModelAnalisarResposta.inNomeFantasiaEmpresaCotada = dadosDaEmpresaCotada.NOME_FANTASIA_EMPRESA;
                    viewModelAnalisarResposta.inNomeUsuarioRespDaEmpresaCotada = usuarioFornecedor;
                    viewModelAnalisarResposta.quantidadeEmpresasParticipantesDestaCotacao = cotacoesIndividuaisDasEmpresasDaCC.Count;
                    viewModelAnalisarResposta.quantidadeEmpresasQueRegistraramOAceiteDosValoresCotados = cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor.Count;

                    //VERIFICAR se a COTAÇÃO JÁ FOI RESPONDIDA
                    if (dadosCotacaoFilha.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS)
                    {
                        viewModelAnalisarResposta.cotacaoRespondida = "sim";
                    }
                    
                    //VERIFICAR SE JÁ FOI ENVIADO CONTRA-PROPOSTA A ALGUM OUTRO FORNECEDOR
                    if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == false)
                    {
                        //SE NÃO RECEBEU CONTRA-PROPOSTA
                        List<cotacao_filha_central_compras> listaDeCotacoesComContraProposta =
                            negociosCotacaoFilhaCC.BuscarListaDeCotacoesFilhaQueReceberamContraProposta(iCM);

                        if (listaDeCotacoesComContraProposta.Count > 0)
                        {
                            viewModelAnalisarResposta.existemCotacoesQueReceberamContraProposta = "sim";
                        }
                    }
                    else
                    {
                        //SE RECEBEU CONTRA-PROPOSTA
                        if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                        {
                            viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- FOI ACEITA PELO FORNECEDOR";
                        }
                        else
                        {
                            if ((dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == false) && (dadosCotacaoFilha.REJEITOU_CONTRA_PROPOSTA))
                            {
                                viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- NÃO FOI ACEITA POR ESTE FORNECEDOR";
                                viewModelAnalisarResposta.inRejeitouContraProposta = "sim";
                            }
                            else if ((dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == false) && (dadosCotacaoFilha.REJEITOU_CONTRA_PROPOSTA == false))
                            {
                                viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- AGUARDANDO RESPOSTA FORNECEDOR";
                                viewModelAnalisarResposta.inRejeitouContraProposta = "nao";
                            }
                        }
                    }

                    if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                    {
                        viewModelAnalisarResposta.possuiContraProposta = "sim";
                    }

                    if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                    {
                        viewModelAnalisarResposta.inAceitouContraProposta = "sim";
                    }
                    else if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == false)
                    {
                        viewModelAnalisarResposta.inAceitouContraProposta = "nao";
                    }

                    //VERIFICAR TODOS os PEDIDOS para BAIXA NESTA COTAÇÃO
                    List<SelectListItem> listaPedidosBaixa = new List<SelectListItem>();
                    List<pedido_central_compras> listaDePedidosParaACotacao = negociosPedidosCentralCompras.BuscarTodosOsPedidosParaBaixaNestaACotacao(iCM);
                    List<pedido_central_compras> listaPedidosBaixadosParaACotacao = negociosPedidosCentralCompras.BuscarTodosOsPedidosBaixadosParaEstaCotacao(iCM);

                    listaPedidosBaixa.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

                    for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                    {
                        if (idsPedidos == "")
                        {
                            idsPedidos = listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString();
                        }
                        else
                        {
                            idsPedidos = (idsPedidos + "," + listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString());
                        }

                        listaPedidosBaixa.Add(new SelectListItem
                        {
                            Text = listaDePedidosParaACotacao[i].COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS,
                            Value = listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString()
                        });
                    }

                    for (int a = 0; a < listaPedidosBaixadosParaACotacao.Count; a++)
                    {
                        dataEntregaPedido = (listaPedidosBaixadosParaACotacao[a].PEDIDO_ENTREGUE_FINALIZADO == true) ? listaPedidosBaixadosParaACotacao[a].DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS.ToShortDateString() : "";
                    }

                    viewModelAnalisarResposta.inListaPedidosBaixa = (listaPedidosBaixa != null) ? listaPedidosBaixa : null;

                    //BUSCAR LISTA de ITENS PEDIDOS
                    List<itens_pedido_central_compras> listaDeItensPedidos = new List<itens_pedido_central_compras>();

                    if (idsPedidos != "")
                    {
                        listaDeItensPedidos = negociosItensPedidosCentralCompras.ConsultarListaDeItensJahPedidos(idsPedidos);
                    }

                    if (listaDeItensPedidos.Count == listaDeItensDaCotacaoFilha.Count)
                    {
                        ViewBag.TodosItensPedidos = "sim";
                    }
                    else if ((listaDeItensPedidos.Count > 0) && (listaDeItensPedidos.Count < listaDeItensDaCotacaoFilha.Count))
                    {
                        ViewBag.TodosItensPedidos = "nao";

                        textoMsgStatus = "PEDIDO PARCIAL FEITO&nbsp;";
                    }

                    percentualIdealConfirmado = ((cotacoesIndividuaisDasEmpresasDaCC.Count * 100) / 100);

                    if (dadosCotacaoFilha.SOLICITAR_CONFIRMACAO_ACEITE_COTACAO)
                    {
                        if (cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor.Count < percentualIdealConfirmado)
                        {
                            viewModelAnalisarResposta.corQuantidadeConfirmada = "#FF0000";
                            viewModelAnalisarResposta.todosCotantesAceitaramNegociacao = "nao";

                            //ATUALIZAR STATUS
                            viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>AGUARDANDO</font>&nbsp;- RESPOSTA COTANTES";
                        }
                        else
                        {
                            viewModelAnalisarResposta.corQuantidadeConfirmada = "#32CD32";
                            viewModelAnalisarResposta.todosCotantesAceitaramNegociacao = "sim";

                            //SETAR NEGOCIAÇÃO COMO ACEITA por TODOS os COTANTES da CENTRAL de COMPRAS
                            if (dadosDaCotacaoMaster.NEGOCIACAO_COTACAO_ACEITA == false)
                            {
                                negociosCotacaoMaster.SetarEstaNegociaçãoComoAceitaPelosCotantes(iCM);
                            }

                            //ATUALIZAR STATUS
                            if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                            {
                                if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                                {
                                    viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- FOI ACEITA PELO FORNECEDOR";
                                }
                                else
                                {
                                    viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- AGUARDANDO RESPOSTA FORNECEDOR";
                                }
                            }

                            if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == false))
                            {
                                viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - AGUARDANDO CONFIRMAÇÃO FORNECEDOR";
                            }
                            else if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == true))
                            {
                                bool todosOsPedidosRecebidos = true;

                                for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                                {
                                    if (listaDePedidosParaACotacao[i].PEDIDO_ENTREGUE_FINALIZADO == false)
                                    {
                                        todosOsPedidosRecebidos = false;
                                    }
                                }

                                if (todosOsPedidosRecebidos == true)
                                {
                                    viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - PEDIDO RECEBIDO em " + dataEntregaPedido;
                                }
                                else
                                {
                                    viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - CONFIRMADO PELO FORNECEDOR / AGUARDANDO ENTREGA";
                                }
                            }
                        }
                    }

                    if (dadosCotacaoFilha.REJEITOU_PEDIDO)
                    {
                        viewModelAnalisarResposta.mensagemStatus = "<font color='#3297E0'>RECEBEU PEDIDO</font>&nbsp;- DESISTIU DE ATENDER A UM PEDIDO DA CENTRAL DE COMPRAS PARA ESTA COTAÇÃO";
                    }

                    if (dadosDaCotacaoMaster.SOLICITAR_CONFIRMACAO_COTACAO)
                    {
                        viewModelAnalisarResposta.existeSolicitacaoDeConfirmacaoAprovandoRespostaDosFornecedores = "sim";
                        viewModelAnalisarResposta.inCodEmpresaFornecedoraComRespostaEmAnalise = Convert.ToInt32(dadosDaCotacaoMaster.ID_EMPRESA_FORNECEDORA_APROVACAO);
                    }
                    else
                    {
                        viewModelAnalisarResposta.existeSolicitacaoDeConfirmacaoAprovandoRespostaDosFornecedores = "nao";
                    }

                    if (dadosDaCotacaoIndividualEmpresaLogada.NEGOCIACAO_COTACAO_ACEITA)
                    {
                        viewModelAnalisarResposta.negociacaoDoAdmComFornecedoresAceita = "sim";
                    }

                    if ((listaPedidosBaixadosParaACotacao.Count > 0) && ((listaPedidosBaixadosParaACotacao.Count) == (listaPedidosBaixadosParaACotacao.Where(m => (m.PEDIDO_ENTREGUE_FINALIZADO == true)).Count())))
                    {
                        viewModelAnalisarResposta.pedidoEntregueIntegralmente = "sim";
                    }

                    //VIEWBAGS
                    ViewBag.idPedido = 0;
                    ViewBag.codControlePedido = "";
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Acompanhar Cotações > Cotações da Central > Mapa da Cotação > Analisar Resposta do Fornecedor";

                    //VERIFICAR se o FORNECEDOR recebeu PEDIDO para ESTA COTAÇÃO
                    pedido_central_compras estaCotacaoRecebeuPedido = negociosPedidosCentralCompras.VerificarSeEstaCotacaoRecebeuPedido(iCM, iCCF);

                    if (estaCotacaoRecebeuPedido != null)
                    {
                        ViewBag.idPedido = estaCotacaoRecebeuPedido.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS;
                        ViewBag.codControlePedido = estaCotacaoRecebeuPedido.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS;
                    }

                    ViewBag.fornecedorConfirmouPedido = dadosCotacaoFilha.CONFIRMOU_PEDIDO;

                    if (dadosCotacaoFilha.REJEITOU_PEDIDO)
                    {
                        viewModelAnalisarResposta.rejeitouPedido = "sim";
                    }

                    return View(viewModelAnalisarResposta);
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

        public ActionResult VisualizarItensCotadosPorEmpresa(int cCC, int iCM)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && (cCC > 0) && (iCM > 0))
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NGruposAtividadesEmpresaProfissionalService negociosGrupoAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService();
                    NEnderecoEmpresaUsuarioService negociosLocalizacao = new NEnderecoEmpresaUsuarioService();
                    NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividualDasEmpresasParticipantesDaCC = new NCotacaoIndividualEmpresaCentralComprasService();

                    DadosDaCotacaoViewModel viewModelItensCotados = new DadosDaCotacaoViewModel();

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

                    List<ListaDeIdsDeEmpresasQueAnexaramACotacaoViewModel> listaDeIdsDeEmpresasQueAnexaramCotacao =
                        negociosCotacaoIndividualDasEmpresasParticipantesDaCC.BuscarListaDeIdsDeEmpresasQueAnexaramACotacao(iCM);

                    //POPULAR VIEW MODEL
                    viewModelItensCotados.cCC = cCC;
                    viewModelItensCotados.iCM = iCM;
                    viewModelItensCotados.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelItensCotados.inCodEmpresaADM = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS;
                    viewModelItensCotados.inEmpresaLogadaAdmCC = dadosEmpresaADM.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelItensCotados.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaADM.NOME_USUARIO;
                    viewModelItensCotados.NOME_FANTASIA_EMPRESA = dadosEmpresaLogada.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelItensCotados.inNomeUsuarioLogado = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelItensCotados.NOME_USUARIO = dadosUsuarioEmpresaLogada.NOME_USUARIO;
                    viewModelItensCotados.inRamoAtividadeCC = dadosGruposAtividadesCC.DESCRICAO_ATIVIDADE;
                    viewModelItensCotados.inCodRamoAtividadeCC = dadosGruposAtividadesCC.ID_GRUPO_ATIVIDADES;
                    viewModelItensCotados.inCidadeEmpresaAdmCC = dadosLocalizacaoEmpresaADM[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacaoEmpresaADM[0].UF_EMPRESA_USUARIO;
                    viewModelItensCotados.listaDeEmpresasQueAnexaramCotacaoESeusItensCotados =
                        ListaDeDeEmpresasQueAnexaramCotacaoEItensCotados(listaDeIdsDeEmpresasQueAnexaramCotacao);

                    //viewModelItensCotados.listaDeEmpresasCotadas = ListaDeEmpresasCotadasParaFornecimento(iCM);
                    //viewModelItensCotados.listaDeProdutosCotados = ListaDeProdutosDaCotacao(iCM);

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Acompanhar Cotações > Cotações da Central > Mapa da Cotação > Visualizar Itens Cotados por Empresa";

                    return View(viewModelItensCotados);
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

        //POPULAR LISTA de EMPRESAS para TESTE do MAPA de COTAÇÃO
        private static List<DadosEmpresasCotadasViewModel> ListaDeEmpresasCotadasParaFornecimento(int iCM)
        {
            try
            {
                var nomeDaEmpresaFornecedora = "";

                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                empresa_usuario dadosDaEmpresaCotada = new empresa_usuario();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaDaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();

                //BUSCAR LISTA de COTAÇÕES ENVIADAS
                List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> listaDeCotacoesEnviadasParaEmpresas =
                    negociosCotacaoFilhaDaCentralDeCompras.BuscarListaDeCotacoesEnviadasConformeCotacaoMaster(iCM);

                for (int a = 0; a < listaDeCotacoesEnviadasParaEmpresas.Count; a++)
                {
                    //BUSCAR DADOS da EMPRESA COTADA
                    dadosDaEmpresaCotada = negociosEmpresaUsuario.BuscarDadosEmpresaCotada(listaDeCotacoesEnviadasParaEmpresas[a].ID_CODIGO_EMPRESA);

                    listaDeCotacoesEnviadasParaEmpresas[a].nomeFantasiaEmpresaCotada = dadosDaEmpresaCotada.NOME_FANTASIA_EMPRESA;
                    listaDeCotacoesEnviadasParaEmpresas[a].telefone1DaEmpresaCotada = dadosDaEmpresaCotada.TELEFONE1_EMPRESA_USUARIO;
                    listaDeCotacoesEnviadasParaEmpresas[a].email1DaEmpresaCotada = dadosDaEmpresaCotada.EMAIL1_EMPRESA;

                    if (listaDeCotacoesEnviadasParaEmpresas[a].RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS)
                    {
                        listaDeCotacoesEnviadasParaEmpresas[a].cotacaoFoiRespondida = "sim";
                    }

                    if (listaDeCotacoesEnviadasParaEmpresas[a].RECEBEU_CONTRA_PROPOSTA == true)
                    {
                        listaDeCotacoesEnviadasParaEmpresas[a].recebeuContraProposta = "sim";
                    }

                    if (listaDeCotacoesEnviadasParaEmpresas[a].ACEITOU_CONTRA_PROPOSTA == true)
                    {
                        listaDeCotacoesEnviadasParaEmpresas[a].aceitouContraProposta = "sim";
                    }

                    if (listaDeCotacoesEnviadasParaEmpresas[a].REJEITOU_CONTRA_PROPOSTA == true)
                    {
                        listaDeCotacoesEnviadasParaEmpresas[a].rejeitouContraProposta = "sim";
                    }
                }

                List<DadosEmpresasCotadasViewModel> listaEmpresasCotadas = new List<DadosEmpresasCotadasViewModel>();

                for (int i = 0; i < listaDeCotacoesEnviadasParaEmpresas.Count; i++)
                {
                    nomeDaEmpresaFornecedora = listaDeCotacoesEnviadasParaEmpresas[i].nomeFantasiaEmpresaCotada.ToLower();

                    char[] delimiters = { ' ' };
                    string[] nomeEmpresa = nomeDaEmpresaFornecedora.Split(delimiters);
                    var nomeComposto = "";

                    if (nomeEmpresa.Length == 1)
                    {
                        nomeComposto = ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[0]);
                    }
                    else if (nomeEmpresa.Length > 1)
                    {
                        if (nomeEmpresa[1] == "&")
                        {
                            nomeComposto = (ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[0]) + " "
                                + ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[1])
                                + ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[2]));
                        }
                        else
                        {
                            nomeComposto = (ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[0]) + " "
                                + ConverterPrimeirosCaracteresEmCaixaAlta.ToProperCase(nomeEmpresa[1]));
                        }
                    }

                    listaEmpresasCotadas.Add(new DadosEmpresasCotadasViewModel
                    {
                        idCodCotacaoFilhaEnviadaAoFornecedor = listaDeCotacoesEnviadasParaEmpresas[i].ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS,
                        idEmpresaCotada = listaDeCotacoesEnviadasParaEmpresas[i].ID_CODIGO_EMPRESA,
                        nomeFantasiaEmpresaCotada = nomeComposto,
                        telefoneEmpresaCotada = listaDeCotacoesEnviadasParaEmpresas[i].telefone1DaEmpresaCotada,
                        emailEmpresaCotada = listaDeCotacoesEnviadasParaEmpresas[i].email1DaEmpresaCotada,
                        cotacaoFoiRespondida = listaDeCotacoesEnviadasParaEmpresas[i].cotacaoFoiRespondida,
                        recebeuContraProposta = listaDeCotacoesEnviadasParaEmpresas[i].recebeuContraProposta,
                        aceitouContraProposta = listaDeCotacoesEnviadasParaEmpresas[i].aceitouContraProposta,
                        rejeitouContraProposta = listaDeCotacoesEnviadasParaEmpresas[i].rejeitouContraProposta
                    });
                }

                return listaEmpresasCotadas;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega as siglas relacionadas às Unidades de peso e medida para os produtos da Cotação
        private static List<SelectListItem> ListagemUnidadesDePesoEMedida()
        {
            try
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
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //LISTA de PRODUTOS COTADOS para TESTE
        private static List<ListaDadosProdutoCotacaoViewModel> ListaDeProdutosDaCotacao(int iCM)
        {
            try
            {
                var unidadeProduto = "";
                decimal somaDosSubTotaisDoProdutoCotado = 0;

                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaDaCentralDeCompras = new NCotacaoFilhaCentralDeComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoCentralDeCompras = new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                NUnidadesProdutosService negociosUnidadesProdutos = new NUnidadesProdutosService();
                List<ListaDadosProdutoCotacaoViewModel> listaProdutosCotacoes = new List<ListaDadosProdutoCotacaoViewModel>();

                //BUSCAR LISTA de COTAÇÕES ENVIADAS
                List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> listaDeCotacoesEnviadasParaEmpresas =
                    negociosCotacaoFilhaDaCentralDeCompras.BuscarListaDeCotacoesEnviadasConformeCotacaoMaster(iCM);

                int[] listaIdsCotacaoFilhas = new int[listaDeCotacoesEnviadasParaEmpresas.Count];

                for (int y = 0; y < listaDeCotacoesEnviadasParaEmpresas.Count; y++)
                {
                    listaIdsCotacaoFilhas[y] = listaDeCotacoesEnviadasParaEmpresas[y].ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS;
                }

                List<ListaDadosProdutoCotacaoViewModel> listaDeItemsDaCotacao = new List<ListaDadosProdutoCotacaoViewModel>();

                //PEGA OS ITENS DA COTACAO, baseando-se na PRIMEIRA COTAÇÃO
                List<itens_cotacao_filha_negociacao_central_compras> itensDaCotacao =
                    negociosItensCotacaoCentralDeCompras.CarregarOsItensDeUmaCotacaoEnviada(listaDeCotacoesEnviadasParaEmpresas[0].ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS);

                for (int i = 0; i < itensDaCotacao.Count; i++)
                {
                    itens_cotacao_individual_empresa_central_compras itemDaCotacaoIndividual =
                        negociosCotacaoIndividual.ConsultarDadosDoItemDaCotacao(iCM, itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    //CARREGAR DADOS DE APENAS 1 PRODUTO COTADO PARA TODAS AS EMPRESAS NESSA COTAÇÃO
                    List<ListaProdutoUnicoCotadoPorEmpresaViewModel> listaDoProdutoCotadoPorEmpresa =
                        negociosItensCotacaoCentralDeCompras.CarregarDadosDeUmProdutoCotadoEmTodasAsEmpresasFornecedoras(listaIdsCotacaoFilhas,
                        itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    unidadeProduto = negociosUnidadesProdutos.ConsultarDescricaoDaUnidadeDoProduto(itemDaCotacaoIndividual.ID_CODIGO_UNIDADE_PRODUTO);

                    //var pos = (itensDaCotacao.Count - 1);
                    var pos = (listaDoProdutoCotadoPorEmpresa.Count - 1);

                    somaDosSubTotaisDoProdutoCotado += listaDoProdutoCotadoPorEmpresa[pos].subTotalMenorPreco;

                    listaProdutosCotacoes.Add(new ListaDadosProdutoCotacaoViewModel
                    {
                        descricaoProdutoCotacao = itensDaCotacao[i].DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS,
                        unidadeProdutoCotacao = unidadeProduto,
                        quantidadeProdutoCotacao = itensDaCotacao[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS.ToString(),
                        listaUmProdutoCotadoPorEmpresa = listaDoProdutoCotadoPorEmpresa, // <-- VER AQUI...
                        somaSubTotais = somaDosSubTotaisDoProdutoCotado.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "")
                    });
                }

                return listaProdutosCotacoes;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CONSULTA do AUTOCOMPLETE da LISTA de COTAÇÕES da CENTRAL de COMPRAS
        [WebMethod]
        public JsonResult BuscarListaDeNomesCotacaoesDaCC(string term)
        {
            try
            {
                //NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();
                NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();

                //CARREGA LISTA de COTAÇÕES da CENTRAL de COMPRAS
                List<cotacao_master_central_compras> listaCotacaoMasterCC = negociosCotacaoMaster.CarregarListaAutoCompleteDasCotacoesDaCC(term);

                return Json(listaCotacaoMasterCC, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
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

                    //QUANTIDADE de EMPRESAS respondendo a COTAÇÃO
                    int quantidadeDeEmpresasRespondendoACotacao =
                        negociosCotacaoFilhaCC.BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                    listaCotacaoesDaCentralCompras[i].quantasEmpresas = quantidadeDeEmpresasRespondendoACotacao;

                    //QUANTIDADE de EMPRESAS que responderam a COTAÇÃO
                    int quantidadeDeEmpresasJahResponderamACotacao =
                        negociosCotacaoFilhaCC.BuscarQuantidadeDeEmpresasJahResponderamACotacao(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                    listaCotacaoesDaCentralCompras[i].quantasEmpresasJahResponderam = quantidadeDeEmpresasJahResponderamACotacao;

                    empresas_participantes_central_de_compras dadosEmpresaPartcipanteCC =
                        negociosEmpresasParticipantesCC.ConsultarSeEmpresaParticipaDaCentralDeCompras(cCC);

                    //VERIFICANDO se TEM / NÃO TEM COTAÇÃO ANEXADA
                    listaCotacaoesDaCentralCompras[i].temCotacaoAnexada =
                        negociosCotacaoIndividualEmpresaCC.VerificarSeEmpresaPossuiCotacaoIndividualAnexada(listaCotacaoesDaCentralCompras[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS, dadosEmpresaPartcipanteCC.ID_EMPRESA_CENTRAL_COMPRAS);

                    //VERIFICAR STATUS da CENTRAL de COMPRAS
                    if ((quantidadeDeEmpresasRespondendoACotacao == 0) && (DateTime.Now.Date <= listaCotacaoesDaCentralCompras[i].DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS.Date))
                    {
                        if (listaCotacaoesDaCentralCompras[i].temCotacaoAnexada == "nao")
                        {
                            textoStatus = "NOVA";
                        }
                        else
                        {
                            textoStatus = "AGUARDANDO";
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
                }

                //APLICAR FILTRO
                List<ListaDeCotacaoesDaCentralDeComprasViewModel> listaDeCotacoesDaCentralDeComprasFiltro = new List<ListaDeCotacaoesDaCentralDeComprasViewModel>();

                if (descricaoFiltro != "")
                {
                    listaDeCotacoesDaCentralDeComprasFiltro = listaCotacaoesDaCentralCompras.Where(m => ((m.NOME_COTACAO_CENTRAL_COMPRAS.ToUpper().Contains(descricaoFiltro.ToUpper())) && (m.temCotacaoAnexada == "sim"))).ToList();
                }
                else
                {
                    //listaDeCotacoesDaCentralDeComprasFiltro = listaCotacaoesDaCentralCompras.Where(m => (m.temCotacaoAnexada == "sim")).ToList();
                    listaDeCotacoesDaCentralDeComprasFiltro = listaCotacaoesDaCentralCompras.ToList();
                }

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

        //CARREGA DADOS das EMPRESAS que ANEXARAM COTAÇÃO
        private static List<ListaEstilizadaDeEmpresasViewModel>
            ListaDeDeEmpresasQueAnexaramCotacaoEItensCotados(List<ListaDeIdsDeEmpresasQueAnexaramACotacaoViewModel> listaDeIdsDeEmpresasQueAnexaramCotacao)
        {
            try
            {
                int[] idsEmpresas = new int[listaDeIdsDeEmpresasQueAnexaramCotacao.Count];
                int[] idsCotacoesIndividuais = new int[listaDeIdsDeEmpresasQueAnexaramCotacao.Count];

                for (int i = 0; i < listaDeIdsDeEmpresasQueAnexaramCotacao.Count; i++)
                {
                    idsEmpresas[i] = Convert.ToInt32(listaDeIdsDeEmpresasQueAnexaramCotacao[i].ID_CODIGO_EMPRESA);
                    idsCotacoesIndividuais[i] = Convert.ToInt32(listaDeIdsDeEmpresasQueAnexaramCotacao[i].ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);
                }

                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoIndividual = 
                    new NItensCotacaoIndividualEmpresaCentralComprasService();

                //CARREGAR os DADOS das EMPRESAS que ANEXARAM COTAÇÃO
                List<ListaEstilizadaDeEmpresasViewModel> dadosDaEmpresasQueAnexaramCotacao =
                    negociosEmpresaUsuario.CarregarDadosDasEmpresasQueAnexaramCotacao(idsEmpresas, idsCotacoesIndividuais);

                //EXIBIR ÍCONE de LIKE para COTAÇÕES com NEGOCIAÇÕES ACEITAS
                for (int j = 0; j < listaDeIdsDeEmpresasQueAnexaramCotacao.Count; j++)
                {
                    if (listaDeIdsDeEmpresasQueAnexaramCotacao[j].NEGOCIACAO_COTACAO_ACEITA)
                    {
                        for (int g = 0; g < dadosDaEmpresasQueAnexaramCotacao[j].listaDeItensCotadosPorEmpresa.Count; g++)
                        {
                            dadosDaEmpresasQueAnexaramCotacao[j].listaDeItensCotadosPorEmpresa[g].cotacaoNegociacaoAceita = "sim";
                        }
                    }
                }

                return dadosDaEmpresasQueAnexaramCotacao;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ENVIAR COTAÇÃO da CENTRAL de COMPRAS
        public ActionResult BuscaListaDeItensDaCotacaoRespondidaPeloFornecedor(int iCM, int iCCF)
        {
            try
            {
                bool possuiContraProposta = false;
                decimal totalCalculado = 0;
                decimal somaTotalCalculado = 0;
                decimal menorPreco = 0;
                var unidadeProduto = "";
                var marcaDoProdutoCotado = "";
                var ehMenorPreco = "";
                var itemFoiPedido = "nao";
                bool itemPedidoCC = false;
                var habilitarSimOuNao = "";
                string hint = "";

                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoCentralDeCompras = new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                NEmpresasFabricantesMarcasService negociosEmpresasfabricantesMarcas = new NEmpresasFabricantesMarcasService();
                NUnidadesProdutosService negociosUnidadesProdutos = new NUnidadesProdutosService();
                NPedidoCentralComprasService pedidosService = new NPedidoCentralComprasService();
                NItensPedidoCentralComprasService negociosItensPedidoCC = new NItensPedidoCentralComprasService();
                List<ListaDadosProdutoCotacaoViewModel> listaProdutosCotacoes = new List<ListaDadosProdutoCotacaoViewModel>();
                pedido_central_compras dadosDoPedido = new pedido_central_compras();

                //PEGA OS ITENS DA COTACAO, baseando-se na PRIMEIRA COTAÇÃO
                List<itens_cotacao_filha_negociacao_central_compras> itensDaCotacao =
                    negociosItensCotacaoCentralDeCompras.CarregarOsItensDeUmaCotacaoEnviada(iCCF);

                for (int i = 0; i < itensDaCotacao.Count; i++)
                {
                    itens_cotacao_individual_empresa_central_compras itemDaCotacaoIndividual =
                        negociosCotacaoIndividual.ConsultarDadosDoItemDaCotacao(iCM, itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    unidadeProduto = negociosUnidadesProdutos.ConsultarDescricaoDaUnidadeDoProduto(itemDaCotacaoIndividual.ID_CODIGO_UNIDADE_PRODUTO);
                    marcaDoProdutoCotado = negociosEmpresasfabricantesMarcas.ConsultarDescricaoDaEmpresaFabricanteOuMarca(itemDaCotacaoIndividual.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                    //CONSULTAR SE o PRODUTO JÁ FOI PEDIDO A ALGUM FORNECEDOR
                    itens_pedido_central_compras itemJahPedido =
                        negociosItensPedidoCC.ConsultarSeOProdutoJahFoiPedido(itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    //CONSULTAR SE o PRODUTO FOI PEDIDO ao FORNECEDOR cuja RESPOSTA está sendo ANALISADA
                    itemPedidoCC =
                        negociosItensPedidoCC.ConsultarSeOFornecedorRecebeuPedidoParaEsteProduto(itensDaCotacao[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS);

                    if ((itemJahPedido != null) && (itemPedidoCC == false))
                    {
                        habilitarSimOuNao = "disabled";
                        hint = "Produto PEDIDO a OUTRO Fornecedor";
                    }

                    if (itemPedidoCC)
                    {
                        itemFoiPedido = "sim";
                        dadosDoPedido = pedidosService.ConsultarDadosDoPedidoPeloCodigo(itemJahPedido.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS);
                    }

                    if (itensDaCotacao[i].PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS > 0)
                    {
                        possuiContraProposta = true;
                    }

                    if (possuiContraProposta)
                    {
                        totalCalculado = (itensDaCotacao[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS * itensDaCotacao[i].PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS);
                    }
                    else
                    {
                        totalCalculado = (itensDaCotacao[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS * itensDaCotacao[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS);
                    }

                    somaTotalCalculado = (somaTotalCalculado + totalCalculado);

                    menorPreco =
                        negociosItensCotacaoCentralDeCompras.BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    ehMenorPreco = itensDaCotacao[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS == menorPreco ? "sim" : "nao";

                    //SE TODOS OS PEDIDOS DO PRODUTO JÁ FORAM ENTREGUES, DESABILTA CHECKBOX DOS PRODUTOS DO PEDIDO
                    habilitarSimOuNao = (dadosDoPedido.PEDIDO_ENTREGUE_FINALIZADO == true) ? "disabled" : "";

                    listaProdutosCotacoes.Add(new ListaDadosProdutoCotacaoViewModel
                    {
                        idItemCotacao = itensDaCotacao[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS,
                        idItemCotacaoIndividualEmpresa = itensDaCotacao[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS,
                        habilitar = habilitarSimOuNao,
                        hint = hint,
                        descricaoProdutoCotacao = itensDaCotacao[i].DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS,
                        marcaProduto = marcaDoProdutoCotado,
                        unidadeProdutoCotacao = unidadeProduto,
                        quantidadeProdutoCotacao = itensDaCotacao[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS.ToString(),
                        valorUnitarioTabela = itensDaCotacao[i].PRECO_UNITARIO_ITENS_TABELA_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", ""),
                        valorUnitarioDiferenciado = itensDaCotacao[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS == 0 ? "Não Cotou" : itensDaCotacao[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", ""),
                        valorUnitarioContraProposta = itensDaCotacao[i].PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", ""),
                        valorTotalUnitarioVsQuantidade = totalCalculado == 0 ? "Não Cotou" : totalCalculado.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", ""),
                        somaTotalCalculado = somaTotalCalculado.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", ""),
                        temContraProposta = possuiContraProposta,
                        ehOMenorPreco = ehMenorPreco,
                        itemFoiPedido = itemFoiPedido,
                        codControlePedido = dadosDoPedido.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS
                    });

                    itemFoiPedido = "nao";
                    habilitarSimOuNao = "";
                    hint = "";
                }

                return Json(
                        new
                        {
                            rows = listaProdutosCotacoes,
                            current = 1,
                            rowCount = listaProdutosCotacoes.Count,
                            total = listaProdutosCotacoes.Count,
                            dadosCarregados = "Ok"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega DIÁLOGOS entre COTANTE e FORNECEDOR
        public JsonResult GravarDialogoEntreCotanteDaCCEFornecedor(int idCotacaoFilha, int idUsuarioEmpresaCotante, int idUsuarioEmpresaCotada, string textoPerguntaOuResposta)
        {
            try
            {
                string retornoGravacao = "nok";

                var resultado = new { gravacaoChat = "" };

                //USUÁRIO EMPRESA COTANTE
                NChatCotacaoCentralComprasService negociosChatCotacaoCC = new NChatCotacaoCentralComprasService();
                chat_cotacao_central_compras dadosChatCotacaoCC = new chat_cotacao_central_compras();

                int numeroDeOrdemNaExibicao = (negociosChatCotacaoCC.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha) + 1);

                //Preparando os dados a serem gravados
                dadosChatCotacaoCC.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = idCotacaoFilha;
                dadosChatCotacaoCC.ID_CODIGO_USUARIO_EMPRESA_COTANTE = idUsuarioEmpresaCotante;
                dadosChatCotacaoCC.ID_CODIGO_USUARIO_EMPRESA_COTADA = idUsuarioEmpresaCotada;
                dadosChatCotacaoCC.DATA_CHAT_COTACAO_CENTRAL_COMPRAS = DateTime.Now;
                dadosChatCotacaoCC.TEXTO_CHAT_COTACAO_CENTRAL_COMPRAS = textoPerguntaOuResposta;
                dadosChatCotacaoCC.ID_CODIGO_USUARIO_DIALOGANDO = (int)Sessao.IdUsuarioLogado;
                dadosChatCotacaoCC.ORDEM_EXIBICAO_CHAT_COTACAO_CENTRAL_COMPRAS = numeroDeOrdemNaExibicao;

                //Gravar CONVERSA no CHAT - CENTRAL COMPRAS
                chat_cotacao_central_compras gravarPerguntaOuRespostaDoChat = negociosChatCotacaoCC.GravarConversaNoChat(dadosChatCotacaoCC);

                if (gravarPerguntaOuRespostaDoChat != null)
                {
                    retornoGravacao = "ok";
                }

                //Resultado a ser retornado
                resultado = new
                {
                    gravacaoChat = retornoGravacao
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega DIÁLOGOS entre COTANTE e FORNECEDOR
        public JsonResult CarregarDialogoEntreCotanteEFornecedor(int idCotacaoFilha)
        {
            try
            {
                //Carrega CHAT entre USUÁRIO ADM da CENTRAL COMPRAS e FORNECEDOR
                NChatCotacaoUsuarioEmpresaService negociosChatCotacaoUsuarioEmpresa = new NChatCotacaoUsuarioEmpresaService();

                NChatCotacaoCentralComprasService negociosChatCotacaoCC = new NChatCotacaoCentralComprasService();
                NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
                List<ChatEntreUsuarioEFornecedor> listaDeConversasEntreCotanteEFornecedorNoChat = new List<ChatEntreUsuarioEFornecedor>();

                //Busca os dados do CHAT entre o COTANTE e o FORNECEDOR
                List<chat_cotacao_central_compras> listaConversasApuradasNoChat =
                    negociosChatCotacaoCC.BuscarChatEntreUsuarioAdmDaCCEFornecedor(idCotacaoFilha);  //CONTINUAR AQUI

                if (listaConversasApuradasNoChat != null)
                {
                    //Montagem da lista de Fornecedores
                    for (int a = 0; a < listaConversasApuradasNoChat.Count; a++)
                    {
                        empresa_usuario dadosEmpresa =
                            negociosEmpresa.ConsultarDadoDaEmpresaPeloCodigoUsuario(listaConversasApuradasNoChat[a].ID_CODIGO_USUARIO_DIALOGANDO);

                        string[] nomeEmpresa = dadosEmpresa.NOME_FANTASIA_EMPRESA.Split(' ');

                        listaDeConversasEntreCotanteEFornecedorNoChat.Add(
                            new ChatEntreUsuarioEFornecedor(
                                listaConversasApuradasNoChat[a].ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS,
                                listaConversasApuradasNoChat[a].ID_CODIGO_USUARIO_EMPRESA_COTADA,
                                nomeEmpresa[0],
                                listaConversasApuradasNoChat[a].DATA_CHAT_COTACAO_CENTRAL_COMPRAS.ToString(),
                                listaConversasApuradasNoChat[a].TEXTO_CHAT_COTACAO_CENTRAL_COMPRAS,
                                listaConversasApuradasNoChat[a].ORDEM_EXIBICAO_CHAT_COTACAO_CENTRAL_COMPRAS
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

        //SOLICITAR CONFIRMAÇÃO de PREÇO aos DEMAIS COTANTES
        public JsonResult SolicitarConfirmacaoDaCotacaoAosDemaisCotantes(int cCC, string eA, int iCM, int iCCF, int idFor)
        {
            try
            {
                string nomeUsuario = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";
                int[] listaIdsEmpresasDaCC;
                string dataLimite = "";

                var resultado = new { confirmacaoSolicitada = "" };

                int codCentralCompras = Convert.ToInt32(cCC);
                int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilha = new NCotacaoFilhaCentralDeComprasService();

                //BUSCA DADOS da CENTRAL de COMPRAS
                central_de_compras dadoscentralDeCompras =
                    negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);

                //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE na tabela cotacao_master_central_compras 
                //(OBs: REGISTRA TBM o FORNECEDOR q está tendo sua RESPOSTA de COTAÇÃO AVALIADA)
                negociosCotacaoMaster.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM, idFor);

                //SETAR FLAG SOLICITAR_CONFIRMACAO_ACEITE_COTACAO na tabela cotacao_filha_central_compras
                negociosCotacaoFilha.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM, iCCF, idFor);

                //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE na tabela cotacao_individual_empresa_central_compras
                List<cotacao_individual_empresa_central_compras> listaDeCotacoesIndividuais =
                    negociosCotacaoIndividual.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM);

                //SETAR FLAG NEGOCIACAO_COTACAO_ACEITA como TRUE, para o ADM da CENTRAL de COMPRAS, na tabela cotacao_individual_empresa_central_compras
                cotacao_individual_empresa_central_compras dadosDaCotacaoIndividualDoADMdaCC =
                    negociosCotacaoIndividual.SetarFlagDeAceitacaoDaNegociacaoDaCotacaoRespondidaPelosFornecedores(codCentralCompras, iCM,
                    dadoscentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS);

                //REFAZ a LISTA eliminando a EMPRESA ADM, que ao SOLICITAR CONFIRMAÇÂO dos integrantes da CENTRAL COMPRAS, já deu como aceita a negociação de sua parte
                listaDeCotacoesIndividuais = listaDeCotacoesIndividuais.Where(m => (m.NEGOCIACAO_COTACAO_ACEITA == false)).ToList();

                listaIdsEmpresasDaCC = new int[listaDeCotacoesIndividuais.Count];

                if (listaDeCotacoesIndividuais.Count > 0)
                {
                    for (int i = 0; i < listaDeCotacoesIndividuais.Count; i++)
                    {
                        //CONSULTAR DADOS da EMPRESA PARTICIPANTE da CENTRAL COMPRAS
                        empresas_participantes_central_de_compras dadosEmpresasParticipantesDaCC =
                            negociosEmpresasParticipantesCC.BuscarDadosDaEmpresaParticipanteDaCC(cCC, listaDeCotacoesIndividuais[i].ID_EMPRESA_CENTRAL_COMPRAS);

                        listaIdsEmpresasDaCC[i] = dadosEmpresasParticipantesDaCC.ID_CODIGO_EMPRESA;
                    }
                }

                List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                    negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(listaIdsEmpresasDaCC);

                //DISPARAR AOS PARCEIROS DA CENTRAL de COMPRAS, AVISO PARA CONFERÊNCIA E ACEITE DA COTAÇÃO
                //---------------------------------------------------------------------------------------------
                //ENVIANDO E-MAILS
                //---------------------------------------------------------------------------------------------
                EnviarEmailSolicitandoConfirmacaoDaCotacao enviarEmailSolicitacao = new EnviarEmailSolicitandoConfirmacaoDaCotacao();

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
                        enviarEmailSolicitacao.EnviandoEmail(dadosEmpresasSelecionadas[i].nomeEmpresa, dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa,
                        dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa, dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa,
                        dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, dadoscentralDeCompras.NOME_CENTRAL_COMPRAS, dataLimite);
                }

                //---------------------------------------------------------------------------------------------
                //ENVIANDO SMS´s
                //---------------------------------------------------------------------------------------------
                EnviarSms enviarSmsMaster = new EnviarSms();

                smsMensagem = "ClienteMercado - Cotacao disponivel com precos para voce analisar e liberar para pedido. Acesse www.clientemercado.com.br.";

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

                    resultado = new { confirmacaoSolicitada = "Ok" };
                }

                //var teste = "";

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //REGISTRAR o ACEITE dos valoeres negociados pela EMPRESA ADM da CENTRAL de COMPRAS com o FORNECEDOR
        public JsonResult RegistrarAceiteDaNegociacaoDoAdmDaCCComOFornecedor(int cCC, int iCM, int idEmpresaLogada)
        {
            try
            {
                var resultado = new { confirmacaoNegociacao = "" };

                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();

                //PEGAR ID da EMPRESA LOGADA na CENTRAL de COMPRAS (ID da Empresa na CENTRAL de COMPRAS)
                empresas_participantes_central_de_compras dadosEmpresaParticipante =
                    negociosEmpresasParticipantesCC.BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(cCC, idEmpresaLogada);

                //SETAR FLAG NEGOCIACAO_COTACAO_ACEITA como TRUE na tabela cotacao_individual_empresa_central_compras
                cotacao_individual_empresa_central_compras cotacaoIndividualConfirmada =
                    negociosCotacaoIndividual.SetarFlagConfirmandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(iCM, dadosEmpresaParticipante.ID_EMPRESA_CENTRAL_COMPRAS);

                resultado = new { confirmacaoNegociacao = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public JsonResult RegistrarNaoAceitacaoDaNegociacaoDoAdmDaCCComOFornecedor(int cCC, int iCM, int idEmpresaLogada)
        {
            try
            {
                var resultado = new { rejeicaoNegociacao = "" };

                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();

                //PEGAR ID da EMPRESA LOGADA na CENTRAL de COMPRAS
                empresas_participantes_central_de_compras dadosEmpresaParticipante =
                    negociosEmpresasParticipantesCC.BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(cCC, idEmpresaLogada);

                //SETAR FLAG NEGOCIACAO_COTACAO_REJEITADA como TRUE na tabela cotacao_individual_empresa_central_compras
                cotacao_individual_empresa_central_compras cotacaoIndividualConfirmada =
                    negociosCotacaoIndividual.SetarFlagRejeitandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(iCM, 
                    dadosEmpresaParticipante.ID_EMPRESA_CENTRAL_COMPRAS);

                resultado = new { rejeicaoNegociacao = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GERAR CONTRA-PROPOSTA para a EMPRESA FORNECEDORA
        public JsonResult GerarContraPropostaAoFornecedorEmQuestao(int idCotacaoFilha, int idFornecedor, int cCC, int codEmpresaAdm)
        {
            try
            {
                var resultado = new { contraPropostaGerada = "" };
                decimal menorPreco = 0;
                var cotacaoIndividualSetadaComContraProposta = "nao";

                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilha = new NCotacaoFilhaCentralDeComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaNegociacao =
                    new NItensCotacaoFilhaNegociacaoCentralDeComprasService();

                List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacaoFilha =
                    negociosItensCotacaoFilhaNegociacao.CarregarOsItensDeUmaCotacaoEnviada(idCotacaoFilha);

                for (int i = 0; i < listaDeItensDaCotacaoFilha.Count; i++)
                {
                    //BUSCAR MENOR VALOR entre as respostas desta COTAÇÃO
                    menorPreco =
                        negociosItensCotacaoFilhaNegociacao.BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(listaDeItensDaCotacaoFilha[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                    //GRAVAR o MENOR VALOR APURADO na consulta como VALOR de CONTRA-PROPOSTA
                    negociosItensCotacaoFilhaNegociacao.GravarValorDaContraProposta(idCotacaoFilha, listaDeItensDaCotacaoFilha[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS, menorPreco);

                    if (cotacaoIndividualSetadaComContraProposta == "nao")
                    {
                        //SETAR como 'true' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
                        negociosCotacaoFilha.SetarCampoDeEnvioDeContraPropostaComoEnviada(idCotacaoFilha);
                        cotacaoIndividualSetadaComContraProposta = "sim";
                    }
                }

                //ENVIAR NOTIFICAÇÃO de CONTRA-PROPOSTA
                //==============================================================================================
                string nomeUsuario = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";

                int[] idEmpresasFornecedoraRespondendoACotacao = new int[1];

                idEmpresasFornecedoraRespondendoACotacao[0] = idFornecedor;

                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();

                //BUSCA DADOS da CENTRAL de COMPRAS
                central_de_compras dadoscentralDeCompras =
                    negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(cCC, codEmpresaAdm);

                //CARREGAR DADOS das EMPRESAS SELECIONADAS para receber a COTAÇÃO
                List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                    negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaRecerACotacaoDaCentralDeCompras(idEmpresasFornecedoraRespondendoACotacao);

                resultado = new { contraPropostaGerada = "Ok" };

                //DISPARAR AVISOS aos FORNECEDORES
                if (resultado.contraPropostaGerada == "Ok")
                {
                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO E-MAILS FORNECEDORES
                    //---------------------------------------------------------------------------------------------
                    EnviarEmailDeContraProposta enviarEmailAvisosContraProposta = new EnviarEmailDeContraProposta();

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
                            enviarEmailAvisosContraProposta.EnviandoEmail(dadosEmpresasSelecionadas[i].nomeEmpresa, dadosEmpresasSelecionadas[i].nomeUsuarioContatoEmpresa,
                            dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa, dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa,
                            dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, dadoscentralDeCompras.NOME_CENTRAL_COMPRAS);
                    }

                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO SMS´s FORNECEDORES
                    //---------------------------------------------------------------------------------------------
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    smsMensagem = "ClienteMercado - Voce tem uma CONTRA-PROPOSTA a analisar. Nao perca vendas. Baixe nosso app e responda ou acesse www.clientemercado.com.br.";

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

                    //NCotacaoMasterCentralDeComprasService negociosCotacaoMasterDaCC = new NCotacaoMasterCentralDeComprasService();

                    ////SETAR COTAÇÃO MASTER como ENVIADA - JÁ DISPARADA para os FORNECEDORES
                    //negociosCotacaoMasterDaCC.SetarCotacaoMasterComoEnviadaAosFornecedores(Convert.ToInt32(iCM));

                    //resultado = new { avisosCotacoesEnviadas = "Ok", cotacaoMasterEnviada = "sim" };
                }
                //==============================================================================================

                resultado = new { contraPropostaGerada = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GERAR CONTRA-PROPOSTA para a EMPRESA FORNECEDORA
        public JsonResult CancelarContraPropostaAoFornecedorEmQuestao(int idCotacaoFilha)
        {
            try
            {
                var resultado = new { contraPropostaGeradaCancelada = "" };

                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilha = new NCotacaoFilhaCentralDeComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaNegociacao =
                    new NItensCotacaoFilhaNegociacaoCentralDeComprasService();

                //LIMPAR CAMPOS com o MENOR VALOR APURADO UTILIZADO como VALOR de CONTRA-PROPOSTA
                negociosItensCotacaoFilhaNegociacao.LimparCamposComValorDeContraPropostaAoFornecedor(idCotacaoFilha);

                //SETAR como 'false' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
                negociosCotacaoFilha.SetarCampoDeEnvioDeContraPropostaComoCancelada(idCotacaoFilha);

                resultado = new { contraPropostaGeradaCancelada = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ENVIAR PEDIDO ao FORNECEDOR
        public JsonResult EnviarOPedidoAoFornecedor(int cCC, int iCM, int iCCF, string codsProdutosNegociacao, string codsItensIndividuais, 
            string somaItensDoPedido, int idPedido, int idFornecedor, string nomeCentralCompras, string aceitouCP)
        {
            try
            {
                var resultado = new { pedidoFeito = "nao", idPedido = 0, codControlePedido = "", todosItensPedidos = "", mensagemStatus = "" };
                var idItemPedido = 0;
                var nomeUsuario = "";
                int idPedidoGeradoCC = idPedido;
                string idsPedidos = "";
                string codControlePedido = "";
                string todosItensPedidos = "nao";
                string dataLimiteAcatamentoDoPedido = ((DateTime.Now.AddDays(1)).Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";
                string textoMsgStatus = "PEDIDO FEITO&nbsp;";
                var dataEntregaPedido = "";

                string[] listaIDsItensPedidosNegociacao, listaItensIndividuaisCotacao;
                listaIDsItensPedidosNegociacao = codsProdutosNegociacao.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                listaItensIndividuaisCotacao = codsItensIndividuais.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                somaItensDoPedido = somaItensDoPedido.Replace('.', ',');

                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoindividualEmpresaCC = 
                    new NItensCotacaoIndividualEmpresaCentralComprasService();
                NPedidoCentralComprasService negociosPedidosCC = new NPedidoCentralComprasService();
                NItensPedidoCentralComprasService negociosItensPedidoCC = new NItensPedidoCentralComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaCentralCompras = 
                    new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();
                pedido_central_compras dadosPedidoCC = new pedido_central_compras();
                pedido_central_compras pedidoGerado = new pedido_central_compras();
                itens_pedido_central_compras dadosItensPedidoCC = new itens_pedido_central_compras();

                if (idPedidoGeradoCC == 0)
                {
                    codControlePedido = GerarCodigoControleDoPedido(cCC);

                    //GERAR o PEDIDO feito pelo USUÁRIO ADM da CENTRAL de COMPRAS (Independente do Pedido ser TOTAL ou PARCIAL)
                    dadosPedidoCC.ID_CODIGO_COTACAO_MASTER_CENTRAL_COMPRAS = iCM;
                    dadosPedidoCC.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = iCCF;
                    dadosPedidoCC.VALOR_PEDIDO_CENTRAL_COMPRAS = Convert.ToDecimal(somaItensDoPedido);
                    dadosPedidoCC.DATA_PEDIDO_CENTRAL_COMPRAS = DateTime.Now;
                    dadosPedidoCC.DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS = Convert.ToDateTime("1900-01-01");
                    dadosPedidoCC.CONFIRMADO_PEDIDO_CENTRAL_COMPRAS = false;
                    dadosPedidoCC.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS = codControlePedido;

                    pedidoGerado = negociosPedidosCC.GerarPedidoCC(dadosPedidoCC);

                    idPedidoGeradoCC = pedidoGerado.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS;

                    //SETAR RECEBIMENTO de PEDIDO pra esta COTACAO
                    negociosCotacaoFilhaCC.SetarReceBimentoDePedidoParaACotacao(iCM, iCCF);
                }
                else
                {
                    //ATUALIZAR PEDIDO (OBS: EM ESPECIAL A DATA, SE O FORNECEDOR ESTÁ RECEBENDO PEDIDO DE MAIS PRODUTOS DESTA COTAÇÃO)
                    negociosCotacaoFilhaCC.SetarReceBimentoDePedidoParaACotacao(iCM, iCCF);

                    //BUSCAR DADOS do PEDIDO
                    dadosPedidoCC = negociosPedidosCC.ConsultarDadosDoPedidoPeloCodigo(idPedidoGeradoCC);

                    //ATUALIZAR o VALOR TOTAL REGISTRADO para o PEDIDO
                    dadosPedidoCC.VALOR_PEDIDO_CENTRAL_COMPRAS = (dadosPedidoCC.VALOR_PEDIDO_CENTRAL_COMPRAS + Convert.ToDecimal(somaItensDoPedido));
                    dadosPedidoCC = negociosPedidosCC.AtualizarValorDoPedido(dadosPedidoCC);

                    codControlePedido = dadosPedidoCC.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS;
                }

                //GRAVAR os ITENS do PEDIDO
                if (idPedidoGeradoCC > 0)
                {
                    for (int i = 0; i < listaIDsItensPedidosNegociacao.Length; i++)
                    {
                        dadosItensPedidoCC.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = idPedidoGeradoCC;
                        dadosItensPedidoCC.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS = Convert.ToInt32(listaIDsItensPedidosNegociacao[i]);
                        dadosItensPedidoCC.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = Convert.ToInt32(listaItensIndividuaisCotacao[i]);

                        idItemPedido = negociosItensPedidoCC.GravarItemDoPedido(dadosItensPedidoCC);

                        //SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO e quem é o FORNECEDOR
                        negociosItensCotacaoindividualEmpresaCC.SetarItemComoPedido(Convert.ToInt32(listaItensIndividuaisCotacao[i]), idFornecedor, 
                            idPedidoGeradoCC);
                    }
                }

                //CONSULTAR LISTA de ITENS da COTACAO FILHA
                List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacaoFilha =
                    negociosItensCotacaoFilhaCentralCompras.ConsultarItensDaCotacaoDaCC(iCCF);

                //VERIFICAR TODOS os PEDIDOS para ESTA COTAÇÃO
                List<pedido_central_compras> listaDePedidosParaACotacao = negociosPedidosCC.BuscarTodosOsPedidosParaACotacao(iCM);

                for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                {
                    if (idsPedidos == "")
                    {
                        idsPedidos = listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString();
                    }
                    else
                    {
                        idsPedidos = (idsPedidos + "," + listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString());
                    }
                }

                //BUSCAR LISTA de ITENS PEDIDOS
                List<itens_pedido_central_compras> listaDeItensPedidos = new List<itens_pedido_central_compras>();

                if (idsPedidos != "")
                {
                    listaDeItensPedidos = negociosItensPedidoCC.ConsultarListaDeItensJahPedidos(idsPedidos);
                }

                if (listaDeItensPedidos.Count == listaDeItensDaCotacaoFilha.Count)
                {
                    todosItensPedidos = "sim";
                }
                else
                {
                    textoMsgStatus = "PEDIDO PARCIAL FEITO&nbsp;";
                }

                //SE FOI CONTRA-PROPOSTA ACEITA, MARCAR NA COTAÇÃO MASTER
                if (aceitouCP == "sim")
                {
                    negociosCotacaoMaster.SetarCampoDeContraProposta(iCM);
                }

                if (idItemPedido > 0)
                {
                    var mensagemDoStatus = "";

                    //CONSULTAR DADOS da COTAÇÃO FILHA
                    cotacao_filha_central_compras dadosCotacaoFilha = negociosCotacaoFilhaCC.ConsultarDadosDaCotacaoFilhaCC(iCM, iCCF);

                    //ATUALIZAR STATUS
                    if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                    {
                        if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                        {
                            mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- FOI ACEITA PELO FORNECEDOR";
                        }
                        else
                        {
                            mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- AGUARDANDO RESPOSTA FORNECEDOR";
                        }
                    }

                    if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == false))
                    {
                        mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - AGUARDANDO CONFIRMAÇÃO FORNECEDOR";
                    }
                    else if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == true))
                    {
                        List<pedido_central_compras> listaDePedidosParaACotacaoCC = negociosPedidosCC.BuscarTodosOsPedidosParaBaixaNestaACotacao(iCM);

                        bool todosOsPedidosRecebidos = true;

                        for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                        {
                            if (listaDePedidosParaACotacao[i].PEDIDO_ENTREGUE_FINALIZADO == false)
                            {
                                todosOsPedidosRecebidos = false;
                            }

                            dataEntregaPedido = (listaDePedidosParaACotacao[i].PEDIDO_ENTREGUE_FINALIZADO == true) ? listaDePedidosParaACotacao[i].DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS.ToShortDateString() : "";
                        }

                        if (todosOsPedidosRecebidos == true)
                        {
                            mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - PEDIDO RECEBIDO em " + dataEntregaPedido;
                        }
                        else
                        {
                            mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - CONFIRMADO PELO FORNECEDOR / AGUARDANDO ENTREGA";
                        }   
                    }

                    resultado = new
                    {
                        pedidoFeito = "sim",
                        idPedido = idPedidoGeradoCC,
                        codControlePedido = pedidoGerado.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS,
                        todosItensPedidos = todosItensPedidos,
                        mensagemStatus = mensagemDoStatus
                    };

                    //===========================================================================
                    /*
                     DISPARAR AQUI PARA O VENDEDOR:                        
                        - E-MAILS, SMS, NOTIFICAÇÕES VIA CELULAR --> INFORMAÇÕES no SISTEMA sobre O PEDIDO
                    */

                    //DISPARAR AO FORNECEDOR, AVISO PARA CONFERÊNCIA E ACEITE DO PEDIDO
                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO E-MAILS
                    //---------------------------------------------------------------------------------------------
                    //CARREGAR DADOS do FORNECEDOR
                    ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel dadosEmpresaSelecionada =
                        negociosEmpresaUsuario.BuscarDadosDaEmpresaParaEnvioDeMensagens(idFornecedor);

                    EnviarEmailAvisandoSobreORecebimentoPedidoPeloFornecedor enviarEmailAvisandoSobreOPedido =
                        new EnviarEmailAvisandoSobreORecebimentoPedidoPeloFornecedor();

                    //DISPARA E-MAIL´s para todas as EMPRESAS a serem COTADAS
                    if (!string.IsNullOrEmpty(dadosEmpresaSelecionada.nickNameUsuarioContatoEmpresa))
                    {
                        nomeUsuario = dadosEmpresaSelecionada.nickNameUsuarioContatoEmpresa;
                    }
                    else
                    {
                        nomeUsuario = dadosEmpresaSelecionada.nomeUsuarioContatoEmpresa;
                    }

                    //ENVIAR E-MAIL
                    bool emailAvisoDePedido =
                        enviarEmailAvisandoSobreOPedido.EnviandoEmail(dadosEmpresaSelecionada.nomeEmpresa, dadosEmpresaSelecionada.nomeUsuarioContatoEmpresa,
                        dadosEmpresaSelecionada.eMail1_Empresa, dadosEmpresaSelecionada.eMail2_Empresa, dadosEmpresaSelecionada.eMaiL1_UsuarioContatoEmpresa,
                        dadosEmpresaSelecionada.eMaiL2_UsuarioContatoEmpresa, nomeCentralCompras, dataLimiteAcatamentoDoPedido, codControlePedido);

                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO SMS´s
                    //---------------------------------------------------------------------------------------------
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    smsMensagem = "ClienteMercado - Você recebeu um PEDIDO. Acesse www.clientemercado.com.br e confirme o ATENDIMENTO";

                    if (emailAvisoDePedido)
                    {
                        if (!string.IsNullOrEmpty(dadosEmpresaSelecionada.celular1_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 1 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresaSelecionada.celular1_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms =
                                "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 2096 ACIMA...

                            if (smsUsuarioVendedor)
                            {
                                //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                NControleSMSService negociosSMS = new NControleSMSService();
                                controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaSelecionada.celular1_UsuarioContatoEmpresa;
                                controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaSelecionada.idUsuarioContatoResponsavel;
                                controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)
                                controleEnvioSms.DATA_ENVIO = DateTime.Now;

                                negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            }
                        }

                        if (!string.IsNullOrEmpty(dadosEmpresaSelecionada.celular2_UsuarioContatoEmpresa))
                        {
                            //TELEFONE 2 do USUÁRIO ADM
                            telefoneUsuarioADM = Regex.Replace(dadosEmpresaSelecionada.celular2_UsuarioContatoEmpresa, "[()-]", "");
                            urlParceiroEnvioSms =
                                "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                            //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                            bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 2120 ACIMA...

                            if (smsUsuarioVendedor)
                            {
                                //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                NControleSMSService negociosSMS = new NControleSMSService();
                                controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaSelecionada.celular2_UsuarioContatoEmpresa;
                                controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaSelecionada.idUsuarioContatoResponsavel;
                                controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)

                                negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                            }
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

        //CANCELAR PEDIDO do ITEM ao FORNECEDOR
        public JsonResult CancelarOPedidoDoItemAoFornecedor(int cCC, int iCM, int iCCF, string codsProdutosNegociacao, string codsItensIndividuais, 
            string somaItensDoPedido, int idPedido, string motivoCancelamento)
        {
            try
            {
                bool itemPedidoExcluido = false;
                string idsPedidos = "";
                string todosItensPedidos = "nao";
                string textoMsgStatus = "PEDIDO FEITO&nbsp;";
                string telefone1EmpresaADM = "";
                string telefone2EmpresaADM = "";
                string telefone1UsuarioADM = "";
                string telefone2UsuarioADM = "";
                var usuarioAdmCC = "";
                var email1_EmpresaAdmCC = "";
                var email2_EmpresaAdmCC = "";
                var email1_UsuarioContatoAdmCC = "";
                var email2_UsuarioContatoAdmCC = "";
                var usuarioFornAInformar = "";
                var email1_EmpresaForn = "";
                var email2_EmpresaForn = "";
                var email1_UsuarioContatoForn = "";
                var email2_UsuarioContatoForn = "";
                var fone1UsuarioContatoForn = "";
                var fone2UsuarioContatoForn = "";
                var dataEnvioPedido = "";
                var numeroPedido = "";
                string smsMensagem = "";
                string urlParceiroEnvioSms = "";
                var dataEntregaPedido = "";

                var resultado = new { itemExcluido = "nao", todosItensPedidos = "", mensagemStatus = "" };

                string[] listaIDsItensNegociacaoPedidosAExcluir, listaItensIndividuaisCotacao;
                listaIDsItensNegociacaoPedidosAExcluir = codsProdutosNegociacao.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                listaItensIndividuaisCotacao = codsItensIndividuais.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                somaItensDoPedido = somaItensDoPedido.Replace('.', ',');

                NCentralDeComprasService serviceCentralCompras = new NCentralDeComprasService();
                NPedidoCentralComprasService negociosPedidosCC = new NPedidoCentralComprasService();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoindividualEmpresaCC = 
                    new NItensCotacaoIndividualEmpresaCentralComprasService();
                NItensPedidoCentralComprasService negociosItensPedidoCC = new NItensPedidoCentralComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
                NItensCotacaoFilhaNegociacaoCentralDeComprasService negociosItensCotacaoFilhaCentralCompras = new NItensCotacaoFilhaNegociacaoCentralDeComprasService();
                NEmpresaUsuarioService empresaUsuarioService = new NEmpresaUsuarioService();
                pedido_central_compras dadosPedidoCC = new pedido_central_compras();
                itens_cotacao_individual_empresa_central_compras itemPedidoDetalhes = new itens_cotacao_individual_empresa_central_compras();
                ItemASerCanceladoOPedidoDetalhesViewModel dadosDoItemCancelado = new ItemASerCanceladoOPedidoDetalhesViewModel();

                //EXCLUIR o ITEM do PEDIDO / ATUALIZAR VALOR do PEDIDO
                for (int i = 0; i < listaIDsItensNegociacaoPedidosAExcluir.Length; i++)
                {
                    //EXCLUIR o ITEM do PEDIDO
                    itemPedidoExcluido =
                        negociosItensPedidoCC.ExcluirItemDoPedido(Convert.ToInt32(listaIDsItensNegociacaoPedidosAExcluir[i]), idPedido);

                    if (itemPedidoExcluido)
                    {
                        //BUSCAR DADOS do PEDIDO
                        dadosPedidoCC = negociosPedidosCC.ConsultarDadosDoPedidoPeloCodigo(idPedido);

                        dataEnvioPedido = dadosPedidoCC.DATA_PEDIDO_CENTRAL_COMPRAS.ToString();

                        //ATUALIZAR o VALOR TOTAL REGISTRADO para o PEDIDO
                        dadosPedidoCC.VALOR_PEDIDO_CENTRAL_COMPRAS = (dadosPedidoCC.VALOR_PEDIDO_CENTRAL_COMPRAS - Convert.ToDecimal(somaItensDoPedido));
                        negociosPedidosCC.AtualizarValorDoPedido(dadosPedidoCC);

                        resultado = new { itemExcluido = "sim", todosItensPedidos = "", mensagemStatus = "" };

                        //DESFAZER SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO
                        itemPedidoDetalhes = 
                            negociosItensCotacaoindividualEmpresaCC.DesfazimentoDeItemComoPedido(Convert.ToInt32(listaItensIndividuaisCotacao[i]), idPedido);

                        dadosDoItemCancelado = CarregarDetalhesDoItem(itemPedidoDetalhes);
                    }
                }

                //CONSULTAR DADOS da COTAÇÃO FILHA
                cotacao_filha_central_compras dadosCotacaoFilha = negociosCotacaoFilhaCC.ConsultarDadosDaCotacaoFilhaCC(iCM, iCCF);

                //===========================================================================
                //DISPARAR AO FORNECEDOR, AVISO SOBRE O CANCELAMENTO DO PEDIDO
                //---------------------------------------------------------------------------------------------
                //ENVIANDO E-MAILS
                //---------------------------------------------------------------------------------------------

                //CARREGAR DADOS da CENTRAL de COMPRAS
                central_de_compras dadosCC = serviceCentralCompras.CarregarDadosDaCentralDeCompras(cCC);

                //CARREGAR DADOS do COMPRADOR
                ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel dadosEmpresaCompradora =
                    empresaUsuarioService.BuscarDadosDaEmpresaParaEnvioDeMensagens(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                //CARREGAR DADOS do FORNECEDOR
                ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel dadosEmpresaFornecedora = 
                    empresaUsuarioService.BuscarDadosDaEmpresaParaEnvioDeMensagens(dadosCotacaoFilha.ID_CODIGO_EMPRESA);

                EnviarEmailSobreCancelamentoDoPedidoAoFornecedor enviarEmailSobreOCancelamentoDoPedido = 
                    new EnviarEmailSobreCancelamentoDoPedidoAoFornecedor();

                if (!string.IsNullOrEmpty(dadosEmpresaCompradora.nickNameUsuarioContatoEmpresa))
                {
                    usuarioAdmCC = dadosEmpresaCompradora.nickNameUsuarioContatoEmpresa;
                    email1_EmpresaAdmCC = dadosEmpresaCompradora.eMail1_Empresa;
                    email2_EmpresaAdmCC = dadosEmpresaCompradora.eMail2_Empresa;
                    email1_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL1_UsuarioContatoEmpresa;
                    email2_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL2_UsuarioContatoEmpresa;
                    telefone1EmpresaADM = dadosEmpresaCompradora.fone1_Empresa;
                    telefone2EmpresaADM = dadosEmpresaCompradora.fone2_Empresa;
                    telefone1UsuarioADM = dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa;
                    telefone2UsuarioADM = dadosEmpresaCompradora.celular2_UsuarioContatoEmpresa;
                }
                else
                {
                    usuarioAdmCC = dadosEmpresaCompradora.nomeUsuarioContatoEmpresa;
                    email1_EmpresaAdmCC = dadosEmpresaCompradora.eMail1_Empresa;
                    email2_EmpresaAdmCC = dadosEmpresaCompradora.eMail2_Empresa;
                    email1_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL1_UsuarioContatoEmpresa;
                    email2_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL2_UsuarioContatoEmpresa;
                    telefone1EmpresaADM = dadosEmpresaCompradora.fone1_Empresa;
                    telefone2EmpresaADM = dadosEmpresaCompradora.fone2_Empresa;
                    telefone1UsuarioADM = dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa;
                    telefone2UsuarioADM = dadosEmpresaCompradora.celular2_UsuarioContatoEmpresa;
                }

                //DISPARA E-MAIL´s para a Empresa ADM da CENTRAL de COMPRAS
                if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.nickNameUsuarioContatoEmpresa))
                {
                    usuarioFornAInformar = dadosEmpresaFornecedora.nickNameUsuarioContatoEmpresa;
                    email1_EmpresaForn = dadosEmpresaFornecedora.eMail1_Empresa;
                    email2_EmpresaForn = dadosEmpresaFornecedora.eMail2_Empresa;
                    email1_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL1_UsuarioContatoEmpresa;
                    email2_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL2_UsuarioContatoEmpresa;
                    fone1UsuarioContatoForn = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                    fone2UsuarioContatoForn = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                }
                else
                {
                    usuarioFornAInformar = dadosEmpresaFornecedora.nomeUsuarioContatoEmpresa;
                    email1_EmpresaForn = dadosEmpresaFornecedora.eMail1_Empresa;
                    email2_EmpresaForn = dadosEmpresaFornecedora.eMail2_Empresa;
                    email1_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL1_UsuarioContatoEmpresa;
                    email2_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL2_UsuarioContatoEmpresa;
                    fone1UsuarioContatoForn = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                    fone2UsuarioContatoForn = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                }

                if (motivoCancelamento == "")
                    motivoCancelamento = "Não informado";

                //ENVIAR E-MAIL
                bool emailAvisoCancelamentoDePedido = enviarEmailSobreOCancelamentoDoPedido.EnviarEmail(dadosCC.NOME_CENTRAL_COMPRAS, usuarioAdmCC,
                    telefone1EmpresaADM, telefone2EmpresaADM, telefone1UsuarioADM, telefone2UsuarioADM, email1_EmpresaAdmCC, email2_EmpresaAdmCC,
                    email1_UsuarioContatoAdmCC, dataEnvioPedido, numeroPedido, motivoCancelamento, dadosEmpresaFornecedora.nomeEmpresa,
                    usuarioFornAInformar, email1_EmpresaForn, email2_EmpresaForn, email1_UsuarioContatoForn, email2_UsuarioContatoForn, 
                    dadosPedidoCC.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS, dadosDoItemCancelado);

                //---------------------------------------------------------------------------------------------
                //ENVIANDO SMS´s
                //---------------------------------------------------------------------------------------------
                EnviarSms enviarSmsMaster = new EnviarSms();

                smsMensagem = "ClienteMercado - O PEDIDO 00010 foi CANCELADO pela C. Compras. Verifique em www.clientemercado.com.br";

                if (emailAvisoCancelamentoDePedido)
                {
                    if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa))
                    {
                        //TELEFONE 1 do USUÁRIO FORNECEDOR
                        fone1UsuarioContatoForn = Regex.Replace(dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa, "[()-]", "").Replace(" ", "");
                        urlParceiroEnvioSms =
                            "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + fone1UsuarioContatoForn;

                        //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, 0);

                        bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 900 ACIMA...

                        if (smsUsuarioVendedor)
                        {
                            //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            NControleSMSService negociosSMS = new NControleSMSService();
                            controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                            controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaFornecedora.idUsuarioContatoResponsavel;
                            controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)
                            controleEnvioSms.DATA_ENVIO = DateTime.Now;

                            negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                        }
                    }

                    if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa))
                    {
                        //TELEFONE 2 do USUÁRIO FORNECEDOR
                        fone2UsuarioContatoForn = Regex.Replace(dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa, "[()-]", "");
                        urlParceiroEnvioSms =
                            "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + fone2UsuarioContatoForn;

                        //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                        bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 926 ACIMA...

                        if (smsUsuarioVendedor)
                        {
                            //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            NControleSMSService negociosSMS = new NControleSMSService();
                            controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                            controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaFornecedora.idUsuarioContatoResponsavel;
                            controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)

                            negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                        }
                    }
                }

                ////---------------------------------------------------------------------------------------------
                ////ENVIANDO NOTIFICAÇÃO CELULAR
                ////---------------------------------------------------------------------------------------------
                ////3 - Enviar ALERT ao aplicativo no celular
                ///*
                //    CODIFICAR...
                //*/
                //    //===========================================================================

                //CONSULTAR LISTA de ITENS da COTACAO FILHA
                List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacaoFilha =
                    negociosItensCotacaoFilhaCentralCompras.ConsultarItensDaCotacaoDaCC(iCCF);

                //VERIFICAR TODOS os PEDIDOS para ESTA COTAÇÃO
                List<pedido_central_compras> listaDePedidosParaACotacao = negociosPedidosCC.BuscarTodosOsPedidosParaACotacao(iCM);

                for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                {
                    if (idsPedidos == "")
                    {
                        idsPedidos = listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString();
                    }
                    else
                    {
                        idsPedidos = (idsPedidos + "," + listaDePedidosParaACotacao[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS.ToString());
                    }
                }

                //BUSCAR LISTA de ITENS PEDIDOS
                List<itens_pedido_central_compras> listaDeItensPedidos = new List<itens_pedido_central_compras>();

                if (idsPedidos != "")
                {
                    listaDeItensPedidos = negociosItensPedidoCC.ConsultarListaDeItensJahPedidos(idsPedidos);

                    if (listaDeItensPedidos.Count == 0)
                    {
                        //EXCLUIR o PEDIDO
                        negociosPedidosCC.ExcluirOPedido(idsPedidos);

                        //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
                        negociosCotacaoFilhaCC.SetarComoNaoReceBimentoDePedidoParaACotacao(iCM, iCCF);
                    }
                }

                if (listaDeItensPedidos.Count == listaDeItensDaCotacaoFilha.Count)
                {
                    todosItensPedidos = "sim";
                }
                else
                {
                    textoMsgStatus = "PEDIDO PARCIAL FEITO&nbsp;";
                }

                //-------------------------------------------------------------------------
                var mensagemDoStatus = "";

                //ATUALIZAR STATUS
                if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                {
                    if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                    {
                        mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- FOI ACEITA PELO FORNECEDOR";
                    }
                    else
                    {
                        mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- AGUARDANDO RESPOSTA FORNECEDOR";
                    }
                }

                if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == false))
                {
                    mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - AGUARDANDO CONFIRMAÇÃO FORNECEDOR";
                }
                else if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == true))
                {
                    List<pedido_central_compras> listaDePedidosParaACotacaoCC = negociosPedidosCC.BuscarTodosOsPedidosParaBaixaNestaACotacao(iCM);

                    bool todosOsPedidosRecebidos = true;

                    for (int i = 0; i < listaDePedidosParaACotacao.Count; i++)
                    {
                        if (listaDePedidosParaACotacao[i].PEDIDO_ENTREGUE_FINALIZADO == false)
                        {
                            todosOsPedidosRecebidos = false;
                        }

                        dataEntregaPedido = (listaDePedidosParaACotacao[i].PEDIDO_ENTREGUE_FINALIZADO == true) ? listaDePedidosParaACotacao[i].DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS.ToShortDateString() : "";
                    }

                    if (todosOsPedidosRecebidos == true)
                    {
                        mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - PEDIDO RECEBIDO em " + dataEntregaPedido;
                    }
                    else
                    {
                        mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - CONFIRMADO PELO FORNECEDOR / AGUARDANDO ENTREGA";
                    }
                }
                //-------------------------------------------------------------------------

                resultado = new
                {
                    itemExcluido = "sim",
                    todosItensPedidos = todosItensPedidos,
                    mensagemStatus = mensagemDoStatus
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CARREGA OS DETALHES DO ITEM DE PEDIDO A SER CANCELADO
        private ItemASerCanceladoOPedidoDetalhesViewModel CarregarDetalhesDoItem(itens_cotacao_individual_empresa_central_compras itemPedidoDetalhes)
        {
            NProdutosServicosEmpresaProfissionalService serviceProdutos = new NProdutosServicosEmpresaProfissionalService();
            NEmpresasFabricantesMarcasService serviceMarca = new NEmpresasFabricantesMarcasService();
            NUnidadesProdutosService serviceUnidade = new NUnidadesProdutosService();
            NEmpresasProdutosEmbalagensService serviceEmbalagem = new NEmpresasProdutosEmbalagensService();
            ItemASerCanceladoOPedidoDetalhesViewModel dadosDoItem = new ItemASerCanceladoOPedidoDetalhesViewModel();

            produtos_servicos_empresa_profissional dadosProduto = 
                serviceProdutos.ConsultarDadosDoProdutoDaCotacao(itemPedidoDetalhes.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);
            unidades_produtos dadosUnidade = serviceUnidade.ConsultarDadosDaUnidadeDoProduto(itemPedidoDetalhes.ID_CODIGO_UNIDADE_PRODUTO);
            empresas_produtos_embalagens dadosEmbalagem = serviceEmbalagem.ConsultarDadosDaEmbalagemDoProduto(itemPedidoDetalhes.ID_EMPRESAS_PRODUTOS_EMBALAGENS);

            dadosDoItem.descricaoItem = dadosProduto.DESCRICAO_PRODUTO_SERVICO;
            dadosDoItem.marca = serviceMarca.ConsultarDescricaoDaEmpresaFabricanteOuMarca(itemPedidoDetalhes.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);
            dadosDoItem.quantidadeItem = itemPedidoDetalhes.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS.ToString();
            dadosDoItem.unidadeItem = dadosUnidade.DESCRICAO_UNIDADE_PRODUTO;
            dadosDoItem.embalagemItem = dadosEmbalagem.DESCRICAO_PRODUTO_EMBALAGEM;

            return dadosDoItem;
        }

        //CANCELAR TODO o PEDIDO ENVIADO ao FORNECEDOR
        public JsonResult CancelarOPedidoEnviadoAoFornecedor(int cCC, int iCM, int iCCF, int idPedido, string aceitouCP, string motivoCancelamento)
        {
            try
            {
                bool pedidoExcluido = false;
                bool itensPedidoExcluidos = false;
                string textoMsgStatus = "PEDIDO FEITO&nbsp;";

                string telefone1EmpresaADM = "";
                string telefone2EmpresaADM = "";
                string telefone1UsuarioADM = "";
                string telefone2UsuarioADM = "";
                var usuarioAdmCC = "";
                var email1_EmpresaAdmCC = "";
                var email2_EmpresaAdmCC = "";
                var email1_UsuarioContatoAdmCC = "";
                var email2_UsuarioContatoAdmCC = "";
                var usuarioFornAInformar = "";
                var email1_EmpresaForn = "";
                var email2_EmpresaForn = "";
                var email1_UsuarioContatoForn = "";
                var email2_UsuarioContatoForn = "";
                var fone1UsuarioContatoForn = "";
                var fone2UsuarioContatoForn = "";
                var dataEnvioPedido = "";
                var numeroPedido = "";
                string smsMensagem = "";
                string urlParceiroEnvioSms = "";
                var dataEntregaPedido = "";

                var resultado = new { pedidoExcluido = "nao", mensagemStatus = "" };

                NCentralDeComprasService serviceCentralCompras = new NCentralDeComprasService();
                NItensCotacaoIndividualEmpresaCentralComprasService negociosItensCotacaoindividualEmpresaCC = 
                    new NItensCotacaoIndividualEmpresaCentralComprasService();
                NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
                NPedidoCentralComprasService negociosPedidosCC = new NPedidoCentralComprasService();
                NItensPedidoCentralComprasService negociosItensPedidoCC = new NItensPedidoCentralComprasService();
                NCotacaoMasterCentralDeComprasService negociosCotacaoMasterCC = new NCotacaoMasterCentralDeComprasService();
                NEmpresaUsuarioService empresaUsuarioService = new NEmpresaUsuarioService();
                ItemASerCanceladoOPedidoDetalhesViewModel itemPedidoDetalhes = new ItemASerCanceladoOPedidoDetalhesViewModel();

                //CARREGAR DADOS do PEDIDO
                pedido_central_compras dadosDoPedido = negociosPedidosCC.ConsultarDadosDoPedidoPeloCodigo(idPedido);
                dataEnvioPedido = dadosDoPedido.DATA_PEDIDO_CENTRAL_COMPRAS.ToString();

                if (dadosDoPedido != null)
                {
                    //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
                    negociosCotacaoFilhaCC.SetarComoNaoReceBimentoDePedidoParaACotacao(iCM, iCCF);

                    //SETAR NULL no CAMPO relacionado ao ID do PEDIDO
                    negociosCotacaoMasterCC.SetarNullNoIdDoPedidoNaCotacaoMaster(iCM);

                    //DESFAZER SETAR TODOS os PRODUTOS da COTAÇÃO INDIVIDUAL como PEDIDO
                    negociosItensCotacaoindividualEmpresaCC.DesfazimentoDeTodosOsItensComoPedido(idPedido);

                    //EXCLUIR TODOS os ITENS do PEDIDO
                    itensPedidoExcluidos = negociosItensPedidoCC.ExcluirTodosOsItensDoPedido(idPedido);

                    if (itensPedidoExcluidos)
                    {
                        //EXCLUIR o PEDIDO
                        pedidoExcluido = negociosPedidosCC.ExcluirOPedido(idPedido.ToString());

                        if (pedidoExcluido)
                        {
                            //CONSULTAR DADOS da COTAÇÃO FILHA
                            cotacao_filha_central_compras dadosCotacaoFilha = negociosCotacaoFilhaCC.ConsultarDadosDaCotacaoFilhaCC(iCM, iCCF);

                            //===========================================================================
                            //DISPARAR AO FORNECEDOR, AVISO SOBRE O CANCELAMENTO DO PEDIDO
                            //---------------------------------------------------------------------------------------------
                            //ENVIANDO E-MAILS
                            //---------------------------------------------------------------------------------------------

                            //CARREGAR DADOS da CENTRAL de COMPRAS
                            central_de_compras dadosCC = serviceCentralCompras.CarregarDadosDaCentralDeCompras(cCC);

                            //CARREGAR DADOS do COMPRADOR
                            ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel dadosEmpresaCompradora =
                                empresaUsuarioService.BuscarDadosDaEmpresaParaEnvioDeMensagens(Convert.ToInt32(Sessao.IdEmpresaUsuario));

                            //CARREGAR DADOS do FORNECEDOR
                            ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel dadosEmpresaFornecedora =
                                empresaUsuarioService.BuscarDadosDaEmpresaParaEnvioDeMensagens(dadosCotacaoFilha.ID_CODIGO_EMPRESA);

                            EnviarEmailSobreCancelamentoDoPedidoAoFornecedor enviarEmailSobreOCancelamentoDoPedido =
                                new EnviarEmailSobreCancelamentoDoPedidoAoFornecedor();

                            if (!string.IsNullOrEmpty(dadosEmpresaCompradora.nickNameUsuarioContatoEmpresa))
                            {
                                usuarioAdmCC = dadosEmpresaCompradora.nickNameUsuarioContatoEmpresa;
                                email1_EmpresaAdmCC = dadosEmpresaCompradora.eMail1_Empresa;
                                email2_EmpresaAdmCC = dadosEmpresaCompradora.eMail2_Empresa;
                                email1_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL1_UsuarioContatoEmpresa;
                                email2_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL2_UsuarioContatoEmpresa;
                                telefone1EmpresaADM = dadosEmpresaCompradora.fone1_Empresa;
                                telefone2EmpresaADM = dadosEmpresaCompradora.fone2_Empresa;
                                telefone1UsuarioADM = dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa;
                                telefone2UsuarioADM = dadosEmpresaCompradora.celular2_UsuarioContatoEmpresa;
                            }
                            else
                            {
                                usuarioAdmCC = dadosEmpresaCompradora.nomeUsuarioContatoEmpresa;
                                email1_EmpresaAdmCC = dadosEmpresaCompradora.eMail1_Empresa;
                                email2_EmpresaAdmCC = dadosEmpresaCompradora.eMail2_Empresa;
                                email1_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL1_UsuarioContatoEmpresa;
                                email2_UsuarioContatoAdmCC = dadosEmpresaCompradora.eMaiL2_UsuarioContatoEmpresa;
                                telefone1EmpresaADM = dadosEmpresaCompradora.fone1_Empresa;
                                telefone2EmpresaADM = dadosEmpresaCompradora.fone2_Empresa;
                                telefone1UsuarioADM = dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa;
                                telefone2UsuarioADM = dadosEmpresaCompradora.celular2_UsuarioContatoEmpresa;
                            }

                            //DISPARA E-MAIL´s para a Empresa ADM da CENTRAL de COMPRAS
                            if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.nickNameUsuarioContatoEmpresa))
                            {
                                usuarioFornAInformar = dadosEmpresaFornecedora.nickNameUsuarioContatoEmpresa;
                                email1_EmpresaForn = dadosEmpresaFornecedora.eMail1_Empresa;
                                email2_EmpresaForn = dadosEmpresaFornecedora.eMail2_Empresa;
                                email1_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL1_UsuarioContatoEmpresa;
                                email2_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL2_UsuarioContatoEmpresa;
                                fone1UsuarioContatoForn = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                                fone2UsuarioContatoForn = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                            }
                            else
                            {
                                usuarioFornAInformar = dadosEmpresaFornecedora.nomeUsuarioContatoEmpresa;
                                email1_EmpresaForn = dadosEmpresaFornecedora.eMail1_Empresa;
                                email2_EmpresaForn = dadosEmpresaFornecedora.eMail2_Empresa;
                                email1_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL1_UsuarioContatoEmpresa;
                                email2_UsuarioContatoForn = dadosEmpresaFornecedora.eMaiL2_UsuarioContatoEmpresa;
                                fone1UsuarioContatoForn = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                                fone2UsuarioContatoForn = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                            }

                            if (motivoCancelamento == "")
                                motivoCancelamento = "Não informado";

                            //ENVIAR E-MAIL
                            bool emailAvisoCancelamentoDePedido = enviarEmailSobreOCancelamentoDoPedido.EnviarEmail(dadosCC.NOME_CENTRAL_COMPRAS, usuarioAdmCC,
                                telefone1EmpresaADM, telefone2EmpresaADM, telefone1UsuarioADM, telefone2UsuarioADM, email1_EmpresaAdmCC, email2_EmpresaAdmCC,
                                email1_UsuarioContatoAdmCC, dataEnvioPedido, numeroPedido, motivoCancelamento, dadosEmpresaFornecedora.nomeEmpresa,
                                usuarioFornAInformar, email1_EmpresaForn, email2_EmpresaForn, email1_UsuarioContatoForn, email2_UsuarioContatoForn, 
                                dadosDoPedido.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS, itemPedidoDetalhes);

                            //---------------------------------------------------------------------------------------------
                            //ENVIANDO SMS´s
                            //---------------------------------------------------------------------------------------------
                            EnviarSms enviarSmsMaster = new EnviarSms();

                            smsMensagem = "ClienteMercado - O PEDIDO 00010 foi CANCELADO pela C. Compras. Verifique em www.clientemercado.com.br";

                            if (emailAvisoCancelamentoDePedido)
                            {
                                if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa))
                                {
                                    //TELEFONE 1 do USUÁRIO FORNECEDOR
                                    fone1UsuarioContatoForn = Regex.Replace(dadosEmpresaCompradora.celular1_UsuarioContatoEmpresa, "[()-]", "").Replace(" ", "");
                                    urlParceiroEnvioSms =
                                        "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + fone1UsuarioContatoForn;

                                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, 0);

                                    bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 900 ACIMA...

                                    if (smsUsuarioVendedor)
                                    {
                                        //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                        NControleSMSService negociosSMS = new NControleSMSService();
                                        controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                        controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaFornecedora.celular1_UsuarioContatoEmpresa;
                                        controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaFornecedora.idUsuarioContatoResponsavel;
                                        controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)
                                        controleEnvioSms.DATA_ENVIO = DateTime.Now;

                                        negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    }
                                }

                                if (!string.IsNullOrEmpty(dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa))
                                {
                                    //TELEFONE 2 do USUÁRIO FORNECEDOR
                                    fone2UsuarioContatoForn = Regex.Replace(dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa, "[()-]", "");
                                    urlParceiroEnvioSms =
                                        "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + fone2UsuarioContatoForn;

                                    //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                                    bool smsUsuarioVendedor = true; //ACESSAR 'http://paineldeenvios.com/', COLOCAR SALDO E DESCOMENTAR LINHA 926 ACIMA...

                                    if (smsUsuarioVendedor)
                                    {
                                        //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                                        NControleSMSService negociosSMS = new NControleSMSService();
                                        controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                                        controleEnvioSms.TELEFONE_DESTINO = dadosEmpresaFornecedora.celular2_UsuarioContatoEmpresa;
                                        controleEnvioSms.ID_CODIGO_USUARIO = dadosEmpresaFornecedora.idUsuarioContatoResponsavel;
                                        controleEnvioSms.MOTIVO_ENVIO = 4; //Valor default. 4 - Envio de AViso de PEDIDO (ver ual valor vai entrar no lugar do 4) (Criar uma tabela com esses valores para referência/leitura)

                                        negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                                    }
                                }
                            }

                            //---------------------------------------------------------------------------------------------
                            //ENVIANDO NOTIFICAÇÃO CELULAR
                            //---------------------------------------------------------------------------------------------
                            //3 - Enviar ALERT ao aplicativo no celular
                            /*
                                CODIFICAR...
                            */

                            var mensagemDoStatus = "";

                            //ATUALIZAR STATUS
                            if (dadosCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                            {
                                if (dadosCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                                {
                                    mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- FOI ACEITA PELO FORNECEDOR";
                                }
                                else
                                {
                                    mensagemDoStatus = "<font color='#3297E0'>CONTRA PROPOSTA ENVIADA</font>&nbsp;- AGUARDANDO RESPOSTA FORNECEDOR";
                                }
                            }

                            if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == false))
                            {
                                mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - AGUARDANDO CONFIRMAÇÃO FORNECEDOR";
                            }
                            else if ((dadosCotacaoFilha.RECEBEU_PEDIDO == true) && (dadosCotacaoFilha.CONFIRMOU_PEDIDO == true))
                            {
                                if (dadosDoPedido.PEDIDO_ENTREGUE_FINALIZADO == true)
                                {
                                    dataEntregaPedido = (dadosDoPedido.PEDIDO_ENTREGUE_FINALIZADO == true) ? dadosDoPedido.DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS.ToShortDateString() : "";
                                    mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - PEDIDO RECEBIDO em " + dataEntregaPedido;
                                }
                                else
                                {
                                    mensagemDoStatus = "<font color='#3297E0'>" + textoMsgStatus + "</font> - CONFIRMADO PELO FORNECEDOR / AGUARDANDO ENTREGA";
                                }
                            }

                            resultado = new { pedidoExcluido = "sim", mensagemStatus = mensagemDoStatus };

                        }
                    }
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GERAR NOVO CODIGO de CONTROLE do PEDIDO
        public string GerarCodigoControleDoPedido(int cCC)
        {
            try
            {
                NPedidoCentralComprasService servicePedidos = new NPedidoCentralComprasService();

                string novoCodControlePedido = servicePedidos.GerarCodigoControleDoPedido(cCC);

                return novoCodControlePedido;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //EFETUAR A BAIXA DO PEDIDO COMO ENTREGUE
        public JsonResult EfetuarBaixaDoPedidoEntregue(int cCC, string eA, int iCM, int iCCF, int idPedidoABaixar, string dataEntrega)
        {
            try
            {
                var resultado = new { pedidoBaixado = "nao", numeroPedido = "" };

                NPedidoCentralComprasService servicePedido = new NPedidoCentralComprasService();
                NItensPedidoCentralComprasService serviceItensPedido = new NItensPedidoCentralComprasService();

                //BAIXAR ITEM(s) DO PEDIDO / PEDIDO
                List<itens_pedido_central_compras> listaItensPedidoBaixados = serviceItensPedido.InformarRecebimentoDoItemDoPedido(idPedidoABaixar, dataEntrega);
                pedido_central_compras dadosDoPedido = servicePedido.DarBaixaNoPedido(idPedidoABaixar);

                resultado = new { pedidoBaixado = "sim", numeroPedido = dadosDoPedido.COD_CONTROLE_PEDIDO_CENTRAL_COMPRAS };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
