using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NAtividadesProfissionalService
    {
        //Gravar atividades particulares do Profissional de Serviços
        public atividades_profissional GravarAtividadeProdutoServicoProfissional(atividades_profissional obj)
        {
            DAtividadesProfissionalRepository datividadesprofissional = new DAtividadesProfissionalRepository();

            return datividadesprofissional.GravarAtividadeProdutoServicoProfissional(obj);
        }

    }
}
