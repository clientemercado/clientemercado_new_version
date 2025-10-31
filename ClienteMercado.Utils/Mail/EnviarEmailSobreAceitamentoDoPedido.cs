using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ClienteMercado.Utils.Mail
{
    public class EnviarEmailSobreAceitamentoDoPedido
    {
        public bool EnviarEmail(string _nomeCC, string _usuarioAdmCC, string _empresaFornecedora, string _email1_EmpresaAdmCC, string _email2_EmpresaAdmCC,
            string _email1_UsuarioContatoAdmCC, string _email2_UsuarioContatoAdmCC, string _dataEnvioPedido, string _numeroPedido, string _dataEntrega, 
            string _tipoFrete, string _usuarioConfirmou, string _fone1UsuarioConfirmou, string _fone2UsuarioConfirmou)
        {
            //Montando Link de Acesso ao site
            string comandoHref = "<a href=";
            string link1 = comandoHref + "http://www.clientemercado.com.br/";
            string link2 = "></a>";

            //Montando o corpo do e-mail
            string linked = (link1 + link2);
            string assunto = "";
            string mensagem = "";

            assunto = "CONFIRMAÇÃO DO PEDIDO - Fornecedor: " + _empresaFornecedora;
            mensagem = @"<html>" +
                        "<body>" +
                        "<tr><td>" + 
                        "<table width='80%' align='center' bgcolor='#E8E8E8'>" + 
                        "<tr><td><img src='cid:Imagem1' /></td></tr>" +
                        "<tr>" +
                        "<td align='center'>" + 
                        "<h2><b>!!! ATENÇÃO !!!</b></h2>" +
                        "</td>" +
                        "</tr>" + 
                        "<tr>" +
                        "<td><br>&nbsp;&nbsp;Sr(a) " + _usuarioAdmCC + ", <br><br>" +
                        "&nbsp;&nbsp;O Fornecedor <b>" + _empresaFornecedora + "</b>, ACEITOU o PEDIDO Nº 00010, efetuado por sua Central de Compras no dia " + _dataEnvioPedido + "." + 
                        "</td>" +
                        "</tr>" + 
                        "<br><br>" +
                        "<tr><td colspan='4'><b>CARACTERÍSTICAS DO PEDIDO:</b></td></tr><br>" +
                        "<tr><td><b>Pedido Nº:</b>&nbsp;</td><td>" + _numeroPedido + "</td><td colspan='2'></td></tr> "+
                        "<tr><td><b>Data Entrega:</b>&nbsp;</td><td>" + _dataEntrega + "/td><td colspan='2'></td></tr> " +
                        "<tr><td><b>Frete:</b>&nbsp;</td><td>" + _tipoFrete + "</td><td colspan='2'></td></tr> " +
                        "<tr><td><b>Confirmou o Pedido:</b>&nbsp;</td><td>" + _usuarioConfirmou + "&nbsp;</td><td><b>Contato:&nbsp;</b></td><td>" + _fone1UsuarioConfirmou + " / " + _fone2UsuarioConfirmou + "</td></tr> " +
                        "<tr><td align='center'><br>Acesse o site " + linked + " e verifique as novas informações sobre este pedido.<br><br>" +
                        "<tr><td><br><br>&nbsp;&nbsp;Atenciosamente,<br></td></tr><tr><td><br>&nbsp;&nbsp;" +
                        "Equipe Cliente&Mercado<br></td></tr>" +
                        "<tr><td>&nbsp;</td></tr>" +
                        "<tr><td>&nbsp;</td></tr>" +
                        "</table></td></tr>" +
                        "</body></html>";

            //ENVIA o E-MAIL
            try
            {
                MailMessage mail = new MailMessage();

                if (!string.IsNullOrEmpty(_email1_EmpresaAdmCC))
                {
                    mail.To.Add(_email1_EmpresaAdmCC);
                }

                if (!string.IsNullOrEmpty(_email2_EmpresaAdmCC))
                {
                    MailAddress copy = new MailAddress(_email2_EmpresaAdmCC);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(_email1_UsuarioContatoAdmCC))
                {
                    MailAddress copy = new MailAddress(_email1_UsuarioContatoAdmCC);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(_email2_UsuarioContatoAdmCC))
                {
                    MailAddress copy = new MailAddress(_email2_UsuarioContatoAdmCC);
                    mail.CC.Add(copy);
                }

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
                //smtp.Send(mail);      <== DESCOMENTAR ANTES DE IR AO AR...

                return true;
            }
            catch (Exception erro)
            {
                throw erro;
            }

        }
    }
}
