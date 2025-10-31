using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresaUsuarioRepository : RepositoryBase<empresa_usuario>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;

        //Gravar Empresa e Usuário Master
        public empresa_usuario GravarEmpresaUsuario(empresa_usuario obj)
        {
            empresa_usuario empresaUsuario =
                _contexto.empresa_usuario.Add(obj);
            _contexto.SaveChanges();

            return empresaUsuario;
        }

        //Consultar e-mail para o Cadastro da Empresa
        public empresa_usuario ConsultarEmailEmpresa(empresa_usuario obj)
        {
            empresa_usuario email =
                _contexto.empresa_usuario.FirstOrDefault(m => m.EMAIL1_EMPRESA.Equals(obj.EMAIL1_EMPRESA));

            return email;
        }

        //Consultar e-mail para o Cadastro do Usuário da Empresa
        public empresa_usuario_logins ConsultarEmailUsuarioEmpresa(empresa_usuario_logins obj)
        {
            empresa_usuario_logins email =
                _contexto.empresa_usuario_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

            return email;
        }

        //Buscar dados da Empresa para usos diversos
        public empresa_usuario BuscarDadosEmpresaUsuario(int idEmpresa)
        {
            empresa_usuario dadosEmpresa =
                _contexto.empresa_usuario.FirstOrDefault(m => m.ID_CODIGO_EMPRESA.Equals(idEmpresa));

            return dadosEmpresa;
        }

        //Buscar dados da Empresa para enviar ao MOIP
        public empresa_usuario BuscarDadosEmpresaUsuarioParaFinanceiro(empresa_usuario obj)
        {
            empresa_usuario dadosEmpresa =
                _contexto.empresa_usuario.FirstOrDefault(m => m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA));

            return dadosEmpresa;
        }

        //Atualizar o Tipo de contrato na Empresa
        public empresa_usuario AtualizarTipoDeContratoNaEmpresa(empresa_usuario obj)
        {
            empresa_usuario atualizarCadastroEmpresa =
                _contexto.empresa_usuario.Find(obj.ID_CODIGO_EMPRESA);
            atualizarCadastroEmpresa.ID_CODIGO_TIPO_CONTRATO_COTADA = obj.ID_CODIGO_TIPO_CONTRATO_COTADA;
            atualizarCadastroEmpresa.ID_GRUPO_ATIVIDADES = obj.ID_GRUPO_ATIVIDADES;
            _contexto.SaveChanges();

            return atualizarCadastroEmpresa;
        }

        //Confirmar o Cadastro da Empresa e do Usuário Master
        public usuario_empresa ConfirmarCadastroUsuarioEmpresa(usuario_empresa obj)
        {
            usuario_empresa confirmaCadastroUsuarioEmpresa =
                _contexto.usuario_empresa.Find(obj.ID_CODIGO_USUARIO);
            confirmaCadastroUsuarioEmpresa.CADASTRO_CONFIRMADO = true;
            _contexto.SaveChanges();

            return confirmaCadastroUsuarioEmpresa;
        }

        //Buscar dados do Usuário Master para enviar ao MOIP
        public usuario_empresa BuscarDadosUsuarioEmpresaParaFinanceiro(usuario_empresa obj)
        {
            usuario_empresa dadosUsuario =
                _contexto.usuario_empresa.FirstOrDefault(m => m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO));

            return dadosUsuario;
        }

        //Consultar dados do Usuário Master
        public usuario_empresa BuscarDadosDoUsuarioMaster(usuario_empresa obj)
        {
            usuario_empresa dadosUsuarioMaster =
                _contexto.usuario_empresa.FirstOrDefault(
                    m => ((m.ID_CODIGO_EMPRESA == obj.ID_CODIGO_EMPRESA) && (m.USUARIO_MASTER == true)));

            return dadosUsuarioMaster;
        }

        //Gravar novo Usuário em Empresa já cadastrada
        public usuario_empresa GravarNovoUsuarioEmEmpresaJaCadastrada(usuario_empresa obj)
        {
            usuario_empresa usuarioEmpresa =
                _contexto.usuario_empresa.Add(obj);
            _contexto.SaveChanges();

            return usuarioEmpresa;
        }

        //Consultar dados da EMPRESA
        public empresa_usuario ConsultarDadosDaEmpresa(empresa_usuario obj)
        {
            empresa_usuario dadosEmpresa =
                _contexto.empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA)));

            return dadosEmpresa;
        }

        //POPULAR LISTA de EMPRESAS para TESTE do MAPA de COTAÇÃO
        public List<empresa_usuario> BuscarListaDeEmpresasCotantesParaTeste()
        {
            List<empresa_usuario> listaDeEmpresas = _contexto.empresa_usuario.Where(m => m.ID_CODIGO_EMPRESA != idEmpresa).Take(6).ToList();

            return listaDeEmpresas;
        }

        //CARREGA LISTA TESTE (Obs: O método será mantido no futuro, porém a consulta será implementada considerando dados espaciais)
        public List<ListaEstilizadaDeEmpresasViewModel> BuscarListaDePossiveisParceirosNaCentralDeCompras(int codEmpresaAdm, int cGA)
        {
            /*
             CONTINUAR AQUI...

             OBS: CORRIGIR ESSA QUERY DE FORMA QUE SÓ TRAGA EMPRESAS DO MESMO RAMO DE ATIVIDADE QUE NÃO PARTICIPEM DE OUTRAS CENTRAIS de COMPRAS DO MESMO RAMO DE ATIVIDADE.
                  CHECAR A TABELA empresas_participantes_central_de_compras
             */

            var query = "SELECT TOP 30 EU.ID_CODIGO_EMPRESA as idEmpresa, EU.NOME_FANTASIA_EMPRESA as nomeEmpresa, EU.EMAIL1_EMPRESA as eMailEmpresa, " +
                        "(EE.TIPO_LOGRADOURO_EMPRESA_USUARIO + ' ' + EE.LOGRADOURO_CEP_EMPRESA_USUARIO + ',  - ' + BE.BAIRRO_CIDADE_EMPRESA_USUARIO) AS logradouroEmpresa, " +
                        "CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, EEU.UF_EMPRESA_USUARIO AS ufEmpresa, UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                        "FROM empresa_usuario EU " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                        "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN bairros_empresa_usuario BE ON(BE.ID_BAIRRO_EMPRESA_USUARIO = EE.ID_BAIRRO_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario EEU ON(EEU.ID_ESTADOS_EMPRESA_USUARIO = CE.ID_ESTADOS_EMPRESA_USUARIO)" +
                        "WHERE EU.ID_GRUPO_ATIVIDADES_VAREJO = " + cGA + " AND EU.ID_CODIGO_EMPRESA <> " + codEmpresaAdm;

            var result = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();
            return result;
        }

        //CARREGAR DADOS das EMPRESAS SELECIONADAS para COMPOR a CENTRAL de COMPRAS RECÉM CRIADA
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> BuscarListaDeEmpresasSelecionadasParaParceriaNaCentralDeCompras(int[] empresasSelecionadas)
        {
            var query = "";
            string listaCodEmpresas = string.Join(",", empresasSelecionadas);

            query = "SELECT EU.ID_CODIGO_EMPRESA AS idEmpresa, EU.NOME_FANTASIA_EMPRESA AS nomeEmpresa, EU.EMAIL1_EMPRESA AS eMail1_Empresa, EU.EMAIL2_EMPRESA AS eMail2_Empresa, " +
                    "UE.ID_CODIGO_USUARIO AS idUsuarioContatoResponsavel, UE.NICK_NAME_USUARIO AS nickNameUsuarioContatoEmpresa, UE.NOME_USUARIO AS nomeUsuarioContatoEmpresa, " +
                    "UL.EMAIL1_USUARIO AS eMaiL1_UsuarioContatoEmpresa, UL.EMAIL2_USUARIO AS eMaiL2_UsuarioContatoEmpresa, UE.TELEFONE1_USUARIO_EMPRESA AS celular1_UsuarioContatoEmpresa, " +
                    "UE.TELEFONE2_USUARIO_EMPRESA AS celular2_UsuarioContatoEmpresa " +
                    "FROM empresa_usuario EU " +
                    "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " + //DESCOMENTAR ISSO DEPOIS
                    "INNER JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    //"LEFT JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                    //"LEFT JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    "WHERE EU.ID_CODIGO_EMPRESA IN(" + listaCodEmpresas + ")";

            var result = _contexto.Database.SqlQuery<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>(query).ToList();
            return result;
        }

        //CARREGAR DADOS das EMPRESAS SELECIONADAS para RECEBER A COTAÇÃO
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel> BuscarListaDeEmpresasSelecionadasParaRecerACotacaoDaCentralDeCompras(int[] empresasSelecionadas)
        {
            var query = "";
            string listaCodEmpresas = string.Join(",", empresasSelecionadas);

            query = "SELECT EU.ID_CODIGO_EMPRESA AS idEmpresa, EU.NOME_FANTASIA_EMPRESA AS nomeEmpresa, EU.EMAIL1_EMPRESA AS eMail1_Empresa, EU.EMAIL2_EMPRESA AS eMail2_Empresa, " +
                    "UE.ID_CODIGO_USUARIO AS idUsuarioContatoResponsavel, UE.NICK_NAME_USUARIO AS nickNameUsuarioContatoEmpresa, UE.NOME_USUARIO AS nomeUsuarioContatoEmpresa, " +
                    "UL.EMAIL1_USUARIO AS eMaiL1_UsuarioContatoEmpresa, UL.EMAIL2_USUARIO AS eMaiL2_UsuarioContatoEmpresa, UE.TELEFONE1_USUARIO_EMPRESA AS celular1_UsuarioContatoEmpresa, " +
                    "UE.TELEFONE2_USUARIO_EMPRESA AS celular2_UsuarioContatoEmpresa " +
                    "FROM empresa_usuario EU " +
                    "LEFT JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                    "LEFT JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    "WHERE EU.ID_CODIGO_EMPRESA IN(" + listaCodEmpresas + ")";

            var result = _contexto.Database.SqlQuery<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>(query).ToList();
            return result;
        }

        //CARREGAR os DADOS das EMPRESAS que ANEXARAM COTAÇÃO
        public List<ListaEstilizadaDeEmpresasViewModel> CarregarDadosDasEmpresasQueAnexaramCotacao(int[] idsEmpresas, int[] idsCotacoesIndividuais)
        {
            var query = "";
            List<ListaEstilizadaDeEmpresasViewModel> resultEmpresas = new List<ListaEstilizadaDeEmpresasViewModel>();

            //CARREGA DADOS das EMPRESAS
            for (int i = 0; i < idsCotacoesIndividuais.Length; i++)
            {
                query = "SELECT EU.ID_CODIGO_EMPRESA as idEmpresa, EU.NOME_FANTASIA_EMPRESA as nomeEmpresa, " +
                        "CE.CIDADE_EMPRESA_USUARIO AS cidadeEmpresa, EEU.UF_EMPRESA_USUARIO AS ufEmpresa, UE.NOME_USUARIO AS usuarioContatoEmpresa " +
                        "FROM empresa_usuario EU " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                        "INNER JOIN enderecos_empresa_usuario EE ON(EE.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CE ON(CE.ID_CIDADE_EMPRESA_USUARIO = EE.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN bairros_empresa_usuario BE ON(BE.ID_BAIRRO_EMPRESA_USUARIO = EE.ID_BAIRRO_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario EEU ON(EEU.ID_ESTADOS_EMPRESA_USUARIO = CE.ID_ESTADOS_EMPRESA_USUARIO) " +
                        "WHERE EU.ID_CODIGO_EMPRESA = " + idsEmpresas[i];
                var resultado = _contexto.Database.SqlQuery<ListaEstilizadaDeEmpresasViewModel>(query).ToList();

                resultEmpresas.Add(new ListaEstilizadaDeEmpresasViewModel
                {
                    idEmpresa = resultado[0].idEmpresa,
                    nomeEmpresa = resultado[0].nomeEmpresa,
                    cidadeEmpresa = resultado[0].cidadeEmpresa,
                    ufEmpresa = resultado[0].ufEmpresa,
                    usuarioContatoEmpresa = resultado[0].usuarioContatoEmpresa
                });
            }

            //CARREGA ITENS COTADOS pelas EMPRESAS
            for (int i = 0; i < idsCotacoesIndividuais.Length; i++)
            {
                query = "SELECT PS.DESCRICAO_PRODUTO_SERVICO AS descricaoProdutoCotado, EF.DESCRICAO_EMPRESA_FABRICANTE_MARCAS AS marcaProdutoCotado, " +
                        "ICI.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS, UP.DESCRICAO_UNIDADE_PRODUTO AS unidadeProdutoCotado, " +
                        "EP.DESCRICAO_PRODUTO_EMBALAGEM AS embalagemProduto, ICI.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS, ICI.ID_EMPRESA_FORNECEDORA_PEDIDO, " +
                        "CAST(COALESCE(PCC.CONFIRMADO_PEDIDO_CENTRAL_COMPRAS,0) AS BIT) AS pedidoConfirmado, " + 
                        "CAST(COALESCE(CFC.RECEBEU_CONTRA_PROPOSTA,0) AS BIT) AS recebeu_cp, CAST(COALESCE(CFC.ACEITOU_CONTRA_PROPOSTA,0) AS BIT) AS aceitou_cp, " +
                        "COALESCE(ICF.PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS,0) AS preco_unitario_resposta, COALESCE(ICF.PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS,0) AS preco_unitario_contra_proposta " +
                        "FROM itens_cotacao_individual_empresa_central_compras ICI " +
                        "INNER JOIN produtos_servicos_empresa_profissional PS ON(PS.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS = ICI.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS) " +
                        "INNER JOIN empresas_fabricantes_marcas EF ON(EF.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = ICI.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS) " +
                        "INNER JOIN unidades_produtos UP ON(UP.ID_CODIGO_UNIDADE_PRODUTO = ICI.ID_CODIGO_UNIDADE_PRODUTO) " +
                        "INNER JOIN empresas_produtos_embalagens EP ON(EP.ID_EMPRESAS_PRODUTOS_EMBALAGENS = ICI.ID_EMPRESAS_PRODUTOS_EMBALAGENS) " +
                        "LEFT JOIN pedido_central_compras PCC ON(PCC.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = ICI.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS) " +
                        "LEFT JOIN cotacao_filha_central_compras CFC ON(CFC.ID_CODIGO_EMPRESA = ICI.ID_EMPRESA_FORNECEDORA_PEDIDO) " +
                        "LEFT JOIN itens_cotacao_filha_negociacao_central_compras ICF ON((ICF.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = ICI.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS) " + 
                        "AND (ICF.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = CFC.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS)) " +
                        "WHERE ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS IN(" + idsCotacoesIndividuais[i] + ")";
                var resultItens = _contexto.Database.SqlQuery<ListaDeItensDaCotacaoIndividualViewModel>(query).ToList();

                resultEmpresas[i].listaDeItensCotadosPorEmpresa = resultItens;
            }

            return resultEmpresas;
        }

        //CARREGAR DADOS do FORNECEDOR para ENVIO de MENSAGENS
        public ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel BuscarDadosDaEmpresaParaEnvioDeMensagens(int idFornecedor)
        {
            var query = "";

            query = "SELECT EU.ID_CODIGO_EMPRESA AS idEmpresa, EU.NOME_FANTASIA_EMPRESA AS nomeEmpresa, EU.EMAIL1_EMPRESA AS eMail1_Empresa, EU.EMAIL2_EMPRESA AS eMail2_Empresa, " +
                    "UE.ID_CODIGO_USUARIO AS idUsuarioContatoResponsavel, UE.NICK_NAME_USUARIO AS nickNameUsuarioContatoEmpresa, UE.NOME_USUARIO AS nomeUsuarioContatoEmpresa, " +
                    "UL.EMAIL1_USUARIO AS eMaiL1_UsuarioContatoEmpresa, UL.EMAIL2_USUARIO AS eMaiL2_UsuarioContatoEmpresa, UE.TELEFONE1_USUARIO_EMPRESA AS celular1_UsuarioContatoEmpresa, " +
                    "UE.TELEFONE2_USUARIO_EMPRESA AS celular2_UsuarioContatoEmpresa, EU.TELEFONE1_EMPRESA_USUARIO AS fone1_Empresa, EU.TELEFONE2_EMPRESA_USUARIO AS fone2_Empresa " +
                    "FROM empresa_usuario EU " +
                    "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " + //DESCOMENTAR ISSO DEPOIS
                    "INNER JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    //"LEFT JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                    //"LEFT JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    "WHERE EU.ID_CODIGO_EMPRESA IN(" + idFornecedor + ")";

            var result = _contexto.Database.SqlQuery<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>(query).FirstOrDefault();
            return result;
        }

        //CONSULTAR DADOS da EMPRESA pelo ID do USUÁRIO da EMPRESA
        public empresa_usuario ConsultarDadoDaEmpresaPeloCodigoUsuario(int idUsuario)
        {
            var query = "SELECT EU.* " +
                        "FROM usuario_empresa UE " +
                        "INNER JOIN empresa_usuario EU ON(EU.ID_CODIGO_EMPRESA = UE.ID_CODIGO_EMPRESA) " +
                        "WHERE UE.ID_CODIGO_USUARIO = " + idUsuario;

            var dadosEmpresa = _contexto.Database.SqlQuery<empresa_usuario>(query).FirstOrDefault();

            return dadosEmpresa;
        }

        //CARREGAR DADOS das EMPRESAS que NÃO ANEXARAM COTAÇÃO
        public List<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>
            BuscarListaDeEmpresasPeloCodigo(string listaIdsEmpresasNaoCotantes)
        {
            var query = "";

            query = "SELECT EU.ID_CODIGO_EMPRESA AS idEmpresa, EU.NOME_FANTASIA_EMPRESA AS nomeEmpresa, EU.EMAIL1_EMPRESA AS eMail1_Empresa, EU.EMAIL2_EMPRESA AS eMail2_Empresa, " +
                    "UE.ID_CODIGO_USUARIO AS idUsuarioContatoResponsavel, UE.NICK_NAME_USUARIO AS nickNameUsuarioContatoEmpresa, UE.NOME_USUARIO AS nomeUsuarioContatoEmpresa, " +
                    "UL.EMAIL1_USUARIO AS eMaiL1_UsuarioContatoEmpresa, UL.EMAIL2_USUARIO AS eMaiL2_UsuarioContatoEmpresa, UE.TELEFONE1_USUARIO_EMPRESA AS celular1_UsuarioContatoEmpresa, " +
                    "UE.TELEFONE2_USUARIO_EMPRESA AS celular2_UsuarioContatoEmpresa " +
                    "FROM empresa_usuario EU " +
                    "LEFT JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA AND UE.USUARIO_MASTER = 1) " +
                    "LEFT JOIN empresa_usuario_logins UL ON(UL.ID_CODIGO_USUARIO = UE.ID_CODIGO_USUARIO) " +
                    "WHERE EU.ID_CODIGO_EMPRESA IN(" + listaIdsEmpresasNaoCotantes + ")";

            var result = _contexto.Database.SqlQuery<ListaDadosEmpresasEUsuariosParaContatoEMensagensViewModel>(query).ToList();
            return result;
        }

        //BUSCAR CIDADE da CENTRAL de COMPRAS
        public string ConsultarCidadeDaEmpresa(int iD_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS)
        {
            var cidade = "";

            empresa_usuario dadosEmpresaAdmCC =
                _contexto.empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == iD_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS));

            if (dadosEmpresaAdmCC != null)
            {
                enderecos_empresa_usuario dadosEnderecoEmpresaAdmCC =
                    _contexto.enderecos_empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_ENDERECO_EMPRESA_USUARIO == dadosEmpresaAdmCC.ID_CODIGO_ENDERECO_EMPRESA_USUARIO));

                cidades_empresa_usuario dadosCidadeEmpresaAdmCC =
                    _contexto.cidades_empresa_usuario.FirstOrDefault(m => (m.ID_CIDADE_EMPRESA_USUARIO == dadosEnderecoEmpresaAdmCC.ID_CIDADE_EMPRESA_USUARIO));

                estados_empresa_usuario dadosEstadoEmpresaAdmCC =
                    _contexto.estados_empresa_usuario.FirstOrDefault(m => (m.ID_ESTADOS_EMPRESA_USUARIO == dadosCidadeEmpresaAdmCC.ID_ESTADOS_EMPRESA_USUARIO));

                cidade = (dadosCidadeEmpresaAdmCC.CIDADE_EMPRESA_USUARIO + "-" + dadosEstadoEmpresaAdmCC.UF_EMPRESA_USUARIO);
            }

            return cidade;
        }

        //REGISTRAR INÍCIO da GRATUIDADE no SISTEMA
        public empresa_usuario IniciarGRatuidadeNoSistema(empresa_usuario obj)
        {
            empresa_usuario dadosEmpresa = _contexto.empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == idEmpresa));

            if (dadosEmpresa != null)
            {
                dadosEmpresa.DATA_FINAL_GRATUIDADE_EMPRESA = obj.DATA_FINAL_GRATUIDADE_EMPRESA;
                dadosEmpresa.DATA_ULTIMA_ATUALIZACAO_EMPRESA = DateTime.Now;

                _contexto.SaveChanges();
            }

            return dadosEmpresa;
        }

        //GRAVAR DADOS ATUALIZADOS da EMPRESA
        public empresa_usuario AtualizarDadosCadastrais(empresa_usuario obj)
        {
            bool atualiza = false;

            empresa_usuario dadosDaEmpresaASerAtualizada =
                _contexto.empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == idEmpresa));

            if (dadosDaEmpresaASerAtualizada != null)
            {
                //dadosDaEmpresaASerAtualizada.ID_CODIGO_EMPRESA = Convert.ToInt32(Sessao.IdEmpresaUsuario);
                //dadosDaEmpresaASerAtualizada.ID_CODIGO_TIPO_EMPRESA_USUARIO = 1;

                if ((obj.RAZAO_SOCIAL_EMPRESA != null) && (obj.RAZAO_SOCIAL_EMPRESA != ""))
                {
                    dadosDaEmpresaASerAtualizada.RAZAO_SOCIAL_EMPRESA = obj.RAZAO_SOCIAL_EMPRESA;
                    atualiza = true;
                }

                if ((obj.NOME_FANTASIA_EMPRESA != null) && (obj.NOME_FANTASIA_EMPRESA != ""))
                {
                    dadosDaEmpresaASerAtualizada.NOME_FANTASIA_EMPRESA = obj.NOME_FANTASIA_EMPRESA;
                    atualiza = true;
                }

                if (obj.ID_CODIGO_ENDERECO_EMPRESA_USUARIO > 0)
                {
                    dadosDaEmpresaASerAtualizada.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = obj.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                    atualiza = true;
                }

                if ((obj.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO != null) && (obj.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO != ""))
                {
                    dadosDaEmpresaASerAtualizada.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO = obj.COMPLEMENTO_ENDERECO_EMPRESA_USUARIO;
                    atualiza = true;
                }

                if ((obj.EMAIL1_EMPRESA != null) && (obj.EMAIL1_EMPRESA != ""))
                {
                    dadosDaEmpresaASerAtualizada.EMAIL1_EMPRESA = obj.EMAIL1_EMPRESA;
                    atualiza = true;
                }

                if ((obj.TELEFONE1_EMPRESA_USUARIO != null) && (obj.TELEFONE1_EMPRESA_USUARIO != ""))
                {
                    dadosDaEmpresaASerAtualizada.TELEFONE1_EMPRESA_USUARIO = obj.TELEFONE1_EMPRESA_USUARIO;
                    atualiza = true;
                }

                if (obj.PAIS_EMPRESA_USUARIO > 0)
                {
                    dadosDaEmpresaASerAtualizada.PAIS_EMPRESA_USUARIO = obj.PAIS_EMPRESA_USUARIO;
                    atualiza = true;
                }

                if (obj.ID_GRUPO_ATIVIDADES_ATACADO > 0)
                {
                    dadosDaEmpresaASerAtualizada.ID_GRUPO_ATIVIDADES_ATACADO = obj.ID_GRUPO_ATIVIDADES_ATACADO;
                    atualiza = true;
                }
                else if (obj.ID_GRUPO_ATIVIDADES_ATACADO == 0)
                {
                    dadosDaEmpresaASerAtualizada.ID_GRUPO_ATIVIDADES_ATACADO = null;
                    atualiza = true;
                }

                if (obj.ID_GRUPO_ATIVIDADES_VAREJO > 0)
                {
                    dadosDaEmpresaASerAtualizada.ID_GRUPO_ATIVIDADES_VAREJO = obj.ID_GRUPO_ATIVIDADES_VAREJO;
                    atualiza = true;
                }
                else if (obj.ID_GRUPO_ATIVIDADES_VAREJO == 0)
                {
                    dadosDaEmpresaASerAtualizada.ID_GRUPO_ATIVIDADES_VAREJO = null;
                    atualiza = true;
                }

                if (atualiza)
                {
                    dadosDaEmpresaASerAtualizada.DATA_ULTIMA_ATUALIZACAO_EMPRESA = DateTime.Now;
                }

                _contexto.SaveChanges();
            }

            return dadosDaEmpresaASerAtualizada;
        }

        //BUSCAR DADOS da EMPRESA COTADA
        public empresa_usuario BuscarDadosEmpresaCotada(int idEmpresa)
        {
            empresa_usuario dadosDaEmpresaCotada =
                _contexto.empresa_usuario.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == idEmpresa));

            return dadosDaEmpresaCotada;
        }
    }
}
