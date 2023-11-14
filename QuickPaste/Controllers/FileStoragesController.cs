using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickPaste.Models;
using QuickPaste.Utils;

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

        public async Task<IActionResult> SubmitFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=quickpastestorage;AccountKey=vPE+2tZagM9Y4FIh5X0l/qXUAzPaz8sZboy8z2K1OgpMRLWbkYEhI8BhGzcFmVEyvt3BzWMCnB/r+AStrqEgkA==;EndpointSuffix=core.windows.net";
            var containerName = "quickpastefiles";

            var container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            var codes = _context.FileStorages.Select(x => x.HashedCode).ToList();
            var currentCode = GenerateUniqueCode();

            foreach (var file in files)
            {
                if (container.GetBlobClient(file.FileName).Exists())
                {
                    await container.DeleteBlobIfExistsAsync(file.FileName);

                    var existingFile = _context.FileStorages.FirstOrDefault(x => x.Filename == file.FileName);
                    _context.FileStorages.Remove(existingFile!);
                }

                var blob = container.GetBlobClient(file.FileName);
                await blob.UploadAsync(file.OpenReadStream());

                var codeHash = HashUtils.GetHash(currentCode);

                var fileStorage = new FileStorage
                {
                    Filename = file.FileName,
                    HashedCode = codeHash
                };

                _context.FileStorages.Add(fileStorage);
            }

            await _context.SaveChangesAsync();

            return Content(currentCode);
        }

        public string GenerateUniqueCode()
        {
            var codes = _context.FileStorages.Select(x => x.HashedCode).ToList();
            var currentCode = CodeGenerationUtils.GenerateCode();
            var codeHash = HashUtils.GetHash(currentCode);

            while (codes.Contains(codeHash))
            {
                currentCode = CodeGenerationUtils.GenerateCode();
                codeHash = HashUtils.GetHash(currentCode);
            }

            return currentCode;
        }
    }
}
