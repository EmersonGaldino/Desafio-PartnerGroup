using Desafio_PartnerGroup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Desafio_PartnerGroup {
    public static class Service {
        public static MarcaService Marcas = new MarcaService();
        public static PatrimonioService Patrimonios = new PatrimonioService();
    }
}