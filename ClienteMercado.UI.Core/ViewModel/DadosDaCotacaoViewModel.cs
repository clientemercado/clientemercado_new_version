using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class DadosDaCotacaoViewModel : NovaCentralDeComprasViewModel
    {
        public string NOME_USUARIO { get; set; }
        public string NOME_FANTASIA_EMPRESA { get; set; }
        public int cCC { get; set; }
        public int iCM { get; set; }
        public int iCCF { get; set; }
        public int inCodEmpresaADM { get; set; }
        public int inCodUsuariomEmpresaADM { get; set; }
        public int inCodEmpresaLogada { get; set; }
        public int inCodEmpresaFornecedoraComRespostaEmAnalise { get; set; }
        public int idEmpresaFornecedoraCotada { get; set; }
        public int idUsuarioEmpresaFornecedoraCotada { get; set; }
        public string inNomeUsuarioLogado { get; set; }
        public string inNomEmpresaAdmCC { get; set; }
        public string inNomeUsuarioRespEmpresaAdmCC { get; set; }
        public string inCidadeEmpresaAdmCC { get; set; }
        public List<ListaEstilizadaDeEmpresasViewModel> listaDeEmpresasQueAnexaramCotacaoESeusItensCotados { get; set; }
        public List<DadosEmpresasCotadasViewModel> listaDeEmpresasCotadas { get; set; }
        public List<ListaDadosProdutoCotacaoViewModel> listaDeProdutosCotados { get; set; }
        public string inNomeFantasiaEmpresaCotada { get; set; }
        public string inNomeUsuarioRespDaEmpresaCotada { get; set; }
        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE_ALTERNATIVO { get; set; }
        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }
        public string existeSolicitacaoDeConfirmacaoAprovandoRespostaDosFornecedores { get; set; }
        public string negociacaoDoAdmComFornecedoresAceita { get; set; }
        public int quantidadeEmpresasParticipantesDestaCotacao { get; set; }
        public int quantidadeEmpresasQueRegistraramOAceiteDosValoresCotados { get; set; }
        public string corQuantidadeConfirmada { get; set; }
        public string cotacaoRespondida { get; set; }
        public string naoCotouTodosOsItens { get; set; }
        public string cotacaoNegociacaoAceita { get; set; }
        public string existemCotacoesQueReceberamContraProposta { get; set; }
        public string possuiContraProposta { get; set; }
        public int inIdEmpresaCotada { get; set; }
        public string inRecebeuContraProposta { get; set; }
        public string inRespondeuContraProposta { get; set; }
        public string inAceitouContraProposta { get; set; }
        public string inRejeitouContraProposta { get; set; }
        public string todosCotantesAceitaramNegociacao { get; set; }
        public string mensagemStatus { get; set; }
        public string rejeitouSolicitacaoAprovandoValoresCotacao { get; set; }
        public string rejeitouPedido { get; set; }
        public List<SelectListItem> inListaDeFormasPagamento { get; set; }
        public List<SelectListItem> inListaTiposFrete { get; set; }
        public List<SelectListItem> inListaPedidosBaixa { get; set; }
        public string pedidoEntregueIntegralmente { get; set; }
        public string inDataEntrega { get; set; }
    }
}
