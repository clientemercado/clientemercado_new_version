using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DFinanceiroCobrancaFaturamentoUsuarioProfissionalRepository
    {
        //Gravando cobrança ClienteMercado para o Profissional de Serviços
        public financeiro_cobranca_faturamento_usuario_profissional GravaCobrancaUsuarioProfissional(financeiro_cobranca_faturamento_usuario_profissional obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                financeiro_cobranca_faturamento_usuario_profissional financeiroCobrancaFaturamentoUsuarioProfissional =
                    _contexto.financeiro_cobranca_faturamento_usuario_profissional.Add(obj);
                _contexto.SaveChanges();

                return financeiroCobrancaFaturamentoUsuarioProfissional;
            }
        }

        //Consulta a existencia de Títulos em aberto para o Usuário, ao se logar
        public financeiro_cobranca_faturamento_usuario_profissional ConsultarTitulosEmAbertoParaOUsuarioProfissionalDeServicos(financeiro_cobranca_faturamento_usuario_profissional obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                financeiro_cobranca_faturamento_usuario_profissional financeiroCobrancaFaturamentoUsuarioProfissional =
                    _contexto.financeiro_cobranca_faturamento_usuario_profissional.FirstOrDefault(m => m.ID_CODIGO_USUARIO_PROFISSIONAL.Equals(obj.ID_CODIGO_USUARIO_PROFISSIONAL) &&
                    m.PARCELA_PAGA_COBRANCA_FATURAMENTO == false);

                return financeiroCobrancaFaturamentoUsuarioProfissional;
            }
        }
    }
}
