using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Controllers
{
    public class CadastroEmpresaUsuarioController : Controller
    {
        //
        // GET: /Cadastro/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CadastroEmpresa/
        public ActionResult CadastroEmpresa()
        {
            CadastroEmpresa empresa = new CadastroEmpresa();
            var listaPaises = ListagemPaises();

            empresa.ListagemPaises = listaPaises;
            empresa.ListagemEstados = ListagemEstados();

            return View(empresa);
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

        //Checa existencia do CNPJ no banco
        [WebMethod]
        public ActionResult VerificaSeCNPJExisteNoBanco(string cnpjDigitado)
        {
            if (cnpjDigitado.Length == 18)
            {
                if (ModelState.IsValid)
                {
                    cnpjDigitado = Regex.Replace(cnpjDigitado, "[./-]", "");

                    try
                    {
                        bool cnpjValido = Validacoes.ValidaCNPJ(cnpjDigitado);

                        if (cnpjValido)
                        {
                            //Consultar o CNPJ digitado
                            NCnpjService negocioCnpj = new NCnpjService();
                            empresa_usuario novoCnpj = new empresa_usuario();

                            novoCnpj.CNPJ_EMPRESA_USUARIO = cnpjDigitado;
                            empresa_usuario empresaUsuario = negocioCnpj.ConsultarCnpj(novoCnpj);

                            if (empresaUsuario != null)
                            {
                                //Buscar dados do Usuário Master
                                NEmpresaUsuarioService negocioUsuarioMaster = new NEmpresaUsuarioService();
                                usuario_empresa dadosUsuarioMaster = new usuario_empresa();

                                dadosUsuarioMaster.ID_CODIGO_EMPRESA = empresaUsuario.ID_CODIGO_EMPRESA;
                                usuario_empresa usuarioEmpresa = negocioUsuarioMaster.BuscarDadosDoUsuarioMaster(dadosUsuarioMaster);

                                //Trazer dados de localização do usuário
                                NCepService negociosCep = new NCepService();
                                enderecos_empresa_usuario buscaCepAfins = new enderecos_empresa_usuario();

                                buscaCepAfins.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = usuarioEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                                enderecos_empresa_usuario enderecosEmpresaUsuario = negociosCep.ConsultarCepPorId(buscaCepAfins);

                                //Montando os parâmetros a serem retornados
                                var resultado = new
                                {
                                    mensagem = "* CNPJ já cadastrado! Existe uma empresa cadastrada com esse CNPJ.",
                                    idEmpresa = empresaUsuario.ID_CODIGO_EMPRESA,
                                    razaoSocial = empresaUsuario.RAZAO_SOCIAL_EMPRESA,
                                    fantasia = empresaUsuario.NOME_FANTASIA_EMPRESA,
                                    ehMaster = usuarioEmpresa.USUARIO_MASTER,
                                    confirmacaoDeCadastro = usuarioEmpresa.CADASTRO_CONFIRMADO,
                                    nomeUsuarioMaster = usuarioEmpresa.NOME_USUARIO,
                                    eMailOne = empresaUsuario.EMAIL1_EMPRESA,
                                    telefoneOne = empresaUsuario.TELEFONE1_EMPRESA_USUARIO,
                                    pais = empresaUsuario.PAIS_EMPRESA_USUARIO,
                                    cep = enderecosEmpresaUsuario.CEP_ENDERECO_EMPRESA_USUARIO,
                                    endereco = enderecosEmpresaUsuario.TIPO_LOGRADOURO_EMPRESA_USUARIO + " " + enderecosEmpresaUsuario.LOGRADOURO_CEP_EMPRESA_USUARIO,
                                    complementoEndereco = empresaUsuario.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO,
                                    ufEstado = enderecosEmpresaUsuario.cidades_empresa_usuario.estados_empresa_usuario.ID_ESTADOS_EMPRESA_USUARIO,
                                    cidade = enderecosEmpresaUsuario.cidades_empresa_usuario.CIDADE_EMPRESA_USUARIO,
                                    bairro = enderecosEmpresaUsuario.bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO,
                                    grupoAtividades = empresaUsuario.ID_GRUPO_ATIVIDADES
                                };

                                return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                    JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            //Montando os parâmetros a serem retornados
                            var resultado = new
                            {
                                mensagem = "* CNPJ Inválido.",
                                idEmpresa = "",
                                razaoSocial = "",
                                fantasia = "",
                                ramoEmpresarial = "",
                                ehMaster = "",
                                confirmacaoDeCadastro = "",
                                nomeUsuarioMaster = "",
                                eMailOne = "",
                                telefoneOne = "",
                                pais = "",
                                cep = "",
                                endereco = "",
                                complementoEndereco = "",
                                ufEstado = "",
                                cidade = "",
                                bairro = "",
                                grupoAtividades = ""
                            };

                            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Erro ao validar CNPJ da empresa!");
                    }
                }
            }
            else
            {
                if (cnpjDigitado != "")
                {
                    //Montando os parâmetros a serem retornados
                    var resultado = new
                    {
                        mensagem = "* Dígitos insuficientes! Entre com o CNPJ completo.",
                        idEmpresa = "",
                        razaoSocial = "",
                        fantasia = "",
                        ramoEmpresarial = "",
                        ehMaster = "",
                        confirmacaoDeCadastro = "",
                        nomeUsuarioMaster = "",
                        eMailOne = "",
                        telefoneOne = "",
                        pais = "",
                        cep = "",
                        endereco = "",
                        complementoEndereco = "",
                        ufEstado = "",
                        cidade = "",
                        bairro = "",
                        grupoAtividades = ""
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
                            empresa_usuario novoEmail = new empresa_usuario();

                            novoEmail.EMAIL1_EMPRESA = emailDigitado;
                            empresa_usuario empresaUsuario = negocio.ConsultarEmailEmpresa(novoEmail);

                            if (empresaUsuario != null)
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
                    return Json("* caracteres insuficientes! Entre com o E-mail completo.", "text/x-json", System.Text.Encoding.UTF8,
                        JsonRequestBehavior.AllowGet);
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
                                    bairro = enderecos_empresa_usuario.bairros_empresa_usuario.BAIRRO_CIDADE_EMPRESA_USUARIO
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
        public ActionResult CadastroEmpresa(CadastroEmpresa cadastroDaEmpresa)
        {
            //Valida novamente o CNPJ, evitando CNPJ´s alterados no código
            string cnpjDigitado = Regex.Replace(cadastroDaEmpresa.CNPJ_CPF_EMPRESA_USUARIO, "[./-]", "");
            bool cnpjValido = Validacoes.ValidaCNPJ(cnpjDigitado);

            if (cnpjValido)
            {
                try
                {
                    empresa_usuario novaEmpresa = new empresa_usuario();

                    novaEmpresa.CNPJ_EMPRESA_USUARIO = cnpjDigitado;
                    novaEmpresa.ID_GRUPO_ATIVIDADES = cadastroDaEmpresa.ID_GRUPO_ATIVIDADES;
                    novaEmpresa.RAZAO_SOCIAL_EMPRESA = cadastroDaEmpresa.RAZAO_SOCIAL_EMPRESA;
                    novaEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO = cadastroDaEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO;
                    novaEmpresa.NOME_FANTASIA_EMPRESA = cadastroDaEmpresa.NOME_FANTASIA_EMPRESA;
                    //novaEmpresa.PAIS_EMPRESA_USUARIO = cadastroDaEmpresa.PAIS_EMPRESA_USUARIO;
                    novaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    novaEmpresa.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO =
                        cadastroDaEmpresa.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO;
                    novaEmpresa.EMAIL1_EMPRESA = cadastroDaEmpresa.EMAIL1_EMPRESA;
                    novaEmpresa.TELEFONE1_EMPRESA_USUARIO = cadastroDaEmpresa.TELEFONE1_EMPRESA_USUARIO;
                    novaEmpresa.RECEBER_EMAILS_EMPRESA = cadastroDaEmpresa.RECEBER_EMAILS_EMPRESA;
                    novaEmpresa.ACEITACAO_TERMOS_POLITICAS = cadastroDaEmpresa.ACEITACAO_TERMOS_POLITICAS;
                    novaEmpresa.DATA_CADASTRO_EMPRESA = DateTime.Now;
                    novaEmpresa.DATA_ULTIMA_ATUALIZACAO_EMPRESA = DateTime.Now;
                    novaEmpresa.ATIVA_INATIVA_EMPRESA = cadastroDaEmpresa.ATIVA_INATIVA_EMPRESA;
                    novaEmpresa.EMPRESA_ADMISTRADORA = cadastroDaEmpresa.EMPRESA_ADMISTRADORA;
                    novaEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA = 1;

                    //Testa em caso de cadastro de outros usuários, cuja empresa já se encontra cadastrada e se for o caso, guarda na session
                    if (cadastroDaEmpresa.ID_EMPRESA != null)
                    {
                        Sessao.IdEmpresaUsuario = Convert.ToInt32(cadastroDaEmpresa.ID_EMPRESA);
                    }

                    //Os dados do cadastro da Empresa foram armazenados na Session pra serem cadastrados somente depois de preenchidos os dados do Usuário
                    Sessao.tipoDeUsuario = novaEmpresa;

                    return RedirectToAction("CadastroUsuario", "CadastroUsuarioEmpresa");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao Cadastrar Empresa");
                }
            }
            else
            {
                return Json("* CNPJ Inválido", "text/x-json", System.Text.Encoding.UTF8,
                    JsonRequestBehavior.AllowGet);
            }

            return View(cadastroDaEmpresa);
        }

        public ActionResult Termos()
        {
            return View();
        }

        public ActionResult Politica()
        {
            return View();
        }
    }
}
