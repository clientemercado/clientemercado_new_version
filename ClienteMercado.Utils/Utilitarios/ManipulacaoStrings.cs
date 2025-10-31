namespace ClienteMercado.Utils.Utilitarios
{
    public class ManipulacaoStrings
    {
        //Pega o primeiro nome do Usuário
        public static string pegarParteNomeUsuario(string usuario)
        {
            int tamanho = usuario.Length;
            int posicao = usuario.IndexOf(" ");

            if (posicao >= 0)
            {
                usuario = usuario.Substring(0, posicao);
            }

            return usuario.Trim();
        }
    }
}
