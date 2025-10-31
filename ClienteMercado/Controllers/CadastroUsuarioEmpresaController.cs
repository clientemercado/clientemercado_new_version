using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Controllers
{
    public class CadastroUsuarioEmpresaController : Controller
    {
        //
        // GET: /CadastroUsuario/
        public ActionResult CadastroUsuario()
        {
            CadastroUsuarioEmpresa usuario = new CadastroUsuarioEmpresa();
            var listaPaises = ListagemPaises();

            usuario.ListagemPaises = listaPaises;
            usuario.ListagemEstados = ListagemEstados();

            return View(usuario);
        }

        //Carrega lista de Países atendidos pelo ClienteMercado
        private static List<SelectListItem> ListagemPaises()
        {
            //Buscar lista de Países
            NPaisesService negocioPaises = new NPaisesService();

            List<paises_empresa_usuario> listaPaises = negocioPaises.ListaPaises();

            List<SelectListItem> listPaises = new List<SelectListItem>();

            foreach (var grupoPaises in listaPaises)
            {
                listPaises.Add(new SelectListItem
                {
                    Text = grupoPaises.PAIS_EMPRESA_USUARIO,
                    Value = grupoPaises.ID_PAISES_EMPRESA_USUARIO.ToString()
                });
            }

            return listPaises;
        }

        //Carrega a lista de Estados (Obs: No momento carregrá todos os estados brasileiros. Depois vejo como ficará)
        private List<SelectListItem> ListagemEstados()
        {
            //Buscar lista de Estados brasileiros
            NEstadosService negocioEstados = new NEstadosService();

            List<estados_empresa_usuario> listaEstados = negocioEstados.ListaEstados();

            List<SelectListItem> listEstados = new List<SelectListItem>();

            listEstados.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var grupoEstados in listaEstados)
            {
                listEstados.Add(new SelectListItem
                {
                    Text = grupoEstados.UF_EMPRESA_USUARIO,
                    Value = grupoEstados.ID_ESTADOS_EMPRESA_USUARIO.ToString()
                });
            }

            return listEstados;
        }

        //Verifica se o CPF digitado já existe no banco
        [WebMethod]
        public ActionResult VerificaSeCPFExisteNoBanco(string cpfDigitado)
        {
            if (cpfDigitado.Length == 14)
            {
                if (ModelState.IsValid)
                {
                    cpfDigitado = Regex.Replace(cpfDigitado, "[.-]", "");

                    try
                    {
                        bool cpfValido = Validacoes.ValidaCPF(cpfDigitado);

                        if (cpfValido)
                        {
                            NCpfService negocio = new NCpfService();
                            usuario_empresa novoCpf = new usuario_empresa();

                            novoCpf.CPF_USUARIO_EMPRESA = cpfDigitado;
                            novoCpf.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);

                            //Pesquisa Cpf de acordo com o tipo de cadastro de usuário (1-de nova Empresa / 2-de Empresa já existente)
                            if (Sessao.IdEmpresaUsuario == null)
                            {
                                //Checando CPF de Usuário de nova Empresa
                                usuario_empresa usuarioEmpresa = negocio.ConsultarCpf(novoCpf);

                                if (usuarioEmpresa != null)
                                {
                                    //Montando o parâmetro a ser retornado
                                    var resultado = new
                                    {
                                        mensagem = "*  CPF já cadastrado como Usuário em outra empresa",
                                        nomeUsuario = "",
                                        nickName = ""
                                    };

                                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                        JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                //Checando CPF de Usuário da Empresa já cadastrada
                                usuario_empresa usuarioEmpresa = negocio.ConsultarCpfEmDeterminadaEmpresa(novoCpf);

                                if (usuarioEmpresa != null)
                                {
                                    //Montando os parâmetros a serem retornados
                                    var resultado = new
                                    {
                                        mensagem = "* CPF já cadastrado como Usuário para esta empresa",
                                        nomeUsuario = usuarioEmpresa.NOME_USUARIO,
                                        nickName = usuarioEmpresa.NICK_NAME_USUARIO
                                    };

                                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                        JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    //Checando CPF se esta como Usuário em outra empresa
                                    usuario_empresa usuarioOutrasEmpresas = negocio.ConsultarCpf(novoCpf);

                                    if (usuarioOutrasEmpresas != null)
                                    {
                                        //Montando o parâmetro a ser retornado
                                        var resultado = new
                                        {
                                            mensagem = "*  CPF já cadastrado como Usuário em outra empresa",
                                            nomeUsuario = "",
                                            nickName = ""
                                        };

                                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                            JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Montando os parâmetros a serem retornados
                            var resultado = new
                            {
                                mensagem = "* CPF Inválido.",
                                nomeUsuario = "",
                                nickName = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Erro ao validar CPF do usuário!");
                    }
                }
            }
            else
            {
                if (cpfDigitado != "")
                {
                    //Montando os parâmetros a serem retornados
                    var resultado = new
                    {
                        mensagem = "* Dígitos insuficientes! Entre com o CPF completo.",
                        nomeUsuario = "",
                        nickName = ""
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //Checa existencia do E-Mail no banco
        [WebMethod]
        public ActionResult VerificaSeEmailExisteNoBanco(string emailDigitado)
        {
            if (emailDigitado.Length <= 50)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        bool emailValido = Validacoes.validaEmail(emailDigitado);

                        if (emailValido)
                        {
                            NEmpresaUsuarioService negocio = new NEmpresaUsuarioService();
                            empresa_usuario_logins novoEmail = new empresa_usuario_logins();

                            novoEmail.EMAIL1_USUARIO = emailDigitado;
                            empresa_usuario_logins empresaUsuarioLogins = negocio.ConsultarEmailUsuarioEmpresa(novoEmail);

                            if (empresaUsuarioLogins != null)
                            {
                                return Json("* E-mail já cadastrado em nossa base! Tente outro E-mail.", "text/x-json", System.Text.Encoding.UTF8,
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("* E-mail Inválido", "text/x-json", System.Text.Encoding.UTF8,
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Erro ao validar E-Mail da empresa!");
                    }
                }
            }
            else
            {
                if (emailDigitado != "")
                {
                    return Json("* Caracteres insuficientes! Entre com o E-mail completo.", "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //Checa existencia do Login no banco
        [WebMethod]
        public ActionResult VerificaSeLoginJaExisteNoBanco(string loginDigitado)
        {
            if (loginDigitado != "")
            {
                try
                {
                    NLoginService negocio = new NLoginService();
                    empresa_usuario_logins checarLogin = new empresa_usuario_logins();

                    checarLogin.LOGIN_EMPRESA_USUARIO_LOGINS = loginDigitado;
                    empresa_usuario_logins empresaUsuarioLogins = negocio.ChecarExistenciaLoginUsuarioEmpresa(checarLogin);

                    if (empresaUsuarioLogins != null)
                    {
                        return Json("*Login já cadastrado para um outro Usuário. Tente outro Login!", "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao checar o Login digitado!");
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //Salvar o cadastro da Empresa / Usuário
        [HttpPost]
        public ActionResult CadastroUsuario(CadastroUsuarioEmpresa cadastroDoUsuarioEEmpresa)
        {
            //Valida novamente o CPF, evitando CPF´s alterados no código
            string cpfDigitado = Regex.Replace(cadastroDoUsuarioEEmpresa.CPF_USUARIO_EMPRESA, "[./-]", "");
            bool cpfValido = Validacoes.ValidaCPF(cpfDigitado);

            if (cpfValido)
            {
                try
                {
                    //Salvando os dados da Empresa
                    NEmpresaUsuarioService negocio = new NEmpresaUsuarioService();
                    empresa_usuario novaEmpresaUsuario = (empresa_usuario)Sessao.tipoDeUsuario;

                    //Salvando os dados do Usuário
                    usuario_empresa novoUsuario = new usuario_empresa();

                    novoUsuario.CPF_USUARIO_EMPRESA = cpfDigitado;
                    novoUsuario.NOME_USUARIO = cadastroDoUsuarioEEmpresa.NOME_USUARIO;
                    novoUsuario.ID_CODIGO_TIPO_EMPRESA_USUARIO =
                        cadastroDoUsuarioEEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO;
                    novoUsuario.NICK_NAME_USUARIO = cadastroDoUsuarioEEmpresa.NICK_NAME_USUARIO;
                    novoUsuario.TELEFONE1_USUARIO_EMPRESA = cadastroDoUsuarioEEmpresa.TELEFONE1_USUARIO_EMPRESA;
                    novoUsuario.RECEBER_EMAILS_USUARIO = true;
                    //novoUsuario.PAIS_USUARIO_EMPRESA = cadastroDoUsuarioEEmpresa.PAIS_USUARIO_EMPRESA;
                    novoUsuario.ID_CODIGO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDoUsuarioEEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    novoUsuario.COMPLEMENTO_ENDERECO_USUARIO = cadastroDoUsuarioEEmpresa.COMPLEMENTO_ENDERECO_USUARIO;
                    novoUsuario.ID_CODIGO_PROFISSAO = cadastroDoUsuarioEEmpresa.ID_CODIGO_PROFISSAO;
                    novoUsuario.DATA_CADASTRO_USUARIO = DateTime.Now;
                    novoUsuario.DATA_ULTIMA_ATUALIZACAO_USUARIO = DateTime.Now;
                    novoUsuario.ATIVA_INATIVO_USUARIO = cadastroDoUsuarioEEmpresa.ATIVA_INATIVO_USUARIO;
                    novoUsuario.CADASTRO_CONFIRMADO = false;

                    //Salvando dados para o Login do Usuário da Empresa
                    empresa_usuario_logins novoUsuarioEmpresaLogins = new empresa_usuario_logins();

                    novoUsuarioEmpresaLogins.LOGIN_EMPRESA_USUARIO_LOGINS =
                        cadastroDoUsuarioEEmpresa.LOGIN_EMPRESA_USUARIO_LOGINS;
                    novoUsuarioEmpresaLogins.SENHA_EMPRESA_USUARIO_LOGINS =
                        Hash.GerarHashMd5(cadastroDoUsuarioEEmpresa.SENHA_EMPRESA_USUARIO_LOGINS);
                    novoUsuarioEmpresaLogins.EMAIL1_USUARIO = cadastroDoUsuarioEEmpresa.EMAIL1_USUARIO;

                    //Atribuindo o objeto do novoUsuario em novaEmpresaUsuario para q ocorra a gravação dos dados em cascata e se necessário, o rollback.
                    novoUsuario.empresa_usuario_logins.Add(novoUsuarioEmpresaLogins);

                    //Cadastrar de acordo com o tipo de Usuário (1-de nova Empresa / 2-de Empresa já existente)
                    if (Sessao.IdEmpresaUsuario == null || Sessao.IdEmpresaUsuario == 0)
                    {
                        //Cadastro de Usuário para uma nova Empresa
                        novoUsuario.USUARIO_MASTER = cadastroDoUsuarioEEmpresa.USUARIO_MASTER; //É o Usuário Master
                        novaEmpresaUsuario.usuario_empresa.Add(novoUsuario);

                        empresa_usuario empresaUsuario = negocio.GravarEmpresaUsuario(novaEmpresaUsuario);

                        return RedirectToAction("ConfirmacaoCadastro", "CadastroUsuarioEmpresa");
                    }
                    else
                    {
                        //Cadastro de Usuário para Empresa já existente
                        novoUsuario.USUARIO_MASTER = false; //Não é o Usuário Master
                        novoUsuario.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);

                        usuario_empresa usuarioEmpresa = negocio.GravarNovoUsuarioEmEmpresaJaCadastrada(novoUsuario);

                        return RedirectToAction("ConfirmacaoCadastro", "CadastroUsuarioEmpresa");
                    }

                }
                catch (Exception erro)
                {
                    throw erro;
                    //ModelState.AddModelError("", "");
                }
            }
            else
            {
                return Json("* CPF Inválido", "text/x-json", System.Text.Encoding.UTF8,
                    JsonRequestBehavior.AllowGet);
            }

            cadastroDoUsuarioEEmpresa.ListagemPaises = ListagemPaises();

            return View(cadastroDoUsuarioEEmpresa).ComMensagem("* Erro interno ao salvar os dados cadastrais da Empresa e Usuário! Por favor, tente mais tarde!");
        }

        // GET: /CadastroUsuarioEmpresa/ConfirmacaoCadastro
        public ActionResult ConfirmacaoCadastro()
        {
            //Remover todas as Sessions criadas pelo usuário
            Session.Abandon();

            return View();
        }

    }
}
