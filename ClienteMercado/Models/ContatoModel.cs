using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class Contato
    {
        public int Protocolo { get; set; }

        [Required(ErrorMessage = "* Informe seu Nome")]
        [StringLength(51)]
        [DisplayName("Nome: ")]
        public string NomePContato { get; set; }

        [Required(ErrorMessage = "* Informe seu e-mail de contato")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "E-mail inválido")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail: ")]
        public string EmailPContato { get; set; }

        [Required(ErrorMessage = "* Informe o Assunto que deseja tratar conosco")]
        [DisplayName("Assunto: ")]
        public int AssuntoPContato { get; set; }
        public List<SelectListItem> ListagemAssuntos { get; set; }

        [Required(ErrorMessage = "* Digite sua Mensagem")]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Mensagem: ")]
        public string MensagemPContato { get; set; }

    }

}