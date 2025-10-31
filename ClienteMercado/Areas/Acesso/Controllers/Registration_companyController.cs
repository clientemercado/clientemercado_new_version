using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Validation;
using System;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace ClienteMercado.Areas.Acesso.Controllers
{
    public class Registration_companyController : Controller
    {
        //
        // GET: /Acesso/Registration-company/

        public ActionResult Index()
        {
            return View();
        }

        //VERIFICA de o CNPJ já está CADASTRADO no BANCO
        [WebMethod]
        public ActionResult VerificaSeCNPJExisteNoBanco(string cnpjDigitado)
        {
            try
            {
                cnpjDigitado = Regex.Replace(cnpjDigitado, "[./-]", "");

                bool cnpjValido = Validacoes.ValidaCNPJ(cnpjDigitado);

                if (cnpjValido)
                {
                    //Consultar o CNPJ digitado
                    empresa_usuario novoCnpj = new empresa_usuario();
                    novoCnpj.CNPJ_EMPRESA_USUARIO = cnpjDigitado;
                    empresa_usuario empresaUsuario = new NCnpjService().ConsultarCnpj(novoCnpj);

                    if (empresaUsuario != null)
                    {
                        //Buscar dados do Usuário Master
                        usuario_empresa dadosUsuarioMaster = new usuario_empresa();
                        dadosUsuarioMaster.ID_CODIGO_EMPRESA = empresaUsuario.ID_CODIGO_EMPRESA;
                        usuario_empresa usuarioEmpresa = new NEmpresaUsuarioService().BuscarDadosDoUsuarioMaster(dadosUsuarioMaster);
                        enderecos_empresa_usuario buscaCepAfins = new enderecos_empresa_usuario();
                        buscaCepAfins.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = usuarioEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                        enderecos_empresa_usuario enderecosEmpresaUsuario = new NCepService().ConsultarCepPorId(buscaCepAfins);

                        //Montando os parâmetros a serem retornados
                        var resultado = new
                        {
                            validacaoCNPJ = "cnpjjacadastrado",
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
                    else
                    {
                        //Montando os parâmetros a serem retornados
                        var resultado = new
                        {
                            validacaoCNPJ = "cnpjnaocadastrado",
                            idEmpresa = "",
                            razaoSocial = "",
                            fantasia = "",
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
                else
                {
                    //Montando os parâmetros a serem retornados
                    var resultado = new
                    {
                        validacaoCNPJ = "cnpjinvalido",
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

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public ActionResult VerificaSeCNPJExisteNoBancoNew(string cnpjDigitado)
        {
            try
            {
                cnpjDigitado = Regex.Replace(cnpjDigitado, "[./-]", "");
                bool cnpjValido = Validacoes.ValidaCNPJ(cnpjDigitado);

                if (cnpjValido)
                {
                    //Consultar o CNPJ digitado
                    EMPRESA_FORNECEDOR novoCnpj = new EMPRESA_FORNECEDOR();
                    novoCnpj.cnpj_empresa_fornecedor = cnpjDigitado;
                    EMPRESA_FORNECEDOR empresaFornecedor = new NCnpjService().ConsultarCnpjNew(novoCnpj);

                    if (empresaFornecedor != null)
                    {
                        //Montando os parâmetros a serem retornados
                        var resultado = new
                        {
                            validacaoCNPJ = "cnpjjacadastrado",
                            idEmpresa = empresaFornecedor.id_empresa_fornecedor,
                            nomeFantasia = empresaFornecedor.nome_fantasia_empresa_fornecedor,
                            ramoAtividade = empresaFornecedor.ramo_atividade_empresa_fornecedor,
                            dataCadastro = empresaFornecedor.data_cadastro_empresa_fornecedor.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                            endereco = empresaFornecedor.endereco_empresa_fornecedor,
                            complemento = empresaFornecedor.complemento_empresa_fornecedor,
                            cep = empresaFornecedor.cep_empresa_fornecedor,
                            cidade = empresaFornecedor.cidade_empresa_fornecedor,
                            uf = empresaFornecedor.uf_empresa_fornecedor,
                            bairro = empresaFornecedor.bairro_empresa_fornecedor
                        };

                        return Json(resultado, "text/x-json", System.Text.Encoding.UTF8,
                            JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Montando os parâmetros a serem retornados
                        var resultado = new
                        {
                            validacaoCNPJ = "cnpjnaocadastrado",
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
                        validacaoCNPJ = "cnpjinvalido",
                    };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Checa existencia do Login no banco
        [WebMethod]
        public ActionResult VerificaSeLoginJaExisteNoBanco(string loginDigitado)
        {
            if (loginDigitado != "")
            {
                try
                {
                    var resultado = new { resultadoLogin = "naoexiste" };

                    NLoginService negocio = new NLoginService();
                    empresa_usuario_logins checarLogin = new empresa_usuario_logins();

                    checarLogin.LOGIN_EMPRESA_USUARIO_LOGINS = loginDigitado;
                    empresa_usuario_logins empresaUsuarioLogins = negocio.ChecarExistenciaLoginUsuarioEmpresa(checarLogin);

                    if (empresaUsuarioLogins != null)
                        resultado = new { resultadoLogin = "jaexiste" };

                    return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao checar o Login digitado!");
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //VALIDAR E-mail
        [WebMethod]
        public ActionResult ValidarEmail(string emailAValidar)
        {
            var resultado = new { emailValido = "" };
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(emailAValidar))
            {
                resultado = new { emailValido = "Ok" };
            }
            else
            {
                resultado = new { emailValido = "NOk" };
            }

            return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //CADASTRO BÁSICO da EMPRESA
        [WebMethod]
        public ActionResult CadastroBasicoDaEmpresa(string cnpj, string razaoSocial, string nomeUsuario, string email1, string login, string passw, string pais)
        {
            try
            {
                var resultado = new { cadastroBasicoEfetuado = "NOk" };

                if (pais == "Brazil")
                    pais = pais.Replace("z", "s");

                empresa_usuario dadosDaEmpresa = new empresa_usuario();

                //BUSCAR CÓD. do PAÍS
                paises_empresa_usuario dadosPais = new NPaisesService().BuscarCodigoDoPaisPeloNomeDoPais(pais);

                //SALVAR DADOS do USUÁRIO
                usuario_empresa usuarioADMNovaEmpresa = new usuario_empresa();

                //POPULANDO OBJ relacionado ao Novo Usuário
                usuarioADMNovaEmpresa.NOME_USUARIO = nomeUsuario;
                usuarioADMNovaEmpresa.CPF_USUARIO_EMPRESA = "56498667591";
                usuarioADMNovaEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO = 1;
                usuarioADMNovaEmpresa.PAIS_USUARIO_EMPRESA = dadosPais.ID_PAISES_EMPRESA_USUARIO;
                usuarioADMNovaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = 2;
                usuarioADMNovaEmpresa.ID_CODIGO_PROFISSAO = 1;
                usuarioADMNovaEmpresa.TELEFONE1_USUARIO_EMPRESA = "(27)99636-6414";
                usuarioADMNovaEmpresa.RECEBER_EMAILS_USUARIO = true;
                usuarioADMNovaEmpresa.DATA_CADASTRO_USUARIO = DateTime.Now;
                usuarioADMNovaEmpresa.DATA_ULTIMA_ATUALIZACAO_USUARIO = DateTime.Now;
                usuarioADMNovaEmpresa.ATIVA_INATIVO_USUARIO = true;
                usuarioADMNovaEmpresa.CADASTRO_CONFIRMADO = false;
                usuarioADMNovaEmpresa.ID_CODIGO_EMPRESA = (Int32)Sessao.IdUsuarioLogado;
                usuarioADMNovaEmpresa.VER_COTACAO_AVULSA = false;
                usuarioADMNovaEmpresa.USUARIO_MASTER = true;

                //Salvando dados para o Login do Usuário da Empresa
                empresa_usuario_logins novoUsuarioEmpresaLogins = new empresa_usuario_logins();

                novoUsuarioEmpresaLogins.LOGIN_EMPRESA_USUARIO_LOGINS = login;
                novoUsuarioEmpresaLogins.SENHA_EMPRESA_USUARIO_LOGINS = Utils.Utilitarios.Hash.GerarHashMd5(passw);
                novoUsuarioEmpresaLogins.EMAIL1_USUARIO = email1;

                //SALVAR DADOS do LOGIN
                usuarioADMNovaEmpresa.empresa_usuario_logins.Add(novoUsuarioEmpresaLogins);

                cnpj = Regex.Replace(cnpj, "[./-]", "");

                //POPULAR DADOS da EMPRESA
                dadosDaEmpresa.ID_CODIGO_TIPO_EMPRESA_USUARIO = 1;
                dadosDaEmpresa.CNPJ_EMPRESA_USUARIO = cnpj;
                dadosDaEmpresa.RAZAO_SOCIAL_EMPRESA = razaoSocial;
                dadosDaEmpresa.NOME_FANTASIA_EMPRESA = razaoSocial;
                dadosDaEmpresa.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = 2;
                dadosDaEmpresa.EMAIL1_EMPRESA = email1;
                dadosDaEmpresa.TELEFONE1_EMPRESA_USUARIO = "(27)99636-6414";
                dadosDaEmpresa.PAIS_EMPRESA_USUARIO = dadosPais.ID_PAISES_EMPRESA_USUARIO;
                dadosDaEmpresa.DATA_CADASTRO_EMPRESA = DateTime.Now;
                dadosDaEmpresa.DATA_ULTIMA_ATUALIZACAO_EMPRESA = DateTime.Now;
                dadosDaEmpresa.ACEITACAO_TERMOS_POLITICAS = true;
                dadosDaEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA = 1;
                dadosDaEmpresa.ATIVA_INATIVA_EMPRESA = true;
                dadosDaEmpresa.EMPRESA_ADMISTRADORA = false;
                dadosDaEmpresa.ID_GRUPO_ATIVIDADES = 1;

                //SALVAR DADOS do USUÁRIO
                dadosDaEmpresa.usuario_empresa.Add(usuarioADMNovaEmpresa);

                //Cadastrar de acordo com o tipo de Usuário (1-de nova Empresa / 2-de Empresa já existente)
                if (Sessao.IdEmpresaUsuario == null || Sessao.IdEmpresaUsuario == 0)
                {
                    empresa_usuario empresaUsuario = new NEmpresaUsuarioService().GravarEmpresaUsuario(dadosDaEmpresa);
                    if (empresaUsuario != null)
                        resultado = new { cadastroBasicoEfetuado = "Ok" };
                }

                return Json(resultado, "text/x-json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException erro)
            {
                foreach (var eve in erro.EntityValidationErrors)
                {
                    Console.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Erro: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw erro;
            }
        }

    }
}
