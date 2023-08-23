
using CadEmpresa.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        {
            new CadastroEmpresaViewModel(){ id = 1, nomeFantasia = "Mercor", bairro = "Centro", CNPJ = "34.397.453/0001-15", complemento = "Fabrica", email = "Merco@gmail.com", razaoSosial = "Mercor S/A",rua ="julio de Castilho", numeroEndereco=532},
            new CadastroEmpresaViewModel(){ id = 2, nomeFantasia = "Apple", bairro = "Avanida", CNPJ = "98.076.736/0001-48", complemento = "Predio", email = "Aple@hotmail.com", razaoSosial = "Apple LTDA" ,rua ="Coronle",numeroEndereco=565},
            new CadastroEmpresaViewModel(){ id = 3, nomeFantasia = "Senac", bairro = "João Pessoa", CNPJ = "14.369.549/0001-62", complemento = "Escola", email = "Senac@gmail.com", razaoSosial = "Senac Santa Cruz", rua="Maceio",numeroEndereco=656},
            new CadastroEmpresaViewModel(){ id = 4, nomeFantasia = "Alura", bairro = "Fernando Abot", CNPJ = "47.660.588/0001-73", complemento = "Escola", email = "Alura@outlok.com", razaoSosial = "Alura Ltda", rua="Henrique Santos",numeroEndereco=824},
            new CadastroEmpresaViewModel(){ id = 5, nomeFantasia = "Magalu", bairro = "Ipiranga", CNPJ = "53.285.725/0001-30", complemento = "Loja", email = "Magalu@email.com", razaoSosial = "Magazine Luiza" ,rua="brono brenoso",numeroEndereco=287},
            new CadastroEmpresaViewModel(){ id = 6, nomeFantasia = "Kabum", bairro = "Centro", CNPJ = "35.685.837/0001-04", complemento = "Loja", email = "Kabum@hotmail.com", razaoSosial = "Kabum Kabum" ,rua="Kleitom Machado",numeroEndereco=179},
            new CadastroEmpresaViewModel(){ id = 7, nomeFantasia = "Clip", bairro = "Igenopolis", CNPJ = "34.883.475/0001-95", complemento = "Predio", email = "Clip@gmail.com", razaoSosial = "Clip Papelaria", rua="julio Sesar",numeroEndereco=752},
            new CadastroEmpresaViewModel(){ id = 7, nomeFantasia = "Sansung", bairro = "Margarida", CNPJ = "95.253.315/0001-57", complemento = "Edificio", email = "Sansung@outlok.com", razaoSosial = "Sansung Mobile", rua="Napoleon",numeroEndereco=951} ,
        };

        public ActionResult Index()
        {
            return View(resultadoPesquisa);
        }

        public string DataHoraAtual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        }

        //public JsonResult Pesquisar(string inputCNPJ)
        public ActionResult Pesquisar(string inputCNPJ)
        {
            IndexViewModel resultadoPesquisa = new IndexViewModel();

            listaEmpresas = new List<CadastroEmpresaViewModel>()
            {
                new CadastroEmpresaViewModel(){ id = 1, nomeFantasia = "Mercor", bairro = "Centro", CNPJ = "34.397.453/0001-15", complemento = "Fabrica", email = "Merco@gmail.com", razaoSosial = "Mercor S/A",rua ="julio de Castilho", numeroEndereco=532},
                new CadastroEmpresaViewModel(){ id = 2, nomeFantasia = "Apple", bairro = "Avanida", CNPJ = "98.076.736/0001-48", complemento = "Predio", email = "Aple@hotmail.com", razaoSosial = "Apple LTDA" ,rua ="Coronle",numeroEndereco=565},
                new CadastroEmpresaViewModel(){ id = 3, nomeFantasia = "Senac", bairro = "João Pessoa", CNPJ = "14.369.549/0001-62", complemento = "Escola", email = "Senac@gmail.com", razaoSosial = "Senac Santa Cruz", rua="Maceio",numeroEndereco=656},
                new CadastroEmpresaViewModel(){ id = 4, nomeFantasia = "Alura", bairro = "Fernando Abot", CNPJ = "47.660.588/0001-73", complemento = "Escola", email = "Alura@outlok.com", razaoSosial = "Alura Ltda", rua="Henrique Santos",numeroEndereco=824},
                new CadastroEmpresaViewModel(){ id = 5, nomeFantasia = "Magalu", bairro = "Ipiranga", CNPJ = "53.285.725/0001-30", complemento = "Loja", email = "Magalu@email.com", razaoSosial = "Magazine Luiza" ,rua="brono brenoso",numeroEndereco=287},
                new CadastroEmpresaViewModel(){ id = 6, nomeFantasia = "Kabum", bairro = "Centro", CNPJ = "35.685.837/0001-04", complemento = "Loja", email = "Kabum@hotmail.com", razaoSosial = "Kabum Kabum" ,rua="Kleitom Machado",numeroEndereco=179},
                new CadastroEmpresaViewModel(){ id = 7, nomeFantasia = "Clip", bairro = "Igenopolis", CNPJ = "34.883.475/0001-95", complemento = "Predio", email = "Clip@gmail.com", razaoSosial = "Clip Papelaria", rua="julio Sesar",numeroEndereco=752},
                new CadastroEmpresaViewModel(){ id = 7, nomeFantasia = "Sansung", bairro = "Margarida", CNPJ = "95.253.315/0001-57", complemento = "Edificio", email = "Sansung@outlok.com", razaoSosial = "Sansung Mobile", rua="Napoleon",numeroEndereco=951} ,
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

            //return Json(new { listaEmpresas }, JsonRequestBehavior.AllowGet);
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


        public ActionResult CadastroEmpresa()
        {
            return View();
        }


        //public ActionResult Empresa(string CNPJ)
        //{

        //    for(int i = 0; i < listaEmpresas.Count; i++)
        //    {
        //        if (CNPJ == listaEmpresas[i].CNPJ)
        //        {
        //            return View(listaEmpresas[i]);
        //        }
        //    }

        //    return View();

        //}


    }
}