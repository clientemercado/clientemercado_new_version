using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoUsuarioCotanteRepository
    {
        //Gravar os Itens que fazem parte da Cotação Master do Usuário Cotante
        public itens_cotacao_usuario_cotante GravarItensDaCotacaoMasterDoUsuarioCotante(itens_cotacao_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_usuario_cotante gravarItensCotacaoUsuarioCotante =
                    _contexto.itens_cotacao_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return gravarItensCotacaoUsuarioCotante;
            }
        }

        //Consultar os ITENS da COTAÇÂO
        public List<itens_cotacao_usuario_cotante> ConsultarItensDaCotacaoDoUsuarioCotante(int idCotacaoMaster)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<itens_cotacao_usuario_cotante> consultarItensDaCotacao =
                    _contexto.itens_cotacao_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE.Equals(idCotacaoMaster))).ToList();

                return consultarItensDaCotacao;
            }
        }

        //Consultar dados dos ITENS da COTAÇÃO FILHA enviada aos FORNECEDORES
        public itens_cotacao_usuario_cotante ConsultarDadosDosItensDaCotacaoFilha(itens_cotacao_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_usuario_cotante consultarItemDaCotacao =
                    _contexto.itens_cotacao_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE.Equals(obj.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE)));

                return consultarItemDaCotacao;
            }
        }
    }
}
