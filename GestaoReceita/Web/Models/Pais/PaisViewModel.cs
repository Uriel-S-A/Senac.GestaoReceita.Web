using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Pais
{
    public class PaisViewModel
    {     
        public int id { get; set; }
        public string descricaoPais { get; set; }

        public static explicit operator PaisViewModel(PaisTO v)
        {
            if (v == null) return null;

            return new PaisViewModel
            {
                id = v.id,
                descricaoPais = v.descricaoPais
            };
        }
    }
}