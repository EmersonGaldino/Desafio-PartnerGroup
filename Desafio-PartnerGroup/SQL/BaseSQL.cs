using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.SQL {
    public class BaseSQL {

        public enum eSqlAuthType {
            SqlAutentication,
            WindowsAutentication
        }

        public string DataSource;
        public string InitialCatalog;

        public string Server;
        public string Database;

        public string UserId;
        public string Password;
        public eSqlAuthType AuthType;

        public static string ConnectionString;
        public static SqlConnection Connection;

        public BaseSQL() {
        }

        public void Connect() {

            Connection = new SqlConnection(BuildConnectionString());

            try {
                Connection.Open();
                Connection.Close();
            } catch (Exception ex){
                throw new Exception(ex.Message);
            }

        }

        public string BuildConnectionString() {
            ConnectionString = String.Empty;

            if (Server != null && Database != null) {
                ConnectionString += String.Format("Server={0}; ", Server);
                ConnectionString += String.Format("Database={0}; ", Database);
            } else if (InitialCatalog != null && DataSource != null) {
                ConnectionString += String.Format("Data Source={0}; ", DataSource);
                ConnectionString += String.Format("Initial Catalog={0}; ", InitialCatalog);
            } else {
                throw new Exception("Configuração de Dados SQL insuficiente!");
            }

            if (UserId != null && Password != null) {
                ConnectionString += String.Format("User Id={0}; ", UserId);
                ConnectionString += String.Format("Password={0}; ", Password);
            } else if (InitialCatalog != null && DataSource != null) {
                ConnectionString += String.Format("Integrated Security=SSPI;");
            } else if (Server != null && Database != null) {
                ConnectionString += String.Format("Trusted_Connection=True;");
            } else {
                throw new Exception("Configuração de Dados SQL insuficiente!");
            }

            Console.WriteLine(ConnectionString);
            return ConnectionString;
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