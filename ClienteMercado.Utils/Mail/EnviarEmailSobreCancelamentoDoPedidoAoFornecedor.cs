using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ClienteMercado.Utils.Mail
{
    public class EnviarEmailSobreCancelamentoDoPedidoAoFornecedor
    {
        public bool EnviarEmail(string _nomeCC, string _usuarioAdmCC, string _fone1EmpresaAdmCC, string _fone2EmpresaAdmCC, string _fone1UsuarioAdmCC, 
            string _fone2UsuarioAdmCC, string _email1EmpresaAdmCC, string _email2EmpresaAdmCC, string _emailContatoEmpresaAdmCC,  string _dataEnvioPedido, 
            string _numeroPedido, string _motivoDesistenciaDoPedido, string _empresaFornecedora, string _nomeContatoEmpresaFornecedora, 
            string _email1_EmpresaForn, string _email2_EmpresaForn, string _email1_UsuarioContatoEmpresaForn, string _email2_UsuarioContatoEmpresaForn,
            string numeroPedido, ItemASerCanceladoOPedidoDetalhesViewModel itemCanceladoDetalhes)
        {
            //Montando Link de Acesso ao site
            string comandoHref = "<a href=";
            string link1 = comandoHref + "http://www.clientemercado.com.br/";
            string link2 = "></a>";

            //Montando o corpo do e-mail
            string linked = (link1 + link2);
            string textoAssunto = "";
            string itemCancelado = "";
            string assunto = "";
            string mensagem = "";

            if (itemCanceladoDetalhes.descricaoItem != null)
            {
                textoAssunto = "CANCELAMENTO DE ITEM DO PEDIDO Nº " + numeroPedido + " - Comprador: " + _nomeCC;
                itemCancelado = itemCanceladoDetalhes.descricaoItem + " - Quant: " + itemCanceladoDetalhes.quantidadeItem + " " 
                    + itemCanceladoDetalhes.unidadeItem + " - " + itemCanceladoDetalhes.embalagemItem;
            }
            else
            {
                textoAssunto = "CANCELAMENTO DO PEDIDO Nº " + numeroPedido + " - Comprador: " + _nomeCC;
                itemCancelado = "TODOS OS ITENS do PEDIDO";
            }

            assunto = textoAssunto;
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
                        "<td><br>&nbsp;&nbsp;Sr(a) " + _nomeContatoEmpresaFornecedora + ", <br><br>" +
                        "&nbsp;&nbsp;A Central de Compras <b>" + _nomeCC + "</b>, DESISTIU do PEDIDO nº " + numeroPedido + ", efetuado no dia " + _dataEnvioPedido + "." +
                        "</td>" +
                        "</tr>" +
                        "<br><br>" +
                        "<tr><td colspan='4'><b>CARACTERÍSTICAS DO PEDIDO:</b></td></tr><br>" +
                        "<tr><td><b>Pedido Nº:</b>&nbsp;</td><td>" + numeroPedido + "</td><td colspan='2'></td></tr> " +
                        "<tr><td><b>Data Pedido:</b>&nbsp;</td><td>" + _dataEnvioPedido + "</td><td colspan='2'></td></tr> " +
                        "<tr><td><b>Confirmou CANCELAMENTO do Pedido:</b>&nbsp;</td><td>" + _usuarioAdmCC + "&nbsp;</td><td colspan='2'></td></tr> " +
                        "<td><b>Fone(s) Contato(s):&nbsp;</b></td><td>" + _fone1UsuarioAdmCC + " / " + _fone2UsuarioAdmCC + "</td><td colspan='2'></td></tr> " +
                        "<td><b>E-mail(s) Contato(s):&nbsp;</b></td><td>" + _email1EmpresaAdmCC + " / " + _emailContatoEmpresaAdmCC + "</td><td colspan='2'></td></tr> " +
                        "<tr><td colspan='3'></td></tr>" +
                        "<tr><td><b>Item CANCELADO:</b>&nbsp;</td><td colspan='2'>" + itemCancelado + "</td></tr>" +
                        "<tr><td><b>Motivo DESISTÊNCIA:</b>&nbsp;</td><td colspan='2'>" + _motivoDesistenciaDoPedido + "</td></tr>" +
                        "<tr><td align='center'><br>Para mais detalhes, acesse o site " + linked + " e verifique as novas informações sobre este pedido.<br><br></td></tr>" +
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

                if (!string.IsNullOrEmpty(_email1_EmpresaForn))
                {
                    mail.To.Add(_email1_EmpresaForn);
                }

                if (!string.IsNullOrEmpty(_email2_EmpresaForn))
                {
                    MailAddress copy = new MailAddress(_email2_EmpresaForn);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(_email1_UsuarioContatoEmpresaForn))
                {
                    MailAddress copy = new MailAddress(_email1_UsuarioContatoEmpresaForn);
                    mail.CC.Add(copy);
                }

                if (!string.IsNullOrEmpty(_email2_UsuarioContatoEmpresaForn))
                {
                    MailAddress copy = new MailAddress(_email2_UsuarioContatoEmpresaForn);
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
