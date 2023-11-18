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
    public class TextStoragesController : Controller
    {
        private readonly QuickPasteContext _context;

        public TextStoragesController(QuickPasteContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetFiles()
        {
            return View();
        }
    }
}
