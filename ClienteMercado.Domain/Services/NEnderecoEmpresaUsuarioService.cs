using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NEnderecoEmpresaUsuarioService
    {
        DEnderecosEmpresaUsuarioRepository denderecosempresausuario = new DEnderecosEmpresaUsuarioRepository();

        //Consulta dos dados atrelados ao endereço da Empresa
        public enderecos_empresa_usuario ConsultarDadosEnderecoEmpresaUsuario(enderecos_empresa_usuario obj)
        {
            return denderecosempresausuario.ConsultarDadosEnderecoEmpresaUsuario(obj);
        }

        //Consulta as Cidades pertencentes a um determinado Estado e monta uma lista
        public List<cidades_empresa_usuario> ConsultarEMontarListaDeCidadesPorEstado(int idEstado)
        {
            return denderecosempresausuario.ConsultarEMontarListaDeCidadesPorEstado(idEstado);
        }

        //CONSULTA DADOS da LOCALIZAÇÃO pelo ID
        public List<ListaDadosDeLocalizacaoViewModel> ConsultarDadosDaLocalizacaoPeloCodigo(int iD_CODIGO_ENDERECO_EMPRESA_USUARIO)
        {
            return denderecosempresausuario.ConsultarDadosDaLocalizacaoPeloCodigo(iD_CODIGO_ENDERECO_EMPRESA_USUARIO);
        }

        public dadosLocalizacao ConsultarDadosDaLocalizacaoPeloCodigo2(int idLocal) => denderecosempresausuario.ConsultarDadosDaLocalizacaoPeloCodigo2(idLocal);
    }
}
