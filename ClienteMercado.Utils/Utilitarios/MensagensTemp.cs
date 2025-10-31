using System.Web.Mvc;

namespace ClienteMercado.Utils.Utilitarios
{
    public static class ActionResultExtensions
    {
        public static ActionResult ComMensagem(this ActionResult actionResult, string mensagem)
        {
            return new TempDataActionResult(actionResult, mensagem);
        }
    }

    public class TempDataActionResult : ActionResult
    {
        private readonly ActionResult _actionResult;
        private readonly string _mensagem;

        public TempDataActionResult(ActionResult actionResult, string mensagem)
        {
            _actionResult = actionResult;
            _mensagem = mensagem;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.TempData["Mensagem"] = MvcHtmlString.Create(_mensagem);
            _actionResult.ExecuteResult(context);
        }
    }
}
