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
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class NovaCentralDeComprasController : Controller
    {
        //CARREGA a VIEW
        public ActionResult CriarCentralDeCompras()
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario != null) && (Sessao.IdEmpresaUsuario > 0))
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

                    NovaCentralDeComprasViewModel viewModelCC = new NovaCentralDeComprasViewModel();

                    empresa_usuario dadosEmpresa = new NEmpresaUsuarioService().ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresa = new NUsuarioEmpresaService().ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));
                    grupo_atividades_empresa dadosGruposAtividadesEmpresa =
                        new NGruposAtividadesEmpresaProfissionalService().ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosEmpresa.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacao = new NEnderecoEmpresaUsuarioService().ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //POPULAR VIEW MODEL
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacao[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacao[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresa.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.DESCRICAO_ATIVIDADE;
                    //viewModelCC.inListaDeRamosDeAtividade = ListaDeGruposDeAtividades();
                    viewModelCC.inListaDeRamosDeAtividadeParaComprasDaCC = ListagemDeDeRamosComercioVarejista();

                    //VIEWBAGS
                    ViewBag.codRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.ID_GRUPO_ATIVIDADES;
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nova Central de Compras";

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
                //Buscar lista de Países
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

        //CRIAR CENTRAL de COMPRAS
        [WebMethod]
        public JsonResult RegistrarCentralDeCompras(string nomeCentralDeCompras, int ramoAtividade, string[] tipoDeCotacaoDaCentral)
        {
            try
            {
                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();
                central_de_compras dadosCentralDeCompras = new central_de_compras();

                //CONSULTAR DADOS do RAMO de ATIVIDADES
                grupo_atividades_empresa dadosDoRamoDeAtividade =
                    negociosGruposDeAtividades.ConsultarDadosDoRamoDeAtividadeDaEmpesaPeloID(ramoAtividade);

                //POPULANDO OBJ para GRAVAÇÃO/CRIAÇÃO da CENTRAL de COMPRAS 
                dadosCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS = (Int32)Sessao.IdEmpresaUsuario;
                dadosCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS = (Int32)Sessao.IdUsuarioLogado;
                dadosCentralDeCompras.NOME_CENTRAL_COMPRAS = nomeCentralDeCompras;
                dadosCentralDeCompras.DATA_CRIACAO_CENTRAL_COMPRAS = DateTime.Now;
                dadosCentralDeCompras.DATA_ENCERRAMENTO_CENTRAL_COMPRAS = DateTime.Now.AddDays(730); //ACRESCENTA 02 ANOS - Obs: Ainda tenho que ver como aproveitar este campo.
                dadosCentralDeCompras.ID_GRUPO_ATIVIDADES = ramoAtividade;

                //CRIAR/GRAVAR CENTRAL de COMPRAS
                DadosDaCentralComprasViewModel dadosDaCentral = negociosCentralDeCompras.CriarCentralDeComprasNoSistema(dadosCentralDeCompras);

                if (dadosDaCentral.tipoParticipacao == "criado")
                {
                    dadosDaCentral.mensagemRetorno = "A CENTRAL de COMPRAS:\n\n" + nomeCentralDeCompras + "\n\nfoi registrada com SUCESSO!\nVocê será redicionado para a BUSCA de participantes para a CENTRAL de COMPRAS.";
                }
                else if (dadosDaCentral.tipoParticipacao == "admin")
                {
                    dadosDaCentral.mensagemRetorno = "ATENÇÃO!\n\nEmpresa já registrada como ADMINISTRADORA da Central de Compras:\n\n" + dadosDaCentral.NOME_CENTRAL_COMPRAS.ToUpper() + "\n\n Ramo de Atividade:\n " + dadosDoRamoDeAtividade.DESCRICAO_ATIVIDADE.ToUpper() + ".\nCada EMPRESA só pode ser administradora ou participar de apenas uma CENTRAL de COMPRAS para o seu ramo de atuação.";
                }
                else if (dadosDaCentral.tipoParticipacao == "partic")
                {
                    dadosDaCentral.mensagemRetorno = "ATENÇÃO!\n\nEmpresa já está registrada como PARTICIPANTE da Central de Compras:\n\n" + dadosDaCentral.NOME_CENTRAL_COMPRAS.ToUpper() + "\n\n Ramo de Atividade:\n " + dadosDoRamoDeAtividade.DESCRICAO_ATIVIDADE.ToUpper() + ".\nCada EMPRESA só pode ser administradora ou participar de apenas uma CENTRAL de COMPRAS para o seu ramo de atuação.";
                }

                dadosDaCentral.idCCCriptografado = MD5Crypt.Criptografar(dadosDaCentral.ID_CENTRAL_COMPRAS.ToString());
                dadosDaCentral.idEmpresaAdmCriptografado = MD5Crypt.Criptografar(dadosDaCentral.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS.ToString());

                return Json(dadosDaCentral, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                Trace.Write(erro.ToString());

                throw erro;
            }
        }

        //BUSCAR POSSÍVEIS PARCEIROS para compor a CENTRAL de COMPRAS
        public ActionResult BuscarParceirosParaComposicaoDaCentralDeCompras(string cCC, string eA, int cGA)
        {
            try
            {
                var resultado = new { dadosCarregados = "" };

                int codCentralCompras = Convert.ToInt32(MD5Crypt.Descriptografar(cCC));
                int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();

                //CARREGA LISTA TESTE (Obs: O método será mantido no futuro, porém a consulta será implementada considerando dados espaciais)
                List<ListaEstilizadaDeEmpresasViewModel> dadosPossiveisEmpresasParceiras =
                    negociosEmpresaUsuario.BuscarListaDePossiveisParceirosNaCentralDeCompras(codEmpresaAdm, cGA);

                return Json(
                    new
                    {
                        rows = dadosPossiveisEmpresasParceiras,
                        current = 1,
                        rowCount = dadosPossiveisEmpresasParceiras.Count,
                        total = dadosPossiveisEmpresasParceiras.Count,
                        dadosCarregados = "Ok"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //CARREGA a VIEW
        public ActionResult ConvidarEmpresasCentralDeCompras(string cCC, string eA)
        {
            try
            {
                if ((Sessao.IdEmpresaUsuario > 0) && ((cCC != null) && (cCC != "")) && ((eA != null) && (eA != "")))
                {
                    DateTime dataHoje = DateTime.Today;
                    int codCentralCompras = Convert.ToInt32(MD5Crypt.Descriptografar(cCC));
                    int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

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

                    NovaCentralDeComprasViewModel viewModelCC = new NovaCentralDeComprasViewModel();

                    //CARREGAR os DADOS da CENTRAL de COMPRAS
                    central_de_compras dadosDaCentralDeCompras = negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);

                    empresa_usuario dadosEmpresaAdm =
                        negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = dadosDaCentralDeCompras.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS });
                    usuario_empresa dadosUsuarioEmpresaAdm = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(dadosDaCentralDeCompras.ID_CODIGO_USUARIO_ADM_CENTRAL_COMPRAS);
                    grupo_atividades_empresa dadosGruposAtividadesDaCentralDeCompras =
                        negociosGrupoAtividadesEmpresa.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES });
                    List<ListaDadosDeLocalizacaoViewModel> dadosLocalizacao = negociosLocalizacao.ConsultarDadosDaLocalizacaoPeloCodigo(dadosEmpresaAdm.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);

                    //POPULAR VIEW MODEL
                    viewModelCC.inNomeCentralCompras = dadosDaCentralDeCompras.NOME_CENTRAL_COMPRAS;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresaAdm.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacao[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacao[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresaAdm.NOME_USUARIO;
                    viewModelCC.inRamoAtividadeEmpresaAdm = dadosGruposAtividadesDaCentralDeCompras.DESCRICAO_ATIVIDADE;
                    viewModelCC.inCodRamoAtividade = dadosDaCentralDeCompras.ID_GRUPO_ATIVIDADES;
                    viewModelCC.inCCCriptografado = cCC;
                    viewModelCC.ineACriptografado = eA;

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Nova Central de Compras";

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

        //ENVIAR CONVITES A POSSÍVEIS PARCEIROS para compor a CENTRAL de COMPRAS
        public ActionResult EnviarConvitesParaEmpresasSelecionadasAComporACentralDeCompras(string cCC, string eA, int[] empresasSelecionadas)
        {
            try
            {
                //TESTAR SMS - CONTINUAR AQUI...
                //OBS: Verificar se JÁ EXISTE COTAÇÃO MASTER CRIADA e se existir, enviar este AVISO tbm; (Ainda não foi feito essa parte)

                string nomeUsuario = "";
                string assuntoEmail = "";
                string corpoEmail = "";
                string smsMensagem = "";
                string telefoneUsuarioADM = "";
                string urlParceiroEnvioSms = "";

                var resultado = new { convitesEnviados = "" };

                int codCentralCompras = Convert.ToInt32(MD5Crypt.Descriptografar(cCC));
                int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

                NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();
                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesCC = new NEmpresasParticipantesCentralDeComprasService();

                empresa_usuario dadosEmpresaADM = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(codEmpresaAdm) });
                central_de_compras dadoscentralDeCompras = negociosCentralDeCompras.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);

                //CARREGAR DADOS das EMPRESAS SELECIONADAS para COMPOR a CENTRAL de COMPRAS RECÉM CRIADA
                List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> dadosEmpresasSelecionadas =
                    negociosEmpresaUsuario.BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(empresasSelecionadas);

                //---------------------------------------------------------------------------------------------
                //REGISTRAR a EMPRESA CONVIDADA na tabela empresas_participantes_central_de_compras como "AGUARDANDO RESPOSTA CONVITE"
                //---------------------------------------------------------------------------------------------
                empresas_participantes_central_de_compras empresaConvidadaCC = new empresas_participantes_central_de_compras();

                for (int a = 0; a < dadosEmpresasSelecionadas.Count; a++)
                {
                    empresaConvidadaCC.ID_CENTRAL_COMPRAS = codCentralCompras;
                    empresaConvidadaCC.ID_CODIGO_EMPRESA = dadosEmpresasSelecionadas[a].idEmpresa;
                    empresaConvidadaCC.DATA_ADESAO_EMPRESA_CENTRAL_COMPRAS = Convert.ToDateTime("1900-01-01");
                    empresaConvidadaCC.DATA_ENCERRAMENTO_PARTICIPACAO_CENTRAL_COMPRAS = Convert.ToDateTime("1900-01-01");
                    empresaConvidadaCC.DATA_CONVITE_CENTRAL_COMPRAS = DateTime.Now;

                    negociosEmpresasParticipantesCC.GravarConviteDeParticipacaoDaEmpresaNaCentralDeCompras(empresaConvidadaCC);
                }

                //---------------------------------------------------------------------------------------------
                //ENVIANDO E-MAILS
                //---------------------------------------------------------------------------------------------
                EnviarEmailConviteParcerias enviarEmailsConvites = new EnviarEmailConviteParcerias();

                assuntoEmail = "CONVITE - Central de Compras: " + dadoscentralDeCompras.NOME_CENTRAL_COMPRAS.ToUpper();

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

                    corpoEmail = " Sr(a) " + nomeUsuario + "<br><br>" +
                                 " A empresa " + dadosEmpresaADM.NOME_FANTASIA_EMPRESA + " está lhe convidando, e a " + dadosEmpresasSelecionadas.Count + " outras empresas, para fazer parte da Central de Compras em grupo " + dadoscentralDeCompras.NOME_CENTRAL_COMPRAS.ToUpper() + ".<br><br>" +
                                 " Fazer parte desta Central de Compras lhe permitirá conseguir melhores preços junto aos FORNECEDORES, tornando sua empresa mais competitiva no seu mercado de atuação.<br>" +
                                 " Para saber mais, clique no botão abaixo e conheça as vantagens que estamos lhe oferecendo:<br><br>" +
                                 " <BOTÃO> link PARA PÁGINA EXPLICATIVA " +
                                 "<br><br><br>Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br>";

                    bool emailConviteAEmpresasParceiras =
                        enviarEmailsConvites.EnviandoEmailConviteParcerias(dadosEmpresasSelecionadas[i].eMail1_Empresa, dadosEmpresasSelecionadas[i].eMail2_Empresa,
                            dadosEmpresasSelecionadas[i].eMaiL1_UsuarioContatoEmpresa, dadosEmpresasSelecionadas[i].eMaiL2_UsuarioContatoEmpresa, assuntoEmail, corpoEmail);
                }

                //---------------------------------------------------------------------------------------------
                //ENVIANDO SMS´s
                //---------------------------------------------------------------------------------------------
                EnviarSms enviarSmsMaster = new EnviarSms();

                smsMensagem = "ClienteMercado - Sua empresa foi convidada a participar da CENTRAL de COMPRAS em grupo. Para saber mais, acesse www.clientemercado.com.br.";

                for (int j = 0; j < dadosEmpresasSelecionadas.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa))
                    {
                        //TELEFONE 1 do USUÁRIO ADM
                        telefoneUsuarioADM = Regex.Replace(dadosEmpresasSelecionadas[j].celular1_UsuarioContatoEmpresa, "[()-]", "");
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

                resultado = new { convitesEnviados = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //EXCLUIR a CENTRAL de COMPRAS
        public ActionResult ExcluirACentralDeComprasQuandoNaoExistemParticipantesAlemDaEmpresaADM(string cCC, string eA)
        {
            try
            {
                var resultado = new { centralComprasExcluida = "" };

                int codCentralCompras = Convert.ToInt32(MD5Crypt.Descriptografar(cCC));
                int codEmpresaAdm = Convert.ToInt32(MD5Crypt.Descriptografar(eA));

                NEmpresasParticipantesCentralDeComprasService negociosEmpresasParticipantesDaCentralDeCompras = new NEmpresasParticipantesCentralDeComprasService();
                NCentralDeComprasService negociosCentralDeCompras = new NCentralDeComprasService();

                //EXCLUIR PARTICIPAÇÃO da EMPRESA ADM na CENTRAL de COMPRAS
                negociosEmpresasParticipantesDaCentralDeCompras.ExcluirParticipacaoNaCentralDeCompras(Convert.ToInt32(codCentralCompras), Convert.ToInt32(codEmpresaAdm));

                //EXCLUIR a CENTRAL de COMPRAS
                negociosCentralDeCompras.ExcluirACentralDeCompras(codCentralCompras, codEmpresaAdm);

                resultado = new { centralComprasExcluida = "Ok" };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //Carrega lista de Ramos de Comércio Varejista
        private static List<SelectListItem> ListagemDeDeRamosComercioVarejista()
        {
            //Buscar lista de CATEGORIAS VAREJISTAS
            NGruposAtividadesEmpresaProfissionalService negociosGruposDeAtividades = new NGruposAtividadesEmpresaProfissionalService();

            List<grupo_atividades_empresa> listaCategoriasVarejistas = negociosGruposDeAtividades.CarregarListaDeCategoriasVarejistas();
            List<SelectListItem> listCategorias = new List<SelectListItem>();

            listCategorias.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

            foreach (var grupoCategoarias in listaCategoriasVarejistas)
            {
                listCategorias.Add(new SelectListItem
                {
                    Text = grupoCategoarias.DESCRICAO_ATIVIDADE,
                    Value = grupoCategoarias.ID_GRUPO_ATIVIDADES.ToString()
                });
            }

            return listCategorias;
        }
    }
}
