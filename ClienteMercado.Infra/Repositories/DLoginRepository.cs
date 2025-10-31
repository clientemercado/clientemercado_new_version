using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System;
using System.Collections;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DLoginRepository : RepositoryBase<empresa_usuario_logins>
    {
        public empresa_usuario_logins ConsultarDadosDeContatoDoUsuarioDaEmpresaCotadaidEmpresaUsuario { get; set; }

        //Verifica se o Login digitado pelo Usuário da Empresa já existe para outro cadastro.
        public empresa_usuario_logins ChecarExistenciaLoginUsuarioEmpresa(empresa_usuario_logins obj)
        {
            empresa_usuario_logins loginPretendidoEmpresa =
                _contexto.empresa_usuario_logins.FirstOrDefault(
                    m => m.LOGIN_EMPRESA_USUARIO_LOGINS.Equals(obj.LOGIN_EMPRESA_USUARIO_LOGINS));

            return loginPretendidoEmpresa;
        }

        //Consulta os dados de Login digitado e se ok, traz os dados / Usuario da Empresa.
        public empresa_usuario_logins ConsultarLoginUsuarioEmpresa(empresa_usuario_logins obj)
        {
            empresa_usuario_logins loginUsuarioEmpresa =
                _contexto.empresa_usuario_logins.FirstOrDefault(
                    m =>
                        (m.LOGIN_EMPRESA_USUARIO_LOGINS.Equals(obj.LOGIN_EMPRESA_USUARIO_LOGINS) ||
                         m.EMAIL1_USUARIO.Equals(obj.LOGIN_EMPRESA_USUARIO_LOGINS)) &&
                        m.SENHA_EMPRESA_USUARIO_LOGINS.Equals(obj.SENHA_EMPRESA_USUARIO_LOGINS));

            return loginUsuarioEmpresa;
        }

        //Consultar o Login e trazer os dados da Empresa e Usuário Master, para o faturamento
        public empresa_usuario_logins BuscarDadosUsuarioEEmpresaParaFaturamento(empresa_usuario_logins obj)
        {
            empresa_usuario_logins dadosUsuarioEEmpresa =
                    _contexto.empresa_usuario_logins.FirstOrDefault(
                        m => m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO));

            return dadosUsuarioEEmpresa;
        }

        //Consultar e trazer o e-mail do Usuário Master
        public empresa_usuario_logins BuscarEmailDoUsuarioMaster(int idMaster)
        {
            empresa_usuario_logins eMailDoUsuarioMaster =
                _contexto.empresa_usuario_logins.FirstOrDefault(m => m.ID_CODIGO_USUARIO.Equals(idMaster));

            return eMailDoUsuarioMaster;
        }

        //Consultar e-mail dos Vendedores que receberão aviso de cotação
        public ArrayList ConsultarEmailsDosVendedoresQueReceberaoAvisoDeCotacao(string[] listaIDsFornecedores)
        {
            ArrayList listaEmailsVendedoresQueReceberaoACotacao = new ArrayList();

            //Consulta o e-mail de cada ID da lista
            for (int i = 0; i < listaIDsFornecedores.Length; i++)
            {
                int idFornecedor = Convert.ToInt32(listaIDsFornecedores[i]);

                empresa_usuario_logins emailFornecedores =
                    _contexto.empresa_usuario_logins.FirstOrDefault(m => m.ID_CODIGO_USUARIO.Equals(idFornecedor));

                if (emailFornecedores != null)
                {
                    listaEmailsVendedoresQueReceberaoACotacao.Add(emailFornecedores.EMAIL1_USUARIO);
                }
            }

            return listaEmailsVendedoresQueReceberaoACotacao;
        }

        //Grava alteração de Senha do Usuário de Empresa que fez solicitação
        public empresa_usuario_logins GravarNovaSenhaUsuarioEmpresa(empresa_usuario_logins obj)
        {
            empresa_usuario_logins novaSenhaUsuarioEmpresa =
                _contexto.empresa_usuario_logins.Find(obj.ID_CODIGO_USUARIO);
            novaSenhaUsuarioEmpresa.SENHA_EMPRESA_USUARIO_LOGINS = obj.SENHA_EMPRESA_USUARIO_LOGINS;

            _contexto.SaveChanges();

            return novaSenhaUsuarioEmpresa;
        }

        //CONSULTA DADOS de CONTATO do USUÁRIO
        public empresa_usuario_logins ConsultarDadosDeContatoDoUsuarioDaEmpresaCotada(int? idEmpresaUsuario)
        {
            empresa_usuario_logins dadosDeContato =
                _contexto.empresa_usuario_logins.FirstOrDefault(m => (m.ID_CODIGO_USUARIO == idEmpresaUsuario));

            return dadosDeContato;
        }

        //Verifica se o Login digitado pelo Usuário Profissional já existe para outro cadastro de Profissional.
        public profissional_usuario_logins ChecarExistenciaLoginUsuarioProfissional(profissional_usuario_logins obj)
        {
            profissional_usuario_logins loginPretendidoProfissional =
                _contexto.profissional_usuario_logins.FirstOrDefault(
                    m => m.LOGIN_PROFISSIONAL_USUARIO_LOGINS.Equals(obj.LOGIN_PROFISSIONAL_USUARIO_LOGINS));

            return loginPretendidoProfissional;
        }

        //Consulta o Login digitado e se ok, traz os dados / Profissional Serviços.
        public profissional_usuario_logins ConsultarLoginProfissionalServicos(
            profissional_usuario_logins obj)
        {
            profissional_usuario_logins loginUsuarioProfissional =
                _contexto.profissional_usuario_logins.FirstOrDefault(
                    m =>
                        (m.LOGIN_PROFISSIONAL_USUARIO_LOGINS.Equals(obj.LOGIN_PROFISSIONAL_USUARIO_LOGINS) ||
                         m.EMAIL1_USUARIO.Equals(obj.LOGIN_PROFISSIONAL_USUARIO_LOGINS)) &&
                        m.SENHA_PROFISSIONAL_USUARIO_LOGINS.Equals(obj.SENHA_PROFISSIONAL_USUARIO_LOGINS));

            return loginUsuarioProfissional;
        }

        //Consultar o Login e trazer os dados do Profissional de Serviços e Usuário Master, para faturamento
        public profissional_usuario_logins BuscarDadosUsuarioProfissionalDeServicosParaFaturamento(profissional_usuario_logins obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            profissional_usuario_logins dadosUsuarioEProfissional =
                _contexto.profissional_usuario_logins.FirstOrDefault(
                    m => m.ID_CODIGO_USUARIO_PROFISSIONAL.Equals(obj.ID_CODIGO_USUARIO_PROFISSIONAL));

            return dadosUsuarioEProfissional;
        }

        //Grava alteração de Senha do Usuário Profissional de Serviços que fez solicitação
        public profissional_usuario_logins GravarNovaSenhaProfissionalServicos(profissional_usuario_logins obj)
        {
            profissional_usuario_logins novaSenhaProfissionalServicos =
                _contexto.profissional_usuario_logins.Find(obj.ID_CODIGO_USUARIO_PROFISSIONAL);
            novaSenhaProfissionalServicos.SENHA_PROFISSIONAL_USUARIO_LOGINS = obj.SENHA_PROFISSIONAL_USUARIO_LOGINS;

            _contexto.SaveChanges();

            return novaSenhaProfissionalServicos;
        }

        //Verifica se o Login digitado pelo Usuário Cotante já existe para outro cadastro de Usuário Cotante.
        public usuario_cotante_logins ChecarExistenciaLoginUsuarioCotante(usuario_cotante_logins obj)
        {
            usuario_cotante_logins loginPretendidoCotante =
                _contexto.usuario_cotante_logins.FirstOrDefault(
                    m => m.LOGIN_USUARIO_COTANTE_LOGINS.Equals(obj.LOGIN_USUARIO_COTANTE_LOGINS));

            return loginPretendidoCotante;
        }

        //Consulta o Login digitado e se ok, traz os dados / Usuario Cotante.
        public usuario_cotante_logins ConsultarLoginUsuarioCotante(usuario_cotante_logins obj)
        {
            usuario_cotante_logins loginUsuarioCotante =
                _contexto.usuario_cotante_logins.FirstOrDefault(
                    m =>
                        (m.LOGIN_USUARIO_COTANTE_LOGINS.Equals(obj.LOGIN_USUARIO_COTANTE_LOGINS) ||
                         m.EMAIL1_USUARIO.Equals(obj.LOGIN_USUARIO_COTANTE_LOGINS)) &&
                        m.SENHA_USUARIO_COTANTE_LOGINS.Equals(obj.SENHA_USUARIO_COTANTE_LOGINS));

            return loginUsuarioCotante;
        }

        //Grava alteração de Senha do Usuário Cotante que fez solicitação
        public usuario_cotante_logins GravarNovaSenhaUsuarioCotante(usuario_cotante_logins obj)
        {
            usuario_cotante_logins novaSenhaUsuarioCotante =
                _contexto.usuario_cotante_logins.Find(obj.ID_CODIGO_USUARIO_COTANTE);
            novaSenhaUsuarioCotante.SENHA_USUARIO_COTANTE_LOGINS = obj.SENHA_USUARIO_COTANTE_LOGINS;

            _contexto.SaveChanges();

            return novaSenhaUsuarioCotante;
        }
    }
}
