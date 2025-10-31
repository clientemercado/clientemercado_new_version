using ClienteMercado.Data.Entities;
using ClienteMercado.Domain.Services;
using ClienteMercado.Models;
using ClienteMercado.Utils.Utilitarios;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.Controllers
{
    public class ContatoController : Controller
    {
        //
        // GET: /Contato/

        public ActionResult Contato()
        {
            Contato contato = new Contato();

            contato.ListagemAssuntos = ListagemAssuntos();

            return View(contato);
        }

        private static List<SelectListItem> ListagemAssuntos()
        {
            List<SelectListItem> listAssuntos = new List<SelectListItem>();

            listAssuntos.Add(new SelectListItem { Text = "Selecione uma opção", Value = "" });
            listAssuntos.Add(new SelectListItem { Text = "Cadastro de usuário", Value = "1" });
            listAssuntos.Add(new SelectListItem { Text = "Crítica ou reclamação", Value = "2" });
            listAssuntos.Add(new SelectListItem { Text = "Elogio", Value = "3" });
            listAssuntos.Add(new SelectListItem { Text = "Sugestão", Value = "4" });
            listAssuntos.Add(new SelectListItem { Text = "Problema com Fornecedor de produtos", Value = "5" });
            listAssuntos.Add(new SelectListItem { Text = "Problema com Prestador de serviços", Value = "6" });
            listAssuntos.Add(new SelectListItem { Text = "Outros", Value = "7" });
            listAssuntos.Add(new SelectListItem { Text = "Aplicativo Cliente & Mercado", Value = "8" });

            return listAssuntos;
        }

        //Salvar o Contato realizado pelo Usuário
        [HttpPost]
        public ActionResult Contato(Contato contatoCliente) //(Contato -> Classe do Modelo da View)
        {
            try
            {
                NContatoService negocio = new NContatoService();
                contato_cliente_mercado novoContato = new contato_cliente_mercado();

                novoContato.ASSUNTO_CONTATO_CLIENTE_MERCADO = contatoCliente.AssuntoPContato;
                novoContato.EMAIL_CONTATO_CLIENTE_MERCADO = contatoCliente.EmailPContato;
                novoContato.MENSAGEM_CONTATO_CLIENTE_MERCADO = contatoCliente.MensagemPContato;
                novoContato.NOME_CONTATO_CLIENTE_MERCADO = contatoCliente.NomePContato;

                contato_cliente_mercado contatoClienteMercado = negocio.GravarContato(novoContato);

                //Redireciona para a action abaixo passando por parametro o número do protocolo de registro no banco de dados.
                return RedirectToAction(String.Format("Confirmacao/" + contatoClienteMercado.ID_CONTATO_CLIENTE_MERCADO));
            }
            catch (Exception erro)
            {
                //Recarrega o DropDown de assuntos de Contato
                contatoCliente.ListagemAssuntos = ListagemAssuntos();

                return View(contatoCliente).ComMensagem("* Erro interno ao registrar seu contato! Por favor, tente mais tarde!");
            }
        }

        // GET: /Contato/Confirmacao
        public ActionResult Confirmacao(int id)
        {
            Models.Contato c = new Contato() { Protocolo = id };
            return View(c);
        }

    }
}
