using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NFinanceiroCobrancaFaturamentoUsuarioProfissionalService
    {
        DFinanceiroCobrancaFaturamentoUsuarioProfissionalRepository dfinanceirocobrancafaturamentousuarioprofissional =
            new DFinanceiroCobrancaFaturamentoUsuarioProfissionalRepository();

        //Gravando cobrança ClienteMercado para o Profissional de Serviços
        public financeiro_cobranca_faturamento_usuario_profissional GravaCobrancaUsuarioProfissional(
            financeiro_cobranca_faturamento_usuario_profissional obj)
        {
            return dfinanceirocobrancafaturamentousuarioprofissional.GravaCobrancaUsuarioProfissional(obj);
        }

        //Consulta a existencia de Títulos em aberto para o Usuário, ao se logar
        public financeiro_cobranca_faturamento_usuario_profissional ConsultarTitulosEmAbertoParaOUsuarioProfissionalDeServicos(
            financeiro_cobranca_faturamento_usuario_profissional obj)
        {
            return dfinanceirocobrancafaturamentousuarioprofissional.ConsultarTitulosEmAbertoParaOUsuarioProfissionalDeServicos(obj);
        }
    }
}
