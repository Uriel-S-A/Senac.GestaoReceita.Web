using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Ingrediente
{
    public class DadosEmpresa
    {
        public int id { get; set; }
        public string CNPJ { get; set; }
        public string razaoSosial { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public int? numeroEndereco { get; set; }
        public string complemento { get; set; }
        public string nomeFantasia { get; set; }
        public string email { get; set; }
    }
}
