using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Models {
    public class Marca {

        public int Id;
        public string Nome;

        public Marca() { }
        public Marca(int id, string nome) {
            this.Id = id;
            this.Nome = nome;
        }

    }
}