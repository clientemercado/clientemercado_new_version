using System;
using System.Text.RegularExpressions;

namespace ClienteMercado.Utils.Utilitarios
{
    public class CodificarNomeDeArquivo
    {
        public static string renomearComHash(string nomeDoArquivo)
        {
            /*
             RENOMEIA ARQUIVO (Gera novo nome para arquivos de imagem neste sistema, considerando a concatenação da geração de 3 sequencias de números double())
             */
            double numeros;
            string novoNomeArquivo = "";
            string[] extensao;
            Random rnd = new Random();

            //Pega a extensão do nome do arquivo
            extensao = nomeDoArquivo.Split('.');

            //Gerar nome codificado
            for (int i = 0; i <= 2; i++)
            {
                numeros = rnd.NextDouble();

                if (novoNomeArquivo == "")
                {
                    novoNomeArquivo = numeros.ToString();
                    novoNomeArquivo = Regex.Replace(novoNomeArquivo, "0,", "");
                }
                else
                {
                    novoNomeArquivo = novoNomeArquivo + "_" + numeros.ToString();
                    novoNomeArquivo = Regex.Replace(novoNomeArquivo, "0,", "");
                }
            }

            //Verifica a EXTENSÃO do nome do arquivo
            if (extensao[1] != "jpg")
            {
                extensao[1] = "jpg";
            }

            //Concatena NOME do ARQUIVO e EXTENSÃO
            if (extensao[1] != "")
            {
                novoNomeArquivo = novoNomeArquivo + "." + extensao[1];
            }

            return novoNomeArquivo;
        }
    }
}
