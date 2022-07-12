using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;

using WebAPI.Models;
using System.Diagnostics;

namespace WebAPI.Controllers
{
    [Authorize]
    public class RegisterCostController : System.Web.Http.ApiController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /RegisterFTECost/
        //public HttpResponseMessage Get(int Operation, FTECost.String ProgramID = "null", FTECost.String ProgramElementID = "null", FTECost.String ProjectID = "null", FTECost.String TrendNumber = "null", FTECost.String ActivityID = "null", FTECost.String FTECostID = "null", FTECost.String FTEStartDate = "null", FTECost.String FTEEndDate = "null", FTECost.String FTEPosition = "null", FTECost.String FTEValue = "null", FTECost.String FTEHourlyRate = "null", FTECost.String FTEHours = "null", FTECost.String FTECost = "null")
        public HttpResponseMessage Post([FromBody] List<Cost> CostRow)
        {

            String status = "";
            Stopwatch clock = Stopwatch.StartNew();
            foreach (var cost in CostRow)
            {
                if (cost.Operation == 4)
                {
                    //Labor
                    if (cost.CostType == "F")
                        status = WebAPI.Models.CostFTE.updateMultipleCostFTE(cost.Operation, cost.ProgramID, cost.ProgramElementID,
                                                                            cost.ProjectID, cost.TrendNumber, cost.ActivityID,
                                                                            cost.CostID, cost.StartDate, cost.EndDate,
                                                                            cost.Description, cost.TextBoxValue, cost.Base,
                                                                            cost.FTEHours, cost.FTECost, cost.Scale,
                                                                           cost.FTEIDList, cost.Drag_Direction, cost.NumberOfTextboxToBeRemoved, cost.EmployeeID);
                    //Subcontractor
                    if (cost.CostType == "L")
                        status = WebAPI.Models.CostLumpsum.updateMultipleCostLumpsum(cost.Operation, cost.ProgramID, cost.ProgramElementID,
                                                                                cost.ProjectID, cost.TrendNumber, cost.ActivityID,
                                                                                cost.CostID, cost.StartDate, cost.EndDate,
                                                                                cost.Description, cost.TextBoxValue, cost.Scale,
                                                                                cost.FTEIDList, cost.Drag_Direction, cost.NumberOfTextboxToBeRemoved, 1, cost.CostLineItemID); //1 - CostTrackTypeID  //Manasi 31-08-2020
                    //Material
                    if (cost.CostType == "U")
                        status = WebAPI.Models.CostUnit.updateMultipleCostUnit(cost.Operation, cost.ProgramID, cost.ProgramElementID,
                                                                                cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID,
                                                                                cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue,
                                                                                cost.Base, cost.Scale, cost.FTEIDList, cost.Drag_Direction, cost.UnitType, cost.NumberOfTextboxToBeRemoved, 1, cost.CostLineItemID);//1 CostTrackTypeID  //Manasi 31-08-2020
                    //ODC
                    if (cost.CostType == "ODC")
                        status = WebAPI.Models.CostODC.updateMultipleCostODC(cost.Operation, cost.ProgramID, cost.ProgramElementID,
                                                                                cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID,
                                                                                cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue,
                                                                                cost.Base, cost.Scale, cost.FTEIDList, cost.Drag_Direction, cost.UnitType, cost.NumberOfTextboxToBeRemoved, 1, cost.CostLineItemID);

                }
                else if (cost.Operation == 5)
                {
                    //update on activity date change = (left-left and right-right)
                    if (cost.CostType == "F")
                        status = WebAPI.Models.CostFTE.updateCostFTELeftLeft(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.FTEHours, cost.FTECost, cost.Scale, cost.FTEIDList, cost.EmployeeID, cost.CostLineItemID, cost.CostTrackTypeID);
                    if (cost.CostType == "L")
                        status = WebAPI.Models.CostLumpsum.updateCostLumpsumLeftLeft(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Scale, cost.FTEIDList, cost.SubcontractorTypeID, cost.SubcontractorID, 1, cost.CostLineItemID); //1 - CostTrackTypeID
                    if (cost.CostType == "U")
                        status = WebAPI.Models.CostUnit.updateUnitCostLeftLeft(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList,cost.MaterialCategoryID, cost.MaterialID, cost.CostLineItemID, 1);//1 - CostTracTypeID
                    if (cost.CostType == "ODC")
                        status = WebAPI.Models.CostODC.updateODCCostLeftLeft(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList, cost.ODCTypeID,cost.CostLineItemID, cost.CostTrackTypeID);


                }


                else
                {
                    var uniqueDuplicateErrorMessage = "";
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    if (cost.CostType == "F")
                    {
                        uniqueDuplicateErrorMessage = WebAPI.Models.CostFTE.updateCostFTE(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.FTEHours, cost.FTECost, cost.Scale, cost.FTEIDList, cost.EmployeeID, cost.CostTrackTypes, cost.CostLineItemID);
                    }
                    if (cost.CostType == "L")
                    {
                        
                       
                        uniqueDuplicateErrorMessage = WebAPI.Models.CostLumpsum.updateCostLumpsum(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Scale, cost.FTEIDList, cost.SubcontractorTypeID, cost.SubcontractorID, cost.CostTrackTypes, cost.CostLineItemID);
                      
                    }
                    if (cost.CostType == "U")
                    {
                        uniqueDuplicateErrorMessage = WebAPI.Models.CostUnit.updateCostUnit(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList, cost.MaterialCategoryID, cost.MaterialID, cost.CostTrackTypes, cost.CostLineItemID);
                    }
                    if (cost.CostType == "ODC")
                    {
                        uniqueDuplicateErrorMessage = WebAPI.Models.CostODC.updateCostODC(cost.Operation, cost.ProgramID, cost.ProgramElementID, cost.ProjectID, cost.TrendNumber, cost.ActivityID, cost.CostID, cost.StartDate, cost.EndDate, cost.Description, cost.TextBoxValue, cost.Base, cost.Scale, cost.UnitType, cost.FTEIDList, cost.CostTrackTypes, cost.CostLineItemID);
                    }

                    if (!status.Contains(uniqueDuplicateErrorMessage))
                    {
                        status += uniqueDuplicateErrorMessage;
                    }
                    stopwatch.Stop();
                    logger.Debug("Save Cost elapsed time " + stopwatch.ElapsedMilliseconds + " for cost type " + cost.CostType);

                }

            }
            var jsonNew = new
            {
                result = status
            };


            clock.Stop();
            logger.Debug("Saving cost elapsed time : " + clock.Elapsed.TotalMilliseconds);
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }
    }
}