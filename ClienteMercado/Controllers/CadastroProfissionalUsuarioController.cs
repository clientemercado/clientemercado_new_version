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
    public class CadastroProfissionalUsuarioController : Controller
    {
        //
        // GET: /CadastroProfissionalServicos/

        public ActionResult CadastroProfissional()
        {
            CadastroProfissionalUsuario usuario = new CadastroProfissionalUsuario();
            var lista = ListagemPaises();
            //var listaGruposAtividades = ListagemGruposAtividades();

            usuario.ListagemPaises = lista;
            //usuario.ListagemGruposAtividadeEmpresa = listaGruposAtividades;

            return View(usuario);
        }

        private static List<SelectListItem> ListagemPaises()
        {
            List<SelectListItem> listPaises = new List<SelectListItem>();
            listPaises.Add(new SelectListItem { Text = "Brasil", Value = "Brasil" });
            listPaises.Add(new SelectListItem { Text = "Argentina", Value = "Argentina" });
            listPaises.Add(new SelectListItem { Text = "Bolívia", Value = "Bolívia" });
            listPaises.Add(new SelectListItem { Text = "Colômbia", Value = "Colômbia" });
            listPaises.Add(new SelectListItem { Text = "Equador", Value = "Equador" });
            listPaises.Add(new SelectListItem { Text = "México", Value = "Mexico" });
            listPaises.Add(new SelectListItem { Text = "Peru", Value = "Peru" });
            listPaises.Add(new SelectListItem { Text = "Uruguai", Value = "Uruguai" });
            listPaises.Add(new SelectListItem { Text = "Venezuela", Value = "Venezuela" });

            return listPaises;
        }

        private static List<SelectListItem> ListagemGruposAtividades()
        {
            //Buscar os Grupos de Atividades da empresariais
            NGruposAtividadesEmpresaProfissionalService negocioGruposAtividadesEmpresaProfissional =
                new NGruposAtividadesEmpresaProfissionalService();
            List<grupo_atividades_empresa> listaGruposAtividades =
                negocioGruposAtividadesEmpresaProfissional.ListaGruposAtividadesEmpresaProfissional();

            List<SelectListItem> listGruposAtividades = new List<SelectListItem>();

            listGruposAtividades.Add(new SelectListItem { Text = "Selecione o grupo de atividades...", Value = "" });

            foreach (var gruposAtividades in listaGruposAtividades)
            {
                listGruposAtividades.Add(new SelectListItem
                {
                    Text = gruposAtividades.DESCRICAO_ATIVIDADE,
                    Value = gruposAtividades.ID_GRUPO_ATIVIDADES.ToString()
                });
            }

            return listGruposAtividades;
        }

        //Verifica se o CPF digitado para o Profissional de Serviços, já existe no banco
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
                            profissional_usuario novoCpf = new profissional_usuario();

                            novoCpf.CPF_PROFISSIONAL_USUARIO = cpfDigitado;
                            profissional_usuario profissionalUsuario = negocio.ConsultarCpfProfissional(novoCpf);

                            if (profissionalUsuario != null)
                            {
                                return Json("* CPF já cadastrado como Profissional para outro Usuário!", "text/x-json", System.Text.Encoding.UTF8,
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
                        ModelState.AddModelError("", "Erro ao validar CPF do Profissional de Serviços!");
                    }
                }
            }
            else
            {
                if (cpfDigitado != "")
                {
                    return Json("* Dígitos insuficientes! Entre com o CPF completo.", "text/x-json", System.Text.Encoding.UTF8,
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
                            NProfissionalUsuarioService negocio = new NProfissionalUsuarioService();
                            profissional_usuario_logins novoEmail = new profissional_usuario_logins();

                            novoEmail.EMAIL1_USUARIO = emailDigitado;
                            profissional_usuario_logins profissionalUsuarioLogins = negocio.ConsultarEmailUsuarioProfissional(novoEmail);

                            if (profissionalUsuarioLogins != null)
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

        //Checa existencia do Login para o Profissional de Serviços no banco
        [WebMethod]
        public ActionResult VerificaSeLoginJaExisteNoBanco(string loginDigitado)
        {
            if (loginDigitado != "")
            {
                try
                {
                    NLoginService negocio = new NLoginService();
                    profissional_usuario_logins checarLogin = new profissional_usuario_logins();

                    checarLogin.LOGIN_PROFISSIONAL_USUARIO_LOGINS = loginDigitado;
                    profissional_usuario_logins profissionalUsuarioLogins = negocio.ChecarExistenciaLoginUsuarioProfissional(checarLogin);

                    if (profissionalUsuarioLogins != null)
                    {
                        return Json("*Login já cadastrado para um outro Usuário Profissional. Tente outro Login!", "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao checar o Login digitado para o Profissional!");
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
                                    //estado = enderecos_empresa_usuario.cidades_empresa_usuario.UF_CIDADE_EMPRESA_USUARIO,
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
        public ActionResult CadastroProfissional(CadastroProfissionalUsuario cadastroDoUsuarioProfissionalDeServicos)
        {
            //Valida novamente o CPF, evitando CPF´s alterados no código
            string cpfDigitado = Regex.Replace(cadastroDoUsuarioProfissionalDeServicos.CPF_PROFISSIONAL_USUARIO, "[./-]", "");
            bool cpfValido = Validacoes.ValidaCPF(cpfDigitado);

            if (cpfValido)
            {
                try
                {
                    //Salvando os dados do Profissional de Serviços / Tipo Empresa
                    NProfissionalUsuarioService negocio = new NProfissionalUsuarioService();
                    profissional_usuario novoProfissionalUsuario = new profissional_usuario();

                    novoProfissionalUsuario.ID_CODIGO_TIPO_EMPRESA_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_TIPO_EMPRESA_USUARIO;
                    novoProfissionalUsuario.CPF_PROFISSIONAL_USUARIO = cpfDigitado;
                    novoProfissionalUsuario.ID_GRUPO_ATIVIDADES =
                        cadastroDoUsuarioProfissionalDeServicos.ID_GRUPO_ATIVIDADES;
                    novoProfissionalUsuario.NOME_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.NOME_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.FOTO_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.FOTO_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.NOME_COMERCIAL_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.NOME_COMERCIAL_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.ID_CODIGO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    novoProfissionalUsuario.COMPLEMENTO_ENDERECO_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.COMPLEMENTO_ENDERECO_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.PAIS_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.PAIS_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.ID_CODIGO_PROFISSAO =
                        cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_PROFISSAO;
                    novoProfissionalUsuario.TELEFONE1_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.TELEFONE1_PROFISSIONAL_USUARIO;
                    novoProfissionalUsuario.RECEBER_EMAILS_PROFISSIONAL_USUARIO = true;
                    novoProfissionalUsuario.ID_CODIGO_TIPO_CONTRATO_COTADA = 1;
                    novoProfissionalUsuario.DATA_CADASTRO_PROFISSIONAL_USUARIO = DateTime.Now;
                    novoProfissionalUsuario.DATA_ULTIMA_ATUALIZACAO_PROFISSIONAL_USUARIO = DateTime.Now;
                    novoProfissionalUsuario.ATIVA_INATIVO_PROFISSIONAL_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.ATIVA_INATIVO_PROFISSIONAL_USUARIO;

                    //Salvando dados do Usuário ligado ao Profissional
                    usuario_profissional novoUsuarioProfissional = new usuario_profissional();

                    novoUsuarioProfissional.ID_CODIGO_TIPO_EMPRESA_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_TIPO_EMPRESA_USUARIO;
                    novoUsuarioProfissional.CPF_USUARIO_PROFISSIONAL = cpfDigitado;
                    novoUsuarioProfissional.NOME_USUARIO_PROFISSIONAL = cadastroDoUsuarioProfissionalDeServicos.NOME_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.ID_CODIGO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    novoUsuarioProfissional.COMPLEMENTO_ENDERECO_USUARIO_PROFISSIONAL =
                        cadastroDoUsuarioProfissionalDeServicos.COMPLEMENTO_ENDERECO_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.PAIS_USUARIO_PROFISSIONAL =
                        cadastroDoUsuarioProfissionalDeServicos.PAIS_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.ID_CODIGO_PROFISSAO = cadastroDoUsuarioProfissionalDeServicos.ID_CODIGO_PROFISSAO;
                    novoUsuarioProfissional.TELEFONE1_USUARIO_PROFISSIONAL =
                        cadastroDoUsuarioProfissionalDeServicos.TELEFONE1_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.RECEBER_EMAILS_USUARIO_PROFISSIONAL =
                        cadastroDoUsuarioProfissionalDeServicos.RECEBER_EMAILS_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.DATA_CADASTRO_USUARIO_PROFISSIONAL = DateTime.Now;
                    novoUsuarioProfissional.DATA_ULTIMA_ATUALIZACAO_USUARIO_PROFISSIONAL = DateTime.Now;
                    novoUsuarioProfissional.ATIVA_INATIVO_USUARIO_PROFISSIONAL =
                        cadastroDoUsuarioProfissionalDeServicos.ATIVA_INATIVO_PROFISSIONAL_USUARIO;
                    novoUsuarioProfissional.CADASTRO_CONFIRMADO = false;
                    novoUsuarioProfissional.USUARIO_MASTER = cadastroDoUsuarioProfissionalDeServicos.USUARIO_MASTER;

                    //Salvando dados para o Login do Usuário
                    profissional_usuario_logins novoLoginUsuarioProfissional = new profissional_usuario_logins();

                    novoLoginUsuarioProfissional.LOGIN_PROFISSIONAL_USUARIO_LOGINS =
                        cadastroDoUsuarioProfissionalDeServicos.LOGIN_PROFISSIONAL_USUARIO_LOGINS;
                    novoLoginUsuarioProfissional.SENHA_PROFISSIONAL_USUARIO_LOGINS = Hash.GerarHashMd5(cadastroDoUsuarioProfissionalDeServicos.SENHA_PROFISSIONAL_USUARIO_LOGINS);
                    novoLoginUsuarioProfissional.EMAIL1_USUARIO = cadastroDoUsuarioProfissionalDeServicos.EMAIL1_USUARIO;

                    //Atribuir os objetos para que ocorram as gravaçõesdos dados em cascata e se necessário, o rollback
                    novoUsuarioProfissional.profissional_usuario_logins.Add(novoLoginUsuarioProfissional);
                    novoProfissionalUsuario.usuario_profissional.Add(novoUsuarioProfissional);

                    profissional_usuario profissionalUsuario = negocio.GravarProfissionalUsuario(novoProfissionalUsuario);

                    //Envio de e-mail solicitando a confirmação de gravação de dados cadastrais do PROFISSIONAL DE SERVIÇOS no ClienteMercado (Tipo 3)
                    if (profissionalUsuario != null)
                    {
                        int tipoEmail = 3;

                        EnviarEmail enviarEmail = new EnviarEmail();

                        bool enviou = enviarEmail.EnviandoEmail(cadastroDoUsuarioProfissionalDeServicos.EMAIL1_USUARIO,
                            cadastroDoUsuarioProfissionalDeServicos.NOME_COMERCIAL_PROFISSIONAL_USUARIO,
                            novoLoginUsuarioProfissional.LOGIN_PROFISSIONAL_USUARIO_LOGINS,
                            novoLoginUsuarioProfissional.SENHA_PROFISSIONAL_USUARIO_LOGINS, tipoEmail, tipoEmail);

                        //Redireciona para página de confirmação de cadastro
                        if (enviou)
                        {
                            return RedirectToAction("ConfirmacaoCadastro", "CadastroProfissionalUsuario");
                        }
                    }

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "");
                }
            }
            else
            {
                return Json("* CPF Inválido", "text/x-json", System.Text.Encoding.UTF8,
                    JsonRequestBehavior.AllowGet);
            }

            cadastroDoUsuarioProfissionalDeServicos.ListagemPaises = ListagemPaises();

            return View(cadastroDoUsuarioProfissionalDeServicos).ComMensagem("* Erro interno ao salvar os dados cadastrais do Profissional Usuário! Por favor, tente mais tarde!");
        }

        // GET: /CadastroProfissionalUsuario/ConfirmacaoCadastro
        public ActionResult ConfirmacaoCadastro()
        {
            return View();
        }
    }
}
