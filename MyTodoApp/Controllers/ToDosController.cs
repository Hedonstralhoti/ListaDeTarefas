using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyTodoApp.Models;

namespace MyTodoApp.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly Contexto _context;

        public ToDosController(Contexto context)
        {
            _context = context;
        }

        // GET: ToDos
        public async Task<IActionResult> Index()
        {
              return _context.ToDo != null ? 
                          View(await _context.ToDo.ToListAsync()) :
                          Problem("Entity set 'Contexto.ToDo'  is null.");
        }

        // GET: ToDos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDo == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // GET: ToDos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Done,CreatedAt,LastUpdateDate,User")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                toDo.User = User.Identity.Name;
                toDo.LastUpdateDate = DateTime.Now;
                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
        }

        // GET: ToDos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDo == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            return View(toDo);
        }

        // POST: ToDos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Done,CreatedAt,LastUpdateDate,User")] ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.Id))
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
            return View(toDo);
        }

        // GET: ToDos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDo == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // POST: ToDos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDo == null)
            {
                return Problem("Entity set 'Contexto.ToDo'  is null.");
            }
            var toDo = await _context.ToDo.FindAsync(id);
            if (toDo != null)
            {
                _context.ToDo.Remove(toDo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoExists(int id)
        {
          return (_context.ToDo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
