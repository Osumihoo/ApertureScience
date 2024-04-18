using ApertureScience.Model;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data
{
    public class SAPLogin
    {
        public async Task<string> SAPLog()
        {
            StringBuilder builder = new StringBuilder();
            LoginSL loginSL = new LoginSL();
            //var bodyjson = Newtonsoft.Json.JsonConvert.SerializeObject(loginSL);
            //var IDss = "";

            //var jsonData = JsonConvert.SerializeObject(data);
            

            // Deshabilitar la validación del certificado SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var data = "{    \"CompanyDB\": \"" + loginSL.CompanyDB + "\",    \"UserName\": \"" + loginSL.UserName + "\",       \"Password\": \"" + loginSL.Password + "\"}"; 
            var url = "https://hanab1:50000/b1s/v1/Login";

            var handler = new HttpClientHandler() { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };


            using (var httpClient = new HttpClient(handler))
            {
                // Configurar cabeceras
                httpClient.DefaultRequestHeaders.Add("B1S-WCFCompatible", "true");
                httpClient.DefaultRequestHeaders.Add("B1S-MetadataWithoutSession", "true");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

                // Configurar contenido de la solicitud
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                // Realizar solicitud HTTP POST
                var response = await httpClient.PostAsync(url, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Obtener cookies de la respuesta
                    var cookies = response.Headers.GetValues("Set-Cookie");

                    // Buscar la cookie B1SESSION
                    string sessionId = null;
                    foreach (var cookie in cookies)
                    {
                        if (cookie.StartsWith("B1SESSION="))
                        {
                            // Extraer el valor de la cookie B1SESSION
                            sessionId = cookie.Split(';')[0].Split('=')[1];
                            break;
                        }
                    }
                    return sessionId;
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
                    //respCreateFixedAsset.conflicts.Add(new Conflicts { Problems = true, Description = value });
                    return value;
                }
            }

            
            
        }


    }
}
