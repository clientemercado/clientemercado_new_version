using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections;

namespace ClienteMercado.Domain.Services
{
    public class NLoginService
    {
        DLoginRepository dlogin = new DLoginRepository();

        //Verifica se o Login digitado pelo Usuário já existe para outro cadastro  / Usuario da Empresa.
        public empresa_usuario_logins ChecarExistenciaLoginUsuarioEmpresa(empresa_usuario_logins obj)
        {
            return dlogin.ChecarExistenciaLoginUsuarioEmpresa(obj);
        }

        //Consulta o Login digitado para se logar ao sistema / Usuario da Empresa.
        public empresa_usuario_logins ConsultarLoginUsuarioEmpresa(empresa_usuario_logins obj)
        {
            return dlogin.ConsultarLoginUsuarioEmpresa(obj);
        }

        //Consultar o Login e trazer os dados da Empresa e Usuário Master, para o faturamento
        public empresa_usuario_logins BuscarDadosUsuarioEEmpresaParaFaturamento(empresa_usuario_logins obj)
        {
            return dlogin.BuscarDadosUsuarioEEmpresaParaFaturamento(obj);
        }

        //Grava alteração de Senha do Usuário de Empresa que fez solicitação
        public empresa_usuario_logins GravarNovaSenhaUsuarioEmpresa(empresa_usuario_logins obj)
        {
            return dlogin.GravarNovaSenhaUsuarioEmpresa(obj);
        }

        //Consultar e-mail dos Vendedores que receberão aviso de cotação
        public ArrayList ConsultarEmailsDosVendedoresQueReceberaoAvisoDeCotacao(string[] listaIDsFornecedores)
        {
            return dlogin.ConsultarEmailsDosVendedoresQueReceberaoAvisoDeCotacao(listaIDsFornecedores);
        }

        //Consulta o Login digitado para se cadastrar no sistema/ Usuario Profissional.
        public profissional_usuario_logins ChecarExistenciaLoginUsuarioProfissional(
            profissional_usuario_logins obj)
        {
            return dlogin.ChecarExistenciaLoginUsuarioProfissional(obj);
        }

        //Consulta o Login digitado para se logar ao sistema / Usuario Profissional Serviços.
        public profissional_usuario_logins ConsultarLoginProfissionalServicos(
            profissional_usuario_logins obj)
        {
            return dlogin.ConsultarLoginProfissionalServicos(obj);
        }

        //Consultar o Login etrazer os dados do Profissional de Serviços e Usuário Master, para faturamento
        public profissional_usuario_logins BuscarDadosUsuarioProfissionalDeServicosParaFaturamento(
            profissional_usuario_logins obj)
        {
            return dlogin.BuscarDadosUsuarioProfissionalDeServicosParaFaturamento(obj);
        }

        //Grava alteração de Senha do Usuário Profissional de Serviços que fez solicitação
        public profissional_usuario_logins GravarNovaSenhaProfissionalServicos(profissional_usuario_logins obj)
        {
            return dlogin.GravarNovaSenhaProfissionalServicos(obj);
        }

        //Consulta o Login digitado para se cadastrar no sistema/ Usuario Cotante.
        public usuario_cotante_logins ChecarExistenciaLoginUsuarioCotante(usuario_cotante_logins obj)
        {
            return dlogin.ChecarExistenciaLoginUsuarioCotante(obj);
        }

        //Consulta o Login digitado para se logar ao sistema / Usuario Cotante.
        public usuario_cotante_logins ConsultarLoginUsuarioCotante(usuario_cotante_logins obj)
        {
            return dlogin.ConsultarLoginUsuarioCotante(obj);
        }

        //Grava alteração de Senha do Usuário Cotante que fez solicitação
        public usuario_cotante_logins GravarNovaSenhaUsuarioCotante(usuario_cotante_logins obj)
        {
            return dlogin.GravarNovaSenhaUsuarioCotante(obj);
        }

        //CONSULTA DADOS de CONTATO do USUÁRIO
        public empresa_usuario_logins ConsultarDadosDeContatoDoUsuarioDaEmpresaCotada(int? idEmpresaUsuario)
        {
            return dlogin.ConsultarDadosDeContatoDoUsuarioDaEmpresaCotada(idEmpresaUsuario);
        }
    }
}
