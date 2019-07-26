using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.DAO.Entities {
    public class MarcaDAO : BaseDAO<Marca> {

        public Marca Find(int id) {

            Marca marca = BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas WHERE MarcaId = {0}", id))
                                                .AsEnumerable()
                                                .Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome")))
                                                .FirstOrDefault();

            return marca;

        }

        public void GetAll() {
            throw new NotImplementedException();
        }

        public void Insert(Marca item) {
            throw new NotImplementedException();
        }

        public void Update(Marca item) {
            throw new NotImplementedException();
        }
    }
}