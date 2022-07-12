using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Helper
{
    public static class CopyUtil
    {
        public static void CopyFields<T>(T source, T destination)
        {
            var fields = source.GetType().GetProperties();

            foreach (var prop in fields)

            {
                //prop.SetValue()
                if (prop.Name == "CreatedDate"
                    || prop.Name == "UpdatedDate"
                    || prop.Name == "CreatedBy"
                    || prop.Name == "UpdatedBy"
                    || prop.Name.ToLower() == "id"
                    || prop.Name.ToLower() == "clientid"  //Client object
                    || prop.Name.ToLower() == "trendstatuscodeid" //TrendStatusCode object
                    || prop.Name == "DocumentTypeID" //DocumentType object
                    || prop.Name == "ODCTypeID" //ODCType Object
                    || prop.Name == "SubcontractorID" //Subcontractor Object
                    || prop.Name == "SubcontractorTypeID" //SubcontractorTypeID
                    || prop.Name == "UnitID" //UnitType Object
                    || prop.Name == "ActivityID" //ActivityID
                    )
                {
                    //Do nothing - don't copy the audit fields -> the audit fields will be handled by the CPPContext Class
                }
                else
                {
                    prop.SetValue(destination, prop.GetValue(source));
                }

            }
        }
    }
}