using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace ClienteMercado.Utils.Sms
{
    public class EnviarSms
    {
        public bool EnviandoSms(string urlParceiroEnvioSms, long foneCelular)
        {
            bool sucesso = false;

            // Criar um objeto HttpWebRequest  
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(urlParceiroEnvioSms);

            // Enviar a requisição e obter uma resposta em forma de um objeto HttpWebResponse
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequest.GetResponse();

            // Verificar se a requisição obteve alguma resposta
            if (httpRequest.HaveResponse)
            {
                //Deserializar retorno Json
                Stream responsestream = httpWebResponse.GetResponseStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MensagemSMS));

                MensagemSMS retornoMensagem = (MensagemSMS)ser.ReadObject(responsestream);

                if (retornoMensagem.status == 1)
                {
                    sucesso = true;
                }
            }

            return sucesso;
        }
    }
}
