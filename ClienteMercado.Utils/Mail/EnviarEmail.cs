using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ClienteMercado.Utils.Mail
{
    public class EnviarEmail
    {
        public bool EnviandoEmail(string _email, string _usuario, string _parametro1, string _parametro2, int _tipoDeEmail, int _tipoLogin)
        {
            string comandoHref = "<a href=";
            string link1 = comandoHref + "'http://www.clientemercado.com.br/";
            string link2 = "";

            //Tipo 0 - e-mail solicitando Login / Senha perdidos
            if (_tipoDeEmail == 0)
            {
                //Alteração de Senha
                link2 = "Login/ForgotPas/" + _tipoLogin + "/" + _parametro1 + "/" + _parametro2 + "'>";//_tipoLogin (de acordo com o usuário), _parametro1 (Login), _parametro2 (Senha)
            }
            else if (_tipoDeEmail == 2)
            {
                //Confirmar cadastro de Usuários de Empresas
                link2 = "UsuarioEmpresa/ConfirmarCadastroUsuario/" + _parametro1 + "/" + _parametro2 + "'>";
            }
            else if (_tipoDeEmail == 3)
            {
                //Confirmar cadastro de Usuários Profissionais de Serviços (que acredito que não será usado)
                link2 = "UsuarioProfissional/ConfirmarCadastroUsuario/" + _parametro1 + "/" + _parametro2 + "'>";
            }
            else if (_tipoDeEmail == 4)
            {
                //Confirmar cadastro de Usuários Cotantes
                link2 = "UsuarioCotante/ConfirmarCadastroUsuario/" + _parametro1 + "/" + _parametro2 + "'>";
            }

            //Montando o link
            string linked = (link1 + link2);
            string assunto = "";
            string mensagem = "";

            /*
            Tipos de e-mail que podem ser disparados pelo sistema (Montando Assunto e Mensagem)
            */
            //Tipo 0 - e-mail solicitando Login / Senha perdidos
            if (_tipoDeEmail == 0)
            {
                assunto = "Alteração de Senha ClienteMercado.com";
                mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>Confirmação de Solicitação de Nova Senha</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuario + ", <br><br>&nbsp;&nbsp;Recebemos sua solicitação de recuperação de senha!</td></tr><tr><td><br>&nbsp;&nbsp;Para alterá-la, clique em " + linked + "redefinir senha</a>.</td></tr><tr><td align='center'><br>" + linked + "<img src='cid:Imagem2' /></a></td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br><br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            }
            //Tipo 1 - e-mail de Contato
            if (_tipoDeEmail == 1)
            {
                assunto = "Contato ClienteMercado.com";
                mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>Confirmação de Contato</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuario + ", <br><br>&nbsp;&nbsp;Seu contato foi registrado por nós!</td></tr><tr><td><br>&nbsp;&nbsp;Se necessário, entraremos em contato no tempo mais breve possível.</td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br><br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            }
            //Tipo 2 (e-mail solicitando confirmação do cadastro da Empresa), Tipo 3 (e-mail solicitando confirmação do cadastro do Profissional), Tipo 4 (e-mail solicitando confirmação do cadastro do Usuário Cotante)
            else if ((_tipoDeEmail == 2) || (_tipoDeEmail == 3) || (_tipoDeEmail == 4))
            {
                assunto = "Confirmação de cadastro ClienteMercado.com";
                mensagem = @"<html><body><tr><td><table width='80%' align='center' bgcolor='#E8E8E8'><tr><td><img src='cid:Imagem1' /></td><tr><td align='center'><h2><b>Confirmação de Cadastro</b></h2></td></tr><tr><td><br>&nbsp;&nbsp;Sr(a) " + _usuario + ", <br><br>&nbsp;&nbsp;Os dados cadastrais informados por você, foram registrados com Sucesso!</td></tr><tr><td><br>&nbsp;&nbsp;Para confirmar seu cadastro, clique em " + linked + "confirmar cadastro</a>.</td></tr><tr><td align='center'><br>" + linked + "<img src='cid:Imagem2' /></a></td></tr><tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;Equipe ClienteMercado<br></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></body></html>";
            }

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

                //Se e-mail for de Solicitação de Senha esquecida
                if (_tipoDeEmail == 0)
                {
                    string caminhoImagem2 = HttpContext.Current.Server.MapPath("~/Content/images/botão_redefinir.png");
                    LinkedResource resource2 = new LinkedResource(caminhoImagem2);
                    resource2.ContentId = "Imagem2";
                    view.LinkedResources.Add(resource2);
                }
                //Se e-mail for solicitando confirmação do cadastro da Empresa
                else if ((_tipoDeEmail == 2) || (_tipoDeEmail == 3) || (_tipoDeEmail == 4))
                {
                    string caminhoImagem2 = HttpContext.Current.Server.MapPath("~/Content/images/botão_confirmar_cadastro.png");//Haverá troca da imagem
                    LinkedResource resource2 = new LinkedResource(caminhoImagem2);
                    resource2.ContentId = "Imagem2";
                    view.LinkedResources.Add(resource2);
                }

                mail.AlternateViews.Add(view);

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
