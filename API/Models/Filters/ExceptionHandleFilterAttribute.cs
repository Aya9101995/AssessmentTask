using API.Models.APIBaseModel;
using MawaqaaCodeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace API.Models.Filters
{
    public class ExceptionHandleFilterAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            APIModel oApiModel = new APIModel();
            oApiModel.APIStatus = -1;
            oApiModel.APIMessage = context.Exception.Message;
            string sErrorLogPath = "~/ErrorLog/Log.xml";
            string sToken = string.Empty;
            if (context.Request.Headers.Authorization != null && context.Request.Headers.Authorization.Scheme.ToLower() == "bearer")
            {
                sToken = context.Request.Headers.Authorization.Parameter;
            }
            ErrorLog.InsertToLog(System.Web.Hosting.HostingEnvironment.MapPath(sErrorLogPath)
                , await context.Request.Content.ReadAsStringAsync(), GetErrorMessage(context.Exception), context.Exception.StackTrace, sToken, "0");
            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, oApiModel);
        }
        public string GetErrorMessage(Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            sbError.Append(ex.Message);
            if (ex.InnerException != null)
            {
                sbError.Append("; Inner Exception: " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null)
                {
                    sbError.Append("; Inner Exception: " + ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null)
                    {
                        sbError.Append("; Inner Exception: " + ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
            return sbError.ToString();
        }
    }
}