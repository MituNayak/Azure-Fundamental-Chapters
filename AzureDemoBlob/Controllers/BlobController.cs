using AzureDemoBlob.Services;
using Microsoft.AspNetCore.Mvc;
using AzureDemoBlob.Models;

namespace AzureDemoBlob.Controllers
{
    public class BlobController : Controller
    {
        private IBlobServices _blobServices;
        public BlobController(IBlobServices blobServices)
        {
            _blobServices = blobServices;
            
        }
        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobsObj = await _blobServices.GetAllBlobs(containerName);
            return View(blobsObj);
           
        }
        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName, Blob blob, IFormFile file)
        {
            if (file == null || file.Length < 1) return View();

            //file name - xps_img2.png 
            //new name - xps_img2_GUIDHERE.png
            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            var result = await _blobServices.UploadBlob(fileName, file, containerName, blob);

            if (result)
                return RedirectToAction("Index", "Container");

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
            return Redirect(await _blobServices.GetBlob(name, containerName));
        }

        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobServices.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "Home");
        }


    }
}
