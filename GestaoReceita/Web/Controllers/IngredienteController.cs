using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.Models.Ingrediente;

namespace Web.Controllers
{
    public class IngredienteController : Controller
    {
        // GET: Ingrediente
        public ActionResult Index(DadosIngrediente dados)
        {
            List<DadosIngrediente> listaIngredientesCadastrados = getDadosIngrediente();
            //List<Empresas> listaDadosEmpresa = getDadosEmpresa();
            //List<UnidadeMedas> listaUnidadeMedidas = getDadosUnidadeMedida();

            IndexViewModel indexViewModel = new IndexViewModel()
            {
                listaIngredientesCadastrados = listaIngredientesCadastrados,
                //listaDadosEmpresa = listaDadosEmpresa,
                //listaDadosUnidadeMedida = listaUnidadeMedidas,
            };

            return View(indexViewModel);
        }

        public List<DadosIngrediente> getDadosIngrediente()
        {
            List<DadosIngrediente> listaDadosIngrediente = new List<DadosIngrediente>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<DadosIngredienteRequest>>(stringResult.Result);

                    foreach(var item in objectJson)
                    {
                        listaDadosIngrediente.Add(
                            new DadosIngrediente()
                            {
                                EmpresaId = item.empresaId,
                                Id = item.id,
                                NomeIngrediente = item.nomeIngrediente,
                                PrecoIngrediente = item.precoIngrediente,
                                QuantidadeUnidade = item.quantidadeUnidade,
                                UnidadeMedidaId = item.unidadeMedidaId,
                            }
                        );
                    }
                }
                else
                {
                    //Erro de requisicao
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return listaDadosIngrediente;
        }
        public ActionResult PersistirIngrediente(DadosIngrediente dados)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject
                    (new {
                            id = dados.Id,
                            nomeIngrediente = dados.NomeIngrediente,
                            precoIngrediente = dados.PrecoIngrediente,
                            quantidadeUnidade = dados.QuantidadeUnidade,
                            empresaId = dados.EmpresaId,
                            unidadeMedidaId = dados.UnidadeMedidaId
                         }
                    ), Encoding.UTF8, "application/json"); ;

                Task<HttpResponseMessage> response = null;

                if (dados.Id > 0)
                {
                    response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes", formContentString);
                }
                else
                {
                    response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes", formContentString);
                }


                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                }
                else
                {
                    //Erro de requisicao
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
            return RedirectToAction("Index");
        }


        //public JsonResult cadIngrediente(string cidadeDescricao)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoCidade = cidadeDescricao, IdEstado = 1 }), Encoding.UTF8, "application/json");

        //        var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes/3");

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

        //        var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes/3");

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

        //        var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes");

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