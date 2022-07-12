using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models.ExportToMPP;

namespace WebAPI.Controllers
{
    public class GraphController : ApiController
    {
        private GetGraph _GetGraph;
        [HttpGet]
        [Route("Request/GetGraph/")]
        public Data Get(int projectId)
        {
            _GetGraph = new GetGraph();
            //var lstExportData = _ExportProcess.Export(projectId, trendNumber, granularity);
            Data lstGraphDetails = _GetGraph.GetDetails(projectId);
            //var jsonNew = new
            //{
            //    result = lstGraphDetails
            //};
            //return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
            return lstGraphDetails;
        }

        //public HttpResponseMessage GetGraph(int projectId, DateTime startDate)
        //{
        //    _GetGraph = new GetGraph();
        //    //var lstExportData = _ExportProcess.Export(projectId, trendNumber, granularity);
        //    List<GraphDetail> lstGraphDetails = _GetGraph.GetDetails(projectId, startDate);
        //    var jsonNew = new
        //    {
        //        result = lstGraphDetails
        //    };
        //    return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        //}

        //public HttpResponseMessage GetGraph(int projectId, DateTime stratDate) //, string granularity
        //{
        //    _GetGraph = new GetGraph();
        //    //var lstExportData = _ExportProcess.Export(projectId, trendNumber, granularity);
        //    List<GraphDetails> lstGraphDetails = _GetGraph.GetDetails(projectId, stratDate);
        //    var jsonNew = new
        //    {
        //        result = lstGraphDetails
        //    };
        //    return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        //}
    }
}
