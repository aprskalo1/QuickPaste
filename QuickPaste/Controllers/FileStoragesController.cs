using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using QuickPaste.Models;
using QuickPaste.Utils;
using QuickPaste.ViewModels;

namespace QuickPaste.Controllers
{
    public class FileStoragesController : Controller
    {
        private readonly QuickPasteContext _context;

        public FileStoragesController(QuickPasteContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }   

        public IActionResult UploadFiles()
        {
            return View();
        }

        public async Task<IActionResult> SubmitFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("To copy please select files.");

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

        public IActionResult GetFiles()
        {
            return View();
        }

        public async Task<IActionResult> GetFilesByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Json(new { message = "To paste files please enter valid code." });


            var codeHash = HashUtils.GetHash(code);
            var files = await _context.FileStorages.Where(x => x.HashedCode == codeHash).ToListAsync();

            if (files.Count == 0)
                return Json(new { message = "Code doesn't match any files." });

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=quickpastestorage;AccountKey=vPE+2tZagM9Y4FIh5X0l/qXUAzPaz8sZboy8z2K1OgpMRLWbkYEhI8BhGzcFmVEyvt3BzWMCnB/r+AStrqEgkA==;EndpointSuffix=core.windows.net";
            var containerName = "quickpastefiles";

            var container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            var fileVMs = new List<FileStorageDTO>();
            foreach (var file in files)
            {
                var blob = container.GetBlobClient(file.Filename);
                var fileVM = new FileStorageDTO
                {
                    Filename = file.Filename,
                    BlobURI = blob.Uri.ToString()
                };

                fileVMs.Add(fileVM);
            }

            return Json(fileVMs);
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
