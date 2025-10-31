using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class AcompanharCotacaoesViewModel : NossasCentraisDeComprasViewModel
    {
        public List<SelectListItem> inListaTiposDeFiltros { get; set; }
        public List<SelectListItem> inListaTiposDeCotacao { get; set; }
    }
}
