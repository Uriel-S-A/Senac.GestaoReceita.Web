using Newtonsoft.Json;
using SolutionWebCadastroLogin.Models;
using SolutionWebCadastroLogin.Models.Usuarios;
using SolutionWebCadastroLogin.Models.UsuariosCadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SolutionWebCadastroLogin.Controllers
{
    public class LoginController : LoginExtention
    {
        public ActionResult Index(string mensagemErro, string mensagemSucesso)
        {
            LoginUsuario loginUsuario = new LoginUsuario();

            loginUsuario.mensagemErroAutenticacao = mensagemErro;
            loginUsuario.mensagemSucesso = mensagemSucesso;

            return View(loginUsuario);
        }

        public ActionResult Login(string Username, string Password)
        {
            var mensagemErro = string.Empty;

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { Username = Username, senha = Password }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/Login", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<LoginUsuarioRequest>(stringResult.Result);

                    saveUsuarioSessao("Username", objectJson.Id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    mensagemErro = response.Result.ReasonPhrase;
                }
            }
            return RedirectToAction("Index", new { mensagemErro = mensagemErro });
        }

        public ActionResult Editar()
        {
            var id = Session["IdUsuario"] as int?;

            var usuario = GetUsuarioById(id.GetValueOrDefault());
           
            return RedirectToAction("Usuarios", usuario);
        }

        public ActionResult Logout()
        {
            limparUsuarioSessao();

            return RedirectToAction("Index");
        }

        private CadastroViewModel GetUsuarioById(int id)
        {
            var usuario = new CadastroViewModel();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/"+id);


                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    usuario = JsonConvert.DeserializeObject<CadastroViewModel>(stringResult.Result);
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var erro = response.Result.ReasonPhrase;
                }
            }
            return usuario;
        }

        private List<UsuarioEdit> GetListaUsuarios()
        {
            var listaUsuarios = new List<UsuarioEdit>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/");


                response.Wait();
                if(response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    listaUsuarios = JsonConvert.DeserializeObject<List<UsuarioEdit>>(stringResult.Result);
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var erro = response.Result.ReasonPhrase;
                }
            }
            return listaUsuarios;
        }

        public void saveUsuarioSessao(string Username, int id)
        {
            Session["Username"] = Username;
            Session["IdUsuario"] = id;
        }

        public void limparUsuarioSessao()
        {
            Session["Username"] = null;
            Session["IdUsuario"] = null;
        }

        public ActionResult Cadastro(CadastroViewModel cadastro)
        {
            var listaEmpresas = getListaEmpresas();

            CadastroViewModel cadastroViewModel = cadastro != null ? cadastro : new CadastroViewModel();

            cadastroViewModel.listaEmpresas = listaEmpresas;

            return View(cadastroViewModel);
        }

        private List<Empresa> getListaEmpresas()
        {
            var listaEmpresas = new List<Empresa>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    listaEmpresas = JsonConvert.DeserializeObject<List<Empresa>>(stringResult.Result);

                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject(content.Result);
                }
            }
            return listaEmpresas;
        }

        public ActionResult PersistirCadastro(int? Id, string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            if (Id != null && Id > 0)
            {
                mensagemErro = updateUsuario(Id.GetValueOrDefault(), Nome, Usuario, Senha, Empresa);
            }
            else
            {
                mensagemErro = cadastrarUsuario(Nome, Usuario, Senha, Empresa);
            }

            if (!string.IsNullOrEmpty(mensagemErro))
            {
                return RedirectToAction("Cadastro", new CadastroViewModel()
                {
                    id = Id.GetValueOrDefault(),
                    nome = Nome,
                    acesso = 0,
                    ativo = 1,
                    empresaId = Empresa,
                    manterLogado = 0,
                    username = Usuario,
                    senha = Senha,
                    mensagemErro = mensagemErro,
                });
            }

            return RedirectToAction("Index", new { mensagemSucesso = "Dados salvos com sucesso" });
        }

        public string updateUsuario(int id, string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        id = id,
                        nome = Nome,
                        username = Usuario,
                        empresaId = Empresa,
                        senha = Senha,
                        acesso = 0,
                        manterLogado = 0,
                        ativo = 1
                    }
                ), Encoding.UTF8, "application/json");

                var response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Usuarios", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<CadastroUsuarioRequest>(stringResult.Result);

                    return string.Empty;
                }
                else
                {
                    mensagemErro = response.Result.ReasonPhrase;
                }
            }

            return mensagemErro;
        }


        public string cadastrarUsuario(string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        nome = Nome,
                        username = Usuario,
                        empresaId = Empresa,
                        senha = Senha,
                        acesso = 0,
                        manterLogado = 0,
                        ativo = 1
                    }
                ), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Usuarios", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<CadastroUsuarioRequest>(stringResult.Result);

                    return string.Empty;
                }
                else
                {
                    mensagemErro = response.Result.ReasonPhrase;
                }
            }

            return mensagemErro;
        }
    }
}
//   

//public JsonResult DeleteUsers(int id)
//{
//    using (var email = new HttpClient())
//    {
//        var response = email.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/" + id);
//            response.Wait();

//        if (response.Result.IsSuccessStatusCode)
//        {
//            var stringResult = response.Result.Content.ReadAsStringAsync();
//            var objectJason = JsonConvert.DeserializeObject<LoginUsuario>(stringResult.Result);

//        }
//        else
//        {

//            var content = response.Result.Content.ReadAsStringAsync();

//            var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
//        }
//    }
//}






