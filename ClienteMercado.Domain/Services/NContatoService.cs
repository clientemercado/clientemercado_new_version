using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.Mail;

namespace ClienteMercado.Domain.Services
{
    public class NContatoService
    {
        public contato_cliente_mercado GravarContato(contato_cliente_mercado obj)
        {
            DContatoRepository dcontato = new DContatoRepository();

            contato_cliente_mercado gravou = dcontato.GravarContato(obj);

            if (gravou != null)
            {
                int tipoEmail = 1;

                EnviarEmail enviarEmail = new EnviarEmail();

                //Envio de e-mail confirmando o contato recebido pelo ClienteMercado (Tipo 1)
                bool email = enviarEmail.EnviandoEmail(obj.EMAIL_CONTATO_CLIENTE_MERCADO, obj.NOME_CONTATO_CLIENTE_MERCADO, null, obj.MENSAGEM_CONTATO_CLIENTE_MERCADO, tipoEmail, tipoEmail);
            }

            //return dcontato.GravarContato(obj);
            return gravou;
        }
    }
}
