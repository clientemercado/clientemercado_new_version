using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Utilitarios;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace ClienteMercado.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //LOGIN
        public ActionResult ConfirmarLogin(string login, string passw)
        {
            try
            {
                var resultado = new { logado = "", mensagemLogin = "" };

                //Login para Usuário de Empresa
                empresa_usuario_logins loginDoUsuario = new empresa_usuario_logins();
                USUARIO_FORNECEDOR loginUsuFornecedor = new USUARIO_FORNECEDOR();

                Login viewModelLogin = new Login();
                loginDoUsuario.LOGIN_EMPRESA_USUARIO_LOGINS = login;
                loginDoUsuario.SENHA_EMPRESA_USUARIO_LOGINS = Hash.GerarHashMd5(passw);
                empresa_usuario_logins empresaUsuarioLogins = new NLoginService().ConsultarLoginUsuarioEmpresa(loginDoUsuario);

                USUARIO_FORNECEDOR usuarioFornecodrLogins = new USUARIO_FORNECEDOR();
                if (empresaUsuarioLogins == null)
                {
                    loginUsuFornecedor.login_usuario_empresa_fornecedor = login;
                    loginUsuFornecedor.passw_usuario_empresa_fornecedor = Hash.GerarHashMd5(passw);
                    usuarioFornecodrLogins = new NUsuarioFornecedorService().ConsultarLoginUsuarioEmpresaFornecedora(loginUsuFornecedor);
                }

                if (empresaUsuarioLogins != null)
                {
                    Sessao.IdUsuarioLogado = empresaUsuarioLogins.ID_CODIGO_USUARIO;
                    Sessao.IdEmpresaUsuario = empresaUsuarioLogins.usuario_empresa.empresa_usuario.ID_CODIGO_EMPRESA;

                    resultado = new { logado = "sim", mensagemLogin = "" };
                }
                else if (usuarioFornecodrLogins != null)
                {
                    Sessao.IdUsuarioLogado = usuarioFornecodrLogins.id_usuario_fornecedor;
                    Sessao.IdEmpresaUsuario = usuarioFornecodrLogins.id_empresa_fornecedor;
                    resultado = new { logado = "sim", mensagemLogin = "" };
                }
                else
                {
                    resultado = new { logado = "nao", mensagemLogin = "Usuário ou Senha inválidos" };
                }

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        // Checa Login (Login ou E-mail) e Senha
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login loginUsuario) //(Login -> Classe do Modelo da View)
        {
            if (ModelState.IsValid)
            {
                Int32 tipoLogin = Convert.ToInt32(loginUsuario.TIPO_LOGIN);

                //Faz o login de acordo com o tipo de usuário selecionado
                if (tipoLogin == 1)
                {
                    //Login para Usuário de Empresa
                    NLoginService negocioLoginEmpresa = new NLoginService();
                    empresa_usuario_logins novoLoginEmpresa = new empresa_usuario_logins();

                    novoLoginEmpresa.LOGIN_EMPRESA_USUARIO_LOGINS = loginUsuario.LOGIN_EMPRESA_USUARIO;
                    novoLoginEmpresa.SENHA_EMPRESA_USUARIO_LOGINS =
                        Hash.GerarHashMd5(loginUsuario.SENHA_EMPRESA_USUARIO);

                    empresa_usuario_logins empresaUsuarioLogins =
                        negocioLoginEmpresa.ConsultarLoginUsuarioEmpresa(novoLoginEmpresa);

                    if (empresaUsuarioLogins != null)
                    {
                        var nomeUsuarioLogado = MD5Crypt.Criptografar(ManipulacaoStrings.pegarParteNomeUsuario(empresaUsuarioLogins.usuario_empresa.NICK_NAME_USUARIO));
                        var nomeEmpresaLogado = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.empresa_usuario.NOME_FANTASIA_EMPRESA);
                        var tipoContrato = empresaUsuarioLogins.usuario_empresa.empresa_usuario.ID_CODIGO_TIPO_CONTRATO_COTADA;

                        Sessao.IdUsuarioLogado = empresaUsuarioLogins.ID_CODIGO_USUARIO;
                        Sessao.IdEmpresaUsuario = empresaUsuarioLogins.usuario_empresa.empresa_usuario.ID_CODIGO_EMPRESA;

                        //Se o cadastro do Usuário Master da Empresa ainda não foi confirmado, redireciona-o para a área
                        // de Configurações onde irá escolher Atividades, Assinatura e modo de Pagamento
                        if (empresaUsuarioLogins.usuario_empresa.CADASTRO_CONFIRMADO == false && empresaUsuarioLogins.usuario_empresa.USUARIO_MASTER)
                        {
                            return RedirectToAction("Configuracoes", "UsuarioEmpresa",
                                    new
                                    {
                                        tpL = tipoLogin,
                                        nmU = nomeUsuarioLogado,
                                        nmE = nomeEmpresaLogado
                                    });
                        }
                        else if (empresaUsuarioLogins.usuario_empresa.CADASTRO_CONFIRMADO == false && empresaUsuarioLogins.usuario_empresa.USUARIO_MASTER == false)
                        {
                            //Retorna uma mensagem ao Usuário sobre a confirmação de cadastro, caso ainda não tenha ocorrido
                            loginUsuario.SENHA_EMPRESA_USUARIO = string.Empty;
                            ModelState.Clear();

                            return View(loginUsuario).ComMensagem("Cadastro aguardando autorização do Usuário Master");
                        }
                        else
                        {
                            //Entrará na pesquisa financeira se o tipo de contrato não for 1 - EMPRESA COTANTE
                            if (tipoContrato != 1)
                            {
                                //Ver Financeiro da Empresa / Usuário
                                NFinanceiroCobrancaFaturamentoUsuarioEmpresaService negocioFinanceiro = new NFinanceiroCobrancaFaturamentoUsuarioEmpresaService();
                                financeiro_cobranca_faturamento_usuario_empresa verEstadoFinanceiro = new financeiro_cobranca_faturamento_usuario_empresa();

                                verEstadoFinanceiro.ID_CODIGO_USUARIO = empresaUsuarioLogins.ID_CODIGO_USUARIO;
                                verEstadoFinanceiro.ID_CODIGO_EMPRESA = empresaUsuarioLogins.usuario_empresa.empresa_usuario.ID_CODIGO_EMPRESA;

                                financeiro_cobranca_faturamento_usuario_empresa financeiroCobrancaFaturamentoUsuarioEmpresa =
                                    negocioFinanceiro.ConsultarTitulosEmAbertoParaEmpresasEUsuarios(verEstadoFinanceiro);

                                /*
                                Aqui considero que o retorno sempre vem positivo (!= null) pq quando o usuário secundário for liberado pelo Master,
                                o registro financeiro será criado automaticamente
                                */
                                string idCobranca = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_COBRANCA_FATURAMENTO_USUARIO_EMPRESA.ToString();
                                var dataComparativa = financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.AddDays(
                                    -financeiroCobrancaFaturamentoUsuarioEmpresa.DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO);

                                /*
                                Define o destino do usuário no sistema, de acordo com situação financeira no sistema (VINCENDO / VENCIDO)
                                */
                                if ((DateTime.Now.Date >= dataComparativa.Date) && (DateTime.Now.Date <= financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.Date)
                                    && (financeiroCobrancaFaturamentoUsuarioEmpresa.PARCELA_PAGA_COBRANCA_FATURAMENTO == false))
                                {
                                    //Status VINCENDO - A VENCER
                                    if (tipoContrato == 2)
                                    {
                                        if (financeiroCobrancaFaturamentoUsuarioEmpresa.FINANCEIRO_TITULO_GERADO == false)
                                        {
                                            //2- INDIVIDUAL USUÁRIO - NÃO EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso para pagamento no MOIP
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vincendo"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }

                                        }
                                        else
                                        {
                                            //2- INDIVIDUAL USUÁRIO - EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso de cobrança
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vincendo"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (financeiroCobrancaFaturamentoUsuarioEmpresa.FINANCEIRO_TITULO_GERADO == false)
                                        {
                                            //3 e 4 - EMPRESA - NÃO EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso para pagamento no MOIP
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vincendo"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }
                                        }
                                        else
                                        {
                                            //3 e 4 - EMPRESA - EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso de cobrança
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vincendo"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }
                                        }

                                    }
                                }
                                else if ((DateTime.Now.Date > financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.Date)
                                    && (financeiroCobrancaFaturamentoUsuarioEmpresa.PARCELA_PAGA_COBRANCA_FATURAMENTO == false))
                                {
                                    //Status VENCIDO - BLOQUEADO PARA ACESSO
                                    if (tipoContrato == 2)
                                    {
                                        //2 - INDIVIDUAL USUÁRIO
                                        if (financeiroCobrancaFaturamentoUsuarioEmpresa.FINANCEIRO_TITULO_GERADO == false)
                                        {
                                            //NÃO EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso para pagamento no MOIP
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vencido"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }

                                        }
                                        else
                                        {
                                            //EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso de cobrança
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vencido"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //3 e 4 - EMPRESA
                                        if (financeiroCobrancaFaturamentoUsuarioEmpresa.FINANCEIRO_TITULO_GERADO == false)
                                        {
                                            //NÃO EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso para pagamento no MOIP
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vencido"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }

                                        }
                                        else
                                        {
                                            //EXISTE TÍTULO GERADO
                                            //Buscar alguns dados do Tipo de Contrato de Serviços
                                            NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                            tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                            verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                            tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                                negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                            //Redirecionar para página de aviso de cobrança
                                            if (buscarDetalhesTipoContratoDeServico != null)
                                            {
                                                //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                                return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                                    new
                                                    {
                                                        dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                        vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                        iRF = MD5Crypt.Criptografar(idCobranca),
                                                        dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioEmpresa.VENCIMENTO_FATURA_USUARIO_EMPRESA.ToShortDateString()),
                                                        sF = MD5Crypt.Criptografar("vencido"),
                                                        tpL = tipoLogin,
                                                        nmU = nomeUsuarioLogado,
                                                        nmE = nomeEmpresaLogado
                                                    });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Não está VINCENDO nem VENCIDO
                                    return RedirectToAction("PerfilUsuarioEmpresa", "UsuarioEmpresa",
                                            new
                                            {
                                                nmU = nomeUsuarioLogado,
                                                nmE = nomeEmpresaLogado,
                                                cloG = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO.ToString()),
                                                cbaiR = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.bairros_empresa_usuario.ID_BAIRRO_EMPRESA_USUARIO.ToString()),
                                                cciD = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.ToString()),
                                                cesT = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.ToString()),
                                                ccouT = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.paises_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.ToString())
                                            });
                                }

                            }
                            else
                            {
                                //Se o tipo de contrato da empresa for 1 - EMPRESA COTANTE
                                return RedirectToAction("PerfilUsuarioEmpresa", "UsuarioEmpresa",
                                    new
                                    {
                                        nmU = nomeUsuarioLogado,
                                        nmE = nomeEmpresaLogado,
                                        cloG = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO.ToString()),
                                        cbaiR = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.bairros_empresa_usuario.ID_BAIRRO_EMPRESA_USUARIO.ToString()),
                                        cciD = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.ToString()),
                                        cesT = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.ToString()),
                                        ccouT = MD5Crypt.Criptografar(empresaUsuarioLogins.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.paises_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.ToString())
                                    });
                            }

                        }
                    }
                }
                else if (tipoLogin == 2)
                {
                    //Login para Usuário Profissional de Serviços
                    NLoginService negocioLoginProfissional = new NLoginService();
                    profissional_usuario_logins novoLoginProfissional = new profissional_usuario_logins();

                    novoLoginProfissional.LOGIN_PROFISSIONAL_USUARIO_LOGINS = loginUsuario.LOGIN_EMPRESA_USUARIO;
                    novoLoginProfissional.SENHA_PROFISSIONAL_USUARIO_LOGINS = Hash.GerarHashMd5(loginUsuario.SENHA_EMPRESA_USUARIO);

                    profissional_usuario_logins profissionalUsuarioLogins =
                        negocioLoginProfissional.ConsultarLoginProfissionalServicos(novoLoginProfissional);

                    if (profissionalUsuarioLogins != null)
                    {
                        var nomeUsuarioLogado = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.NOME_USUARIO_PROFISSIONAL);
                        var nomeEmpresaLogado = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.profissional_usuario.NOME_COMERCIAL_PROFISSIONAL_USUARIO);

                        Sessao.IdUsuarioLogado = profissionalUsuarioLogins.usuario_profissional.ID_CODIGO_USUARIO_PROFISSIONAL;
                        Sessao.IdEmpresaUsuario = profissionalUsuarioLogins.usuario_profissional.profissional_usuario.ID_CODIGO_PROFISSIONAL_USUARIO;

                        //Se o cadastro do Usuário Master Profissional de serviços ainda não foi confirmado, 
                        //redireciona-o para a área de Configurações onde irá escolher Atividades, Assinatura e modo de Pagamento
                        if (profissionalUsuarioLogins.usuario_profissional.CADASTRO_CONFIRMADO == false && profissionalUsuarioLogins.usuario_profissional.USUARIO_MASTER)
                        {
                            return RedirectToAction("Configuracoes", "UsuarioProfissional",
                                new
                                {
                                    tpL = tipoLogin,
                                    nmU = nomeUsuarioLogado,
                                    nmE = nomeEmpresaLogado
                                });

                        }
                        else if (profissionalUsuarioLogins.usuario_profissional.CADASTRO_CONFIRMADO == false && profissionalUsuarioLogins.usuario_profissional.USUARIO_MASTER == false)
                        {
                            //Retorna uma mensagem ao Usuário sobre a confirmação de cadastro, caso ainda não tenha ocorrido
                            loginUsuario.SENHA_EMPRESA_USUARIO = string.Empty;
                            ModelState.Clear();

                            return View(loginUsuario).ComMensagem("Seu cadastro ainda não foi confirmado através do e-mail que lhe foi enviado ao cadastrar!");
                        }
                        else
                        {
                            /*
                            Entrará na pesquisa financeira se para o único tipo de contrato para esta modalidade de usuário - USUÁRIO PROFISSIONAL DE SERVIÇOS
                            */

                            //Ver Financeiro do Profissional de Serviços
                            NFinanceiroCobrancaFaturamentoUsuarioProfissionalService negocioFinanceiro = new NFinanceiroCobrancaFaturamentoUsuarioProfissionalService();
                            financeiro_cobranca_faturamento_usuario_profissional verEstadoFinanceiro = new financeiro_cobranca_faturamento_usuario_profissional();

                            verEstadoFinanceiro.ID_CODIGO_USUARIO_PROFISSIONAL = profissionalUsuarioLogins.ID_CODIGO_USUARIO_PROFISSIONAL;

                            financeiro_cobranca_faturamento_usuario_profissional financeiroCobrancaFaturamentoUsuarioProfissional =
                                negocioFinanceiro.ConsultarTitulosEmAbertoParaOUsuarioProfissionalDeServicos(verEstadoFinanceiro);

                            /*
                            Aqui considero que o retorno sempre vem positivo (!= null) pq quando o usuário  profissional se logar pela primeira vez, 
                            o registro financeiro será criado ao término da configuração
                            */

                            string idCobranca = financeiroCobrancaFaturamentoUsuarioProfissional.ID_COBRANCA_FATURAMENTO_USUARIO_PROFISSIONAL.ToString();
                            var dataComparativa = financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.AddDays(
                                -financeiroCobrancaFaturamentoUsuarioProfissional.DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO);

                            /*
                            Define o destino do usuário no sistema, de acordo com situação financeira no sistema (VINCENDO / VENCIDO)
                            */
                            if ((DateTime.Now.Date >= dataComparativa.Date) && (DateTime.Now.Date <= financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.Date)
                                && (financeiroCobrancaFaturamentoUsuarioProfissional.PARCELA_PAGA_COBRANCA_FATURAMENTO == false))
                            {
                                //Status VINCENDO - A VENCER
                                if (financeiroCobrancaFaturamentoUsuarioProfissional.FINANCEIRO_TITULO_GERADO == false)
                                {
                                    //NÃO EXISTE TÍTULO GERADO
                                    //Buscar alguns dados do Tipo de Contrato de Serviços
                                    NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                    tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                    verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                    tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                        negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                    //Redirecionar para página de aviso para pagamento no MOIP
                                    if (buscarDetalhesTipoContratoDeServico != null)
                                    {
                                        //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                        return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                            new
                                            {
                                                dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                iRF = MD5Crypt.Criptografar(idCobranca),
                                                dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.ToShortDateString()),
                                                sF = MD5Crypt.Criptografar("vincendo"),
                                                tpL = tipoLogin,
                                                nmU = nomeUsuarioLogado,
                                                nmE = nomeEmpresaLogado
                                            });
                                    }

                                }
                                else
                                {
                                    //EXISTE TÍTULO GERADO
                                    //Buscar alguns dados do Tipo de Contrato de Serviços
                                    NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                    tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                    verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                    tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                        negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                    //Redirecionar para página de aviso para pagamento no MOIP
                                    if (buscarDetalhesTipoContratoDeServico != null)
                                    {
                                        //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                        return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                            new
                                            {
                                                dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                iRF = MD5Crypt.Criptografar(idCobranca),
                                                dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.ToShortDateString()),
                                                sF = MD5Crypt.Criptografar("vincendo"),
                                                tpL = tipoLogin,
                                                nmU = nomeUsuarioLogado,
                                                nmE = nomeEmpresaLogado
                                            });
                                    }

                                }
                            }
                            else if ((DateTime.Now.Date > financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.Date)
                                && (financeiroCobrancaFaturamentoUsuarioProfissional.PARCELA_PAGA_COBRANCA_FATURAMENTO == false))
                            {
                                //Status VENCIDO - BLOQUEADO PARA ACESSO
                                if (financeiroCobrancaFaturamentoUsuarioProfissional.FINANCEIRO_TITULO_GERADO == false)
                                {
                                    //NÃO EXISTE TÍTULO GERADO
                                    //Buscar alguns dados do Tipo de Contrato de Serviços
                                    NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                    tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                    verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                    tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                        negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                    //Redirecionar para página de aviso para pagamento no MOIP
                                    if (buscarDetalhesTipoContratoDeServico != null)
                                    {
                                        //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                        return RedirectToAction("AvisoAssinatura", "FinanceiroClienteMercado",
                                            new
                                            {
                                                dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                iRF = MD5Crypt.Criptografar(idCobranca),
                                                dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.ToShortDateString()),
                                                sF = MD5Crypt.Criptografar("vencido"),
                                                tpL = tipoLogin,
                                                nmU = nomeUsuarioLogado,
                                                nmE = nomeEmpresaLogado
                                            });
                                    }
                                }
                                else
                                {
                                    //EXISTE TÍTULO GERADO
                                    //Buscar alguns dados do Tipo de Contrato de Serviços
                                    NTiposContratosServicosService negocioTiposContratosServicos = new NTiposContratosServicosService();
                                    tipos_contratos_servicos verTipoContratoDeServico = new tipos_contratos_servicos();

                                    verTipoContratoDeServico.ID_CODIGO_TIPO_CONTRATO_COTADA = financeiroCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_TIPO_CONTRATO_COTADA;

                                    tipos_contratos_servicos buscarDetalhesTipoContratoDeServico =
                                        negocioTiposContratosServicos.BuscarDadosDoTipoDeContratoDeServicos(verTipoContratoDeServico);

                                    //Redirecionar para página de aviso para pagamento no MOIP
                                    if (buscarDetalhesTipoContratoDeServico != null)
                                    {
                                        //Redireciona para a Action responsável por montar o Aviso de Pagamento de assinatura, passando os parâmetros necessários
                                        return RedirectToAction("AvisoAssinaturaWaiting", "FinanceiroClienteMercado",
                                            new
                                            {
                                                dcPC = MD5Crypt.Criptografar("Plano " + buscarDetalhesTipoContratoDeServico.DESCRICAO_TIPO_CONTRATO_COTADA),
                                                vPC = MD5Crypt.Criptografar(Regex.Replace(buscarDetalhesTipoContratoDeServico.VALOR_MENSAL_TIPO_CONTRATO_COTADA.ToString(), "[.,]", "")),
                                                iRF = MD5Crypt.Criptografar(idCobranca),
                                                dVF = MD5Crypt.Criptografar(financeiroCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL.ToShortDateString()),
                                                sF = MD5Crypt.Criptografar("vencido"),
                                                tpL = tipoLogin,
                                                nmU = nomeUsuarioLogado,
                                                nmE = nomeEmpresaLogado
                                            });
                                    }
                                }
                            }
                            else
                            {
                                //Não está VINCENDO nem VENCIDO
                                return RedirectToAction("PerfilUsuarioProfissional", "UsuarioProfissional",
                                new
                                {
                                    tpL = tipoLogin,
                                    nmU = nomeUsuarioLogado,
                                    nmE = nomeEmpresaLogado
                                });
                            }
                        }
                    }
                }
                else if (tipoLogin == 3)
                {
                    //Login para Usuário Cotante
                    NLoginService negocioLoginCotante = new NLoginService();
                    usuario_cotante_logins novoLoginCotante = new usuario_cotante_logins();

                    novoLoginCotante.LOGIN_USUARIO_COTANTE_LOGINS = loginUsuario.LOGIN_EMPRESA_USUARIO;
                    novoLoginCotante.SENHA_USUARIO_COTANTE_LOGINS = Hash.GerarHashMd5(loginUsuario.SENHA_EMPRESA_USUARIO);

                    usuario_cotante_logins usuarioCotanteLogins =
                        negocioLoginCotante.ConsultarLoginUsuarioCotante(novoLoginCotante);

                    if (usuarioCotanteLogins != null)
                    {
                        Sessao.IdUsuarioLogado = usuarioCotanteLogins.usuario_cotante.ID_CODIGO_USUARIO_COTANTE;

                        return RedirectToAction("PerfilUsuarioCotante", "UsuarioCotante",
                            new
                            {
                                nmU = MD5Crypt.Criptografar(ManipulacaoStrings.pegarParteNomeUsuario(usuarioCotanteLogins.usuario_cotante.NICK_NAME_USUARIO_COTANTE)),
                                cloG = MD5Crypt.Criptografar(usuarioCotanteLogins.usuario_cotante.ID_CODIGO_ENDERECO_EMPRESA_USUARIO.ToString()),
                                cbaiR = MD5Crypt.Criptografar(usuarioCotanteLogins.usuario_cotante.enderecos_empresa_usuario.bairros_empresa_usuario.ID_BAIRRO_EMPRESA_USUARIO.ToString()),
                                cciD = MD5Crypt.Criptografar(usuarioCotanteLogins.usuario_cotante.enderecos_empresa_usuario.cidades_empresa_usuario.ID_CIDADE_EMPRESA_USUARIO.ToString()),
                                cesT = MD5Crypt.Criptografar(usuarioCotanteLogins.usuario_cotante.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO.ToString()),
                                ccouT = MD5Crypt.Criptografar(usuarioCotanteLogins.usuario_cotante.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.paises_empresa_usuario.ID_PAISES_EMPRESA_USUARIO.ToString())
                            });
                    }
                }

            }

            loginUsuario.SENHA_EMPRESA_USUARIO = string.Empty;
            ModelState.Clear();

            return View(loginUsuario).ComMensagem("Usuário ou Senha inválidos para tipo de Login selecionado.");
        }

        public ActionResult Lembrete()
        {
            Lembrete retorno = new Lembrete();
            retorno.retornoEnvio = true;

            return View(retorno);
        }

        // Checa E-mail para envio de senha perdida
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lembrete(Lembrete emaiLembrete) //(Lembrete -> Classe do Modelo da View)
        {
            try
            {
                Int32 tipoLogin = Convert.ToInt32(emaiLembrete.TIPO_LOGIN); //Indica o tipo de usuário que esqueceu a Senha (Empresa, Profissional ou Cotante)
                int tipoDeEmail = 0;   //Indica que é um e-mail de Lembrete de Senha, em caso de esquecimento
                bool enviou = false;

                if (ModelState.IsValid)
                {
                    NLembreteService negociosLembrete = new NLembreteService();

                    //Busca o e-mail para lembrete, de acordo com o tipo de usuário selecionado
                    if (tipoLogin == 1)
                    {
                        //E-mail de recuperação de senha para Usuário de Empresa
                        empresa_usuario_logins checarEmail = new empresa_usuario_logins();

                        checarEmail.EMAIL1_USUARIO = emaiLembrete.EMAIL_ENVIO_SENHA;

                        empresa_usuario_logins emailRecuperacaoSenha = negociosLembrete.ConsultarEmailEmpresaUsuario(checarEmail);

                        if (emailRecuperacaoSenha != null)
                        {
                            EnviarEmail enviarEmail = new EnviarEmail();

                            enviou = enviarEmail.EnviandoEmail(emailRecuperacaoSenha.EMAIL1_USUARIO, emailRecuperacaoSenha.usuario_empresa.NICK_NAME_USUARIO, emailRecuperacaoSenha.LOGIN_EMPRESA_USUARIO_LOGINS, emailRecuperacaoSenha.SENHA_EMPRESA_USUARIO_LOGINS, tipoDeEmail, tipoLogin);

                            if (enviou)
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Você receberá no e-mail: <br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br>instruções para recuperação de sua senha.");
                            }
                            else
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Não foi possível enviar as instruções de recuperação de sua senha para o e-mail: <br><br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br> Tente mais tarde!");
                            }
                        }
                    }
                    else if (tipoLogin == 2)
                    {
                        //E-mail de recuperação de senha para Usuário Profissional de Serviços
                        profissional_usuario_logins checarEmail = new profissional_usuario_logins();

                        checarEmail.EMAIL1_USUARIO = emaiLembrete.EMAIL_ENVIO_SENHA;

                        profissional_usuario_logins emailRecuperacaoSenha = negociosLembrete.ConsultarEmailProfissionalUsuario(checarEmail);

                        if (emailRecuperacaoSenha != null)
                        {
                            EnviarEmail enviarEmail = new EnviarEmail();

                            enviou = enviarEmail.EnviandoEmail(emailRecuperacaoSenha.EMAIL1_USUARIO, emailRecuperacaoSenha.usuario_profissional.NOME_USUARIO_PROFISSIONAL, emailRecuperacaoSenha.LOGIN_PROFISSIONAL_USUARIO_LOGINS, emailRecuperacaoSenha.SENHA_PROFISSIONAL_USUARIO_LOGINS, tipoDeEmail, tipoLogin);

                            if (enviou)
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Você receberá no e-mail: <br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br>instruções para recuperação de sua senha.");
                            }
                            else
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Não foi possível enviar as instruções de recuperação de sua senha para o e-mail: <br><br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br> Tente mais tarde!");
                            }
                        }
                    }
                    else if (tipoLogin == 3)
                    {
                        //E-mail de recuperação de senha para Usuário Cotante
                        usuario_cotante_logins checarEmail = new usuario_cotante_logins();

                        checarEmail.EMAIL1_USUARIO = emaiLembrete.EMAIL_ENVIO_SENHA;

                        usuario_cotante_logins emailRecuperacaoSenha = negociosLembrete.ConsultarEmailUsuarioCotante(checarEmail);

                        if (emailRecuperacaoSenha != null)
                        {
                            EnviarEmail enviarEmail = new EnviarEmail();

                            enviou = enviarEmail.EnviandoEmail(emailRecuperacaoSenha.EMAIL1_USUARIO, emailRecuperacaoSenha.usuario_cotante.NICK_NAME_USUARIO_COTANTE, emailRecuperacaoSenha.LOGIN_USUARIO_COTANTE_LOGINS, emailRecuperacaoSenha.SENHA_USUARIO_COTANTE_LOGINS, tipoDeEmail, tipoLogin);

                            if (enviou)
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Você receberá no e-mail: <br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br>instruções para recuperação de sua senha.");
                            }
                            else
                            {
                                emaiLembrete.retornoEnvio = false;

                                return
                                    View(emaiLembrete)
                                        .ComMensagem("Não foi possível enviar as instruções de recuperação de sua senha para o e-mail: <br><br>" + emaiLembrete.EMAIL_ENVIO_SENHA +
                                                     "<br><br> Tente mais tarde!");
                            }
                        }
                    }

                }

                emaiLembrete.EMAIL_ENVIO_SENHA = string.Empty;
                emaiLembrete.retornoEnvio = true;
                ModelState.Clear(); //Limpa o campo de texto

                return View(emaiLembrete).ComMensagem("E-mail não foi encontrado em nossa base para esta conta.");
            }
            catch (Exception erro)
            {
                //return View(emaiLembrete).ComMensagem("E-mail não foi encontrado em nossa base para esta conta."+erro.Message);
                return View(emaiLembrete).ComMensagem("O E-mail não foi encontrado em nossa base para esta conta.");
            }
        }

        //Alterar Senha (Forgot = esqueci)
        public ActionResult ForgotPas(int tipoUsuario, string login, string senha)
        {
            //Checa o login de acordo com o tipo de usuário selecionado
            if (tipoUsuario == 1)
            {
                //Login para Usuário de Empresa
                NLoginService negocioLoginEmpresa = new NLoginService();
                empresa_usuario_logins checaLoginEmpresa = new empresa_usuario_logins();

                checaLoginEmpresa.LOGIN_EMPRESA_USUARIO_LOGINS = login;
                checaLoginEmpresa.SENHA_EMPRESA_USUARIO_LOGINS = senha;

                empresa_usuario_logins empresaUsuarioLogins =
                    negocioLoginEmpresa.ConsultarLoginUsuarioEmpresa(checaLoginEmpresa);

                if (empresaUsuarioLogins != null)
                {
                    Sessao.IdUsuarioLogado = empresaUsuarioLogins.ID_CODIGO_USUARIO;
                    ViewBag.tipoLogin = tipoUsuario;

                    return View();
                }
            }
            else if (tipoUsuario == 2)
            {
                //Login para Usuário Profissional de Serviços
                NLoginService negocioLoginProfissional = new NLoginService();
                profissional_usuario_logins novoLoginProfissional = new profissional_usuario_logins();

                novoLoginProfissional.LOGIN_PROFISSIONAL_USUARIO_LOGINS = login;
                novoLoginProfissional.SENHA_PROFISSIONAL_USUARIO_LOGINS = senha;

                profissional_usuario_logins profissionalUsuarioLogins =
                    negocioLoginProfissional.ConsultarLoginProfissionalServicos(novoLoginProfissional);

                if (profissionalUsuarioLogins != null)
                {
                    Sessao.IdUsuarioLogado = profissionalUsuarioLogins.ID_CODIGO_PROFISSIONAL_USUARIO_LOGINS;
                    ViewBag.tipoLogin = tipoUsuario;

                    return View();
                }
            }
            else if (tipoUsuario == 3)
            {
                //Login para Usuário Cotante
                NLoginService negocioLoginCotante = new NLoginService();
                usuario_cotante_logins novoLoginCotante = new usuario_cotante_logins();

                novoLoginCotante.LOGIN_USUARIO_COTANTE_LOGINS = login;
                novoLoginCotante.SENHA_USUARIO_COTANTE_LOGINS = senha;

                usuario_cotante_logins usuarioCotanteLogins =
                    negocioLoginCotante.ConsultarLoginUsuarioCotante(novoLoginCotante);

                if (usuarioCotanteLogins != null)
                {
                    Sessao.IdUsuarioLogado = usuarioCotanteLogins.ID_CODIGO_USUARIO_COTANTE;
                    ViewBag.tipoLogin = tipoUsuario;

                    return View();
                }
            }

            //Se os dados de localização do Usuário não estiverem Ok, redireciona o User para a área de Login.
            return RedirectToAction("Login", "Login");
        }

        //Salvar o cadastro da Empresa / Usuário
        [HttpPost]
        public ActionResult ForgotPas(CadastroNovaSenha cadastroNovaSenhaUsuario)
        {
            try
            {
                if (cadastroNovaSenhaUsuario.TIPO_LOGIN == 1)
                {
                    //Cadastro de nova Senha para Usuário de Empresa
                    NLoginService negocioAlteraSenhaEmpresa = new NLoginService();
                    empresa_usuario_logins empresaUsuarioLogins = new empresa_usuario_logins();

                    empresaUsuarioLogins.ID_CODIGO_USUARIO = (Int32)Sessao.IdUsuarioLogado;
                    empresaUsuarioLogins.SENHA_EMPRESA_USUARIO_LOGINS = Hash.GerarHashMd5(cadastroNovaSenhaUsuario.SENHA_EMPRESA_USUARIO_LOGINS);

                    empresa_usuario_logins alteraSenhaUsuarioEmpresa =
                        negocioAlteraSenhaEmpresa.GravarNovaSenhaUsuarioEmpresa(empresaUsuarioLogins);

                    if (alteraSenhaUsuarioEmpresa != null)
                    {
                        return RedirectToAction("ConfirmacaoRecadastroPas", "Login");
                    }
                }
                else if (cadastroNovaSenhaUsuario.TIPO_LOGIN == 2)
                {
                    //Cadastro de nova Senha para Usuário Profissional de Serviços
                    NLoginService negocioAlteraSenhaProfissional = new NLoginService();
                    profissional_usuario_logins profissionalServicosLogins = new profissional_usuario_logins();

                    profissionalServicosLogins.ID_CODIGO_USUARIO_PROFISSIONAL = (Int32)Sessao.IdUsuarioLogado;
                    profissionalServicosLogins.SENHA_PROFISSIONAL_USUARIO_LOGINS =
                        Hash.GerarHashMd5(cadastroNovaSenhaUsuario.SENHA_EMPRESA_USUARIO_LOGINS);

                    profissional_usuario_logins alteraSenhaProfissionalServicos =
                        negocioAlteraSenhaProfissional.GravarNovaSenhaProfissionalServicos(profissionalServicosLogins);

                    if (alteraSenhaProfissionalServicos != null)
                    {
                        return RedirectToAction("ConfirmacaoRecadastroPas", "Login");
                    }
                }
                else if (cadastroNovaSenhaUsuario.TIPO_LOGIN == 3)
                {
                    //Cadastro de nova Senha para Usuário Profissional de Serviços
                    NLoginService negocioAlteraSenhaUsuarioCotante = new NLoginService();
                    usuario_cotante_logins usuarioCotanteLogins = new usuario_cotante_logins();

                    usuarioCotanteLogins.ID_CODIGO_USUARIO_COTANTE = (Int32)Sessao.IdUsuarioLogado;
                    usuarioCotanteLogins.SENHA_USUARIO_COTANTE_LOGINS = Hash.GerarHashMd5(cadastroNovaSenhaUsuario.SENHA_EMPRESA_USUARIO_LOGINS);

                    usuario_cotante_logins alteraSenhaUsuarioCotante =
                        negocioAlteraSenhaUsuarioCotante.GravarNovaSenhaUsuarioCotante(usuarioCotanteLogins);

                    if (alteraSenhaUsuarioCotante != null)
                    {
                        return RedirectToAction("ConfirmacaoRecadastroPas", "Login");
                    }
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return View().ComMensagem("* Erro interno ao salvar a nova Senha de Usuário no sistema! Por favor, tente mais tarde!");
        }

        public ActionResult ConfirmacaoRecadastroPas()
        {
            return View();
        }

        //LOGOUT
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            //Remover todas as Sessions criadas pelo usuário
            Session.Abandon();

            return RedirectToAction("Index", "Login", new { area = "" });
        }

    }
}
