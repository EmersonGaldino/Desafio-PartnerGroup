using Desafio_PartnerGroup.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_PartnerGroup {
    public interface IRepository<T> where T : Entity {

        T GetById(int id);

        int Insert(T item);

        void Delete(int id);

        void Update(int id, T item);

        List<T> Exist(T item);

        List<T> GetAll();
    }


}
