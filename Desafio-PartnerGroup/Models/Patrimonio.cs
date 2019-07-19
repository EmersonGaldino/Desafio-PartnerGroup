using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Models {
    public class Patrimonio {

        public int Id           { get; set; }
        public string Nome      { get; set; }
        public Marca Marca      { get; set; }
        public int MarcaId      { get; set; }
        public string Descricao { get; set; }

        public Patrimonio() { }
        public Patrimonio(string nome, Marca marca) {
            this.Nome = nome;
            this.Marca = marca;
        }
    }
}