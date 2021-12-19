using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverTheBoard.Infrastructure.Services
{
    class UnitOfWork : IUnitOfWork
    {
        private IHostingEnvironment _hostingEnvironment;
        public UnitOfWork(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async void UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string filename = file.FileName.Trim('"');
            filename = EnsureFileName(filename);
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = File.Create(GetPathAndFileName(filename)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string GetPathAndFileName(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            return filename;
        }

        private string EnsureFileName(string filename)
        {
            string path = _hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }
    }
}
