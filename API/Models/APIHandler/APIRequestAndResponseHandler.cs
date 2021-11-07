using API.Models.APIBaseModel;
using Core.Models.Modules.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace API.Models.APIHandler
{
    public class APIRequestAndResponseHandler : DelegatingHandler
    {
        List<string> lstApisWithoutAuth { get; set; }
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                lstApisWithoutAuth = GetApisWithoutAuth();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                string sAPIKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"].ToString();
                string requestBody = await request.Content.ReadAsStringAsync();
                APIModel oAPIModel = JsonConvert.DeserializeObject<APIModel>(requestBody);
                bool IsValidToken = false;
                if (lstApisWithoutAuth.Any(x => x.ToLower() == request.RequestUri.Segments[request.RequestUri.Segments.Length - 1].ToLower()))
                {
                    if (oAPIModel.APIKey == sAPIKey)
                    {
                        IsValidToken = true;
                    }
                    else
                    {
                        IsValidToken = false;
                    }
                }
                else
                {
                    if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme.ToLower() == "bearer")
                    {
                        string sToken = request.Headers.Authorization.Parameter;
                        oAPIModel.APIToken = sToken;
                        oAPIModel.UserID = UsersCOM.GetUserIDByToken(sToken);
                        IsValidToken = oAPIModel.UserID > 0;
                    }
                    else
                    {
                        oAPIModel.LanguageID = oAPIModel.LanguageID != 0 ? oAPIModel.LanguageID : 1;
                        oAPIModel.APIStatus = -3;
                        return request.CreateResponse(HttpStatusCode.OK, oAPIModel);
                    }
                }
                if (IsValidToken)
                {
                    // log request body


                    // let other handlers process the request
                    var result = await base.SendAsync(request, cancellationToken);
                    var responseBody = "";
                    if (result.Content != null)
                    {
                        // once response body is ready, log it
                        responseBody = await result.Content.ReadAsStringAsync();
                    }
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                    return result;
                }
                else
                {
                    oAPIModel.APIStatus = -3;
                    oAPIModel.APIMessage = "you are not authorized to access";
                    return request.CreateResponse(HttpStatusCode.OK, oAPIModel);
                }

            }
            catch (Exception ex)
            {
                APIModel oAPIModel = new APIModel();
                oAPIModel.APIStatus = -1;
                oAPIModel.APIMessage = ex.Message;
                return request.CreateResponse(HttpStatusCode.OK, oAPIModel);
            }

        }


        public List<string> GetApisWithoutAuth()
        {
            List<string> lstApis = new List<string>();

            lstApis.Add("SaveItem");
            lstApis.Add("GetAvailableItems");
            lstApis.Add("GetItem");
            lstApis.Add("DeleteItem");
            lstApis.Add("Register");
            lstApis.Add("Login");
            lstApis.Add("GetAllUsers");
            return lstApis;
        }
    }
}