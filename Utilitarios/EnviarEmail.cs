using System;
using System.Web.Helpers;

namespace Utilitarios
{
    public class EnviarEmail
    {
        public bool EnviandoEmail(string email, string usuario) // Depois de funcionando, incluir o parâmetro "int tipo"
        {
            string assunto = "Teste Envio E-mail";
            string mensagem = "Prezado " + usuario + ", \nE-mail pra você!\n\nAtt,\n\nEquipe ClienteMercado";

            try
            {
                WebMail.SmtpServer = "mail.clientemercado.com.br";     //Servidor SMTP
                WebMail.SmtpServer = "131.72.60.6";     //Servidor SMTP
                WebMail.UserName = "administrador@clientemercado.com.br";    //Ver dados para autenticação
                WebMail.Password = "24689012ed";              //Ver dados para autenticação

                WebMail.From = "contato@clientemercado.com.br";
                WebMail.Send(email, assunto, mensagem);

                return true;
            }
            catch (Exception erro)
            {
                return false;
            }

        }
    }
}