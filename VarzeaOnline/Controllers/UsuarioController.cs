using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using VarzeaOnline.Data;
using VarzeaOnline.Models;

namespace VarzeaOnline.Controllers
{
    public class UsuarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Gera hash SHA256 da senha para nunca armazenar texto puro no banco
        public static string HashSHA256(string senhaPura)
        {
            if (string.IsNullOrEmpty(senhaPura)) return "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senhaPura));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }

        // GET: Usuario/Login
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Campeonato");
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string senha, string returnUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    ViewBag.Erro = "Email e senha são obrigatórios.";
                    return View();
                }

                // Aplica hash na senha digitada para comparar com o hash salvo no banco
                string senhaHash = HashSHA256(senha);

                var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senhaHash);
                if (usuario == null)
                {
                    ViewBag.Erro = "Email ou senha incorretos.";
                    return View();
                }

                FormsAuthentication.SetAuthCookie(usuario.Nome, false);
                Session["UsuarioLogado"] = usuario.Nome;
                Session["UsuarioId"] = usuario.IdUsuario;

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Campeonato");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao processar: " + ex.Message;
                return View();
            }
        }

        // GET: Usuario/Cadastro
        public ActionResult Cadastro()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Campeonato");
            return View();
        }

        // POST: Usuario/Cadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(string nome, string email, string senha, string confirmarSenha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(confirmarSenha))
                {
                    ViewBag.Erro = "Todos os campos são obrigatórios.";
                    return View();
                }

                if (senha.Length < 6)
                {
                    ViewBag.Erro = "A senha deve ter pelo menos 6 caracteres.";
                    return View();
                }

                if (senha != confirmarSenha)
                {
                    ViewBag.Erro = "As senhas não coincidem.";
                    return View();
                }

                if (db.Usuarios.Any(u => u.Email == email))
                {
                    ViewBag.Erro = "Este email já está cadastrado.";
                    return View();
                }

                // Aplica hash SHA256 na senha antes de salvar — texto puro nunca vai ao banco
                var usuario = new Usuario
                {
                    Nome = nome,
                    Email = email,
                    Senha = HashSHA256(senha)
                };

                db.Usuarios.Add(usuario);
                db.SaveChanges();

                // Inicia sessão após cadastro bem-sucedido
                FormsAuthentication.SetAuthCookie(usuario.Nome, false);
                Session["UsuarioLogado"] = usuario.Nome;
                Session["UsuarioId"] = usuario.IdUsuario;

                TempData["Sucesso"] = "Conta criada com sucesso! Bem-vindo, " + usuario.Nome + "!";
                return RedirectToAction("Index", "Campeonato");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao processar: " + ex.Message;
                return View();
            }
        }

        // GET: Usuario/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Usuario");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
