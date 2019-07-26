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

            if (patrimonio == null) {
                return ErrorMessage(HttpStatusCode.BadRequest, new Exception("Corpo de Patrimônio incorreto."));
            }

            try {

                int id = Service.Patrimonios.Create(patrimonio);
                return Result("Registro de Patrimônio realizado com sucesso.", id);

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, new Exception(ex.Message));

            }

        }

        [Route("patrimonios")]
        public HttpResponseMessage Get() {

            try {

                // Transforma a consulta em objeto genérico para mostrar Json com os atributos corretos (sem MarcaId)

                List<Patrimonio> patrimonios = BaseSQL.Execute(String.Format(@"SELECT Patrimonios.*,Marcas.Nome as Marca
                                                                FROM Patrimonios
                                                                INNER JOIN Marcas ON Patrimonios.MarcaId = Marcas.MarcaId"))
                                                                .AsEnumerable()
                                                                .Select(s => new Patrimonio(s))
                                                                .ToList();



                var patrimoniosJson = patrimonios.Select(s => new {
                    s.Id,
                    s.Nome,
                    s.Marca,
                    s.Descricao
                }).ToList();
                

                return this.Request.CreateResponse(HttpStatusCode.Accepted, patrimoniosJson);

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Get(int id) {

            if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Patrimonio patrimonio = Service.Patrimonios.Find(id);
                var patrimonioJson = new {
                    patrimonio.Id,
                    patrimonio.Nome,
                    patrimonio.Marca,
                    patrimonio.Descricao
                };


                if (patrimonio != null) {
                    return this.Request.CreateResponse(HttpStatusCode.Accepted, patrimonioJson);
                } else {
                    return ErrorMessage(HttpStatusCode.BadRequest, "Não há Patrimônio com este ID no sistema");
                }

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Put(Patrimonio patrimonio, int id) {

            if (patrimonio == null) {
                return ErrorMessage(HttpStatusCode.BadRequest, new Exception("Corpo (Json) de Patrimônio incorreto."));
            } else if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Service.Patrimonios.Modify(id, patrimonio);
                return Result("Alteração realizada com sucesso.", id);

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("patrimonios/{id}")]
        public HttpResponseMessage Delete(int id) {

            if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Service.Patrimonios.Delete(id);
                return Result("Registro de Patrimônio excluido com sucesso", id);


            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }


        private HttpResponseMessage Result(string message, int id) {
            var response = new {
                Message = message,
                Url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/patrimonios/" + id
            };

            return this.Request.CreateResponse(HttpStatusCode.Accepted, response);
        }

        public HttpResponseMessage ErrorMessage(HttpStatusCode code, Exception ex) {
            var response = new {
                Type = code.ToString(),
                Message = ex.Message
            };

            return this.Request.CreateResponse(code, response);
        }

        public HttpResponseMessage ErrorMessage(HttpStatusCode code, string message) {
            var response = new {
                Type = code.ToString(),
                Message = message
            };

            return this.Request.CreateResponse(code, response);
        }

    }
}
