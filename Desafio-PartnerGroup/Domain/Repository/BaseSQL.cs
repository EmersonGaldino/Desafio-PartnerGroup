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

        public static SqlConnection Connection;

        public BaseSQL() {
        }

        public static void Connect(string connectionString) {

            Connection = new SqlConnection(connectionString);

            try {

                Connection.Open();
                Connection.Close();

            } catch (Exception ex){

                throw new Exception(ex.Message);
            }

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