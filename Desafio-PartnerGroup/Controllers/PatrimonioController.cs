using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Desafio_PartnerGroup.Controllers
{
    public class PatrimonioController : ApiController {

        [Route("patrimonios")]
        public HttpResponseMessage Post(Patrimonio patrimonio) {

            // VERIFICA PATRIMONIO

            if (patrimonio == null || patrimonio.Nome == null) {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Corpo de Patrimonio incorreto."));
            }

            // BUSCA PATRIMONIOS PARA VERIFICAR DUPLICIDADE

            //DataTable patrimonios = BaseSQL.Execute(String.Format(@"SELECT *
            //                FROM dbo.Patrimonios
            //                WHERE PatrimonioId = {0}", patrimonio.Id, patrimonio.Nome));

            // VERIFICA DUPLICIDADE

            //if (patrimonios.Rows.Count > 0) {

            //    if (patrimonios.AsEnumerable().Where(wh => wh.Field<int>("PatrimonioId") == patrimonio.Id).FirstOrDefault() != null) {
            //        return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("ID de Patrimônio já existente no sistema"));
            //    }

            //    if (patrimonios.AsEnumerable().Where(wh => String.Equals(wh.Field<string>("Nome"), patrimonio.Nome, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null) {
            //        return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Nome de Patrimônio já existente no sistema"));
            //    }

            //}

            try {

                // Regista Patrimonio e retorna o ID gerado pelo sistema

                DataTable result = BaseSQL.Execute(String.Format("INSERT INTO Patrimonios VALUES('{0}',{1},{2}); SELECT SCOPE_IDENTITY();",
                                               patrimonio.Nome,
                                               patrimonio.Marca != null ? patrimonio.Marca.Id : patrimonio.MarcaId,
                                               patrimonio.Descricao != null ? String.Format("'{0}'", patrimonio.Descricao) : "null"));

                patrimonio.Id = Decimal.ToInt32((Decimal)result.Rows[0][0]);

                var response = new {
                    Message = "Registro de Patrimônio realizado com sucesso.",
                    Url = "http://localhost:51549/patrimonios/" + patrimonio.Id
                };

                return this.Request.CreateResponse(HttpStatusCode.Accepted, response);

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios")]
        public HttpResponseMessage Get() {

            try {

                // Transforma a consulta em objeto genérico para mostrar Json com os atributos corretos (sem MarcaId)

                var patrimonios = BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId"))
                                                                .AsEnumerable()
                                                                .Select(s => new {
                                                                    Id = s.Field<int>("PatrimonioId"),
                                                                    Nome = s.Field<string>("Nome"),
                                                                    Marca = new Marca(s.Field<int>("MarcaId"), s.Field<string>("Marca")),
                                                                    Descricao = s.Field<string>("Descrição")
                                                                });
                

                return this.Request.CreateResponse(HttpStatusCode.Accepted, patrimonios);

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Get(int id) {

            try {

                var patrimonio = BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId
                                                                WHERE PatrimonioId = {0}", id))
                                                                .AsEnumerable()
                                                                .Select(s => new {
                                                                    Id = s.Field<int>("PatrimonioId"),
                                                                    Nome = s.Field<string>("Nome"),
                                                                    Marca = new Marca(s.Field<int>("MarcaId"), s.Field<string>("Marca")),
                                                                    Descricao = s.Field<string>("Descrição")
                                                                }).FirstOrDefault();

                if (patrimonio != null)
                    return this.Request.CreateResponse(HttpStatusCode.Accepted, patrimonio);
                else
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não há Patrimônio com este ID no sistema");

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Put(Patrimonio patrimonio, int id) {


            // VERIFICA MARCA

            if (patrimonio == null) {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Corpo de Patrimônio incorreta."));
            }

            if (patrimonio.MarcaId != 0 && patrimonio.Marca != null && patrimonio.Marca.Id != patrimonio.MarcaId) {
                return this.Request.CreateErrorResponse(HttpStatusCode.Conflict, "Há um conflito entre o atributo MarcaId e Id do Atributo Marca, esses atributos não podem ter IDs diferentes");
            } else if (patrimonio.MarcaId == 0 && patrimonio.Marca != null && patrimonio.Marca.Id == 0 && !String.IsNullOrEmpty(patrimonio.Marca.Nome)) {

                // Verifica o Id da Marca caso só o nome esteja preenchido e preenche corretamente a Marca

                try {

                    DataTable marca = BaseSQL.Execute(String.Format("SELECT * FROM Marcas WHERE Nome = '{0}'", patrimonio.Marca.Nome));
                    if (marca.Rows.Count > 0) {
                        patrimonio.Marca = marca.AsEnumerable().Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome"))).FirstOrDefault();
                    } else {
                        return this.Request.CreateErrorResponse(HttpStatusCode.Conflict, "Não existe uma Marca registrada com esse nome");
                    }

                } catch (Exception ex) {

                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

                }
            }

            // REALIZA O UPDATE

            try {

                String query = "UPDATE Patrimonios SET ";
                query += !String.IsNullOrEmpty(patrimonio.Nome) ? String.Format("Nome = '{0}',", patrimonio.Nome) : String.Empty;
                query += patrimonio.MarcaId != 0 ? String.Format("MarcaId = {0},", patrimonio.Marca.Id) :
                         patrimonio.Marca != null ? String.Format("MarcaId = {0},", patrimonio.Marca.Id) : String.Empty;
                query += !String.IsNullOrEmpty(patrimonio.Descricao) ? String.Format("Descrição = '{0}',", patrimonio.Descricao) : String.Empty;

                if (query[query.Length - 1] == ',')
                    query = query.Remove(query.Length - 1);

                query += String.Format(" WHERE PatrimonioId = {0}", id);

                BaseSQL.Execute(query);

                return this.Request.CreateResponse(HttpStatusCode.Accepted, "Alteração realizada com sucesso");

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Delete(int id) {

            try {

                Patrimonio marca = BaseSQL.Execute(String.Format(@"SELECT PatrimonioId FROM Patrimonios WHERE PatrimonioId = {0}", id))
                                                .AsEnumerable()
                                                .Select(s => new Patrimonio() { Id = s.Field<int>("PatrimonioId") })
                                                .FirstOrDefault();

                if (marca != null) {

                    BaseSQL.Execute(String.Format(@"DELETE FROM Patrimonios WHERE PatrimonioId = {0}", id));
                    return this.Request.CreateResponse(HttpStatusCode.Accepted, "Registro de Patrimonio excluido com sucesso");

                } else {

                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não há marca com este ID no sistema");

                }

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

    }
}
