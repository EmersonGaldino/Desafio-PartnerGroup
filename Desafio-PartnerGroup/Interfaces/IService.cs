using Desafio_PartnerGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.Services {
    public interface IService<T> {

        int Create(T item);

        void Delete(int id);

        void Modify(int id, T item);

        T Find(int id);

        List<T> GetAll();
    }
}