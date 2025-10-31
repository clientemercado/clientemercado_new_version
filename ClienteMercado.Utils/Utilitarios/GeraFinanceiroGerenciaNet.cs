using System;
using System.Collections.Specialized;
using System.Net;

namespace ClienteMercado.Utils.Utilitarios
{
    public class GeraFinanceiroGerenciaNet
    {
        public bool GerarFinanceiro(string _descricaoPlano, string _valorMensalidade, string _vencimento, bool _primeiroPagamento,
            string _nomeUsuarioEmpresaCobrado, string _enderecoDoCobrado, string _cidadeDoCobrado, string _bairroDoCobrado, string _cepDoCobrado,
            string _estadoDoCobrado, string _cpfCnpjcnpjDoCobrado)
        {
            try
            {
                //String url = "http://go.gerencianet.com.br/api/pagamento/xml";
                String url = "https://go.gerencianet.com.br/teste/api/pagamento/xml HTTP/1.1";
                String token = "AC5972D933E99D2F38A78D296944C84CE08F80F6";
                String urlRetorno = "";

                ////Comentado pois acredito que o que entendi desta url de retorno, esteja totalmente errado. Sei isto se confirmar, 
                ////excluir as linhas comentadas abaixo
                //if (_primeiroPagamento)
                //{
                //    urlRetorno = ""; //Retornar para tela de aviso de "AGUARDAR CONFIRMAÇÃO DE PGTO"
                //}
                //else
                //{
                //    urlRetorno = "http://www.clientemercado.com.br//Login/Login"; //Retornar à tela de LOGIN
                //}

                String xml = "<?xml version='1.0' encoding='utf-8'?>" +
                             "<integracao>" +
                                "<itens>" +
                                    "<item>" +
                                        "<itemValor>" + _valorMensalidade + "</itemValor>" +
                                        "<itemDescricao>" + _descricaoPlano + "</itemDescricao>" +
                                    "</item>" +
                                "</itens>" +
                                "<vencimento>" + _vencimento + "</vencimento>" +
                                "<solicitarEndereco>n</solicitarEndereco>" +
                                "<retorno>" +
                                "<url>" + urlRetorno + "</url>" +
                                "</retorno>" +
                                "<cliente>" +
                                    "<nome>" + _nomeUsuarioEmpresaCobrado + "</nome>" +
                                    "<cpf>" + _cpfCnpjcnpjDoCobrado + "</cpf>" +
                                    "<logradouro>" + _enderecoDoCobrado + "</logradouro>" +
                                    "<bairro>" + _bairroDoCobrado + "</bairro>" +
                                    "<cidade>" + _cidadeDoCobrado + "</cidade>" +
                                    "<cep>" + _cepDoCobrado + "</cep>" +
                                    "<estado>" + _estadoDoCobrado + "</estado>" +
                                "</cliente>" +
                              "</integracao>";

                WebClient client = new WebClient();
                NameValueCollection postData = new NameValueCollection() { { "token", token }, { "dados", xml } };
                byte[] response = client.UploadValues(url, postData);

                Console.WriteLine("Output: " + System.Text.Encoding.Default.GetString(response));

                return true;
            }
            catch (Exception erro)
            {
                throw erro;
            }

        }

    }
}
