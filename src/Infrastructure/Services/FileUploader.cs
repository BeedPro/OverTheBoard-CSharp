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
    public class FileUploader : IFileUploader
    {
        private IHostingEnvironment _hostingEnvironment;
        public FileUploader(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<string> UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string filename = file.FileName.Trim('"');
            var path = EnsureFileName(filename);

            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = File.Create(path))
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
            return filename;
        }

        private string GetPathAndFileName(string path)
        {
            if (path.Contains("\\"))
                path = path.Substring(path.LastIndexOf("\\") + 1);

            return path;
        }

        private string EnsureFileName(string filename)
        {
            string path = _hostingEnvironment.ContentRootPath + "\\Uploads\\DisplayImages\\";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + filename;
        }
    }
}
