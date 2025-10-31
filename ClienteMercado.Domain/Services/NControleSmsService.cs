using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NControleSMSService
    {
        DControleSMSRepository dcontrolesms = new DControleSMSRepository();

        //Grava informações sobre o SMS enviado, para controle de saldos de SMS´s
        public controle_sms_usuario_empresa GravarDadosSmsEnviado(controle_sms_usuario_empresa obj)
        {
            return dcontrolesms.GravarDadosSmsEnviado(obj);
        }
    }
}
