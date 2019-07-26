using Desafio_PartnerGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Services {
    public class MarcaService : IService<Marca> {

        public MarcaRepository Factory = new MarcaRepository();

        public MarcaService() {
        }

        public int Create(Marca item) {

            List<Marca> Existentes = Factory.Exist(item);

            if (Existentes.FirstOrDefault() == null) {

                return Factory.Insert(item);

            } else {

                if (Existentes.Where(wh => wh.Id == item.Id).FirstOrDefault() != null) {
                    throw new Exception("ID de Marca já existente no sistema");
                } else {
                    throw new Exception("Nome de Marca já existente no sistema");
                }
            }
        }

        public void Delete(int id) {

            Factory.Delete(Find(id).Id);

        }

        public Marca Find(int id) {

            Marca marca = Factory.GetById(id);

            if (marca != null) {
                return marca;
            } else {
                throw new Exception(String.Format("Não existe Marca com ID {0} no sistema", marca.Id));
            }

        }

        public Marca Find(Marca item) {

            return Factory.GetByItem(item);

        }

        public List<Marca> GetAll() {

            return Factory.GetAll();

        }

        public List<Patrimonio> GetAllByMarca(int id) {

            return Factory.GetAllPatrimoniosByMarca(id);

        }

        public void Modify(int id, Marca item) {


            List<Marca> Existentes = Factory.Exist(id, item);

            Marca alvo = Existentes.Where(wh => wh.Id == id).FirstOrDefault();

            // VERIFICA E REMOVE MARCA A SER ALTERADA

            if (alvo != null)
                Existentes.Remove(alvo);
            else
                throw new Exception(String.Format("Marca com ID {0} não existe no sistema", id));

            // VERIFICA SE O ID DA ALTERAÇÃO JA EXISTE

            if (item.Id != id) {
                if (Existentes.Where(wh => wh.Id == item.Id).FirstOrDefault() != null) {
                    throw new Exception("O ID que você esta tentando utilizar na alteração já existe em outro registro no sistema");
                }
            }

            // VERIFICA SE O NOME DA ALTERAÇÃO JA EXISTE

            if (Existentes.Where(wh => wh.Nome == item.Nome && wh.Id != item.Id).FirstOrDefault() != null) {
                throw new Exception("O Nome que você esta tentando utilizar na alteração já existe em outro registro no sistema");
            }

            Factory.Update(id, item);
        }
    }
}