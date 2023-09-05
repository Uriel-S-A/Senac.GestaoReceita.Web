
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
using System.Web.Script.Serialization;
using Web.Models;

namespace CadEmpresa.Controllers
{
    public class EmpresaController : Controller
    {

        public EmpresaController()
        {

        }


        //Index da tela
        public ActionResult Index(string mensagemErro, string mensagemSucesso)
        {
            IndexViewModel indexViewModel = new IndexViewModel();

            var listaEmpresas = getEmpresa();

            indexViewModel.listaEmpresa = listaEmpresas;

            indexViewModel.mensagemErro = mensagemErro;
            indexViewModel.mensagemSucesso = mensagemSucesso;

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
                    return RedirectToAction("DadosCadastroEmpresa", new { CNPJ = inputCNPJ });
                }

            }

            return RedirectToRoute(new { controller = "Empresa", action = "Index", resultadoPesquisa });
        }


        //carrega dados das empresas por id
        public ActionResult DadosCadastroEmpresa(int? id, string CNPJ)
        {
            var empresa = new CadastroEmpresaViewModel();
            if (id != null)
            {
                empresa = getEmpresaById(id);
            }
            else
            {
                empresa.CNPJ = CNPJ;
            }

            empresa.listaCidade = getListaCidade();

            return View("DadosCadastroEmpresa", empresa);
        }



        //método para pegar todas as empresas
        public List<CadastroEmpresaViewModel> getEmpresa()
        {
            var lista = new List<CadastroEmpresaViewModel>();

            using (var client = new HttpClient())
            {

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
                            telefone = item.telefone,
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


        //método para pegar uma única empresa
        public CadastroEmpresaViewModel getEmpresaById(int? id)
        {

            var cadEmpresa = new CadastroEmpresaViewModel();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<fooEmpresaDTO>(stringResult.Result);

                    cadEmpresa = new CadastroEmpresaViewModel()
                    {
                        id = objectJson.id,
                        bairro = objectJson.bairro,
                        CNPJ = objectJson.CNPJ,
                        complemento = objectJson.complemento,
                        email = objectJson.email,
                        nomeFantasia = objectJson.nomeFantasia,
                        numeroEndereco = objectJson.numeroEndereco,
                        razaoSosial = objectJson.razaoSosial,
                        rua = objectJson.rua,
                        telefone = objectJson.telefone,
                        idCidade = objectJson.cidade.id,
                    };


                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
                return cadEmpresa;
            }
        }


        //método para deletar empresa
        public ActionResult DeletarEmpresa(int idDelete)
        {
            var mensagemSucesso = "";
            var mensagemErro = "";

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Empresas/" + idDelete);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    mensagemSucesso = "Empresa deletada com sucesso";
                }
                else
                {
                    mensagemErro = "Erro ao realizar o delete: " + response.Result.ReasonPhrase;
                }
            }

            return RedirectToAction("Index", new { mensagemErro = mensagemErro, mensagemSucesso = mensagemSucesso });
        }


        //Atualiza ou cadastra empresa
        public ActionResult PersistirCadastro(CadastroEmpresaViewModel dados)
        {
            var mensagemErro = "";
            var mensagemSucesso = "";

            var stringRetorno = "";
            if (dados.id != null && dados.id > 0)
            {
                stringRetorno = updateEmpresa(dados);
                mensagemSucesso = "Dados salvos com sucesso!";
            }

            else
            {
                stringRetorno = cadastrarEmpresa(dados);
            }

            if (!string.IsNullOrEmpty(stringRetorno))
            {
                mensagemErro = stringRetorno;
            }
            else
            {
                mensagemSucesso = "Empresa cadastrada com sucesso!";
            }

            return RedirectToAction("Index", new { mensagemErro = mensagemErro, mensagemSucesso = mensagemSucesso });
        }


        //método para atualizar empresa
        public string updateEmpresa(CadastroEmpresaViewModel dados)
        {
            var stringRetorno = "";
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new
                {
                    cnpj = dados.CNPJ,
                    razaoSosial = dados.razaoSosial,
                    rua = dados.rua,
                    bairro = dados.bairro,
                    numeroEndereco = dados.numeroEndereco,
                    complemento = dados.complemento,
                    email = dados.email,
                    telefone = dados.telefone,
                    nomeFantasia = dados.nomeFantasia,
                    idcidade = dados.idCidade,
                    updateEmpresa = DateTime.Now,
                    id = dados.id
                }), Encoding.UTF8, "application/json");

                var response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Empresas/" + dados.id, formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<fooEmpresaDTO>(stringResult.Result);
                }
                else
                {
                    stringRetorno = "Erro ao atualizar empresa: " + response.Result.ReasonPhrase;
                }
            }
            return stringRetorno;
        }

        //método para cadastrar empresa
        public string cadastrarEmpresa(CadastroEmpresaViewModel dados)
        {
            var stringRetorno = "";
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new
                {
                    CNPJ = dados.CNPJ,
                    razaoSosial = dados.razaoSosial,
                    rua = dados.rua,
                    bairro = dados.bairro,
                    numeroEndereco = dados.numeroEndereco,
                    complemento = dados.complemento,
                    email = dados.email,
                    telefone = dados.telefone,
                    nomeFantasia = dados.nomeFantasia,
                    idcidade = dados.idCidade,
                    createEmpresa = DateTime.Now,
                }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Empresas", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<fooEmpresaDTO>(stringResult.Result);
                }
                else
                {
                    stringRetorno = "Erro ao realizar o cadastro: " + response.Result.ReasonPhrase;
                }
            }
            return stringRetorno;
        }


        //gerar uma lista de cidades 
        public List<Cidade> getListaCidade()
        {
            var listaCidade = new List<Cidade>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<Cidade>>(stringResult.Result);

                    objectJson.ForEach(item => listaCidade.Add(
                        new Cidade()
                        {
                            id = item.id,
                            descricaoCidade = item.descricaoCidade
                        }));
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return listaCidade;
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
        public string telefone { get; set; }
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