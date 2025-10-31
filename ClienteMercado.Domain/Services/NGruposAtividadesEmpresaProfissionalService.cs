using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NGruposAtividadesEmpresaProfissionalService
    {
        DGruposAtividadesEmpresaProfissionalRepository dgruposatividadesempresaprofissional = new DGruposAtividadesEmpresaProfissionalRepository();

        //Busca os Grupos de Atividades para montagem da Cotação
        public List<grupo_atividades_empresa> ListaGruposAtividadesEmpresaProfissional()
        {
            return dgruposatividadesempresaprofissional.ListaGruposAtividadesEmpresaProfissional();
        }

        //Consultar dados do Grupo de Atividade registrado para a Empresa
        public grupo_atividades_empresa ConsultarDadosDoGrupoDeAtividadesDaEmpresa(grupo_atividades_empresa obj)
        {
            return dgruposatividadesempresaprofissional.ConsultarDadosDoGrupoDeAtividadesDaEmpresa(obj);
        }

        //CONSULTAR DADOS do RAMO de ATIVIDADES
        public grupo_atividades_empresa ConsultarDadosDoRamoDeAtividadeDaEmpesaPeloID(int iD_GRUPO_ATIVIDADES)
        {
            return dgruposatividadesempresaprofissional.ConsultarDadosDoRamoDeAtividadeDaEmpesaPeloID(iD_GRUPO_ATIVIDADES);
        }

        //BUSCA DADOS sobre o GRUPO de ATIVIDADES
        public grupo_atividades_empresa ConsultarDadosGeraisSobreOGrupoDeAtividades(int iD_GRUPO_ATIVIDADES)
        {
            return dgruposatividadesempresaprofissional.ConsultarDadosGeraisSobreOGrupoDeAtividades(iD_GRUPO_ATIVIDADES);
        }

        //Buscar lista de CATEGORIAS ATACADISTAS
        public List<grupo_atividades_empresa> CarregarListaDeCategoriasAtacadistas()
        {
            return dgruposatividadesempresaprofissional.CarregarListaDeCategoriasAtacadistas();
        }

        //Buscar lista de CATEGORIAS VAREJISTAS
        public List<grupo_atividades_empresa> CarregarListaDeCategoriasVarejistas()
        {
            return dgruposatividadesempresaprofissional.CarregarListaDeCategoriasVarejistas();
        }

        //Buscar lista de CATEGORIAS VAREJISTAS 2
        public List<grupo_atividades_empresa> CarregarListaDeCategoriasVarejistasNew() => dgruposatividadesempresaprofissional.CarregarListaDeCategoriasVarejistasNew();

        public grupo_atividades_empresa ConsultarDadosDoGrupoDeAtividadesDaEmpresaFornecedor(grupo_atividades_empresa obj) => dgruposatividadesempresaprofissional.ConsultarDadosDoGrupoDeAtividadesDaEmpresaFornecedor(obj);
    }
}
