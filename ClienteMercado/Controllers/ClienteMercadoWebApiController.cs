using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace ClienteMercado.Controllers
{
    public class ClienteMercadoWebApiController : ApiController
    {
        //
        // GET: /ClienteMercadoWebApi/

        ////============================================
        //[HttpPost]
        //[Route("api/Produtos/ImportarCsv")]
        //public async Task<IHttpActionResult> ImportarCsv()
        //{
        //    // Verifica se veio arquivo
        //    if (!Request.Content.IsMimeMultipartContent())
        //        return BadRequest("Conteúdo inválido. Deve ser multipart/form-data.");

        //    var root = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Uploads");
        //    Directory.CreateDirectory(root);

        //    var provider = new MultipartFormDataStreamProvider(root);
        //    await Request.Content.ReadAsMultipartAsync(provider);

        //    var arquivo = provider.FileData[0].LocalFileName;
        //    int totalProcessados = 0;

        //    try
        //    {
        //        using (var stream = new FileStream(arquivo, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        using (var reader = new StreamReader(stream))
        //        {
        //            string linha;
        //            bool primeiraLinha = true;

        //            while ((linha = await reader.ReadLineAsync()) != null)
        //            {
        //                if (primeiraLinha)
        //                {
        //                    primeiraLinha = false;
        //                    continue; // Pula cabeçalho
        //                }

        //                var partes = linha.Split(';'); // ou ',' conforme seu CSV

        //                if (partes.Length < 3)
        //                    continue;

        //                var produto = new dadosProdutosImportacao
        //                {
        //                    //Codigo = partes[0].Trim(),
        //                    //Nome = partes[1].Trim(),
        //                    //Preco = decimal.TryParse(partes[2], out var preco) ? preco : 0,
        //                    //Categoria = partes.Length > 3 ? partes[3].Trim() : null
        //                };

        //                // Exemplo: salva incrementalmente (banco, fila, etc.)
        //                SalvarProdutoNoBanco(produto);

        //                totalProcessados++;
        //            }
        //        }

        //        return Ok(new { processados = totalProcessados });
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //    finally
        //    {
        //        try { File.Delete(arquivo); } catch { }
        //    }
        //}

        //private void SalvarProdutoNoBanco(dadosProdutosImportacao produto)
        //{
        //    // Aqui você pode fazer uma inserção otimizada, por exemplo:
        //    // - Inserir em lote a cada 500 registros
        //    // - Usar SqlBulkCopy
        //    // - Inserir numa fila (RabbitMQ, etc.) para processamento posterior
        //}
        ////============================================

    }
}
