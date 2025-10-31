using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.Mail;
using ClienteMercado.Utils.Sms;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresaUsuarioService
    {
        DEmpresaUsuarioRepository dempresausuario = new DEmpresaUsuarioRepository();

        //Gravar Empresa e Usuário Master
        public empresa_usuario GravarEmpresaUsuario(empresa_usuario obj)
        {
            empresa_usuario gravou = dempresausuario.GravarEmpresaUsuario(obj);

            if (gravou != null)
            {
                int tipoEmail = 2;

                //Atribuindo valores a serem passados como parâmetros para o e-mail de validação de cadastro
                string loginValidacao =
                    obj.usuario_empresa.ElementAt(0).empresa_usuario_logins.ElementAt(0).LOGIN_EMPRESA_USUARIO_LOGINS;
                string passwordValidacao =
                    obj.usuario_empresa.ElementAt(0).empresa_usuario_logins.ElementAt(0).SENHA_EMPRESA_USUARIO_LOGINS;

                EnviarEmail enviarEmail = new EnviarEmail();

                /*
                DESCOMENTAR O TRECHO ABAIXO... 
                */

                ////Envio de e-mail solicitando a confirmação de gravação de dados cadastrais da EMPRESA/USUÁRIO ADM no ClienteMercado (Tipo 2)
                //bool email = enviarEmail.EnviandoEmail(obj.EMAIL1_EMPRESA, obj.NOME_FANTASIA_EMPRESA, loginValidacao, passwordValidacao, tipoEmail, tipoEmail);
            }

            return gravou;
        }

        //Consultar e-mail para o Cadastro da Empresa
        public empresa_usuario ConsultarEmailEmpresa(empresa_usuario obj)
        {
            return dempresausuario.ConsultarEmailEmpresa(obj);
        }

        //Consultar e-mail para o Cadastro do Usuário da Empresa
        public empresa_usuario_logins ConsultarEmailUsuarioEmpresa(empresa_usuario_logins obj)
        {
            return dempresausuario.ConsultarEmailUsuarioEmpresa(obj);
        }

        //Buscar dados da Empresa para enviar ao MOIP
        public empresa_usuario BuscarDadosEmpresaUsuarioParaFinanceiro(empresa_usuario obj)
        {
            return dempresausuario.BuscarDadosEmpresaUsuarioParaFinanceiro(obj);
        }

        //Confirmar o Cadastro da Empresa e do Usuário Master
        public usuario_empresa ConfirmarCadastroUsuarioEmpresa(usuario_empresa obj)
        {
            return dempresausuario.ConfirmarCadastroUsuarioEmpresa(obj);
        }

        //Buscar dados do Usuário Master para enviar ao MOIP
        public usuario_empresa BuscarDadosUsuarioEmpresaParaFinanceiro(usuario_empresa obj)
        {
            return dempresausuario.BuscarDadosUsuarioEmpresaParaFinanceiro(obj);
        }

        //Consultar dados do Usuário Master
        public usuario_empresa BuscarDadosDoUsuarioMaster(usuario_empresa obj)
        {
            return dempresausuario.BuscarDadosDoUsuarioMaster(obj);
        }

        //Gravar novo Usuário em Empresa já cadastrada
        public usuario_empresa GravarNovoUsuarioEmEmpresaJaCadastrada(usuario_empresa obj)
        {
            usuario_empresa gravou = dempresausuario.GravarNovoUsuarioEmEmpresaJaCadastrada(obj);

            if (gravou != null)
            {
                int tipoEmail = 0;

                //Buscar Empresa
                empresa_usuario dadosEmpresa = dempresausuario.BuscarDadosEmpresaUsuario(obj.ID_CODIGO_EMPRESA);

                //Buscar Usuario Master da Empresa
                usuario_empresa dadosDoMaster = dempresausuario.BuscarDadosDoUsuarioMaster(obj);

                if (dadosDoMaster != null)
                {
                    //Buscar e-mail do Usuário Master da Empresa
                    DLoginRepository dlogin = new DLoginRepository();
                    empresa_usuario_logins eMailUsuarioMaster = dlogin.BuscarEmailDoUsuarioMaster(dadosDoMaster.ID_CODIGO_USUARIO);

                    //Disparar e-mails de avisos / SMS de aviso ao Master em caso de novo Usuário cadastrado e bloqueado
                    EnviarEmailAvisos enviarEmailAvisos = new EnviarEmailAvisos();
                    EnviarSms enviarSmsMaster = new EnviarSms();

                    //Envio de e-mail solicitando ao usuário que acabou de se cadastrar, que aguarde a liberação de seu acesso pelo Usuário Master da Empresa (Tipo 5)
                    tipoEmail = 5;
                    bool emailNovoUsuario = enviarEmailAvisos.EnviandoEmail(obj.empresa_usuario_logins.ElementAt(0).EMAIL1_USUARIO, obj.NOME_USUARIO, dadosDoMaster.NOME_USUARIO, dadosEmpresa.RAZAO_SOCIAL_EMPRESA, tipoEmail);

                    //Envio de e-mail solicitando ao Usuário Master liberação do cadastro de um novo Usuário secundário na mesmo Empresa (Tipo 6)
                    tipoEmail = 6;
                    bool emailUsuarioMaster = enviarEmailAvisos.EnviandoEmail(eMailUsuarioMaster.EMAIL1_USUARIO, obj.NOME_USUARIO, dadosDoMaster.NOME_USUARIO, dadosEmpresa.RAZAO_SOCIAL_EMPRESA, tipoEmail);

                    //Envio de SMS ao Usuário Master, para liberação do novo usuário cadastrado para a empresa
                    if (tipoEmail == 6)
                    {
                        string smsMensagem = "ClienteMercado - Um novo usuario aguarda confirmacao e liberacao. Nao perca vendas. Acesse www.clientemercado.com.br e libere o acesso do novo usuario.";
                        string telefoneusuarioMaster = Regex.Replace(dadosDoMaster.TELEFONE1_USUARIO_EMPRESA, "[()-]", "");
                        string urlParceiroEnvioSms = "http://paineldeenvios.com/painel/app/modulo/api/index.php?action=sendsms&lgn=27992691260&pwd=teste&msg=" + smsMensagem + "&numbers=" + telefoneusuarioMaster;

                        bool smsUsuarioMaster = enviarSmsMaster.EnviandoSms(urlParceiroEnvioSms, Convert.ToInt64(telefoneusuarioMaster));

                        if (smsUsuarioMaster)
                        {
                            //Registrar o envio do SMS, para controle de saldos de sms´s enviados
                            DControleSMSRepository dcontrolesms = new DControleSMSRepository();
                            controle_sms_usuario_empresa controleEnvioSms = new controle_sms_usuario_empresa();

                            controleEnvioSms.TELEFONE_DESTINO = dadosDoMaster.TELEFONE1_USUARIO_EMPRESA;
                            controleEnvioSms.ID_CODIGO_USUARIO = dadosDoMaster.ID_CODIGO_USUARIO;
                            controleEnvioSms.MOTIVO_ENVIO = 1; //Valor default. 1 -  Solicitação de desbloqueio de novo usuário cadastrado (Criar uma tabela com esses valores para referência/leitura)

                            controle_sms_usuario_empresa controleSmsUsuarioEmpresa = dcontrolesms.GravarDadosSmsEnviado(controleEnvioSms);
                        }
                    }
                }
            }

            return gravou;
        }

        //Consultar dados da EMPRESA
        public empresa_usuario ConsultarDadosDaEmpresa(empresa_usuario obj)
        {
            return dempresausuario.ConsultarDadosDaEmpresa(obj);
        }

        //Atualizar o Tipo de contrato na Empresa
        public empresa_usuario AtualizarTipoDeContratoNaEmpresa(empresa_usuario obj)
        {
            return dempresausuario.AtualizarTipoDeContratoNaEmpresa(obj);
        }

        //POPULAR LISTA de EMPRESAS para TESTE do MAPA de COTAÇÃO
        public List<empresa_usuario> BuscarListaDeEmpresasCotantesParaTeste()
        {
            return dempresausuario.BuscarListaDeEmpresasCotantesParaTeste();
        }

        //CARREGA LISTA TESTE (Obs: O método será mantido no futuro, porém a consulta será implementada considerando dados espaciais)
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDePossiveisParceirosNaCentralDeCompras(int codEmpresaAdm, int cGA)
        {
            return dempresausuario.BuscarListaDePossiveisParceirosNaCentralDeCompras(codEmpresaAdm, cGA);
        }

        //CARREGAR DADOS das EMPRESAS SELECIONADAS para COMPOR a CENTRAL de COMPRAS RECÉM CRIADA
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(int[] empresasSelecionadas)
        {
            return dempresausuario.BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(empresasSelecionadas);
        }

        //BUSCAR CIDADE da CENTRAL de COMPRAS
        public string ConsultarCidadeDaEmpresa(int iD_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS)
        {
            return dempresausuario.ConsultarCidadeDaEmpresa(iD_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS);
        }

        //BUSCAR DADOS da EMPRESA COTADA
        public empresa_usuario BuscarDadosEmpresaCotada(int idEmpresa)
        {
            return dempresausuario.BuscarDadosEmpresaCotada(idEmpresa);
        }

        //GRAVAR DADOS ATUALIZADOS da EMPRESA
        public empresa_usuario AtualizarDadosCadastrais(empresa_usuario obj)
        {
            return dempresausuario.AtualizarDadosCadastrais(obj);
        }

        //REGISTRAR INÍCIO da GRATUIDADE no SISTEMA
        public empresa_usuario IniciarGRatuidadeNoSistema(empresa_usuario obj)
        {
            return dempresausuario.IniciarGRatuidadeNoSistema(obj);
        }

        //CARREGAR DADOS das EMPRESAS SELECIONADAS para receber a COTAÇÃO
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>
            BuscarListaDeEmpresasSelecionadasParaRecerACotacaoDaCentralDeCompras(int[] empresasSelecionadas)
        {
            return dempresausuario.BuscarListaDeEmpresasSelecionadasParaRecerACotacaoDaCentralDeCompras(empresasSelecionadas);
        }

        //CARREGAR DADOS das EMPRESAS que NÃO ANEXARAM COTAÇÃO
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> BuscarListaDeEmpresasPeloCodigo(string listaIdsEmpresasNaoCotantes)
        {
            return dempresausuario.BuscarListaDeEmpresasPeloCodigo(listaIdsEmpresasNaoCotantes);
        }

        //CARREGAR os DADOS das EMPRESAS que ANEXARAM COTAÇÃO
        public List<ListaEstilizadaDeEmpresasViewModel> CarregarDadosDasEmpresasQueAnexaramCotacao(int[] idsEmpresas, int[] idsCotacoesIndividuais)
        {
            decimal totalProduto = 0;
            NEmpresaUsuarioService negociosEmpresaUsuario = new NEmpresaUsuarioService();

            List<ListaEstilizadaDeEmpresasViewModel> dadosDasEmpresasQueAnexaramCotacao =
                dempresausuario.CarregarDadosDasEmpresasQueAnexaramCotacao(idsEmpresas, idsCotacoesIndividuais);

            //POPULAR o campo alias 'quantidadeProdutoCotado' com o valor de QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS, fazendo o CAST do DECIMAL pro STRING
            for (int i = 0; i < dadosDasEmpresasQueAnexaramCotacao.Count; i++)
            {
                for (int j = 0; j < dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa.Count; j++)
                {
                    dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].quantidadeProdutoCotado =
                        dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS.ToString();

                    if ((dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS > 0) 
                        && (dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].pedidoConfirmado))
                    {
                        dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].itemFoiPedido = "sim";

                        //BUSCAR DADOS da EMPRESA FORNECEDORA
                        empresa_usuario dadosEmpresaFornecedora = 
                            negociosEmpresaUsuario.BuscarDadosEmpresaCotada((int)dadosDasEmpresasQueAnexaramCotacao[i]
                            .listaDeItensCotadosPorEmpresa[j].ID_EMPRESA_FORNECEDORA_PEDIDO);

                        if (dadosEmpresaFornecedora != null)
                        {
                            dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].fornecedorItemPedido = 
                                dadosEmpresaFornecedora.NOME_FANTASIA_EMPRESA;
                        }

                        //DEFINIR VALOR UNITÁRIO A SER EXIBIDO
                        if ((dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].recebeu_cp) 
                            && (dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].aceitou_cp))
                        {
                            dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].valorDoProdutoCotado = 
                                dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].preco_unitario_contra_proposta.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");

                            totalProduto = 
                                (Convert.ToDecimal(dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].quantidadeProdutoCotado) 
                                * dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].preco_unitario_contra_proposta);
                        }
                        else
                        {
                            dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].valorDoProdutoCotado = 
                                dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].preco_unitario_resposta.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");

                            totalProduto =
                                (Convert.ToDecimal(dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].quantidadeProdutoCotado) 
                                * dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].preco_unitario_resposta);
                        }

                        dadosDasEmpresasQueAnexaramCotacao[i].listaDeItensCotadosPorEmpresa[j].totalDoProdutoCotado = 
                            totalProduto.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                    }
                }
            }

            return dadosDasEmpresasQueAnexaramCotacao;
        }

        //CONSULTAR DADOS da EMPRESA pelo ID do USUÁRIO da EMPRESA
        public empresa_usuario ConsultarDadoDaEmpresaPeloCodigoUsuario(int idUsuario)
        {
            return dempresausuario.ConsultarDadoDaEmpresaPeloCodigoUsuario(idUsuario);
        }

        //CARREGAR DADOS do FORNECEDOR para ENVIO de MENSAGENS
        public ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel BuscarDadosDaEmpresaParaEnvioDeMensagens(int idFornecedor)
        {
            return dempresausuario.BuscarDadosDaEmpresaParaEnvioDeMensagens(idFornecedor);
        }
    }
}
