using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace WebAPI.Controllers
{
    public class ConnectionManager
    {
        MySqlConnection connection;

        public static MySqlConnection getConnection()
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CPP_MySQL"].ConnectionString;
            return connection;
        }

       

    }
}