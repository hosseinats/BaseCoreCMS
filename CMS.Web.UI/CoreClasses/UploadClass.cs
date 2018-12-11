using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CMS.Web.UI.CoreClasses
{
    public class UploadClass
    {
        public string fileSaveAddress { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
        public async Task Upload(string webRoot, string uploadFolder, IFormFile file)
        {

            var uploadDirectory = Path.Combine(webRoot, "uploads/" + uploadFolder);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            //if (file.Length > 0)

            fileName = Guid.NewGuid() + "--" + file.FileName;
            filePath = Path.Combine(uploadDirectory, fileName);
            fileSaveAddress = "/uploads/" + uploadFolder + "/" + fileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream).ConfigureAwait(true);
            }


        }


    }
}
