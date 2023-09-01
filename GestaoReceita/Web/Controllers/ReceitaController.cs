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
using System.Threading.Tasks;

namespace ReceitaFrontEnd.Controllers
{

    public class ReceitaController : Controller
    {
        private List<ReceitaViewModel> list = new List<ReceitaViewModel>();
        private Random random = new Random();
        // GET: Receita
        public ActionResult Index()
        {


            var receitas = receitasBuscar();


            return View(receitas);


        }

        public ActionResult Cadastro(int? idReceita)
        {
            CadastroViewModel cadastroViewModel = new CadastroViewModel();

            //Chamar API
            var list = pegarListaIngredientes();

            ////Chamar API
            var listaIngredientesDaReceita = new List<DadosIngredientes>();
            if (idReceita != null && idReceita > 0)
            {
                listaIngredientesDaReceita = pegarIngredientesDaReceita(idReceita.GetValueOrDefault());
                cadastroViewModel.NomeReceita = listaIngredientesDaReceita.Count > 0 ? listaIngredientesDaReceita[0].NomeReceita : "";
                cadastroViewModel.Id = listaIngredientesDaReceita.Count > 0 ? listaIngredientesDaReceita[0].IdReceita : null;
            }

            cadastroViewModel.DadosIngredientes = list;
            cadastroViewModel.DadosIngredientesDaReceita = listaIngredientesDaReceita;


            return View(cadastroViewModel);
        }

        public List<DadosIngredientes> pegarIngredientesDaReceita(int idReceita)
        {

            var listaIgredientes = new List<DadosIngredientes>();

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Receita/"+idReceita);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var receita = JsonConvert.DeserializeObject<ReceitaRequest>(stringResult.Result);

                    foreach (var item in receita.receitaIngrediente)
                    {
                        listaIgredientes.Add(new DadosIngredientes()
                        {
                            Id = item.idingrediente,
                            NomeIngrediente = item.ingrediente.nomeIngrediente,
                            PrecoIngrediente = item.ingrediente.precoIngrediente,
                            UnidadeMedida = item.ingrediente.unidadeMedida.sigla,
                            Quantidade = item.ingrediente.quantidadeUnidade,
                            NomeReceita = receita.nomeReceita,
                            IdReceita = receita.id
                        });
                    }

                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();
                }
            }

            return listaIgredientes;
        }

        public bool excluirTudo(List<int> listaId)
        {
            bool check = true;

            using (var client = new HttpClient())
            {

                foreach (var id in listaId)
                {
                    if (check == true)
                    {

                        var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Receita/" + id);

                        response.Wait();

                        if (response.Result.IsSuccessStatusCode)
                        {
                            check = true;

                        }
                        else
                        {

                            var content = response.Result.Content.ReadAsStringAsync();

                            var ret = JsonConvert.DeserializeObject<List<ValidationResult>>(content.Result);

                            check = false;
                        }
                    }

                }
                return check;
            }


        }

        [HttpPost]
        public string submeterCodigo(string digitado)
        {
            var token = Session["token"] as string;

            if (!String.IsNullOrEmpty(digitado))
            {
                if (token == digitado)
                {
                    return "Codigo correto";

                }
                else
                {
                    return "Codigo não bate";

                }

            }
            return "Codigo vazio";

        }

        public List<DadosReceitaViewModel> receitasBuscar()
        {
            var listaReceita = new List<DadosReceitaViewModel>();

            using (var client = new HttpClient())
            {


                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Receita");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();


                    var objectJson = JsonConvert.DeserializeObject<List<ReceitaViewModel>>(stringResult.Result);


                    foreach (var item in objectJson)
                    {
                        listaReceita.Add(new DadosReceitaViewModel()
                        {
                            Id = item.Id,
                            valorTotalReceita = item.valorTotalReceita,
                            nomeReceita = item.nomeReceita,
                        });


                    }

                }
                else
                {

                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<List<ValidationResult>>(content.Result);

                }
                return listaReceita;
            }


        }

        public bool Excluir(List<int> listaId)
        {
            bool check = true;

            using (var client = new HttpClient())
            {

                foreach (var id in listaId)
                {
                    if (check == true)
                    {

                        var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Receita/" + id);

                        response.Wait();

                        if (response.Result.IsSuccessStatusCode)
                        {
                            check = true;

                        }
                        else
                        {

                            var content = response.Result.Content.ReadAsStringAsync();

                            var ret = JsonConvert.DeserializeObject<List<ValidationResult>>(content.Result);

                            check = false;
                        }
                    }

                }
                return check;
            }


        }

        public string GenerateToken()
        {
            Session["token"] = new StringBuilder();

            int length = 10;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder tokenBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char randomChar = chars[index];
                tokenBuilder.Append(randomChar);
            }

            Session["token"] = tokenBuilder.ToString();

            return tokenBuilder.ToString();
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


        public JsonResult cadastrarReceita(string nomereceita, List<CadastrarIngredientesViewModel> listaingredientes, int? id)
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
                        idingrediente = item.Id,
                        quantidadeIngrediente = item.Quantidade,
                    });
                }
                var objetoData = new
                {
                    nomeReceita = nomereceita,
                    modoPreparo = "aaaa",
                    receitaIngrediente = listaParametro
                };

                Task<HttpResponseMessage> response;

                var formContentString = new StringContent(JsonConvert.SerializeObject(objetoData), Encoding.UTF8, "application/json");
                if(id != null && id > 0)
                {
                    response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Receita/"+ id, formContentString);
                }
                else
                {
                    response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Receita", formContentString);
                }

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


