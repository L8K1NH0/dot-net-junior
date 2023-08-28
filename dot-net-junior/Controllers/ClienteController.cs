using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dot_net_junior.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using dot_net_junior.LogicaNegocio;

namespace dot_net_junior.Controllers
{
    public class ClienteController : Controller
    {
        private readonly BancoContext _context;
        public ClienteController(BancoContext context)
        {
            _context = context;
        }


        // GET: Cliente
        public async Task<IActionResult> Index()
        {
              return _context.Cliente != null ? 
                          View(await _context.Cliente.ToListAsync()) :
                          Problem("Entity set 'BancoContext.Cliente'  is null.");
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,CPF_CNPJ,TipoDocumento")] Cliente cliente)
        {
            string CPF_CNPJ = cliente.CPF_CNPJ;
            var DocExistente = _context.Cliente.FirstOrDefault(c => c.CPF_CNPJ == CPF_CNPJ);

            bool res;

            if (cliente.TipoDocumento == "CPF")
            {                
                res = ValidaCPF.IsCpf(CPF_CNPJ);
            }
            else
            {
                res = ValidaCNPJ.IsCnpj(CPF_CNPJ);
            }



            try
            {
                if (DocExistente != null)
                {
                    TempData["MensagemErro"] = $"Ops, Não foi possivel realizar o cadastro do cliente. Ja existe um conta com este documento.";               
                }
                else if (res == false) 
                {
                    TempData["MensagemErro"] = $"Ops, Documento invalido";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(cliente);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", "Endereco");

                    }
                }
                return View(cliente);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, Não foi possivel realizar o cadastro do cliente. Detalhe: {erro.Message}";
                return View(cliente);
            }





        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,CPF_CNPJ,TipoDocumento")] Cliente cliente)
        {            
            if (id != cliente.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["MensagemSucesso"] = $"Editado com Sucesso";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'BancoContext.Cliente'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);

                string sqlContato = $"DELETE FROM Contato WHERE IDCliente = {id}";                
                _context.Database.ExecuteSqlRaw(sqlContato);

                string sqlEndereco = $"DELETE FROM Endereco WHERE IDCliente = {id}";
                _context.Database.ExecuteSqlRaw(sqlEndereco);
            }
            
            await _context.SaveChangesAsync();
            TempData["MensagemSucesso"] = $"Conta apagada com Sucesso";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Cliente?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
