using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Desafio_PartnerGroup.SQL;

namespace Desafio_PartnerGroup {
    public class SqlConfig {

        public static void Start() {

            new BaseSQL(){
                Server = "DESKTOP-NNOAUCQ\\SQLEXPRESS",
                Database = "db-vgmm",
            }.Connect();

        }
    }
}