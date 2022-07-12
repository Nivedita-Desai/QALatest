using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using WebAPI.Controllers;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using WebAPI.Helper;

namespace WebAPI.Models
{ 
    [Table("user")]
    public class User
    {
        [NotMapped]
        public int Operation;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String FullName { get; set; }
        
        public String Role { get; set; }
        [NotMapped]
        public string[] RoleList { get; set; }

        [NotMapped]
        public String AccessControlList { get; set; }
        [NotMapped]
        public String routeInfo { get; set; }
        public String UserID { get; set; }
        public String LoginPassword { get; set; }
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        [NotMapped]
        public String Threshold { get; set; }

        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }

        public Boolean PasswordChangeRequired { get; set; }

        //ON DELETE RESTRICT
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [NotMapped]
        public List<UserRoleRelation> lstUserRoleRelation { get; set; }

        [NotMapped]
        public List<UserRole> lstUserRole { get; set; }

        
        public User(String auth, String fName, String mName, String lName, String email, String userid, int id, String loginPassword, int employeeid, int departmentid)
        {
            Role = auth;
            FirstName = fName;
            MiddleName = mName;
            LastName = lName;
            Email = email;
            UserID = userid;
            EmployeeID = employeeid;
            DepartmentID = departmentid;
            Id = id;
            LoginPassword = loginPassword;
        }

       public User()
        {
            //Role = "Not Authorized";
            Role = "";
            FirstName = "N/A";
            MiddleName = "N/A";
            LastName = "N/A";
            Email = "N/A";
            UserID = "N/A";
            EmployeeID = 0;
            DepartmentID = 0;
            Id = 0;
        }

        public virtual RandomToken RandomToken { get; set; }

        public static User authenticateLogin(String UserID, String LoginPassword)
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            User LoggedInUser = new User();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            var username = UserID.ToLower();
            var isFound = false;
            try
            {
                //Salt and hash password
                var usrPassword = SimpleHash.ComputeHash(LoginPassword, "MD5", null);

                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "SELECT u.FullName, u.Role, u.UserID, u.LoginPassword, a.Cost, u.EmployeeID FROM user u left outer join approval_matrix  a on u.Role = a.Role ";
                query += " WHERE 1=1";
                query += " AND u.UserID = '" + username + "'";
                //   query += " AND u.LoginPassword = '" + exPectedPassword + "'";
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //LoggedInUser = new User(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim());
                            var dbPass = reader.GetValue(3).ToString().Trim();
                            isFound = SimpleHash.VerifyHash(LoginPassword, "MD5", dbPass);
                            if (isFound == true)
                            {
                                LoggedInUser.FullName = reader.GetValue(0).ToString().Trim();
                                LoggedInUser.Role = reader.GetValue(1).ToString().Trim();
                                LoggedInUser.UserID = reader.GetValue(2).ToString().Trim();
                                LoggedInUser.Threshold = reader.GetValue(4).ToString().Trim();
                                LoggedInUser.EmployeeID = Convert.ToInt16(reader.GetValue(5).ToString().Trim());
                            }
                            else
                            {
                                isFound = false;
                            }

                        }
                    }
                    else
                        return LoggedInUser;
                }
                if (isFound == true)
                {
                    //Get ACL Bit Vector
                    query = "SELECT * FROM user_roles";
                    query += " WHERE Role = '" + LoggedInUser.Role + "'";
                    command = new MySqlCommand(query, conn);
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Object[] values = new Object[reader.FieldCount];
                            int fieldCount = reader.GetValues(values);
                            LoggedInUser.AccessControlList = null;
                            for (int i = 1; i < fieldCount; i++)
                                LoggedInUser.AccessControlList += reader.GetValue(i).ToString().Trim();

                        }
                    }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return LoggedInUser;
            */
            #endregion

            #region New Era
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String hashPassword = "";
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            User user = new User();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    user = ctx.User.Where(u => u.UserID == UserID).FirstOrDefault();    //Legendary bug. First login will step over to nowhere. Second login will work. No exact method for replicating it consistently.
                                                                                        //No, it must be something else. I think the first login click will not go to this breakpoint at all?
                    //------------Manasi 14-08-2020------------------
                    if (user == null)
                    {
                        User invalidUser = new User();
                        invalidUser.Role = "Not Authorized";
                        return invalidUser;
                    }
                    //-----------------------------------------------

                    if (user.UserID != UserID) {
						User invalidUser = new User();
						invalidUser.Role = "Not Authorized";
						return invalidUser;
					}

                    ApprovalMatrix approvalMatrix = ctx.ApprovalMatrix.Where(u => u.Role == user.Role).FirstOrDefault();

                    if (user != null)
                    {

                        if (!(LoginPassword == null || LoginPassword == ""))
                        {
                            if (SimpleHash.VerifyHash(LoginPassword, "MD5", user.LoginPassword))
                            {
                                //Get ACL Bit Vector
                                conn = ConnectionManager.getConnection();
                                conn.Open();

                                user.Threshold = "0";

                                if (approvalMatrix != null)
                                {
                                    user.Threshold = approvalMatrix.Cost.ToString();
                                }

                                //Nivedita changes in rolemenurelation

                                List<UserRoleRelation> userRoleRelation = ctx.UserRoleRelation.Where(u => u.UserId == user.Id).ToList();

                                //String RoleList = string.Join(",", userRoleRelation.Select(a => a.UserRoleId.ToString()).ToArray());
                                List<string> RoleList = new List<string>();
                                String query1 = "select Role from user_roles where Id in ( select UserRoleId from user_role_relation";
                                query1 += " WHERE UserId = @UserId)";
                                MySqlCommand command1 = new MySqlCommand(query1, conn);
                                command1.Parameters.AddWithValue("@UserId", user.Id);
                                user.Role = null;
                                using (reader = command1.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        RoleList.Add(reader.GetValue(0).ToString().Trim());
                                        //user.Role += string.Join(",",reader.GetValue(0).ToString().Trim());
                                        //user.Role += reader.GetValue(0).ToString().Trim() + ',';
                                    }
                                    
                                }
                                user.Role = string.Join(",", RoleList);
                                String query = "get_AccessControlList";
                                MySqlCommand command = new MySqlCommand(query, conn);
                                command.CommandType = CommandType.StoredProcedure;
                                //command.Parameters.AddWithValue("@Role", user.Role);
                                command.Parameters.AddWithValue("_userId", user.Id);
                                using (reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Object[] values = new Object[reader.FieldCount];
                                        int fieldCount = reader.GetValues(values);
                                        user.AccessControlList = null;
                                        for (int i = 0; i < fieldCount; i++)
                                            user.AccessControlList += reader.GetValue(i).ToString().Trim();

                                    }
                                }

                                return user;
                            }
                            else
                            {
                                User invalidUser = new User();
                                invalidUser.Role = "Not Authorized";
                                return invalidUser;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.InnerException.ToString(), line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return user;
            #endregion New Era
        }

        //From RequestUserController
        public static List<User> getUser()
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<User> UserList = new List<User>();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                //String query = "SELECT FullName, Role, UserID FROM user";
                //query += " WHERE 1=1";
                //if (UserID != "null")
                //    query += " AND UserID = '" + UserID + "'";
                //if (FirstName != "null")
                //    query += " AND FirstName like '%" + FirstName + "%'";
                //if (LastName != "null")
                //    query += " AND LastName like '%" + LastName + "%'";
                //if (Role != "null")
                //    query += " AND Role = '" + Role + "'";
                String query = "SELECT u.Role, u.FirstName, u.MiddleName, u.LastName, u.Email, u.UserID, u.Id,a.Cost, u.EmployeeID, '' as loginPassword "
                    +" FROM user u left outer join approval_matrix  a on u.Role = a.Role"
                    + " order by u.Role";
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //User returnedUser = new User();
                        //returnedUser.FullName = reader.GetValue(0).ToString().Trim();
                        //returnedUser.Role = reader.GetValue(1).ToString().Trim();
                        //UserList.Add(returnedUser);

                        var retreivedEmployeeID = 0;
                        if (reader.GetValue(8).ToString() != "" || reader.GetValue(8).ToString() != "null")
                        {
                            retreivedEmployeeID = Convert.ToInt16(reader.GetValue(8).ToString().Trim());
                        } else
                        {
                            retreivedEmployeeID = 10000;
                        }

                        var ctx = new CPPDbContext();

                        List<Employee> employeeList = ctx.Employee.Where(e => e.ID == retreivedEmployeeID).ToList();

                        User RetreivedUser = new User(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(),
                            reader.GetValue(2).ToString().Trim(), reader.GetValue(3).ToString().Trim(),
                            reader.GetValue(4).ToString().Trim(), reader.GetValue(5).ToString().Trim(),
                            Convert.ToInt16(reader.GetValue(6).ToString().Trim()), reader.GetValue(9).ToString().Trim(), retreivedEmployeeID);

                        RetreivedUser.Threshold = reader.GetValue(7).ToString().Trim();
                        UserList.Add(RetreivedUser);
                    }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return UserList;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            List<User> userList = new List<User>();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    userList = ctx.User.OrderBy(a => a.Id).ToList();

                    for(int x = 0; x < userList.Count; x++)
                    {
                        userList[x].LoginPassword = "";
                        userList[x].lstUserRole = WebAPI.Models.UserRole.getRole(null);

                        int id = userList[x].Id;
                    userList[x].lstUserRoleRelation = ctx.UserRoleRelation.Where(u => u.UserId == id).ToList();
                        List<string> lstRole = new List<string>();
                        for (int i = 0; i < userList[x].lstUserRole.Count; i++)
                        {
                            for (int j = 0; j < userList[x].lstUserRoleRelation.Count; j++)
                            {
                                if (userList[x].lstUserRoleRelation[j].UserRoleId == userList[x].lstUserRole[i].Id)
                                {
                                    userList[x].lstUserRole[i].isSelected = true;
                                    lstRole.Add(userList[x].lstUserRole[i].Role);
                                    break;
                                }
                                else
                                    userList[x].lstUserRole[i].isSelected = false;
                            }
                            

                        }
                        userList[x].RoleList = lstRole.ToArray();

                    }
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

            return userList;
        }

        public static List<User> getUserListOfApproval(String EmployeeListID)
        {
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            List<User> UserList = new List<User>();
            List<User> AllUserList = new List<User>();
            AllUserList = getUser();
            try
            {
                var ctx = new CPPDbContext();

                if(EmployeeListID == "0")
                {
                    EmployeeListID = "";
                }

                String[] employeeListAr = EmployeeListID.Split(null);

                for (int x = 0; x < employeeListAr.Length; x++)
                {
                    for (int y = 0; y < AllUserList.Count; y++)
                    {
                        if (Convert.ToInt16(employeeListAr[x]) == AllUserList[y].EmployeeID)
                        {
                            UserList.Add(AllUserList[y]);
                            break;
                        }
                    }
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
            return UserList;
        }

        //From RegisterUserController
        public static String registerUser(User user)
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String register_result = "";
            var password = SimpleHash.ComputeHash(LoginPassword, "MD5", null);
            var lowerCaseUsername = UserID.ToLower();
            bool OKForRegister = false;

            if (EmployeeID == 0)
            {
                //If there is no employee -> choose TBD
                using (var ctx = new CPPDbContext())
                {
                    Employee emp = ctx.Employee.Where(a => a.Name == "TBD").FirstOrDefault();
                    if (emp != null)
                        EmployeeID = emp.ID;
                }
            }
           
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();
                String DOB = "";
                string defaultRole = "User";
                if (Role != "")
                {
                    defaultRole = Role;
                }
                //Check if user already exists in system
                String query = "SELECT FullName, UserID, LoginPassword FROM user";
                query += " WHERE 1=1";
                query += " AND UserID = '" + UserID + "'";
                //query += " OR (FullName = '" + FullName + "' AND LastName = '" + LastName + "' AND MiddleName = '" + MiddleName + "')";
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(1).ToString().Trim() == UserID)
                            register_result += UserID + " failed to be created, it already exist";
                        //else if (reader.GetValue(0).ToString().Trim() == FirstName && reader.GetValue(0).ToString().Trim() == FirstName)
                        //    register_result += "User " + FirstName + " " + MiddleName + " " + LastName + " already exists in system";

                    }
                    else
                        OKForRegister = true;
                }

                //User does not already exist in system. Insert new user row in 'user' table
                if (OKForRegister)
                {
                    //write to DB
                    query = "INSERT INTO user ( FullName, UserID, FirstName, MiddleName, LastName, LoginPassword, Role, DOB, Email, EmployeeID) VALUES";
                    query += " (";
                    query += "'" + FullName + "', ";
                    query += "'" + lowerCaseUsername + "', ";
                    query += "'" + FirstName + "', ";
                    query += "'" + MiddleName + "', ";
                    query += "'" + LastName + "', ";

                    query += "'" + password + "', ";
                    query += "'" + defaultRole + "', ";
                    query += "'" + DOB + "', ";
                    query += "'" + Email + "'";
                     query += "," + EmployeeID + "";
                    query += ")";
                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    register_result = UserID + " has been created successfully.\n";
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return register_result;
            */
            #endregion

            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            var password = SimpleHash.ComputeHash(user.LoginPassword, "MD5", null);
            var lowerCaseUsername = user.UserID.ToLower();
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User retreivedUser = new User();
                    retreivedUser = ctx.User.Where(u => u.UserID == user.UserID).FirstOrDefault();

                    if (retreivedUser == null)
                    {
                        //register
                        //set encryption before saving
                        user.LoginPassword = password;
                        user.UserID = lowerCaseUsername;
                        user.PasswordChangeRequired = true;    //require password change!

                        ctx.User.Add(user);
                        ctx.SaveChanges();

                        //List<String> RoleList = new List<string>(user.Role.Split(','));
                        List<UserRole> selectedUserRole = new List<UserRole>();
                        selectedUserRole = user.lstUserRole.Where(a => a.isSelected == true).ToList();
                        //foreach (var item in user.RoleList)
                        //{
                        //    selectedUserRole.Add(ctx.Where(a => item.Contains(a.Role)));
                        //}

                        for (int i = 0; i < selectedUserRole.Count; i++)
                        {
                            // user.lstUserRole = WebAPI.Models.UserRole.getRole(RoleList[i]);
                            //if (user.lstUserRole[i].isSelected == true)
                            //{
                                UserRoleRelation userRoleRelation = new UserRoleRelation();
                                userRoleRelation.UserId = user.Id;
                                userRoleRelation.UserRoleId = selectedUserRole[i].Id;
                                ctx.UserRoleRelation.Add(userRoleRelation);
                                ctx.SaveChanges();
                            //}
                            
                        }
                        
                        
                        result += user.UserID + " has been created successfully.\n";
                    }
                    else
                    {
                        result += user.UserID + " failed to be created, it already exist.\n";
                    }
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

            return result;
        }

        public static String updateUser(User user)
        {
            #region Legacy
            /*
            //public static String updateUser(int Id, String FullName, String UserID, String FirstName, String MiddleName, String LastName, String Email, String Role, String loginPassword, int EmployeeID,Boolean isUpdateFromEmail)
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            String update_result = "";
            var lowerCaseUserName = UserID.ToLower();
            bool OKForUpdate = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();
                String DOB = "";
                //Check if user exists in system
                String query = "SELECT Id FROM user";
                query += " WHERE 1=1";
                query += " AND Id = " + Id;
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (Convert.ToInt16(reader.GetValue(0).ToString().Trim()) == Id)
                            OKForUpdate = true;

                    }
                    else
                        update_result += UserID + " failed to be updated, it does not exist.\n";
                }

                //Update the Program
                if (OKForUpdate)
                {
                    //write to DB
                     var hashPassword="";
                    if (!(loginPassword == null || loginPassword == ""))
                           hashPassword = SimpleHash.ComputeHash(loginPassword, "MD5", null);
                    query = "UPDATE user SET";

                    query += " DOB = '"        + DOB + "',";
                    query += " UserID = '"     + UserID + "',";
                    query += " FirstName = '"  + FirstName + "',";
                    query += " MiddleName = '" + MiddleName + "',";
                    query += " LastName = '"   + LastName + "',";
                    query += " Role = '"       + Role + "',";
                    query += " FullName = '"   + FullName + "',";
                    query += " Email = '"      + Email + "'";
                    if(EmployeeID != 0)
                         query += ", EmployeeID = " + EmployeeID + "";
                    if (!(loginPassword == null || loginPassword == ""))
                        query += ", LoginPassword = '" + hashPassword + "'";
                    query += " WHERE";
                    query += " Id = " + Id + "";

                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    update_result = UserID + " has been updated successfully.\n";

                    if(isUpdateFromEmail)
                    {
                        using (var ctx = new CPPDbContext())
                        {
                            RandomToken token = ctx.RandomToken.Where(a => a.UserID == Id).FirstOrDefault();
                            if (token != null)
                                token.Active = 0;

                            ctx.Entry<RandomToken>(token).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                        }
                    }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return update_result;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String hashPassword = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User retreivedUser = new User();
                    retreivedUser = ctx.User.Where(u => u.Id == user.Id).FirstOrDefault();

                    int employeeID = retreivedUser.EmployeeID;

                    User duplicateUser = new User();
                    duplicateUser = ctx.User.Where(u => u.UserID == user.UserID
                                                        && u.Id != user.Id).FirstOrDefault();

                    if (duplicateUser != null)
                    {
                        result = user.UserID + " failed to be updated, duplicate will be created.\n";
                    }
                    else if (retreivedUser != null)
                    {
                        //Deal with not saving password when given empty or null
                        String tempGivenPassword = user.LoginPassword;
                        user.LoginPassword = retreivedUser.LoginPassword;
                        //user.EmployeeID = retreivedUser.EmployeeID;      //Manasi
                        CopyUtil.CopyFields<User>(user, retreivedUser);

                        var newRoleIds = new List<int> { };
                        if (user.lstUserRole != null)
                        {
                            for (int i = 0; i < user.lstUserRole.Count; i++)
                            {
                                if (user.lstUserRole[i].isSelected == true)
                                {
                                    newRoleIds.Add(user.lstUserRole[i].Id);
                                }

                            }
                        }
                        

                        //var struser = ctx.User.Include("UserRoleRelation").Single(u => u.Id == user.Id);
                        List<UserRoleRelation> userRoleRelation = new List<UserRoleRelation>();
                        userRoleRelation = ctx.UserRoleRelation.Where(u => u.UserId == user.Id).ToList();


                        //retreiveduserRoleRelation = ctx.UserRoleRelation.Where(r => newRoleIds.Contains(r.UserRoleId) && r.UserId == user.Id).ToList();
                        if (newRoleIds.Count > 0)
                        {
                            foreach (var item in userRoleRelation)
                            {
                                ctx.UserRoleRelation.Remove(item);
                            }
                            ctx.SaveChanges();
                        }
                        

                        
                        
                        foreach (var item in newRoleIds)
                        {
                            UserRoleRelation retreiveduserRoleRelation = new UserRoleRelation();
                            retreiveduserRoleRelation.UserId = user.Id;
                            retreiveduserRoleRelation.UserRoleId = item;
                            ctx.UserRoleRelation.Add(retreiveduserRoleRelation);
                            ctx.SaveChanges();
                        }
                        
                        
                        
                        if (!(tempGivenPassword == null || tempGivenPassword == ""))
                        {
                            hashPassword = SimpleHash.ComputeHash(tempGivenPassword, "MD5", null);
                            retreivedUser.LoginPassword = hashPassword;
                            retreivedUser.EmployeeID = employeeID;
                        }

                        ctx.Entry(retreivedUser).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += user.UserID + " has been updated successfully.\n";
                    }
                    else
                    {
                        result += user.UserID + " failed to be updated, it does not exist.\n";
                    }
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

            return result;
        }

        public static String deleteUser(User user)
        {
            #region Legacy
            /*
            //public static String deleteUser(int Id, String UserID)
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            String delete_result = "";
            bool OKForDelete = false;
            bool isCaughtException = true;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                //Check if program exists in system
                String query = "SELECT UserID from user";
                query += " WHERE 1=1";
                query += " AND UserID = '" + UserID + "'";
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim() == UserID)
                            OKForDelete = true;

                    }
                    else
                    {
                        isCaughtException = false;
                        delete_result += UserID + " failed to be deleted, it does not exist.\n";
                    }
                }

                //Delete the Program
                if (OKForDelete)
                {

                    //Delete random_token
                    User u = null;
                    var randomTokenUserID = 0;
                    using(var ctx = new CPPDbContext())
                    {
                        u = ctx.User.Include("RandomToken").Where(a => a.UserID == UserID).FirstOrDefault();
                        if (u != null && u.RandomToken != null && u.RandomToken.UserID != 0)
                        {
                            randomTokenUserID = u.RandomToken.UserID;
                            query = "DELETE FROM random_token where UserID =  '" + randomTokenUserID + "'";
                            command = new MySqlCommand(query, conn);
                            command.ExecuteNonQuery();
                        }

                    }
               

                    //write to DB
                    query = "DELETE FROM user";
                    query += " WHERE";
                    query += " UserID = '" + UserID + "'";
                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    isCaughtException = false;
                    delete_result = UserID + " has been deleted successfully.\n";
                }

            }
            catch (Exception ex)
            {
                if(isCaughtException)
                {
                    delete_result = UserID + " failed to be deleted due to dependencies.\n";
                }
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);



            return delete_result;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User retreivedUser = new User();
                    retreivedUser = ctx.User.Where(u => u.Id == user.Id).FirstOrDefault();
                    if (retreivedUser != null)
                    {
                        ctx.User.Remove(retreivedUser);
                        ctx.SaveChanges();
                        
                        List<UserRoleRelation> userRoleRelation = new List<UserRoleRelation>();
                        userRoleRelation = ctx.UserRoleRelation.Where(u => u.UserId == retreivedUser.Id).ToList();
                        foreach (var item in userRoleRelation)
                        {
                            ctx.UserRoleRelation.Remove(item);
                            ctx.SaveChanges();
                        }
                        
                            
                        result = user.UserID + " has been deleted successfully.\n";
                    }
                    else
                    {
                        result = user.UserID + " failed to be deleted, it does not exist.\n";
                    }
                }

            }
            catch (Exception ex)
            {
                result = user.UserID + " failed to be deleted due to dependencies.\n";
                var stackTrace = new StackTrace(ex, true);
                var line = stackTrace.GetFrame(0).GetFileLineNumber();
                Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);
            }
            finally
            {
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);

            return result;
        }

        public static String forgotPassword(User user)
        {
            var status = "";
            var userFound = false;
            var token = "";
            var userName = user.UserID.ToLower().Trim();
            var email = user.Email.Trim();

            using (var ctx = new CPPDbContext())
            {
                try
                {
                    User retrievedUser = ctx.User.Where(u => u.UserID == userName && u.Email == email).FirstOrDefault();
                    if (retrievedUser != null)
                    {
                        userFound = true;
                        retrievedUser.routeInfo = user.routeInfo;
                        user = retrievedUser;
                    }
                    else
                    {
                        status = "The information you've entered does not match our record. Make sure that you enter the username and email correctly";
                    }

                    if (userFound)
                    {
                        //Generate a random Token
                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                        {

                            Byte[] bytes = new Byte[64];
                            rng.GetBytes(bytes);

                            token = Convert.ToBase64String(bytes);
                            token = token.Replace("=", "");
                            token = token.Replace("+", "");
                            //register token in database
                            RandomToken rt = new RandomToken();
                            RandomToken retrievedToken = ctx.RandomToken.Where(r => r.UserID == retrievedUser.Id && r.Active == 1).FirstOrDefault();

                            if (retrievedToken == null)
                            {
                                //register
                                using (var dbCtx = new CPPDbContext())
                                {
                                    rt.UserID = retrievedUser.Id;
                                    rt.Token = token;
                                    rt.DateCreate = DateTime.Today;
                                    rt.ExpiredDate = DateTime.Today.AddDays(1);
                                    rt.Active = 1;
                                    dbCtx.RandomToken.Add(rt);
                                    dbCtx.SaveChanges();

                                }

                            }
                            else
                            {
                                //update
                                retrievedToken.DateCreate = DateTime.Today;
                                retrievedToken.ExpiredDate = DateTime.Today.AddDays(1);
                                retrievedToken.Token = token;
                                retrievedToken.Active = 1;
                                ctx.Entry(retrievedToken).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();

                            }


                            //send mail
                            WebAPI.Services.MailServices.SendForgotPassword(user, token);
                            status = "The instructions to reset your password has been sent to your email. Please check your email.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    var stackTrace = new StackTrace(ex, true);
                    var line = stackTrace.GetFrame(0).GetFileLineNumber();
                    Logger.LogExceptions(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex.Message, line.ToString(), Logger.logLevel.Exception);

                }
            }

            //return token;
            return status;
        }

        public static String changePasswordById(User user)
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            String update_result = "";
            bool OKForUpdate = false;
            try
            {
                // create and open a connection object
                conn = ConnectionManager.getConnection();
                conn.Open();

                // check if user exists in system
                String query = "SELECT Id FROM user";
                query += " WHERE 1=1";
                query += " AND Id = " + user.Id;
                MySqlCommand command = new MySqlCommand(query, conn);
                using (reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (Convert.ToInt16(reader.GetValue(0).ToString().Trim()) == user.Id)
                            OKForUpdate = true;

                    }
                    else
                        update_result += "User does not in system";
                }

                // update db
                if (OKForUpdate)
                {
                    // change password
                    var hashPassword = SimpleHash.ComputeHash(user.LoginPassword, "MD5", null);
                    query = "UPDATE user SET";
                    query += " LoginPassword = '" + hashPassword + "'";
                    query += " WHERE";
                    query += " Id = " + user.Id;
                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    // change token to inactive
                    RandomToken token = RandomToken.getTokenByUserID(user.Id);
                    query = "UPDATE random_token SET";
                    query += " Active = 0";
                    query += " WHERE";
                    query += " ID = " + token.ID;
                    command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();

                    update_result = "Success";
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            
            return update_result;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            String result = "";
            String hashPassword = "";

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    User retreivedUser = new User();
                    retreivedUser = ctx.User.Where(u => u.Id == user.Id).FirstOrDefault();

                    if (retreivedUser != null)
                    {
                        CopyUtil.CopyFields<User>(user, retreivedUser);


                        if (!(user.LoginPassword == null || user.LoginPassword == ""))
                        {
                            hashPassword = SimpleHash.ComputeHash(user.LoginPassword, "MD5", null);
                        }

                        retreivedUser.LoginPassword = hashPassword;

                        // change token to inactive
                        RandomToken token = RandomToken.getTokenByUserID(user.Id);
                        token.Active = 0;
                        ctx.Entry(token).State = System.Data.Entity.EntityState.Modified;

                        ctx.Entry(retreivedUser).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        result += user.UserID + " password has been updated successfully.\n";
                    }
                    else
                    {
                        result += user.UserID + " failed to be updated, it does not exist.\n";
                    }
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

            return result;
        }

        public static User getUserById(int Id)
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            User user = new User();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "SELECT u.FirstName, u.MiddleName, u.LastName, u.FullName, u.Id, u.Role, u.EmployeeID FROM user u WHERE u.Id = " + Id;
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.FirstName = reader.GetValue(0).ToString().Trim();
                        user.MiddleName = reader.GetValue(1).ToString().Trim();
                        user.LastName = reader.GetValue(2).ToString().Trim();
                        user.FullName = reader.GetValue(3).ToString().Trim();
                        user.Id = Convert.ToInt16(reader.GetValue(4).ToString().Trim());
                        user.Role = reader.GetValue(5).ToString().Trim();
                        
                        int temp = 0;
                        int.TryParse(reader.GetValue(6).ToString().Trim(), out temp);

                        user.EmployeeID = temp;
                    }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return user;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            User user = new User();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    user = ctx.User.Where(u => u.Id == Id).FirstOrDefault();
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

            return user;
        }

        public static User getUserByUserID(string userID)
        {
            #region Legacy
            /*
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);
            User user = new User();
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            try
            {
                conn = ConnectionManager.getConnection();
                conn.Open();
                String query = "SELECT u.FirstName, u.MiddleName, u.LastName, u.FullName, u.Id, u.Role, u.EmployeeID FROM user u WHERE u.userID = '" + userID + "'";
                MySqlCommand command = new MySqlCommand(query, conn);

                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.FirstName = reader.GetValue(0).ToString().Trim();
                        user.MiddleName = reader.GetValue(1).ToString().Trim();
                        user.LastName = reader.GetValue(2).ToString().Trim();
                        user.FullName = reader.GetValue(3).ToString().Trim();
                        user.Id = Convert.ToInt16(reader.GetValue(4).ToString().Trim());
                        user.Role = reader.GetValue(5).ToString().Trim();

                        int temp = 0;
                        int.TryParse(reader.GetValue(6).ToString().Trim(), out temp);

                        user.EmployeeID = temp;
                    }
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
                if (conn != null)
                {
                    conn.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Exit Point", Logger.logLevel.Debug);
            return user;
            */
            #endregion
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "Entry Point", Logger.logLevel.Info);
            Logger.LogDebug(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, "", Logger.logLevel.Debug);

            User user = new User();

            try
            {
                using (var ctx = new CPPDbContext())
                {
                    user = ctx.User.Where(u => u.UserID == userID).FirstOrDefault();
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

            return user;
        }
    }




    public class SimpleHash
    {
        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText,
                                      string hashAlgorithm,
                                      string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }
    }
}