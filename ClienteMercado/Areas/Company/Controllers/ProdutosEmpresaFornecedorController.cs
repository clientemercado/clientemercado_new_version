using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class ProdutosEmpresaFornecedorController : Controller
    {
        //
        // GET: /Company/ProdutosEmpresaFornecedor/

        public ActionResult CadastroDosProdutosEmpresaFornecedora()
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

    }
}
