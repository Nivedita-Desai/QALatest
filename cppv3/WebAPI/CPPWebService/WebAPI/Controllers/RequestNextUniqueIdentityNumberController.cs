using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class RequestNextUniqueIdentityNumberController : ApiController
    {
        public HttpResponseMessage Get(String NumberType, int OrganizationID, int PhaseID, String CategoryID)
        {
            String UniqueIdentityNumber = "";

            int num = 0;
            string paddedNewNum = "";

            if (NumberType == "Client")
            {
                UniqueIdentityNumber = Client.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BC"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BC" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "ClientPOC")                                 //Tanmay - 15/12/2021
            {
                UniqueIdentityNumber = clientPOC.nextUniqueIdentityNumber();
                if (UniqueIdentityNumber.Contains("PC"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "PC" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "Subcontractor")
            {
                UniqueIdentityNumber = Subcontractor.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BS"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BS" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "Material")
            {
                UniqueIdentityNumber = Material.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BM"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BM" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "MaterialCategory")
            {
                UniqueIdentityNumber = MaterialCategory.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BMC"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(3), out num);
                    num++;
                    paddedNewNum = "BMC" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "SubcontractorType")
            {
                UniqueIdentityNumber = SubcontractorType.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BST"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(3), out num);
                    num++;
                    paddedNewNum = "BST" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "FTEPosition")
            {
                UniqueIdentityNumber = FTEPosition.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BEP"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(3), out num);
                    num++;
                    paddedNewNum = "BEP" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "Employee")
            {
                UniqueIdentityNumber = Employee.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BE"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BE" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "ODCType")
            {
                UniqueIdentityNumber = ODCType.nextUniqueIdentityNumber();

                if (UniqueIdentityNumber.Contains("BO"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BO" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "Vendor")
            {
                UniqueIdentityNumber = Vendor.nextUniqueIdentityNumber();
                if (UniqueIdentityNumber.Contains("BV"))
                {
                    Int32.TryParse(UniqueIdentityNumber.Substring(2), out num);
                    num++;
                    paddedNewNum = "BV" + num.ToString().PadLeft(5, '0');
                }
            }
            else if (NumberType == "CategoryID")
            {
                paddedNewNum = Activity.nextCategoryID(OrganizationID, PhaseID);
            }
            else if (NumberType == "SubCategoryID")
            {
                paddedNewNum = Activity.nextSubCategoryID(OrganizationID, PhaseID, CategoryID);
            }
            var jsonNew = new
            {
                result = paddedNewNum
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);        
        }
    }
}