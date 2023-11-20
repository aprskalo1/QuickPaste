using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using QuickPaste.Models;
using QuickPaste.Utils;

namespace QuickPaste.Controllers
{
    public class TextStoragesController : Controller
    {
        private readonly CosmosClient _cosmosClient;

        public TextStoragesController()
        {
            _cosmosClient = new CosmosClient(
                connectionString: "AccountEndpoint=https://quickpaste.documents.azure.com:443/;AccountKey=vS9I59RuGV7tV3w6jYu8PABgIT5NuIyS6OeAVSqXczrwWqv0GmTp9Scih5PV5GnAZ8SbllNYCkDxACDbFzuy9w==;"
            );
        }

        private Container container
        {
            get => _cosmosClient.GetDatabase("quickpastetext").GetContainer("textfiles");
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult UploadFiles()
        {
            return View();
        }

        public async Task<IActionResult> SubmitText(string textContent)
        {
            if (string.IsNullOrEmpty(textContent))
            {
                return Content("Please enter text that you want to copy.");
            }

            var code = CodeGenerationUtils.GenerateCode();

            var textStorage = new TextStorage
            {
                Id = Guid.NewGuid().ToString(),
                TimeUploaded = DateTime.Now,
                HashedCode = HashUtils.GetHash(code),
                TextContent = textContent
            };

            try
            {
                await container.CreateItemAsync(textStorage);
            }
            catch (Exception)
            {
                return Content("Upload to the database failed. Please try again later.");
            }

            return Content(code);
        }
    }
}
