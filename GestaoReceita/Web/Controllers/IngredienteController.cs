using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class IngredienteController : Controller
    {
        // GET: Ingrediente
        public ActionResult Index(DadosIngrediente dados)
        {
            //var ingredienteById = getIngredienteById();

            //var retorno = cadIngrediente("Açúcar");

            //var listaIngredientes = getIngredientes();

            List<DadosIngrediente> listaIngredientesCadastrados = new List<DadosIngrediente>()
            {
                new DadosIngrediente()
                {
                    Id = dados.Id,
                    NomeIngrediente = dados.NomeIngrediente,
                    QuantidadeUnidade = dados.QuantidadeUnidade,
                    PrecoIngrediente = dados.PrecoIngrediente,
                    UnidadeMedidaId = dados.UnidadeMedidaId,
                    EmpresaId = dados.EmpresaId
                },
                new DadosIngrediente(){
                    Id = 1,
                    NomeIngrediente = "Arroz",
                    QuantidadeUnidade = 3,
                    PrecoIngrediente = 10.00M,
                    UnidadeMedidaId = 1,
                    EmpresaId = 1
                },
                new DadosIngrediente(){
                    Id = 2,
                    NomeIngrediente = "Abacaxi",
                    QuantidadeUnidade = 2,
                    PrecoIngrediente = 5.00M,
                    UnidadeMedidaId = 2,
                    EmpresaId = 2
                },
                new DadosIngrediente(){
                    Id = 3,
                    NomeIngrediente = "Carne",
                    QuantidadeUnidade = 9,
                    PrecoIngrediente = 90.00M,
                    UnidadeMedidaId = 3,
                    EmpresaId = 3
                },
            };

            IndexViewModel indexViewModel = new IndexViewModel()
            {
                listaIngredientesCadastrados = listaIngredientesCadastrados,
            };

            return View(indexViewModel);
        }

        //public JsonResult cadIngrediente(string cidadeDescricao)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoCidade = cidadeDescricao, IdEstado = 1 }), Encoding.UTF8, "application/json");

        //        var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Cidades/3");

        //        response.Wait();

        //        if (response.Result.IsSuccessStatusCode)
        //        {
        //            var stringResult = response.Result.Content.ReadAsStringAsync();

        //            var objectJson = JsonConvert.DeserializeObject<fooCidadeDTO>(stringResult.Result);
        //        }
        //        else
        //        {
        //            //Erro de requisicao
        //            var content = response.Result.Content.ReadAsStringAsync();

        //            var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
        //        }
        //    }

        //    return Json(new { });
        //}

        //public JsonResult getIngredienteById()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

        //        var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades/3");

        //        response.Wait();

        //        if (response.Result.IsSuccessStatusCode)
        //        {
        //            var stringResult = response.Result.Content.ReadAsStringAsync();

        //            var objectJson = JsonConvert.DeserializeObject<fooCidadeDTO>(stringResult.Result);
        //        }
        //        else
        //        {
        //            //Erro de requisicao
        //            var content = response.Result.Content.ReadAsStringAsync();

        //            var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
        //        }
        //    }

        //    return Json(new { });
        //}



        //public JsonResult getIngredientes()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

        //        var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades");

        //        response.Wait();

        //        if (response.Result.IsSuccessStatusCode)
        //        {
        //            var stringResult = response.Result.Content.ReadAsStringAsync();





        //            var objectJson = JsonConvert.DeserializeObject<List<fooCidadeDTO>>(stringResult.Result);
        //        }
        //        else
        //        {
        //            //Erro de requisicao
        //            var content = response.Result.Content.ReadAsStringAsync();

        //            var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
        //        }
        //    }

        //    return Json(new { });
        //}

        public ActionResult MostrarItens(DadosIngrediente listaIngredientesCadastrados)
        {
            return RedirectToAction("Index");
        }
    }
}