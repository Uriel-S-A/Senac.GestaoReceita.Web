using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Empresa
{
    public class CadastroNovaEmpresa
    {
        public int id { get; set; }
        public string CNPJ { get; set; }
        public string razaoSosial { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public int? numeroEndereco { get; set; }
        public string complemento { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string nomeFantasia { get; set; }
        public int idcidade { get; set; }
        public DateTime createEmpresa { get; set; }
        public DateTime updateEmpresa { get; set; }
        public int idUsername { get; set; }
    }
}