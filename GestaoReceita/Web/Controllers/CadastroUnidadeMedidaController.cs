using System.Web.Mvc;
using Web.Model.CadastroUnidadeMedida;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http;
using System;
using System.Text;

namespace Web.Controllers
{
    public class CadastroUnidadeMedidaController : Controller
    {
        // GET: CadastroUnidadeMedida
        public ActionResult Index()
        {
            //chamada da api
            var listaUnidadeMedida = getListaUnidadeMedida();

            var indexViewModel = new CadastrarUnidadeMedidaViewModel()
            {
                listaUnidadeMedida = listaUnidadeMedida
            };

            return View(indexViewModel);
        }

        public List<UnidadeMedida> getListaUnidadeMedida()
        {
            var lista = new List<UnidadeMedida>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/UnidadeMedidas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<List<UnidadeMedidaRequest>>(stringResult.Result);

                    objectJson.ForEach(unidadeMedidaRequest =>
                        lista.Add(
                            new UnidadeMedida()
                            {
                                Id = unidadeMedidaRequest.id,
                                Descricao = unidadeMedidaRequest.descUnidMedIngrediente,
                                Sigla = unidadeMedidaRequest.sigla,
                            }
                          )
                        );
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return lista;
        }


        public ActionResult PersistirUnidadeMedida(int? id, string descricao, string sigla)
        {

            if (id != null && id > 0)
            {
                updateReceita(id.GetValueOrDefault(), descricao, sigla);
            }
            else
            {
                cadastrarReceita(descricao, sigla);
            }


            return RedirectToAction("Index");
        }

        public void updateReceita(int id, string descricao, string sigla)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new
                {
                    id = id,
                    descUnidMedIngrediente = descricao,
                    sigla = sigla,
                }), Encoding.UTF8, "application/json");

                var response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/UnidadeMedidas/" + id, formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
        }

        public void cadastrarReceita(string descricao, string sigla)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new
                {        
                    descUnidMedIngrediente = descricao,
                    sigla = sigla,
                }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/UnidadeMedidas/", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
        }

        public JsonResult Deletar(int id)
        {
            var sucesso = true;
            var msgRetorno = "Deletado com sucesso";
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/UnidadeMedidas/"+id);

                response.Wait();

                if (!response.Result.IsSuccessStatusCode)
                {
                    msgRetorno = response.Result.ReasonPhrase;
                    sucesso = false;
                }
            }

            return Json(new { mensagemRetorno = msgRetorno, sucesso = sucesso });
        }
    }
}