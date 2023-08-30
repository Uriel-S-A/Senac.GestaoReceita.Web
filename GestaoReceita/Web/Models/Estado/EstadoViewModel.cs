using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Estado
{
    public class EstadoViewModel
    {
        public int Id { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Sigla { get; set; }
    }
}