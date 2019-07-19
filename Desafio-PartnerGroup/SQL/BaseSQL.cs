using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Desafio_PartnerGroup.SQL {
    public class BaseSQL {

        public enum eSqlAuthType {
            SqlAutentication,
            WindowsAutentication
        }

        public static string DataSource;
        public static string InitialCatalog;

        public static string Server;

        public static string UserId;
        public static string Password;
        public static eSqlAuthType AuthType;

        public static string ConnectionString;
        public static SqlConnection Connection;
        public static string Database = "db-vgmm";

        public BaseSQL() {
        }

        public static void Connect(string connectionString) {

            Connection = new SqlConnection(connectionString);

            try {
                Connection.Open();
                Connection.Close();
            } catch (Exception ex){
                //if (ex.Message.Contains(Database)) {

                //    try {

                //        Connection = new SqlConnection(BuildConnectionString(false, connectionString));
                //        Connection.Open();
                //        Connection.Close();

                //        // CRIA DATABASE E TABELAS

                //        Execute(String.Format("CREATE DATABASE \"{0}\"", Database));
                //        Execute(String.Format(@"USE [{0}]; CREATE TABLE Marcas(
	               //                             MarcaId INT PRIMARY KEY NOT NULL,
	               //                             Nome VARCHAR(15) NOT NULL
                //                                );", Database));
                //        Execute(String.Format(@"USE [{0}]; CREATE TABLE Patrimonios(
	               //                             PatrimonioId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,  
	               //                             Nome VARCHAR(25) NOT NULL,
	               //                             MarcaId INT NOT NULL FOREIGN KEY REFERENCES Marcas(MarcaId),
	               //                             Descrição TEXT
                //                                );", Database));

                //        // TENTA RECONECTAR COM A DATABASE NO CONNECTION STRINGS

                //        Connect(ConnectionString);


                //    } catch (Exception ex2) {
                //        throw new Exception(ex2.Message);
                //    }

                //} else {

                throw new Exception(ex.Message);

                //}
            }

        }

        public void Connect() {
            Connection = new SqlConnection(BuildConnectionString(true, ConfigurationManager.ConnectionStrings["SQL"].ConnectionString));

            try {

                Connection.Open();
                Connection.Close();

            } catch (Exception ex) {

            }
        }

        private static string BuildConnectionString(bool dbConnection, string connectionString) {

            if (!dbConnection) {
                connectionString = Regex.Replace(connectionString, @"database(.*?);", "" , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                connectionString = Regex.Replace(connectionString, @"initialcatalog(.*?);", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            }


            //if (Server != null && Database != null) {
            //    ConnectionString += String.Format("Server={0}; ", Server);
            //    ConnectionString += String.Format("Database={0}; ", Database);
            //} else if (InitialCatalog != null && DataSource != null) {
            //    ConnectionString += String.Format("Data Source={0}; ", DataSource);
            //    ConnectionString += String.Format("Initial Catalog={0}; ", InitialCatalog);
            //} else {
            //    throw new Exception("Configuração de Dados SQL insuficiente!");
            //}

            //if (UserId != null && Password != null) {
            //    ConnectionString += String.Format("User Id={0}; ", UserId);
            //    ConnectionString += String.Format("Password={0}; ", Password);
            //} else if (InitialCatalog != null && DataSource != null) {
            //    ConnectionString += String.Format("Integrated Security=SSPI;");
            //} else if (Server != null && Database != null) {
            //    ConnectionString += String.Format("Trusted_Connection=True;");
            //} else {
            //    throw new Exception("Configuração de Dados SQL insuficiente!");
            //}

            return connectionString;
        }

        public static DataTable Execute(string query) {

            DataTable dt = new DataTable();
            Connection.Open();

                SqlCommand command = null;

            try {
                command = new SqlCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Connection = Connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                if (command != null)
                    command.Dispose();

                Connection.Close();

            }

            return dt;

        }
    }
}