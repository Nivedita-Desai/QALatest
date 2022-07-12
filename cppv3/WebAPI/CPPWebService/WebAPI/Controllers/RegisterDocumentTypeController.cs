using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterDocumentTypeController : ApiController
    {
        public HttpResponseMessage Post([FromBody] List<DocumentType> document_type_list)
        {

            String status = "";
            foreach (var document_type in document_type_list)
            {

                if (document_type.Operation == 1)
                    status += WebAPI.Models.DocumentType.registerDocumentType(document_type);

                if (document_type.Operation == 2)
                    status += WebAPI.Models.DocumentType.updateDocumentType(document_type);

                if (document_type.Operation == 3)
                    status += WebAPI.Models.DocumentType.deleteDocumentType(document_type);

                //4 Do nothing
                if (document_type.Operation == 4)
                    status += "";
            }

            var jsonNew = new
            {
                result = status
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}
