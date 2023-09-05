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
        public ActionResult Index(string BuscaIngredientes)
        {
            // chama o método GetDadosIngrediente e passa a resposta pra lista
            List<DadosIngrediente> listaIngredientesCadastrados = GetDadosIngrediente();

            // verifica se o campo buscar NÃO está nulo ou vazio
            if (!string.IsNullOrEmpty(BuscaIngredientes))
            {
                // passa pra lista apenas os ingredientes que contém o que foi digitado no campo buscar
                listaIngredientesCadastrados = listaIngredientesCadastrados.Where(w => w.NomeIngrediente.Contains(BuscaIngredientes)).ToList();
            }
            else
            {
                // chama o método Get novamente, para retornar a lista completa de dados
                listaIngredientesCadastrados = GetDadosIngrediente();
            }

            // chama o método GetDadosEmpresa e passa a resposta pra lista
            List<DadosEmpresa> listaDadosEmpresa = GetDadosEmpresa();

            // chama o método GetDadosUnidadeMedida e passa a resposta pra lista
            List<DadosUnidadeMedida> listaDadosUnidadeMedida = GetDadosUnidadeMedida();

            // passa pra indexViewModel todas as listas com todos os dados necessários
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                listaIngredientesCadastrados = listaIngredientesCadastrados,
                listaDadosEmpresa = listaDadosEmpresa,
                listaDadosUnidadeMedida = listaDadosUnidadeMedida,
            };

            // retorna a indexViewModel pra index
            return View(indexViewModel);
        }

        // Método GET: Ingrediente
        public List<DadosIngrediente> GetDadosIngrediente()
        {
            List<DadosIngrediente> listaDadosIngrediente = new List<DadosIngrediente>();

            using (var client = new HttpClient())
            {
                // envia a requisição pra API para receber os dados dos ingredientes através do método GET
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes");
                // espera receber uma resposta
                response.Wait();

                // verifica se a requisição foi efetuada com sucesso
                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    // retorno da API
                    var objectJson = JsonConvert.DeserializeObject<List<DadosIngredienteRequest>>(stringResult.Result);

                    foreach (var item in objectJson)
                    {
                        listaDadosIngrediente.Add(
                            new DadosIngrediente()
                            {
                                EmpresaId = item.empresaId,
                                Id = item.id,
                                NomeIngrediente = item.nomeIngrediente,
                                PrecoIngrediente = item.precoIngrediente.ToString(),
                                QuantidadeUnidade = item.quantidadeUnidade,
                                UnidadeMedidaId = item.unidadeMedidaId,
                                Empresa = item.empresa,
                                UnidadeMedida = item.unidadeMedida
                            }
                        );
                    }
                }
                else
                {
                    // erro na requisicao
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
            // retorna a listaDadosIngrediente
            return listaDadosIngrediente;
        }

        // Método GET: Empresa
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
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }

            return listaDadosEmpresa;
        }

        // Método GET: UnidadeMedida
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

        // Método PersistirIngrediente (POST / PUT)
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
                ), Encoding.UTF8, "application/json");

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
                    // erro de requisicao
                    throw new Exception(response.Result.ReasonPhrase);
                }
            }
            // chama a index novamente para atualizar a tela
            return RedirectToAction("Index");
        }

        // Método DELETE: Ingrediente
        public ActionResult DeleteDadosIngrediente(DadosIngrediente dados)
        {
            using (var client = new HttpClient())
            {
                // envia a requisição pra API para deletar os dados dos ingredientes através do método DELETE
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Ingredientes/" + dados.Id);
                // espera a resposta da requisição
                response.Wait();

                // verifica se a requisição teve sucesso ou não
                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception(response.Result.ReasonPhrase);
                }
                // chama a index novamente para atualizar a tela
                return RedirectToAction("Index");
            }
        }
    }
}