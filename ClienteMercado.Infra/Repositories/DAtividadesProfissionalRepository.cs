using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;

namespace ClienteMercado.Infra.Repositories
{
    public class DAtividadesProfissionalRepository : RepositoryBase<atividades_profissional>
    {
        //Gravar atividades particulares do Profissional de Serviços
        public atividades_profissional GravarAtividadeProdutoServicoProfissional(atividades_profissional obj)
        {
            atividades_profissional atividadesProfissional = _contexto.atividades_profissional.Add(obj);
            _contexto.SaveChanges();

            return atividadesProfissional;
        }

    }
}
