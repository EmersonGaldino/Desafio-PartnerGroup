using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Repository {
    public class PatrimonioRepository : IRepository<Patrimonio> {

        public void Delete(int id) {
            BaseSQL.Execute(String.Format("DELETE FROM * WHERE PatrimonioId = {0}", id));
        }

        public List<Patrimonio> GetAll() {
            return BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                    FROM Patrimonios
                                                    INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId"))
                                                    .AsEnumerable()
                                                    .Select(s => new Patrimonio(s))
                                                    .ToList();
        }

        public Patrimonio GetById(int id) {
            return BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                    FROM Patrimonios
                                                    INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId
                                                    WHERE PatrimonioId = {0}", id))
                                                    .AsEnumerable()
                                                    .Select(s => new Patrimonio(s))
                                                    .FirstOrDefault();
        }

        public int Insert(Patrimonio item) {
            DataTable result = BaseSQL.Execute(String.Format("INSERT INTO Patrimonios VALUES('{0}',{1},{2}); SELECT SCOPE_IDENTITY();",
                                               item.Nome,
                                               item.Marca != null ? item.Marca.Id : item.MarcaId,
                                               item.Descricao != null ? String.Format("'{0}'", item.Descricao) : "null"));

            return Decimal.ToInt32((Decimal)result.Rows[0][0]);
        }

        public void Update(int id, Patrimonio item) {

            String query = "UPDATE Patrimonios SET ";
            query += !String.IsNullOrEmpty(item.Nome) ? String.Format("Nome = '{0}',", item.Nome) : String.Empty;
            query += item.MarcaId != 0 ? String.Format("MarcaId = {0},", item.Marca.Id) :
                     item.Marca != null ? String.Format("MarcaId = {0},", item.Marca.Id) : String.Empty;
            query += !String.IsNullOrEmpty(item.Descricao) ? String.Format("Descrição = '{0}',", item.Descricao) : String.Empty;

            if (query[query.Length - 1] == ',')
                query = query.Remove(query.Length - 1);

            query += String.Format(" WHERE PatrimonioId = {0}", id);

            BaseSQL.Execute(query);
        }
    }
}