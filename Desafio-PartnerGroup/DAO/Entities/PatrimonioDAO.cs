using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.DAO.Entities {
    public class PatrimonioDAO : BaseDAO<Patrimonio> {
        public override Patrimonio Find(int id) {

            return BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId
                                                                WHERE PatrimonioId = {0}", id))
                                                                .AsEnumerable()
                                                                .Select(s => new Patrimonio(s.Field<int>("PatrimonioId"), s.Field<string>("Marca") {
                                                                    Id = 
                                                                }),
                                                                    Descricao = s.Field<string>("Descrição")
                                                                }).ToArray();

            return patJson ;

        }
    
        public override void GetAll() {
            throw new NotImplementedException();
        }

        public override void Insert(Patrimonio item) {
            throw new NotImplementedException();
        }

        public override void Update(Patrimonio item) {
            throw new NotImplementedException();
        }
    }
}