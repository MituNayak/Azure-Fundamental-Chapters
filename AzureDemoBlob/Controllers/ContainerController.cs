using AzureDemoBlob.Services;
using Microsoft.AspNetCore.Mvc;
using AzureDemoBlob.Models;

namespace AzureDemoBlob.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerServices _containerServices;
        public ContainerController(IContainerServices containerService)
        {
            _containerServices = containerService;
        }
        public async Task<IActionResult> Index()
        {
            var allaContainer = await _containerServices.GetAllContainer();
            return View(allaContainer);

            // return View();
        }

        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerServices.DeleteContainer(containerName);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View(new Container());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerServices.CreateContainer(container.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
