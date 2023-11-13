using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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
        public IActionResult UploadFiles()
        {
            return View();
        }

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=quickpastestorage;AccountKey=vPE+2tZagM9Y4FIh5X0l/qXUAzPaz8sZboy8z2K1OgpMRLWbkYEhI8BhGzcFmVEyvt3BzWMCnB/r+AStrqEgkA==;EndpointSuffix=core.windows.net";
            var containerName = "quickpastefiles";
            var blobName = file.FileName;

            var container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(blobName);
            await blob.UploadAsync(file.OpenReadStream());

            return Content("File uploaded successfully");
        }
    }
}
