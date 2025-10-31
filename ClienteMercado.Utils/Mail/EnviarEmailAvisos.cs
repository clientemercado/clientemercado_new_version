using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ClienteMercado.Utils.Mail
{
    public class EnviarEmailAvisos
    {
        public bool EnviandoEmail(string _email, string _usuario, string _usuarioMaster, string _empresa, int _tipoDeEmail)
        {
            //Montando Link de Acesso ao site
            string comandoHref = "<a href=";
            string link1 = comandoHref + "'http://www.clientemercado.com.br/";
            string link2 = "";

            //Montando o corpo do e-mail
            string assunto = "";
            string mensagem = "";

            /*
            Tipos de e-mail que podem ser disparados pelo sistema (Montando Assunto e Mensagem)
            */
            //Tipo 5 (e-mail informando ao Usuário que deve aguardar confirmação de cadastro pelo usuário Master)
            if (_tipoDeEmail == 5)
            {
                assunto = "Cadastro de Usuário no ClienteMercado.com";
                mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>Informações sobre o Cadastro</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuario + ", <br><br>&nbsp;&nbsp;Os dados cadastrais inseridos por você, foram registrados por nós!</td></tr><tr><td><br>&nbsp;&nbsp;Por favor, aguarde o Usuário Master da empresa <b>" + _empresa + "</b> confirmar seu cadastro e liberar-lhe o acesso para receber e responder cotações.</td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            }
            //Tipo 6 (e-mail informando ao Usuário Master que existe um novo Usuário foi cadastrado e que necessita de confirmação/liberação dele)
            else if (_tipoDeEmail == 6)
            {
                assunto = "Liberação de novo Usuário - ClienteMercado.com";
                mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>Liberação de Cadastro</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuarioMaster + ", <br><br>&nbsp;&nbsp;Existe um novo cadastro de Usuário para a empresa <b>" + _empresa + "</b>, aguardando sua liberação!</td></tr><tr><td><br>&nbsp;&nbsp;O nome do Usuário cadastrado é: <b>" + _usuario + "</b>.</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;&nbsp;Não deixe sua equipe perder vendas!</td></tr><br><tr><td>&nbsp;&nbsp;Acesse agora mesmo www.clientemercado.com.br e confirme o cadastro do membro de sua equipe.</td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            }
            ////Tipo 7 (Aviso de nova COTAÇÃO para os vendedores)
            //else if (_tipoDeEmail == 7)
            //{
            //    assunto = "COTAÇÃO pra você no CLIENTEMERCADO.COM";
            //    mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>AVISO de COTAÇÃO</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuario + ", <br><br>&nbsp;&nbsp;Você tem uma COTAÇÃO a ser respondida no ClienteMercado.com!</td></tr><tr><td align='center'><br>Não perca vendas!</td></tr><tr><td align='center'><br>Acesse " + linked + " e responda agora mesmo, pois a necessidade do seu cliente não pode esperar.</td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            //}

            //Envia o e-mail
            try
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(_email);
                mail.From = new MailAddress("contato@clientemercado.com.br");
                mail.Subject = assunto;
                string Body = mensagem;

                //--
                AlternateView view = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);

                //Header do e-mail - Obs: É o mesmo para todos
                string caminhoImagem1 = HttpContext.Current.Server.MapPath("~/Content/images/header-email_clientemercado.jpg");
                LinkedResource resource = new LinkedResource(caminhoImagem1);
                resource.ContentId = "Imagem1";
                view.LinkedResources.Add(resource);

                mail.AlternateViews.Add(view);
                //--

                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "mail.clientemercado.com.br";
                //smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("administrador@clientemercado.com.br", "24689012ed");
                smtp.EnableSsl = false;
                smtp.Send(mail);

                return true;
            }
            catch (Exception erro)
            {
                throw erro;
            }

        }
    }
}
