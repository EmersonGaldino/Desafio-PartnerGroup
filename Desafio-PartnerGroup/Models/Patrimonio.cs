using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Models {

    public class Patrimonio {

        public int Id           { get; set; }
        public string Nome      { get; set; }
        public Marca Marca      { get; set; }
        public int MarcaId      { get; set; }
        public string Descricao { get; set; }

        public Patrimonio() {
        }

        public Patrimonio(DataRow row) {

            Id = row.Field<int>("PatrimonioId");
            Nome = row.Field<string>("Nome");
            Marca = new Marca(row.Field<int>("MarcaId"), row.Field<string>("Marca"));
            Descricao = String.IsNullOrEmpty(row.Field<string>("Descrição")) ? String.Empty : row.Field<string>("Descrição");

        }

    }
}