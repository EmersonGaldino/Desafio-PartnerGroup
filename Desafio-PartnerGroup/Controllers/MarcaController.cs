using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Desafio_PartnerGroup.Controllers
{

    public class MarcaController : ApiController {

        [Route("marcas")]
        public HttpResponseMessage Post(Marca marca) {

            // VERIFICA MARCA

            if (marca == null || marca.Id == 0 || marca.Nome == null) {
                if (marca.Id == 0) {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("O ID da Marca precisa ser diferente de 0."));
                } else {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Corpo de Marca incorreta."));
                }
            }

            // BUSCA MARCAS PARA VERIFICAR DUPLICIDADE

            DataTable marcas = BaseSQL.Execute(String.Format(@"SELECT MarcaId, Nome
                            FROM Marcas
                            WHERE MarcaId = {0} OR Nome = '{1}'", marca.Id, marca.Nome));

            // VERIFICA DUPLICIDADE

            if (marcas.Rows.Count > 0) {

                if (marcas.AsEnumerable().Where(wh => wh.Field<int>("MarcaId") == marca.Id).FirstOrDefault() != null) {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("ID de Marca já existente no sistema"));
                }

                if (marcas.AsEnumerable().Where(wh => String.Equals(wh.Field<string>("Nome"), marca.Nome, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null) {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Nome de Marca já existente no sistema"));
                }

            }

            try {
                BaseSQL.Execute(String.Format("INSERT INTO Marcas VALUES({0},'{1}')",
                                               marca.Id,
                                               marca.Nome));

                var response = new {
                    Message = "Registro de Marca realizado com sucesso.",
                    Url = "http://localhost:51549/marcas/" + marca.Id
                };


                return this.Request.CreateResponse(HttpStatusCode.Accepted, response);
            } catch (Exception ex) {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("marcas")]
        public HttpResponseMessage Get() {

            try {

                DataTable marcas = BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas"));
                //<Marca> marcas = result.AsEnumerable().Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome"))).ToList();

                return this.Request.CreateResponse(HttpStatusCode.Accepted, marcas);

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas/{id}")]
        public HttpResponseMessage Get(int id) {

            try {

                Marca marca = BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas WHERE MarcaId = {0}", id))
                                                .AsEnumerable()
                                                .Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome")))
                                                .FirstOrDefault();

                if (marca != null)
                    return this.Request.CreateResponse(HttpStatusCode.Accepted, marca);
                else
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não há marca com este ID no sistema");

            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas/{id}/patrimonios")]
        public HttpResponseMessage GetPatrimonios(int id) {

            try {

                // Transforma a consulta em objeto genérico para mostrar Json com os atributos corretos (sem MarcaId)

                var patrimonios = BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId
                                                                WHERE Patrimonios.MarcaId = {0}", id))
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

        [Route("marcas/{id}")]
        public HttpResponseMessage Put(Marca marca, int id) {


            // VERIFICA MARCA

            if ((marca == null) || (marca.Id == 0 && String.IsNullOrEmpty(marca.Nome))) {
                if (marca != null && marca.Id == 0) {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("O ID do registro não pode ser 0."));
                } else {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Corpo de Marca incorreta."));
                }
            }

            // BUSCA MARCAS PARA VERIFICAR DUPLICIDADE

            List<Marca> marcasDuplicadas = BaseSQL.Execute(String.Format(@"SELECT * FROM Marcas WHERE MarcaId = {0} OR MarcaID = {1} OR Nome = '{2}'", id, marca.Id, marca.Nome))
                   .AsEnumerable()
                   .Select(s => new Marca(s.Field<int>("MarcaId"), s.Field<string>("Nome")))
                   .ToList();

            Marca alvo = marcasDuplicadas.Where(wh => wh.Id == id).FirstOrDefault();

            // VERIFICA E REMOVE MARCA A SER ALTERADA

            if (alvo != null)
                marcasDuplicadas.Remove(alvo);
            else
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception(String.Format("Marca com ID {0} não existe no sistema", id)));

            // VERIFICA SE O ID DA ALTERAÇÃO JA EXISTE

            if (marca.Id != id) {
               if (marcasDuplicadas.Where(wh => wh.Id == marca.Id).FirstOrDefault() != null) {
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("O ID que você esta tentando utilizar na alteração já existe em outro registro no sistema"));
               }
            }

            // VERIFICA SE O NOME DA ALTERAÇÃO JA EXISTE

            if (marcasDuplicadas.Where(wh => wh.Nome == marca.Nome && wh.Id != marca.Id).FirstOrDefault() != null) {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("O Nome que você esta tentando utilizar na alteração já existe em outro registro no sistema"));
            }

            try {

                string query = "UPDATE Marcas SET ";
                query += marca.Id != 0 ? String.Format("MarcaId = {0},", marca.Id) : String.Empty;
                query += !String.IsNullOrEmpty(marca.Nome) ? String.Format("Nome = '{0}'", marca.Nome) : String.Empty;

                if (query[query.Length - 1] == ',')
                    query = query.Remove(query.Length - 1);

                query += String.Format(" WHERE MarcaId = {0}", id);


                BaseSQL.Execute(query);

                var response = new {
                    Message = "Registro de Marca alterado com sucesso.",
                    Url = "http://localhost:51549/marcas/" + (marca.Id != 0 ? marca.Id : id)
                };

                return this.Request.CreateResponse(HttpStatusCode.Accepted, response);

            } catch (Exception ex) {

                if (ex.Message.Contains("conflicted with the REFERENCE constraint")) {

                    return this.Request.CreateErrorResponse(HttpStatusCode.Conflict, "A alteração do ID não é possivel pois esta vinculado a um ou mais Patrimônios.");

                } else {

                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }

            }
        }

        [Route("marcas/{id}")]
        public HttpResponseMessage Delete(int id) {

            try {

                BaseSQL.Execute(String.Format(@"DELETE FROM Marcas WHERE MarcaId = {0}", id));

                    return this.Request.CreateResponse(HttpStatusCode.Accepted, "Registro de Marca excluido com sucesso");


            } catch (Exception ex) {

                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }
        }
    }
}
