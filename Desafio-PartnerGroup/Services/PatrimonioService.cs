using Desafio_PartnerGroup.Models;
using Desafio_PartnerGroup.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Services {
    public class PatrimonioService : IService<Patrimonio> {

        public PatrimonioRepository Factory = new PatrimonioRepository();

        public int Create(Patrimonio item) {

            Marca marca = ValidateMarca(item);

            if (marca == null) {
                throw new Exception("Marca inexistente no sistema");
            } else {
                item.Marca = marca;
            }

            return Factory.Insert(item);

        }

        public void Delete(int id) {
            Factory.Delete(Find(id).Id);
        }

        public Patrimonio Find(int id) {

            Patrimonio patrimonio = Factory.GetById(id);

            if (patrimonio != null) {
                return patrimonio;
            } else {
                throw new Exception(String.Format("Não existe Patrimônio com ID {0} no sistema", patrimonio.Id));
            }

        }

        public List<Patrimonio> GetAll() {

            return Factory.GetAll();

        }

        public void Modify(int id, Patrimonio item) {

            Patrimonio patrimonio = Find(id);

            if (patrimonio == null)
                throw new Exception(String.Format("Patrimônio com ID {0} não existe no sistema", id));


            if (item.Marca != null || item.MarcaId != 0) {

                try {
                    Marca marca = ValidateMarca(item);
                    if (marca != null) {
                        item.Marca = marca;
                    } else {
                        throw new Exception("Marca inexistente no Sistema");
                    }
                } catch (Exception ex) {
                    if (!ex.Message.Contains("Atributo MarcaId ou Marca")) {
                        throw ex;
                    }
                }
            }

            Factory.Update(id, item);

        }

        public Marca ValidateMarca(Patrimonio item) {

            if (item.MarcaId != 0) {
                if (item.Marca == null) {
                    item.Marca = new Marca() { Id = item.MarcaId };
                } else if (item.Marca.Id != 0 && item.Marca.Id != item.Marca.Id) {
                    throw new Exception("Os atributos MarcaId e Marca (ID) não podem ser diferentes. Defina o ID a ser ignorado como 0");
                } else if (item.Marca.Id == 0) {
                    item.Marca.Id = item.MarcaId;
                }
            } else {
                if (item.Marca == null || (item.Marca.Id == 0 && String.IsNullOrEmpty(item.Marca.Nome))) {
                    throw new Exception("Atributo MarcaId ou Marca (ID ou Nome) é necessario");
                }
            }

            return Service.Marcas.Find(item.Marca);

        }
    }
}