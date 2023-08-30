using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.Models;
using Web.Models.Ingrediente;

namespace Web.Controllers
{
    public class IngredienteController : Controller
    {
        // GET: Ingrediente
        public ActionResult Index()
        {
            List<DadosIngrediente> listaIngredientesCadastrados = GetDadosIngrediente();
            List<DadosEmpresa> listaDadosEmpresa = GetDadosEmpresa();
            List<DadosUnidadeMedida> listaDadosUnidadeMedida = GetDadosUnidadeMedida();

            IndexViewModel indexViewModel = new IndexViewModel()
            {
                listaIngredientesCadastrados = listaIngredientesCadastrados,
                listaDadosEmpresa = listaDadosEmpresa,
                listaDadosUnidadeMedida = listaDadosUnidadeMedida,
            };

            return View(indexViewModel);
        }

        public List<DadosIngrediente> GetDadosIngrediente()
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

        public List<DadosEmpresa> GetDadosEmpresa()
        {
            List<DadosEmpresa> listaDadosEmpresa = new List<DadosEmpresa>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<DadosEmpresa>>(stringResult.Result);

                    foreach (var item in objectJson)
                    {
                        listaDadosEmpresa.Add(
                            new DadosEmpresa()
                            {
                                id = item.id,
                                CNPJ = item.CNPJ,
                                razaoSosial = item.razaoSosial,
                                rua = item.rua,
                                bairro = item.bairro,
                                numeroEndereco = item.numeroEndereco,
                                complemento = item.complemento,
                                nomeFantasia = item.nomeFantasia,
                                email = item.email,
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

            return listaDadosEmpresa;
        }

        public List<DadosUnidadeMedida> GetDadosUnidadeMedida()
        {
            List<DadosUnidadeMedida> listaDadosUnidadeMedida = new List<DadosUnidadeMedida>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/UnidadeMedidas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<DadosUnidadeMedida>>(stringResult.Result);

                    foreach (var item in objectJson)
                    {
                        listaDadosUnidadeMedida.Add(
                            new DadosUnidadeMedida()
                            {
                                Id = item.Id,
                                descUnidMedIngrediente = item.descUnidMedIngrediente,
                                Sigla = item.Sigla,
                            }
                        );
                    }
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
            return listaDadosUnidadeMedida;
        }
 
        public ActionResult PersistirIngrediente(DadosIngrediente dados)
        {

            using (var client = new HttpClient())
            {
                // json stringify
                var formContentString = new StringContent(JsonConvert.SerializeObject
                    (new
                    {
                        id = dados.Id,
                        nomeIngrediente = dados.NomeIngrediente,
                        precoIngrediente = dados.PrecoIngrediente,
                        quantidadeUnidade = dados.QuantidadeUnidade,
                        empresaId = dados.EmpresaId,
                        unidadeMedidaId = dados.UnidadeMedidaId
                    }
                    // cabeçalho da requisição
                    ), Encoding.UTF8, "application/json"); ;

                // prepara a variável para receber a resposta da requisição
                Task<HttpResponseMessage> response = null;

                // verifica se o ingrediente já tem Id, se tiver, significa que já está cadastrado
                if (dados.Id > 0)
                { // se está cadastrado, então chama o método PUT para atualizar / editar
                    response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes/" + dados.Id, formContentString);
                }
                else
                { // se não está cadastrado, chama o método POST para cadastrar no banco de dados
                    response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes", formContentString);
                }

                // espera a resposta da requisição
                response.Wait();

                // verifica se a resposta(requisição) teve sucesso
                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                }
                else
                { // se não, joga um erro na tela
                    //Erro de requisicao
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            // chama a index novamente para atualizar a tela
            return RedirectToAction("Index");
        }

        // DELETE --------------------------------------------------
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

        // GET by Id ----------------------------------------------------------------------
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


        // GET --------------------------------------------------------------------
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
    }
}