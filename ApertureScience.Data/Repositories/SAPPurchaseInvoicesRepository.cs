using ApertureScience.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public class SAPPurchaseInvoicesRepository : ISAPPurchaseInvoicesRepository
    {

        public async Task<Response> CreatePurchaseInvoices(SAPPurchaseInvoices sapPurchaseInvoices, string IDss)
        {
            Response respCreateFixedAsset = new Response();
            respCreateFixedAsset.conflicts = new List<Conflicts>();

            // Deshabilitar la validación del certificado SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;


            var url = "https://hanab1:50000/b1s/v1/PurchaseInvoices";
        
            var data = JsonConvert.SerializeObject(sapPurchaseInvoices);

            // Configurar cliente HTTP con cookies y cabeceras
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("B1S-WCFCompatible", "true");
                httpClient.DefaultRequestHeaders.Add("B1S-MetadataWithoutSession", "true");
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

                // Configurar cookies
                var cookies = new CookieContainer();
                cookies.Add(new Cookie("B1SESSION", IDss) { Domain = "hanab1" });
                cookies.Add(new Cookie("ROUTEID", ".node1") { Domain = "hanab1" });

                var handler = new HttpClientHandler() { CookieContainer = cookies, ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };

                // Asociar cliente HTTP con el controlador de cookies

                using (var httpClientWithHandler = new HttpClient(handler))
                {
                    var streamContent = new StringContent(data, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = streamContent };

                    var response = await httpClientWithHandler.SendAsync(request).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        // El activo fijo se creó exitosamente
                        respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Creado Correctamente" });
                        return respCreateFixedAsset;
                    }
                    else
                    {
                        //Response conflictsCreateFixedAsset = new Response();
                        // Manejar la respuesta no exitosa aquí
                        var errorResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        // Parsear la cadena JSON
                        var jsonResponse = JObject.Parse(errorResponse);

                        // Obtener el valor de "value"
                        var value = jsonResponse["error"]["message"]["value"].ToString();
                        //Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        //Console.WriteLine($"Detalles del error: {errorResponse}");
                        respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = value });
                        return respCreateFixedAsset;
                    }
                }
            }
        }
    }
}
