using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.UI.Core.ViewModel;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace ClienteMercado.Areas.Company.Controllers
{
    public class CotacaoAvulsaController : Controller
    {
        //
        // GET: /Company/CotacaoAvulsa/

        public ActionResult Index()
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
                    DadosEmpresaEUsuarioViewModel dadosDaEmpresa = new DadosEmpresaEUsuarioViewModel();

                    empresa_usuario dadosEmpresa = negociosEmpresaUsuario.ConsultarDadosDaEmpresa(new empresa_usuario { ID_CODIGO_EMPRESA = Convert.ToInt32(Session["IdEmpresaUsuario"]) });
                    usuario_empresa dadosUsuarioEmpresa = negociosUsuarioEmpresa.ConsultarDadosDoUsuarioDaEmpresa(Convert.ToInt32(Session["IdUsuarioLogado"]));

                    //POPULAR VIEW MODEL
                    dadosDaEmpresa.NOME_FANTASIA_EMPRESA = dadosEmpresa.NOME_FANTASIA_EMPRESA.ToUpper();
                    dadosDaEmpresa.NOME_USUARIO = dadosUsuarioEmpresa.NOME_USUARIO;

                    //VIEWBAGS
                    ViewBag.dataHoje = diaDaSemana + ", " + diaDoMes + " de " + mesAtual + " de " + anoAtual;
                    ViewBag.ondeEstouAgora = "Cotação Avulsa";

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

    }
}
