using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dot_net_junior.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dot_net_junior.Controllers
{
    public class ContatoController : Controller
    {
        private readonly BancoContext _context;

        public ContatoController(BancoContext context)
        {
            _context = context;
        }

        // GET: Contato
        public async Task<IActionResult> Index(int ID)
        {
            //return _context.Contato != null ? 
            //              View(await _context.Contato.ToListAsync()) :
            //              Problem("Entity set 'BancoContext.Contato'  is null.");

            var contatosDoCliente = await _context.Contato.Where(c => c.IDcliente == ID).ToListAsync();

            if (contatosDoCliente != null)
            {
                return View(contatosDoCliente);
            }
            else
            {
                return Problem($"Os contatos do cliente com ID {ID} está vazio.");
            }

        }

        // GET: Contato/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contato == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // GET: Contato/Create
        public IActionResult Create()
        {
            int ultimoUsuarioId = _context.Cliente.OrderByDescending(u => u.ID).Select(u => u.ID).FirstOrDefault();

            ViewBag.UltimoUsuarioId = ultimoUsuarioId;

            return View();

        }

        // POST: Contato/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DDD,NumeroContato,Tipo,IDcliente")] ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(contato);
                    await _context.SaveChangesAsync();
                    TempData["MensagemSucesso"] = $"Cliente Cadastrado com Sucesso";
                    return RedirectToAction("Index","Cliente");

                }
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, Não foi possivel realizar o cadastro do cliente. Detalhe: {erro.Message}";                
            }
            return RedirectToAction("Index", "Cliente");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContato([Bind("ID,DDD,NumeroContato,Tipo,IDcliente")] ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Contato");
            }
            return View(contato);
        }

        // GET: Contato/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contato == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        // POST: Contato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DDD,NumeroContato,Tipo,IDcliente")] ContatoModel contato)
        {
            if (id != contato.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string sqlDDD = $"UPDATE Contato SET DDD = '{contato.DDD}' WHERE ID= {contato.ID}";
                    _context.Database.ExecuteSqlRaw(sqlDDD);

                    string sqlNumeroContato = $"UPDATE Contato SET NumeroContato = '{contato.NumeroContato}' WHERE ID= {contato.ID}";
                    _context.Database.ExecuteSqlRaw(sqlNumeroContato);

                    string sqlTipo = $"UPDATE Contato SET Tipo = '{contato.Tipo}' WHERE ID= {contato.ID}";
                    _context.Database.ExecuteSqlRaw(sqlTipo);

                    TempData["MensagemSucesso"] = $"Contato editado com Sucesso";


                    //_context.Update(contato);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(contato.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Cliente");
            }
            return RedirectToAction("Index","Cliente");
        }

        // GET: Contato/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contato == null)
            {
                return NotFound();
            }

            var contato = await _context.Contato
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // POST: Contato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contato == null)
            {
                return Problem("Entity set 'BancoContext.Contato'  is null.");
            }
            var contato = await _context.Contato.FindAsync(id);
            if (contato != null)
            {
                _context.Contato.Remove(contato);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContatoExists(int id)
        {
          return (_context.Contato?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
