using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Pais;

namespace Web.Controllers
{
    public class PaisController : Controller
    {
       
        public ActionResult Index()
        {
            List<PaisViewModel> minhaLista = getPaises();            
            return View(minhaLista);
        }


        public List<PaisViewModel> getPaises()
        {
            List<PaisViewModel> paisViewModel = new List<PaisViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Pais");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                                        
                    var paisToList = JsonConvert.DeserializeObject<List<PaisTO>>(stringResult.Result);
                    List<PaisViewModel> paisViewModelList = new List<PaisViewModel>();
                    
                    foreach (var paisTo in paisToList)
                    {
                        PaisViewModel paisvm = new PaisViewModel
                        {                            
                            id = paisTo.id,
                            descricaoPais = paisTo.descricaoPais,
                        };

                        paisViewModelList.Add(paisvm);
                    }                        
                    
                    return paisViewModel = paisViewModelList;
                }
                else
                {                    
                    //mensagem de erro de validação

                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                    
                }
            }

            return paisViewModel;
        }

        public ActionResult AdicionarPais(PaisViewModel novoPais)
        {            
            if (ModelState.IsValid)
            {
                List<PaisViewModel> minhaLista = getPaises();

                //comparação será sensível a maiúsculas e minúsculas usando StringComparison.OrdinalIgnoreCase
                bool paisJaExiste = minhaLista.Any(p => p.descricaoPais.Equals(novoPais.descricaoPais, StringComparison.OrdinalIgnoreCase));

                if (paisJaExiste)
                {
                    //não aparece na tela
                    ModelState.AddModelError("descricaoPais", "O país já existe na lista.");
                    List<PaisViewModel> lista = getPaises();
                    return View("Index", lista);
                }

                int novoId = minhaLista.Max(max => max.id) + 1;

                using (var client = new HttpClient())
                {
                    var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoPais = novoPais.descricaoPais, id = novoId }), Encoding.UTF8, "application/json");

                    var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Pais", formContentString);
                    

                    response.Wait();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        // adicionar mensagem de sucesso

                        var stringResult = response.Result.Content.ReadAsStringAsync();
                        var objectJson = JsonConvert.DeserializeObject<PaisTO>(stringResult.Result); //todas info que vem do banco devem ser armazenadas num TO.
                    }
                    else
                    {
                        // adicionar mensagem de erro
                        var content = response.Result.Content.ReadAsStringAsync();
                        var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                    }
                }
            }

            return Index();
        }
        
        public ActionResult EditarPais(PaisViewModel paisEditar)
        {
            //já chegando id e descrição do pais.
                       
            var dados = getCidadeById(paisEditar.id);

            var Id = dados.id = paisEditar.id;
            var DescricaoPais =  dados.descricaoPais = paisEditar.descricaoPais;            

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoPais = DescricaoPais, id = Id }), Encoding.UTF8, "application/json");
                
                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Pais/" + dados.id), formContentString);                

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    // adicionar mensagem de sucesso

                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<PaisViewModel>(stringResult.Result);
                }
                else
                {
                    // adicionar mensagem de erro
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return Json(new { });
        }

        public PaisViewModel getCidadeById(int id)
        {
            PaisViewModel paisViewModel = new PaisViewModel();

            using (var client = new HttpClient())
            {                
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Pais/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var paisTO = JsonConvert.DeserializeObject<PaisTO>(stringResult.Result);                    

                    PaisViewModel paisView = (PaisViewModel)paisTO;
                                        
                    return paisViewModel = paisView;
                }
                else
                {
                    //tratar erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return paisViewModel;
        }



        public ActionResult DeletarPais(PaisViewModel paisDeletar)
        {
            return Json(new { });
        }

    }
}


public class fooCidadeDTO
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
    public PaisTO pais { get; set; }
}
