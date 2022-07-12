using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Data.Entity;
using WebAPI.Helper;

namespace WebAPI.Models
{
    [Table("employee")]
    public class Employee : Audit
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int FTEPositionID { get; set; }
        public int OrganizationID { get; set; }
        public String Name { get; set; } //Full Name
        public Double HourlyRate { get; set; }
        public byte isActive { get; set; }
        public String ReferenceID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleName { get; set; }
        public Boolean IsUnion { get; set; }
        public Boolean IsExempt { get; set; } //For Future use
        public String UniqueIdentityNumber { get; set; }
        //Audit Fields
        //public String CreatedBy { get; set; }
        //public String UpdatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime CreatedDate { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime UpdatedDate { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("FTEPositionID")]
        public virtual FTEPosition FTEPosition { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }

        //Get all Employees
        public static List<Employee> getAllEmployees()
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Employee> employeeList = new List<Employee>();
            
            using (var ctx = new CPPDbContext())
            {
                try
                {

                    employeeList = ctx.Employee.OrderBy(e => e.UniqueIdentityNumber).OrderBy(x=>x.Name).ToList();

                    #region Two table join example. Also uncomment class
                    //List<Employee_FTEDEsc> employee_FTEDEscList = new List<Employee_FTEDEsc>();
                    //employee_FTEDEscList = ctx.Employee.
                    //                Join(ctx.FtePosition, e => e.FTEPositionID, fte => fte.Id, (e, fte) => new { e, fte }).
                    //                OrderBy(x => x.e.Name).
                    //                Select(m => new Employee_FTEDEsc
                    //                {
                    //                    ID = m.e.ID,
                    //                    FTEPositionID = m.e.FTEPositionID,
                    //                    PositionDesc = m.fte.PositionDescription,
                    //                    Name = m.e.Name,
                    //                    HourlyRate = m.e.HourlyRate,
                    //                    isActive = m.e.isActive
                    //                }).
                    //                ToList();
                    #endregion

                    //foreach (var employee in employeeList)
                    //{

                    //foreach (var item in facility.facilityAssets)
                    //{
                    //    item.Asset = ctx.Asset
                    //                    .Include(ar => ar.assetSupplier)
                    //                    .Include(f => f.assetComponents.Select(ach => ach.assetComponentHistories))
                    //                    .Include(h => h.assetHistories)
                    //                    .Where(i => i.ID == item.AssetID).FirstOrDefault();
                    //}
                    //}

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                finally
                {
                    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
                }
            }

            return employeeList;
        }

        //Get Employees by OrgID
        public static List<Employee> getEmployeesByOrgID(int orgID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<Employee> employeeList = new List<Employee>();

            using (var ctx = new CPPDbContext())
            {
                try
                {

                    // employeeList = ctx.Employee.Where(e => e.OrganizationID.Equals(orgID)).OrderBy(e => e.UniqueIdentityNumber).ToList();
                    employeeList = ctx.Employee.Where(e => e.OrganizationID.Equals(orgID)).OrderBy(e => e.Name).ToList();

                    #region Two table join example. Also uncomment class
                    //List<Employee_FTEDEsc> employee_FTEDEscList = new List<Employee_FTEDEsc>();
                    //employee_FTEDEscList = ctx.Employee.
                    //                Join(ctx.FtePosition, e => e.FTEPositionID, fte => fte.Id, (e, fte) => new { e, fte }).
                    //                OrderBy(x => x.e.Name).
                    //                Select(m => new Employee_FTEDEsc
                    //                {
                    //                    ID = m.e.ID,
                    //                    FTEPositionID = m.e.FTEPositionID,
                    //                    PositionDesc = m.fte.PositionDescription,
                    //                    Name = m.e.Name,
                    //                    HourlyRate = m.e.HourlyRate,
                    //                    isActive = m.e.isActive
                    //                }).
                    //                ToList();
                    #endregion

                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
                finally
                {
                    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
                }
            }

            return employeeList;
        }

        public static String add(Employee employee)
        {

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            String result = "";

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    Employee retreivedEmployee = new Employee();
                    retreivedEmployee = ctx.Employee.Where(u => u.Name == employee.Name && u.FTEPositionID == employee.FTEPositionID && u.OrganizationID == employee.OrganizationID).FirstOrDefault();

                    Employee duplicateUniqueIdentityNumber = new Employee();
                    duplicateUniqueIdentityNumber = ctx.Employee.Where(u => u.UniqueIdentityNumber == employee.UniqueIdentityNumber && u.OrganizationID == employee.OrganizationID).FirstOrDefault();

                    if (duplicateUniqueIdentityNumber != null)
                    {
                        result += employee.Name + "' failed to be created, duplicate unique identifier found for this organization.\n";
                    }
                    else if (retreivedEmployee == null)
                    {
                        ctx.Employee.Add(employee);
                        ctx.SaveChanges();
                        result += employee.Name + " has been created successfully.\n";
                    }
                    else
                    {
                        result += employee.Name + " failed to be created, it already exist in the same position and organization.\n";
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    //result = ex.Message;
                }
                finally
                {
                    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
                }
            }
            return result;
        }

        //From RegisterEmployeeController
        public static String update(Employee employee)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            String result = "";

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    Employee myEmp = new Employee();
                    myEmp = ctx.Employee.Where(e => e.ID.Equals(employee.ID)).FirstOrDefault();

                    Employee duplicateUniqueIdentityNumber = new Employee();
                    duplicateUniqueIdentityNumber = ctx.Employee.Where(u => u.UniqueIdentityNumber == employee.UniqueIdentityNumber && u.ID != employee.ID).FirstOrDefault();

                    if (myEmp.Equals(null))
                    {
                        result += "Employee ID: " + employee.ID + ", could not be found.\n";
                        //throw new Exception(result);
                    }
                    else if (duplicateUniqueIdentityNumber != null)
                    {
                        result += employee.Name + "' failed to be created, duplicate unique identifier found.\n";
                    }
                    else
                    {
                        //  ctx.Entry(myEmp).CurrentValues.SetValues(employee); Good way to update an object but it will overwrite the existing object
                        CopyUtil.CopyFields<Employee>(employee, myEmp);
                        ctx.SaveChanges();
                        result += employee.Name + " has been updated successfully.\n";
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    result = ex.Message;
                }
                finally
                {
                    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
                }
            }
            return result;
        }

        //From RegisterEmployeeController
        public static String delete(Employee employee)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            String result = "";
            bool isCaughtException = true;

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    Employee myEmp = new Employee();
                    myEmp = ctx.Employee.Where(e => e.ID.Equals(employee.ID)).FirstOrDefault();
                    if (myEmp.Equals(null))
                    {
                        result = employee.Name + " failed to be deleted, it does not exist.\n";
                        isCaughtException = false;
                        //throw new Exception(result);
                    }
                    else
                    {
                        ctx.Employee.Remove(myEmp);
                        ctx.SaveChanges();
                        result = employee.Name + " has been deleted successfully.\n";
                        isCaughtException = false;
                    }
                }
                catch (Exception ex)
                {
                    isCaughtException = true;
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
                    //result = ex.Message;
                }
                finally
                {
                    if (isCaughtException)
                    {
                        result += employee.Name + " failed to be deleted due to dependencies.\n";
                    }
                    Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
                }
            }
            return result;
        }

		public static String nextUniqueIdentityNumber()
		{
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
			String nextUniqueIdentityNumber = "";

			try
			{
				using (var ctx = new CPPDbContext())
				{
					nextUniqueIdentityNumber = ctx.Employee.Max(u => u.UniqueIdentityNumber);
				}

			}
			catch (Exception ex)
			{
				var stackTrace = new StackTrace(ex, true);
				var line = stackTrace.GetFrame(0).GetFileLineNumber();
				Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
			}
			finally
			{
			}
			Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

			return nextUniqueIdentityNumber;
		}

	}

    #region Two table join example class
    //public class Employee_FTEDEsc
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int ID { get; set; }
    //    public int FTEPositionID { get; set; }
    //    public String PositionDesc { get; set; }
    //    public String Name { get; set; }
    //    public Double HourlyRate { get; set; }
    //    public byte isActive { get; set; }
    //}
    #endregion

}