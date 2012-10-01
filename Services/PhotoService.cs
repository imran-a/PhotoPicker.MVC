using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PhotoPicker.MVC.Services
{
    public class PhotoService
    {
        public string[] GetPhotos(string relativeFilePath, int pageSize, int page)
        {

            var serverPath = HttpContext.Current.Server.MapPath(relativeFilePath);

            if (Directory.Exists(serverPath))
                return new DirectoryInfo(serverPath).GetFiles().Select(f => f.Name).ToArray();

            throw new Exception("Directory does not exist");

        }
    }
}