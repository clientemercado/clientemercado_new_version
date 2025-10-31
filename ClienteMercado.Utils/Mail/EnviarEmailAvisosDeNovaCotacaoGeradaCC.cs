using System;
using System.Net.Mail;
using System.Net.Mime;

namespace ClienteMercado.Utils.Mail
{
    public class EnviarEmailAvisosDeNovaCotacaoGeradaCC
    {
        //==================================================================
        /*
        OBS: ITENS A SEREM VERIFICADOS NESTA CLASSE:
            1 - TESTAR ESTA ESTRUTURA DE ENVIO DE E-MAIL; 
            2 - COMPARAR O CÓDIGO COM OS CÓDIGOS ANTERIORES NO LEGADO E CONFIGURAR DE ACORDO;
            3 - CHECAR SE ENVIA CERTO AS CÓPIAS (CC´S);
            4 - GERAR HTML PARA ESTE E-MAIL.
        */

        public bool EnviandoEmailAvisoNovaCotacaoCC(string email1_EmpresaParceira, string email2_EmpresaParceira, string email1_UsuarioContato, string email2_UsuarioContato,
            string assuntoEmail, string corpoEmail)
        {
            //MONTA o CORPO do E-MAIL
            string assunto = assuntoEmail;

            //ENVIAR o E-MAIL
            try
            {
                MailMessage mail = new MailMessage();

                if (!string.IsNullOrEmpty(email1_EmpresaParceira))
                {
                    mail.To.Add(email1_EmpresaParceira);
                }

                if (!string.IsNullOrEmpty(email2_EmpresaParceira))
                {
                    MailAddress copy = new MailAddress(email2_EmpresaParceira);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(email1_UsuarioContato))
                {
                    MailAddress copy = new MailAddress(email1_UsuarioContato);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(email2_UsuarioContato))
                {
                    MailAddress copy = new MailAddress(email2_UsuarioContato);
                    mail.CC.Add(copy);
                }

                mail.From = new MailAddress("contato@clientemercado.com.br"); //OBS: ALTERAR AQUI ANTES DE TESTAR...

                mail.Subject = assunto;
                string Body = corpoEmail;

                AlternateView view = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);

                mail.AlternateViews.Add(view);

                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();

                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential("TesteEmailSox@gmail.com", "19929121");

                smtp.Host = "127.0.0.1";  //OBS: ALTERAR AQUI ANTES DE TESTAR...
                smtp.Port = 25;           //OBS: ALTERAR AQUI ANTES DE TESTAR...
                smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential("TesteEmailSox@gmail.com", "19929121");

                ////QUANDO EXISTEM ANEXOS
                //MemoryStream memoryStream = new MemoryStream(array);
                //Attachment attachment = new Attachment(memoryStream, "Recibo", "application/PDF");
                //mail.Attachments.Add(attachment);

                //smtp.Send(mail); //COMENTADO PRA EFEITO DE TESTES. DESCOMENTAR QUANDO FOR USAR...

                return true;
            }
            catch (Exception erro)
            {
                return false;
            }
        }
        //==================================================================
    }
}
