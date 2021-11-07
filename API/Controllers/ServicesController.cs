using API.Models.APIs.InvoicesAPI;
using API.Models.APIs.ItemsAPI;
using API.Models.APIs.UsersAPI;
using API.Models.Filters;
using MawaqaaCodeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;

namespace API.Controllers
{
    [ExceptionHandleFilter]
    public class ServicesController : ApiController
    {
        #region ModelValidation
        protected internal bool TryValidateModel(object model)
        {
            return TryValidateModel(model, null /* prefix */);
        }
        protected internal bool TryValidateModel(object model, string prefix)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var t = new ModelBindingExecutionContext(new HttpContextWrapper(HttpContext.Current), new System.Web.ModelBinding.ModelStateDictionary());

            foreach (ModelValidationResult validationResult in ModelValidator.GetModelValidator(metadata, t).Validate(null))
            {
                ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
            }

            return ModelState.IsValid;
        }
        #endregion
        #region ItemsAPI
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("SaveItem")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> SaveItem(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            ItemsRequestAPI oItemsRequestAPI = requestBody.DeserializeJsonToObject<ItemsRequestAPI>();
            oItemsRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            ItemsAPI oItemsAPI = new ItemsAPI();
            ItemsResponseAPI oItemsResponseAPI = new ItemsResponseAPI();
            if (ModelState.IsValid && TryValidateModel(oItemsRequestAPI.oItem))
            {
                oItemsResponseAPI = oItemsAPI.SaveItem(oItemsRequestAPI);
            }
            else
            {
                oItemsResponseAPI.APIMessage = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                oItemsResponseAPI.APIStatus = -1;
            }
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetAvailableItems")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetAvailableItems(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            ItemsRequestAPI oItemsRequestAPI = requestBody.DeserializeJsonToObject<ItemsRequestAPI>();
            oItemsRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            ItemsAPI oItemsAPI = new ItemsAPI();
            ItemsResponseAPI oItemsResponseAPI = oItemsAPI.GetAvailableItems(oItemsRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("DeleteItem")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> DeleteItem(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            ItemsRequestAPI oItemsRequestAPI = requestBody.DeserializeJsonToObject<ItemsRequestAPI>();
            oItemsRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            ItemsAPI oItemsAPI = new ItemsAPI();
            ItemsResponseAPI oItemsResponseAPI = oItemsAPI.DeleteItem(oItemsRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetItem")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetItem(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            ItemsRequestAPI oItemsRequestAPI = requestBody.DeserializeJsonToObject<ItemsRequestAPI>();
            oItemsRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            ItemsAPI oItemsAPI = new ItemsAPI();
            ItemResponseAPI oItemsResponseAPI = oItemsAPI.GetItem(oItemsRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }
        #endregion
        #region UsersAPI
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("Login")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> Login(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UserResponseAPI oItemsResponseAPI = oUsersAPI.Login(oUsersRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("Register")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> Register(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();
            if (ModelState.IsValid && TryValidateModel(oUsersRequestAPI.oUser))
            {
                oUserResponseAPI = oUsersAPI.Register(oUsersRequestAPI);
            }
            else
            {
                oUserResponseAPI.APIMessage = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                oUserResponseAPI.APIStatus = -1;
            }
            return Request.CreateResponse(HttpStatusCode.OK, oUserResponseAPI);
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetUserProfile")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetUserProfile(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UserResponseAPI oItemsResponseAPI = oUsersAPI.GetUserProfile(oUsersRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("UpdateUserProfile")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> UpdateUserProfile(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UserResponseAPI oUserResponseAPI = new UserResponseAPI();
            if (ModelState.IsValid && TryValidateModel(oUsersRequestAPI.oUser))
            {
                oUserResponseAPI = oUsersAPI.UpdateUserProfile(oUsersRequestAPI);
            }
            else
            {
                oUserResponseAPI.APIMessage = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                oUserResponseAPI.APIStatus = -1;
            }
            return Request.CreateResponse(HttpStatusCode.OK, oUserResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("Logout")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> Logout(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UserResponseAPI oItemsResponseAPI = oUsersAPI.Logout(oUsersRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetAllUsers")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetAllUsers(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            UsersRequestAPI oUsersRequestAPI = requestBody.DeserializeJsonToObject<UsersRequestAPI>();
            oUsersRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            UsersAPI oUsersAPI = new UsersAPI();
            UsersResponseAPI oItemsResponseAPI = oUsersAPI.GetAllUsers(oUsersRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oItemsResponseAPI);
        }

        #endregion
        #region InvoicesAPI

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("PlaceOrder")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> PlaceOrder(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            InvoiceRequestAPI oInvoiceRequestAPI = requestBody.DeserializeJsonToObject<InvoiceRequestAPI>();
            oInvoiceRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            InvoicesAPI oInvoicesAPI = new InvoicesAPI();
            InvoiceResponseAPI oInvoiceResponseAPI = oInvoicesAPI.PlaceOrder(oInvoiceRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oInvoiceResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetUserInvoices")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetUserInvoices(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            InvoiceRequestAPI oInvoiceRequestAPI = requestBody.DeserializeJsonToObject<InvoiceRequestAPI>();
            oInvoiceRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            InvoicesAPI oInvoicesAPI = new InvoicesAPI();
            InvoicesResponseAPI oInvoicesResponseAPI = oInvoicesAPI.GetUserInvoices(oInvoiceRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oInvoicesResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("GetUserInvoiceItems")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetUserInvoiceItems(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            InvoiceRequestAPI oInvoiceRequestAPI = requestBody.DeserializeJsonToObject<InvoiceRequestAPI>();
            oInvoiceRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            InvoicesAPI oInvoicesAPI = new InvoicesAPI();
            InvoiceResponseAPI oInvoicesResponseAPI = oInvoicesAPI.GetUserInvoiceItems(oInvoiceRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oInvoicesResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("ChangeOrderStatus")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ChangeOrderStatus(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            InvoiceRequestAPI oInvoiceRequestAPI = requestBody.DeserializeJsonToObject<InvoiceRequestAPI>();
            oInvoiceRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            InvoicesAPI oInvoicesAPI = new InvoicesAPI();
            InvoiceResponseAPI oInvoicesResponseAPI = oInvoicesAPI.ChangeOrderStatus(oInvoiceRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oInvoicesResponseAPI);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [ActionName("CancelOrder")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> CancelOrder(HttpRequestMessage request)
        {
            string requestBody = await request.Content.ReadAsStringAsync();

            InvoiceRequestAPI oInvoiceRequestAPI = requestBody.DeserializeJsonToObject<InvoiceRequestAPI>();
            oInvoiceRequestAPI.APIToken = request.Headers.Authorization != null ? request.Headers.Authorization.Parameter : "";
            InvoicesAPI oInvoicesAPI = new InvoicesAPI();
            InvoiceResponseAPI oInvoicesResponseAPI = oInvoicesAPI.CancelOrder(oInvoiceRequestAPI);
            return Request.CreateResponse(HttpStatusCode.OK, oInvoicesResponseAPI);
        }

        #endregion
    }
}
