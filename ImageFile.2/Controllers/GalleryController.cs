using Microsoft.AspNetCore.Mvc;

namespace ImageFile._2.Controllers
{
    public class GalleryController : Controller
    {
        private readonly string wwwrootDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
        public IActionResult Index()
        {
            List<string> images = Directory.GetFiles(wwwrootDir,"*").Select(Path.GetFileName).ToList();
            return View(images);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile myfile)
        {
            if (myfile!=null)
            {
                var path= Path.Combine(wwwrootDir, DateTime.Now.Ticks.ToString()+Path.GetExtension (myfile.FileName));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await myfile.CopyToAsync(stream);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        // Images Downlode 
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", filePath);
                var memory=new MemoryStream();
                using (var stream = new FileStream(path , FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "APPLICATION/octet-stream";
                var fileName=Path .GetFileName(filePath);
                return File(memory, contentType, fileName); 
            }
            return View();
        }
        public IActionResult DeleteFile(string filePath)
        { 
            if (!string.IsNullOrEmpty(filePath))
            {
                var path = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot\\uploads", filePath);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
