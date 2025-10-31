using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoUsuarioCotanteService
    {
        DItensCotacaoUsuarioCotanteRepository dcotacaomasterusuariocotante =
            new DItensCotacaoUsuarioCotanteRepository();

        //Gravar os Itens que fazem parte da Cotação Master do Usuário Cotante
        public itens_cotacao_usuario_cotante GravarItensDaCotacaoMasterDoUsuarioCotante(itens_cotacao_usuario_cotante obj)
        {
            return dcotacaomasterusuariocotante.GravarItensDaCotacaoMasterDoUsuarioCotante(obj);
        }

        //Consultar os ITENS da COTAÇÃO
        public List<itens_cotacao_usuario_cotante> ConsultarItensDaCotacaoDoUsuarioCotante(int idCotacaoMaster)
        {
            return dcotacaomasterusuariocotante.ConsultarItensDaCotacaoDoUsuarioCotante(idCotacaoMaster);
        }

        //Consultar dados dos ITENS da COTAÇÃO FILHA enviada aos FORNECEDORES
        public itens_cotacao_usuario_cotante ConsultarDadosDosItensDaCotacaoFilha(itens_cotacao_usuario_cotante obj)
        {
            return dcotacaomasterusuariocotante.ConsultarDadosDosItensDaCotacaoFilha(obj);
        }
    }
}
