using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Utilitarios;
using ClienteMercado.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Controllers
{
    public class CadastroUsuarioCotanteController : Controller
    {
        //
        // GET: /CadastroUsuarioCotante/

        public ActionResult CadastroUsuario()
        {
            CadastroUsuarioCotante usuario = new CadastroUsuarioCotante();

            usuario.ListagemPaises = ListagemPaises();
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

        //Verifica se o CPF digitado para o UsuárioCotante, já existe no banco
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
                            usuario_cotante novoCpf = new usuario_cotante();

                            novoCpf.CPF_USUARIO_COTANTE = cpfDigitado;
                            usuario_cotante usuarioCotante = negocio.ConsultarCpfUsuarioCotante(novoCpf);

                            if (usuarioCotante != null)
                            {
                                return Json("* CPF já cadastrado para outro Usuário de Cotações", "text/x-json", System.Text.Encoding.UTF8,
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("* CPF Inválido", "text/x-json", System.Text.Encoding.UTF8,
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Erro ao validar CPF do Usuário de Cotações");
                    }
                }
            }
            else
            {
                if (cpfDigitado != "")
                {
                    return Json("* Dígitos insuficientes! Entre com o CPF completo", "text/x-json", System.Text.Encoding.UTF8,
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
                            NUsuarioCotanteService negocio = new NUsuarioCotanteService();
                            usuario_cotante_logins novoEmail = new usuario_cotante_logins();

                            novoEmail.EMAIL1_USUARIO = emailDigitado;
                            usuario_cotante_logins usuarioCotanteLogins = negocio.ConsultarEmailUsuarioCotante(novoEmail);

                            if (usuarioCotanteLogins != null)
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

        //Checa existencia do Login para o Usuário Cotante no banco
        [WebMethod]
        public ActionResult VerificaSeLoginJaExisteNoBanco(string loginDigitado)
        {
            if (loginDigitado != "")
            {
                try
                {
                    NLoginService negocio = new NLoginService();
                    usuario_cotante_logins checarLogin = new usuario_cotante_logins();

                    checarLogin.LOGIN_USUARIO_COTANTE_LOGINS = loginDigitado;
                    usuario_cotante_logins usuarioCotanteLogins = negocio.ChecarExistenciaLoginUsuarioCotante(checarLogin);

                    if (usuarioCotanteLogins != null)
                    {
                        return Json("*Login já cadastrado para um outro Usuário Cotante. Tente outro Login!", "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao checar o Login digitado para o Usuário Cotante!");
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //Busca dados completos do endereço pelo CEP digitado
        [WebMethod]
        public ActionResult BuscaEndereco(string cepDigitado)
        {
            if (cepDigitado.Length == 9)
            {
                if (ModelState.IsValid)
                {
                    cepDigitado = Regex.Replace(cepDigitado, "[.-]", "");

                    try
                    {
                        NCepService negocio = new NCepService();
                        enderecos_empresa_usuario buscaCepAfins = new enderecos_empresa_usuario();

                        buscaCepAfins.CEP_ENDERECO_EMPRESA_USUARIO = Convert.ToInt64(cepDigitado);
                        enderecos_empresa_usuario enderecos_empresa_usuario = negocio.ConsultarCep(buscaCepAfins);

                        if (enderecos_empresa_usuario != null)
                        {
                            var enderecoCep =
                                new
                                {
                                    id_endereco = enderecos_empresa_usuario.ID_CODIGO_ENDERECO_EMPRESA_USUARIO,
                                    endereco = enderecos_empresa_usuario.TIPO_LOGRADOURO_EMPRESA_USUARIO + " " + enderecos_empresa_usuario.LOGRADOURO_CEP_EMPRESA_USUARIO,
                                    estado = enderecos_empresa_usuario.cidades_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO,
                                    cidade = enderecos_empresa_usuario.cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO,
                                    bairro =
                                        enderecos_empresa_usuario.bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO
                                };

                            return Json(enderecoCep, "text/x-json", System.Text.Encoding.UTF8,
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Erro ao checar CEP");
                    }
                }
            }
            else if (cepDigitado.Length < 9)
            {
                if (cepDigitado != "")
                {
                    return Json(false, "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
                }
            }

            return Json(cepDigitado, "text/x-json", System.Text.Encoding.UTF8,
                                    JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CadastroUsuario(CadastroUsuarioCotante cadastroDoUsuarioCotante)
        {
            //Valida novamente o CPF, evitando CPF´s alterados no código
            string cpfDigitado = Regex.Replace(cadastroDoUsuarioCotante.CPF_USUARIO_COTANTE, "[./-]", "");
            bool cpfValido = Validacoes.ValidaCPF(cpfDigitado);

            if (cpfValido)
            {
                try
                {
                    //Salvando os dados do Usuário Cotante
                    NUsuarioCotanteService negocio = new NUsuarioCotanteService();
                    usuario_cotante novoUsuarioCotante = new usuario_cotante();

                    novoUsuarioCotante.ID_CODIGO_TIPO_EMPRESA_USUARIO =
                        cadastroDoUsuarioCotante.ID_CODIGO_TIPO_EMPRESA_USUARIO;
                    novoUsuarioCotante.CPF_USUARIO_COTANTE = cpfDigitado;
                    novoUsuarioCotante.NOME_USUARIO_COTANTE = cadastroDoUsuarioCotante.NOME_USUARIO_COTANTE;
                    novoUsuarioCotante.NICK_NAME_USUARIO_COTANTE = cadastroDoUsuarioCotante.NICK_NAME_USUARIO_COTANTE;
                    novoUsuarioCotante.ID_CODIGO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDoUsuarioCotante.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    novoUsuarioCotante.COMPLEMENTO_ENDERECO_USUARIO_COTANTE =
                        cadastroDoUsuarioCotante.COMPLEMENTO_ENDERECO_USUARIO_COTANTE;
                    novoUsuarioCotante.PAIS_USUARIO_COTANTE = cadastroDoUsuarioCotante.PAIS_USUARIO_COTANTE;
                    novoUsuarioCotante.ID_CODIGO_PROFISSAO = cadastroDoUsuarioCotante.ID_CODIGO_PROFISSAO;
                    novoUsuarioCotante.TELEFONE1_USUARIO_COTANTE = cadastroDoUsuarioCotante.TELEFONE1_USUARIO_COTANTE;
                    novoUsuarioCotante.RECEBER_EMAILS_USUARIO_COTANTE = true;
                    novoUsuarioCotante.DATA_CADASTRO_USUARIO_COTANTE = DateTime.Now;
                    novoUsuarioCotante.DATA_ULTIMA_ATUALIZACAO_USUARIO_COTANTE = DateTime.Now;
                    novoUsuarioCotante.ATIVA_INATIVO_USUARIO_COTANTE =
                        cadastroDoUsuarioCotante.ATIVA_INATIVO_USUARIO_COTANTE;
                    novoUsuarioCotante.CADASTRO_CONFIRMADO = false;

                    //Salvando dados para o Login do Usuário Cotante
                    usuario_cotante_logins novoUsuarioCotanteLogins = new usuario_cotante_logins();

                    novoUsuarioCotanteLogins.LOGIN_USUARIO_COTANTE_LOGINS =
                        cadastroDoUsuarioCotante.LOGIN_USUARIO_COTANTE_LOGINS;
                    novoUsuarioCotanteLogins.SENHA_USUARIO_COTANTE_LOGINS = Hash.GerarHashMd5(cadastroDoUsuarioCotante.SENHA_USUARIO_COTANTE_LOGINS);
                    novoUsuarioCotanteLogins.EMAIL1_USUARIO = cadastroDoUsuarioCotante.EMAIL1_USUARIO;

                    //Atribuir o objeto para que ocorram as gravaçõesdos dados em cascata e se necessário, o rollback
                    novoUsuarioCotante.usuario_cotante_logins.Add(novoUsuarioCotanteLogins);

                    usuario_cotante usuarioCotante = negocio.GravarUsuarioCotante(novoUsuarioCotante);

                    //Envio de e-mail solicitando a confirmação de gravação de dados cadastrais do USUARIO COTANTE no ClienteMercado (Tipo 4)
                    if (usuarioCotante != null)
                    {
                        int tipoEmail = 4;

                        EnviarEmail enviarEmail = new EnviarEmail();

                        bool enviou = enviarEmail.EnviandoEmail(cadastroDoUsuarioCotante.EMAIL1_USUARIO,
                            cadastroDoUsuarioCotante.NICK_NAME_USUARIO_COTANTE,
                            novoUsuarioCotanteLogins.LOGIN_USUARIO_COTANTE_LOGINS,
                            novoUsuarioCotanteLogins.SENHA_USUARIO_COTANTE_LOGINS, tipoEmail, tipoEmail);

                        //Redireciona para página de confirmação de cadastro
                        if (enviou)
                        {
                            return RedirectToAction("ConfirmacaoCadastro", "CadastroUsuarioCotante");
                        }
                    }
                }
                catch (Exception erro)
                {
                    //ModelState.AddModelError("", "");
                    throw erro;
                }
            }
            else
            {
                return Json("* CPF Inválido", "text/x-json", System.Text.Encoding.UTF8,
                    JsonRequestBehavior.AllowGet);
            }

            cadastroDoUsuarioCotante.ListagemPaises = ListagemPaises();

            return View(cadastroDoUsuarioCotante).ComMensagem("* Erro interno ao salvar os dados cadastrais do Usuário Cotante! Por favor, tente mais tarde!");
        }

        // GET: /CadastroUsuarioCotante/ConfirmacaoCadastro
        public ActionResult ConfirmacaoCadastro()
        {
            return View();
        }

    }
}
