using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Models {
    public class Marca {

        public int Id       { get; set; }
        public string Nome  { get; set; }

        public Marca() {
        }

        public Marca(int id, string nome) {
            this.Id = id;
            this.Nome = nome;
        }

        public Marca(DataRow row) {
            this.Id = row.Field<int>("MarcaId");
            this.Nome = row.Field<string>("Nome");
        }

    }
}