namespace anotheraspnetapp.Services
{
    using Microsoft.AspNetCore.Hosting;
    using System.IO;

    public class PictureService : IPictureService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PictureService (IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetRandomPicture()
        {
            var directory = new DirectoryInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "Pictures"));
            var files = directory.GetFiles();

            var i = new System.Random().Next(files.Length);

            return Path.Combine("/Pictures", files[i].Name);
        }
    }
}