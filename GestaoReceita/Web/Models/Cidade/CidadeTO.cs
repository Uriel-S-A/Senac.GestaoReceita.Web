using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Estado;

namespace Web.Models.Cidade
{
    public class CidadeTO
    {
        public int id { get; set; }
        public string descricaoCidade { get; set; }
        public int idEstado { get; set; }
        public EstadoTO estado { get; set; }
    }
}