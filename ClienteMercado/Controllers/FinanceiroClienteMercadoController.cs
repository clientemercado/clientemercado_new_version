using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Utilitarios;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.Controllers
{
    public class FinanceiroClienteMercadoController : Controller
    {
        //Financeiro do momento da configuração e antes primeiro acesso
        public ActionResult ValidarAssinatura(string nmU, string nmE, int tpL, string dPc, string vPc, string iRf)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                //Carrega itens a serem exibidos na View
                GerarFinanceiro gerarFinanceiro = new GerarFinanceiro();

                //Busca dados da Empresa/Profissional de Serviços e Usuário Master das mesmas, necessários ao faturamento
                NLoginService negocioLoginUsuario = new NLoginService();

                //Operação pré faturamento
                if (tpL == 1)
                {
                    //Dados da Empresa e do Usuário Master (Faturamento)
                    empresa_usuario_logins buscarDadosUsuario = new empresa_usuario_logins();

                    buscarDadosUsuario.ID_CODIGO_USUARIO = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    empresa_usuario_logins dadosEmpresaUsuario =
                        negocioLoginUsuario.BuscarDadosUsuarioEEmpresaParaFaturamento(buscarDadosUsuario);

                    if (dadosEmpresaUsuario != null)
                    {
                        //Atribuir dados da Empresa e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.NOME_FANTASIA_EMPRESA;
                        gerarFinanceiro.EMAIL_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.EMAIL1_EMPRESA;
                        gerarFinanceiro.CEP_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.TELEFONE1_EMPRESA_USUARIO;
                        gerarFinanceiro.LOGRADOURO_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_EMPRESA_PAGADOR = "1100"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.ESTADO_EMPRESA_PAGADOR =
                        //dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                        //    .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                        //Atribuir dados do Usuário Master e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_USUARIO_PAGADOR = dadosEmpresaUsuario.usuario_empresa.NOME_USUARIO;
                        gerarFinanceiro.EMAIL_USUARIO_PAGADOR = dadosEmpresaUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.TELEFONE1_USUARIO_EMPRESA;
                        gerarFinanceiro.LOGRADOURO_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_USUARIO_PAGADOR = "1110"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.bairros_empresa_usuario
                                .BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario
                                .CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_USUARIO_PAGADOR =
                        //    dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario
                        //        .UF_CIDADE_EMPRESA_USUARIO;

                    }
                }
                else if (tpL == 2)
                {
                    //Busca dados Usuário Master Profissional / Profissional Serviços
                    profissional_usuario_logins buscarDadosUsuario = new profissional_usuario_logins();
                    buscarDadosUsuario.ID_CODIGO_USUARIO_PROFISSIONAL = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    profissional_usuario_logins dadosProfissionalUsuario =
                        negocioLoginUsuario.BuscarDadosUsuarioProfissionalDeServicosParaFaturamento(buscarDadosUsuario);

                    if (dadosProfissionalUsuario != null)
                    {
                        //Atribuir dados da Empresa do Profissional de Serviços e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.profissional_usuario
                                .NOME_COMERCIAL_PROFISSIONAL_USUARIO;
                        gerarFinanceiro.EMAIL_EMPRESA_PAGADOR = dadosProfissionalUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.profissional_usuario
                                .TELEFONE1_PROFISSIONAL_USUARIO;
                        gerarFinanceiro.LOGRADOURO_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_EMPRESA_PAGADOR = "1100"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_EMPRESA_PAGADOR =
                        //    dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                        //        .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                        //Atribuir dados do Usuário Profissional de Serviços e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.NOME_USUARIO_PROFISSIONAL;
                        gerarFinanceiro.EMAIL_USUARIO_PAGADOR = dadosProfissionalUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.TELEFONE1_USUARIO_PROFISSIONAL;
                        gerarFinanceiro.LOGRADOURO_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_USUARIO_PAGADOR = "1110"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_USUARIO_PAGADOR =
                        //    dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                        //        .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                    }
                }

                /*
                Tipos da listagem dos Responsáveis(0 - Configurações / 1 - Dia a dia)
                */
                var listaResponsaveisACobrar = ListagemResponsaveisACobrar(0);

                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                ViewBag.descricaoPlanoContratado = MD5Crypt.Descriptografar(dPc);
                ViewBag.valorPlanoContratado = MD5Crypt.Descriptografar(vPc);
                ViewBag.idRegistroFinanceiro = MD5Crypt.Descriptografar(iRf);

                gerarFinanceiro.ListagemResponsaveisCobranca = listaResponsaveisACobrar;

                return View(gerarFinanceiro);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Financeiro do acesso habitual, para os casos em que o título de pagamento ainda não foi gerado em nosso parceiro (atualmente MOIP)
        public ActionResult AvisoAssinatura(string dcPC, string vPC, string iRF, string dVF, string sF, int tpL, string nmU, string nmE)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                //Carrega itens a serem exibidos na View
                GerarFinanceiro gerarFinanceiro = new GerarFinanceiro();

                //Busca dados da Empresa/Profissional de Serviços e Usuário Master das mesmas, necessários ao faturamento
                NLoginService negocioLoginUsuario = new NLoginService();

                //Operação pré faturamento
                if (tpL == 1)
                {
                    //Dados da Empresa e do Usuário Master (Faturamento)
                    empresa_usuario_logins buscarDadosUsuario = new empresa_usuario_logins();

                    buscarDadosUsuario.ID_CODIGO_USUARIO = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    empresa_usuario_logins dadosEmpresaUsuario =
                        negocioLoginUsuario.BuscarDadosUsuarioEEmpresaParaFaturamento(buscarDadosUsuario);

                    if (dadosEmpresaUsuario != null)
                    {
                        //Atribuir dados da Empresa e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.NOME_FANTASIA_EMPRESA;
                        gerarFinanceiro.EMAIL_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.EMAIL1_EMPRESA;
                        gerarFinanceiro.CEP_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.TELEFONE1_EMPRESA_USUARIO;
                        gerarFinanceiro.LOGRADOURO_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_EMPRESA_PAGADOR = "1100"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_EMPRESA_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_EMPRESA_PAGADOR =
                        //    dadosEmpresaUsuario.usuario_empresa.empresa_usuario.enderecos_empresa_usuario
                        //        .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                        //Atribuir dados do Usuário Master e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_USUARIO_PAGADOR = dadosEmpresaUsuario.usuario_empresa.NOME_USUARIO;
                        gerarFinanceiro.EMAIL_USUARIO_PAGADOR = dadosEmpresaUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.TELEFONE1_USUARIO_EMPRESA;
                        gerarFinanceiro.LOGRADOURO_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_USUARIO_PAGADOR = "1110"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.bairros_empresa_usuario
                                .BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_USUARIO_PAGADOR =
                            dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario
                                .CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_USUARIO_PAGADOR =
                        //    dadosEmpresaUsuario.usuario_empresa.enderecos_empresa_usuario.cidades_empresa_usuario
                        //        .UF_CIDADE_EMPRESA_USUARIO;

                        //Parâmetro que será usado para definir o conteúdo da tela de cobrança conforme o tipo de usuário logado
                        if ((dadosEmpresaUsuario.usuario_empresa.USUARIO_MASTER) && (dadosEmpresaUsuario.usuario_empresa.empresa_usuario.ID_CODIGO_TIPO_CONTRATO_COTADA == 3
                            || dadosEmpresaUsuario.usuario_empresa.empresa_usuario.ID_CODIGO_TIPO_CONTRATO_COTADA == 4))
                        {
                            ViewBag.ehMaster = true;
                            ViewBag.tipoContrato = 34; //Obs: Tipo de contrato 34 (3 ou 4 onde a cobrança é feita à empresa pelos usuários ligados ao sistema)
                        }
                        else if ((dadosEmpresaUsuario.usuario_empresa.USUARIO_MASTER == false) && (dadosEmpresaUsuario.usuario_empresa.empresa_usuario.ID_CODIGO_TIPO_CONTRATO_COTADA == 3
                            || dadosEmpresaUsuario.usuario_empresa.empresa_usuario.ID_CODIGO_TIPO_CONTRATO_COTADA == 4))
                        {
                            ViewBag.ehMaster = false;
                            ViewBag.tipoContrato = 34;  //Obs: Tipo de contrato 34 (3 ou 4 onde a cobrança é feita à empresa pelos usuários ligados ao sistema)
                        }
                        else
                        {
                            ViewBag.ehMaster = false;
                            ViewBag.tipoContrato = 2;  //Obs: Tipo de contrato 2 (Usuário Individual, é o que paga pelo seu uso)
                        }

                        ViewBag.descricaoPlanoContratado = MD5Crypt.Descriptografar(dcPC);
                        ViewBag.valorPlanoContratado = MD5Crypt.Descriptografar(vPC);
                        ViewBag.idRegistroFinanceiro = MD5Crypt.Descriptografar(iRF);
                        ViewBag.dataVencimentoFatura = MD5Crypt.Descriptografar(dVF);
                        ViewBag.statusFinanceiro = MD5Crypt.Descriptografar(sF);
                        ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                        ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                        ViewBag.tipoLogin = tpL;
                    }
                }
                else if (tpL == 2)
                {
                    //Busca dados Usuário Master Profissional / Profissional Serviços
                    profissional_usuario_logins buscarDadosUsuario = new profissional_usuario_logins();
                    buscarDadosUsuario.ID_CODIGO_USUARIO_PROFISSIONAL = Convert.ToInt32(Session["IdUsuarioLogado"]);

                    profissional_usuario_logins dadosProfissionalUsuario =
                        negocioLoginUsuario.BuscarDadosUsuarioProfissionalDeServicosParaFaturamento(buscarDadosUsuario);

                    if (dadosProfissionalUsuario != null)
                    {
                        //Atribuir dados da Empresa do Profissional de Serviços e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.profissional_usuario
                                .NOME_COMERCIAL_PROFISSIONAL_USUARIO;
                        gerarFinanceiro.EMAIL_EMPRESA_PAGADOR = dadosProfissionalUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.profissional_usuario
                                .TELEFONE1_PROFISSIONAL_USUARIO;
                        gerarFinanceiro.LOGRADOURO_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_EMPRESA_PAGADOR = "1100"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_EMPRESA_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_EMPRESA_PAGADOR =
                        //    dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                        //        .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                        //Atribuir dados do Usuário Profissional de Serviços e de localização, às variáveis do modelo da View
                        gerarFinanceiro.NOME_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.NOME_USUARIO_PROFISSIONAL;
                        gerarFinanceiro.EMAIL_USUARIO_PAGADOR = dadosProfissionalUsuario.EMAIL1_USUARIO;
                        gerarFinanceiro.CEP_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .CEP_ENDERECO_EMPRESA_USUARIO.ToString();
                        gerarFinanceiro.TELEFONE_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.TELEFONE1_USUARIO_PROFISSIONAL;
                        gerarFinanceiro.LOGRADOURO_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .TIPO_LOGRADOURO_EMPRESA_USUARIO + " " +
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .LOGRADOURO_CEP_EMPRESA_USUARIO;
                        gerarFinanceiro.NUMERO_LOGRADOURO_USUARIO_PAGADOR = "1110"; //Este número é Fake.
                        gerarFinanceiro.BAIRRO_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                        gerarFinanceiro.CIDADE_USUARIO_PAGADOR =
                            dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                                .cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                        //gerarFinanceiro.ESTADO_USUARIO_PAGADOR =
                        //    dadosProfissionalUsuario.usuario_profissional.enderecos_empresa_usuario
                        //        .cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO;

                        ViewBag.ehMaster = false;   //Parâmetro que será usado para definir o conteúdo da tela de cobrança conforme o tipo de usuário logado
                        ViewBag.tipoContrato = 2;  //Obs: Tipo de contrato 2 (Usuário Individual, é o que paga pelo seu uso)

                        ViewBag.descricaoPlanoContratado = MD5Crypt.Descriptografar(dcPC);
                        ViewBag.valorPlanoContratado = MD5Crypt.Descriptografar(vPC);
                        ViewBag.idRegistroFinanceiro = MD5Crypt.Descriptografar(iRF);
                        ViewBag.dataVencimentoFatura = MD5Crypt.Descriptografar(dVF);
                        ViewBag.statusFinanceiro = MD5Crypt.Descriptografar(sF);
                        ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                        ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                        ViewBag.tipoLogin = tpL;
                    }
                }

                /*
                Tipos da listagem dos Responsáveis(0 - Configurações / 1 - Dia a dia)
                */
                var listaResponsaveisACobrar = ListagemResponsaveisACobrar(1);

                gerarFinanceiro.ListagemResponsaveisCobranca = listaResponsaveisACobrar;

                return View(gerarFinanceiro);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Financeiro do acesso habitual, para os casos em que o título de pagamento já foi gerado em nosso parceiro (atualmente MOIP)
        public ActionResult AvisoAssinaturaWaiting(string dcPC, string vPC, string iRF, string dVF, string sF, int tpL, string nmU, string nmE)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                ViewBag.descricaoPlanoContratado = dcPC;
                ViewBag.valorPlanoContratado = vPC;
                ViewBag.idRegistroFinanceiro = iRF;
                ViewBag.dataVencimentoFatura = MD5Crypt.Descriptografar(dVF);
                ViewBag.statusFinanceiro = MD5Crypt.Descriptografar(sF);
                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                ViewBag.tipoLogin = tpL;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Redireciona para o perfil do Usuário (Nos casos de cobrança do tipo "vincendo" ou o mesmo que "a vencer")
        public ActionResult IrParaOPerfilDoUsuario(string nmU, string nmE, int tpL)
        {
            //Redireciona para o perfil adequado, depois de passar pelas telas de aviso de cobrança
            if (tpL == 1)
            {
                //Redireciona para a área de trabalho do Usuário de Empresa
                return RedirectToAction("PerfilUsuarioEmpresa", "UsuarioEmpresa",
                    new
                    {
                        nmU = nmU,
                        nmE = nmE,
                        tpL = tpL
                    });
            }
            else
            {
                //Redireciona para a área de trabalho do Usuário Profissional de serviços
                return RedirectToAction("PerfilUsuarioProfissional", "UsuarioProfissional",
                    new
                    {
                        nmU = nmU,
                        nmE = nmE,
                        tpl = tpL
                    });
            }

        }

        private static List<SelectListItem> ListagemResponsaveisACobrar(int tipo)
        {
            List<SelectListItem> listResponsaveisACobrar = new List<SelectListItem>();
            listResponsaveisACobrar.Add(new SelectListItem { Text = "Selecione...", Value = "" });
            listResponsaveisACobrar.Add(new SelectListItem { Text = "Empresa", Value = "1" });

            if (tipo == 0)
            {
                listResponsaveisACobrar.Add(new SelectListItem { Text = "Usuário Master", Value = "2" });
            }
            else
            {
                listResponsaveisACobrar.Add(new SelectListItem { Text = "Usuário", Value = "2" });
            }

            return listResponsaveisACobrar;
        }

        ////Action responsável por setar 1 no campo FINANCEIRO_TITULO_GERADO nas tabelas (financeiro_cobranca_faturamento_usuario_empresa / financeiro_cobranca_faturamento_usuario_profissional)
        //[HttpPost]
        //public ActionResult ConfirmarTituloGerado(string id_transacao, string valor, int status_pagamento, int cod_moip, int forma_pagamento, string tipo_pagamento,
        //    int parcelas, string email_consumidor, string classificacao)
        //{
        //    //1) Verificar se o campo FINANCEIRO_TITULO_GERADO já está setado (buscar pelo id_transacao) e agir de acordo com esse retorno;
        //    //2) Checar o status retornado em 'status_pagamento' e se for 'BoletoImpresso', setar 1 no campo FINANCEIRO_TITULO_GERADO, nas tabelas acima mencionadas;
        //    //2) Ver possibilidade de depois aproveitar outros status do MOIP;

        //    NFinanceiroCobrancaFaturamentoUsuarioEmpresa negocioFinanceiroCobranca = new NFinanceiroCobrancaFaturamentoUsuarioEmpresa();
        //    financeiro_cobranca_faturamento_usuario_empresa financeiroCobrancaFaturamentoUsuarioEmpresa =
        //        new financeiro_cobranca_faturamento_usuario_empresa();

        //    //financeiroCobrancaFaturamentoUsuarioEmpresa.ID_COBRANCA_FATURAMENTO_USUARIO_EMPRESA = OBS: Continuar daqui...


        //    //A Ação abaixo deverá ocorrer de acordo com o resultado do item 1
        //    if (status_pagamento == 1 || status_pagamento == 2 || status_pagamento == 3 || status_pagamento == 6)
        //    {

        //    }

        //    return null;
        //}

    }
}
