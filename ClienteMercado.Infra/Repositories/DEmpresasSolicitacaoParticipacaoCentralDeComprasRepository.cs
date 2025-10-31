using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresasSolicitacaoParticipacaoCentralDeComprasRepository : RepositoryBase<empresas_solicitacao_participacao_central_de_compras>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;

        //REGISTRAR SOLICITAÇÃO de PARTICIPAÇÃO na CC
        public bool RegistrarPedidoDeParticipacaoNaCC(empresas_solicitacao_participacao_central_de_compras obj)
        {
            bool solicitacaoRegistrada = false;

            empresas_solicitacao_participacao_central_de_compras solicitacaoAnteriorParaEstaCC =
                _contexto.empresas_solicitacao_participacao_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == obj.ID_CENTRAL_COMPRAS) && (m.ID_CODIGO_EMPRESA_SOLICITANTE == obj.ID_CODIGO_EMPRESA_SOLICITANTE)));

            if (solicitacaoAnteriorParaEstaCC == null)
            {
                _contexto.empresas_solicitacao_participacao_central_de_compras.Add(obj);
                _contexto.SaveChanges();

                solicitacaoRegistrada = true;
            }

            return solicitacaoRegistrada;
        }

        //VERIFICAR se a EMPRESA LOGADA possui SOLICITAÇÃO de PARTICIPAÇÂO na CENTRAL de COMPRAS
        public empresas_solicitacao_participacao_central_de_compras BuscaSolicitacaoDeParticipacaoNaCCDaEmpresaLogada(int? idCC)
        {
            empresas_solicitacao_participacao_central_de_compras dadosSolicitacaoDaEmpresa =
                _contexto.empresas_solicitacao_participacao_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == idCC) && (m.ID_CODIGO_EMPRESA_SOLICITANTE == idEmpresa)));

            return dadosSolicitacaoDaEmpresa;
        }
    }
}
