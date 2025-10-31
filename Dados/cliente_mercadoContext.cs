using System.Data.Entity;
using Modelo.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Dados
{
    public partial class cliente_mercadoContext_ : DbContext
    {

        public cliente_mercadoContext_()
            : base("Name=cliente_mercadoContext")
        {
            //Database.SetInitializer<cliente_mercadoContext>   //Código que, quando descomentado, recria o banco de dados.
            //    (new cliente_mercadoContextInitializer());

            Database.SetInitializer<cliente_mercadoContext_>(null);
        }

        public DbSet<empresa_usuario> empresa_usuario { get; set; }
        public DbSet<usuario_empresa> usuario_empresa { get; set; }
        public DbSet<empresa_usuario_logins> empresa_usuario_logins { get; set; }
        public DbSet<atividades_empresa> atividades_empresa { get; set; }
        public DbSet<atividades_profissional> atividades_profissional { get; set; }
        public DbSet<profissional_usuario> profissional_usuario { get; set; }
        public DbSet<usuario_profissional> usuario_profissional { get; set; }
        public DbSet<profissional_usuario_logins> profissional_usuario_logins { get; set; }
        public DbSet<usuario_historico_empresas> usuario_historico_empresas { get; set; }
        public DbSet<usuario_cotante> usuario_cotante { get; set; }
        public DbSet<usuario_cotante_logins> usuario_cotante_logins { get; set; }
        public DbSet<tipo_empresa_usuario> tipo_empresa_usuario { get; set; }
        public DbSet<contato_cliente_mercado> contato_cliente_mercado { get; set; }
        public DbSet<cidades_empresa_usuario> cidades_empresa_usuario { get; set; }
        public DbSet<bairros_empresa_usuario> bairros_empresa_usuario { get; set; }
        public DbSet<enderecos_empresa_usuario> enderecos_empresa_usuario { get; set; }
        public DbSet<estados_empresa_usuario> estados_empresa_usuario { get; set; }
        public DbSet<paises_empresa_usuario> paises_empresa_usuario { get; set; }
        public DbSet<profissoes_usuario> profissoes_usuario { get; set; }
        public DbSet<grupo_atividades_empresa_profissional> grupo_atividades_empresa_profissional { get; set; }
        public DbSet<tipos_contratos_servicos> tipos_contratos_servicos { get; set; }
        public DbSet<meios_pagamento_fatura_servicos> meios_pagamento_fatura_servicos { get; set; }
        public DbSet<financeiro_cobranca_faturamento_usuario_empresa> financeiro_cobranca_faturamento_usuario_empresa { get; set; } 
        public DbSet<financeiro_cobranca_faturamento_usuario_profissional> financeiro_cobranca_faturamento_usuario_profissional { get; set; }
        public DbSet<cards_empresa> cards_empresa { get; set; }
        public DbSet<cards_usuario_empresa> cards_usuario_empresa { get; set; }
        public DbSet<cards_usuario_profissional> cards_usuario_profissional { get; set; }
        public DbSet<produtos_servicos_empresa_profissional> produtos_servicos_empresa_profissional { get; set; }
        public DbSet<controle_sms_usuario_empresa> controle_sms_usuario_empresa { get; set; }
        public DbSet<controle_sms_usuario_profissional> controle_sms_usuario_profissional { get; set; }
        public DbSet<cotacao_master_usuario_empresa> cotacao_master_usuario_empresa { get; set; }
        public DbSet<chat_cotacao_usuario_empresa> chat_cotacao_usuario_empresa { get; set; }
        public DbSet<pedido_usuario_empresa> pedido_usuario_empresa { get; set; }
        public DbSet<itens_pedido_usuario_empresa> itens_pedido_usuario_empresa { get; set; }
        public DbSet<itens_cotacao_usuario_empresa> itens_cotacao_usuario_empresa { get; set; }
        public DbSet<itens_cotacao_filha_negociacao_usuario_empresa> itens_cotacao_filha_negociacao_usuario_empresa { get; set; }
        public DbSet<tipos_cotacao> tipos_cotacao { get; set; }
        public DbSet<classificacao_tipo_itens_cotacao> classificacao_tipo_itens_cotacao { get; set; }
        public DbSet<cotacao_master_usuario_cotante> cotacao_master_usuario_cotante { get; set; }
        public DbSet<itens_cotacao_usuario_cotante> itens_cotacao_usuario_cotante { get; set; }
        public DbSet<tipos_frete> tipos_frete { get; set; }
        public DbSet<unidades_produtos> unidades_produtos { get; set; }
        public DbSet<cotacao_filha_central_compras> cotacao_filha_central_compras { get; set; }
        public DbSet<itens_cotacao_filha_negociacao_central_compras> itens_cotacao_filha_negociacao_central_compras { get; set; }
        public DbSet<chat_cotacao_central_compras> chat_cotacao_central_compras { get; set; }
        public DbSet<pedido_central_compras> pedido_central_conmpras { get; set; }
        public DbSet<itens_pedido_central_compras> itens_pedido_central_compras { get; set; }
        public DbSet<cotacao_filha_usuario_cotante> cotacao_filha_usuario_cotante { get; set; }
        public DbSet<avaliacao_empresa_cotada> avaliacao_empresa_cotada { get; set; }
        public DbSet<avaliacao_usuario_empresa_cotada> avaliacao_usuario_empresa_cotada { get; set; }
        public DbSet<central_de_compras> central_de_compras { get; set; }
        public DbSet<chat_cotacao_usuario_cotante> chat_cotacao_usuario_cotante { get; set; }
        public DbSet<classificacao_empresas_cotadas> classificacao_empresas_cotadas { get; set; }
        public DbSet<classificacao_usuario_empresas_cotadas> classificacao_usuario_empresas_cotadas { get; set; }
        public DbSet<cotacao_filha_usuario_empresa> cotacao_filha_usuario_empresa { get; set; }
        public DbSet<cotacao_individual_empresa_central_compras> cotacao_individual_empresa_central_compras { get; set; }
        public DbSet<cotacao_master_central_compras> cotacao_master_central_compras { get; set; }
        public DbSet<empresas_fabricantes_marcas> empresas_fabricantes_marcas { get; set; }
        public DbSet<empresas_participantes_central_de_compras> empresas_participantes_central_de_compras { get; set; }
        public DbSet<itens_cotacao_filha_negociacao_usuario_cotante> itens_cotacao_filha_negociacao_usuario_cotante { get; set; }
        public DbSet<itens_cotacao_individual_empresa_central_compras> itens_cotacao_individual_empresa_central_compras { get; set; }
        public DbSet<itens_pedido_usuario_cotante> itens_pedido_usuario_cotante { get; set; }
        public DbSet<pedido_usuario_cotante> pedido_usuario_cotante { get; set; }
        public DbSet<status_cotacao> status_cotacao { get; set; }
        public DbSet<tipo_resposta_cotacao> tipo_resposta_cotacao { get; set; }
        public DbSet<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa { get; set; }
        public DbSet<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante { get; set; }
        public DbSet<fotos_itens_alternativos_cotacao_filha_negociacao_central_compras> fotos_itens_alternativos_cotacao_filha_negociacao_central_compras { get; set; }

        public class cliente_mercadoContextInitializer : DropCreateDatabaseAlways<cliente_mercadoContext_> //Sempre dropar e criar o database
        {

        }

        //Definir aqui métodos das classes da pasta Mapping (quando for necessário criar o Mapping de um modelo, usando Fluent Api)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Desliga exclusões por cascata, evitando tbm os erros ocasionados pelas referências a foreign´s keys.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
