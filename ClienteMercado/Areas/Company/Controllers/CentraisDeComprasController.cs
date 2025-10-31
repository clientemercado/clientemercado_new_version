using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Sms;
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
    public class CentraisDeComprasController : Controller
    {
        //
        // GET: /Company/CentraisDeCompras/

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
                    viewModelCC.inListaDeUFs = ListagemEstados();

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Centrais de Compras Sistema";

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

        public ActionResult CotacoesDaCentralDeComprasSistema(int cCC)
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
                    //viewModelCC.inListaUnidadesProdutosACotar = ListagemUnidadesDePesoEMedida();
                    viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();

                    NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                    //BUSCAR LISTA de EMPRESAS da CENTRAL de COMPRAS
                    List<empresas_participantes_central_de_compras> listaDeEmpresasParticipantesDaCC = negociosEmpresasParticipantesCC.BuscarListaDeEmpresasParticipantesDaCC(cCC);

                    ////VERIFICAR se a EMPRESA em questâo já ESTÁ ATIVA na CENTRAL de COMPRAS
                    //bool empresaAtivaCC = negociosEmpresasParticipantesCC.VerificarSeEmpresaParticipanteAceitouConviteParticipacaoCC(cCC);

                    //if (empresaAtivaCC)
                    //{
                    //    viewModelCC.inConviteAceitoCC = "sim";
                    //}
                    //else
                    //{
                    //    viewModelCC.inConviteAceitoCC = "nao";
                    //}

                    //VIEWBAGS
                    ViewBag.quantasEmpresas = (listaDeEmpresasParticipantesDaCC.Count - 2);
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Centrais de Compras Sistema > Cotações da Central Sistema";

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

        //BUSCAR POSSÍVEIS PARCEIROS para compor a CENTRAL de COMPRAS
        public ActionResult BuscarListaDeCentraisDeComprasDoSistema(string descricaoFiltro, int idGrupoAtividadesFiltro, string cidadeDaCC, string estadoCC)
        {
            try
            {
                var textoStatusCC = "";

                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();
                NCentralDeComprasService negociosCentraisDeCompras = new NCentralDeComprasService();
                NEmpresasSolicitacaoParticipacaoCentralDeComprasService negociosSolicitacaoParticipacaoCC =
                    new NEmpresasSolicitacaoParticipacaoCentralDeComprasService();

                List<ListaCentraisComprasViewModel> listaCentraisDeComprasDoSistema = negociosCentraisDeCompras.BuscarListaDeCentraisDeComprasDoSistema();

                //BUSCAR MAIS INFORMAÇÕES para popular alguns campos
                for (int i = 0; i < listaCentraisDeComprasDoSistema.Count; i++)
                {
                    var textoStatusConvite = "";

                    //QUANTIDADE de EMPRESAS
                    int quantidadeDeEmpresasCC =
                         negociosEmpresasParticipantesCC.BuscarQuantidadeDeEmpresasParticipantes(listaCentraisDeComprasDoSistema[i].ID_CENTRAL_COMPRAS);

                    listaCentraisDeComprasDoSistema[i].quantasEmpresas = quantidadeDeEmpresasCC;

                    //VERIFICAR STATUS da CENTRAL de COMPRAS
                    if (DateTime.Now <= listaCentraisDeComprasDoSistema[i].DATA_ENCERRAMENTO_CENTRAL_COMPRAS)
                    {
                        textoStatusCC = "ATIVA";
                    }

                    /*
                    OBS: Definir aqui outras condições para o STATUS.
                    */

                    //VERIFICAR se a EMPRESA LOGADA possui SOLICITAÇÃO de PARTICIPAÇÂO na CENTRAL de COMPRAS
                    empresas_solicitacao_participacao_central_de_compras dadosSolicitacaoParticipacaoCC =
                        negociosSolicitacaoParticipacaoCC.BuscaSolicitacaoDeParticipacaoNaCCDaEmpresaLogada(listaCentraisDeComprasDoSistema[i].ID_CENTRAL_COMPRAS);

                    if (dadosSolicitacaoParticipacaoCC != null)
                    {
                        if ((dadosSolicitacaoParticipacaoCC.SOLICITACAO_PARTICIPACAO_ACEITO == false) && (dadosSolicitacaoParticipacaoCC.ID_CODIGO_USUARIO_ACEITOU_SOLICITACAO == null))
                        {
                            textoStatusConvite = "AGUARD. LIB.";
                        }
                    }

                    //VERIFICAR SE A EMPRESA LOGADA JÁ É PARTICIPANTE DA CENTRAL COMPRAS em QUESTÃO
                    empresas_participantes_central_de_compras participoDestaCC =
                        negociosEmpresasParticipantesCC.ConsultarSeEmpresaParticipaDaCentralDeCompras(listaCentraisDeComprasDoSistema[i].ID_CENTRAL_COMPRAS);

                    if (participoDestaCC != null)
                    {
                        textoStatusConvite = "FAÇO PARTE";
                    }
                    else
                    {
                        //VERIFICAR SE A EMPRESA LOGADA RECEBEU CONVITE DA CENTRAL COMPRAS em QUESTÃO
                        empresas_participantes_central_de_compras fuiConvidadoParaEstaCC =
                            negociosEmpresasParticipantesCC.ConsultarSeEmpresaTemConvitePendenteNestaCC(listaCentraisDeComprasDoSistema[i].ID_CENTRAL_COMPRAS);

                        if (fuiConvidadoParaEstaCC != null)
                        {
                            textoStatusConvite = "CONVIDADO";
                        }
                    }

                    listaCentraisDeComprasDoSistema[i].statusCentralCompras = textoStatusCC;
                    listaCentraisDeComprasDoSistema[i].statusConviteCentralCompras = textoStatusConvite;
                }

                //------------------------------------------------------------------
                //APLICAR FILTRO
                List<ListaCentraisComprasViewModel> listaCentraisDeComprasEmQueParticipoRetorno =
                    new List<ListaCentraisComprasViewModel>();

                if ((String.IsNullOrEmpty(descricaoFiltro) == false) || (idGrupoAtividadesFiltro > 0) || (String.IsNullOrEmpty(cidadeDaCC) == false)
                    || (String.IsNullOrEmpty(estadoCC) == false))
                {
                    if (String.IsNullOrEmpty(descricaoFiltro) == false)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasDoSistema.Where(m => (m.nomeCentralCompras.Contains(descricaoFiltro))).ToList();
                    }

                    if (idGrupoAtividadesFiltro > 0)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasDoSistema.Where(m => (m.idGrupoAtividades == idGrupoAtividadesFiltro)).ToList();
                    }

                    if (String.IsNullOrEmpty(cidadeDaCC) == false)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasDoSistema.Where(m => (m.cidadeDaCentralCompras == cidadeDaCC)).ToList();
                    }

                    if (String.IsNullOrEmpty(estadoCC) == false)
                    {
                        listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasDoSistema.Where(m => (m.ufDaCentralCompras == estadoCC)).ToList();
                    }
                }
                else
                {
                    listaCentraisDeComprasEmQueParticipoRetorno = listaCentraisDeComprasDoSistema;
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

        //CONSULTA do AUTOCOMPLETE da LISTA de CENTRAIS de COMPRAS do SISTEMA
        [WebMethod]
        public JsonResult BuscarListaDescricaoCCDoSistema(string term)
        {
            try
            {
                NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();

                //CARREGA LISTA de CENTRAIS de COMPRAS
                List<ListaCentraisComprasViewModel> listaCC = negociosCentraisCompras.CarregarListaAutoCompleteCentraisDeComprasDoSistema(term);

                return Json(listaCC, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CONSULTA do AUTOCOMPLETE da LISTA de CIDADES
        [WebMethod]
        public JsonResult BuscarListaDeCidades(string term)
        {
            try
            {
                NCidadesService negociosCidades = new NCidadesService();

                //CARREGA LISTA de CIDADES
                List<ListaDeCidadesViewModel> listaCidades = negociosCidades.CarregarListadeCidades(term);

                return Json(listaCidades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //SOLICITAR PARTICIPAÇÃO na CENTRAL COMPRAS
        public JsonResult SolicitarParticipacaoNaCC(int cCC)
        {
            try
            {
                var resultado = new { participacaoSolicitada = "nOk", nomeCentralCompras = "", usuarioLogadoSolicitante = "" };

                string nomeUsuario = "";
                string assuntoEmail = "";
                string corpoEmail = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";

                NCentralDeComprasService negociosCentraisCompras = new NCentralDeComprasService();
                NEmpresasSolicitacaoParticipacaoCentralDeComprasService negociosSolicitacaoParticipacao = new NEmpresasSolicitacaoParticipacaoCentralDeComprasService();
                empresas_solicitacao_participacao_central_de_compras dadosSolicitacao = new empresas_solicitacao_participacao_central_de_compras();
                usuario_empresa dadosUsuarioSolicitandoParticapacaoNaCC = new usuario_empresa();
                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();

                //CARREGA DADOS da CENTRAL de COMPRAS
                central_de_compras dadosCC = negociosCentraisCompras.CarregarDadosDaCentralDeCompras(cCC);

                //DADOS EMPRESA SOLICITANTE
                empresa_usuario dadosEmpresaSolicitandoParticapacaoNaCC =
                    negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = (int)Sessao.IdEmpresaUsuario });
                dadosUsuarioSolicitandoParticapacaoNaCC = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa((int)Sessao.IdUsuarioLogado);

                //POPULAR OBJ para REGISTRAR SOLICITAÇÃO de PARTICIPAÇÃO
                dadosSolicitacao.ID_CENTRAL_COMPRAS = dadosCC.ID_CENTRAL_COMPRAS;
                dadosSolicitacao.ID_CODIGO_EMPRESA_SOLICITANTE = (int)Sessao.IdEmpresaUsuario;
                dadosSolicitacao.SOLICITACAO_PARTICIPACAO_ACEITO = false;
                dadosSolicitacao.ID_CODIGO_USUARIO_ACEITOU_SOLICITACAO = null;

                //REGISTRAR SOLICITAÇÃO de PARTICIPAÇÃO na CC
                bool registrarSolicitacao = negociosSolicitacaoParticipacao.RegistrarPedidoDeParticipacaoNaCC(dadosSolicitacao);

                if (registrarSolicitacao)
                {
                    NEmpresaUsuarioLoginsService negociosEmpresaUsuarioLogins = new NEmpresaUsuarioLoginsService();

                    //DADOS EMPRESA/USUARIO ADM da CC
                    empresa_usuario dadosEmpresaADMCC =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosCC.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioADMCC = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosCC.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);
                    empresa_usuario_logins dadosContatoUsuarioADMCC = negociosEmpresaUsuarioLogins.ConsultarDadosDeContatoDoUsuario(dadosCC.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);

                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO E-MAILS
                    //---------------------------------------------------------------------------------------------
                    EnviarEmailSolicitacaoDeParticipacaoNaCC enviarEmailsSolicitacao = new EnviarEmailSolicitacaoDeParticipacaoNaCC();

                    assuntoEmail = "SOLICITAÇÃO PARTICIPAÇÃO - Central de Compras: " + dadosCC.NOME_CENTRAL_COMPRAS.ToUpper();

                    //ENVIANDO E-MAIL para o USUÁRIO ADM da CENTRAL de COMPRAS
                    if (!string.IsNullOrEmpty(dadosUsuarioADMCC.NICK_NAME_USUARIO))
                    {
                        nomeUsuario = dadosUsuarioADMCC.NICK_NAME_USUARIO;
                    }
                    else
                    {
                        nomeUsuario = dadosUsuarioADMCC.NOME_USUARIO;
                    }

                    corpoEmail = " Sr(a) " + nomeUsuario + "<br><br>" +
                                 " A empresa " + dadosEmpresaSolicitandoParticapacaoNaCC.NOME_FANTASIA_EMPRESA.ToUpper() + " está lhe solicitando PERMISSÃO para fazer parte da Central de Compras em grupo " + dadosCC.NOME_CENTRAL_COMPRAS.ToUpper() + ".<br><br>" +
                                 " Ter mais uma empresa fazendo parte desta Central de Compras lhe permitirá conseguir melhores preços junto aos FORNECEDORES, pois quanto mais empresas no grupo, maior poder de barganha vocês terão.<br>" +
                                 " Para liberar a participação, acesse www.clientemercado.com ou clique no botão abaixo, e o sistema lhe apresentará os meios de dar a permissão à EMPRESA SOLICITANTE.<br><br>" +
                                 " <BOTÃO> link PARA PÁGINA EXPLICATIVA " +
                                 "<br><br><br>Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br>";

                    bool emailSolicitacaoDeParticapacao =
                        enviarEmailsSolicitacao.EnviandoEmailSolicitandoParticipacaoNaCC(dadosEmpresaADMCC.EMAIL1_EMPRESA, dadosEmpresaADMCC.EMAIL2_EMPRESA,
                            dadosContatoUsuarioADMCC.EMAIL1_USUARIO, dadosContatoUsuarioADMCC.EMAIL2_USUARIO, assuntoEmail, corpoEmail);


                    //---------------------------------------------------------------------------------------------
                    //ENVIANDO SMS´s
                    //---------------------------------------------------------------------------------------------
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    smsMensagem = "ClienteMercado - Uma empresa deseja participar da sua CENTRAL de COMPRAS em grupo. Para saber mais, acesse www.clientemercado.com.br.";

                    if (!string.IsNullOrEmpty(dadosUsuarioADMCC.TELEFONE1_USUARIO_EMPRESA))
                    {
                        //TELEFONE 1 do USUÁRIO ADM
                        telefoneUsuarioADM = Regex.Replace(dadosUsuarioADMCC.TELEFONE1_USUARIO_EMPRESA, "[()-]", "");
                        urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                        //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                        //if (smsUsuarioVendedor)
                        //{
                        //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                        //    NControleSMSService negociosSMS = new NControleSMSService();
                        //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                        //    controleEnvioSms.TELEFONE_DESTINO = dadosUsuarioADMCC.TELEFONE1_USUARIO_EMPRESA;
                        //    controleEnvioSms.ID_CODIGO_USUARIO = dadosUsuarioADMCC.ID_CODIGO_USUARIO;
                        //    controleEnvioSms.MOTIVO_ENVIO = 5; //Valor default. 5 - Envio de SOLICITAÇÃO de PARTICIPAÇÃO na CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)
                        //    controleEnvioSms.DATA_ENVIO = DateTime.Now;

                        //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                        //}
                    }

                    if (!string.IsNullOrEmpty(dadosUsuarioADMCC.TELEFONE2_USUARIO_EMPRESA))
                    {
                        //TELEFONE 2 do USUÁRIO ADM
                        telefoneUsuarioADM = Regex.Replace(dadosUsuarioADMCC.TELEFONE2_USUARIO_EMPRESA, "[()-]", "");
                        urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneUsuarioADM;

                        //bool smsUsuarioVendedor = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneUsuarioADM));

                        //if (smsUsuarioVendedor)
                        //{
                        //    //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                        //    NControleSMSService negociosSMS = new NControleSMSService();
                        //    controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                        //    controleEnvioSms.TELEFONE_DESTINO = dadosUsuarioADMCC.TELEFONE2_USUARIO_EMPRESA;
                        //    controleEnvioSms.ID_CODIGO_USUARIO = dadosUsuarioADMCC.ID_CODIGO_USUARIO;
                        //    controleEnvioSms.MOTIVO_ENVIO = 5; //Valor default. 5 - Envio de SOLICITAÇÃO de PARTICIPAÇÃO na CENTRAL de COMPRAS (Criar uma tabela com esses valores para referência/leitura)

                        //    negociosSMS.GravarDadosSmsEnviado(controleEnvioSms);
                        //}
                    }

                    resultado = new { participacaoSolicitada = "Ok", nomeCentralCompras = dadosCC.NOME_CENTRAL_COMPRAS, usuarioLogadoSolicitante = dadosUsuarioSolicitandoParticapacaoNaCC.NOME_USUARIO };
                }
                else
                {
                    resultado = new { participacaoSolicitada = "nOk", nomeCentralCompras = dadosCC.NOME_CENTRAL_COMPRAS, usuarioLogadoSolicitante = dadosUsuarioSolicitandoParticapacaoNaCC.NOME_USUARIO };
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
