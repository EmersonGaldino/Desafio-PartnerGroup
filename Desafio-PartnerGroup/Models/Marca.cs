using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Models {
    public class Marca : Entity {

        public string Nome;

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