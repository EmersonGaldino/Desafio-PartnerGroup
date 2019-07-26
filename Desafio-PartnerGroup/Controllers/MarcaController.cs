using Desafio_PartnerGroup.Models;
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

            if (marca == null)
                return ErrorMessage(HttpStatusCode.BadRequest, "Corpo (Json) de Marca incorreto.");

            try {

                int id = Service.Marcas.Create(marca);
                return Result("Registro de Marca criado com sucesso.", id);

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas")]
        public HttpResponseMessage Get() {

            try {

                List<Marca> marcas = Service.Marcas.GetAll();
                return this.Request.CreateResponse(HttpStatusCode.Accepted, marcas);

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas/{id}")]
        public HttpResponseMessage Get(int id) {

            if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Marca marca = Service.Marcas.Find(id);

                if (marca != null)
                    return this.Request.CreateResponse(HttpStatusCode.Accepted, marca);
                else
                    return ErrorMessage(HttpStatusCode.BadRequest, "Não há marca com este ID no sistema");

            } catch (Exception ex) {

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas/{id}")]
        public HttpResponseMessage Put(Marca marca, int id) {

            if (marca == null) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Corpo (Json) de Marca incorreto.");
            } else if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Service.Marcas.Modify(id, marca);
                return Result("Registro de Marca alterado com sucesso.", marca.Id != 0 ? marca.Id : id);

            } catch (Exception ex) {

                if (ex.Message.Contains("conflicted with the REFERENCE constraint")) {
                    return ErrorMessage(HttpStatusCode.Conflict, "A alteração do ID não é possivel pois esta vinculado a um ou mais Patrimônios.");
                } else {
                    return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);
                }
            }        
        }

        [Route("marcas/{id}")]
        public HttpResponseMessage Delete(int id) {

            if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                Service.Marcas.Delete(id);
                return Result("Registro de Marca excluido com sucesso", id);


            } catch (Exception ex) {

                if (ex.Message.Contains("The DELETE statement conflicted"))
                    return ErrorMessage(HttpStatusCode.Conflict, "Falha na exclusão. Existem Patrimônios vinculados a essa Marca.");

                return ErrorMessage(HttpStatusCode.BadRequest, ex.Message);

            }
        }

        [Route("marcas/{id}/patrimonios")]
        public HttpResponseMessage GetPatrimonios(int id) {

            if (id <= 0) {
                return ErrorMessage(HttpStatusCode.BadRequest, "Número de ID precisa ser maior que 0.");
            }

            try {

                List<Patrimonio> patrimonios = Service.Marcas.GetAllByMarca(id);

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

        private HttpResponseMessage Result(string message, int id) {
            var response = new {
                Message = message,
                Url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/marcas/" + id
            };

            return this.Request.CreateResponse(HttpStatusCode.Accepted, response);
        }

        public HttpResponseMessage ErrorMessage(HttpStatusCode code, Exception ex) {
            var response = new {
                Type = code.ToString(),
                ex.Message
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
