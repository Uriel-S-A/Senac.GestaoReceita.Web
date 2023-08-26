
using CadEmpresa.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CadEmpresa.Controllers
{
    public class EmpresaController : Controller
    {

        public EmpresaController()
        {
            resultadoPesquisa.listaEmpresa = listaEmpresas;
        }

        IndexViewModel resultadoPesquisa = new IndexViewModel();

        private List<CadastroEmpresaViewModel> listaEmpresas = new List<CadastroEmpresaViewModel>()
        { };

        public ActionResult Index()
        {
            var listaEmpresas = getEmpresa();

            return View();

        }


        public string DataHoraAtual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        }

        public ActionResult Pesquisar(string inputCNPJ)
        {

            IndexViewModel resultadoPesquisa = new IndexViewModel();

            listaEmpresas = new List<CadastroEmpresaViewModel>()
            {

            };


            for (int i = 0; i < listaEmpresas.Count; i++)
            {
                if (inputCNPJ == listaEmpresas[i].CNPJ)
                {
                    resultadoPesquisa.listaEmpresa = new List<CadastroEmpresaViewModel>();
                    resultadoPesquisa.listaEmpresa.Add(listaEmpresas[i]);

                    return View("Index", resultadoPesquisa);
                    //return RedirectToRoute(new { controller = "Empresa", action = "Index", resultadoPesquisa });
                }

                if (i == listaEmpresas.Count - 1 && inputCNPJ != listaEmpresas[i].CNPJ)
                {
                    return RedirectToRoute(new { controller = "Empresa", action = "DadosCadastroEmpresa" });
                }

            }

            resultadoPesquisa.listaEmpresa = listaEmpresas;

            return RedirectToRoute(new { controller = "Empresa", action = "Index", resultadoPesquisa });
        }

        public ActionResult DadosCadastroEmpresa(int? id)
        {
            var empresa = new CadastroEmpresaViewModel();
            if (id != null)
            {
                empresa = _getDadosEmpresa(id);
            }

            return View(empresa);
        }

        private CadastroEmpresaViewModel _getDadosEmpresa(int? id)
        {
            var empresa = listaEmpresas.SingleOrDefault(search => search.id == id);

            return empresa;
        }


        public JsonResult getEmpresa()
        {
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<fooEmpresaDTO>>(stringResult.Result);

                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return Json(new { });
        }


        public class fooEmpresaDTO
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
            public fooEstadoRequestDTO cidade { get; set; }
        }

        public class fooCidadeRequestDTO
        {
            public int id { get; set; }
            public string descricaoCidade { get; set; }
            public int idEstado { get; set; }
            public fooEstadoRequestDTO estado { get; set; }
        }

        public class fooEstadoRequestDTO
        {
            public int id { get; set; }
            public string descricaoEstado { get; set; }
            public int idPais { get; set; }
            public fooPaisRequestDTO pais { get; set; }
        }

        public class fooPaisRequestDTO
        {
            public int id { get; set; }
            public string descricaoPais { get; set; }
        }

    }
}