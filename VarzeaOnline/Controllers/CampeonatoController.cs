using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VarzeaOnline.Data;
using VarzeaOnline.Models;

namespace VarzeaOnline.Controllers
{
    public class CampeonatoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Campeonato
        public ActionResult Index()
        {
            var campeonatos = db.Campeonatos.Include(c => c.Usuario).ToList();
            return View(campeonatos);
        }

        // GET: Campeonato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Campeonato campeonato = db.Campeonatos
                .Include(c => c.Jogadores.Select(j => j.Time))
                .Include(c => c.Usuario)
                .FirstOrDefault(c => c.IdCampeonato == id);

            if (campeonato == null)
                return HttpNotFound();

            var partidas = db.Partidas
                .Include(p => p.JogadorCasa.Time)
                .Include(p => p.JogadorFora.Time)
                .Where(p => p.IdCampeonato == id)
                .OrderBy(p => p.Rodada)
                .ThenBy(p => p.IdPartida)
                .ToList();

            ViewBag.Partidas = partidas;

            var classificacao = db.Classificacoes
                .Include(c => c.Jogador.Time)
                .Where(c => c.IdCampeonato == id)
                .OrderByDescending(c => c.Pontos)
                .ThenByDescending(c => c.Vitorias)
                .ThenByDescending(c => c.SaldoGols)
                .ThenByDescending(c => c.GolsPro)
                .ToList();

            ViewBag.Classificacao = classificacao;

            return View(campeonato);
        }

        // GET: Campeonato/Create
        public ActionResult Create()
        {
            CarregarDropdowns();
            return View();
        }

        // POST: Campeonato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,TipoCampeonato,QuantidadeJogadores")] Campeonato campeonato)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.FirstOrDefault();
                if (usuario == null)
                {
                    ModelState.AddModelError("", "Nenhum usuario cadastrado. Cadastre um usuario primeiro.");
                    CarregarDropdowns();
                    return View(campeonato);
                }

                campeonato.IdUsuario = usuario.IdUsuario;
                campeonato.DataCriacao = DateTime.Now;
                campeonato.Status = "Aguardando Jogadores";
                campeonato.RodadaAtual = 0;
                campeonato.TotalRodadas = 0;

                db.Campeonatos.Add(campeonato);
                db.SaveChanges();

                return RedirectToAction("AdicionarJogadores", new { id = campeonato.IdCampeonato });
            }

            CarregarDropdowns();
            return View(campeonato);
        }

        // GET: Campeonato/AdicionarJogadores/5
        public ActionResult AdicionarJogadores(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var campeonato = db.Campeonatos
                .Include(c => c.Jogadores.Select(j => j.Time))
                .FirstOrDefault(c => c.IdCampeonato == id);

            if (campeonato == null)
                return HttpNotFound();

            CarregarTimesDropdown();
            return View(campeonato);
        }

        // POST: Campeonato/AdicionarJogador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarJogador(int IdCampeonato, string NomeJogador, int IdTime)
        {
            var campeonato = db.Campeonatos
                .Include(c => c.Jogadores)
                .FirstOrDefault(c => c.IdCampeonato == IdCampeonato);

            if (campeonato == null)
                return HttpNotFound();

            if (campeonato.Jogadores.Count >= campeonato.QuantidadeJogadores)
            {
                TempData["Erro"] = "O campeonato ja atingiu o limite de jogadores!";
                return RedirectToAction("AdicionarJogadores", new { id = IdCampeonato });
            }

            var timeJaEscolhido = db.Jogadores
                .Any(j => j.IdCampeonato == IdCampeonato && j.IdTime == IdTime);

            if (timeJaEscolhido)
            {
                TempData["Erro"] = "Este time ja foi escolhido por outro jogador!";
                return RedirectToAction("AdicionarJogadores", new { id = IdCampeonato });
            }

            var jogador = new Jogador
            {
                NomeJogador = NomeJogador,
                IdTime = IdTime,
                IdCampeonato = IdCampeonato
            };

            db.Jogadores.Add(jogador);
            db.SaveChanges();

            TempData["Sucesso"] = "Jogador adicionado!";

            var totalJogadores = db.Jogadores.Count(j => j.IdCampeonato == IdCampeonato);
            if (totalJogadores >= campeonato.QuantidadeJogadores)
            {
                campeonato.Status = "Pronto para Iniciar";
                db.Entry(campeonato).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Sucesso"] = "Todos os jogadores adicionados! Campeonato pronto para iniciar.";
            }

            return RedirectToAction("AdicionarJogadores", new { id = IdCampeonato });
        }

        // POST: Campeonato/RemoverJogador/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverJogador(int id)
        {
            var jogador = db.Jogadores.Find(id);
            if (jogador == null)
                return HttpNotFound();

            int campeonatoId = jogador.IdCampeonato;
            db.Jogadores.Remove(jogador);

            var campeonato = db.Campeonatos.Find(campeonatoId);
            if (campeonato != null)
            {
                campeonato.Status = "Aguardando Jogadores";
                db.Entry(campeonato).State = EntityState.Modified;
            }

            db.SaveChanges();
            TempData["Sucesso"] = "Jogador removido!";
            return RedirectToAction("AdicionarJogadores", new { id = campeonatoId });
        }

        // =============================================
        // INICIAR CAMPEONATO - Gerar partidas
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IniciarCampeonato(int id)
        {
            var campeonato = db.Campeonatos
                .Include(c => c.Jogadores)
                .FirstOrDefault(c => c.IdCampeonato == id);

            if (campeonato == null)
                return HttpNotFound();

            if (campeonato.Status != "Pronto para Iniciar")
            {
                TempData["Erro"] = "O campeonato nao esta pronto para iniciar!";
                return RedirectToAction("Index");
            }

            var jogadores = campeonato.Jogadores.ToList();
            int n = jogadores.Count;

            if (campeonato.TipoCampeonato == "Pontos Corridos")
            {
                var lista = new List<Jogador>(jogadores);
                if (n % 2 != 0)
                {
                    lista.Add(null);
                    n = lista.Count;
                }

                int totalRodadas = n - 1;
                campeonato.TotalRodadas = totalRodadas;
                campeonato.RodadaAtual = 1;
                campeonato.Status = "Em Andamento";

                var fixo = lista[0];
                var rotativos = lista.Skip(1).ToList();

                for (int rodada = 1; rodada <= totalRodadas; rodada++)
                {
                    var rodadaJogadores = new List<Jogador> { fixo };
                    rodadaJogadores.AddRange(rotativos);

                    int metade = rodadaJogadores.Count / 2;

                    for (int i = 0; i < metade; i++)
                    {
                        var casa = rodadaJogadores[i];
                        var fora = rodadaJogadores[rodadaJogadores.Count - 1 - i];

                        if (casa == null || fora == null)
                            continue;

                        var partida = new Partida
                        {
                            Rodada = rodada,
                            IdCampeonato = campeonato.IdCampeonato,
                            IdJogadorCasa = casa.IdJogador,
                            IdJogadorFora = fora.IdJogador,
                            GolsCasa = null,
                            GolsFora = null,
                            ResultadoRegistrado = false,
                            DataPartida = DateTime.Now
                        };

                        db.Partidas.Add(partida);
                    }

                    var ultimo = rotativos[rotativos.Count - 1];
                    rotativos.RemoveAt(rotativos.Count - 1);
                    rotativos.Insert(0, ultimo);
                }

                foreach (var jogador in campeonato.Jogadores)
                {
                    var classif = new Classificacao
                    {
                        IdCampeonato = campeonato.IdCampeonato,
                        IdJogador = jogador.IdJogador,
                        Jogos = 0,
                        Pontos = 0,
                        Vitorias = 0,
                        Empates = 0,
                        Derrotas = 0,
                        GolsPro = 0,
                        GolsContra = 0,
                        SaldoGols = 0
                    };
                    db.Classificacoes.Add(classif);
                }

                db.SaveChanges();
                TempData["Sucesso"] = "Campeonato iniciado! " + totalRodadas + " rodadas geradas.";
            }
            else if (campeonato.TipoCampeonato == "Mata-Mata")
            {
                campeonato.TotalRodadas = (int)Math.Log(n, 2);
                campeonato.RodadaAtual = 1;
                campeonato.Status = "Em Andamento";

                var rng = new Random();
                var embaralhados = jogadores.OrderBy(x => rng.Next()).ToList();

                for (int i = 0; i < embaralhados.Count; i += 2)
                {
                    var partida = new Partida
                    {
                        Rodada = 1,
                        IdCampeonato = campeonato.IdCampeonato,
                        IdJogadorCasa = embaralhados[i].IdJogador,
                        IdJogadorFora = embaralhados[i + 1].IdJogador,
                        GolsCasa = null,
                        GolsFora = null,
                        ResultadoRegistrado = false,
                        DataPartida = DateTime.Now
                    };
                    db.Partidas.Add(partida);
                }

                db.SaveChanges();
                TempData["Sucesso"] = "Mata-Mata iniciado! Rodada 1 gerada.";
            }

            return RedirectToAction("Details", new { id = campeonato.IdCampeonato });
        }

        // =============================================
        // REGISTRAR RESULTADO
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarResultado(int IdPartida, int GolsCasa, int GolsFora)
        {
            var partida = db.Partidas.Find(IdPartida);
            if (partida == null)
                return HttpNotFound();

            partida.GolsCasa = GolsCasa;
            partida.GolsFora = GolsFora;
            partida.ResultadoRegistrado = true;
            db.Entry(partida).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Sucesso"] = "Resultado registrado!";
            return RedirectToAction("Details", new { id = partida.IdCampeonato });
        }

        // =============================================
        // FINALIZAR RODADA
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizarRodada(int id)
        {
            var campeonato = db.Campeonatos.Find(id);
            if (campeonato == null)
                return HttpNotFound();

            var partidasRodada = db.Partidas
                .Where(p => p.IdCampeonato == id && p.Rodada == campeonato.RodadaAtual)
                .ToList();

            if (partidasRodada.Any(p => !p.ResultadoRegistrado))
            {
                TempData["Erro"] = "Registre todos os resultados antes de finalizar a rodada!";
                return RedirectToAction("Details", new { id = id });
            }

            if (campeonato.TipoCampeonato == "Pontos Corridos")
            {
                foreach (var partida in partidasRodada)
                {
                    var classCasa = db.Classificacoes
                        .FirstOrDefault(c => c.IdCampeonato == id && c.IdJogador == partida.IdJogadorCasa);
                    var classFora = db.Classificacoes
                        .FirstOrDefault(c => c.IdCampeonato == id && c.IdJogador == partida.IdJogadorFora);

                    if (classCasa == null || classFora == null) continue;

                    int golsCasa = partida.GolsCasa ?? 0;
                    int golsFora = partida.GolsFora ?? 0;

                    classCasa.Jogos++;
                    classFora.Jogos++;

                    classCasa.GolsPro += golsCasa;
                    classCasa.GolsContra += golsFora;
                    classFora.GolsPro += golsFora;
                    classFora.GolsContra += golsCasa;

                    classCasa.SaldoGols = classCasa.GolsPro - classCasa.GolsContra;
                    classFora.SaldoGols = classFora.GolsPro - classFora.GolsContra;

                    if (golsCasa > golsFora)
                    {
                        classCasa.Vitorias++;
                        classCasa.Pontos += 3;
                        classFora.Derrotas++;
                    }
                    else if (golsFora > golsCasa)
                    {
                        classFora.Vitorias++;
                        classFora.Pontos += 3;
                        classCasa.Derrotas++;
                    }
                    else
                    {
                        classCasa.Empates++;
                        classCasa.Pontos += 1;
                        classFora.Empates++;
                        classFora.Pontos += 1;
                    }
                }

                if (campeonato.RodadaAtual >= campeonato.TotalRodadas)
                {
                    campeonato.Status = "Finalizado";
                    TempData["Sucesso"] = "Campeonato finalizado!";
                }
                else
                {
                    campeonato.RodadaAtual++;
                    TempData["Sucesso"] = "Rodada finalizada! Agora estamos na Rodada " + campeonato.RodadaAtual + ".";
                }
            }
            else if (campeonato.TipoCampeonato == "Mata-Mata")
            {
                var vencedores = new List<int>();
                foreach (var partida in partidasRodada)
                {
                    int golsCasa = partida.GolsCasa ?? 0;
                    int golsFora = partida.GolsFora ?? 0;

                    if (golsCasa > golsFora)
                        vencedores.Add(partida.IdJogadorCasa);
                    else if (golsFora > golsCasa)
                        vencedores.Add(partida.IdJogadorFora);
                    else
                        vencedores.Add(partida.IdJogadorCasa);
                }

                if (vencedores.Count == 1)
                {
                    campeonato.Status = "Finalizado";
                    TempData["Sucesso"] = "Campeonato finalizado! Temos um campeao!";
                }
                else
                {
                    campeonato.RodadaAtual++;

                    for (int i = 0; i < vencedores.Count; i += 2)
                    {
                        var partida = new Partida
                        {
                            Rodada = campeonato.RodadaAtual,
                            IdCampeonato = campeonato.IdCampeonato,
                            IdJogadorCasa = vencedores[i],
                            IdJogadorFora = vencedores[i + 1],
                            GolsCasa = null,
                            GolsFora = null,
                            ResultadoRegistrado = false,
                            DataPartida = DateTime.Now
                        };
                        db.Partidas.Add(partida);
                    }

                    TempData["Sucesso"] = "Rodada finalizada! Proxima fase gerada.";
                }
            }

            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Campeonato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Campeonato campeonato = db.Campeonatos.Find(id);
            if (campeonato == null)
                return HttpNotFound();

            CarregarDropdowns(campeonato.TipoCampeonato, campeonato.QuantidadeJogadores);
            return View(campeonato);
        }

        // POST: Campeonato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCampeonato,Nome,TipoCampeonato,QuantidadeJogadores")] Campeonato campeonato)
        {
            if (ModelState.IsValid)
            {
                var original = db.Campeonatos
                    .Include(c => c.Jogadores)
                    .FirstOrDefault(c => c.IdCampeonato == campeonato.IdCampeonato);

                if (original == null) return HttpNotFound();

                original.Nome = campeonato.Nome;
                original.TipoCampeonato = campeonato.TipoCampeonato;
                original.QuantidadeJogadores = campeonato.QuantidadeJogadores;

                int totalJogadores = original.Jogadores.Count;

                if (totalJogadores == campeonato.QuantidadeJogadores)
                {
                    original.Status = "Pronto para Iniciar";
                }
                else
                {
                    original.Status = "Aguardando Jogadores";
                }

                db.SaveChanges();

                if (totalJogadores != campeonato.QuantidadeJogadores)
                {
                    if (totalJogadores > campeonato.QuantidadeJogadores)
                    {
                        TempData["Erro"] = "O campeonato tem " + totalJogadores + " jogadores mas agora o limite e " + campeonato.QuantidadeJogadores + ". Remova " + (totalJogadores - campeonato.QuantidadeJogadores) + " jogador(es).";
                    }
                    else
                    {
                        TempData["Sucesso"] = "Campeonato atualizado! Adicione mais " + (campeonato.QuantidadeJogadores - totalJogadores) + " jogador(es).";
                    }
                    return RedirectToAction("AdicionarJogadores", new { id = original.IdCampeonato });
                }

                return RedirectToAction("Index");
            }

            CarregarDropdowns(campeonato.TipoCampeonato, campeonato.QuantidadeJogadores);
            return View(campeonato);
        }

        // GET: Campeonato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Campeonato campeonato = db.Campeonatos.Find(id);
            if (campeonato == null)
                return HttpNotFound();

            return View(campeonato);
        }

        // POST: Campeonato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var classificacoes = db.Classificacoes.Where(c => c.IdCampeonato == id).ToList();
            db.Classificacoes.RemoveRange(classificacoes);

            var partidas = db.Partidas.Where(p => p.IdCampeonato == id).ToList();
            db.Partidas.RemoveRange(partidas);

            var jogadores = db.Jogadores.Where(j => j.IdCampeonato == id).ToList();
            db.Jogadores.RemoveRange(jogadores);

            Campeonato campeonato = db.Campeonatos.Find(id);
            db.Campeonatos.Remove(campeonato);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // =============================================
        // HELPERS
        // =============================================
        private void CarregarDropdowns(string tipoSelecionado = null, int? qtdSelecionada = null)
        {
            ViewBag.TipoCampeonato = new SelectList(new List<string>
            {
                "Pontos Corridos",
                "Mata-Mata",
                "Fase de Grupos + Mata-Mata"
            }, tipoSelecionado);

            ViewBag.QuantidadeOpcoes = new SelectList(new List<int> { 4, 8, 16, 32 }, qtdSelecionada);
        }

        private void CarregarTimesDropdown()
        {
            var times = db.Times.OrderBy(t => t.Pais).ThenBy(t => t.Liga).ThenBy(t => t.Nome).ToList();

            var timesSelectList = new List<SelectListItem>();
            foreach (var grupo in times.GroupBy(t => t.Pais + " - " + t.Liga))
            {
                foreach (var time in grupo)
                {
                    timesSelectList.Add(new SelectListItem
                    {
                        Value = time.IdTime.ToString(),
                        Text = time.Nome,
                        Group = new SelectListGroup { Name = grupo.Key }
                    });
                }
            }
            ViewBag.TimesSelect = timesSelectList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}