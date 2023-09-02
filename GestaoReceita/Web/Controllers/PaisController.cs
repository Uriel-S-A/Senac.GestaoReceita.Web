using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Estado;
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
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                    
                }
            }

            return paisViewModel;
        }

        public ActionResult AdicionarPais(PaisViewModel novoPais)
        {
            List<PaisViewModel> minhaLista = getPaises();

            if (ModelState.IsValid)
            {                                
                bool paisJaExiste = minhaLista.Any(p => p.descricaoPais.Equals(novoPais.descricaoPais, StringComparison.OrdinalIgnoreCase));

                if (paisJaExiste)
                {
                    ModelState.AddModelError("paisExistente", "Este pais já existe.");

                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
                    
                int novoId = minhaLista.Max(max => max.id) + 1;

                this.CadastrarNovoPais(novoPais, novoId);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("paisExistente", "Este pais já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }
            
            return View("Index", minhaLista);
        }

        private void CadastrarNovoPais(PaisViewModel novoPais, int novoId)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoPais = novoPais.descricaoPais, id = novoId }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Pais", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {                    
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<PaisTO>(stringResult.Result); 
                }
                else
                {                    
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }
        }

        public ActionResult EditarPais(PaisViewModel paisEditar)
        {

            List<PaisViewModel> minhaLista = getPaises();

            if (ModelState.IsValid)
            {                                
                bool paisJaExiste = minhaLista.Any(p => p.descricaoPais.Equals(paisEditar.descricaoPais, StringComparison.OrdinalIgnoreCase));

                if (paisJaExiste)
                {
                    ModelState.AddModelError("paisExistente", "Este pais já existe.");

                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }

                var dados = getEstadoById(paisEditar.id);

                var Id = dados.id = paisEditar.id;
                var DescricaoPais = dados.descricaoPais = paisEditar.descricaoPais;

                EditarNovoPais(dados, DescricaoPais, Id);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("paisExistente", "Este pais já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }

            return View("Index", minhaLista);
        }

        private void EditarNovoPais(PaisViewModel dados, string DescricaoPais, int Id)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { descricaoPais = DescricaoPais, id = Id }), Encoding.UTF8, "application/json");

                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Pais/" + dados.id), formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {                    
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<PaisViewModel>(stringResult.Result);
                }
                else
                {                    
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }
        }

        public PaisViewModel getEstadoById(int id)
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
