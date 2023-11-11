using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickPaste.Models;

namespace QuickPaste.Controllers
{
    public class FileStoragesController : Controller
    {
        private readonly QuickPasteContext _context;

        public FileStoragesController(QuickPasteContext context)
        {
            _context = context;
        }

        // GET: FileStorages
        public async Task<IActionResult> Index()
        {
            var quickPasteContext = _context.FileStorages.Include(f => f.PastedCode);
            return View(await quickPasteContext.ToListAsync());
        }

        // GET: FileStorages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileStorages == null)
            {
                return NotFound();
            }

            var fileStorage = await _context.FileStorages
                .Include(f => f.PastedCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileStorage == null)
            {
                return NotFound();
            }

            return View(fileStorage);
        }

        // GET: FileStorages/Create
        public IActionResult Create()
        {
            ViewData["PastedCodeId"] = new SelectList(_context.PastedCodes, "Id", "Id");
            return View();
        }

        // POST: FileStorages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TimeUploaded,Filename,PastedCodeId")] FileStorage fileStorage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fileStorage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PastedCodeId"] = new SelectList(_context.PastedCodes, "Id", "Id", fileStorage.PastedCodeId);
            return View(fileStorage);
        }

        // GET: FileStorages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FileStorages == null)
            {
                return NotFound();
            }

            var fileStorage = await _context.FileStorages.FindAsync(id);
            if (fileStorage == null)
            {
                return NotFound();
            }
            ViewData["PastedCodeId"] = new SelectList(_context.PastedCodes, "Id", "Id", fileStorage.PastedCodeId);
            return View(fileStorage);
        }

        // POST: FileStorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TimeUploaded,Filename,PastedCodeId")] FileStorage fileStorage)
        {
            if (id != fileStorage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileStorage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileStorageExists(fileStorage.Id))
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
            ViewData["PastedCodeId"] = new SelectList(_context.PastedCodes, "Id", "Id", fileStorage.PastedCodeId);
            return View(fileStorage);
        }

        // GET: FileStorages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FileStorages == null)
            {
                return NotFound();
            }

            var fileStorage = await _context.FileStorages
                .Include(f => f.PastedCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileStorage == null)
            {
                return NotFound();
            }

            return View(fileStorage);
        }

        // POST: FileStorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FileStorages == null)
            {
                return Problem("Entity set 'QuickPasteContext.FileStorages'  is null.");
            }
            var fileStorage = await _context.FileStorages.FindAsync(id);
            if (fileStorage != null)
            {
                _context.FileStorages.Remove(fileStorage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileStorageExists(int id)
        {
          return (_context.FileStorages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
