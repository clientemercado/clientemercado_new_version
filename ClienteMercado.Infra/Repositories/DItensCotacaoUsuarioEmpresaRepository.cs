using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoUsuarioEmpresaRepository
    {
        //Gravar os Itens que fazem parte da COTAÇÃO MASTER do USUÁRIO EMPRESA
        public itens_cotacao_usuario_empresa GravarItensDaCotacaoMasterDoUsuarioEmpresa(itens_cotacao_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_usuario_empresa gravarItensCotacaoUsuarioEmpresa =
                    _contexto.itens_cotacao_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return gravarItensCotacaoUsuarioEmpresa;
            }
        }

        //Consultar os ITENS da COTAÇÃO
        public List<itens_cotacao_usuario_empresa> ConsultarItensDaCotacaoDoUsuarioEmpresa(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<itens_cotacao_usuario_empresa> consultarItensDaCotacao =
                    _contexto.itens_cotacao_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_EMPRESA.Equals(idCotacaoMaster))).ToList();

                return consultarItensDaCotacao;
            }
        }

        //Consultar dados dos ITENS da COTAÇÃO FILHA enviada aos FORNECEDORES
        public itens_cotacao_usuario_empresa ConsultarDadosDosItensDaCotacaoFilha(itens_cotacao_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_usuario_empresa consultarItemDaCotacao =
                    _contexto.itens_cotacao_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA.Equals(obj.ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA)));

                return consultarItemDaCotacao;
            }
        }
    }
}
