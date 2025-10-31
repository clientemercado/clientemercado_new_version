using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DFinanceiroCobrancaFaturamentoUsuarioEmpresaRepository
    {
        //Gravando cobrança ClienteMercado para o Usuário ou Empresa
        public financeiro_cobranca_faturamento_usuario_empresa GravarCobrancaUsuarioEmpresa(financeiro_cobranca_faturamento_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                financeiro_cobranca_faturamento_usuario_empresa financeiroCobrancaFaturamentoUsuarioEmpresa =
                    _contexto.financeiro_cobranca_faturamento_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return financeiroCobrancaFaturamentoUsuarioEmpresa;
            }
        }

        //Consuta a existencia de Títulos em aberto para Empresa ou Usuário, ao se logar
        public financeiro_cobranca_faturamento_usuario_empresa ConsultarTitulosEmAbertoParaEmpresasEUsuarios(financeiro_cobranca_faturamento_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                financeiro_cobranca_faturamento_usuario_empresa financeiroCobrancaFaturamentoUsuarioEmpresa =
                    _contexto.financeiro_cobranca_faturamento_usuario_empresa.FirstOrDefault(m => m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO) &&
                    m.ID_CODIGO_EMPRESA.Equals(m.ID_CODIGO_EMPRESA) && m.PARCELA_PAGA_COBRANCA_FATURAMENTO == false);

                return financeiroCobrancaFaturamentoUsuarioEmpresa;
            }
        }
    }
}
