using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace CadEmpresa.Models
{
    public class CadastroEmpresaViewModel
    {
        public int id { get; set; }
        public string CNPJ { get; set; }
        public string razaoSosial { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public int? numeroEndereco { get; set; }
        public string complemento { get; set; }
        public string nomeFantasia { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public int idCidade { get; set; }

        public string inputCNPJ { get; set; }

        public List<Cidade> listaCidade { get; set; }
    }
}