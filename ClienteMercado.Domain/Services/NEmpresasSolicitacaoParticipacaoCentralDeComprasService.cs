using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresasSolicitacaoParticipacaoCentralDeComprasService
    {
        DEmpresasSolicitacaoParticipacaoCentralDeComprasRepository dRepository = new DEmpresasSolicitacaoParticipacaoCentralDeComprasRepository();

        //REGISTRAR SOLICITAÇÃO de PARTICIPAÇÃO na CC
        public bool RegistrarPedidoDeParticipacaoNaCC(empresas_solicitacao_participacao_central_de_compras obj)
        {
            return dRepository.RegistrarPedidoDeParticipacaoNaCC(obj);
        }

        //VERIFICAR se a EMPRESA LOGADA possui SOLICITAÇÃO de PARTICIPAÇÂO na CENTRAL de COMPRAS
        public empresas_solicitacao_participacao_central_de_compras BuscaSolicitacaoDeParticipacaoNaCCDaEmpresaLogada(int? idCC)
        {
            return dRepository.BuscaSolicitacaoDeParticipacaoNaCCDaEmpresaLogada(idCC);
        }
    }
}
