using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Desafio_PartnerGroup {
    public class MarcaRepository : IRepository<Marca> {

        public Marca GetById(int id) {
            
            return BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas WHERE MarcaId = {0}", id))
                                                .AsEnumerable()
                                                .Select(s => new Marca(s))
                                                .FirstOrDefault();

        }

        public void Delete(int id) {

            BaseSQL.Execute(String.Format("DELETE FROM * WHERE MarcaId = {0}", id));

        }

        public void Update(int id, Marca item) {

            string query = "UPDATE Marcas SET ";
            query += item.Id != 0 ? String.Format("MarcaId = {0},", item.Id) : String.Empty;
            query += !String.IsNullOrEmpty(item.Nome) ? String.Format("Nome = '{0}'", item.Nome) : String.Empty;

            if (query[query.Length - 1] == ',')
                query = query.Remove(query.Length - 1);

            query += String.Format(" WHERE MarcaId = {0}", id);


            BaseSQL.Execute(query);
            
        }

        public List<Patrimonio> GetAllPatrimoniosByMarca(int id) {
            return BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId
                                                                WHERE Patrimonios.MarcaId = {0}", id))
                                                                .AsEnumerable()
                                                                .Select(s => new Patrimonio(s))
                                                                .ToList();
        }

        public Marca GetByItem(Marca item) {

            string query = @"SELECT * FROM Marcas WHERE ";
            if (item.Id != 0 && !String.IsNullOrEmpty(item.Nome)) {
                query += String.Format("MarcaId = {0} AND Nome = '{1}'", item.Id, item.Nome);
            } else if (item.Id != 0) {
                query += String.Format("MarcaId = {0}", item.Id);
            } else {
                query += String.Format("Nome = {0}", item.Nome);
            }

            return BaseSQL.Execute(query).AsEnumerable()
                                        .Select(s => new Marca(s))
                                        .FirstOrDefault();
        }

        public int Insert(Marca item) {

           BaseSQL.Execute(String.Format("INSERT INTO Marcas VALUES({0},'{1}')",
                                            item.Id,
                                            item.Nome));

            return item.Id;

        }

        public List<Marca> Exist(Marca item) {

            return BaseSQL.Execute(String.Format(@"SELECT MarcaId, Nome
                            FROM Marcas
                            WHERE MarcaId = {0} OR Nome = '{1}'", item.Id, item.Nome))
                            .AsEnumerable()
                            .Select(s => new Marca(s))
                            .ToList();

        }

        public List<Marca> Exist(int id, Marca item) {
            return BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas WHERE MarcaId = {0} OR MarcaId = {1} OR Nome = '{2}'", id, item.Id, item.Nome))
                   .AsEnumerable()
                   .Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome")))
                   .ToList();
        }

        public List<Marca> GetAll() {

            return BaseSQL.Execute("SELECT * FROM Marcas")
                .AsEnumerable()
                .Select(s => new Marca(s))
                .ToList();

        }
    }
}