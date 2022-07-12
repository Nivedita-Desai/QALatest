using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestDocumentController : ApiController
    {
        public HttpResponseMessage Get(String DocumentSet, int projectID)
        {


            List<DocumentView> documentList = new List<DocumentView>();
            documentList = WebAPI.Models.Document.GetDocument(DocumentSet, projectID);


            var jsonNew = new
            {
                result = documentList
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        public dynamic GetDocumentStream(int documentID)
        {
            Document document = new Document();
            document = WebAPI.Models.Document.GetDocumentByDocID(documentID);
            var list = document;
            var Document = "";
            string base64String = Convert.ToBase64String(list.DocumentBinaryData, 0, list.DocumentBinaryData.Length);
            Document = "data:" + list.FileTye + ";base64," + base64String;


            return Document;
        }
        //public HttpResponseMessage GetDocumentStream(int documentID)
        //{
        //    //string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
        //    //string bookName = "sample." + format.ToLower();
        //    ////converting Pdf file into bytes array  
        //    //var dataBytes = File.ReadAllBytes(reqBook);
        //    ////adding bytes to memory stream   
        //    ////var dataStream = new MemoryStream(dataBytes);

        //    Document document = new Document();
        //    document = WebAPI.Models.Document.GetDocumentByDocID(documentID);
        //    var dataStream = new MemoryStream(document.DocumentBinaryData);
        //    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new StreamContent(dataStream);
        //    //httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = document.DocumentName;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        //    return httpResponseMessage;
        //}



    }
}