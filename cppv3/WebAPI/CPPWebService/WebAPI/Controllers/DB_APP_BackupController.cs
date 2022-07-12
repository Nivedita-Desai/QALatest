using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO.Compression;
using System.Configuration;

namespace WebAPI.Controllers
{
    public class DB_APP_BackupController : ApiController
    {
        [HttpGet]
        [Route("Request/Backup/")]
        public HttpResponseMessage Get()
        {
            //string constring = "server=localhost;user=root;pwd=123456;database=cppdev;Convert Zero Datetime=True;";
            string constring = ConfigurationManager.ConnectionStrings["CPP_MySQL"].ConnectionString.ToString();
            string dbBkpPath = ConfigurationManager.AppSettings["DB_BackupPath"].ToString();
            string date = DateTime.Now.ToString("MMddyyyy_HH-mm-ss");
            //date = date.Replace("/", "");
           
            if (!Directory.Exists(dbBkpPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(dbBkpPath);
            }
            else
            {
                if(!Directory.Exists(dbBkpPath + date))
                {
                    DirectoryInfo di1 = Directory.CreateDirectory(dbBkpPath + date);
                }
                
            }
            if (!Directory.Exists(dbBkpPath + date))
            {
                DirectoryInfo di1 = Directory.CreateDirectory(dbBkpPath + date);
            }
            string file = dbBkpPath + date + "\\backup" + date + ".sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
                ZipFile.CreateFromDirectory(dbBkpPath + date, dbBkpPath + date + ".zip");
                if (Directory.Exists(dbBkpPath + date))
                {
                    string result = EmptyFolder(new DirectoryInfo(dbBkpPath + date));
                    if(result=="Deleted")
                    {
                        Directory.Delete(dbBkpPath + date);
                    }
                }
            }

            //string FolderName = ConfigurationManager.AppSettings["FolderName"];
            //string DestinationPath = ConfigurationManager.AppSettings["DestinationPath"];
            //string SourcePath = ConfigurationManager.AppSettings["SourcePath"];
            //string[] filenames = Directory.GetFiles(SourcePath);
            string SourcePath = ConfigurationManager.AppSettings["App_SourcePath"].ToString();
            string DestinationPath = ConfigurationManager.AppSettings["App_BackupPath"].ToString();
            string[] folderName = SourcePath.Split('\\');


            ZipFile.CreateFromDirectory(SourcePath, DestinationPath + folderName[3].ToString() + date + ".zip");

            var jsonNew = new
            {
                result = "Success"
            };
            return Request.CreateResponse(HttpStatusCode.OK, jsonNew);
        }

        private string EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder);
            }

            return "Deleted";
        }
    }
}
