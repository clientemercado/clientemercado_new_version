using System.Collections.Generic;

namespace ClienteMercado.Models
{
    public class PerfilUsuarioEmpresa : PerfilUsuarioCotante
    {
        public string NOME_COTACAO_USUARIO_EMPRESA_DIRECIONADA { get; set; }

        public string NOME_COTACAO_USUARIO_EMPRESA_AVULSA { get; set; }

        public string PERMISSAO_VER_COTACOES_AVULSAS { get; set; }

        //Demais campos
        public List<CotacoesRecebidasPeloUsuario> listagemDasCotacoesDirecionadasRecebidasPeloUsuarioEmpresa { get; set; }
        public List<CotacoesRecebidasPeloUsuario> listagemDasCotacoesAvulsasRecebidasPeloUsuarioEmpresa { get; set; }
    }
}
