using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;

namespace DifflibSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IFileProvider _fileProvider;

        public HomeController(IHostingEnvironment hostingEnv, IFileProvider fileProvider)
        {
            _hostingEnv = hostingEnv;
            _fileProvider = fileProvider;
        }

        public IActionResult Index(string diff)
        {
            var beforeFile = _fileProvider.GetFileInfo($"App_Data/diffs/{diff}-before.txt");
            var afterFile = _fileProvider.GetFileInfo($"App_Data/diffs/{diff}-after.txt");

            if(!beforeFile.Exists || !afterFile.Exists)
            {
                return HttpNotFound();
            }

            var viewModel = new DiffViewModel
            {
                Before = Read(beforeFile),
                After = Read(afterFile)
            };

            return View(viewModel);
        }

        private static string[] Read(IFileInfo file)
        {
            using (var stream = file.CreateReadStream())
            using (var streamReader = new StreamReader(stream))
            {
                var lines = new List<string>();
                while (true)
                {
                    var line = streamReader.ReadLine();
                    if(line == null)
                    {
                        break;
                    }

                    lines.Add(line);
                }

                return lines.ToArray();
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    public class DiffViewModel
    {
        public string[] Before { get; set; }
        public string[] After { get; set; }
    }
}
