using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;

namespace ClienteMercado.Infra.Repositories
{
    public class DControleSMSRepository : RepositoryBase<controle_sms_usuario_empresa>
    {
        //Grava informações sobre o SMS enviado, para controle de saldos de SMS´s
        public controle_sms_usuario_empresa GravarDadosSmsEnviado(controle_sms_usuario_empresa obj)
        {
            controle_sms_usuario_empresa controleSms =
                _contexto.controle_sms_usuario_empresa.Add(obj);
            _contexto.SaveChanges();

            return controleSms;
        }
    }
}
