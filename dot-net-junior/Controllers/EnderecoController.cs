using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dot_net_junior.Models;

namespace dot_net_junior.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly BancoContext _context;

        public EnderecoController(BancoContext context)
        {
            _context = context;
        }

        // GET: Endereco
        public async Task<IActionResult> Index()
        {
              return _context.Endereco != null ? 
                          View(await _context.Endereco.ToListAsync()) :
                          Problem("Entity set 'BancoContext.Endereco'  is null.");
        }

        // GET: Endereco/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Endereco == null)
            {
                return NotFound();
            }

            var endereco = await _context.Endereco
                .FirstOrDefaultAsync(m => m.ID == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // GET: Endereco/Create
        public IActionResult Create()
        {
            int ultimoUsuarioId = _context.Cliente.OrderByDescending(u => u.ID).Select(u => u.ID).FirstOrDefault();
            ViewBag.UltimoUsuarioId = ultimoUsuarioId;

            var tipoDoc = _context.Cliente.FirstOrDefault(c => c.ID == ultimoUsuarioId);

            if (tipoDoc != null)
            {
                string TipoDocumento = tipoDoc.TipoDocumento;

                if (TipoDocumento == "CPF")
                {
                    ViewBag.EnderecoCPF = "Residencial";
                }
                else if (TipoDocumento == "CNPJ")
                {
                    ViewBag.EnderecoCPF = "Comercial";
                }
            }


            return View();
        }

        // POST: Endereco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Rua,Numero,CEP,Bairro,Cidade,TipoEndereco,IDcliente")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endereco);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","Contato");
            }
            return View(endereco);
        }

        // GET: Endereco/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Endereco == null)
            {
                return NotFound();
            }

            var endereco = await _context.Endereco.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            return View(endereco);
        }

        // POST: Endereco/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Rua,Numero,CEP,Bairro,Cidade,TipoEndereco,IDcliente")] Endereco endereco)
        {
            if (id != endereco.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string sqlRua = $"UPDATE Endereco SET Rua = '{endereco.Rua}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlRua);

                    string sqlNumero = $"UPDATE Endereco SET Numero = '{endereco.Numero}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlNumero);

                    string sqlCEP = $"UPDATE Endereco SET CEP = '{endereco.CEP}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlCEP);

                    string sqlBairro = $"UPDATE Endereco SET Bairro = '{endereco.Bairro}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlBairro);

                    string sqlCidade = $"UPDATE Endereco SET Cidade = '{endereco.Cidade}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlCidade);

                    string sqlTipoEndereco = $"UPDATE Endereco SET TipoEndereco = '{endereco.TipoEndereco}' WHERE ID= {endereco.ID}";
                    _context.Database.ExecuteSqlRaw(sqlTipoEndereco);

                    TempData["MensagemSucesso"] = $"Endereço editado com Sucesso";


                    //_context.Update(endereco);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecoExists(endereco.ID))
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
            return RedirectToAction("Index", "Cliente");
        }

        // GET: Endereco/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Endereco == null)
            {
                return NotFound();
            }

            var endereco = await _context.Endereco
                .FirstOrDefaultAsync(m => m.ID == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Endereco == null)
            {
                return Problem("Entity set 'BancoContext.Endereco'  is null.");
            }
            var endereco = await _context.Endereco.FindAsync(id);
            if (endereco != null)
            {
                _context.Endereco.Remove(endereco);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecoExists(int id)
        {
          return (_context.Endereco?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
