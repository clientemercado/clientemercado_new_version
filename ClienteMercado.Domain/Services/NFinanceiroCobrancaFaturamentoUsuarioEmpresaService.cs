using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NFinanceiroCobrancaFaturamentoUsuarioEmpresaService
    {
        DFinanceiroCobrancaFaturamentoUsuarioEmpresaRepository dfinanceirocobrancafaturamentousuarioempresa =
            new DFinanceiroCobrancaFaturamentoUsuarioEmpresaRepository();

        //Gravando cobrança ClienteMercado para o Usuário ou Empresa
        public financeiro_cobranca_faturamento_usuario_empresa GravaCobrancaUsuarioEmpresa(
            financeiro_cobranca_faturamento_usuario_empresa obj)
        {
            return dfinanceirocobrancafaturamentousuarioempresa.GravarCobrancaUsuarioEmpresa(obj);
        }

        //Consuta a existencia de Títulos em aberto para Empresa ou Usuário, ao se logar
        public financeiro_cobranca_faturamento_usuario_empresa ConsultarTitulosEmAbertoParaEmpresasEUsuarios(financeiro_cobranca_faturamento_usuario_empresa obj)
        {
            return dfinanceirocobrancafaturamentousuarioempresa.ConsultarTitulosEmAbertoParaEmpresasEUsuarios(obj);
        }
    }
}
