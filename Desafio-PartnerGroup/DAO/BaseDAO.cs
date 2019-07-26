using Desafio_PartnerGroup.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup.DAO {
    public abstract class BaseDAO<T> {
        public abstract T Find(int id);
        public abstract void Insert(T item);
        public abstract void Update(T item);
        public void Delete(int id) {

            BaseSQL.Execute(String.Format(@"DELETE FROM {0}s WHERE {0}Id = {1}", typeof(T).ToString(), id));

        }
        public abstract void GetAll();
    }
}