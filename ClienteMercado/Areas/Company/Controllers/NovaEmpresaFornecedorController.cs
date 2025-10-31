using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class NovaEmpresaFornecedorController : Controller
    {
        //
        // GET: /Company/NovaEmpresaFornecedor/

        public ActionResult CriarNovaEmpresaFornecedor()
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
                    dadosLocalizacao dadosLocalizacao = new dadosLocalizacao();
                    empresa_usuario dadosEmpresa = new NEmpresaUsuarioService().ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario) });
                    usuario_empresa dadosUsuarioEmpresa = new NUsuarioEmpresaService().ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(Convert.ToInt32(Sessao.IdEmpresaUsuario));
                    EMPRESA_FORNECEDOR dadosEmpresaFornecedor = new EMPRESA_FORNECEDOR();
                    USUARIO_FORNECEDOR dadosUsuFornecedor = new USUARIO_FORNECEDOR();
                    grupo_atividades_empresa dadosGruposAtividadesEmpresa = new grupo_atividades_empresa();

                    //POPULAR VIEW MODEL
                    if (dadosEmpresa == null)
                    {
                        dadosEmpresaFornecedor = new NEmpresaFornecedorService().ConsultarDadosEmpresaFornecedor(Convert.ToInt32(Session["IdEmpresaUsuario"]));
                        dadosUsuFornecedor = new NUsuarioFornecedorService().ConsultarDadosUsuarioMasterEmpForn(Convert.ToInt32(Session["IdUsuarioLogado"]));
                    }

                    if (dadosEmpresa != null)
                    {
                        dadosGruposAtividadesEmpresa = new NGruposAtividadesEmpresaProfissionalService().ConsultarDadosDoGrupoDeAtividadesDaEmpresa(new grupo_atividades_empresa { ID_GRUPO_ATIVIDADES = dadosEmpresa.ID_GRUPO_ATIVIDADES });
                        dadosLocalizacao = new NEnderecoEmpresaUsuarioService().ConsultarDadosDaLocalizacaoPeloCodigo2(dadosEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO);
                    }
                    else
                    {
                        dadosGruposAtividadesEmpresa = 
                            new NGruposAtividadesEmpresaProfissionalService().ConsultarDadosDoGrupoDeAtividadesDaEmpresaFornecedor(new grupo_atividades_empresa { DESCRICAO_ATIVIDADE = dadosEmpresaFornecedor.ramo_atividade_empresa_fornecedor });
                    }

                    viewModelCC.NOME_FANTASIA_EMPRESA = (dadosEmpresa != null) ? dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper() : dadosEmpresaFornecedor.nome_fantasia_empresa_fornecedor.ToUpper();
                    viewModelCC.NOME_USUARIO = (dadosEmpresa != null) ? dadosUsuarioEmpresa.NOME_USUARIO : dadosUsuFornecedor.nome_usuario_fornecedor;
                    viewModelCC.inRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.DESCRICAO_ATIVIDADE;
                    viewModelCC.inCidadeEmpresaAdmCC = (dadosEmpresa != null) ? (dadosLocalizacao.CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacao.UF_EMPRESA_USUARIO) : dadosEmpresaFornecedor.cidade_empresa_fornecedor + "-" + dadosEmpresaFornecedor.uf_empresa_fornecedor;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inListaDeRamosDeAtividadeParaComprasDaCC = ListagemDeDeRamosComercioVarejista();
                    viewModelCC.inListaUFs = ListagemUFs();

                    //VIEWBAGS
                    ViewBag.codRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.ID_GRUPO_ATIVIDADES;
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Cadastro Empresa Fornecedor";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Carrega lista de Ramos de Comércio Varejista
        private static List<SelectListItem> ListagemDeDeRamosComercioVarejista()
        {
            //Buscar lista de TIPOS COMÉRCIO
            List<grupo_atividades_empresa> listaCategoriasVarejistas = new NGruposAtividadesEmpresaProfissionalService().CarregarListaDeCategoriasVarejistasNew();
            List<SelectListItem> listCategorias = new List<SelectListItem>();

            listCategorias.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

            foreach (var grupoCategoarias in listaCategoriasVarejistas)
            {
                listCategorias.Add(new SelectListItem
                {
                    Text = grupoCategoarias.DESCRICAO_ATIVIDADE,
                    Value = grupoCategoarias.DESCRICAO_ATIVIDADE.ToString()
                });
            }

            return listCategorias;
        }

        //Carrega lista Estados do Brasil
        private static List<SelectListItem> ListagemUFs()
        {
            //Buscar lista de TIPOS COMÉRCIO
            List<estados_empresa_usuario> listaEstados = new NEstadosService().ListaEstados();
            List<SelectListItem> listUFs = new List<SelectListItem>();

            listUFs.Add(new SelectListItem { Text = "Selecione...", Value = "0" });

            foreach (var grupoEstados in listaEstados)
            {
                listUFs.Add(new SelectListItem
                {
                    Text = grupoEstados.UF_EMPRESA_USUARIO,
                    Value = grupoEstados.UF_EMPRESA_USUARIO.ToString()
                });
            }

            return listUFs;
        }

        public JsonResult RegistrarEmpresaFornecedor(string cnpjEmpresa, string nomeEmpresa, string ramoAtividade, string enderecoEmpresa, string bairroEmpresa, string compEnderecoEmpresa, 
            string cepEmpresa, string cidadeEmpresa, string ufEmpresa, string tipoG)
        {
            try
            {
                var msgRet = "";
                int id = 0;
                var result = new { msg = "", id = 0 };
                EMPRESA_FORNECEDOR dadosEmpresaFornecedor = new EMPRESA_FORNECEDOR();

                if (tipoG == "N")
                {
                    //Populando o objeto para a gravação dos dados de uma NOVA Empresa Fornecedora
                    dadosEmpresaFornecedor.cnpj_empresa_fornecedor = Regex.Replace(cnpjEmpresa, "[./-]", "");
                    dadosEmpresaFornecedor.nome_fantasia_empresa_fornecedor = nomeEmpresa;
                    dadosEmpresaFornecedor.data_cadastro_empresa_fornecedor = DateTime.Now;
                    dadosEmpresaFornecedor.empresa_ativa_empresa_fornecedor = true;
                    dadosEmpresaFornecedor.data_inativacao_empresa_fornecedor = Convert.ToDateTime("01-01-1900");
                    dadosEmpresaFornecedor.endereco_empresa_fornecedor = enderecoEmpresa;
                    dadosEmpresaFornecedor.complemento_empresa_fornecedor = compEnderecoEmpresa;
                    dadosEmpresaFornecedor.cep_empresa_fornecedor = cepEmpresa;
                    dadosEmpresaFornecedor.cidade_empresa_fornecedor = cidadeEmpresa;
                    dadosEmpresaFornecedor.uf_empresa_fornecedor = ufEmpresa;
                    dadosEmpresaFornecedor.bairro_empresa_fornecedor = bairroEmpresa;
                    dadosEmpresaFornecedor.ramo_atividade_empresa_fornecedor = ramoAtividade;
                    dadosEmpresaFornecedor.emp_adm_soft = false;

                    //GRAVAR NOVA Empresa Fornecedora
                    EMPRESA_FORNECEDOR novaEmpresa = new NEmpresaFornecedorService().GravarNovaEmpresaFornecedor(dadosEmpresaFornecedor);
                    id = novaEmpresa.id_empresa_fornecedor;
                    msgRet = (novaEmpresa != null) ? "Ok" : "NOk";
                }
                else if (tipoG == "E")
                {
                    //Populando o objeto para a gravação dos dados EDITADOS da Empresa Fornecedora
                    dadosEmpresaFornecedor.cnpj_empresa_fornecedor = Regex.Replace(cnpjEmpresa, "[./-]", "");
                    dadosEmpresaFornecedor.nome_fantasia_empresa_fornecedor = nomeEmpresa;
                    //dadosEmpresaFornecedor.data_cadastro_empresa_fornecedor = DateTime.Now;
                    dadosEmpresaFornecedor.empresa_ativa_empresa_fornecedor = true;
                    //dadosEmpresaFornecedor.data_inativacao_empresa_fornecedor = Convert.ToDateTime("01-01-1900");
                    dadosEmpresaFornecedor.endereco_empresa_fornecedor = enderecoEmpresa;
                    dadosEmpresaFornecedor.complemento_empresa_fornecedor = compEnderecoEmpresa;
                    dadosEmpresaFornecedor.cep_empresa_fornecedor = cepEmpresa;
                    dadosEmpresaFornecedor.cidade_empresa_fornecedor = nomeEmpresa;
                    dadosEmpresaFornecedor.uf_empresa_fornecedor = ufEmpresa;
                    dadosEmpresaFornecedor.bairro_empresa_fornecedor = bairroEmpresa;
                    dadosEmpresaFornecedor.ramo_atividade_empresa_fornecedor = ramoAtividade;

                    //GRAVAR dados EDITADOS da Empresa Fornecedora
                    new NEmpresaFornecedorService().GravarDadosEditadosEmpresaFornecedor(dadosEmpresaFornecedor);
                    msgRet = "Ok";
                }

                result = new { msg = msgRet, id = id };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult CadastroUsuarioEmpresaFornecedor(int id = 0)
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
                    USUARIO_FORNECEDOR dadosUsuarioMasterForn = new NUsuarioFornecedorService().ConsultarDadosUsuarioEmpresaFornecedor(id);

                    //POPULAR VIEW MODEL
                    viewModelCC.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;
                    viewModelCC.inEmpresaLogadaAdmCC = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    viewModelCC.inCidadeEmpresaAdmCC = dadosLocalizacao[0].CIDADE_EMPRESA_USUARIO + "-" + dadosLocalizacao[0].UF_EMPRESA_USUARIO;
                    viewModelCC.inNomeUsuarioRespEmpresaAdmCC = dadosUsuarioEmpresa.NOME_USUARIO;
                    viewModelCC.inDataCriacaoCC = diaDoMes + "/" + mesDoAno + "/" + anoAtual;
                    viewModelCC.inRamoAtividadeEmpresaAdm = dadosGruposAtividadesEmpresa.DESCRICAO_ATIVIDADE;
                    viewModelCC.inDataCadastroUsuario = (dadosUsuarioMasterForn != null) ? (dadosUsuarioMasterForn.data_cadastro_empresa_fornecedor.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)) 
                        : (diaDoMes + "/" + mesDoAno + "/" + anoAtual);
                    viewModelCC.inCPF = (dadosUsuarioMasterForn != null) ? ClienteMercado.Utils.Utilitarios.Mascaras.MascaraCnpjCpf(dadosUsuarioMasterForn.cpf_usuario_fornecedor) : "";
                    viewModelCC.inNomeUsuarioEmpresa = (dadosUsuarioMasterForn != null) ? dadosUsuarioMasterForn.nome_usuario_fornecedor : "";
                    viewModelCC.inEmailUsuario = (dadosUsuarioMasterForn != null) ? dadosUsuarioMasterForn.email_usuario_empresa_fornecedor : "";
                    viewModelCC.inLogin = (dadosUsuarioMasterForn != null) ? dadosUsuarioMasterForn.login_usuario_empresa_fornecedor : "";
                    viewModelCC.inSenha = (dadosUsuarioMasterForn != null) ? dadosUsuarioMasterForn.passw_usuario_empresa_fornecedor : "";

                    //VIEWBAGS
                    ViewBag.inEdit = (dadosUsuarioMasterForn != null) ? "E": "N";
                    ViewBag.idEmp = id;
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Cadastro Usuário Empresa Fornecedor";

                    return View(viewModelCC);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //========================================================================
        public JsonResult RegistrarUsuEmpresaFornecedor(string cpfUsu, string nomeUsu, int idEmp, string log, string pass, string tipoG, string emailUsu)
        {
            var msgRet = "";
            USUARIO_FORNECEDOR usuADMEmpresaFornecedor = new USUARIO_FORNECEDOR();

            if (tipoG == "N")
            {
                //POPULANDO OBJ relacionado ao Novo Usuário
                usuADMEmpresaFornecedor.id_empresa_fornecedor = idEmp;
                usuADMEmpresaFornecedor.cpf_usuario_fornecedor = Regex.Replace(cpfUsu, "[./-]", "");
                usuADMEmpresaFornecedor.nome_usuario_fornecedor = nomeUsu;
                usuADMEmpresaFornecedor.eh_master_usuario_fornecedor = true;
                usuADMEmpresaFornecedor.usuario_ativo_usuario_fornecedor = true;
                usuADMEmpresaFornecedor.data_inativacao_usuario_fornecedor = Convert.ToDateTime("01-01-1900");
                usuADMEmpresaFornecedor.data_cadastro_empresa_fornecedor = DateTime.Now;
                usuADMEmpresaFornecedor.login_usuario_empresa_fornecedor = log;
                usuADMEmpresaFornecedor.passw_usuario_empresa_fornecedor = Utils.Utilitarios.Hash.GerarHashMd5(pass);
                usuADMEmpresaFornecedor.email_usuario_empresa_fornecedor = emailUsu;

                USUARIO_FORNECEDOR dadosUsu = new NUsuarioFornecedorService().GravarNovoUsuarioAdmEmpresaFornecedor(usuADMEmpresaFornecedor);
                if (dadosUsu != null)
                    msgRet = "Ok";
            }
            else if (tipoG == "E")
            {
                //GRAVAR dados EDITADOS do Usuário da Empresa Fornecedora
                usuADMEmpresaFornecedor.cpf_usuario_fornecedor = Regex.Replace(cpfUsu, "[./-]", "");
                usuADMEmpresaFornecedor.nome_usuario_fornecedor = nomeUsu;
                usuADMEmpresaFornecedor.eh_master_usuario_fornecedor = true;
                usuADMEmpresaFornecedor.usuario_ativo_usuario_fornecedor = true;
                usuADMEmpresaFornecedor.login_usuario_empresa_fornecedor = log;
                usuADMEmpresaFornecedor.email_usuario_empresa_fornecedor = emailUsu;

                new NUsuarioFornecedorService().GravarDadosEditadosDoUsuarioEmpresaFornecedor(usuADMEmpresaFornecedor);
                msgRet = "Ok";
            }

            var result = new { msg = msgRet };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //========================================================================
    }
}
