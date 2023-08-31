using ReceitaFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;

namespace ReceitaFrontEnd.Controllers
{
    public class ReceitaController : Controller
    {
        // GET: Receita
        public ActionResult Index()
        {




            return View();
        }

        public ActionResult Cadastro(int? idReceita)
        {
            //Chamar API
            var list = pegarListaIngredientes();

            ////Chamar API
            //var litaIngredientesDaReceita = pegarIngredientesDaReceita(idReceita.getValueOrDefault());

            CadastroViewModel cadastroViewModel = new CadastroViewModel();

            cadastroViewModel.DadosIngredientes = list;

            return View(cadastroViewModel);
        }



        public List<DadosIngredientes> pegarListaIngredientes()
        {

            var listaIgredientes = new List<DadosIngredientes>();

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var listaIngredienteResponse = JsonConvert.DeserializeObject<List<IngredienteResponse>>(stringResult.Result);


                    foreach (var item in listaIngredienteResponse)
                    {
                        listaIgredientes.Add(new DadosIngredientes()
                        {
                            Id = item.id,
                            NomeIngrediente = item.nomeIngrediente,
                            PrecoIngrediente = item.precoIngrediente,
                            UnidadeMedida = item.unidadeMedida.sigla,
                        });
                    }

                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return listaIgredientes;
        }


        public JsonResult cadastrarReceita(string nomereceita, List<CadastrarIngredientesViewModel> listaingredientes)
        {

            var msgRetorno = "";
            var isSucesso = false;

            using (var client = new HttpClient())
            {
                var listaParametro = new List<object>();
                foreach (var item in listaingredientes)
                {
                    listaParametro.Add(
                    new
                    {
                        id = 0,
                        idReceita = 0,
                        idingrediente = item.Id,
                        quantidadeIngrediente = item.Quantidade,
                        idGastoVariado = 0,
                        qntGastoVariado = 0
                    });
                }
                var objetoData = new
                {
                    nomeReceita = nomereceita,
                    modoPreparo = "aaaa",
                    valorTotalReceita = 0,
                    receitaIngrediente = listaParametro
                };

                var formContentString = new StringContent(JsonConvert.SerializeObject(objetoData), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Receita", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    isSucesso = true;

                    msgRetorno = "Cadastrado com sucesso";
                }
                else
                {
                    msgRetorno = response.Result.ReasonPhrase;
                }
            }

            return Json(new { result = msgRetorno, isSuccess = isSucesso });
        }
    }
}


//{
//  "nomeReceita": "string",
//  "modoPreparo": "string",
//  "valorTotalReceita": 0,
//  "receitaIngrediente": [
//    {
//      "id": 0,
//      "idReceita": 0,
//      "idingrediente": 0,
//      "quantidadeIngrediente": 0,
//      "idGastoVariado": 0,
//      "qntGastoVariado": 0
//    }
//  ]
//}