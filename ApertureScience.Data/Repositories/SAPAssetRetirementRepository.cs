using ApertureScience.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public class SAPAssetRetirementRepository : ISAPAssetRetirementRepository
    {
        private readonly ODBCConfiguration _connectionString;

        public SAPAssetRetirementRepository(ODBCConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected OdbcConnection dbConnection()
        {
            return new OdbcConnection(_connectionString.ConnectionString);
        }
        public async Task<Response> CreateAssetRetirement(SAPAssetRetirement sapAssetRetirement, string IDss)
        {
            Response respCreateAssetRetirement = new Response();
            respCreateAssetRetirement.conflicts = new List<Conflicts>();

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var url = "https://hanab1:50000/b1s/v1/AssetRetirement";
            var data = JsonConvert.SerializeObject(sapAssetRetirement);

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
                        // El activo fijo se dió de baja exitosamente
                        respCreateAssetRetirement.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Dado de Baja Correctamente" });
                        return respCreateAssetRetirement;
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
                        respCreateAssetRetirement.conflicts.Add(new Conflicts { Problems = true, Description = value });
                        return respCreateAssetRetirement;
                    }
                }
            }

        }
    }
}
