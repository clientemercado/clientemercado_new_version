using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class Configuration_registryController : Controller
    {
        //
        // GET: /Acesso/Configuration_registry/

        public ActionResult Index(string one)
        {
            try
            {
                if (Session["IdUsuarioLogado"] != null)
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NEmpresaUsuarioLoginsService negociosEmpresaUsuarioLogins = new NEmpresaUsuarioLoginsService();

                    CadastroEmpresaViewModel dadosDaEmpresa = new CadastroEmpresaViewModel();

                    empresa_usuario dadosEmpresa = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]) });
                    usuario_empresa dadosUsuarioEmpresa = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Session["IdUsuarioLogado"]));
                    empresa_usuario_logins dadosLoginEmpresaUsuario =
                        negociosEmpresaUsuarioLogins.ConsultarDadosDeContatoDoUsuario(Convert.ToInt32(Session["IdUsuarioLogado"]));

                    //POPULAR VIEW MODEL - DADOS EMPRESA
                    dadosDaEmpresa.CNPJ_CPF_EMPRESA_USUARIO = FormatarCpfCnpj(dadosEmpresa.CNPJ_EMPRESA_USUARIO);
                    dadosDaEmpresa.RAZAO_SOCIAL_EMPRESA = dadosEmpresa.RAZAO_SOCIAL_EMPRESA;
                    dadosDaEmpresa.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    dadosDaEmpresa.EMAIL1_EMPRESA = dadosEmpresa.EMAIL1_EMPRESA;
                    dadosDaEmpresa.TELEFONE1_EMPRESA_USUARIO = dadosEmpresa.TELEFONE1_EMPRESA_USUARIO;
                    dadosDaEmpresa.PAIS_EMPRESA_USUARIO = dadosEmpresa.PAIS_EMPRESA_USUARIO;
                    dadosDaEmpresa.ENDERECO_EMPRESA_USUARIO = dadosEmpresa.enderecos_empresa_usuario.TIPO_LOGRADOURO_EMPRESA_USUARIO + " " + dadosEmpresa.enderecos_empresa_usuario.LOGRADOURO_CEP_EMPRESA_USUARIO;
                    dadosDaEmpresa.CEP_SEQUENCIAL_ENDERECO = String.Format("{0:00000-000}", dadosEmpresa.enderecos_empresa_usuario.CEP_ENDERECO_EMPRESA_USUARIO);
                    dadosDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = dadosEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    dadosDaEmpresa.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO = dadosEmpresa.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO;
                    dadosDaEmpresa.NOME_BAIRRO_EMPRESA_USUARIO = dadosEmpresa.enderecos_empresa_usuario.bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO;
                    dadosDaEmpresa.NOME_CIDADE_EMPRESA_USUARIO = dadosEmpresa.enderecos_empresa_usuario.cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO;
                    dadosDaEmpresa.NOME_ESTADO_EMPRESA_USUARIO = dadosEmpresa.enderecos_empresa_usuario.cidades_empresa_usuario.estados_empresa_usuario.UF_EMPRESA_USUARIO;
                    dadosDaEmpresa.ID_ESTADOS_EMPRESA_USUARIO = dadosEmpresa.enderecos_empresa_usuario.cidades_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO;

                    //POPULAR VIEW MODEL - DADOS USUARIO
                    if (dadosUsuarioEmpresa.CPF_USUARIO_EMPRESA != "56498667591")
                    {
                        dadosDaEmpresa.CPF_USUARIO = FormatarCpfCnpj(dadosUsuarioEmpresa.CPF_USUARIO_EMPRESA);
                    }
                    else
                    {
                        dadosDaEmpresa.CPF_USUARIO = "";
                    }

                    dadosDaEmpresa.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;
                    dadosDaEmpresa.APELIDO_USUARIO = dadosUsuarioEmpresa.NICK_NAME_USUARIO;
                    dadosDaEmpresa.EMAIL1_USUARIO = dadosLoginEmpresaUsuario.EMAIL1_USUARIO;
                    dadosDaEmpresa.TELEFONE1_USUARIO_EMPRESA = dadosUsuarioEmpresa.TELEFONE1_USUARIO_EMPRESA;
                    dadosDaEmpresa.TELEFONE2_USUARIO_EMPRESA = dadosUsuarioEmpresa.TELEFONE2_USUARIO_EMPRESA;

                    dadosDaEmpresa.ListagemPaises = ListagemPaises();
                    dadosDaEmpresa.ListagemEstados = ListagemEstados();

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Configurações Iniciais";

                    if (one == "N")
                    {
                        ViewBag.primeiroAcesso = one;
                    }
                    else
                    {
                        ViewBag.primeiroAcesso = "S";
                    }

                    return View(dadosDaEmpresa);
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

        public ActionResult DataLocation()
        {
            try
            {
                if (Session["IdUsuarioLogado"] != null)
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NEmpresaUsuarioLoginsService negociosEmpresaUsuarioLogins = new NEmpresaUsuarioLoginsService();

                    CadastroEmpresaViewModel dadosDaEmpresa = new CadastroEmpresaViewModel();

                    empresa_usuario dadosEmpresa = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]) });
                    usuario_empresa dadosUsuarioEmpresa = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Session["IdUsuarioLogado"]));
                    empresa_usuario_logins dadosLoginEmpresaUsuario =
                        negociosEmpresaUsuarioLogins.ConsultarDadosDeContatoDoUsuario(Convert.ToInt32(Session["IdUsuarioLogado"]));

                    dadosDaEmpresa.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    dadosDaEmpresa.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;
                    dadosDaEmpresa.ListagemRamosComercioAtacadista = ListagemDeDeRamosComercioAtacadista();
                    dadosDaEmpresa.ListagemRamosComercioVarejista = ListagemDeDeRamosComercioVarejista();
                    dadosDaEmpresa.ID_GRUPO_ATIVIDADES_ATACADO = dadosEmpresa.ID_GRUPO_ATIVIDADES_ATACADO;
                    dadosDaEmpresa.ID_GRUPO_ATIVIDADES_VAREJO = dadosEmpresa.ID_GRUPO_ATIVIDADES_VAREJO;

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Configurações Iniciais > Dados Localização";

                    if ((dadosEmpresa.ID_GRUPO_ATIVIDADES_ATACADO > 0) && (dadosEmpresa.ID_GRUPO_ATIVIDADES_VAREJO > 0))
                    {
                        ViewBag.tipoAtuacao = "CF";
                        ViewBag.checadoCF = "checked";
                        ViewBag.checadoF = "";
                        ViewBag.checadoC = "";
                    }
                    else if ((dadosEmpresa.ID_GRUPO_ATIVIDADES_ATACADO > 0))
                    {
                        ViewBag.tipoAtuacao = "F";
                        ViewBag.checadoCF = "";
                        ViewBag.checadoF = "checked";
                        ViewBag.checadoC = "";
                    }
                    //else if (dadosEmpresa.ID_GRUPO_ATIVIDADES_VAREJO > 0)
                    //{

                    //}
                    else
                    {
                        ViewBag.tipoAtuacao = "C";
                        ViewBag.checadoCF = "";
                        ViewBag.checadoF = "";
                        ViewBag.checadoC = "checked";
                    }

                    return View(dadosDaEmpresa);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        public ActionResult PaymentData()
        {
            try
            {
                if (Session["IdUsuarioLogado"] != null)
                {
                    DateTime dataHoje = DateTime.Today;

                    //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
                    string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
                    diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

                    int diaDoMes = dataHoje.Day;
                    string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
                    mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
                    int anoAtual = dataHoje.Year;

                    NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
                    NUsuarioEmpresaService negociosUsuarioEmpresa = new NUsuarioEmpresaService();
                    NEmpresaUsuarioLoginsService negociosEmpresaUsuarioLogins = new NEmpresaUsuarioLoginsService();

                    CadastroEmpresaViewModel dadosDaEmpresa = new CadastroEmpresaViewModel();

                    empresa_usuario dadosEmpresa = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]) });
                    usuario_empresa dadosUsuarioEmpresa = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Session["IdUsuarioLogado"]));
                    empresa_usuario_logins dadosLoginEmpresaUsuario =
                        negociosEmpresaUsuarioLogins.ConsultarDadosDeContatoDoUsuario(Convert.ToInt32(Session["IdUsuarioLogado"]));

                    dadosDaEmpresa.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    dadosDaEmpresa.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;
                    dadosDaEmpresa.ListagemRamosComercioAtacadista = ListagemDeDeRamosComercioAtacadista();
                    dadosDaEmpresa.ListagemRamosComercioVarejista = ListagemDeDeRamosComercioVarejista();
                    dadosDaEmpresa.ID_GRUPO_ATIVIDADES_ATACADO = dadosEmpresa.ID_GRUPO_ATIVIDADES_ATACADO;
                    dadosDaEmpresa.ID_GRUPO_ATIVIDADES_VAREJO = dadosEmpresa.ID_GRUPO_ATIVIDADES_VAREJO;
                    dadosDaEmpresa.APELIDO_USUARIO = dadosUsuarioEmpresa.NICK_NAME_USUARIO;

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.dataHojeFormatada = dataHoje.Day + "/" + dataHoje.Month + "/" + dataHoje.Year;
                    ViewBag.dataVencimentoPeriodoGratuito = dataHoje.AddDays(60).Day + "/" + dataHoje.AddDays(60).Month + "/" + dataHoje.AddDays(60).Year;
                    ViewBag.ondeEstouAgora = "Configurações Iniciais > Dados Financeiros";

                    return View(dadosDaEmpresa);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return null;
        }

        //Carrega lista de Países atendidos pelo ClienteMercado
        private static List<SelectListItem> ListagemPaises()
        {
            //Buscar lista de Países
            NPaisesService negocioPaises = new NPaisesService();

            List<paises_empresa_usuario> listaPaises = negocioPaises.ListaPaises();

            List<SelectListItem> listPaises = new List<SelectListItem>();

            foreach (var grupoPaises in listaPaises)
            {
                listPaises.Add(new SelectListItem
                {
                    Text = grupoPaises.PAIS_EMPRESA_USUARIO,
                    Value = grupoPaises.ID_PAISES_EMPRESA_USUARIO.ToString()
                });
            }

            return listPaises;
        }

        //Carrega a lista de Estados (Obs: No momento carregrá todos os estados brasileiros. Depois vejo como ficará)
        private List<SelectListItem> ListagemEstados()
        {
            //Buscar lista de Estados brasileiros
            NEstadosService negocioEstados = new NEstadosService();

            List<estados_empresa_usuario> listaEstados = negocioEstados.ListaEstados();

            List<SelectListItem> listEstados = new List<SelectListItem>();

            listEstados.Add(new SelectListItem { Text = "", Value = "" });

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

        //CARREGA TERMOS
        public ActionResult Termos()
        {
            DateTime dataHoje = DateTime.Today;

            //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
            string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
            diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

            int diaDoMes = dataHoje.Day;
            string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
            mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
            int anoAtual = dataHoje.Year;

            //VIEWBAGS
            ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
            ViewBag.ondeEstouAgora = "Termos";

            return View();
        }

        //CARREGA POLÍTICA
        public ActionResult Politica()
        {
            DateTime dataHoje = DateTime.Today;

            //Montando o dia por extenso a se exibido no site (* Não se usa mais isso)
            string diaDaSemana = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dataHoje.DayOfWeek);
            diaDaSemana = char.ToUpper(diaDaSemana[0]) + diaDaSemana.Substring(1);

            int diaDoMes = dataHoje.Day;
            string mesAtual = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(dataHoje.Month);
            mesAtual = char.ToUpper(mesAtual[0]) + mesAtual.Substring(1);
            int anoAtual = dataHoje.Year;

            //VIEWBAGS
            ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
            ViewBag.ondeEstouAgora = "Políticas";

            return View();
        }

        //FORMATAR CPF/CNPJ
        public static string FormatarCpfCnpj(string strCpfCnpj)
        {
            if (strCpfCnpj.Length <= 11)
            {
                MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(ZerosEsquerda(strCpfCnpj, 11));

                return mtpCpf.ToString();
            }
            else
            {
                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(ZerosEsquerda(strCpfCnpj, 11));

                return mtpCnpj.ToString();
            }
        }

        //ACRESCENTA ZEROS À ESQUERDA
        public static string ZerosEsquerda(string strString, int intTamanho)
        {
            string strResult = "";

            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {
                strResult += "0";
            }

            return strResult + strString;
        }

        //Busca dados completos do endereço pelo CEP digitado
        [WebMethod]
        public ActionResult BuscaEndereco(string cepDigitado)
        {
            if (ModelState.IsValid)
            {
                cepDigitado = Regex.Replace(cepDigitado, "[.-]", "");

                try
                {
                    NCepService negocio = new NCepService();
                    enderecos_empresa_usuario buscaCepAfins = new enderecos_empresa_usuario();

                    buscaCepAfins.CEP_ENDERECO_EMPRESA_USUARIO = Convert.ToInt64(cepDigitado);
                    enderecos_empresa_usuario enderecos_empresa_usuario = negocio.ConsultarCep(buscaCepAfins);

                    if (enderecos_empresa_usuario != null)
                    {
                        var enderecoCep =
                            new
                            {
                                id_endereco = enderecos_empresa_usuario.ID_CODIGO_ENDERECO_EMPRESA_USUARIO,
                                endereco = enderecos_empresa_usuario.TIPO_LOGRADOURO_EMPRESA_USUARIO + " " + enderecos_empresa_usuario.LOGRADOURO_CEP_EMPRESA_USUARIO,
                                estado = enderecos_empresa_usuario.cidades_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO,
                                cidade = enderecos_empresa_usuario.cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO,
                                bairro = enderecos_empresa_usuario.bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO
                            };

                        return Json(enderecoCep, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception erro)
                {
                    //ModelState.AddModelError("", "Erro ao checar CEP");
                    throw erro;
                }
            }

            return Json(cepDigitado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //ATUALIZAR DADOS da EMPRESA e do USUÁRIO
        public ActionResult CompletarDadosEmpresaEUsuario(string cnpjEmpresa, string razaoSocialEmpresa, string nomeFantasiaEmpresa, string email1Empresa, string telefone1Empresa,
            int idPais, int idEnderecoEmpresa, string complementoEndEmpresa, string cpfUsuario, string nomeUsuario, string apelidoUsuario, string emai1lUsuario,
            string telefone1Usuario, string telefone2Usuario, string receberEmailsEmpresa)
        {
            var resultado = new { dadosAtualizados = "nOk" };

            NEmpresaUsuarioService negociosEmpresa = new NEmpresaUsuarioService();
            NUsuarioEmpresaService negociosUsuario = new NUsuarioEmpresaService();
            empresa_usuario dadosDaEmpresa = new empresa_usuario();
            usuario_empresa dadosUsuario = new usuario_empresa();
            empresa_usuario dadosEmpresaAtualizados = new empresa_usuario();
            usuario_empresa dadosUsuarioAtualizados = new usuario_empresa();

            /*
             CONTINUAR AQUI... OBS: VERIFICAR DEPOIS QUE DEFINIR PELA BUSCA DE CEPS NOS CORREIOS, como serão atualizados estes campos nas em USUARIO e EMPRESA.
             */

            //POPULAR DADOS da EMPRESA para ATUALIZAR
            dadosDaEmpresa.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
            dadosDaEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO = 1;
            dadosDaEmpresa.RAZAO_SOCIAL_EMPRESA = razaoSocialEmpresa;

            if ((nomeFantasiaEmpresa != null) && (nomeFantasiaEmpresa != ""))
            {
                dadosDaEmpresa.NOME_FANTASIA_EMPRESA = nomeFantasiaEmpresa;
            }

            dadosDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = idEnderecoEmpresa;
            dadosDaEmpresa.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO = complementoEndEmpresa;
            dadosDaEmpresa.EMAIL1_EMPRESA = email1Empresa;
            dadosDaEmpresa.TELEFONE1_EMPRESA_USUARIO = telefone1Empresa;

            if (idPais > 0)
            {
                dadosDaEmpresa.PAIS_EMPRESA_USUARIO = idPais;
            }

            dadosDaEmpresa.DATA_ULTIMA_ATUALIZACAO_EMPRESA = DateTime.Now;

            //GRAVAR DADOS ATUALIZADOS da EMPRESA
            dadosEmpresaAtualizados = negociosEmpresa.AtualizarDadosCadastrais(dadosDaEmpresa);

            if (dadosEmpresaAtualizados != null)
            {
                //POPULAR DADOS do USUÁRIO
                dadosUsuario.ID_CODIGO_USUARIO = Convert.ToInt32(Sessao.IdUsuarioLogado);
                dadosUsuario.NOME_USUARIO = nomeUsuario;

                if ((apelidoUsuario != null) && (apelidoUsuario != ""))
                {
                    dadosUsuario.NICK_NAME_USUARIO = apelidoUsuario;
                }

                if ((cpfUsuario != null) && (cpfUsuario != ""))
                {
                    cpfUsuario = Regex.Replace(cpfUsuario, "[./-]", "");
                    dadosUsuario.CPF_USUARIO_EMPRESA = cpfUsuario;
                }

                if (idPais > 0)
                {
                    dadosUsuario.PAIS_USUARIO_EMPRESA = idPais;
                }

                dadosUsuario.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = idEnderecoEmpresa;
                dadosUsuario.TELEFONE1_USUARIO_EMPRESA = telefone1Usuario;
                dadosUsuario.TELEFONE2_USUARIO_EMPRESA = telefone2Usuario;
                dadosUsuario.DATA_ULTIMA_ATUALIZACAO_USUARIO = DateTime.Now;

                //GRAVAR DADOS ATUALIZADOS do USUÁRIO
                dadosUsuarioAtualizados = negociosUsuario.AtualizarDadosCadastrais(dadosUsuario);
            }

            if ((dadosEmpresaAtualizados != null) && (dadosUsuarioAtualizados != null))
            {
                resultado = new { dadosAtualizados = "Ok" };
            }

            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //Verifica se o CPF digitado já existe no banco
        [WebMethod]
        public ActionResult ValidacaoDoCPF(string cpfDigitado)
        {
            try
            {
                var resultado = new { cpfValido = "nOk" };

                cpfDigitado = Regex.Replace(cpfDigitado, "[.-]", "");

                bool cpfValido = Validacoes.ValidaCPF(cpfDigitado);

                if (cpfValido)
                {
                    resultado = new { cpfValido = "Ok" };
                }

                return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //ATUALIZAR DADOS da EMPRESA e do USUÁRIO
        public ActionResult AtualizarDadosDeLocalizacaoNoSistema(int codAtacadista, int codVarejista)
        {
            var resultado = new { dadosDeLocalizacaoAtualizados = "nOk" };

            NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
            empresa_usuario dadosEmpresa = new empresa_usuario();

            //POPULAR os CAMPOS do OBJ
            if (codAtacadista >= 0)
            {
                dadosEmpresa.ID_GRUPO_ATIVIDADES_ATACADO = codAtacadista;
            }

            if (codVarejista >= 0)
            {
                dadosEmpresa.ID_GRUPO_ATIVIDADES_VAREJO = codVarejista;
            }

            //ATUALIZAR os DADOS da EMPRESA com as informações de LOCALIZAÇÃO no SISTEMA
            if ((codAtacadista >= 0) || (codVarejista >= 0))
            {
                empresa_usuario dadosAAtualizar = negociosEmpresaUsuario.AtualizarDadosCadastrais(dadosEmpresa);
                resultado = new { dadosDeLocalizacaoAtualizados = "Ok" };
            }

            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //ATUALIZAR DADOS da EMPRESA e do USUÁRIO
        public ActionResult ComecarGratuidadeNoSistema(string dataFinalGratuidade)
        {
            var resultado = new { dataFinalGratuidadeRegistrada = "nOk" };

            NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();
            empresa_usuario dadosEmpresaUsuario = new empresa_usuario();

            //POPULAR OBJ
            dadosEmpresaUsuario.DATA_FINAL_GRATUIDADE_EMPRESA = Convert.ToDateTime(dataFinalGratuidade);

            //REGISTRAR INÍCIO da GRATUIDADE no SISTEMA
            empresa_usuario dadosAtualizadosDaEmpresa = negociosEmpresaUsuario.IniciarGRatuidadeNoSistema(dadosEmpresaUsuario);

            if (dadosAtualizadosDaEmpresa != null)
            {
                resultado = new { dataFinalGratuidadeRegistrada = "Ok" };
            }

            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
    }
}
