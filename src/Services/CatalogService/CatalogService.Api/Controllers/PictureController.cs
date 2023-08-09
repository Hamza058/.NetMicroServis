using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CatalogService.Api.Controllers
{
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly CatalogContext _catalogContext;

        public PictureController(IWebHostEnvironment env, CatalogContext catalogContext)
        {
            _env = env;
            _catalogContext = catalogContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok("App and Running");
        }

        [HttpGet]
        [Route("api/v1/catalog/items/{catalogItemId:int}/pic")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetImageAsync(int catalogItemId)
        {
            if (catalogItemId == 0)
            {
                return BadRequest();
            }

            var item=await _catalogContext.CatalogItems
                .SingleOrDefaultAsync(ci=>ci.Id == catalogItemId);

            if (item == null)
            {
                var webRoot = _env.WebRootPath;
                var path = Path.Combine(webRoot, item.PictureFileName);

                string imageFileExtension = Path.GetExtension(item.PictureFileName);
                string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

                var buffer =await System.IO.File.ReadAllBytesAsync(path);

                return File(buffer, mimetype);
            }

            return NotFound();
        }

        private string GetImageMimeTypeFromImageFileExtension(string imageFileExtension)
        {
            throw new NotImplementedException();
        }
    }
}
