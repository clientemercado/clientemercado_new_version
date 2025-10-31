using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEnderecosEmpresaUsuarioRepository : RepositoryBase<enderecos_empresa_usuario>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;

        //Consulta dos dados atrelados ao endereço da Empresa
        public enderecos_empresa_usuario ConsultarDadosEnderecoEmpresaUsuario(enderecos_empresa_usuario obj)
        {
            enderecos_empresa_usuario dadosDoEndereco =
                _contexto.enderecos_empresa_usuario.FirstOrDefault(
                    m => m.ID_CODIGO_ENDERECO_EMPRESA_USUARIO.Equals(obj.ID_CODIGO_ENDERECO_EMPRESA_USUARIO));

            return dadosDoEndereco;
        }

        //Consulta as Cidades pertencentes a um determinado Estado e retorna uma lista
        public List<cidades_empresa_usuario> ConsultarEMontarListaDeCidadesPorEstado(int idEstado)
        {
            return _contexto.cidades_empresa_usuario.Where(m => m.ID_ESTADOS_EMPRESA_USUARIO == idEstado).OrderBy(m => m.CIDADE_EMPRESA_USUARIO).ToList();
        }

        //CONSULTA DADOS da LOCALIZAÇÃO pelo ID
        public List<ListaDadosDeLocalizacaoViewModel> ConsultarDadosDaLocalizacaoPeloCodigo(int iD_CODIGO_ENDERECO_EMPRESA_USUARIO)
        {
            var query = " SELECT CEU.CIDADE_EMPRESA_USUARIO, ES.ID_ESTADOS_EMPRESA_USUARIO, ES.UF_EMPRESA_USUARIO " +
                    " FROM empresa_usuario EU " +
                    " INNER JOIN enderecos_empresa_usuario EEU ON(EEU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                    " INNER JOIN cidades_empresa_usuario CEU ON(CEU.ID_CIDADE_EMPRESA_USUARIO = EEU.ID_CIDADE_EMPRESA_USUARIO) " +
                    " INNER JOIN estados_empresa_usuario ES ON(ES.ID_ESTADOS_EMPRESA_USUARIO = CEU.ID_ESTADOS_EMPRESA_USUARIO) " +
                    " WHERE EU.ID_CODIGO_EMPRESA = " + idEmpresa;

            var listaDeEnderecos = _contexto.Database.SqlQuery<ListaDadosDeLocalizacaoViewModel>(query).ToList();

            return listaDeEnderecos;
        }

        public dadosLocalizacao ConsultarDadosDaLocalizacaoPeloCodigo2(int idLocal)
        {
            var query = " SELECT CEU.CIDADE_EMPRESA_USUARIO, ES.ID_ESTADOS_EMPRESA_USUARIO, ES.UF_EMPRESA_USUARIO " +
                    " FROM empresa_usuario EU " +
                    " INNER JOIN enderecos_empresa_usuario EEU ON(EEU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                    " INNER JOIN cidades_empresa_usuario CEU ON(CEU.ID_CIDADE_EMPRESA_USUARIO = EEU.ID_CIDADE_EMPRESA_USUARIO) " +
                    " INNER JOIN estados_empresa_usuario ES ON(ES.ID_ESTADOS_EMPRESA_USUARIO = CEU.ID_ESTADOS_EMPRESA_USUARIO) " +
                    " WHERE EU.ID_CODIGO_EMPRESA = " + idEmpresa;
            var enderecos = _contexto.Database.SqlQuery<dadosLocalizacao>(query).ToList();

            dadosLocalizacao dadosCidade = new dadosLocalizacao();
            if (enderecos != null)
            {
                dadosCidade.CIDADE_EMPRESA_USUARIO = enderecos[0].CIDADE_EMPRESA_USUARIO;
                dadosCidade.UF_EMPRESA_USUARIO = enderecos[0].UF_EMPRESA_USUARIO;
            }

            return dadosCidade;
        }
    }
}
