using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RegisterDocumentController : ApiController
    {
        public HttpResponseMessage Get(string docIDs)
        {
            List<int> myDocIDs = docIDs.Split(',').Select(Int32.Parse).ToList();
            //List<DocumentView> documentList = new List<DocumentView>();
            string results = "";
            results = WebAPI.Models.Document.deleteDocumentByDocIDs(myDocIDs);

            var jsonNew = new
            {
                result = results
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public HttpResponseMessage Post([FromBody] List<Document> document_list)
        {

            String status = "";
            foreach (var document in document_list)
            {

                if (document.Operation == 1)
                    status += WebAPI.Models.Document.registerDocument(document);

                if (document.Operation == 2)
                    status += WebAPI.Models.Document.updateDocument(document);

                if (document.Operation == 3)
                    status += WebAPI.Models.Document.deleteDocument(document);

                //4 Do nothing
                if (document.Operation == 4)
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
