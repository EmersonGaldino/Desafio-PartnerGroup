using Desafio_PartnerGroup.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_PartnerGroup {
    public interface IRepository<T> {

        int Insert(T item);

        void Update(int id, T item);

        void Delete(int id);

        T GetById(int id);

        List<T> GetAll();
    }


}
