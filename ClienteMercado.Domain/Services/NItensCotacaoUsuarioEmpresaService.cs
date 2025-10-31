using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoUsuarioEmpresaService
    {
        DItensCotacaoUsuarioEmpresaRepository dcotacaomasterusuarioempresa =
            new DItensCotacaoUsuarioEmpresaRepository();

        //Gravar os Itens que fazem parte da COTAÇÃO MASTER do USUÁRIO EMPRESA
        public itens_cotacao_usuario_empresa GravarItensDaCotacaoMasterDoUsuarioEmpresa(itens_cotacao_usuario_empresa obj)
        {
            return dcotacaomasterusuarioempresa.GravarItensDaCotacaoMasterDoUsuarioEmpresa(obj);
        }

        //Consultar os ITENS da COTAÇÃO
        public List<itens_cotacao_usuario_empresa> ConsultarItensDaCotacaoDoUsuarioEmpresa(int idCotacaoMaster)
        {
            return dcotacaomasterusuarioempresa.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoMaster);
        }

        //Consultar dados dos ITENS da COTAÇÃO FILHA enviada aos FORNECEDORES
        public itens_cotacao_usuario_empresa ConsultarDadosDosItensDaCotacaoFilha(itens_cotacao_usuario_empresa obj)
        {
            return dcotacaomasterusuarioempresa.ConsultarDadosDosItensDaCotacaoFilha(obj);
        }
    }
}
