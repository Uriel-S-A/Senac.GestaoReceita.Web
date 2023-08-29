
using CadEmpresa.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
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

        }


        public ActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel();

            var listaEmpresas = getEmpresa();

            indexViewModel.listaEmpresa = listaEmpresas;

            return View(indexViewModel);

        }


        //pesquisar empresas
        public ActionResult Pesquisar(string inputCNPJ)
        {

            IndexViewModel resultadoPesquisa = new IndexViewModel();

            var listaEmpresas = getEmpresa();

            resultadoPesquisa.listaEmpresa = listaEmpresas;


            for (int i = 0; i < resultadoPesquisa.listaEmpresa.Count; i++)
            {
                if (inputCNPJ == resultadoPesquisa.listaEmpresa[i].CNPJ)
                {
                    resultadoPesquisa.listaEmpresa = new List<CadastroEmpresaViewModel>();
                    resultadoPesquisa.listaEmpresa.Add(listaEmpresas[i]);

                    return View("Index", resultadoPesquisa);
                    //return RedirectToRoute(new { controller = "Empresa", action = "Index", resultadoPesquisa });
                }

                if (i == resultadoPesquisa.listaEmpresa.Count - 1 && inputCNPJ != resultadoPesquisa.listaEmpresa[i].CNPJ)
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
            var listaEmpresa = getEmpresaById(id);

            var empresa = listaEmpresa.SingleOrDefault(search => search.id == id);

            return new CadastroEmpresaViewModel();
        }



        //metudo para pegar todas as empresas
        public List<CadastroEmpresaViewModel> getEmpresa()
        {
            var lista = new List<CadastroEmpresaViewModel>();

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<fooEmpresaDTO>>(stringResult.Result);

                    objectJson.ForEach(item => lista.Add(
                        new CadastroEmpresaViewModel()
                        {
                            id = item.id,
                            bairro = item.bairro,
                            CNPJ = item.CNPJ,
                            complemento = item.complemento,
                            email = item.email,
                            nomeFantasia = item.nomeFantasia,
                            numeroEndereco = item.numeroEndereco,
                            razaoSosial = item.razaoSosial,
                            rua = item.rua,
                        }));
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return lista;
        }


        //métudo para pegar uma única empresa
        public List<CadastroEmpresaViewModel> getEmpresaById(int? id)
        {
            var lista = new List<CadastroEmpresaViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<fooEmpresaDTO>>(stringResult.Result);

                    objectJson.ForEach(item => lista.Add(
                        new CadastroEmpresaViewModel()
                        {
                            id = item.id,
                            bairro = item.bairro,
                            CNPJ = item.CNPJ,
                            complemento = item.complemento,
                            email = item.email,
                            nomeFantasia = item.nomeFantasia,
                            numeroEndereco = item.numeroEndereco,
                            razaoSosial = item.razaoSosial,
                            rua = item.rua,
                        }));
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return lista;
        }
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
