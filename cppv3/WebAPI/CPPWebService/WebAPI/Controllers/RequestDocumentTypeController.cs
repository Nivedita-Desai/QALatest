using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestDocumentTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {

            List<DocumentType> documentTypeList = new List<DocumentType>();
            documentTypeList = WebAPI.Models.DocumentType.GetDocumentType();


            var jsonNew = new
            {
                result = documentTypeList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}