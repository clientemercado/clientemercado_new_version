using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClienteMercado.Controllers
{
    public class UsuarioProfissionalController : Controller
    {
        //
        // GET: /UsuarioProfissional/
        //[Authorize]
        public ActionResult PerfilUsuarioProfissional(string nmU, string nmE, int tpL)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);
                ViewBag.tipoLogin = tpL;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Acionada em caso de primeiro acesso, onde o usuário configura Atividades, Assinatura e modo de Pagamento,
        //para o uso do sistema
        public ActionResult Configuracoes(int tpL, string nmU, string nmE)
        {
            if (Session["IdUsuarioLogado"] != null)
            {
                //Carrega listas Atividades Profissionais / Planos Assinatura / Meios de Pagamento
                CadastroProfissionalUsuario profissionalUsuario = new CadastroProfissionalUsuario();

                var listaGruposAtividades = ListagemGruposAtividades();

                ViewBag.tipoLogin = tpL;
                ViewBag.nomeUsuarioLogado = MD5Crypt.Descriptografar(nmU);
                ViewBag.nomeEmpresaLogado = MD5Crypt.Descriptografar(nmE);

                profissionalUsuario.ListagemGruposAtividadeEmpresa = listaGruposAtividades;

                return View(profissionalUsuario);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        //Carrega os Grupos de Atividades
        private static List<SelectListItem> ListagemGruposAtividades()
        {
            //Buscar os Grupos de Atividades da empresariais
            NGruposAtividadesEmpresaProfissionalService negocioGruposAtividadesEmpresaProfissional =
                new NGruposAtividadesEmpresaProfissionalService();
            List<grupo_atividades_empresa> listaGruposAtividades =
                negocioGruposAtividadesEmpresaProfissional.ListaGruposAtividadesEmpresaProfissional();

            List<SelectListItem> listGruposAtividades = new List<SelectListItem>();

            listGruposAtividades.Add(new SelectListItem { Text = "Selecione...", Value = "" });

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

        //Confirmação do cadastro do Usuário Profissional
        public ActionResult ConfirmarCadastroUsuario(string login, string senha)
        {
            try
            {
                //Booleano q indica se é ou nao o primeiro acesso. Valor inicial indica q não é.
                bool primeiroAcesso = false;

                //Login para Usuário Profissional de Serviços
                NLoginService negocioLoginProfissional = new NLoginService();
                profissional_usuario_logins confirmaCadastroProfissional = new profissional_usuario_logins();

                confirmaCadastroProfissional.LOGIN_PROFISSIONAL_USUARIO_LOGINS = login;
                confirmaCadastroProfissional.SENHA_PROFISSIONAL_USUARIO_LOGINS = senha;

                profissional_usuario_logins profissionalUsuarioLogins =
                    negocioLoginProfissional.ConsultarLoginProfissionalServicos(confirmaCadastroProfissional);

                //Se campo CADASTRO_CONFIRMADO não estiver setado como verdadeiro, então inicia-se o processo de 
                //confirmação de cadastro (OBS: Isso acontecia antes, mas agora mandei q a confirmação de cadastro ocorra depois da configuração)
                //if (profissionalUsuarioLogins.usuario_profissional.CADASTRO_CONFIRMADO != true)
                if (profissionalUsuarioLogins.usuario_profissional.CADASTRO_CONFIRMADO == false && profissionalUsuarioLogins.usuario_profissional.USUARIO_MASTER)
                {
                    NProfissionalUsuarioService negocioProfissionalUsuario = new NProfissionalUsuarioService();
                    usuario_profissional confirmaCadastroUsuarioProfissional = new usuario_profissional();

                    //confirmaCadastroUsuarioProfissional.ID_CODIGO_PROFISSIONAL_USUARIO =
                    //    profissionalUsuarioLogins.ID_CODIGO_USUARIO_PROFISSIONAL;

                    //usuario_profissional profissionalUsuario =
                    //    negocioProfissionalUsuario.ConfirmarCadastroUsuarioProfissional(confirmaCadastroUsuarioProfissional);

                    //if (profissionalUsuario != null)
                    //{
                    Sessao.IdUsuarioLogado =
                        profissionalUsuarioLogins.usuario_profissional.ID_CODIGO_USUARIO_PROFISSIONAL;
                    Sessao.IdEmpresaUsuario =
                        profissionalUsuarioLogins.usuario_profissional.profissional_usuario
                            .ID_CODIGO_PROFISSIONAL_USUARIO;
                    primeiroAcesso = true;
                    //}
                }

                //Esse redirecionamento ocorrerá de qualquer forma. Se o cadastro e existência do usuário forem confirmados no
                //sistema, o método PerfilUsuarioProfissional permitirá o acesso, caso contrário, será direcionado para a página de Login

                if (primeiroAcesso)
                {
                    return RedirectToAction("Configuracoes", "UsuarioProfissional",
                             new
                             {
                                 tpL = 1,
                                 nmU = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.NOME_USUARIO_PROFISSIONAL),
                                 nmE = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.profissional_usuario.NOME_COMERCIAL_PROFISSIONAL_USUARIO)
                             });
                }

                return RedirectToAction("PerfilUsuarioProfissional", "UsuarioProfissional",
                        new
                        {
                            nmU = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.NOME_USUARIO_PROFISSIONAL),
                            nmE = MD5Crypt.Criptografar(profissionalUsuarioLogins.usuario_profissional.profissional_usuario.NOME_COMERCIAL_PROFISSIONAL_USUARIO)
                        });
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //AutoComplete
        public JsonResult AutoCompleteRamosAtividade(string term, int idRamoAtividade)
        {
            //Buscar lista de atividades ligadas ao grupo de atividades da empresa
            NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();

            List<produtos_servicos_empresa_profissional> listaAtividadesEmpresa =
                negocioProdutosServicosEmpresaProfissional.ListaAtividadesEmpresaProfissional(idRamoAtividade);

            var lista = (from t in listaAtividadesEmpresa
                         where t.DESCRICAO_PRODUTO_SERVICO.ToLower().Contains(term.ToLower())
                         select new { t.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS, t.DESCRICAO_PRODUTO_SERVICO }).Distinct().ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Configuracoes(CadastroProfissionalUsuario configurarPerfil, FormCollection form)
        {
            int responsavelCobranca = 2;
            int idRegistroFinanceiro = 0;
            string[] listaIDsProdutosServicos, listaDescricaoProdutosServicos;

            listaIDsProdutosServicos = configurarPerfil.RAMOS_ATIVIDADES_SELECIONADOS.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            listaDescricaoProdutosServicos =
                configurarPerfil.DESCRICAO_RAMOS_ATIVIDADES_SELECIONADOS_ORIGINAL.Split(",".ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

            //Salvando os dados da configuração para o PROFISSIONAL de SERVIÇOS através do USUÁRIO MASTER
            try
            {
                //Inserir novos Produtos / Serviços vinculados a um Grupo de Atividades
                NProdutosServicosEmpresaProfissionalService negocioProdutosServicosEmpresaProfissional = new NProdutosServicosEmpresaProfissionalService();
                produtos_servicos_empresa_profissional novoProdutoServicoEmpresaProfissional = new produtos_servicos_empresa_profissional();

                //Atividades praticadas pelo Profissional de Serviços
                NAtividadesProfissionalService negocioAtividadesProfissional = new NAtividadesProfissionalService();
                atividades_profissional novasAtividadesProfissional = new atividades_profissional();

                //Configurar atividades
                for (int i = 0; i < listaIDsProdutosServicos.LongLength; i++)
                {
                    //Salvando os novos Produtos/Serviços na tabela PRODUTOS_SERVICOS_EMPRESA_PROFISSIONAL, se ainda não existirem no banco
                    if (listaIDsProdutosServicos[i].IndexOf('a') > 0)
                    {
                        novoProdutoServicoEmpresaProfissional.ID_GRUPO_ATIVIDADES = configurarPerfil.ID_GRUPO_ATIVIDADES;
                        novoProdutoServicoEmpresaProfissional.DESCRICAO_PRODUTO_SERVICO = listaDescricaoProdutosServicos[i];

                        produtos_servicos_empresa_profissional produtosServicosEmpresaProfissional =
                            negocioProdutosServicosEmpresaProfissional.GravarNovoProdutoServicoEmpresaProfissional(novoProdutoServicoEmpresaProfissional);

                        if (produtosServicosEmpresaProfissional != null)
                        {
                            listaIDsProdutosServicos[i] = produtosServicosEmpresaProfissional.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS.ToString();
                        }
                    }

                    //Salvando os ramos de atividades informados e praticados pelo profissiona, na tabela ATIVIDADES_PROFISSIONAL
                    novasAtividadesProfissional.ID_CODIGO_PROFISSIONAL_USUARIO =
                        Convert.ToInt32(Session["IdEmpresaUsuario"]);
                    novasAtividadesProfissional.ID_GRUPO_ATIVIDADES = configurarPerfil.ID_GRUPO_ATIVIDADES;
                    novasAtividadesProfissional.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS =
                        Convert.ToInt32(listaIDsProdutosServicos[i]);

                    atividades_profissional atividadesProfissional =
                        negocioAtividadesProfissional.GravarAtividadeProdutoServicoProfissional(novasAtividadesProfissional);
                }

                //Salvar dados financeiros
                if (configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA != 1)//Trata dos tipos de contrato que exijam pagamento
                {
                    //Salvando e gerando o registro financeiro da cobrança para o profissional de serviços
                    NFinanceiroCobrancaFaturamentoUsuarioProfissionalService negocioFinanceiroCobrancaFaturamentoUsuarioProfissional =
                        new NFinanceiroCobrancaFaturamentoUsuarioProfissionalService();

                    financeiro_cobranca_faturamento_usuario_profissional novoCobrancaFaturamentoUsuarioProfissional
                        = new financeiro_cobranca_faturamento_usuario_profissional();

                    novoCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_TIPO_CONTRATO_COTADA =
                        configurarPerfil.ID_CODIGO_TIPO_CONTRATO_COTADA;
                    novoCobrancaFaturamentoUsuarioProfissional.ID_CODIGO_USUARIO_PROFISSIONAL =
                        Convert.ToInt32(Session["IdEmpresaUsuario"]);
                    novoCobrancaFaturamentoUsuarioProfissional.ID_MEIO_PAGAMENTO =
                        configurarPerfil.ID_MEIO_PAGAMENTO;
                    novoCobrancaFaturamentoUsuarioProfissional.VENCIMENTO_FATURA_USUARIO_PROFISSIONAL = DateTime.Now;
                    novoCobrancaFaturamentoUsuarioProfissional.DIAS_TOLERANCIA_COBRANCA_ANTES_POS_VENCIMENTO = 5;
                    novoCobrancaFaturamentoUsuarioProfissional.PARCELA_PAGA_COBRANCA_FATURAMENTO = false;
                    novoCobrancaFaturamentoUsuarioProfissional.DATA_PAGAMENTO_COBRANCA_FATURAMENTO =
                        Convert.ToDateTime("01/01/1900");   //Estou inserindo a data assim pois não sei ainda pq esta data está como requerida na tabela.

                    financeiro_cobranca_faturamento_usuario_profissional
                        financeiroCobrancaFaturamentoUsuarioProfissional =
                            negocioFinanceiroCobrancaFaturamentoUsuarioProfissional.GravaCobrancaUsuarioProfissional
                                (novoCobrancaFaturamentoUsuarioProfissional);

                    if (financeiroCobrancaFaturamentoUsuarioProfissional != null)
                    {
                        //Armazenda o identificador do registro financeiro gerado para planos cobrados
                        idRegistroFinanceiro = financeiroCobrancaFaturamentoUsuarioProfissional.ID_COBRANCA_FATURAMENTO_USUARIO_PROFISSIONAL;
                    }

                    //Confirma o cadastro do Usuário após especificados os dados de configurações de Atividades / Assinatura
                    NProfissionalUsuarioService negocioProfissionalUsuario = new NProfissionalUsuarioService();
                    usuario_profissional confirmaCadastroUsuarioProfissional = new usuario_profissional();

                    confirmaCadastroUsuarioProfissional.ID_CODIGO_PROFISSIONAL_USUARIO =
                        Convert.ToInt32(Session["IdUsuarioLogado"]);

                    usuario_profissional confirmandoCadastro =
                        negocioProfissionalUsuario.ConfirmarCadastroUsuarioProfissional(
                            confirmaCadastroUsuarioProfissional);

                    //Redireciona para a emissão da cobrança
                    if (confirmandoCadastro != null)
                    {
                        return RedirectToAction("ValidarAssinatura", "FinanceiroClienteMercado",
                             new
                             {
                                 tpL = configurarPerfil.TIPO_LOGIN,
                                 nmU = MD5Crypt.Criptografar(configurarPerfil.NOME_PROFISSIONAL_USUARIO),
                                 nmE = MD5Crypt.Criptografar(configurarPerfil.NOME_COMERCIAL_PROFISSIONAL_USUARIO),
                                 dPc = MD5Crypt.Criptografar(configurarPerfil.DESCRICAO_TIPO_CONTRATO_COTADA),
                                 vPc = MD5Crypt.Criptografar(configurarPerfil.VALOR_PLANO_CONTRATADO.ToString() + "00"),
                                 iRf = MD5Crypt.Criptografar(idRegistroFinanceiro.ToString())
                             });
                    }
                }
            }
            catch (Exception erro)
            {
                //throw erro;
                ModelState.AddModelError("", "Ocorreu um erro ao gravar os dados de configurações (Atividades / Planos Assinatura) do Profissional de Serviços!");
            }

            return null;
        }
    }
}
