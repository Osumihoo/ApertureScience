using ApertureScience.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Dapper;
using System.Data.Odbc;
using System.Data;
using SAPbobsCOM;

namespace ApertureScience.Data.Repositories
{
    public class SAPFixedAssetRepository : ISAPFixedAssetRepository
    {
        private readonly ODBCConfiguration _connectionString;

        public SAPFixedAssetRepository(ODBCConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected OdbcConnection dbConnection()
        {
            return new OdbcConnection(_connectionString.ConnectionString);
        }
        public async Task<Response> CreateFixedAsset(SAPFixedAsset sapFixedAsset, string IDss)
        {
            Response respCreateFixedAsset = new Response();
            respCreateFixedAsset.conflicts = new List<Conflicts>();
            
            // Deshabilitar la validación del certificado SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;


            var url = "https://hanab1:50000/b1s/v1/Items";
            var data = JsonConvert.SerializeObject(sapFixedAsset);

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

        public async Task<Response> GetTheLast()
        {

            Response responseTheLast = new Response();
            responseTheLast.responseItemCode = new ResponseItemCode();
            responseTheLast.conflicts = new List<Conflicts>();
            SAPDB companyDB = new SAPDB();

            using (var db = dbConnection())
            {
                if (db.State == ConnectionState.Open)
                    db.Close();

                db.Open();

                try
                {
                    string queryLIC = "Select \"ItemCode\" " +
                                        "From " + companyDB.CompanyDB + ".OITM \r\n" +
                                        "Where \"ItmsGrpCod\" = '141' " +
                                        "AND \"CreateDate\"=(SELECT MAX(\"CreateDate\") FROM " + companyDB.CompanyDB + ".OITM WHERE \"ItmsGrpCod\" = '141');";

                    using (var cmd = new OdbcCommand(queryLIC, db))
                    {
                        cmd.CommandTimeout = 2000;

                        using (OdbcDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var itemCode = dr["ItemCode"].ToString();
                                responseTheLast.responseItemCode.ItemCode = itemCode;
                            }
                        }
                    }
                    return responseTheLast;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    Console.WriteLine($"Error: {ex.Message}");
                    responseTheLast.conflicts.Add(new Conflicts { Problems = true, Description = ex.Message });
                    return responseTheLast;
                }
            }
        }

        public async Task<IEnumerable<Response>> GetAllActiveClass()
        {
            Response responseAllActiveClass = new Response();
            responseAllActiveClass.responseSAPActiveClass = new ResponseSAPActiveClass();
            responseAllActiveClass.conflicts = new List<Conflicts>();
            SAPDB companyDB = new SAPDB();

            List<Response> responseList = new List<Response>();

            using (var db = dbConnection())
            {
                if (db.State == ConnectionState.Open)
                    db.Close();

                db.Open();

                try
                {

                    string queryLIC = "SELECT \"Code\",\"Name\" FROM " + companyDB.CompanyDB + " .OACS";

                    using (var cmd = new OdbcCommand(queryLIC, db))
                    {
                        cmd.CommandTimeout = 2000;

                        using (OdbcDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                responseAllActiveClass = new Response();
                                responseAllActiveClass.responseSAPActiveClass = new ResponseSAPActiveClass();
                                responseAllActiveClass.conflicts = new List<Conflicts>();

                                var code = dr["Code"].ToString();
                                responseAllActiveClass.responseSAPActiveClass.Code = code;
                                var name = dr["Name"].ToString();
                                responseAllActiveClass.responseSAPActiveClass.Name = name;

                                responseList.Add(responseAllActiveClass);
                            }
                        }
                    }
                    return responseList;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    Console.WriteLine($"Error: {ex.Message}");
                    responseAllActiveClass.conflicts.Add(new Conflicts { Problems = true, Description = ex.Message });
                    responseList.Add(responseAllActiveClass);
                    return responseList;
                }
            }
        }

        public async Task<Response> UpdateFixedAssetDepreciation(SAPFixedAssetPatch sapFixedAsset, string IDss)
        {
            Response respUpdateFixedAssetDepreciation = new Response();
            respUpdateFixedAssetDepreciation.conflicts = new List<Conflicts>();

            // Deshabilitar la validación del certificado SSL
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var itemCode = sapFixedAsset.ItemCode;
            var url = $"https://hanab1:50000/b1s/v1/Items('{itemCode}')";
            var data = JsonConvert.SerializeObject(sapFixedAsset);

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
                    var request = new HttpRequestMessage(HttpMethod.Patch, url) { Content = streamContent };

                    var response = await httpClientWithHandler.SendAsync(request).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        // El activo fijo se creó exitosamente
                        respUpdateFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = false, Description = "Activo Creado Correctamente" });
                        return respUpdateFixedAssetDepreciation;
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
                        respUpdateFixedAssetDepreciation.conflicts.Add(new Conflicts { Problems = true, Description = value });
                        return respUpdateFixedAssetDepreciation;
                    }
                }
            }
        }
    }
}
