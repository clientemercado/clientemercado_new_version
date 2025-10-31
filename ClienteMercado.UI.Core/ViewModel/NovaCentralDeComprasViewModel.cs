using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class NovaCentralDeComprasViewModel
    {
        public string NOME_FANTASIA_EMPRESA { get; set; }
        public string NOME_USUARIO { get; set; }
        public string inEmpresaLogadaAdmCC { get; set; }
        public string inRamoAtividadeEmpresaAdmCC { get; set; }
        public string inCidadeEmpresaAdmCC { get; set; }
        public string inNomeUsuarioRespEmpresaAdmCC { get; set; }
        public string inNomeCentralCompras { get; set; }
        public string inDataCriacaoCC { get; set; }
        public int inCodRamoAtividadeCC { get; set; }
        public string inRamoAtividadeCC { get; set; }
        public string inRamoAtividadeEmpresaAdm { get; set; }
        public List<SelectListItem> inListaDeRamosDeAtividade { get; set; }
        public List<SelectListItem> ListagemRamosComercioAtacadista { get; set; }
        public List<SelectListItem> inListaDeRamosDeAtividadeParaComprasDaCC { get; set; }
        public int inCodRamoAtividade { get; set; }
        public int inTipoCotacaoDirecionadaCC { get; set; }
        public int inTipoCotacaoAvulsaCC { get; set; }
        public string inCCCriptografado { get; set; }
        public string ineACriptografado { get; set; }
        public List<SelectListItem> inListaUnidadesProdutosACotar { get; set; }
        public List<SelectListItem> inListaUFs { get; set; }

        public string inDataCadastroUsuario { get; set; }
        public string inCPF { get; set; }
        public string inNomeUsuarioEmpresa { get; set; }
        public string inEmailUsuario { get; set; }
        public string inLogin { get; set; }
        public string inSenha { get; set; }
    }
}
