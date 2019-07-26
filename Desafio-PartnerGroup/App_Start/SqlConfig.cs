using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Desafio_PartnerGroup.SQL;

namespace Desafio_PartnerGroup {
    public class SqlConfig {

        public static void Start() {

            BaseSQL.Connect(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        }
    }
}